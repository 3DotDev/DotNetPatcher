Imports Mono.Cecil

Namespace CecilHelper
    Public Class MemberExtensions

        Public Shared Function HasCustomAttributeByMemberName(member As IMemberDefinition, CaName$) As Boolean
            If Not member.HasCustomAttributes Then Return False
            Return member.CustomAttributes.Any(Function(f) f.AttributeType.Name = CaName)
        End Function

        Public Shared Function MakeGeneric(self As MethodReference, ParamArray arguments As TypeReference()) As MethodReference
            Dim reference = New MethodReference(self.Name, self.ReturnType) With {
            .DeclaringType = TypeExtensions.MakeGenericType(self.DeclaringType, arguments),
            .HasThis = self.HasThis,
            .ExplicitThis = self.ExplicitThis,
            .CallingConvention = self.CallingConvention}
            For Each parameter In self.Parameters
                reference.Parameters.Add(New ParameterDefinition(parameter.ParameterType))
            Next

            For Each generic_parameter In self.GenericParameters
                reference.GenericParameters.Add(New GenericParameter(generic_parameter.Name, reference))
            Next
            Return reference
        End Function

        Public Shared Function MakeGeneric(method As MethodReference, genericarg As TypeReference) As MethodReference
            Dim genericTypeRef = New GenericInstanceMethod(method)
            genericTypeRef.GenericArguments.Add(genericarg)
            Return genericTypeRef
        End Function

    End Class
End Namespace
