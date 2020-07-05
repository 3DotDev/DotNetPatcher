Imports Mono.Cecil
Imports Helper.CecilHelper
Imports Mono.Cecil.Cil
Imports Mono.Cecil.Rocks
Imports Helper.RandomizeHelper

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class Scattering
        Inherits Protection

#Region " Private Class "
        Private Class MethodsCollection(Of T)
            Inherits List(Of KeyValuePair(Of T, MethodDefinition))

            Private ReadOnly KeyMethods As List(Of KeyValuePair(Of Integer, MethodDefinition))
            Private ReadOnly Rnd As Random

            Public Sub New()
                KeyMethods = New List(Of KeyValuePair(Of Integer, MethodDefinition))
                Rnd = New Random
            End Sub

            Public Overloads Sub Add(key As T, value As MethodDefinition)
                Add(New KeyValuePair(Of T, MethodDefinition)(key, value))
            End Sub

            Public Overloads Function GetValue(md As MethodDefinition, key As T, Randomizer As Randomizer) As Tuple(Of MethodDefinition, Integer)
                Dim mdFinal As MethodDefinition
                Dim rand As Integer
                Dim tups As Tuple(Of MethodDefinition, Integer)

                Dim allMethods = Where(Function(f) f.Value?.DeclaringType.FullName = md.DeclaringType.FullName)
                If allMethods.Count > 0 Then
                    If Any(Function(m) m.Value?.DeclaringType.FullName = md.DeclaringType.FullName AndAlso m.Key.Equals(key)) Then
                        mdFinal = Where(Function(m) m.Value?.DeclaringType.FullName = md.DeclaringType.FullName AndAlso m.Key.Equals(key)).First.Value
                        rand = KeyMethods.Where(Function(r) r.Value Is mdFinal).First.Key
                        tups = Tuple.Create(mdFinal, rand)
                    Else
                        tups = CreateMethod(key, md, Randomizer)
                        mdFinal = tups.Item1
                        rand = tups.Item2
                        AddToList(mdFinal, key, rand)
                    End If
                Else
                    tups = CreateMethod(key, md, Randomizer)
                    mdFinal = tups.Item1
                    rand = tups.Item2
                    AddToList(mdFinal, key, rand)
                End If
                Return tups
            End Function

            Private Sub AddToList(mdFinal As MethodDefinition, key As T, Rand As Integer)
                If Not mdFinal Is Nothing Then
                    mdFinal.Attributes = MethodAttributes.Static Or MethodAttributes.Private
                    Add(New KeyValuePair(Of T, MethodDefinition)(key, mdFinal))
                    KeyMethods.Add(New KeyValuePair(Of Integer, MethodDefinition)(Rand, mdFinal))
                End If
            End Sub

            Private Function CreateMethod(value As Object, md As MethodDefinition, Randomizer As Randomizer) As Tuple(Of MethodDefinition, Integer)
                Dim opc As OpCode = Nothing
                Dim Rand = Rnd.Next(8, 100)
                Select Case value.GetType
                    Case GetType(String)
                        opc = OpCodes.Ldstr
                    Case GetType(Integer)
                        opc = OpCodes.Ldc_I4
                    Case GetType(Long)
                        opc = OpCodes.Ldc_I8
                    Case GetType(Single)
                        opc = OpCodes.Ldc_R4
                    Case GetType(Double)
                        opc = OpCodes.Ldc_R8
                    Case Else
                        Return Nothing
                End Select

                Dim item As New MethodDefinition(Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), md.DeclaringType.Module.Import(value.GetType))
                Dim par1 As New ParameterDefinition(md.DeclaringType.Module.Import(GetType(Integer)))
                Dim var1 As New VariableDefinition(md.DeclaringType.Module.Import(value.GetType))
                Dim var2 As New VariableDefinition(md.DeclaringType.Module.Import(GetType(Integer)))
                item.Parameters.Add(par1)
                item.Body.Variables.Add(var1)
                item.Body.Variables.Add(var2)

                Dim ilProc As ILProcessor = item.Body.GetILProcessor()
                With ilProc
                    .Body.MaxStackSize = 2
                    .Body.InitLocals = True

                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldc_I4_8))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Stloc_1))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldloc_1))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldarg_0))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Nop)) '4
                    .Body.Instructions.Add(ilProc.Create(opc, value))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Stloc_0))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Nop)) '7
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldloc_1)) '8
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldc_I4_1))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Add_Ovf))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Stloc_1))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldloc_1))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldc_I4, 100))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ble_S, .Body.Instructions(2)))
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ldloc_0)) '15
                    .Body.Instructions.Add(ilProc.Create(OpCodes.Ret))

                    ilProc.Replace(.Body.Instructions(7), ilProc.Create(OpCodes.Br_S, .Body.Instructions(15)))
                    ilProc.Replace(.Body.Instructions(4), ilProc.Create(OpCodes.Bne_Un_S, .Body.Instructions(8)))
                End With

                md.DeclaringType.Methods.Add(item)

                Return Tuple.Create(item, Rand)
            End Function
        End Class
