Imports Mono.Cecil
Imports Mono.Cecil.Rocks
Imports Mono.Cecil.Cil
Imports Helper.RandomizeHelper
Imports Helper.CecilHelper
Imports System.Text.RegularExpressions
Imports Helper.CryptoHelper
Imports Implementer.Core.Obfuscation.Builder
Imports Implementer.Core.Dependencing.DependenciesInfos

Namespace Core.Obfuscation.Protections
    Public Class NumericEncryption
        Inherits Protection

#Region " Fields "
        Private DecryptReadResources As DecryptionResources
        Private DecryptInt As DecryptionInteger
        Private DecryptRPN As DecryptionRpn
        Private ReadOnly MethodByInteger As Dictionary(Of Integer, MethodDefinition)
        Private ReadOnly MethodByDouble As Dictionary(Of Double, MethodDefinition)
        Private ReadOnly MethodBySingle As Dictionary(Of Single, MethodDefinition)
        Private ReadOnly Types As List(Of TypeDefinition)
        Private ReadOnly OpCodeDic As Dictionary(Of OpCode, String)
        Private ReadOnly CompletedMethods As Mono.Collections.Generic.Collection(Of MethodDefinition)
        Private EncryptToResources As EncryptType
        Private ReadOnly Rand As Random
        Private ReadOnly PackerState As Boolean

        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Numeric encryption part 1...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String = "Numeric encryption"
        Public Overrides Property Context As ProtectionContext
        Public Overrides ReadOnly Property MustReadWriteAssembly As Boolean = True
        Public Overrides ReadOnly Property ProgressIncrement As Integer = 38
        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.EncryptNumeric Then
                _Enabled = True
                PackerState = PackerStat
                CompletedMethods = New Mono.Collections.Generic.Collection(Of MethodDefinition)
                MethodByInteger = New Dictionary(Of Integer, MethodDefinition)
                MethodByDouble = New Dictionary(Of Double, MethodDefinition)
                MethodBySingle = New Dictionary(Of Single, MethodDefinition)
                Rand = New Random
                Types = New List(Of TypeDefinition)
                OpCodeDic = New Dictionary(Of OpCode, String) From {{OpCodes.Ldc_I4, "System.Int32"}, {OpCodes.Ldc_R4, "System.Single"}, {OpCodes.Ldc_R8, "System.Double"}}
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()

            'DO NOT encrypt OVER another protection encryption , so we will check the best configuration before !!!
            If Context.Params.TaskAccept.Obfuscation.HidePublicCalls Then
                EncryptToResources = EncryptType.ByDefault
            Else
                EncryptToResources = If(Context.Params.TaskAccept.Obfuscation.CompressResources OrElse Context.Params.TaskAccept.Obfuscation.EncryptResources, EncryptType.ByDefault, EncryptType.ToResources)
                If PackerState Then EncryptToResources = EncryptType.ByDefault
            End If

            'Build Decryption Resources Stub and retrieve its methoddefinition
            If EncryptToResources = EncryptType.ToResources Then
                DecryptReadResources = New DecryptionResources(New ResManagerContext(Context.InputAssembly, PackerState, Context.Randomizer, Context.Randomizer.GenerateNew))
                CompletedMethods.Add(DecryptReadResources.ReadFromResource)
            End If

            'Build Decryption Integer Stub and retrieve its methoddefinition
            DecryptInt = New DecryptionInteger(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
            CompletedMethods.Add(DecryptInt.DecryptInt())

            'Build Decryption RPN Stub and retrieve those methoddefinition
            DecryptRPN = New DecryptionRpn(New StubContext(Context.InputAssembly, PackerState, Context.Randomizer))
            CompletedMethods.Add(DecryptRPN.BrowseOperands())
            CompletedMethods.Add(DecryptRPN.CleanupExpressions())

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
                                                            Not TypeExtensions.HasCustomAttributeByMemberName(m.DeclaringType, "EditorBrowsableAttribute") AndAlso
                                                            Not Utils.HasUnsafeInstructions(m)))
            Try
                For Each md In publicMethods
                    md.Body.SimplifyMacros
                    ProcessInstructions(md.Body)
                    md.Body.OptimizeMacros
                    md.Body.ComputeOffsets()
                    md.Body.ComputeHeader()
                Next
            Catch ex As Exception
                MsgBox("Numeric encoding Error : " & vbNewLine & ex.ToString)
            End Try
            publicMethods.Clear()
        End Sub

        Private Sub ProcessInstructions(body As MethodBody)
            Dim instructions = body.Instructions
            Dim il = body.GetILProcessor()
            Dim instructionsToExpand As List(Of Instruction) = New List(Of Instruction)()

            For Each instruction As Instruction In instructions
                Select Case instruction.OpCode
                    Case OpCodes.Ldc_I4, OpCodes.Ldc_R4, OpCodes.Ldc_R8
                        OpCodesFilter(instruction.OpCode, instruction, instructionsToExpand, body)
                End Select
            Next

            For Each instruction As Instruction In instructionsToExpand
                Select Case instruction.OpCode
                    Case OpCodes.Ldc_I4
                        Dim mdFinal As MethodDefinition = Nothing
                        If Not MethodByInteger.TryGetValue(CInt(instruction.Operand), mdFinal) Then
                            If Context.Randomizer.GenerateBoolean Then
                                If (instruction.Operand < Integer.MaxValue AndAlso instruction.Operand > 9) AndAlso (Not instruction.Next Is Nothing AndAlso Not instruction.Next.OpCode = OpCodes.Newarr) Then

                                    mdFinal = New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), body.Method.DeclaringType.Module.Import(GetType(Integer)))
                                    mdFinal.Body = New MethodBody(mdFinal)
                                    mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))

                                    If EncryptToResources = EncryptType.ToResources Then
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))

                                        Dim integ = Context.Randomizer.GenerateInvisible
                                        Dim encStr = Generator.IntEncrypt(CInt(instruction.Operand), integ)
                                        Dim dataKeyName = Context.Randomizer.GenerateNew
                                        DecryptReadResources.AddResource(dataKeyName, encStr)

                                        Dim ilProc = mdFinal.Body.GetILProcessor()
                                        With ilProc
                                            .Body.MaxStackSize = 4
                                            .Body.InitLocals = True

                                            .Emit(OpCodes.Ldstr, dataKeyName)
                                            .Emit(OpCodes.Stloc_1)
                                            .Emit(OpCodes.Ldloc_1)
                                            .Emit(OpCodes.Call, body.Method.Module.Import(DecryptReadResources.ReadFromResource))
                                            .Emit(OpCodes.Stloc_2)
                                            .Emit(OpCodes.Ldloc_2)
                                            .Emit(OpCodes.Ldc_I4, integ)
                                            .Emit(OpCodes.Stloc_3)
                                            .Emit(OpCodes.Ldloc_3)
                                            .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptInt.DecryptInt()))
                                            .Emit(OpCodes.Stloc_0)
                                            .Emit(OpCodes.Ldloc_0)
                                            .Emit(OpCodes.Ret)
                                        End With

                                        body.Method.DeclaringType.Methods.Add(mdFinal)
                                        MethodByInteger.Add(CInt(instruction.Operand), mdFinal)
                                    Else
                                        Dim integ = Context.Randomizer.GenerateInvisible
                                        Dim encStr = Generator.IntEncrypt(CInt(instruction.Operand), integ)

                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))

                                        Dim ilProc = mdFinal.Body.GetILProcessor()
                                        With ilProc
                                            .Body.MaxStackSize = 4
                                            .Body.InitLocals = True
                                            .Emit(OpCodes.Ldstr, encStr)
                                            .Emit(OpCodes.Stloc_1)
                                            .Emit(OpCodes.Ldloc_1)
                                            .Emit(OpCodes.Ldc_I4, integ)
                                            .Emit(OpCodes.Stloc_2)
                                            .Emit(OpCodes.Ldloc_2)
                                            .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptInt.DecryptInt()))
                                            .Emit(OpCodes.Stloc_0)
                                            .Emit(OpCodes.Ldloc_0)
                                            .Emit(OpCodes.Ret)
                                        End With

                                        body.Method.DeclaringType.Methods.Add(mdFinal)
                                        MethodByInteger.Add(CInt(instruction.Operand), mdFinal)
                                    End If
                                ElseIf (CInt(instruction.Operand) >= 1 AndAlso CInt(instruction.Operand) < 10) AndAlso (Not instruction.Next Is Nothing AndAlso Not instruction.Next.OpCode = OpCodes.Newarr) Then
                                    mdFinal = New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), body.Method.DeclaringType.Module.Import(GetType(Integer)))
                                    mdFinal.Body = New MethodBody(mdFinal)
                                    mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))

                                    Dim integ = Context.Randomizer.GenerateInvisible
                                    Dim encStr = Generator.IntEncrypt(CInt(instruction.Operand), integ)

                                    Dim ilProc = mdFinal.Body.GetILProcessor()
                                    With ilProc
                                        .Body.MaxStackSize = 4
                                        .Body.InitLocals = True

                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))

                                        .Emit(OpCodes.Ldstr, encStr)
                                        .Emit(OpCodes.Stloc_1)
                                        .Emit(OpCodes.Ldloc_1)
                                        .Emit(OpCodes.Ldc_I4, integ)
                                        .Emit(OpCodes.Stloc_2)
                                        .Emit(OpCodes.Ldloc_2)
                                        .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptInt.DecryptInt()))
                                        .Emit(OpCodes.Stloc_0)
                                        .Emit(OpCodes.Ldloc_0)
                                        .Emit(OpCodes.Ret)
                                    End With

                                    body.Method.DeclaringType.Methods.Add(mdFinal)
                                    MethodByInteger.Add(CInt(instruction.Operand), mdFinal)
                                End If
                            Else
                                If (instruction.Operand < Integer.MaxValue AndAlso instruction.Operand >= 2) AndAlso (Not instruction.Next Is Nothing AndAlso Not instruction.Next.OpCode = OpCodes.Newarr) Then
                                    Dim resultPrimes = PrimeFactors(instruction.Operand)
                                    Dim countPrimes = resultPrimes.Count
                                    If countPrimes >= 2 Then
                                        Dim num = CInt(instruction.Operand)
                                        Dim divider0 = 0
                                        Dim resultdivider0 = DetermineDiv(num, divider0)
                                        Dim StrDivider0 = resultdivider0 & " / " & divider0
                                        Dim divider1 = 0
                                        Dim resultdivider1 = DetermineDiv(num, divider1)
                                        Dim StrDivider1 = resultdivider1 & " / " & divider1
                                        Dim StrDivider = StrDivider0 & " - " & StrDivider1 & " + "

                                        Dim strPrimes = String.Empty
                                        strPrimes = String.Join(" ", resultPrimes).TrimEnd(" ")
                                        For k% = 0 To countPrimes - 2
                                            strPrimes &= " *"
                                        Next

                                        Dim InFix = (StrDivider & strPrimes).TrimEnd(" ")
                                        Dim postfix = String.Empty
                                        Dim bResult = InfixToPostfixConvert(InFix, postfix)
                                        postfix = postfix.TrimEnd(" ").Replace(" ", ",")

                                        mdFinal = New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), Context.InputAssembly.MainModule.Import(GetType(Integer)))
                                        mdFinal.Body = New MethodBody(mdFinal)
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))

                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String()))))

                                        Dim ilProc = mdFinal.Body.GetILProcessor()
                                        With ilProc
                                            .Body.MaxStackSize = 4
                                            .Body.InitLocals = True
                                            .Emit(OpCodes.Ldstr, postfix)
                                            .Emit(OpCodes.Stloc_1)
                                            .Emit(OpCodes.Ldloc_1)
                                            .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptRPN.CleanupExpressions))
                                            .Emit(OpCodes.Stloc_2)
                                            .Emit(OpCodes.Ldloc_2)
                                            .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptRPN.BrowseOperands))
                                            .Emit(OpCodes.Stloc_0)
                                            .Emit(OpCodes.Ldloc_0)
                                            .Emit(OpCodes.Ret)
                                        End With

                                        body.Method.DeclaringType.Methods.Add(mdFinal)
                                        MethodByInteger.Add(CInt(instruction.Operand), mdFinal)
                                    Else
                                        Dim divider0 = 0
                                        Dim resultdivider0 = DetermineDiv(CInt(instruction.Operand), divider0)
                                        Dim str = resultdivider0 & "," & divider0 & ",/"

                                        mdFinal = New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), Context.InputAssembly.MainModule.Import(GetType(Integer)))
                                        mdFinal.Body = New MethodBody(mdFinal)
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer))))
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String))))
                                        mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(String()))))

                                        Dim ilProc = mdFinal.Body.GetILProcessor()
                                        With ilProc
                                            .Body.MaxStackSize = 4
                                            .Body.InitLocals = True

                                            .Emit(OpCodes.Ldstr, str)
                                            .Emit(OpCodes.Stloc_1)
                                            .Emit(OpCodes.Ldloc_1)
                                            .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptRPN.CleanupExpressions))
                                            .Emit(OpCodes.Stloc_2)
                                            .Emit(OpCodes.Ldloc_2)
                                            .Emit(OpCodes.Call, Context.InputAssembly.MainModule.Import(DecryptRPN.BrowseOperands))
                                            .Emit(OpCodes.Stloc_0)
                                            .Emit(OpCodes.Ldloc_0)
                                            .Emit(OpCodes.Ret)
                                        End With

                                        body.Method.DeclaringType.Methods.Add(mdFinal)
                                        MethodByInteger.Add(CInt(instruction.Operand), mdFinal)
                                    End If
                                End If
                            End If
                        Else
                            mdFinal = MethodByInteger.Item(CInt(instruction.Operand))
                        End If
                        If (Not mdFinal Is Nothing) Then
                            If mdFinal.DeclaringType.IsNotPublic Then
                                mdFinal.DeclaringType.IsPublic = True
                            End If
                            Dim Instruct = il.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(mdFinal))
                            il.Replace(instruction, Instruct)
                            CompletedMethods.Add(mdFinal)
                        End If
                    Case OpCodes.Ldc_R4
                        If Utils.IsValidOperand(instruction) AndAlso CSng(instruction.Operand) >= 0 Then
                            Dim mdFinal As MethodDefinition = Nothing
                            If Not MethodBySingle.TryGetValue(CSng(instruction.Operand), mdFinal) Then
                                Dim Sng As Single
                                If Single.TryParse(instruction.Operand, Sng) Then
                                    Dim pdefName = Context.Randomizer.GenerateNew

                                    Dim pdef As New PropertyDefinition(pdefName, PropertyAttributes.None, Context.InputAssembly.MainModule.Import(GetType(Single)))
                                    body.Method.DeclaringType.Properties.Add(pdef)

                                    mdFinal = New MethodDefinition(("get_" & pdef.Name), MethodAttributes.Static Or MethodAttributes.Public, pdef.PropertyType)
                                    mdFinal.Body = New MethodBody(mdFinal)
                                    mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Single))))

                                    pdef.GetMethod = mdFinal
                                    pdef.DeclaringType.Methods.Add(mdFinal)

                                    If Not pdef.DeclaringType.IsInterface Then
                                        Dim iLProcessor = mdFinal.Body.GetILProcessor
                                        With iLProcessor
                                            .Body.MaxStackSize = 1
                                            .Body.InitLocals = True

                                            .Emit(OpCodes.Ldc_R4, Sng)
                                            .Emit(OpCodes.Ret)
                                        End With
                                    Else
                                        mdFinal.IsAbstract = True
                                        mdFinal.IsVirtual = True
                                        mdFinal.IsNewSlot = True
                                    End If
                                    mdFinal.IsSpecialName = True
                                    mdFinal.IsGetter = True

                                    MethodBySingle.Add(CSng(instruction.Operand), mdFinal)
                                End If
                            Else
                                mdFinal = MethodBySingle.Item(CSng(instruction.Operand))
                            End If
                            If (Not mdFinal Is Nothing) Then
                                Dim Instruct = il.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(mdFinal))
                                il.Replace(instruction, Instruct)
                                CompletedMethods.Add(mdFinal)
                            End If
                        End If
                    Case OpCodes.Ldc_R8
                        If Utils.IsValidOperand(instruction) AndAlso CDbl(instruction.Operand) >= 0 Then
                            Dim mdFinal As MethodDefinition = Nothing

                            If Not MethodByDouble.TryGetValue(CDbl(instruction.Operand), mdFinal) Then
                                Dim integ As Double
                                If Double.TryParse(instruction.Operand, integ) Then
                                    Dim pdefName = Context.Randomizer.GenerateNew

                                    Dim pdef As New PropertyDefinition(pdefName, PropertyAttributes.None, Context.InputAssembly.MainModule.Import(GetType(Double)))
                                    body.Method.DeclaringType.Properties.Add(pdef)

                                    mdFinal = New MethodDefinition(("get_" & pdef.Name), MethodAttributes.Static Or MethodAttributes.Public, pdef.PropertyType)
                                    mdFinal.Body = New MethodBody(mdFinal)
                                    mdFinal.Body.Variables.Add(New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Double))))

                                    pdef.GetMethod = mdFinal
                                    pdef.DeclaringType.Methods.Add(mdFinal)

                                    If Not pdef.DeclaringType.IsInterface Then
                                        Dim iLProcessor = mdFinal.Body.GetILProcessor
                                        With iLProcessor
                                            .Body.MaxStackSize = 1
                                            .Body.InitLocals = True
                                            .Emit(OpCodes.Ldc_R8, integ)
                                            .Emit(OpCodes.Ret)
                                        End With
                                    Else
                                        mdFinal.IsAbstract = True
                                        mdFinal.IsVirtual = True
                                        mdFinal.IsNewSlot = True
                                    End If
                                    mdFinal.IsSpecialName = True
                                    mdFinal.IsGetter = True

                                    MethodByDouble.Add(CDbl(instruction.Operand), mdFinal)
                                End If
                            Else
                                mdFinal = MethodByDouble.Item(CDbl(instruction.Operand))
                            End If
                            If (Not mdFinal Is Nothing) Then
                                Dim Instruct = il.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(mdFinal))
                                il.Replace(instruction, Instruct)
                                CompletedMethods.Add(mdFinal)
                            End If
                        End If
                End Select
            Next
        End Sub

        Private Sub OpCodesFilter(opcode As OpCode, Instruction As Instruction, instructionsToExpand As List(Of Instruction), body As MethodBody)
            Dim opCodeStr = OpCodeDic.Item(opcode)

            Dim instructNext = Instruction.Next
            If instructNext.OpCode = OpCodes.Stloc OrElse instructNext.OpCode = OpCodes.Ldloc Then
                Dim varIndex = CInt(instructNext.Operand.ToString.ToLower.Replace("v_", String.Empty))
                Dim varType = body.Variables(varIndex).VariableType
                If varType.ToString = opCodeStr Then
                    If Not Instruction.Operand = 0 Then
                        instructionsToExpand.Add(Instruction)
                    End If
                End If
            Else
                If Not instructNext.Operand Is Nothing Then
                    If instructNext.Operand.ToString.ToLower.EndsWith(opCodeStr.ToLower & ")") OrElse instructNext.Operand.ToString.ToLower.StartsWith(opCodeStr.ToLower) Then
                        If Not Instruction.Operand = 0 Then
                            instructionsToExpand.Add(Instruction)
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function PrimeFactors(a As Integer) As List(Of Integer)
            Dim retval As New List(Of Integer)
            Dim b As Integer = 2
            While a > 1
                While a Mod b = 0
                    a /= b
                    retval.Add(b)
                End While
                b += 1
            End While
            Return retval
        End Function

        Private Function DetermineDiv(real As Integer, ByRef div As Integer) As Integer
            Dim num As Integer = Rand.Next(5, 40)
            div = num
            Dim v% = real
            Try
                v = (real * num)
            Catch ex As OverflowException
                div = 1
            End Try
            Return v
        End Function

        Private Function InfixToPostfixConvert(ByRef infixBuffer As String, ByRef postfixBuffer As String) As Boolean
            Dim prior%
            postfixBuffer = ""

            Dim s1 As New Stack(Of Char)

            For i% = 0 To infixBuffer.Length - 1
                Dim item As Char = infixBuffer.Chars(i)
                Select Case item
                    Case "+"c, "-"c, "*"c, "/"c
                        If (s1.Count <= 0) Then
                            s1.Push(item)
                        Else
                            If ((s1.Peek = "*"c) OrElse (s1.Peek = "/"c)) Then
                                prior = 1
                            Else
                                prior = 0
                            End If
                            If (prior = 1) Then
                                Select Case item
                                    Case "+"c, "-"c
                                        postfixBuffer = (postfixBuffer & CStr(s1.Pop))
                                        i -= 1
                                        Continue For
                                End Select
                                postfixBuffer = (postfixBuffer & CStr(s1.Pop))
                                i -= 1
                            Else
                                Select Case item
                                    Case "+"c, "-"c
                                        postfixBuffer = (postfixBuffer & CStr(s1.Pop))
                                        s1.Push(item)
                                        Continue For
                                End Select
                                s1.Push(item)
                            End If
                        End If
                        Exit Select
                    Case Else
                        postfixBuffer = (postfixBuffer & CStr(item))
                        Exit Select
                End Select
            Next

            Dim len% = s1.Count
            For j% = 0 To len - 1
                postfixBuffer = (postfixBuffer & CStr(s1.Pop))
            Next

            postfixBuffer = postfixBuffer.Replace("/", " / ").Replace("*", " * ").Replace("+", " + ").Replace("-", " - ")
            postfixBuffer = New Regex("[ ]{2,}", RegexOptions.None).Replace(postfixBuffer, " ")
            Return True
        End Function

        Private Sub DeleteStubs()
            If Not DecryptReadResources Is Nothing Then DecryptReadResources.RemoveStubFile()
            If Not DecryptInt Is Nothing Then DecryptInt.RemoveStubFile()
            If Not DecryptRPN Is Nothing Then DecryptRPN.RemoveStubFile()
        End Sub

        Private Sub MethodByClear()
            MethodByInteger.Clear()
            MethodByDouble.Clear()
            MethodBySingle.Clear()
        End Sub
#End Region

    End Class

End Namespace
