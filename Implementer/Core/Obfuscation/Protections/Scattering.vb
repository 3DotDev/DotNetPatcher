﻿Imports Mono.Cecil
Imports Helper.CecilHelper
Imports Mono.Cecil.Cil
Imports Mono.Cecil.Rocks
Imports Helper.RandomizeHelper

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class Scattering
        Inherits Protection

#Region " ¨Private Class "
        Private Class MethodsCollection(Of T)
            Inherits List(Of KeyValuePair(Of T, MethodDefinition))

            Public Overloads Sub Add(key As T, value As MethodDefinition)
                Add(New KeyValuePair(Of T, MethodDefinition)(key, value))
            End Sub

            Public Overloads Function GetValue(md As MethodDefinition, key As T, Randomizer As Randomizer) As MethodDefinition
                Dim mdFinal As MethodDefinition

                Dim allMethods = Where(Function(f) f.Value?.DeclaringType.FullName = md.DeclaringType.FullName)
                If allMethods.Count > 0 Then
                    If Any(Function(f) f.Value?.DeclaringType.FullName = md.DeclaringType.FullName AndAlso f.Key.Equals(key)) Then
                        mdFinal = Where(Function(f) f.Value?.DeclaringType.FullName = md.DeclaringType.FullName AndAlso f.Key.Equals(key)).First.Value
                    Else
                        mdFinal = CreateMethod(key, md, Randomizer)
                        AddToList(mdFinal, key)
                    End If
                Else
                    mdFinal = CreateMethod(key, md, Randomizer)
                    AddToList(mdFinal, key)
                End If
                Return mdFinal
            End Function

            Private Sub AddToList(mdFinal As MethodDefinition, key As T)
                If Not mdFinal Is Nothing Then
                    mdFinal.Attributes = MethodAttributes.Static Or MethodAttributes.Private
                    Add(New KeyValuePair(Of T, MethodDefinition)(key, mdFinal))
                End If
            End Sub

            Private Function CreateMethod(value As Object, md As MethodDefinition, Randomizer As Randomizer) As MethodDefinition
                Dim opc As OpCode = Nothing
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
                item.Body = New MethodBody(item)
                item.IsPublic = True
                Dim ilProc As ILProcessor = item.Body.GetILProcessor()
                With ilProc
                    .Body.MaxStackSize = 1
                    .Body.InitLocals = True
                    .Emit(opc, value)
                    .Emit(OpCodes.Ret)
                End With
                md.DeclaringType.Methods.Add(item)

                Return item
            End Function
        End Class
#End Region

