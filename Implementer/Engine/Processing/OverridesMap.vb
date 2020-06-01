Imports Helper.CecilHelper
Imports Mono.Cecil

Namespace Engine.Processing
    ''' <summary>
    ''' by Mono.Linker
    ''' </summary>
    Public Class OverridesMap
        Inherits Dictionary(Of MethodDefinition, List(Of MethodDefinition))

        Public Sub RemoveRoot(mtd As MethodDefinition)
            Me.Remove(mtd)
        End Sub

        Public Sub AnalyzeTree(type As TypeDefinition)
            MapVirtualMethods(type)
            MapInterfaceMethodsInTypeHierarchy(type)
        End Sub

        Private Sub MapVirtualMethods(type As TypeDefinition)
            If Not type.HasMethods Then Return
            For Each method As MethodDefinition In type.Methods
                If Not method.IsVirtual Then Continue For
                MapVirtualMethod(method)
                If method.HasOverrides Then
                    MapOverrides(method)
                End If
            Next
        End Sub

        Private Sub MapVirtualMethod(method As MethodDefinition)
            MapVirtualBaseMethod(method)
            MapVirtualInterfaceMethod(method)
        End Sub

        Private Sub MapVirtualBaseMethod(method As MethodDefinition)
            If MemberExtensions.HasCustomAttributeByMemberName(method, "CompilerGeneratedAttribute") Then
                AnnotateMethods(method, Nothing)
            Else
                Dim base As MethodDefinition = GetBaseMethodInTypeHierarchy(method)
                If base Is Nothing Then Return
                AnnotateMethods(base, method)
            End If
        End Sub

        Private Sub MapVirtualInterfaceMethod(method As MethodDefinition)
            For Each base As MethodDefinition In GetBaseMethodsInInterfaceHierarchy(method)
                AnnotateMethods(base, method)
            Next
        End Sub

        Private Sub MapOverrides(method As MethodDefinition)
            For Each override_ref As MethodReference In method.Overrides
                Dim override As MethodDefinition = override_ref.Resolve()
                If override Is Nothing Then Continue For
                AnnotateMethods(override, method)
            Next
        End Sub

        Private Sub AnnotateMethods(base As MethodDefinition, override As MethodDefinition)
            AddOverride(base, override)
        End Sub

        Public Sub AddOverride(base As MethodDefinition, override As MethodDefinition)
            Dim methods As List(Of MethodDefinition) = Nothing

            If Not Me.TryGetValue(base, methods) Then
                methods = New List(Of MethodDefinition)
                If Not override Is Nothing Then
                    methods.Add(override)
                End If
                Add(base, methods)
            Else
                Me(base).Add(override)
            End If
        End Sub

        Private Shared Function GetBaseMethodInTypeHierarchy(method As MethodDefinition) As MethodDefinition
            Return GetBaseMethodInTypeHierarchy(method.DeclaringType, method)
        End Function

        Private Shared Function GetBaseInflatedInterfaceMethodInTypeHierarchy(context As Inflater.GenericContext, type As TypeDefinition, interfaceMethod As MethodDefinition) As MethodDefinition
            Dim base As TypeReference = TypeExtensions.GetInflatedBaseType(type)
            While base IsNot Nothing
                Dim candidate = CreateGenericInstanceCandidate(context, base.Resolve(), interfaceMethod)
                Dim base_method As MethodDefinition = TryMatchMethod(base, candidate)
                If base_method IsNot Nothing Then Return base_method
                base = TypeExtensions.GetInflatedBaseType(base)
            End While

            Return Nothing
        End Function

        Private Shared Function GetBaseMethodsInInterfaceHierarchy(method As MethodDefinition) As IEnumerable(Of MethodDefinition)
            Return GetBaseMethodsInInterfaceHierarchy(method.DeclaringType, method)
        End Function

        Private Shared Iterator Function GetBaseMethodsInInterfaceHierarchy(type As TypeReference, method As MethodDefinition) As IEnumerable(Of MethodDefinition)
            For Each Interf As TypeReference In TypeExtensions.GetInflatedInterfaces(type)
                Dim base_method As MethodDefinition = TryMatchMethod(Interf, method)
                If base_method IsNot Nothing Then Yield base_method

                For Each base As MethodDefinition In GetBaseMethodsInInterfaceHierarchy(Interf, method)
                    Yield base
                Next
            Next
        End Function

        Private Sub MapInterfaceMethodsInTypeHierarchy(type As TypeDefinition)
            If Not type.HasInterfaces Then Return
            Dim genericInterfaceInstance As GenericInstanceType = Nothing

            For Each Interf In type.Interfaces
                Dim interfType = Interf
                Dim iface = interfType.Resolve()
                If iface Is Nothing OrElse Not iface.HasMethods Then Continue For

                For Each interfMethod As MethodDefinition In iface.Methods
                    If TryMatchMethod(type, interfMethod) IsNot Nothing Then Continue For
                    Dim base = GetBaseMethodInTypeHierarchy(type, interfMethod)
                    If base IsNot Nothing Then AnnotateMethods(interfMethod, base)

                    If Utils.InlineAssignHelper(genericInterfaceInstance, TryCast(interfType, GenericInstanceType)) IsNot Nothing Then
                        Dim genericContext = New Inflater.GenericContext(genericInterfaceInstance, Nothing)
                        Dim baseInflated = GetBaseInflatedInterfaceMethodInTypeHierarchy(genericContext, type, interfMethod)
                        If baseInflated IsNot Nothing Then AddOverride(interfMethod, baseInflated)
                    End If
                Next
            Next
        End Sub

        Private Shared Function GetBaseMethodInTypeHierarchy(type As TypeDefinition, method As MethodDefinition) As MethodDefinition
            Dim base As TypeReference = TypeExtensions.GetInflatedBaseType(type)
            While base IsNot Nothing
                Dim base_method As MethodDefinition = TryMatchMethod(base, method)
                If base_method IsNot Nothing Then Return base_method
                base = TypeExtensions.GetInflatedBaseType(base)
            End While
            Return Nothing
        End Function

        Private Shared Function CreateGenericInstanceCandidate(context As Inflater.GenericContext, candidateType As TypeDefinition, interfaceMethod As MethodDefinition) As MethodReference
            Dim methodReference = New MethodReference(interfaceMethod.Name, interfaceMethod.ReturnType, candidateType) With {.HasThis = interfaceMethod.HasThis}

            For Each genericMethodParameter In interfaceMethod.GenericParameters
                methodReference.GenericParameters.Add(New GenericParameter(genericMethodParameter.Name, methodReference))
            Next

            If interfaceMethod.ReturnType.IsGenericParameter OrElse interfaceMethod.ReturnType.IsGenericInstance Then methodReference.ReturnType = Inflater.InflateType(context, interfaceMethod.ReturnType)

            For Each p In interfaceMethod.Parameters
                Dim parameterType = p.ParameterType
                If parameterType.IsGenericParameter OrElse parameterType.IsGenericInstance Then parameterType = Inflater.InflateType(context, parameterType)
                methodReference.Parameters.Add(New ParameterDefinition(p.Name, p.Attributes, parameterType))
            Next
            Return methodReference
        End Function

        Private Shared Function TryMatchMethod(type As TypeReference, method As MethodReference) As MethodDefinition
            Dim gen As New Dictionary(Of String, String)
            For Each candidate In TypeExtensions.GetMethods(type)
                If MethodMatch(candidate, method, gen) Then
                    Return candidate.Resolve()
                End If
                If Not gen Is Nothing Then gen.Clear()
            Next
            Return Nothing
        End Function

        Public Shared Function MethodMatch(candidate As MethodReference, method As MethodReference, ByRef Gen As Dictionary(Of String, String)) As Boolean
            Dim candidateDef = candidate.Resolve()
            If Not candidateDef.IsVirtual Then Return False
            If candidate.HasParameters <> method.HasParameters Then Return False
            If candidate.Name <> method.Name Then Return False
            If candidate.HasGenericParameters <> method.HasGenericParameters Then Return False
            If Not TypeMatch(GetReturnType(candidate), GetReturnType(method), Gen) Then Return False
            If Not candidate.HasParameters Then Return True
            Dim cp = candidate.Parameters
            Dim mp = method.Parameters
            If cp.Count <> mp.Count Then Return False
            If candidate.GenericParameters.Count <> method.GenericParameters.Count Then Return False
            For i As Integer = 0 To cp.Count - 1
                If Not TypeMatch(GetParameterType(method, i), GetParameterType(candidate, i), Gen) Then Return False
            Next
            Return True
        End Function

        Private Shared Function TypeMatch(a As IModifierType, b As IModifierType, ByRef Gen As Dictionary(Of String, String)) As Boolean
            If Not TypeMatch(a.ModifierType, b.ModifierType, Gen) Then Return False
            Return TypeMatch(a.ElementType, b.ElementType, Gen)
        End Function

        Private Shared Function TypeMatch(a As TypeSpecification, b As TypeSpecification, ByRef Gen As Dictionary(Of String, String)) As Boolean
            Dim gita As GenericInstanceType = Nothing
            If Utils.InlineAssignHelper(gita, TryCast(a, GenericInstanceType)) IsNot Nothing Then Return TypeMatch(gita, CType(b, GenericInstanceType), Gen)
            Dim mta As IModifierType = Nothing
            If Utils.InlineAssignHelper(mta, TryCast(a, IModifierType)) IsNot Nothing Then Return TypeMatch(mta, CType(b, IModifierType), Gen)
            Return TypeMatch(a.ElementType, b.ElementType, Gen)
        End Function

        Private Shared Function TypeMatch(a As GenericInstanceType, b As GenericInstanceType, ByRef Gen As Dictionary(Of String, String)) As Boolean
            If Not TypeMatch(a.ElementType, b.ElementType, Gen) Then Return False
            If a.HasGenericArguments <> b.HasGenericArguments Then Return False
            If Not a.HasGenericArguments Then Return True
            Dim gaa = a.GenericArguments
            Dim gab = b.GenericArguments
            If gaa.Count <> gab.Count Then Return False
            For i As Integer = 0 To gaa.Count - 1
                If Not TypeMatch(gaa(i), gab(i), Gen) Then Return False
            Next
            Return True
        End Function

        Public Shared Function TypeMatch(a As TypeReference, b As TypeReference, ByRef gp As Dictionary(Of String, String)) As Boolean
            Dim gpa = TryCast(a, GenericParameter)

            If gpa IsNot Nothing Then
                If gp Is Nothing Then gp = New Dictionary(Of String, String)()
                Dim match As String = Nothing
                If Not gp.TryGetValue(gpa.FullName, match) Then
                    gp.Add(gpa.FullName, b.ToString())
                    Return True
                End If
                Return match = b.ToString()
            End If

            If TypeOf a Is TypeSpecification OrElse TypeOf b Is TypeSpecification Then
                If a.GetType() <> b.GetType() Then Return False
                Return TypeMatch(CType(a, TypeSpecification), CType(b, TypeSpecification), gp)
            End If

            Return a.FullName = b.FullName
        End Function

        Private Shared Function GetReturnType(method As MethodReference) As TypeReference
            Dim genericInstance As GenericInstanceType = Nothing
            If Utils.InlineAssignHelper(genericInstance, TryCast(method.DeclaringType, GenericInstanceType)) IsNot Nothing Then Return TypeExtensions.InflateGenericType(genericInstance, method.ReturnType)
            Return method.ReturnType
        End Function

        Private Shared Function GetParameterType(method As MethodReference, parameterIndex As Integer) As TypeReference
            Dim genericInstance As GenericInstanceType = Nothing
            If Utils.InlineAssignHelper(genericInstance, TryCast(method.DeclaringType, GenericInstanceType)) IsNot Nothing Then Return TypeExtensions.InflateGenericType(genericInstance, method.Parameters(parameterIndex).ParameterType)
            Return method.Parameters(parameterIndex).ParameterType
        End Function

    End Class
End Namespace