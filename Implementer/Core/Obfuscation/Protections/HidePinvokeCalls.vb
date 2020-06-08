Imports Mono.Cecil
Imports Mono.Cecil.Cil
Imports Mono.Cecil.Rocks
Imports Helper.CecilHelper
Imports Implementer.Core.Obfuscation.Builder
Imports System.IO
Imports Helper.UtilsHelper
Imports Implementer.Engine.Processing

Namespace Core.Obfuscation.Protections
    Public NotInheritable Class HidePinvokeCalls
        Inherits Protection

#Region " Fields "
        Private LoaderInvoke As DynamicPinvoke
        Private ReadOnly PinvokeCreate As PinvokeModifier
        Private ReadOnly ExportedDll As List(Of NativeDllFunction)
        Private ReadOnly CompletedMethods As Mono.Collections.Generic.Collection(Of MethodDefinition)
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Hide calls part 1...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Hide Pinvoke calls"
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
                Return 65
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.HidePublicCalls AndAlso Contex.Params.TaskAccept.Packer.Enabled = False Then
                _Enabled = True
                PackerState = PackerStat
                PinvokeCreate = New PinvokeModifier(Contex.Randomizer)
                ExportedDll = New List(Of NativeDllFunction)
                CompletedMethods = New Mono.Collections.Generic.Collection(Of MethodDefinition)
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()

            Dim Types As New List(Of TypeDefinition)
            Dim HasPinvokeCalls As Boolean
            For Each mo As ModuleDefinition In Context.InputAssembly.Modules
                For Each t In mo.GetAllTypes()
                    If t.Methods.Any(Function(f) (f.IsPInvokeImpl AndAlso Not f.Name.ToLower = "virtualprotect" AndAlso f.HasParameters = False) OrElse (f.IsPInvokeImpl AndAlso Not f.Name.ToLower = "virtualprotect" AndAlso f.HasParameters AndAlso f.Parameters.All(Function(m) m.HasFieldMarshal = False))) Then
                        HasPinvokeCalls = True
                    End If
                    Types.Add(t)
                Next
            Next

            If HasPinvokeCalls Then
                LoaderInvoke = New DynamicPinvoke(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
                CompletedMethods.Add(LoaderInvoke.LoadLibrary)
                CompletedMethods.Add(LoaderInvoke.GetProcAddress)
                CompletedMethods.Add(LoaderInvoke.DynamicInvoke)

                PinvokeCreate.AddModuleRef(Context.InputAssembly.MainModule)

                If Not File.Exists(Functions.GetTempFolder & "\dllexp.exe") Then
                    File.WriteAllBytes(Functions.GetTempFolder & "\dllexp.exe", My.Resources.dllexp)
                End If

                For Each type As TypeDefinition In Types
                    IterateType(type)
                Next

                LoaderInvoke.RemoveStubFile()
                PinvokeCreate.Dispose()
            End If

            Types.Clear()
            ExportedDll.Clear()
        End Sub

        Private Sub IterateType(td As TypeDefinition)
            Dim publicMethods As New List(Of MethodDefinition)()
            publicMethods.AddRange(From m In td.Methods Where (m.HasBody AndAlso m.Body.Instructions.Count > 2 AndAlso
                                                            Not CompletedMethods.Contains(m) AndAlso
                                                            Not Utils.HasUnsafeInstructions(m) AndAlso
                                                            Not m.Body.Instructions(0).OpCode = OpCodes.Ldtoken))

            Try
                For Each md In publicMethods
                    md.Body.SimplifyMacros
                    ProcessInstructions(md.Body)
                    md.Body.OptimizeMacros
                    md.Body.ComputeOffsets()
                    md.Body.ComputeHeader()
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            publicMethods.Clear()
        End Sub

        Private Sub ProcessInstructions(body As MethodBody)
            Dim instructions = body.Instructions
            Dim instructionsToExpand As List(Of Instruction) = New List(Of Instruction)()

            For Each instruction As Instruction In instructions
                Select Case instruction.OpCode
                    Case OpCodes.Call
                        If Utils.IsValidPinvokeCallOperand(instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                End Select
            Next

            For Each instruction As Instruction In instructionsToExpand
                Try
                    Dim originalReference As MethodReference = TryCast(instruction.Operand, MethodReference)
                    Dim originalMethod As MethodDefinition = originalReference.Resolve

                    If originalMethod.PInvokeInfo.EntryPoint.StartsWith("#") Then
                        originalMethod = Renamer.RenameMethod(originalMethod, "")
                        CompletedMethods.Add(originalMethod)
                    Else
                        Dim functionName As String = If(originalMethod.PInvokeInfo.EntryPoint = String.Empty, originalMethod.Name, originalMethod.PInvokeInfo.EntryPoint)

                        If functionName.ToLower.StartsWith("loadlibrary") OrElse functionName.ToLower.StartsWith("getprocaddress") OrElse functionName.ToLower.StartsWith("virtualprotect") Then
                            Continue For
                        Else
                            Dim dllName As String = originalMethod.PInvokeInfo.Module.Name

                            Dim is86 = If(Context.InputAssembly.MainModule.Architecture = TargetArchitecture.I386, True, False)
                            Dim dllSourcePath = If(is86, Environment.GetFolderPath(Environment.SpecialFolder.System), Environment.SpecialFolder.SystemX86)

                            If Not File.Exists(originalMethod.PInvokeInfo.Module.Name) Then
                                Dim dllNameEndswithDll As Boolean = dllName.ToLower.EndsWith(".dll")
                                Dim dllEntireNew As String = If(dllNameEndswithDll, Path.Combine(dllSourcePath, dllName), Path.Combine(dllSourcePath, dllName) & ".dll")

                                If File.Exists(dllEntireNew) Then
                                    Dim TmpPath = Path.GetTempFileName & ".txt"

                                    If Not ExportedDll.Any(Function(f) f.FileName = dllEntireNew) Then
                                        Shell(Functions.GetTempFolder & "\dllexp.exe /from_files " & Chr(34) & dllEntireNew & Chr(34) & " /scomma " & Chr(34) & TmpPath & Chr(34), AppWinStyle.Hide, True)
                                        Dim lines = File.ReadAllLines(TmpPath)

                                        For Each line In lines
                                            Dim dllexp As New NativeDllFunction
                                            Dim lineSplitted = line.Split(",")

                                            dllexp.FunctionName = lineSplitted(0)
                                            dllexp.FileName = dllEntireNew
                                            dllexp.ExportedFunction = If(lineSplitted(6) = "Exported Function", True, False)

                                            ExportedDll.Add(dllexp)

                                            If ReadyToGeneratePinvoke(originalMethod, dllexp, functionName) Then
                                                PinvokeCreate.InitPinvokeInfos(originalMethod, body.Method.DeclaringType)
                                                PinvokeCreate.CreatePinvokeBody(LoaderInvoke.DynamicInvoke)
                                                CompletedMethods.Add(originalMethod)
                                            Else
                                                Continue For
                                            End If
                                        Next
                                    Else
                                        Dim vals = ExportedDll.Where(Function(f) f.FileName = dllEntireNew)
                                        For Each dllexp In vals
                                            If ReadyToGeneratePinvoke(originalMethod, dllexp, functionName) Then
                                                PinvokeCreate.InitPinvokeInfos(originalMethod, body.Method.DeclaringType)
                                                PinvokeCreate.CreatePinvokeBody(LoaderInvoke.DynamicInvoke)
                                                CompletedMethods.Add(originalMethod)
                                            Else
                                                Continue For
                                            End If
                                        Next
                                    End If
                                Else
                                    Continue For
                                End If
                            Else
                                Continue For
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Continue For
                End Try
            Next
        End Sub

        Private Function ReadyToGeneratePinvoke(originalMethod As MethodDefinition, dllexp As NativeDllFunction, functionName As String) As Boolean

            If dllexp.ExportedFunction Then
                If dllexp.FunctionName.ToLower = functionName.ToLower Then
                    Return True
                Else
                    If (dllexp.FunctionName.ToUpper.EndsWith("A") OrElse dllexp.FunctionName.ToUpper.EndsWith("W")) AndAlso dllexp.FunctionName.Substring(0, dllexp.FunctionName.Length - 1).ToLower = functionName.ToLower AndAlso dllexp.ExportedFunction Then
                        If originalMethod.PInvokeInfo.IsCharSetAnsi Then
                            originalMethod.Name = functionName & "A"
                            originalMethod.PInvokeInfo.EntryPoint = functionName & "A"
                            Return True
                        ElseIf originalMethod.PInvokeInfo.IsCharSetUnicode Then
                            originalMethod.Name = functionName & "W"
                            originalMethod.PInvokeInfo.EntryPoint = functionName & "W"
                            Return True
                        ElseIf originalMethod.PInvokeInfo.IsCharSetAuto Then
                            Return False
                        ElseIf originalMethod.PInvokeInfo.IsCharSetNotSpec Then
                            originalMethod.Name = functionName & "A"
                            originalMethod.PInvokeInfo.EntryPoint = functionName & "A"
                            Return True
                        End If
                    Else
                        Return False
                    End If
                    Return False
                End If
            Else
                Return False
            End If

        End Function

#End Region

    End Class

End Namespace

