Imports Mono.Cecil.Cil

Namespace CecilHelper
    Public Class InstructionGroup
        Inherits List(Of Instruction)

        Public ID As Integer
        Public nextGroup As Integer
    End Class
End Namespace