#Region " Fields "
        Private ReadOnly MdByString As MethodsCollection(Of String)
        Private ReadOnly MdByInteger As MethodsCollection(Of Integer)
        Private ReadOnly MdByLong As MethodsCollection(Of Long)
        Private ReadOnly MdByDouble As MethodsCollection(Of Double)
        Private ReadOnly MdBySingle As MethodsCollection(Of Single)
        Private ReadOnly MdByByte As MethodsCollection(Of Byte)
        Private ReadOnly MdByRef As Dictionary(Of MethodReference, MethodDefinition)
        Private ReadOnly Types As List(Of TypeDefinition)
        Private ReadOnly OpCodeDic As Dictionary(Of OpCode, String)
        Private ReadOnly CompletedMethods As Mono.Collections.Generic.Collection(Of MethodDefinition)
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (Hide calls part 2...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Anti-Debug"
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
                MdByByte = New MethodsCollection(Of Byte)
                MdByRef = New Dictionary(Of MethodReference, MethodDefinition)
                Types = New List(Of TypeDefinition)
                OpCodeDic = New Dictionary(Of OpCode, String) From {{OpCodes.Ldc_I4, "System.Int32"}, {OpCodes.Ldc_R4, "System.Single"}, {OpCodes.Ldc_R8, "System.Double"}}
                CompletedMethods = New Mono.Collections.Generic.Collection(Of MethodDefinition)
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
            MdByByte.Clear()
            MdByInteger.Clear()
            MdByLong.Clear()
            MdByDouble.Clear()
            MdBySingle.Clear()
            MdByByte.Clear()
            MdByRef.Clear()
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

        Private Function ConvertToByteArray(inputElements As Integer()) As Byte()
            Dim myFinalBytes As Byte() = New Byte(inputElements.Length * 4 - 1) {}

            For cnt As Integer = 0 To inputElements.Length - 1
                Dim myBytes As Byte() = BitConverter.GetBytes(inputElements(cnt))
                Array.Copy(myBytes, 0, myFinalBytes, cnt * 4, 4)
            Next

            Return myFinalBytes
        End Function

        Private Function ConvertToInt32Array(inputElements As Byte()) As Integer()
            Dim myFinalIntegerArray As Integer() = New Integer(inputElements.Length / 4 - 1) {}

            For cnt As Integer = 0 To inputElements.Length - 1 Step 4
                myFinalIntegerArray(cnt / 4) = BitConverter.ToInt32(inputElements, cnt)
            Next

            Return myFinalIntegerArray
        End Function

        Private Sub ProcessInstructions(body As MethodBody)
            Dim instructions = body.Instructions
            Dim il = body.GetILProcessor()
            Dim instructionsToExpand As List(Of Instruction) = New List(Of Instruction)()

            For Each instruction As Instruction In instructions
                Select Case instruction.OpCode
                    Case OpCodes.Ldc_I4
                        If Utils.IsValidIntegerOperand(instruction) AndAlso Not Context.Randomizer.invisibleChars.Contains(CInt(instruction.Operand)) Then
                            OpCodesFilter(instruction, instructionsToExpand, body)
                        End If
                    Case OpCodes.Ldc_I8
                        If Utils.IsValidLongOperand(instruction) Then
                            instructionsToExpand.Add(instruction)
                        End If
                    Case OpCodes.Ldc_R4
                        If Utils.IsValidSingleOperand(instruction) Then
                            OpCodesFilter(instruction, instructionsToExpand, body)
                        End If
                    Case OpCodes.Ldc_R8
                        If Utils.IsValidDoubleOperand(instruction) Then
                            OpCodesFilter(instruction, instructionsToExpand, body)
                        End If
                    Case OpCodes.Ldstr
                        If Utils.IsValidStringOperand(instruction) Then
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
                        Dim Value As Integer = CInt(instruction.Operand)
                        Dim mdFinal As MethodDefinition = MdByInteger.GetValue(body.Method, Value, Context.Randomizer)
                        ReplaceInstruction(il, mdFinal, instruction)
                    Case OpCodes.Ldc_I8
                        Dim Value As Long = CLng(instruction.Operand)
                        Dim mdFinal As MethodDefinition = MdByLong.GetValue(body.Method, Value, Context.Randomizer)
                        ReplaceInstruction(il, mdFinal, instruction)
                    Case OpCodes.Ldc_R4
                        Dim Value As Single = CSng(instruction.Operand)
                        Dim mdFinal As MethodDefinition = MdBySingle.GetValue(body.Method, Value, Context.Randomizer)
                        ReplaceInstruction(il, mdFinal, instruction)
                    Case OpCodes.Ldc_R8
                        Dim Value As Double = CDbl(instruction.Operand)
                        Dim mdFinal As MethodDefinition = MdByDouble.GetValue(body.Method, Value, Context.Randomizer)
                        ReplaceInstruction(il, mdFinal, instruction)
                    Case OpCodes.Ldstr
                        Dim Value As String = CStr(instruction.Operand)
                        Dim mdFinal As MethodDefinition = MdByString.GetValue(body.Method, Value, Context.Randomizer)
                        ReplaceInstruction(il, mdFinal, instruction)
                    Case OpCodes.Newobj
                        Dim Mref = DirectCast(instruction.Operand, MethodReference)
                        Dim mdFinal As MethodDefinition

                        If MdByRef.ContainsKey(Mref) Then
                            mdFinal = MdByRef.Item(Mref)
                        Else
                            mdFinal = CreateReferenceMethod(Mref, body.Method)
                            If Not mdFinal Is Nothing Then
                                MdByRef.Add(Mref, mdFinal)
                            End If
                        End If
                        ReplaceInstruction(il, mdFinal, instruction)
                End Select
            Next
        End Sub

        Private Sub ReplaceInstruction(il As ILProcessor, mdFinal As MethodDefinition, instruction As Instruction)
            If (Not mdFinal Is Nothing) Then
                Dim Instruct = il.Create(OpCodes.Call, Context.InputAssembly.MainModule.Import(mdFinal))
                il.Replace(instruction, Instruct)
                CompletedMethods.Add(mdFinal)
            End If
        End Sub

        Private Sub OpCodesFilter(Instruction As Instruction, instructionsToExpand As List(Of Instruction), body As MethodBody)
            Dim opCodeStr = OpCodeDic.Item(Instruction.OpCode)

            Dim instructNext = Instruction.Next
            If instructNext.OpCode = OpCodes.Stloc OrElse instructNext.OpCode = OpCodes.Ldloc Then
                Dim varIndex = CInt(instructNext.Operand.ToString.ToLower.Replace("v_", String.Empty))
                Dim varType = body.Variables(varIndex).VariableType
                If varType.ToString = opCodeStr Then
                    If Not Instruction.Operand Is Nothing Then
                        instructionsToExpand.Add(Instruction)
                    End If
                End If
            Else
                If Not instructNext.Operand Is Nothing Then
                    If instructNext.Operand.ToString.ToLower.EndsWith(opCodeStr.ToLower & ")") OrElse instructNext.Operand.ToString.ToLower.StartsWith(opCodeStr.ToLower) Then
                        If Not Instruction.Operand Is Nothing Then
                            instructionsToExpand.Add(Instruction)
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function CreateReferenceMethod(targetConstructor As MethodReference, md As MethodDefinition) As MethodDefinition
            If (targetConstructor.Parameters.Count <> 0) OrElse targetConstructor.DeclaringType.IsGenericInstance OrElse targetConstructor.HasGenericParameters Then
                Return Nothing
            End If
            Dim item As New MethodDefinition(Context.Randomizer.GenerateNew, (MethodAttributes.CompilerControlled Or (MethodAttributes.FamANDAssem Or (MethodAttributes.Family Or MethodAttributes.Static))), Context.InputAssembly.MainModule.Import(targetConstructor.DeclaringType))
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

            Return item
        End Function
#End Region

    End Class
End Namespace
