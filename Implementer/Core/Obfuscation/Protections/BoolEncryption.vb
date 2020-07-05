Imports Mono.Cecil
Imports Mono.Cecil.Rocks
Imports Mono.Cecil.Cil
Imports Helper.CecilHelper
Imports Implementer.Core.Obfuscation.Builder
Imports Implementer.Core.Dependencing.DependenciesInfos

Namespace Core.Obfuscation.Protections
    Public Class BoolEncryption
        Inherits Protection

#Region " Fields "

        Private DecryptReadResources As DecryptionResources
        Private DecryptInt As DecryptionInteger
        Private DecryptOdd As DecryptionOdd
        Private DecryptPrime As DecryptionPrime
        Private EncryptToResources As EncryptType

        Private ReadOnly MtdByInteger As Dictionary(Of Integer, MethodDefinition)
        Private ReadOnly Types As List(Of TypeDefinition)
        Private ReadOnly CompletedMethods As Mono.Collections.Generic.Collection(Of MethodDefinition)
        Private ReadOnly Rand As Random
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Boolean encryption ...)"
            End Get
        End Property
        Public Overrides ReadOnly Property Name As String = "Boolean encryption"
        Public Overrides Property Context As ProtectionContext
        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean = True
        Public Overrides ReadOnly Property ProgressIncrement As Integer = 40
        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Methods "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.EncryptBoolean Then
                _Enabled = True
                PackerState = PackerStat
                MtdByInteger = New Dictionary(Of Integer, MethodDefinition)
                Types = New List(Of TypeDefinition)
                CompletedMethods = New Mono.Collections.Generic.Collection(Of MethodDefinition)
                EncryptToResources = If(Contex.Params.TaskAccept.Packer.Enabled, EncryptType.ByDefault, EncryptType.ToResources)
                Rand = New Random
            End If
        End Sub

        Public Overrides Sub Execute()
            'DO NOT encrypt OVER another protection encryption , so we will check the best configuration before !!!
            If Context.Params.TaskAccept.Obfuscation.HidePublicCalls Then
                EncryptToResources = EncryptType.ByDefault
            Else
                EncryptToResources = If(Context.Params.TaskAccept.Obfuscation.CompressResources OrElse Context.Params.TaskAccept.Obfuscation.EncryptResources, EncryptType.ByDefault, EncryptType.ToResources)
                If PackerState Then
                    EncryptToResources = EncryptType.ByDefault
                End If
            End If

            'Build Decryption Resources Stub and retrieve its methoddefinition
            If EncryptToResources = EncryptType.ToResources Then
                DecryptReadResources = New DecryptionResources(New ResManagerContext(Context.InputAssembly, PackerState, Context.Randomizer, Context.Randomizer.GenerateNew))
                CompletedMethods.Add(DecryptReadResources.ReadFromResource)
                'Build Decryption Integer Stub and retrieve its methoddefinition
                DecryptInt = New DecryptionInteger(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
                CompletedMethods.Add(DecryptInt.DecryptInt())
            End If

            'Build Decryption Odd Stub and retrieve its methoddefinition
            DecryptOdd = New DecryptionOdd(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
            CompletedMethods.Add(DecryptOdd.DecryptOdd())

            'Build Decryption Prime Stub and retrieve its methoddefinition
            DecryptPrime = New DecryptionPrime(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
            CompletedMethods.Add(DecryptPrime.DecryptPrime())

            'ModuleDefintion types iteration 
            For Each m As ModuleDefinition In Context.InputAssembly.Modules
                Types.AddRange(m.GetAllTypes())
                For Each type As TypeDefinition In Types
                    IterateType(type)
                Next
                Types.Clear()
            Next

            'Inject encrypted data into assemblydefintion resources
            If EncryptToResources = EncryptType.ToResources Then
                DecryptReadResources.InjectResource()
            End If

            'Cleanup
            MethodByClear()
            DeleteStubs()
        End Sub

        Private Sub IterateType(td As TypeDefinition)
            Dim publicMethods As New List(Of MethodDefinition)()
            publicMethods.AddRange(From m In td.Methods Where (m.HasBody AndAlso m.Body.Instructions.Count >= 2 AndAlso
                                                            Not CompletedMethods.Contains(m) AndAlso
                                                            Not Utils.HasUnsafeInstructions(m) AndAlso
                                                            Not TypeExtensions.HasCustomAttributeByMemberName(m.DeclaringType, "EditorBrowsableAttribute")))
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
            Dim il = body.GetILProcessor()
            Dim instructionsToExpand As List(Of Instruction) = New List(Of Instruction)()

            For Each instruction As Instruction In instructions
                If instruction.OpCode = OpCodes.Ldc_I4 Then
                    If Utils.IsValidBoolOperand(instruction) Then
                        Dim instructNext = instruction.Next
                        If Utils.IsValidOperand(instructNext) Then
                            If instructNext.OpCode = OpCodes.Stloc Then
                                Dim varIndex = CInt(instructNext.Operand.ToString.ToLower.Replace("v_", String.Empty))
                                Dim varType = body.Variables(varIndex).VariableType
                                If varType.ToString = "System.Boolean" Then
                                    instructionsToExpand.Add(instruction)
                                End If
                            Else
                                If instructNext.Operand.ToString.ToLower.EndsWith("system.boolean)") OrElse instructNext.Operand.ToString.ToLower.StartsWith("system.boolean") Then
                                    instructionsToExpand.Add(instruction)
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            For Each instruction As Instruction In instructionsToExpand
                Dim value = CInt(instruction.Operand)
                Dim mdFinal As MethodDefinition = Nothing
                CreateMethod(mdFinal, value, body.Method)
                InjectMethodCall(mdFinal, instruction, il)
            Next

        End Sub

        Private Sub InjectMethodCall(mdFinal As MethodDefinition, instruction As Instruction, ilProc As ILProcessor)
            If (Not mdFinal Is Nothing) Then
                If mdFinal.DeclaringType.IsNotPublic Then
                    mdFinal.DeclaringType.IsPublic = True
                End If
                Dim CallMethod = ilProc.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(mdFinal))
                ilProc.Replace(instruction, CallMethod)
                CompletedMethods.Add(mdFinal)
            End If
        End Sub

        Private Sub CreateMethod(ByRef mDef As MethodDefinition, value%, ByRef md As MethodDefinition)
            If Context.Randomizer.GenerateBoolean Then
                mDef = New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), Context.InputAssembly.MainModule.Import(GetType(Boolean)))
                mDef.Body = New MethodBody(mDef)

                If EncryptToResources = EncryptType.ToResources Then
                    Dim integ = Context.Randomizer.GenerateInvisible

                    Dim encSt = Generator.IntEncrypt(TestNumber(value <> 0), integ)
                    Dim dataKeyName = Context.Randomizer.GenerateNew
                    DecryptReadResources.AddResource(dataKeyName, encSt)

                    Dim ilProc As ILProcessor = mDef.Body.GetILProcessor()
                    With ilProc
                        .Body.MaxStackSize = 8
                        .Body.InitLocals = True

                        mDef.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Boolean))))
                        .Emit(OpCodes.Ldstr, dataKeyName)
                        .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptReadResources.ReadFromResource))
                        .Emit(OpCodes.Ldc_I4, integ)
                        .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptInt.DecryptInt))
                        .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptOdd.DecryptOdd))
                        .Emit(OpCodes.Stloc_0)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ret)
                    End With

                    md.DeclaringType.Methods.Add(mDef)
                Else
                    Dim encStr = TestNumber(CInt(value) <> 0)
                    Dim IlProc1 As ILProcessor = mDef.Body.GetILProcessor()
                    With IlProc1
                        .Body.MaxStackSize = 4
                        .Body.InitLocals = True
                        mDef.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Boolean))))
                        .Emit(OpCodes.Ldc_I4, encStr)
                        .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptOdd.DecryptOdd))
                        .Emit(OpCodes.Stloc_0)
                        .Emit(OpCodes.Ldloc_0)
                        .Emit(OpCodes.Ret)
                    End With

                    md.DeclaringType.Methods.Add(mDef)
                End If
            Else
                Dim UnPrime = Rand.Next(Generator.numberUnPrime.Length)
                Dim Prime = Rand.Next(Generator.numberPrime.Length)
                Dim valFinale%

                If value = 0 Then
                    valFinale = Generator.numberUnPrime(UnPrime)
                ElseIf value = 1 Then
                    valFinale = Generator.numberPrime(Prime)
                End If

                mDef = New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), Context.InputAssembly.MainModule.Import(GetType(Boolean)))
                mDef.Body = New MethodBody(mDef)
                mDef.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Boolean))))

                Dim IlProc1 As ILProcessor = mDef.Body.GetILProcessor()
                With IlProc1
                    .Body.MaxStackSize = 4
                    .Body.InitLocals = True

                    .Emit(OpCodes.Ldc_I4, valFinale)
                    .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptPrime.DecryptPrime))
                    .Emit(OpCodes.Stloc_0)
                    .Emit(OpCodes.Ldloc_0)
                    .Emit(OpCodes.Ret)
                End With

                md.DeclaringType.Methods.Add(mDef)
            End If
        End Sub

        Private Function IsOdd(num%) As Boolean
            Return num Mod 2 <> 0
        End Function

        Private Function TestNumber(isPair As Boolean) As Integer
            Dim n%
            Dim result As Boolean = False
            Do While result = False
                n = Rand.Next(1000000, 99999999)
                result = (IsOdd(n) = isPair)
                If result Then
                    Exit Do
                End If
            Loop
            Return n
        End Function

        Private Sub DeleteStubs()
            If Not DecryptReadResources Is Nothing Then DecryptReadResources.RemoveStubFile()
            If Not DecryptInt Is Nothing Then DecryptInt.RemoveStubFile()
            If Not DecryptOdd Is Nothing Then DecryptOdd.RemoveStubFile()
        End Sub

        Private Sub MethodByClear()
            MtdByInteger.Clear()
        End Sub
#End Region

    End Class
End Namespace
