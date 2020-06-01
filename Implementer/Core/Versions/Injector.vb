Imports Mono.Cecil
Imports System.Reflection
Imports Helper.CecilHelper
Imports Vestris.ResourceLib

Namespace Core.Versions
    Friend NotInheritable Class Injector

#Region " Fields "
        Private Shared ReadOnly langId As Integer()
#End Region

#Region " Constructor "
        Shared Sub New()
            langId = New Integer(1) {0, 1033}
        End Sub
#End Region

#Region " Methods "
        Private Shared Sub InjectAssemblyInfos(SelectedFilePath$, vInfos As Infos)
            Try
                Dim AssDefTarget = AssemblyDefinition.ReadAssembly(SelectedFilePath)
                Dim AttribInfos As New Dictionary(Of Type, String) From {
                                                                    {GetType(AssemblyTitleAttribute), vInfos.FileDescription},
                                                                    {GetType(AssemblyDescriptionAttribute), vInfos.Comments},
                                                                    {GetType(AssemblyCompanyAttribute), vInfos.CompanyName},
                                                                    {GetType(AssemblyProductAttribute), vInfos.ProductName},
                                                                    {GetType(AssemblyCopyrightAttribute), vInfos.LegalCopyright},
                                                                    {GetType(AssemblyTrademarkAttribute), vInfos.LegalTrademarks},
                                                                    {GetType(AssemblyFileVersionAttribute), vInfos.FileVersion},
                                                                    {GetType(AssemblyVersionAttribute), vInfos.ProductVersion}}

                For Each info In AttribInfos
                    Utils.RemoveCustomAttributeByName(AssDefTarget, info.Key.Name)
                    Injecter.InjectAssemblyInfoCustomAttribute(AssDefTarget, info.Key, info.Value)
                Next

                AssDefTarget.Write(SelectedFilePath)
            Catch ex As Exception
                MsgBox("InjectAssemblyInfos Exception : " & ex.ToString)
            End Try

        End Sub

        Private Shared Sub InjectVersionInfos(SelectedFilePath$, vInfos As Infos)

            Try
                DeleteVersionFromLangId(SelectedFilePath)

                Dim versionResource As New VersionResource()
                With versionResource
                    .FileVersion = vInfos.FileVersion
                    .ProductVersion = vInfos.ProductVersion

                    Dim stringFileInfo As New StringFileInfo()
                    versionResource(stringFileInfo.Key) = stringFileInfo

                    Dim stringFileInfoStrings As New StringTable With {
                        .LanguageID = 1033,
                        .CodePage = 1252
                    }
                    stringFileInfo.Strings.Add(stringFileInfoStrings.Key, stringFileInfoStrings)
                    stringFileInfoStrings("ProductName") = vInfos.ProductName
                    stringFileInfoStrings("FileDescription") = vInfos.FileDescription
                    stringFileInfoStrings("CompanyName") = vInfos.CompanyName
                    stringFileInfoStrings("LegalCopyright") = vInfos.LegalCopyright
                    stringFileInfoStrings("LegalTrademarks") = vInfos.LegalTrademarks
                    stringFileInfoStrings("Comments") = vInfos.Comments
                    stringFileInfoStrings("FileVersion") = versionResource.FileVersion
                    stringFileInfoStrings("ProductVersion") = versionResource.ProductVersion

                    Dim varFileInfo As New VarFileInfo()
                    versionResource(varFileInfo.Key) = varFileInfo
                    Dim varFileInfoTranslation As New VarTable("Translation")
                    varFileInfo.Vars.Add(varFileInfoTranslation.Key, varFileInfoTranslation)
                    varFileInfoTranslation(ResourceUtil.NEUTRALLANGID) = 1252

                    .SaveTo(SelectedFilePath)

                End With
            Catch ex As Exception
                MsgBox("InjectVersionInfos Exception : " & ex.ToString)
            End Try
        End Sub

        Friend Shared Sub InjectAssemblyVersionInfos(SelectedFilePath$, vInfos As Infos)
            InjectVersionInfos(SelectedFilePath, vInfos)
            InjectAssemblyInfos(SelectedFilePath, vInfos)
        End Sub

        Private Shared Sub DeleteVersionFromLangId(SelectedFilePath$)
            Dim OldversionResource As New VersionResource()
            For Each id In langId
                Try
                    With OldversionResource
                        .Language = id
                        .LoadFrom(SelectedFilePath)
                        .DeleteFrom(SelectedFilePath)
                    End With
                Catch ex As Exception
                End Try
            Next

        End Sub
#End Region

    End Class
End Namespace