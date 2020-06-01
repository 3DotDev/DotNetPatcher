Imports System.IO
Imports Vestris.ResourceLib
Imports Helper.UtilsHelper

Namespace Core.ManifestRequest
    Friend NotInheritable Class ManifestWriter

#Region " Fields "
        Private Shared ReadOnly m_xmlstart As String =
          "<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>" & vbNewLine &
          "<assembly xmlns=""urn:schemas-microsoft-com:asm.v1"" manifestVersion=""1.0"">" & vbNewLine &
          "    <assemblyIdentity version=""1.0.0.0"" name=""MyApplication.app""/>" & vbNewLine &
          "    <trustInfo xmlns=""urn:schemas-microsoft-com:asm.v2"">" & vbNewLine &
          "        <security>" & vbNewLine &
          "            <requestedPrivileges xmlns=""urn:schemas-microsoft-com:asm.v3"">"
        Private Shared _xmlmiddle As String = "            <requestedExecutionLevel  level=""asInvoker""  uiAccess=""false"" />"
        Private Shared ReadOnly m_xmlEnd As String =
                "           </requestedPrivileges>" & vbNewLine &
                "        </security>" & vbNewLine &
                "    </trustInfo>" & vbNewLine &
                "</assembly>" & vbNewLine &
                "<!-- DNP4 -->"
#End Region

#Region " Methods "
        Friend Shared Sub ApplyManifest(FilePath As String, PrivilegeName As String)
            WriteXmlFile(PrivilegeName)
            If File.Exists(Functions.GetTempFolder & "\DNMP.txt") Then
                Dim Res As New GenericResource(New ResourceId(Kernel32.ResourceTypes.RT_MANIFEST), New ResourceId(CUInt(1)), CUShort(0)) With {
                    .Data = IO.File.ReadAllBytes(Functions.GetTempFolder & "\DNMP.txt"),
                    .Name = New ResourceId(CUInt(1))
                }
                Res.SaveTo(FilePath)
                File.Delete(Functions.GetTempFolder & "\DNMP.txt")
            End If
        End Sub

        Private Shared Sub WriteXmlFile(PrivilegeName As String)
            Dim s As Stream = New FileStream(Functions.GetTempFolder & "\DNMP.txt", FileMode.Create, FileAccess.Write)
            Dim writer As New StreamWriter(s, Text.Encoding.UTF8)

            Select Case PrivilegeName
                Case "asInvoker"
                    _xmlmiddle = "            <requestedExecutionLevel  level=""asInvoker""  uiAccess=""false"" />"
                Case "requireAdministrator"
                    _xmlmiddle = "            <requestedExecutionLevel  level=""requireAdministrator""  uiAccess=""false"" />"
                Case "highestAvailable"
                    _xmlmiddle = "            <requestedExecutionLevel  level=""highestAvailable""  uiAccess=""false"" />"
            End Select

            writer.WriteLine(m_xmlstart)
            writer.WriteLine(_xmlmiddle)
            writer.WriteLine(m_xmlEnd)
            writer.Flush()
            s.Close()
            s.Dispose()
        End Sub
#End Region

    End Class

End Namespace

