Imports Mono.Cecil
Imports Mono.Cecil.Cil

Namespace CecilHelper
    Public NotInheritable Class Finder

#Region " Methods "

        Public Shared Function FindType(moduleDef As ModuleDefinition, Name As String, Optional ByVal Full As Boolean = False) As TypeDefinition
            For Each typeDef As TypeDefinition In moduleDef.Types
                Dim returnType As TypeDefinition

                If Full Then
                    If typeDef.FullName = Name Then
                        Return typeDef
                    End If
                Else
                    If typeDef.Name = Name Then
                        Return typeDef
                    End If
                End If

                returnType = FindNestedType(typeDef, Name)

                If returnType IsNot Nothing Then
                    Return returnType
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Recursive search for nested types
        ''' </summary>
        ''' <param name="parentType"></param>
        ''' <param name="fullname"></param>
        ''' <returns></returns>
        Private Shared Function FindNestedType(parentType As TypeDefinition, fullname As String) As TypeDefinition
            For Each type In parentType.NestedTypes
                If type.FullName = fullname Then
                    Return type
                End If

                If type.HasNestedTypes Then
                    Return FindNestedType(type, fullname)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' by The Unknown Programmer
        ''' </summary>
        ''' <param name="parentDef"></param>
        ''' <param name="name"></param>
        ''' <returns></returns>
        Public Shared Function FindType(parentDef As TypeDefinition, name As String) As TypeDefinition
            Return parentDef.NestedTypes.First(Function(t) t.Name = name)
        End Function

        Public Shared Function FindMethod(assDef As AssemblyDefinition, name As String) As MethodDefinition
            For Each t In assDef.MainModule.Types
                If t.HasMethods Then
                    For Each methodDef As MethodDefinition In t.Methods
                        If methodDef.Name = name Then
                            Return methodDef
                        End If
                    Next
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function FindMethod(typeDef As TypeDefinition, name As String) As MethodDefinition
            For Each methodDef As MethodDefinition In typeDef.Methods
                If methodDef.Name = name Then
                    Return methodDef
                End If
            Next
            Return Nothing
        End Function

        Public Shared Function FindField(parentType As TypeDefinition, name As String) As FieldDefinition
            Return parentType.Fields.First(Function(f) f.Name = name)
        End Function

        Public Shared Function FindDefaultNamespace(assDef As AssemblyDefinition, Packer As Boolean) As String
            If Packer Then
                Return String.Empty
            End If
            Return assDef.MainModule.EntryPoint.DeclaringType.Namespace
        End Function

        Public Shared Function FindFrameworkVersion(assDef As AssemblyDefinition) As String
            Return If(assDef.MainModule.Runtime.ToString.StartsWith("Net_4"), "v4.0", "v2.0")
        End Function

        Public Shared Function AccessorMethods(type As TypeDefinition) As List(Of MethodDefinition)
            Dim list As New List(Of MethodDefinition)
            For Each Pdef In type.Properties
                list.Add(Pdef.GetMethod)
                list.Add(Pdef.SetMethod)
                If Pdef.HasOtherMethods Then
                    For Each oDef In Pdef.OtherMethods
                        list.Add(oDef)
                    Next
                End If
            Next
            For Each eDef In type.Events
                list.Add(eDef.AddMethod)
                list.Add(eDef.RemoveMethod)
                list.Add(eDef.InvokeMethod)
                If eDef.HasOtherMethods Then
                    For Each oDef In eDef.OtherMethods
                        list.Add(oDef)
                    Next
                End If
            Next
            Return list
        End Function

        Public Shared Function TryGetField(TypeDef As TypeDefinition, PropDef As PropertyDefinition) As FieldDefinition
            Dim propertyName = PropDef.Name
            Dim fieldsWithSameType = TypeDef.Fields.Where(Function(x) x.DeclaringType.Equals(TypeDef) AndAlso Utils.AreSame(x.FieldType, PropDef.PropertyType)).ToList()

            For Each field In fieldsWithSameType

                If field.Name = $"<{propertyName}>k__BackingField" Then
                    Return field
                End If
            Next

            For Each field In fieldsWithSameType
                Dim upperPropertyName = propertyName.ToUpper()
                Dim fieldUpper = field.Name.ToUpper()

                If fieldUpper = upperPropertyName Then
                    Return field
                End If

                If fieldUpper = "_" & upperPropertyName Then
                    Return field
                End If
            Next

            Return GetSingleField(PropDef)
        End Function

        Private Shared Function GetSingleField(PropDef As PropertyDefinition) As FieldDefinition
            Dim fieldDefinition = GetSingleField(PropDef, Code.Stfld, PropDef.SetMethod)

            If fieldDefinition IsNot Nothing Then
                Return fieldDefinition
            End If

            Return GetSingleField(PropDef, Code.Ldfld, PropDef.GetMethod)
        End Function

        Private Shared Function GetSingleField(PropDef As PropertyDefinition, code As Code, mDef As MethodDefinition) As FieldDefinition
            If mDef?.Body Is Nothing Then
                Return Nothing
            End If

            Dim fieldReference As FieldReference = Nothing
            Dim field As FieldReference = Nothing

            For Each instruction In mDef.Body.Instructions

                If instruction.OpCode.Code = code Then

                    If fieldReference IsNot Nothing Then
                        Return Nothing
                    End If

                    If Not (Utils.InlineAssignHelper(field, TryCast(instruction.Operand, FieldReference)) IsNot Nothing) Then
                        Continue For
                    End If

                    If field.DeclaringType Is PropDef.DeclaringType Then
                        Continue For
                    End If

                    If field.FieldType Is PropDef.PropertyType Then
                        Continue For
                    End If

                    Dim fieldDef = TryCast(instruction.Operand, FieldDefinition)
                    Dim fAttributes = fieldDef?.Attributes And FieldAttributes.InitOnly

                    If fAttributes = FieldAttributes.InitOnly Then
                        Continue For
                    End If

                    fieldReference = field
                End If
            Next

            Return fieldReference?.Resolve()
        End Function

        Public Shared Function TryGetProperty(TypeDef As TypeDefinition, fDef As FieldDefinition) As PropertyDefinition
            Dim fDefName = fDef.Name

            If TypeDef.HasProperties Then
                Dim propsWithSameType = TypeDef.Properties.Where(Function(x) x.DeclaringType.Equals(TypeDef) AndAlso Utils.AreSame(x.PropertyType, fDef.FieldType))

                For Each prop In propsWithSameType
                    Dim upperPropertyName = prop.Name.ToUpper()
                    Dim fieldUpper = fDefName.ToUpper()

                    If fDefName = $"<{prop.Name}>k__BackingField" Then
                        Return prop
                    End If

                    If fieldUpper = upperPropertyName Then
                        Return prop
                    End If

                    If fieldUpper = "_" & upperPropertyName Then
                        Return prop
                    End If
                Next
            End If

            Return Nothing
        End Function

#End Region

    End Class
End Namespace
