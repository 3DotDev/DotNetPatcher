Imports Helper.RandomizeHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder

    Public Class CompressContext
        Inherits StubContext

#Region " Properties "
        Friend ReadOnly Property ResourceName As String
        Friend Property ResourceCompress As Boolean
        Friend Property ResourceEncrypt As Boolean
#End Region

#Region " Constructor "
        Public Sub New(InputA As AssemblyDefinition, PackerT As Boolean, Randomize As Randomizer, ResourceN As String, Compress As Boolean, Encrypt As Boolean)
            MyBase.New(InputA, PackerT, Randomize)
            _ResourceName = ResourceN
            _ResourceCompress = Compress
            _ResourceEncrypt = Encrypt
        End Sub
#End Region

    End Class
End Namespace
