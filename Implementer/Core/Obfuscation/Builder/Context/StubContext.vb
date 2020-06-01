Imports Helper.CecilHelper
Imports Helper.RandomizeHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder

    Public Class StubContext

#Region " Properties "
        Friend Property InputAssembly As AssemblyDefinition
        Friend ReadOnly Property FrameworkVersion As String
        Friend ReadOnly Property PackerTask As Boolean
        Friend ReadOnly Property Randomizer As Randomizer
#End Region

#Region " Constructor "
        Public Sub New(InputA As AssemblyDefinition, PackerT As Boolean, Randomize As Randomizer)
            _InputAssembly = InputA
            _FrameworkVersion = Finder.FindFrameworkVersion(InputAssembly)
            _PackerTask = PackerT
            _Randomizer = Randomize
        End Sub
#End Region

    End Class
End Namespace
