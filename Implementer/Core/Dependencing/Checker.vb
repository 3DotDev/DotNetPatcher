Imports System.IO
Imports System.Windows.Forms
Imports Helper.UtilsHelper
Imports Implementer.Engine.Analyze
Imports dnlib
Imports System.Reflection

Namespace Core.Dependencing
    Public NotInheritable Class Checker

#Region " Events "
        Public Event CheckerResult As Check
#End Region

#Region " Fields "
        Private ReadOnly m_Lbx As ListBox
        Public ReadOnly Property DependencyHasSerializableAttribute As Boolean
#End Region

#Region " Constructor "
        Public Sub New(lbx As ListBox)
            m_Lbx = lbx
        End Sub
#End Region

#Region " Methods "
        Private Sub RaiseCheckerResultEvent(message As String, title As String, addedFiles As String)
            Dim itemEvent As New CheckEventArgs(message, title, addedFiles)
            RaiseEvent CheckerResult(Nothing, itemEvent)
        End Sub

        Public Sub AddReferences(filesToAdd As String())
            Try
                For Each f In filesToAdd
                    If New FileInfo(f).Length = 0 Then
                        RaiseCheckerResultEvent("The file : " & New FileInfo(f).Name & " is empty !", "Bad size", "")
                    Else
                        If ReferencesExists(New FileInfo(f).Name) Then
                            Exit Sub
                        End If
                        If m_Lbx.Items.Contains(f) Then
                            Exit Sub
                        End If

                        If Not Functions.IsValid(File.ReadAllBytes(f)) Then
                            RaiseCheckerResultEvent("The file : " & New FileInfo(f).Name & " isn't a Dynamic-Link Library !", "Bad file", "")
                        Else
                            Dim pe As New PeReader(f)

                            If pe.isExecutable Then
                                RaiseCheckerResultEvent("The file : " & New FileInfo(f).Name & " is a DotNet executable file !" & vbNewLine & "You had to choose a DotNet Dynamic-Link Library file !", "Bad DotNet file", "")
                            Else
                                If pe.IsManaged Then
                                    Try
                                        Dim AssemblyName As AssemblyName = AssemblyName.GetAssemblyName(f)

                                        If _DependencyHasSerializableAttribute = False Then
                                            Dim ass = Assembly.Load(AssemblyName)

                                            Dim TypesClass = ass.ManifestModule.GetTypes.Where(Function(t) t.IsClass)
                                            For Each typ In TypesClass
                                                If _DependencyHasSerializableAttribute Then Exit For
                                                If typ.Attributes.HasFlag(TypeAttributes.Serializable) Then
                                                    _DependencyHasSerializableAttribute = True
                                                    Exit For
                                                End If
                                            Next
                                        End If

                                        RaiseCheckerResultEvent("File Added", "Operation Completed", f)
                                    Catch ex As FileNotFoundException
                                        RaiseCheckerResultEvent("The file : " & New FileInfo(f).Name & " doesn't exist !", "Inexistant file", "")
                                    Catch ex As BadImageFormatException
                                        RaiseCheckerResultEvent("The file : " & New FileInfo(f).Name & " isn't a DotNet assembly or was probably modified by an obfuscator !", "Bad file", "")
                                    Catch ex As FileLoadException
                                        RaiseCheckerResultEvent("The file : " & New FileInfo(f).Name & " seems to be loaded somewhere else !", "Open file", "")
                                    End Try
                                End If

                            End If
                        End If
                    End If
                Next
            Catch ex As Exception
                RaiseCheckerResultEvent(ex.Message, "Error", "")
            End Try
        End Sub

        Private Function ReferencesExists(fileName As String) As Boolean
            Return m_Lbx.Items.Cast(Of String).ToList().Any(Function(x) New FileInfo(x.ToString).Name.ToLower = fileName.ToLower)
        End Function
#End Region

    End Class
End Namespace

