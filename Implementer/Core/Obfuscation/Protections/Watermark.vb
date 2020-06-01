Imports Mono.Cecil
Imports System.IO
Imports Mono.Cecil.Cil
Imports System.Reflection

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class Watermark
        Inherits Protection

#Region " Fields "
        Private ReadOnly Attribs As String()
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Watermark injecting ...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Watermark"
            End Get
        End Property

        Public Overrides Property Context As ProtectionContext

        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property ProgressIncrement As Integer
            Get
                Return 97
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If HasObfuscationTask() Then
                _Enabled = True
                Attribs = New String() {"DotNetPatcherObfuscatorAttribute", "DotNetPatcherPackerAttribute", "DotfuscatorAttribute", "ConfusedByAttribute",
                                          "ObfuscatedByGoliath", "dotNetProtector", "PoweredByAttribute", "AssemblyInfoAttribute"}
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()

            Context.InputAssembly.MainModule.Resources.Add(New EmbeddedResource(If(PackerState, My.Resources.DnpPattribute, My.Resources.DnpOattribute), ManifestResourceAttributes.Private, File.ReadAllBytes(Path.GetTempFileName)))

            For Each it In Attribs
                Dim item As New TypeDefinition("", it, Mono.Cecil.TypeAttributes.AnsiClass, Context.InputAssembly.MainModule.Import(GetType(Attribute)))
                If it = "DotNetPatcherObfuscatorAttribute" AndAlso PackerState = False Then
                    CreatAttribut(Context.InputAssembly, item, it)
                ElseIf it = "DotNetPatcherPackerAttribute" AndAlso PackerState = True Then
                    CreatAttribut(Context.InputAssembly, item, it)
                ElseIf it = "AssemblyInfoAttribute" Then
                    CreatAttribut(Context.InputAssembly, item, it)
                Else
                    If it = "DotNetPatcherObfuscatorAttribute" Then
                    ElseIf it = "DotNetPatcherPackerAttribute" Then
                    ElseIf it = "AssemblyInfoAttribute" Then
                    Else
                        CreatAttribut(Context.InputAssembly, item, it)
                    End If
                End If
            Next
        End Sub

        Private Sub CreatAttribut(assdef As AssemblyDefinition, item As TypeDefinition, it As String)
            Dim method As New MethodDefinition(".ctor", (Mono.Cecil.MethodAttributes.CompilerControlled Or (Mono.Cecil.MethodAttributes.FamANDAssem Or (Mono.Cecil.MethodAttributes.Family Or (Mono.Cecil.MethodAttributes.RTSpecialName Or Mono.Cecil.MethodAttributes.SpecialName)))), assdef.MainModule.TypeSystem.Void)
            method.Parameters.Add(New ParameterDefinition(assdef.MainModule.TypeSystem.String))

            Dim iLProc As ILProcessor = method.Body.GetILProcessor
            With iLProc
                .Emit(OpCodes.Ldarg_0)
                .Emit(OpCodes.Call, assdef.MainModule.Import(GetType(Attribute).GetConstructor((BindingFlags.NonPublic Or BindingFlags.Instance), Nothing, Type.EmptyTypes, Nothing)))
                .Emit(OpCodes.Ret)
            End With

            item.Methods.Add(method)
            assdef.MainModule.Types.Add(item)
            Dim att As New CustomAttribute(method)
            Select Case it
                Case "DotNetPatcherObfuscatorAttribute", "DotNetPatcherObfuscatorAttribute"
                    att.ConstructorArguments.Add(New CustomAttributeArgument(assdef.MainModule.TypeSystem.String, If(it = "AssemblyInfoAttribute", "", String.Format(("DotNetPatcher v" & GetType(Watermark).Assembly.GetName.Version.ToString), New Object(0 - 1) {}))))
                Case Else
                    att.ConstructorArguments.Add(New CustomAttributeArgument(assdef.MainModule.TypeSystem.String, String.Empty))
            End Select
            assdef.MainModule.CustomAttributes.Add(att)
            assdef.CustomAttributes.Add(att)
        End Sub

        Private Function HasObfuscationTask() As Boolean
            With Context.Params.TaskAccept.Obfuscation
                If .Enabled AndAlso
              (.EncryptNumeric OrElse
              .EncryptBoolean OrElse
              .EncryptString OrElse
              .AntiTamper OrElse
              .AntiDebug OrElse
              .AntiDumper OrElse
              .AntiIlDasm OrElse
              .HidePublicCalls OrElse
              .CompressResources OrElse
              .RenameAssembly OrElse
              .ControlFlow) Then
                    Return True
                End If
            End With
            Return False
        End Function

        Private Function HasPackerTask() As Boolean
            Return Context.Params.TaskAccept.Packer.Enabled
        End Function
#End Region

    End Class
End Namespace
