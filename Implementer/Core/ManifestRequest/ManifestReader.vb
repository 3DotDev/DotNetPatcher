Imports System.IO
Imports Vestris.ResourceLib
Imports System.Xml

Namespace Core.ManifestRequest
    Friend NotInheritable Class ManifestReader

#Region " Methods "
        Friend Shared Function ExtractManifest(ByVal FilePath As String) As String
            Try
                Dim rc As New ManifestResource
                rc.LoadFrom(FilePath)
                Dim ManifestXmlDocument As XmlDocument = New XmlDocument()
                ManifestXmlDocument.LoadXml(rc.Manifest.OuterXml)
                Dim elemList As XmlNodeList = ManifestXmlDocument.GetElementsByTagName("requestedExecutionLevel")
                For i As Integer = 0 To elemList.Count - 1
                    If elemList(i).Attributes("level") IsNot Nothing Then
                        Return elemList(i).Attributes("level").Value
                    End If
                Next
            Catch ex As Exception
                Return "asInvoker"
            End Try
            Return "asInvoker"
        End Function
#End Region

    End Class

End Namespace