#End Region

#Region " Fields "
        Private ReadOnly MdByString As MethodsCollection(Of String)
        Private ReadOnly MdByInteger As MethodsCollection(Of Integer)
        Private ReadOnly MdByLong As MethodsCollection(Of Long)
        Private ReadOnly MdByDouble As MethodsCollection(Of Double)
        Private ReadOnly MdBySingle As MethodsCollection(Of Single)
        Private ReadOnly MdByRef As Dictionary(Of MethodReference, MethodDefinition)
        Private ReadOnly Types As List(Of TypeDefinition)
        Private ReadOnly OpCodeDic As Dictionary(Of OpCode, String)
        Private ReadOnly CompletedMethods As Mono.Collections.Generic.Collection(Of MethodDefinition)
        Private ReadOnly PackerState As Boolean
        Private Rnd As Random
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Hide calls part 2...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Scattering"
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
                Return 75
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            PackerState = PackerStat
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.HidePublicCalls Then
                _Enabled = True
                MdByString = New MethodsCollection(Of String)
                MdByInteger = New MethodsCollection(Of Integer)
                MdByLong = New MethodsCollection(Of Long)
                MdByDouble = New MethodsCollection(Of Double)
                MdBySingle = New MethodsCollection(Of Single)
                MdByRef = New Dictionary(Of MethodReference, MethodDefinition)
                OpCodeDic = New Dictionary(Of OpCode, String) From {{OpCodes.Ldc_I4, "System.Int32"}, {OpCodes.Ldc_I8, "System.Int64"}, {OpCodes.Ldc_R4, "System.Single"}, {OpCodes.Ldc_R8, "System.Double"}}
                Types = New List(Of TypeDefinition)
                CompletedMethods = New Mono.Collections.Generic.Collection(Of MethodDefinition)
                Rnd = New Random
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            For Each m As ModuleDefinition In Context.InputAssembly.Modules
                Types.AddRange(m.GetAllTypes())
                For Each type As TypeDefinition In Types
                    IterateType(type)
                Next
                Types.Clear()
            Next

            MethodByClear()
        End Sub

        Private Sub MethodByClear()
            MdByString.Clear()
            MdByInteger.Clear()
            MdByLong.Clear()
            MdByDouble.Clear()
            MdBySingle.Clear()
            MdByRef.Clear()
            OpCodeDic.Clear()
        End Sub

        Private Sub IterateType(td As TypeDefinition)
            Dim publicMethods As New List(Of MethodDefinition)()
            publicMethods.AddRange(From m In td.Methods Where (m.HasBody AndAlso m.Body.Instructions.Count > 1 AndAlso Not CompletedMethods.Contains(m) AndAlso
                                                            Not m.DeclaringType.BaseType Is Nothing AndAlso
                                                            Not m.DeclaringType.BaseType.Name = "ApplicationSettingsBase" AndAlso
                                                            Not m.DeclaringType.BaseType.Name = "WindowsFormsApplicationBase" AndAlso
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
                MsgBox(ex.ToString)
            End Try

            publicMethods.Clear()
        End Sub

        Private Sub ProcessInstructions(body As MethodBody)
            Dim instructions = body.Instructions
            Dim il = body.GetILProcessor()
            Dim instructionsToExpand As List(Of Instruction) = New List(Of Instruction)()

            For Each instruction As Instruction In instructions
                Select Case instruction.OpCode
                    Case OpCodes.Ldc_I4
                        If Not Context.Randomizer.invisibleChars.Contains(instruction.Operand) AndAlso Utils.IsValidIntegerOperand(body, instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                    Case OpCodes.Ldc_I8
                        If Utils.IsValidLongOperand(body, instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                    Case OpCodes.Ldc_R4
                        If Utils.IsValidSingleOperand(body, instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                    Case OpCodes.Ldc_R8
                        If Utils.IsValidDoubleOperand(body, instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                    Case OpCodes.Ldstr
                        If Utils.IsValidStringOperand(body, instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                    Case OpCodes.Newobj
                        If Utils.IsValidNewObjOperand(instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                End Select
            Next

            For Each instruction As Instruction In instructionsToExpand
                Select Case instruction.OpCode
                    Case OpCodes.Ldc_I4
                        ReplaceInstruction(il, MdByInteger.GetValue(body.Method, instruction.Operand, Context.Randomizer), instruction)
                    Case OpCodes.Ldc_I8
                        ReplaceInstruction(il, MdByLong.GetValue(body.Method, instruction.Operand, Context.Randomizer), instruction)
                    Case OpCodes.Ldc_R4
                        ReplaceInstruction(il, MdBySingle.GetValue(body.Method, instruction.Operand, Context.Randomizer), instruction)
                    Case OpCodes.Ldc_R8
                        ReplaceInstruction(il, MdByDouble.GetValue(body.Method, instruction.Operand, Context.Randomizer), instruction)
                    Case OpCodes.Ldstr
                        ReplaceInstruction(il, MdByString.GetValue(body.Method, instruction.Operand, Context.Randomizer), instruction)
                    Case OpCodes.Newobj
                        Dim Mref = DirectCast(instruction.Operand, MethodReference)
                        Dim mdFinal = CreateReferenceMethod(Mref, body.Method)
                        ReplaceInstruction(il, mdFinal, instruction)
                End Select
            Next
        End Sub

        Private Sub ReplaceInstruction(il As ILProcessor, mdfinal As MethodDefinition, instruction As Instruction)
            If (Not mdfinal Is Nothing) Then
                Dim Instruct = il.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(mdfinal))
                il.Replace(instruction, Instruct)
                CompletedMethods.Add(mdfinal)
            End If
        End Sub

        Private Sub ReplaceInstruction(il As ILProcessor, mdfinal As Tuple(Of MethodDefinition, Integer), instruction As Instruction)
            If (Not mdfinal Is Nothing) Then
                Dim Instruct = il.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(mdfinal.Item1))
                il.Replace(instruction, Instruct)
                Dim i4Instruct = Instruction.Create(OpCodes.Ldc_I4, mdfinal.Item2)
                il.InsertBefore(Instruct, i4Instruct)
                CompletedMethods.Add(mdfinal.Item1)
            End If
        End Sub

        Private Function CreateReferenceMethod(targetConstructor As MethodReference, md As MethodDefinition) As MethodDefinition
            If (targetConstructor.Parameters.Count <> 0) OrElse targetConstructor.DeclaringType.IsGenericInstance OrElse targetConstructor.HasGenericParameters Then
                Return Nothing
            End If

            Dim item As MethodDefinition = Nothing

            If MdByRef.TryGetValue(targetConstructor, item) Then
            Else
                item = New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), Context.InputAssembly.MainModule.Import(targetConstructor.DeclaringType))
                item.Body = New MethodBody(item)
                item.IsPublic = True
                Dim ilProc As ILProcessor = item.Body.GetILProcessor()
                With ilProc
                    .Body.MaxStackSize = 1
                    .Body.InitLocals = True
                    .Emit(OpCodes.Newobj, targetConstructor)
                    .Emit(OpCodes.Ret)
                End With
                md.DeclaringType.Methods.Add(item)

                MdByRef.Add(targetConstructor, item)
            End If

            Return item
        End Function
#End Region

    End Class
End Namespace
