Imports Mono.Cecil

Namespace CecilHelper
    ''' <summary>
    ''' by Mono.Linker
    ''' </summary>
    Public Class TypeExtensions

        Public Shared Function GetInflatedBaseType(type As TypeReference) As TypeReference
            If type Is Nothing Then Return Nothing
            If type.IsGenericParameter OrElse type.IsByReference OrElse type.IsPointer Then Return Nothing
            Dim sentinelType As SentinelType = Nothing
            If Utils.InlineAssignHelper(sentinelType, TryCast(type, SentinelType)) IsNot Nothing Then Return GetInflatedBaseType(sentinelType.ElementType)
            Dim pinnedType As PinnedType = Nothing
            If Utils.InlineAssignHelper(pinnedType, TryCast(type, PinnedType)) IsNot Nothing Then Return GetInflatedBaseType(pinnedType.ElementType)
            Dim requiredModifierType As RequiredModifierType = Nothing
            If Utils.InlineAssignHelper(requiredModifierType, TryCast(type, RequiredModifierType)) IsNot Nothing Then Return GetInflatedBaseType(requiredModifierType.ElementType)
            Dim genericInstance As GenericInstanceType = Nothing

            If Utils.InlineAssignHelper(genericInstance, TryCast(type, GenericInstanceType)) IsNot Nothing Then
                Dim baseType = type.Resolve()?.BaseType
                If TypeOf baseType Is GenericInstanceType Then Return InflateGenericType(genericInstance, baseType)
                Return baseType
            End If

            Return type.Resolve()?.BaseType
        End Function

        Public Shared Iterator Function GetInflatedInterfaces(typeRef As TypeReference) As IEnumerable(Of TypeReference)
            Dim typeDef = typeRef.Resolve()
            If typeDef?.HasInterfaces <> True Then Return
            Dim genericInstance As GenericInstanceType = Nothing

            If Utils.InlineAssignHelper(genericInstance, TryCast(typeRef, GenericInstanceType)) IsNot Nothing Then

                For Each interfaceImpl In typeDef.Interfaces
                    Yield InflateGenericType(genericInstance, interfaceImpl)
                Next
            Else

                For Each interfaceImpl In typeDef.Interfaces
                    Yield interfaceImpl
                Next
            End If
        End Function

        Public Shared Function InflateGenericType(genericInstanceProvider As GenericInstanceType, typeToInflate As TypeReference) As TypeReference
            Dim arrayType As ArrayType = Nothing

            If Utils.InlineAssignHelper(arrayType, TryCast(typeToInflate, ArrayType)) IsNot Nothing Then
                Dim inflatedElementType = InflateGenericType(genericInstanceProvider, arrayType.ElementType)
                If inflatedElementType IsNot arrayType.ElementType Then Return New ArrayType(inflatedElementType, arrayType.Rank)
                Return arrayType
            End If

            Dim genericInst As GenericInstanceType = Nothing
            If Utils.InlineAssignHelper(genericInst, TryCast(typeToInflate, GenericInstanceType)) IsNot Nothing Then Return MakeGenericType(genericInstanceProvider, genericInst)
            Dim genericParameter As GenericParameter = Nothing

            If Utils.InlineAssignHelper(genericParameter, TryCast(typeToInflate, GenericParameter)) IsNot Nothing Then
                If TypeOf genericParameter.Owner Is MethodReference Then Return genericParameter
                Dim elementType = genericInstanceProvider.ElementType.Resolve()
                Dim parameter = elementType.GenericParameters.Single(Function(p) p Is genericParameter)
                Return genericInstanceProvider.GenericArguments(parameter.Position)
            End If

            Dim functionPointerType As FunctionPointerType = Nothing

            If Utils.InlineAssignHelper(functionPointerType, TryCast(typeToInflate, FunctionPointerType)) IsNot Nothing Then
                Dim result = New FunctionPointerType With {
                    .ReturnType = InflateGenericType(genericInstanceProvider, functionPointerType.ReturnType)
                }

                For i As Integer = 0 To functionPointerType.Parameters.Count - 1
                    Dim inflatedParameterType = InflateGenericType(genericInstanceProvider, functionPointerType.Parameters(i).ParameterType)
                    result.Parameters.Add(New ParameterDefinition(inflatedParameterType))
                Next

                Return result
            End If

            Dim modifierType As IModifierType = Nothing

            If Utils.InlineAssignHelper(modifierType, TryCast(typeToInflate, IModifierType)) IsNot Nothing Then
                Dim modifier = InflateGenericType(genericInstanceProvider, modifierType.ModifierType)
                Dim elementType = InflateGenericType(genericInstanceProvider, modifierType.ElementType)

                If TypeOf modifierType Is OptionalModifierType Then
                    Return New OptionalModifierType(modifier, elementType)
                End If

                Return New RequiredModifierType(modifier, elementType)
            End If

            Dim pinnedType As PinnedType = Nothing

            If Utils.InlineAssignHelper(pinnedType, TryCast(typeToInflate, PinnedType)) IsNot Nothing Then
                Dim elementType = InflateGenericType(genericInstanceProvider, pinnedType.ElementType)
                If elementType IsNot pinnedType.ElementType Then Return New PinnedType(elementType)
                Return pinnedType
            End If

            Dim pointerType As PointerType = Nothing

            If Utils.InlineAssignHelper(pointerType, TryCast(typeToInflate, PointerType)) IsNot Nothing Then
                Dim elementType = InflateGenericType(genericInstanceProvider, pointerType.ElementType)
                If elementType IsNot pointerType.ElementType Then Return New PointerType(elementType)
                Return pointerType
            End If

            Dim byReferenceType As ByReferenceType = Nothing

            If Utils.InlineAssignHelper(byReferenceType, TryCast(typeToInflate, ByReferenceType)) IsNot Nothing Then
                Dim elementType = InflateGenericType(genericInstanceProvider, byReferenceType.ElementType)
                If elementType IsNot byReferenceType.ElementType Then Return New ByReferenceType(elementType)
                Return byReferenceType
            End If

            Dim sentinelType As SentinelType = Nothing

            If Utils.InlineAssignHelper(sentinelType, TryCast(typeToInflate, SentinelType)) IsNot Nothing Then
                Dim elementType = InflateGenericType(genericInstanceProvider, sentinelType.ElementType)
                If elementType IsNot sentinelType.ElementType Then Return New SentinelType(elementType)
                Return sentinelType
            End If

            Return typeToInflate
        End Function

        Public Shared Function MakeGenericType(genericInstanceProvider As GenericInstanceType, type As GenericInstanceType) As GenericInstanceType
            Dim result = New GenericInstanceType(type.ElementType)

            For i = 0 To type.GenericArguments.Count - 1
                result.GenericArguments.Add(InflateGenericType(genericInstanceProvider, type.GenericArguments(i)))
            Next

            Return result
        End Function

        Public Shared Function MakeGenericType(self As TypeReference, ParamArray arguments As TypeReference()) As TypeReference
            If self.GenericParameters.Count <> arguments.Length Then Return Nothing
            Dim instance = New GenericInstanceType(self)

            For Each argument In arguments
                instance.GenericArguments.Add(argument)
            Next

            Return instance
        End Function

        Public Shared Iterator Function GetMethods(type As TypeReference) As IEnumerable(Of MethodReference)
            Dim typeDef = type.Resolve()
            If typeDef?.HasMethods <> True Then Return
            Dim genericInstanceType As GenericInstanceType = Nothing

            If Utils.InlineAssignHelper(genericInstanceType, TryCast(type, GenericInstanceType)) IsNot Nothing Then
                For Each methodDef In typeDef.Methods
                    Yield MakeMethodReferenceForGenericInstanceType(genericInstanceType, methodDef)
                Next
            Else

                For Each method In typeDef.Methods
                    Yield method
                Next
            End If
        End Function

        Private Shared Function MakeMethodReferenceForGenericInstanceType(genericInstanceType As GenericInstanceType, methodDef As MethodDefinition) As MethodReference
            Dim method = New MethodReference(methodDef.Name, methodDef.ReturnType, genericInstanceType) With {
                .HasThis = methodDef.HasThis,
                .ExplicitThis = methodDef.ExplicitThis,
                .CallingConvention = methodDef.CallingConvention
            }

            For Each parameter In methodDef.Parameters
                method.Parameters.Add(New ParameterDefinition(parameter.Name, parameter.Attributes, parameter.ParameterType))
            Next

            For Each gp In methodDef.GenericParameters
                method.GenericParameters.Add(New GenericParameter(gp.Name, method))
            Next
            Return method
        End Function

        Public Shared Function TypeMatches(tdef As TypeDefinition, type As TypeReference) As Boolean
            Dim instanceType = TryCast(type, GenericInstanceType)
            If instanceType Is Nothing Then type.DeclaringType = type.DeclaringType
            Dim typefullname As String = type.ToString()
            If instanceType IsNot Nothing Then typefullname = instanceType.ElementType.ToString()
            Return typefullname = tdef.FullName
        End Function

        Public Shared Function HasCustomAttributeByMemberName(member As TypeDefinition, CaName$) As Boolean
            If Not member.HasCustomAttributes Then Return False
            Return member.CustomAttributes.Any(Function(f) f.AttributeType.Name = CaName)
        End Function
    End Class
End Namespace
