Imports Mono.Cecil

Namespace CecilHelper
    ''' <summary>
    ''' by Mono.Linker
    ''' </summary>
    Public Class Inflater
        Public Shared Function InflateType(context As GenericContext, typeReference As TypeReference) As TypeReference
            Dim typeDefinition = InflateTypeWithoutException(context, typeReference)
            If typeDefinition Is Nothing Then Throw New InvalidOperationException($"Unable to resolve a reference to the type '{typeReference.FullName}' in the assembly '{typeReference.[Module].Assembly.FullName}'. Does this type exist in a different assembly in the project?")
            Return typeDefinition
        End Function

        Public Shared Function InflateType(context As GenericContext, typeDefinition As TypeDefinition) As GenericInstanceType
            Return ConstructGenericType(context, typeDefinition, typeDefinition.GenericParameters)
        End Function

        Public Shared Function InflateType(context As GenericContext, genericInstanceType As GenericInstanceType) As GenericInstanceType
            Dim inflatedType = ConstructGenericType(context, genericInstanceType.Resolve(), genericInstanceType.GenericArguments)
            inflatedType.MetadataToken = genericInstanceType.MetadataToken
            Return inflatedType
        End Function

        Public Shared Function InflateTypeWithoutException(context As GenericContext, typeReference As TypeReference) As TypeReference
            Dim genericParameter As GenericParameter = Nothing

            If Utils.InlineAssignHelper(genericParameter, TryCast(typeReference, GenericParameter)) IsNot Nothing Then
                If context.Method Is Nothing AndAlso Not genericParameter.Owner Is typeReference Then
                    Return typeReference
                End If
                Dim genericArgumentType = If(genericParameter.Owner Is typeReference, context.Type.GenericArguments(genericParameter.Position), context.Method.GenericArguments(genericParameter.Position))
                Dim inflatedType = genericArgumentType
                Return inflatedType
            End If

            Dim genericInstanceType As GenericInstanceType = Nothing
            If Utils.InlineAssignHelper(genericInstanceType, TryCast(typeReference, GenericInstanceType)) IsNot Nothing Then Return InflateType(context, genericInstanceType)
            Dim arrayType As ArrayType = Nothing
            If Utils.InlineAssignHelper(arrayType, TryCast(typeReference, ArrayType)) IsNot Nothing Then Return New ArrayType(InflateType(context, arrayType.ElementType), arrayType.Rank)
            Dim byReferenceType As ByReferenceType = Nothing
            If Utils.InlineAssignHelper(byReferenceType, TryCast(typeReference, ByReferenceType)) IsNot Nothing Then Return New ByReferenceType(InflateType(context, byReferenceType.ElementType))
            Dim pointerType As PointerType = Nothing
            If Utils.InlineAssignHelper(pointerType, TryCast(typeReference, PointerType)) IsNot Nothing Then Return New PointerType(InflateType(context, pointerType.ElementType))
            Dim reqModType As RequiredModifierType = Nothing
            If Utils.InlineAssignHelper(reqModType, TryCast(typeReference, RequiredModifierType)) IsNot Nothing Then Return InflateTypeWithoutException(context, reqModType.ElementType)
            Dim optModType As OptionalModifierType = Nothing
            If Utils.InlineAssignHelper(optModType, TryCast(typeReference, OptionalModifierType)) IsNot Nothing Then Return InflateTypeWithoutException(context, optModType.ElementType)
            Return typeReference.Resolve()
        End Function

        Private Shared Function ConstructGenericType(context As GenericContext, typeDefinition As TypeDefinition, genericArguments As IEnumerable(Of TypeReference)) As GenericInstanceType
            Dim inflatedType = New GenericInstanceType(typeDefinition)

            For Each genericArgument In genericArguments
                inflatedType.GenericArguments.Add(InflateType(context, genericArgument))
            Next

            Return inflatedType
        End Function

        Public Class GenericContext
            Public Sub New(type As GenericInstanceType, method As GenericInstanceMethod)
                Me.Type = type
                Me.Method = method
            End Sub

            Public ReadOnly Property Type As GenericInstanceType

            Public ReadOnly Property Method As GenericInstanceMethod
        End Class

    End Class
End Namespace
