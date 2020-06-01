Imports Helper.ResourcesHelper

Namespace Core.Versions
    Friend NotInheritable Class Extractor

#Region " Methods "
        Friend Shared Sub SaveFileInfoTo(ByVal FileName As String, ByVal ResPath As Byte())
            ResourceEx.UpdateResourceEx(FileName, 1, 16, ResPath)
        End Sub

        Friend Shared Function LoadFileInfoFrom(ByVal FileName As String) As Byte()
            Return ResourceEx.ExtractResourceEx(FileName, "1", New IntPtr(16))
        End Function

        Friend Shared Function ReturnAssemblyInfosFormat(ByVal filename As String) As String
            Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(filename)
            Return "<" & "Assembly: AssemblyTitle(""" & fvi.FileDescription & """)>" & vbNewLine _
          & "<Assembly: AssemblyDescription(""" & fvi.Comments & """)>" & vbNewLine _
          & "<" & "Assembly: AssemblyCompany(""" & fvi.CompanyName & """)>" & vbNewLine _
          & "<Assembly: AssemblyProduct(""" & fvi.ProductName & """)>" & vbNewLine _
          & "<Assembly: AssemblyCopyright(""" & fvi.LegalCopyright & """)>" & vbNewLine _
          & "<" & "Assembly: AssemblyTrademark(""" & fvi.LegalTrademarks & """)>" & vbNewLine _
          & "<Assembly: AssemblyVersion(""" & fvi.FileVersion & """)>" & vbNewLine _
          & "<Assembly: AssemblyFileVersion(""" & fvi.ProductVersion & """)>" & vbNewLine & vbNewLine
        End Function
#End Region

    End Class
End Namespace