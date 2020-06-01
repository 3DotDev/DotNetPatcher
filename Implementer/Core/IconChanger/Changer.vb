Imports System.IO
Imports System.Drawing
Imports Implementer.Engine.Checking
Imports Helper.UtilsHelper

Namespace Core.IconChanger
    Public NotInheritable Class Changer

#Region " Events "
        Public Event CheckerResult As Check
#End Region

#Region " Methods "
        Private Sub RaiseCheckerResultEvent(message As String, title As String, addedFiles As String)
            Dim itemEvent As New CheckEventArgs(message, title, addedFiles)
            RaiseEvent CheckerResult(Nothing, itemEvent)
        End Sub

        Public Sub SelectingIcon(ByVal FileName As String)
            If New FileInfo(FileName).Length = 0 Then
                RaiseCheckerResultEvent("The file : " & New FileInfo(FileName).Name & " is empty !", "Bad size", "")
            Else
                Try
                    If Functions.isValid(FileName, Imaging.ImageFormat.Icon) Then
                        RaiseCheckerResultEvent("File Added", "Operation Completed", FileName)
                    Else
                        RaiseCheckerResultEvent("Unsupported selected file", "Error", "")
                    End If
                Catch ex As Exception
                    RaiseCheckerResultEvent(ex.Message, "Error", "")
                    Exit Sub
                End Try
            End If
        End Sub
#End Region

    End Class
End Namespace
