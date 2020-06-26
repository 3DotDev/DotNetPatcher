Imports Mono.Cecil
Imports Helper.CecilHelper
Imports Mono.Cecil.Rocks
Imports Mono.Cecil.Cil

Namespace Core.Obfuscation.Protections

    Public NotInheritable Class ControlFlow
        Inherits Protection

#Region " Fields "
        Private ReadOnly m_Types As List(Of TypeDefinition)
        Private ReadOnly PackerState As Boolean
#End Region

#Region " Properties "
        Public Overrides ReadOnly Property ProgressMessage As String
            Get
                Return If(PackerState, "Packing", "Obfuscating") & " (ControlFlow...)"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "ControlFlow"
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
                Return 90
            End Get
        End Property

        Public Overrides ReadOnly Property Enabled As Boolean
#End Region

#Region " Constructor "
        Public Sub New(Contex As ProtectionContext, Optional ByVal PackerStat As Boolean = False)
            MyBase.New()
            Context = Contex
            If Contex.Params.TaskAccept.Obfuscation.Enabled AndAlso Contex.Params.TaskAccept.Obfuscation.ControlFlow Then
                _Enabled = True
                PackerState = PackerStat
                m_Types = New List(Of TypeDefinition)
            End If
        End Sub
#End Region

#Region " Methods "
        Public Overrides Sub Execute()
            For Each m As ModuleDefinition In Context.InputAssembly.Modules
                m_Types.AddRange(m.GetAllTypes())
                For Each type As TypeDefinition In m_Types
                    IterateType(type)
                Next
                m_Types.Clear()
            Next
        End Sub

        Private Sub IterateType(td As TypeDefinition)
            Dim publicMethods As New List(Of MethodDefinition)
            publicMethods.AddRange(From m In td.Methods Where (m.HasBody AndAlso m.Body.HasVariables AndAlso m.Body.Instructions.Count > 0 AndAlso m.Body.ExceptionHandlers.Count = 0 AndAlso
                                                            Not m.DeclaringType.BaseType Is Nothing AndAlso
                                                            Not m.IsConstructor AndAlso
                                                            Not Utils.HasUnsafeInstructions(m) AndAlso
                                                            Not m.Body.Instructions.Any(Function(f) f.OpCode = OpCodes.Throw) AndAlso 'CtrlFlow Stack calculation doesn't take charge Throw opcode like ExceptionHandlers !
                                                            Not m.Body.Instructions.Any(Function(f) f.OpCode = OpCodes.Rethrow) AndAlso
                                                            Not m.Body.Instructions.Any(Function(f) f.OpCode = OpCodes.Initobj))) 'CtrlFlow Stack calculation doesn't take charge Initobj opcode !

            Try
                For Each md In publicMethods
                    md.Body.SimplifyMacros
                    ProcessInstructions(md.Body)
                    md.Body.OptimizeMacros
                    md.Body.ComputeOffsets()
                    md.Body.ComputeHeader()
                Next
            Catch ex As Exception
            End Try
            publicMethods.Clear()
        End Sub

        Private Sub ProcessInstructions(body As MethodBody)
            Try
                Dim instructions = body.Instructions
                Dim il = body.GetILProcessor()

                Dim incGroups As New InstructionGroups
                Dim item1 As New InstructionGroup
                Dim incremId As Integer = 0
                Dim incremStackUsage As Integer = 0
                Dim flag As Boolean = False

                For Each instruction As Instruction In instructions
                    Dim Instruct = instruction
                    Dim stacks As Integer
                    Dim pops As Integer = 0
                    Msil.CalculateStackUsage(Instruct, stacks, pops)
                    item1.Add(Instruct)
                    incremStackUsage += (stacks - pops)
                    If (((stacks = 0) AndAlso (Not Instruct.OpCode = OpCodes.Nop)) AndAlso ((incremStackUsage = 0) OrElse (Instruct.OpCode = OpCodes.Ret))) Then
                        If Not flag Then
                            Dim item2 As New InstructionGroup With {
                                .ID = incremId
                            }
                            incremId += 1
                            item2.nextGroup = (item2.ID + 1)
                            incGroups.Add(item2)
                            item2 = New InstructionGroup With {
                                .ID = incremId
                            }
                            incremId += 1
                            item2.nextGroup = (item2.ID + 1)
                            incGroups.Add(item2)
                            flag = True
                        End If
                        item1.ID = incremId
                        incremId += 1
                        item1.nextGroup = (item1.ID + 1)
                        incGroups.Add(item1)
                        item1 = New InstructionGroup
                    End If
                Next
                If (incGroups.Count <> 1) Then
                    Dim item3 As InstructionGroup = incGroups.GetLast
                    incGroups.Scramble(incGroups)
                    body.Instructions.Clear()
                    Dim local As New VariableDefinition(Context.InputAssembly.MainModule.Import(GetType(Integer)))
                    body.Variables.Add(local)
                    Dim target As Instruction = Instruction.Create(OpCodes.Nop)
                    Dim instruction3 As Instruction = Instruction.Create(OpCodes.Br, target)
                    body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4_0))
                    body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local))
                    body.Instructions.Add(Instruction.Create(OpCodes.Br, instruction3))
                    body.Instructions.Add(target)
                    Dim group4 As InstructionGroup
                    For Each group4 In incGroups
                        If (Not group4 Is item3) Then
                            body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local))
                            body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, group4.ID))
                            body.Instructions.Add(Instruction.Create(OpCodes.Ceq))
                            Dim instruction4 As Instruction = Instruction.Create(OpCodes.Nop)
                            body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction4))
                            Dim instruction5 As Instruction
                            For Each instruction5 In group4
                                body.Instructions.Add(instruction5)
                            Next
                            body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, group4.nextGroup))
                            body.Instructions.Add(Instruction.Create(OpCodes.Stloc, local))
                            body.Instructions.Add(instruction4)
                        End If
                    Next
                    body.Instructions.Add(Instruction.Create(OpCodes.Ldloc, local))
                    body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, incGroups.Count - 1))
                    body.Instructions.Add(Instruction.Create(OpCodes.Ceq))
                    body.Instructions.Add(Instruction.Create(OpCodes.Brfalse, instruction3))
                    body.Instructions.Add(Instruction.Create(OpCodes.Br, item3.Item(0)))
                    body.Instructions.Add(instruction3)
                    Dim instruction6 As Instruction
                    For Each instruction6 In item3
                        body.Instructions.Add(instruction6)
                    Next
                    'Setting Initlocals to True because of adding new variables !
                    body.InitLocals = True
                End If
            Catch ex As Exception
                MsgBox(body.Method.FullName & " :" & vbNewLine & ex.ToString)
            End Try
        End Sub
#End Region

    End Class
End Namespace
