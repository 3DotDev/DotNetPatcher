Imports System.Resources
Imports Mono.Cecil
Imports Helper.RandomizeHelper
Imports System.IO
Imports Helper.CecilHelper
Imports System.Text.RegularExpressions
Imports Helper.UtilsHelper
Imports Implementer.Engine.Processing

Namespace Core.Obfuscation.Protections

    Public Class ResourcesContentRenaming
        Inherits Protection

#Region " Fields "
        Private ReadOnly m_Resources As List(Of EmbeddedResource)
        Private ReadOnly m_embeddedResource As Dictionary(Of EmbeddedResource, EmbeddedResource)
#End Region

#Region " Properties "
        Public Overrides Property Context As ProtectionContext

        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return "Obfuscating (Resources content renaming ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property ProgressIncrement As Integer
            Get
                Return 30
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "RenameResourcesContent"
            End Get
        End Property

        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext)
            MyBase.New()
            Context = Contex
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.RenameResourcesContent AndAlso Contex.Params.TaskAccept.Packer.Enabled = False Then
                If Context.InputAssembly.MainModule.HasResources Then
                    _Enabled = True
                    m_Resources = New List(Of EmbeddedResource)
                    m_embeddedResource = New Dictionary(Of EmbeddedResource, EmbeddedResource)
                End If
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            For Each EmbRes As Resource In Context.InputAssembly.MainModule.Resources
                m_Resources.Add(EmbRes)
            Next

            For Each modul In (From m In Context.InputAssembly.Modules
                               Where m.HasTypes
                               Select m)
                For Each type As TypeDefinition In modul.Types
                    RenameContent(type)
                Next
            Next
        End Sub

        Private Sub RenameContent(typeDef As TypeDefinition)
            For Each prop In (From p In typeDef.Properties
                              Where Not p.GetMethod Is Nothing AndAlso p.GetMethod.Name = "get_ResourceManager" AndAlso p.GetMethod.HasBody
                              Select p)
                If prop.GetMethod.Body.Instructions.Count <> 0 Then
                    For Each instruction In prop.GetMethod.Body.Instructions
                        If TypeOf instruction.Operand Is String Then
                            Dim NewResManagerName$ = instruction.Operand
                            For Each EmbRes As Resource In m_Resources
                                UpdateResources(typeDef, EmbRes, NewResManagerName)
                            Next
                        End If
                    Next
                End If
            Next
        End Sub

        Private Sub UpdateResources(TypeDef As TypeDefinition, OriginalEmbeddedRes As EmbeddedResource, KeyNameOriginal As String)
            Try
                Dim ToHex = Functions.StreamToHex(OriginalEmbeddedRes.GetResourceStream)
                If Not ToHex.StartsWith(Functions.StrToHex("MZ")) Then
                    If ToHex.Contains(Functions.StrToHex("System.Resources.ResourceReader, mscorlib, ")) Then

                        If Not OriginalEmbeddedRes.GetResourceStream Is Nothing Then
                            Dim NewEmbeddedRes As New ResourceWriter(KeyNameOriginal)

                            Using read As New ResourceReader(OriginalEmbeddedRes.GetResourceStream)
                                For Each Dat As DictionaryEntry In read
                                    Dim data() As Byte = Nothing
                                    Dim dataType = String.Empty
                                    Dim originalDataKey$ = Dat.Key
                                    read.GetResourceData(Dat.Key, dataType, data)
                                    Dim obfuscatedDataKey$ = UpdateKey(NewEmbeddedRes, dataType, data)
                                    UpdateResourcesKeys(TypeDef, obfuscatedDataKey, originalDataKey, OriginalEmbeddedRes.Name)
                                Next
                            End Using

                            NewEmbeddedRes.Generate()
                            NewEmbeddedRes.Close()
                            NewEmbeddedRes.Dispose()

                            UpdateAssembly(TypeDef, KeyNameOriginal, OriginalEmbeddedRes)
                        End If
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub

        Private Sub UpdateResourcesKeys(TypeDef As TypeDefinition, NewKeyName$, OriginalKeyName$, resName$)
            If resName.EndsWith("Resources.resources") = False Then
                If resName.EndsWith(".resources") Then
                    Dim typeFullName$ = resName.Substring(0, resName.LastIndexOf("."))
                    Dim typeName$ = typeFullName.Replace(".resources", String.Empty).Substring(typeFullName.LastIndexOf(".") + 1)

                    Dim typeSearch As TypeDefinition = Finder.FindType(TypeDef.Module.Assembly.MainModule, typeName)
                    If Not typeSearch Is Nothing Then
                        Dim methodSearch As MethodDefinition = Finder.FindMethod(typeSearch, "InitializeComponent")
                        If Not methodSearch Is Nothing Then
                            UpdateMethodBody(methodSearch, NewKeyName, OriginalKeyName)
                        End If
                    End If
                End If
            ElseIf resName.EndsWith("Resources.resources") Then
                For Each pr In TypeDef.Properties
                    If Not pr.GetMethod Is Nothing Then
                        If pr.GetMethod.Name = "get_" & Regex.Replace(OriginalKeyName, "[^\w]+", "_") Then
                            NewKeyName = Mapping.RenameMethodMember(pr.GetMethod, NewKeyName)
                            UpdateMethodBody(pr.GetMethod, NewKeyName, OriginalKeyName)
                        End If
                    End If
                Next
            End If
        End Sub

        Private Sub UpdateMethodBody(Meth As MethodDefinition, NewKeyName$, OriginalKeyName$)
            If Meth.HasBody AndAlso Meth.Body.Instructions.Count <> 0 Then
                For Each instruction As Cil.Instruction In Meth.Body.Instructions
                    If TypeOf instruction.Operand Is String Then
                        If CStr(instruction.Operand) = OriginalKeyName Then
                            instruction.Operand = NewKeyName
                        End If
                    End If
                Next
            End If
        End Sub

        Private Function UpdateKey(NewEmbeddedRes As ResourceWriter, datatype As Object, data As Byte()) As String
            Dim newdataKey = Context.Randomizer.GenerateNew()
            NewEmbeddedRes.AddResourceData(newdataKey, datatype, data)
            Return newdataKey
        End Function

        Private Sub UpdateAssembly(TypeDef As TypeDefinition, resWriterPath$, OriginalEmbeddedResource As Resource)
            Try
                Dim CompressRes = New EmbeddedResource(OriginalEmbeddedResource.Name, ManifestResourceAttributes.Private, File.ReadAllBytes(My.Application.Info.DirectoryPath & "\" & resWriterPath))
                If Not m_embeddedResource.ContainsKey(OriginalEmbeddedResource) Then
                    m_embeddedResource.Add(OriginalEmbeddedResource, CompressRes)
                    TypeDef.Module.Assembly.MainModule.Resources.Remove(OriginalEmbeddedResource)
                    TypeDef.Module.Assembly.MainModule.Resources.Add(CompressRes)
                End If
                File.Delete(My.Application.Info.DirectoryPath & "\" & resWriterPath)
            Catch ex As Exception
                'MsgBox("UpdateAssembly Exception : " & ex.ToString)
            End Try
        End Sub

        Private Sub CleanUpTmpFiles()
            Try
                For Each f In Directory.GetFiles(My.Application.Info.DirectoryPath, "*.resources", SearchOption.TopDirectoryOnly)
                    File.Delete(f)
                Next
            Catch ex As Exception
            End Try
        End Sub

        Friend Sub Cleanup()
            If m_Resources.Count <> 0 Then m_Resources.Clear()
            If m_embeddedResource.Count <> 0 Then m_embeddedResource.Clear()
            CleanUpTmpFiles()
        End Sub

#End Region

    End Class
End Namespace