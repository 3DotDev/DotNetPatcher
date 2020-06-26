Imports System.Runtime.InteropServices
Imports Mono.Cecil
Imports Mono.Cecil.Cil

Namespace CecilHelper
    Public Class Msil

#Region " Methods "
        Public Shared Sub CalculateStackUsage(inst As Instruction, <Out> ByRef pushes As Integer, <Out> ByRef pops As Integer)
            Dim hasReturnValue As Boolean = False
            Dim opCode As OpCode = inst.OpCode
            If opCode.FlowControl = FlowControl.Call Then
                If opCode.Code <> Code.Jmp Then
                    pushes = 0
                    pops = 0
                    Dim methodSig As IMethodSignature
                    Dim operand As Object = inst.Operand
                    Dim method As MethodReference = TryCast(operand, MethodReference)

                    If (Not method Is Nothing) Then
                        methodSig = method
                    Else
                        methodSig = TryCast(operand, IMethodSignature)
                    End If
                    If (Not methodSig Is Nothing) Then
                        Dim implicitThis As Boolean = methodSig.HasThis AndAlso Not methodSig.ExplicitThis
                        If (methodSig.ReturnType.MetadataType <> MetadataType.Void OrElse ((opCode.Code = Code.Newobj) AndAlso methodSig.HasThis)) Then
                            pushes += 1
                        End If
                        pops += methodSig.Parameters.Count

                        Dim paramsAfterSentinel As New List(Of TypeReference)
                        For Each p In methodSig.Parameters
                            If p.ParameterType.IsSentinel Then
                                paramsAfterSentinel.Add(p.ParameterType)
                            End If
                        Next
                        If (Not paramsAfterSentinel Is Nothing) Then
                            pops += paramsAfterSentinel.Count
                        End If
                        If (implicitThis AndAlso (opCode.Code <> Code.Newobj)) Then
                            pops += 1
                        End If
                        If (opCode.Code = Code.Calli) Then
                            pops += 1
                        End If
                    End If
                End If
            Else
                pushes = 0
                pops = 0
                Select Case opCode.StackBehaviourPush
                    Case StackBehaviour.Push1, StackBehaviour.Pushi, StackBehaviour.Pushi8, StackBehaviour.Pushr4, StackBehaviour.Pushr8, StackBehaviour.Pushref
                        pushes += 1
                        Exit Select
                    Case StackBehaviour.Push1_push1
                        pushes += 2
                        Exit Select
                End Select
                Select Case opCode.StackBehaviourPop
                    Case StackBehaviour.Pop1, StackBehaviour.Popi, StackBehaviour.Popref
                        pops += 1
                        Exit Select
                    Case StackBehaviour.Pop1_pop1, StackBehaviour.Popi_pop1, StackBehaviour.Popi_popi, StackBehaviour.Popi_popi8, StackBehaviour.Popi_popr4, StackBehaviour.Popi_popr8, StackBehaviour.Popref_pop1, StackBehaviour.Popref_popi
                        pops += 2
                        Exit Select
                    Case StackBehaviour.Popi_popi_popi, StackBehaviour.Popref_popi_popi, StackBehaviour.Popref_popi_popi8, StackBehaviour.Popref_popi_popr4, StackBehaviour.Popref_popi_popr8, StackBehaviour.Popref_popi_popref, StackBehaviour.Varpush
                        pops += 3
                        Exit Select
                    Case StackBehaviour.Varpop
                        If hasReturnValue Then
                            pops += 1
                        End If
                        Exit Select
                    Case StackBehaviour.PopAll
                        pops = -1
                        Exit Select
                End Select

            End If
        End Sub


#End Region

    End Class

End Namespace


