Imports Mono.Cecil

Namespace CecilHelper
    Public Class AssemblyExtensions

        Public Shared Function HasCustomAttributeByAssemblyName(member As AssemblyDefinition, CaName$) As Boolean
            If Not member.HasCustomAttributes Then Return False
            Return member.CustomAttributes.Any(Function(f) f.AttributeType.Name = CaName)
        End Function

    End Class
End Namespace
