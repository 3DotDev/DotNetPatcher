Imports System.IO
Imports System.Resources
Imports Helper.RandomizeHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder

    Public Class ResManagerContext
        Inherits StubContext

#Region " Properties "
        Friend ReadOnly Property ResourceName As String
        Friend Property ResourceWriter As ResourceWriter
        Friend ReadOnly Property ResourceNamePath As String
#End Region

#Region " Constructor "
        Public Sub New(InputA As AssemblyDefinition, PackerT As Boolean, Randomize As Randomizer, ResourceN As String)
            MyBase.New(InputA, PackerT, Randomize)
            _ResourceName = ResourceN
            _ResourceNamePath = Path.Combine(Path.GetTempPath, ResourceN & ".resources")
            _ResourceWriter = New ResourceWriter(_ResourceNamePath)
        End Sub
#End Region

    End Class
End Namespace
