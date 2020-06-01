Imports System.Reflection
Imports System.IO

Namespace AssemblyHelper

    <Serializable>
    Public Class Infos
        Implements IAssemblyInfos
        Implements IDisposable

#Region " Methods "

        Private Sub AssemblyInfo(assemblyBuffer As Byte(), ByRef FrmwkVersion$, ByRef AssVersion$, ByRef IsWpfApp As Boolean, ByRef EntryPoint As MethodInfo, ByRef AssemblyReferences As AssemblyName(), ByRef ManifestResourceNames As IEnumerable(Of String), ByRef ManifestResourceStreams As List(Of Stream), ByRef TypesClass As IEnumerable(Of Type), ByRef HasSerializableAttribute As Boolean, ByRef Result As Data.Message, Optional ByVal LoadMaxInfos As Boolean = False)
            Dim assembly As Assembly
            Try
                assembly = AppDomain.CurrentDomain.Load(assemblyBuffer)
                Dim manifest = assembly.ManifestModule
                AssVersion = assembly.GetName.Version.ToString()

                Dim frameworkName = String.Empty
                Dim frameworkDisplayName = String.Empty
                Dim customAttributes = assembly.GetCustomAttributesData()
                For Each att In customAttributes
                    For Each attca In att.NamedArguments
                        If attca.MemberInfo.Name.ToString = "FrameworkDisplayName" Then
                            If att.ConstructorArguments.Count <> 0 Then
                                If Not att.ConstructorArguments(0).Value Is Nothing Then
                                    If att.ConstructorArguments(0).Value.ToString().ToLower.Contains(",version=") Then
                                        FrmwkVersion = att.ConstructorArguments(0).Value.ToString().Split("=")(1).Replace(",Client", String.Empty).Replace(",Profile", String.Empty).Trim
                                        Exit For
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next

                Dim isWpfProg = assembly.GetReferencedAssemblies().Any(Function(x) x.Name.ToLower = "system.xaml") AndAlso
        assembly.GetManifestResourceNames().Any(Function(x) x.ToLower.EndsWith(".g.resources"))

                IsWpfApp = isWpfProg
                EntryPoint = assembly.EntryPoint
                AssemblyReferences = assembly.GetReferencedAssemblies

                If LoadMaxInfos = True Then

                    ManifestResourceNames = assembly.GetManifestResourceNames

                    For Each r In ManifestResourceNames
                        Dim resourceStream As Stream = assembly.GetManifestResourceStream(r)
                        If Not resourceStream Is Nothing Then
                            ManifestResourceStreams.Add(resourceStream)
                        End If
                    Next

                    TypesClass = assembly.ManifestModule.GetTypes
                    If Not TypesClass Is Nothing Then
                        TypesClass = assembly.GetTypes.Where(Function(t) t.IsClass)
                        For Each typ In TypesClass
                            If HasSerializableAttribute Then Exit For
                            If typ.Attributes.HasFlag(TypeAttributes.Serializable) Then
                                HasSerializableAttribute = True
                                Exit For
                            End If
                        Next
                    End If
                End If
                Result = Data.Message.Success
            Catch ex As ReflectionTypeLoadException
                Result = Data.Message.Failed
            Catch ex As FileNotFoundException
                Result = Data.Message.Failed
            Catch ex As FileLoadException
                Result = Data.Message.Failed
            Catch ex As NotSupportedException
                Result = Data.Message.Failed
            Catch ex As BadImageFormatException
                Result = Data.Message.Failed
            Finally

            End Try
        End Sub

        Public Sub GetAssemblyInfo(assembly As Byte(), ByRef FrmwkVersion$, ByRef AssVersion$, ByRef IsWpfApp As Boolean, ByRef EntryPoint As MethodInfo, ByRef AssemblyReferences As AssemblyName(), ByRef ManifestResourceNames As IEnumerable(Of String), ByRef ManifestResourceStreams As List(Of Stream), ByRef TypesClass As IEnumerable(Of Type), ByRef HasSerializableAttribute As Boolean, ByRef Result As Data.Message, Optional ByVal LoadMaxInfos As Boolean = False) Implements IAssemblyInfos.GetAssemblyInfo
            AssemblyInfo(assembly, FrmwkVersion, AssVersion, IsWpfApp, EntryPoint, AssemblyReferences, ManifestResourceNames, ManifestResourceStreams, TypesClass, HasSerializableAttribute, Result, LoadMaxInfos)
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' Pour détecter les appels redondants

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: supprimer l'état managé (objets managés).
                End If
                AppDomain.Unload(AppDomain.CurrentDomain)
                ' TODO: libérer les ressources non managées (objets non managés) et remplacer Finalize() ci-dessous.
                ' TODO: définir les champs de grande taille avec la valeur Null.
            End If
            disposedValue = True
        End Sub

        ' TODO: remplacer Finalize() seulement si la fonction Dispose(disposing As Boolean) ci-dessus a du code pour libérer les ressources non managées.
        'Protected Overrides Sub Finalize()
        '    ' Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(disposing As Boolean) ci-dessus.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Ce code est ajouté par Visual Basic pour implémenter correctement le modèle supprimable.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(disposing As Boolean) ci-dessus.
            Dispose(True)
            ' TODO: supprimer les marques de commentaire pour la ligne suivante si Finalize() est remplacé ci-dessus.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region

#End Region

    End Class

End Namespace
