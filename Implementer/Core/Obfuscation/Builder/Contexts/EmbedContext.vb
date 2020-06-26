Imports Helper.RandomizeHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder

    Public Class EmbedContext
        Inherits StubContext

#Region " Properties "
        Friend Property ResourceCompress As Boolean
        Friend Property ResourceEncrypt As Boolean
#End Region

#Region " Constructor "
        Public Sub New(InputA As AssemblyDefinition, PackerT As Boolean, Randomize As Randomizer, Compress As Boolean, Encrypt As Boolean)
            MyBase.New(InputA, PackerT, Randomize)
            _ResourceCompress = Compress
            _ResourceEncrypt = Encrypt
        End Sub
#End Region

    End Class
End Namespace
