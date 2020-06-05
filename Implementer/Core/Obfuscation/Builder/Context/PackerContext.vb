Imports System.IO
Imports System.Text
Imports Helper.CecilHelper
Imports Helper.CryptoHelper
Imports Helper.RandomizeHelper
Imports Helper.UtilsHelper
Imports Implementer.Core.Packer
Imports Mono.Cecil
Imports SevenZipLib

Namespace Core.Obfuscation.Builder

    Public Class PackerContext
        Inherits StubContext

#Region " Properties "
        Friend ReadOnly Property ResourceName As String
        Friend Property AssemblyTempFilePath As String
        Friend ReadOnly Property PolyXor As Crypt
        Friend ReadOnly Property Reverse As Boolean
        Friend ReadOnly Property ReferencedZipperAssembly As ZipInfos
        Friend ReadOnly Property EncryptedBytes As Byte()
#End Region

#Region " Constructor "
        Public Sub New(InputA As AssemblyDefinition, PackerT As Boolean, Randomize As Randomizer, AssemblyTempFilePath As String)
            MyBase.New(InputA, PackerT, Randomize)
            _ResourceName = GetEncodedFileName(AssemblyTempFilePath)
            _AssemblyTempFilePath = AssemblyTempFilePath
            _PolyXor = New Crypt()
            _Reverse = Randomizer.GenerateBoolean
            _ReferencedZipperAssembly = New ZipInfos(Functions.GetTempFolder & "\SevenzipLib.dll", My.Resources.SevenzipLib, "SevenZipLib", "SevenZipHelper", "Decompress")
        End Sub
#End Region

#Region " Methods "
        Private Function GetEncodedFileName(asmName As String) As String
            Dim asm = AssemblyDefinition.ReadAssembly(asmName)
            Dim compressedName = Convert.ToBase64String(Encoding.Default.GetBytes(asm.FullName.ToLower))
            compressedName &= ".resources"
            Return compressedName
        End Function

        Public Sub EncryptAndInjectToResources()
            _EncryptedBytes = _PolyXor.Encrypt(File.ReadAllBytes(AssemblyTempFilePath))
            If _Reverse Then Array.Reverse(_EncryptedBytes)
            Injecter.InjectResource(InputAssembly.MainModule, _ResourceName, ResourceType.Embedded, CompressWithSevenZip(_EncryptedBytes).ToArray)
        End Sub

        Private Function CompressWithSevenZip(raw As Byte()) As Byte()
            Return SevenZipHelper.Compress(raw)
        End Function
#End Region

    End Class
End Namespace
