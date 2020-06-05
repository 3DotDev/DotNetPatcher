Imports System.IO
Imports Helper.UtilsHelper
Imports Implementer.Core.IconChanger
Imports Implementer.Core.ManifestRequest
Imports Implementer.Core.Obfuscation.Builder
Imports Implementer.Core.Versions
Imports Mono.Cecil

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class PackerStubCreation
        Inherits Protection

#Region " Fields "
        Private ReadOnly m_FilePathToPack As String
        Private ReadOnly CompletedMethods As Mono.Collections.Generic.Collection(Of MethodDefinition)
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return "Packing (Creating stub ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String = "Packer Stub"
        Public Overrides Property Context As ProtectionContext
        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean = False
        Public Overrides ReadOnly Property ProgressIncrement As Integer = 13
        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext)
            MyBase.New()
            Context = Contex
            If Contex.Params.TaskAccept.Packer.Enabled Then
                _Enabled = True
                m_FilePathToPack = Contex.Params.InputFile
                CompletedMethods = New Mono.Collections.Generic.Collection(Of MethodDefinition)
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()

            Dim tmpFile = Functions.GetTempFolder & "\" & New FileInfo(m_FilePathToPack).Name.Replace(".exe", Context.Randomizer.GenerateNewAlphabetic & ".exe")
            Try
                Dim targetRuntime As TargetRuntime = TargetRuntime.Net_4_0
                Select Case Environment.Version.Major
                    Case 2
                        targetRuntime = TargetRuntime.Net_2_0
                    Case 3
                        targetRuntime = TargetRuntime.Net_2_0
                    Case 4
                        targetRuntime = TargetRuntime.Net_4_0
                End Select

                Dim mainmodule = AssemblyDefinition.ReadAssembly(m_FilePathToPack).MainModule
                mainmodule.Runtime = targetRuntime
                Dim parameters As New ModuleParameters With {.Architecture = mainmodule.Architecture,
                                                             .Kind = mainmodule.Kind,
                                                             .Runtime = mainmodule.Runtime}

                Dim asm = AssemblyDefinition.CreateAssembly(mainmodule.Assembly.Name, mainmodule.Name, parameters)
                Dim asmModule = asm.MainModule

                For Each m In mainmodule.AssemblyReferences
                    asmModule.AssemblyReferences.Add(m)
                Next

                For Each m In mainmodule.ModuleReferences
                    asmModule.ModuleReferences.Add(m)
                Next

                asmModule.Attributes = (asmModule.Attributes Or (mainmodule.Attributes And ModuleAttributes.Required32Bit))

                File.Copy(m_FilePathToPack, tmpFile, True)

                Dim PackerResolver As New PackerResolver(New PackerContext(asm, True, Context.Randomizer, tmpFile))
                CompletedMethods.Add(PackerResolver.Resolver)

                asm.MainModule.Assembly.EntryPoint = PackerResolver.GetMainMethod
                asm.Write(m_FilePathToPack)

                PackerResolver.RemoveStubFile()

            Catch ex As Exception
                MsgBox("Error : Packer CreateExecutable : " & vbNewLine & ex.ToString)
            Finally
                Try
                    File.Delete(tmpFile)
                    File.Delete(Functions.GetTempFolder & "\SevenzipLib.dll")
                Catch ex As Exception
                End Try
            End Try

            Replacer.ReplaceFromIcon(m_FilePathToPack, Context.Params.TaskAccept.Packer.NewIcon)
            Injector.InjectAssemblyVersionInfos(m_FilePathToPack, Context.Params.TaskAccept.VersionInfos)
            ManifestWriter.ApplyManifest(m_FilePathToPack, Context.Params.TaskAccept.Packer.RequestedLevel)
        End Sub
#End Region

    End Class
End Namespace
