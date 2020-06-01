Imports System.IO
Imports Mono.Cecil
Imports Helper.CecilHelper
Imports Implementer.Core.Dependencing.DependenciesInfos
Imports Helper.UtilsHelper
Imports Implementer.Core.Obfuscation.Builder
Imports Implementer.Core.Obfuscation.Protections

Namespace Core.Dependencing
    Public NotInheritable Class Embedding

#Region " Fields "
        Private ReadOnly Context As ProtectionContext
        Private ReadOnly StubContex As EmbedContext
#End Region

#Region " Constructor "
        Friend Sub New(Contex As ProtectionContext, PackerStat As Boolean)
            Context = Contex
            Dim Encrypt As Boolean
            Dim Compress As Boolean
            Select Case Contex.Params.TaskAccept.DllReferences.DependenciesCompressEncryptMode
                Case CompressEncryptMode.Both
                    Encrypt = True
                    Compress = True
                Case CompressEncryptMode.Compress
                    Compress = True
                Case CompressEncryptMode.Encrypt
                    Encrypt = True
            End Select

            StubContex = New EmbedContext(Contex.InputAssembly, PackerStat, Contex.Randomizer, Compress, Encrypt)
        End Sub

#End Region

#Region " Methods "
        Friend Sub CreateResolverClass()
            Try
                Dim ResourcesResolver As New ResourcesResolver(StubContex)
                ResourcesResolver.RemoveStubFile()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End Sub

        Friend Sub InjectFiles()
            For Each f In Context.Params.TaskAccept.DllReferences.Dependencies
                Dim assname = AssemblyDefinition.ReadAssembly(f)
                Dim resNameGUID = CStr(assname.FullName.GetHashCode)
                If StubContex.PackerTask Then resNameGUID = Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(resNameGUID))

                Dim encryptedBytes = File.ReadAllBytes(f)
                Dim resourceName = String.Format("{0}.{1}", resNameGUID, "resources")

                If StubContex.ResourceEncrypt Then Array.Reverse(encryptedBytes)
                If StubContex.ResourceCompress Then encryptedBytes = Functions.GZipedByte(encryptedBytes)

                Injecter.InjectResource(StubContex.InputAssembly.MainModule, resourceName, ResourceType.Embedded, encryptedBytes)
            Next
        End Sub
#End Region

    End Class
End Namespace
