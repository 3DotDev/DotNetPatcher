Imports Mono.Cecil
Imports Mono.Cecil.Cil
Imports Mono.Cecil.Rocks
Imports Mono.Collections.Generic

Namespace CecilHelper
    Public NotInheritable Class Utils

#Region " Methods "

        Public Shared Function HasUnsafeInstructions(member As MethodDefinition) As Boolean
            If member.HasBody Then
                If member.Body.HasVariables Then
                    For Each v In member.Body.Variables
                        If v.IsPinned Then
                            Return True
                        ElseIf v.VariableType.IsPointer Then
                            Return True
                        ElseIf v.VariableType.IsFunctionPointer Then
                            Return True
                        End If
                    Next
                End If
            End If
            Return False
        End Function

        Public Shared Function RemoveCustomAttributeByName(member As AssemblyDefinition, CaName$) As Boolean
            If member.HasCustomAttributes Then
                Dim caList = Enumerable.Where(member.CustomAttributes, Function(ca) ca.AttributeType.Name = CaName)
                Dim caCount = caList.Count
                If caCount <> 0 Then
                    Dim Finded = caList.First
                    If Not Finded Is Nothing Then
                        Return member.CustomAttributes.Remove(Finded)
                    End If
                End If
            End If
            Return False
        End Function

        Public Shared Function RemoveCustomAttributeByName(member As IMemberDefinition, CaName$) As Boolean
            If member.HasCustomAttributes Then
                Dim caList = Enumerable.Where(member.CustomAttributes, Function(ca) ca.AttributeType.Name = CaName)
                Dim caCount = caList.Count
                If caCount <> 0 Then
                    Dim Finded = caList.First
                    If Not Finded Is Nothing Then
                        Try
                            Return member.CustomAttributes.Remove(Finded)
                        Catch ex As Exception
                            Return False
                        End Try
                    End If
                End If
            End If
            Return False
        End Function

        Public Shared Function IsValidBoolOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso (CInt(instruct.Operand) = 0 OrElse CInt(instruct.Operand) = 1) Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValidOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValidIntegerOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Integer.Parse(instruct.Operand.ToString) = Nothing Then
                If CInt(instruct.Operand) > 7 Then
                    Return True
                End If
            End If
            Return False
        End Function

        Public Shared Function IsValidIntegerOperand(body As MethodBody, instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Integer.Parse(instruct.Operand.ToString) = Nothing Then
                If CInt(instruct.Operand) > 7 Then
                    Return IsValidInstructionReference(body, instruct)
                End If
            End If
            Return False
        End Function

        Private Shared Function IsValidInstructionReference(body As MethodBody, Instruct As Instruction) As Boolean
            For Each ins In body.Instructions
                If ins.OpCode = Cil.OpCodes.Switch Then
                    For Each Inst As Instruction In ins.Operand
                        If Inst Is Instruct Then
                            Return False
                        End If
                    Next
                Else
                    If ins.Operand IsNot Nothing AndAlso TypeOf ins.Operand Is Instruction AndAlso Instruct Is TryCast(ins.Operand, Instruction) AndAlso Instruct.Index = TryCast(ins.Operand, Instruction).Index Then
                        Return False
                    End If
                End If
            Next

            If body.HasExceptionHandlers Then
                For Each exc In body.ExceptionHandlers
                    If exc.TryStart.Index <= Instruct.Index AndAlso exc.TryEnd.Index >= Instruct.Index Then
                        Return False
                    End If
                Next
            End If

            Return True
        End Function

        Public Shared Function IsValidNewObjOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not DirectCast(instruct.Operand, MethodReference) Is Nothing Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValidNewObjOperand(body As MethodBody, instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not DirectCast(instruct.Operand, MethodReference) Is Nothing Then
                Return IsValidInstructionReference(body, instruct)
            End If
            Return False
        End Function

        Public Shared Function IsValidLongOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Long.Parse(instruct.Operand.ToString) = Nothing Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValidLongOperand(body As MethodBody, instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Long.Parse(instruct.Operand.ToString) = Nothing Then
                Return IsValidInstructionReference(body, instruct)
            End If
            Return False
        End Function

        Public Shared Function IsValidSingleOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Single.Parse(instruct.Operand.ToString) = Nothing Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValidSingleOperand(body As MethodBody, instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Single.Parse(instruct.Operand.ToString) = Nothing Then
                Return IsValidInstructionReference(body, instruct)
            End If
            Return False
        End Function

        Public Shared Function IsValidDoubleOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Double.Parse(instruct.Operand.ToString) = Nothing Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValidDoubleOperand(body As MethodBody, instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not Double.Parse(instruct.Operand.ToString) = Nothing Then
                Return IsValidInstructionReference(body, instruct)
            End If
            Return False
        End Function

        Public Shared Function IsValidStringOperand(instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not String.IsNullOrWhiteSpace(CStr(instruct.Operand)) AndAlso Not CStr(instruct.Operand).Length = 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function IsValidStringOperand(body As MethodBody, instruct As Instruction) As Boolean
            If Not instruct.Operand Is Nothing AndAlso Not String.IsNullOrWhiteSpace(CStr(instruct.Operand)) AndAlso Not CStr(instruct.Operand).Length = 0 Then
                Return IsValidInstructionReference(body, instruct)
            End If
            Return False
        End Function

        Public Shared Function IsValidPinvokeCallOperand(instruct As Instruction) As Boolean
            Dim originalReference As MethodReference
            Try
                originalReference = TryCast(instruct.Operand, MethodReference)
                If Not originalReference Is Nothing Then
                    Dim originalMethod As MethodDefinition = originalReference.Resolve
                    If Not originalMethod Is Nothing AndAlso Not originalMethod.DeclaringType Is Nothing Then
                        If (originalMethod.IsPInvokeImpl AndAlso originalMethod.HasParameters = False) OrElse (originalMethod.IsPInvokeImpl AndAlso originalMethod.HasParameters AndAlso originalMethod.Parameters.All(Function(m) m.HasFieldMarshal = False)) Then
                            Return True
                        End If
                    End If
                End If
            Catch ex As Exception
                Return False
            End Try
            Return False
        End Function

        Public Shared Sub AddHelpKeywordAttribute(Modul As ModuleDefinition, targetMember As ICustomAttributeProvider, browsable As TypeReference)
            Dim attribType = Modul.Import(GetType(ComponentModel.Design.HelpKeywordAttribute))
            Dim constructor = attribType.Resolve.GetConstructors().Single(Function(ctor) 1 = ctor.Parameters.Count AndAlso "System.Type" = ctor.Parameters(0).ParameterType.FullName)
            Dim constructorRef = Modul.Import(constructor)
            Dim attrib = New CustomAttribute(constructorRef)
            Dim browsableArg = New CustomAttributeArgument(Modul.Import(GetType(Type)), browsable)
            attrib.ConstructorArguments.Add(browsableArg)
            targetMember.CustomAttributes.Add(attrib)
        End Sub

        Public Shared Function IsStronglyTypedResourceBuilder(td As TypeDefinition) As Boolean
            If td.HasCustomAttributes = False Then Return False

            For Each ca In (From c In td.CustomAttributes
                            Where c IsNot Nothing AndAlso c.AttributeType.Name = "GeneratedCodeAttribute" AndAlso c.HasConstructorArguments AndAlso c.ConstructorArguments(0).Value = "System.Resources.Tools.StronglyTypedResourceBuilder"
                            Select c)
                Return True
            Next
            Return False
        End Function

        Public Shared Function IsSettingStr(md As MethodDefinition, str$) As Boolean
            If md.IsGetter Then
                Return md.Name.ToLower = "get_" & str.ToLower
            ElseIf md.IsSetter Then
                Return md.Name.ToLower = "set_" & str.ToLower
            End If
            Return False
        End Function

        Public Shared Function IsDebuggerNonUserCode(assDef As AssemblyDefinition) As Boolean
            Return assDef.MainModule.EntryPoint.DeclaringType.Namespace.EndsWith(".My")
        End Function

        Public Shared Function HasSerializableAttributes(path) As Boolean
            Dim ass As AssemblyDefinition = AssemblyDefinition.ReadAssembly(path)
            Dim HasAtt As Boolean = False
            For Each mo In ass.Modules
                If HasAtt Then Exit For
                For Each ty In mo.GetAllTypes
                    If ty.Attributes.HasFlag(TypeAttributes.Serializable) Then
                        HasAtt = True
                        Exit For
                    End If
                Next
            Next
            Return HasAtt
        End Function

        Public Shared Function HasINotifyPropertyChanged(prop As PropertyDefinition) As Boolean
            Dim baseType As TypeReference = prop.DeclaringType.BaseType
            While baseType IsNot Nothing
                Dim def As TypeDefinition = baseType.Resolve()
                If def IsNot Nothing Then
                    For Each inter In def.Interfaces
                        If inter.Name = "INotifyPropertyChanged" Then
                            Return True
                        End If
                    Next
                    baseType = def.BaseType
                Else
                    baseType = Nothing
                End If
            End While
            Return False
        End Function

        Public Shared Function AreSame(method As MethodReference, ByVal reference As MethodReference) As Boolean
            If method Is Nothing OrElse reference Is Nothing Then Return False
            If method.HasGenericParameters <> reference.HasGenericParameters Then Return False
            If method.HasGenericParameters AndAlso method.GenericParameters.Count <> reference.GenericParameters.Count Then Return False
            If Not AreSame(method.ReturnType, reference.ReturnType) Then Return False
            If method.HasParameters <> reference.HasParameters Then Return False
            If Not AreSame(method.Parameters, reference.Parameters) Then Return False
            Return True
        End Function

        Private Shared Function AreSame(a As Collection(Of ParameterDefinition), b As Collection(Of ParameterDefinition)) As Boolean
            Dim count = a.Count
            If count <> b.Count Then Return False
            If count = 0 Then Return True
            For i As Integer = 0 To count - 1
                If Not AreSame(a(i).ParameterType, b(i).ParameterType) Then Return False
            Next
            Return True
        End Function

        Private Shared Function AreSame(a As TypeSpecification, b As TypeSpecification) As Boolean
            If Not AreSame(a.ElementType, b.ElementType) Then Return False
            If a.IsGenericInstance Then Return AreSame(CType(a, GenericInstanceType), CType(b, GenericInstanceType))
            If a.IsRequiredModifier OrElse a.IsOptionalModifier Then Return AreSame(CType(a, IModifierType), CType(b, IModifierType))
            If a.IsArray Then Return AreSame(CType(a, ArrayType), CType(b, ArrayType))
            Return True
        End Function

        Private Shared Function AreSame(a As ArrayType, b As ArrayType) As Boolean
            If a.Rank <> b.Rank Then Return False
            Return True
        End Function

        Private Shared Function AreSame(a As IModifierType, b As IModifierType) As Boolean
            Return AreSame(a.ModifierType, b.ModifierType)
        End Function

        Private Shared Function AreSame(a As GenericInstanceType, b As GenericInstanceType) As Boolean
            If a.GenericArguments.Count <> b.GenericArguments.Count Then Return False
            For i As Integer = 0 To a.GenericArguments.Count - 1
                If Not AreSame(a.GenericArguments(i), b.GenericArguments(i)) Then Return False
            Next
            Return True
        End Function

        Private Shared Function AreSame(a As GenericParameter, b As GenericParameter) As Boolean
            Return a.Position = b.Position
        End Function

        Public Shared Function AreSame(a As TypeReference, b As TypeReference) As Boolean
            If ReferenceEquals(a, b) Then Return True
            If a Is Nothing OrElse b Is Nothing Then Return False
            If a.MetadataType <> b.MetadataType Then Return False
            If a.IsGenericParameter Then Return AreSame(CType(a, GenericParameter), CType(b, GenericParameter))
            If TypeOf a Is TypeSpecification Then Return AreSame(CType(a, TypeSpecification), CType(b, TypeSpecification))
            If a.Name <> b.Name OrElse a.[Namespace] <> b.[Namespace] Then Return False
            Return AreSame(a.DeclaringType, b.DeclaringType)
        End Function

        Public Shared Function MethodMatch(ByVal candidate As MethodDefinition, ByVal method As MethodReference) As Boolean
            If candidate.Name <> method.Name Then Return False
            If Not TypeMatch(candidate.ReturnType, method.ReturnType) Then Return False
            If candidate.Parameters.Count <> method.Parameters.Count Then Return False
            For i As Integer = 0 To candidate.Parameters.Count - 1
                If Not TypeMatch(candidate.Parameters(i).ParameterType, method.Parameters(i).ParameterType) Then Return False
            Next
            Return True
        End Function

        Private Shared Function TypeMatch(ByVal a As IModifierType, ByVal b As IModifierType) As Boolean
            If Not TypeMatch(a.ModifierType, b.ModifierType) Then Return False
            Return TypeMatch(a.ElementType, b.ElementType)
        End Function

        Private Shared Function TypeMatch(ByVal a As TypeSpecification, ByVal b As TypeSpecification) As Boolean
            If TypeOf a Is GenericInstanceType Then Return TypeMatch(CType(a, GenericInstanceType), CType(b, GenericInstanceType))
            If TypeOf a Is IModifierType Then Return TypeMatch(CType(a, IModifierType), CType(b, IModifierType))
            Return TypeMatch(a.ElementType, b.ElementType)
        End Function

        Private Shared Function TypeMatch(ByVal a As GenericInstanceType, ByVal b As GenericInstanceType) As Boolean
            If Not TypeMatch(a.ElementType, b.ElementType) Then Return False
            If a.GenericArguments.Count <> b.GenericArguments.Count Then Return False
            If a.GenericArguments.Count = 0 Then Return True
            For i As Integer = 0 To a.GenericArguments.Count - 1
                If Not TypeMatch(a.GenericArguments(i), b.GenericArguments(i)) Then Return False
            Next
            Return True
        End Function

        Private Shared Function TypeMatch(ByVal a As TypeReference, ByVal b As TypeReference) As Boolean
            If TypeOf a Is GenericParameter Then Return True
            If TypeOf a Is TypeSpecification OrElse TypeOf b Is TypeSpecification Then
                If a.[GetType]() <> b.[GetType]() Then Return False
                Return TypeMatch(CType(a, TypeSpecification), CType(b, TypeSpecification))
            End If
            Return a.FullName = b.FullName
        End Function

        Public Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

    End Class

#End Region

End Namespace