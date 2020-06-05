Imports Mono.Cecil

Namespace CecilHelper
    Public NotInheritable Class NameChecker

#Region " Methods "
        ''' <summary>
        ''' INFO : Verifying if typeDefinition is renamable
        ''' </summary>
        ''' <param name="type"></param>
        Public Shared Function IsRenamable(type As TypeDefinition) As Boolean
            Return Not type.FullName = "<Module>" AndAlso Not type.Name.StartsWith("<") AndAlso Not type.Name.Contains("__") AndAlso Not type.IsImport AndAlso Not type.IsSerializable AndAlso Not type.IsRuntimeSpecialName
        End Function

        ''' <summary>
        ''' INFO : Verifying if methodDefinition is renamable
        ''' </summary>
        ''' <param name="method"></param>
        Public Shared Function IsRenamable(method As MethodDefinition, Optional ByVal Force As Boolean = False) As Boolean
            If method IsNot Nothing Then
                If Force Then
                    If method.HasBody Then
                        If Finder.AccessorMethods(method.DeclaringType).Contains(method) Then
                            Dim ForceIsOK = If(Not MemberExtensions.HasCustomAttributeByMemberName(method, "DebuggerHiddenAttribute"), True, False)
                            Return If(ForceIsOK, IsRenamable2(method), False)
                        End If
                    End If
                End If
                Return IsRenamable2(method)
            End If
            Return False
        End Function

        Private Shared Function IsRenamable2(method As MethodDefinition)
            If (method.IsRuntimeSpecialName OrElse method.IsConstructor) Then
                Return False
            End If

            If method.DeclaringType.IsImport AndAlso Not method.CustomAttributes.Any(Function(f) f.AttributeType.FullName = "System.Runtime.InteropServices.DispIdAttribute") Then
                Return False
            End If
            If method.DeclaringType.BaseType IsNot Nothing AndAlso method.DeclaringType.BaseType.Resolve() IsNot Nothing Then
                Dim bType As TypeReference = method.DeclaringType.BaseType
                If bType.FullName = "System.Delegate" OrElse bType.FullName = "System.MulticastDelegate" Then
                    Return False
                End If
            End If
            Return True
        End Function

        ''' <summary>
        ''' INFO : Verifying if eventDefinition is renamable
        ''' </summary>
        ''' <param name="Events"></param>
        Public Shared Function IsRenamable(Events As EventDefinition) As Boolean
            If Not Events Is Nothing Then
                Return If((Not Events.IsSpecialName AndAlso Not Events.IsRuntimeSpecialName AndAlso Not Events.DeclaringType.IsSerializable), True, False)
            End If
            Return False
        End Function

        ''' <summary>
        ''' INFO : Verifying if propertyDefinition is renamable
        ''' </summary>
        ''' <param name="prop"></param>
        Public Shared Function IsRenamable(prop As PropertyDefinition) As Boolean
            If prop IsNot Nothing Then
                If prop.IsRuntimeSpecialName Then
                    Return False
                End If

                Dim IsSerializable As Boolean = MemberExtensions.HasCustomAttributeByMemberName(prop, "XmlIgnoreAttribute") = False AndAlso prop.DeclaringType.IsSerializable
                Return If(Not prop.IsSpecialName AndAlso Not IsSerializable AndAlso Not Utils.HasINotifyPropertyChanged(prop) AndAlso Not prop.DeclaringType.Name.Contains("AnonymousType"), True, False)
            End If
            Return False
        End Function

        ''' <summary>
        ''' INFO : Verifying if fieldDefinition is renamable
        ''' </summary>
        ''' <param name="field"></param>
        Public Shared Function IsRenamable(field As FieldDefinition) As Boolean
            If field.IsRuntimeSpecialName OrElse field.DeclaringType.IsEnum OrElse field.IsPInvokeImpl Then
                Return False
            End If

            If MemberExtensions.HasCustomAttributeByMemberName(field, "XmlIgnoreAttribute") = False AndAlso field.DeclaringType.IsSerializable Then
                Return False
            End If

            Return True
        End Function
#End Region

    End Class
End Namespace

