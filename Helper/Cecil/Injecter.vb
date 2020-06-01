Imports System.IO
Imports Mono.Cecil
Imports Mono.Cecil.Cil

Namespace CecilHelper
    ' Credits: yck1509 / Confuser
    Public NotInheritable Class Injecter

#Region " Methods "
        Public Shared Function CreateGenericCctor(assDef As AssemblyDefinition) As MethodDefinition
            Dim cctor__1 As New MethodDefinition(".cctor", MethodAttributes.Private Or MethodAttributes.HideBySig Or MethodAttributes.SpecialName Or MethodAttributes.RTSpecialName Or MethodAttributes.[Static], assDef.MainModule.Import(GetType(Void)))
            Dim ilproc As ILProcessor
            cctor__1.Body = New Cil.MethodBody(cctor__1)
            ilproc = cctor__1.Body.GetILProcessor()
            ilproc.Emit(OpCodes.Ret)
            Return cctor__1
        End Function

        Public Shared Sub InjectAssemblyInfoCustomAttribute(member As AssemblyDefinition, AttributeType As Type, attributeValue$)
            Dim ca As New CustomAttribute(member.MainModule.Import(AttributeType.GetConstructor(New Type() {GetType(String)})))
            Dim carg = New CustomAttributeArgument(member.MainModule.Import(GetType(String)), attributeValue)
            ca.ConstructorArguments.Add(carg)
            member.CustomAttributes.Add(ca)
        End Sub

        Public Shared Function CreateAndInjectTypeDefinition(mdef As ModuleDefinition, name As String, ByVal baseType As TypeReference) As TypeDefinition
            Dim str As String = String.Empty
            If name.Contains(".") Then
                Dim length As Integer = name.LastIndexOf(".")
                str = name.Substring(0, length)
                name = name.Substring((length + 1))
            End If
            Dim item As New TypeDefinition(str, name, (TypeAttributes.AnsiClass Or TypeAttributes.NotPublic), baseType)
            mdef.Types.Add(item)
            Return item
        End Function

        Private Shared Function ImportType(typeRef As TypeReference, mDef As ModuleDefinition, context As MethodReference, mems As Dictionary(Of MetadataToken, IMemberDefinition)) As TypeReference
            Dim ret As TypeReference = typeRef
            If TypeOf typeRef Is TypeSpecification Then
                If TypeOf typeRef Is ArrayType Then
                    Dim _spec As ArrayType = TryCast(typeRef, ArrayType)
                    ret = New ArrayType(ImportType(_spec.ElementType, mDef, context, mems))
                    TryCast(ret, ArrayType).Dimensions.Clear()
                    For Each i In _spec.Dimensions
                        TryCast(ret, ArrayType).Dimensions.Add(i)
                    Next
                ElseIf TypeOf typeRef Is GenericInstanceType Then
                    Dim _spec As GenericInstanceType = TryCast(typeRef, GenericInstanceType)
                    ret = New GenericInstanceType(ImportType(_spec.ElementType, mDef, context, mems))
                    For Each i In _spec.GenericArguments
                        TryCast(ret, GenericInstanceType).GenericArguments.Add(ImportType(i, mDef, context, mems))
                    Next
                ElseIf TypeOf typeRef Is OptionalModifierType Then
                    ret = New OptionalModifierType(ImportType(TryCast(typeRef, OptionalModifierType).ModifierType, mDef, context, mems), ImportType(TryCast(typeRef, TypeSpecification).ElementType, mDef, context, mems))
                ElseIf TypeOf typeRef Is RequiredModifierType Then
                    ret = New RequiredModifierType(ImportType(TryCast(typeRef, RequiredModifierType).ModifierType, mDef, context, mems), ImportType(TryCast(typeRef, TypeSpecification).ElementType, mDef, context, mems))
                ElseIf TypeOf typeRef Is ByReferenceType Then
                    ret = New ByReferenceType(ImportType(TryCast(typeRef, TypeSpecification).ElementType, mDef, context, mems))
                ElseIf TypeOf typeRef Is PointerType Then
                    ret = New PointerType(ImportType(TryCast(typeRef, TypeSpecification).ElementType, mDef, context, mems))
                ElseIf TypeOf typeRef Is PinnedType Then
                    ret = New PinnedType(ImportType(TryCast(typeRef, TypeSpecification).ElementType, mDef, context, mems))
                ElseIf TypeOf typeRef Is SentinelType Then
                    ret = New SentinelType(ImportType(TryCast(typeRef, TypeSpecification).ElementType, mDef, context, mems))
                Else
                    Throw New NotSupportedException()
                End If
            ElseIf TypeOf typeRef Is GenericParameter Then
                If context Is Nothing OrElse TypeOf TryCast(typeRef, GenericParameter).Owner Is TypeReference OrElse TryCast(typeRef, GenericParameter).Position >= context.GenericParameters.Count Then
                    Return typeRef
                End If
                Return context.GenericParameters(TryCast(typeRef, GenericParameter).Position)
            Else
                If mems IsNot Nothing AndAlso mems.ContainsKey(typeRef.MetadataToken) Then
                    ret = TryCast(mems(typeRef.MetadataToken), TypeReference)
                ElseIf Not (TypeOf ret Is TypeDefinition) Then
                    ret = mDef.Import(ret)
                End If
            End If
            Return ret
        End Function

        Private Shared Function ImportMethod(mtdRef As MethodReference, mDef As ModuleDefinition, context As MethodReference, mems As Dictionary(Of MetadataToken, IMemberDefinition)) As MethodReference
            Dim ret As MethodReference = mtdRef
            If TypeOf mtdRef Is GenericInstanceMethod Then
                Dim _spec As GenericInstanceMethod = TryCast(mtdRef, GenericInstanceMethod)
                ret = New GenericInstanceMethod(ImportMethod(_spec.ElementMethod, mDef, context, mems))
                For Each i In _spec.GenericArguments
                    TryCast(ret, GenericInstanceMethod).GenericArguments.Add(ImportType(i, mDef, context, mems))
                Next

                ret.ReturnType = ImportType(ret.ReturnType, mDef, ret, mems)
                For Each i In ret.Parameters
                    i.ParameterType = ImportType(i.ParameterType, mDef, ret, mems)
                Next
            Else
                If mems IsNot Nothing AndAlso mems.ContainsKey(mtdRef.MetadataToken) Then
                    ret = TryCast(mems(mtdRef.MetadataToken), MethodReference)
                Else
                    ret = mDef.Import(ret)
                    ret.ReturnType = ImportType(ret.ReturnType, mDef, ret, mems)
                    For Each i In ret.Parameters
                        i.ParameterType = ImportType(i.ParameterType, mDef, ret, mems)
                    Next
                End If
            End If
            If Not (TypeOf mtdRef Is MethodDefinition) AndAlso Not (TypeOf mtdRef Is MethodSpecification) Then
                ret.DeclaringType = ImportType(mtdRef.DeclaringType, mDef, context, mems)
            End If
            Return ret
        End Function

        Private Shared Function ImportField(fldRef As FieldReference, mDef As ModuleDefinition, mems As Dictionary(Of MetadataToken, IMemberDefinition)) As FieldReference
            If mems IsNot Nothing AndAlso mems.ContainsKey(fldRef.MetadataToken) Then
                Return TryCast(mems(fldRef.MetadataToken), FieldReference)
            Else
                Return mDef.Import(fldRef)
            End If
        End Function

        Private Shared Sub PopulateDatas(mDef As ModuleDefinition, type As TypeDefinition, mems As Dictionary(Of MetadataToken, IMemberDefinition))
            Dim newType As TypeDefinition = TryCast(mems(type.MetadataToken), TypeDefinition)

            If type.BaseType IsNot Nothing Then
                newType.BaseType = ImportType(type.BaseType, mDef, Nothing, mems)
            End If

            For Each ty As TypeDefinition In type.NestedTypes
                PopulateDatas(mDef, ty, mems)
            Next
            For Each fld As FieldDefinition In type.Fields
                If fld.IsLiteral Then
                    Continue For
                End If
                TryCast(mems(fld.MetadataToken), FieldDefinition).FieldType = ImportType(fld.FieldType, mDef, Nothing, mems)
            Next
            For Each mtd As MethodDefinition In type.Methods
                PopulateMethod(mDef, mtd, TryCast(mems(mtd.MetadataToken), MethodDefinition), mems)
            Next
        End Sub

        Public Shared Sub PopulateMethod(mDef As ModuleDefinition, mtd As MethodDefinition, newMtd As MethodDefinition, mems As Dictionary(Of MetadataToken, IMemberDefinition))
            newMtd.Attributes = mtd.Attributes
            newMtd.ImplAttributes = mtd.ImplAttributes
            If mtd.IsPInvokeImpl Then
                newMtd.PInvokeInfo = mtd.PInvokeInfo
                Dim has As Boolean = False
                For Each modRef As ModuleReference In mDef.ModuleReferences
                    If modRef.Name = newMtd.PInvokeInfo.[Module].Name Then
                        has = True
                        newMtd.PInvokeInfo.[Module] = modRef
                        Exit For
                    End If
                Next
                If Not has Then
                    mDef.ModuleReferences.Add(newMtd.PInvokeInfo.[Module])
                End If
            End If
            If mtd.HasCustomAttributes Then
                For Each attr As CustomAttribute In mtd.CustomAttributes
                    Dim nAttr As New CustomAttribute(ImportMethod(attr.Constructor, mDef, newMtd, mems), attr.GetBlob())
                    newMtd.CustomAttributes.Add(nAttr)
                Next
            End If

            For Each param As GenericParameter In mtd.GenericParameters
                Dim p = New GenericParameter(param.Name, newMtd)
                If param.HasCustomAttributes Then
                    For Each attr As CustomAttribute In param.CustomAttributes
                        Dim nAttr As New CustomAttribute(mDef.Import(attr.Constructor), attr.GetBlob())
                        p.CustomAttributes.Add(nAttr)
                    Next
                End If
                newMtd.GenericParameters.Add(p)
            Next

            newMtd.ReturnType = ImportType(mtd.ReturnType, mDef, newMtd, mems)

            For Each param As ParameterDefinition In mtd.Parameters
                Dim p = New ParameterDefinition(param.Name, param.Attributes, ImportType(param.ParameterType, mDef, newMtd, mems))
                If param.HasCustomAttributes Then
                    For Each attr As CustomAttribute In param.CustomAttributes
                        Dim nAttr As New CustomAttribute(ImportMethod(attr.Constructor, mDef, newMtd, mems), attr.GetBlob())
                        p.CustomAttributes.Add(nAttr)
                    Next
                End If
                newMtd.Parameters.Add(p)
            Next

            If mtd.HasBody Then
                Dim old As MethodBody = mtd.Body
                Dim bdy As New MethodBody(newMtd) With {
                    .MaxStackSize = old.MaxStackSize,
                    .InitLocals = old.InitLocals
                }

                Dim psr As ILProcessor = bdy.GetILProcessor()

                For Each varDef As VariableDefinition In old.Variables
                    bdy.Variables.Add(New VariableDefinition(ImportType(varDef.VariableType, mDef, newMtd, mems)))
                Next

                For Each inst As Instruction In old.Instructions
                    Select Case inst.OpCode.OperandType
                        Case OperandType.InlineArg, OperandType.ShortInlineArg
                            If inst.Operand Is old.ThisParameter Then
                                psr.Emit(inst.OpCode, bdy.ThisParameter)
                            Else
                                Dim param As Integer = mtd.Parameters.IndexOf(TryCast(inst.Operand, ParameterDefinition))
                                psr.Emit(inst.OpCode, newMtd.Parameters(param))
                            End If
                            Exit Select
                        Case OperandType.InlineVar, OperandType.ShortInlineVar
                            Dim var As Integer = old.Variables.IndexOf(TryCast(inst.Operand, VariableDefinition))
                            psr.Emit(inst.OpCode, bdy.Variables(var))
                            Exit Select
                        Case OperandType.InlineField
                            psr.Emit(inst.OpCode, ImportField(TryCast(inst.Operand, FieldReference), mDef, mems))
                            Exit Select
                        Case OperandType.InlineMethod
                            psr.Emit(inst.OpCode, ImportMethod(TryCast(inst.Operand, MethodReference), mDef, newMtd, mems))
                            Exit Select
                        Case OperandType.InlineType
                            psr.Emit(inst.OpCode, ImportType(TryCast(inst.Operand, TypeReference), mDef, newMtd, mems))
                            Exit Select
                        Case OperandType.InlineTok
                            If TypeOf inst.Operand Is FieldReference Then
                                psr.Emit(inst.OpCode, ImportField(TryCast(inst.Operand, FieldReference), mDef, mems))
                            ElseIf TypeOf inst.Operand Is MethodReference Then
                                psr.Emit(inst.OpCode, ImportMethod(TryCast(inst.Operand, MethodReference), mDef, newMtd, mems))
                            ElseIf TypeOf inst.Operand Is TypeReference Then
                                psr.Emit(inst.OpCode, ImportType(TryCast(inst.Operand, TypeReference), mDef, newMtd, mems))
                            End If
                            Exit Select
                        Case Else
                            psr.Append(inst)
                            Exit Select
                    End Select
                Next

                For i As Integer = 0 To bdy.Instructions.Count - 1
                    Dim inst As Instruction = bdy.Instructions(i)
                    Dim o As Instruction = old.Instructions(i)

                    If inst.OpCode.OperandType = OperandType.InlineSwitch Then
                        Dim olds As Instruction() = DirectCast(o.Operand, Instruction())
                        Dim news As Instruction() = New Instruction(olds.Length - 1) {}

                        For ii As Integer = 0 To news.Length - 1
                            news(ii) = bdy.Instructions(old.Instructions.IndexOf(olds(ii)))
                        Next

                        inst.Operand = news
                    ElseIf inst.OpCode.OperandType = OperandType.ShortInlineBrTarget OrElse inst.OpCode.OperandType = OperandType.InlineBrTarget Then
                        inst.Operand = bdy.Instructions(old.Instructions.IndexOf(TryCast(inst.Operand, Instruction)))
                    End If
                Next

                For Each eh As ExceptionHandler In old.ExceptionHandlers
                    Dim neh As New ExceptionHandler(eh.HandlerType)
                    If old.Instructions.IndexOf(eh.TryStart) <> -1 Then
                        neh.TryStart = bdy.Instructions(old.Instructions.IndexOf(eh.TryStart))
                    End If
                    If old.Instructions.IndexOf(eh.TryEnd) <> -1 Then
                        neh.TryEnd = bdy.Instructions(old.Instructions.IndexOf(eh.TryEnd))
                    End If
                    If old.Instructions.IndexOf(eh.HandlerStart) <> -1 Then
                        neh.HandlerStart = bdy.Instructions(old.Instructions.IndexOf(eh.HandlerStart))
                    End If
                    If old.Instructions.IndexOf(eh.HandlerEnd) <> -1 Then
                        neh.HandlerEnd = bdy.Instructions(old.Instructions.IndexOf(eh.HandlerEnd))
                    End If

                    Select Case eh.HandlerType
                        Case ExceptionHandlerType.Catch
                            neh.CatchType = ImportType(eh.CatchType, mDef, newMtd, mems)
                            Exit Select
                        Case ExceptionHandlerType.Filter
                            neh.FilterStart = bdy.Instructions(old.Instructions.IndexOf(eh.FilterStart))
                            'neh.FilterEnd = bdy.Instructions(old.Instructions.IndexOf(eh.FilterEnd))
                            Exit Select
                    End Select

                    bdy.ExceptionHandlers.Add(neh)
                Next

                newMtd.Body = bdy
            End If
        End Sub

        Public Shared Function Inject(targetModule As ModuleDefinition, mtd As MethodDefinition) As MethodDefinition

            If mtd Is Nothing Then Return Nothing

            Dim ret As New MethodDefinition(mtd.Name, mtd.Attributes, targetModule.TypeSystem.Void) With {
                .Attributes = mtd.Attributes,
                .ImplAttributes = mtd.ImplAttributes
            }

            If mtd.IsPInvokeImpl Then
                ret.PInvokeInfo = mtd.PInvokeInfo
                Dim has As Boolean = False
                For Each modRef As ModuleReference In targetModule.ModuleReferences
                    If modRef.Name = ret.PInvokeInfo.Module.Name Then
                        has = True
                        ret.PInvokeInfo.[Module] = modRef
                        Exit For
                    End If
                Next
                If Not has Then
                    targetModule.ModuleReferences.Add(ret.PInvokeInfo.[Module])
                End If
            End If
            If mtd.HasCustomAttributes Then
                For Each attr As CustomAttribute In mtd.CustomAttributes
                    Dim nAttr As New CustomAttribute(ImportMethod(attr.Constructor, targetModule, ret, Nothing), attr.GetBlob())
                    ret.CustomAttributes.Add(nAttr)
                Next
            End If

            For Each param As GenericParameter In mtd.GenericParameters
                Dim p = New GenericParameter(param.Name, ret)
                If param.HasCustomAttributes Then
                    For Each attr As CustomAttribute In param.CustomAttributes
                        Dim nAttr As New CustomAttribute(ImportMethod(attr.Constructor, targetModule, ret, Nothing), attr.GetBlob())
                        p.CustomAttributes.Add(nAttr)
                    Next
                End If
                ret.GenericParameters.Add(p)
            Next

            ret.ReturnType = ImportType(mtd.ReturnType, targetModule, ret, Nothing)

            For Each param As ParameterDefinition In mtd.Parameters
                Dim p = New ParameterDefinition(param.Name, param.Attributes, ImportType(param.ParameterType, targetModule, ret, Nothing))
                If param.HasCustomAttributes Then
                    For Each attr As CustomAttribute In param.CustomAttributes
                        Dim nAttr As New CustomAttribute(ImportMethod(attr.Constructor, targetModule, ret, Nothing), attr.GetBlob())
                        p.CustomAttributes.Add(nAttr)
                    Next
                End If
                ret.Parameters.Add(p)
            Next

            If mtd.HasBody Then
                Dim old As MethodBody = mtd.Body
                Dim bdy As New MethodBody(ret) With {
                    .MaxStackSize = old.MaxStackSize,
                    .InitLocals = old.InitLocals
                }

                Dim psr As ILProcessor = bdy.GetILProcessor()

                For Each var As VariableDefinition In old.Variables
                    bdy.Variables.Add(New VariableDefinition(ImportType(var.VariableType, targetModule, ret, Nothing)))
                Next

                For Each inst As Instruction In old.Instructions
                    Select Case inst.OpCode.OperandType
                        Case OperandType.InlineArg, OperandType.ShortInlineArg
                            If inst.Operand Is old.ThisParameter Then
                                psr.Emit(inst.OpCode, bdy.ThisParameter)
                            Else
                                Dim param As Integer = mtd.Parameters.IndexOf(TryCast(inst.Operand, ParameterDefinition))
                                psr.Emit(inst.OpCode, ret.Parameters(param))
                            End If
                            Exit Select
                        Case OperandType.InlineVar, OperandType.ShortInlineVar
                            Dim var As Integer = old.Variables.IndexOf(TryCast(inst.Operand, VariableDefinition))
                            psr.Emit(inst.OpCode, bdy.Variables(var))
                            Exit Select
                        Case OperandType.InlineField
                            psr.Emit(inst.OpCode, ImportField(TryCast(inst.Operand, FieldReference), targetModule, Nothing))
                            Exit Select
                        Case OperandType.InlineMethod
                            If inst.Operand Is mtd Then
                                psr.Emit(inst.OpCode, ret)
                            Else
                                psr.Emit(inst.OpCode, ImportMethod(TryCast(inst.Operand, MethodReference), targetModule, ret, Nothing))
                            End If
                            Exit Select
                        Case OperandType.InlineType
                            psr.Emit(inst.OpCode, ImportType(TryCast(inst.Operand, TypeReference), targetModule, ret, Nothing))
                            Exit Select
                        Case OperandType.InlineTok
                            If TypeOf inst.Operand Is TypeReference Then
                                psr.Emit(inst.OpCode, ImportType(TryCast(inst.Operand, TypeReference), targetModule, ret, Nothing))
                            ElseIf TypeOf inst.Operand Is FieldReference Then
                                psr.Emit(inst.OpCode, ImportField(TryCast(inst.Operand, FieldReference), targetModule, Nothing))
                            ElseIf TypeOf inst.Operand Is MethodReference Then
                                psr.Emit(inst.OpCode, ImportMethod(TryCast(inst.Operand, MethodReference), targetModule, ret, Nothing))
                            End If
                            Exit Select
                        Case Else
                            psr.Append(inst)
                            Exit Select
                    End Select
                Next

                For i As Integer = 0 To bdy.Instructions.Count - 1
                    Dim inst As Instruction = bdy.Instructions(i)
                    Dim o As Instruction = old.Instructions(i)

                    If inst.OpCode.OperandType = OperandType.InlineSwitch Then
                        Dim olds As Instruction() = DirectCast(o.Operand, Instruction())
                        Dim news As Instruction() = New Instruction(olds.Length - 1) {}

                        For ii As Integer = 0 To news.Length - 1
                            news(ii) = bdy.Instructions(old.Instructions.IndexOf(olds(ii)))
                        Next

                        inst.Operand = news
                    ElseIf inst.OpCode.OperandType = OperandType.ShortInlineBrTarget OrElse inst.OpCode.OperandType = OperandType.InlineBrTarget Then
                        inst.Operand = bdy.Instructions(old.Instructions.IndexOf(TryCast(inst.Operand, Instruction)))
                    End If
                Next

                For Each eh As ExceptionHandler In old.ExceptionHandlers
                    Dim neh As New ExceptionHandler(eh.HandlerType)
                    If old.Instructions.IndexOf(eh.TryStart) <> -1 Then
                        neh.TryStart = bdy.Instructions(old.Instructions.IndexOf(eh.TryStart))
                    End If
                    If old.Instructions.IndexOf(eh.TryEnd) <> -1 Then
                        neh.TryEnd = bdy.Instructions(old.Instructions.IndexOf(eh.TryEnd))
                    End If
                    If old.Instructions.IndexOf(eh.HandlerStart) <> -1 Then
                        neh.HandlerStart = bdy.Instructions(old.Instructions.IndexOf(eh.HandlerStart))
                    End If
                    If old.Instructions.IndexOf(eh.HandlerEnd) <> -1 Then
                        neh.HandlerEnd = bdy.Instructions(old.Instructions.IndexOf(eh.HandlerEnd))
                    End If

                    Select Case eh.HandlerType
                        Case ExceptionHandlerType.Catch
                            neh.CatchType = ImportType(eh.CatchType, targetModule, ret, Nothing)
                            Exit Select
                        Case ExceptionHandlerType.Filter
                            neh.FilterStart = bdy.Instructions(old.Instructions.IndexOf(eh.FilterStart))
                            Exit Select
                    End Select

                    bdy.ExceptionHandlers.Add(neh)
                Next

                ret.Body = bdy
            End If

            Return ret
        End Function

        Public Shared Function InjectTypeDefinition(m As ModuleDefinition, type As TypeDefinition) As TypeDefinition
            Dim mems As New Dictionary(Of MetadataToken, IMemberDefinition)
            Dim definition As TypeDefinition = InjectTypeDef(m, type, mems)
            PopulateDatas(m, type, mems)
            Return definition
        End Function

        Private Shared Function InjectTypeDef(mDef As ModuleDefinition, type As TypeDefinition, mems As Dictionary(Of MetadataToken, IMemberDefinition)) As TypeDefinition
            Dim definition As New TypeDefinition(type.Namespace, type.Name, type.Attributes) With {
                .Scope = mDef,
                .ClassSize = type.ClassSize,
                .PackingSize = type.PackingSize
            }
            If type.HasCustomAttributes Then
                For Each attr As CustomAttribute In type.CustomAttributes
                    Dim nAttr As New CustomAttribute(ImportMethod(attr.Constructor, mDef, attr.Constructor, mems), attr.GetBlob())
                    definition.CustomAttributes.Add(nAttr)
                Next
            End If

            If (Not type.BaseType Is Nothing) Then
                definition.BaseType = mDef.Import(type.BaseType)
            End If
            mems.Add(type.MetadataToken, definition)
            For Each definition2 In type.NestedTypes
                Dim item As TypeDefinition = InjectTypeDef(mDef, definition2, mems)
                definition.NestedTypes.Add(item)
            Next
            For Each definition4 In type.Fields
                If Not definition4.IsLiteral Then
                    Dim definition5 As New FieldDefinition(definition4.Name, definition4.Attributes, mDef.TypeSystem.Void)
                    mems.Add(definition4.MetadataToken, definition5)
                    definition.Fields.Add(definition5)
                End If
            Next
            For Each definition6 In type.Methods
                Dim definition7 As New MethodDefinition(definition6.Name, definition6.Attributes, definition6.ReturnType)
                mems.Add(definition6.MetadataToken, definition7)
                definition.Methods.Add(definition7)
            Next
            Return definition
        End Function

        Public Shared Function InjectResource(mdef As ModuleDefinition, name As String, resourceType As ResourceType, data As Byte()) As Resource
            Dim item As Resource
            Select Case resourceType
                Case ResourceType.Embedded
                    item = New EmbeddedResource(name, ManifestResourceAttributes.Private, data)
                    Exit Select
                Case ResourceType.Linked
                    item = New LinkedResource(name, ManifestResourceAttributes.Public)
                    Exit Select
                Case ResourceType.AssemblyLinked
                    Dim reference As New AssemblyNameReference(name, New Version)
                    mdef.AssemblyReferences.Add(reference)
                    item = New AssemblyLinkedResource(name, ManifestResourceAttributes.Public, reference)
                    Exit Select
                Case Else
                    Throw New ArgumentException
            End Select
            mdef.Resources.Add(item)
            Return item
        End Function
        Public Shared Sub InjectResource(TargetAssembly As AssemblyDefinition, ResName As String)
            Dim CompressRes As EmbeddedResource = New EmbeddedResource(ResName & ".resources", ManifestResourceAttributes.Private, File.ReadAllBytes(My.Application.Info.DirectoryPath & "\" & ResName & ".resources"))
            TargetAssembly.MainModule.Resources.Add(CompressRes)
        End Sub

#End Region

    End Class
End Namespace