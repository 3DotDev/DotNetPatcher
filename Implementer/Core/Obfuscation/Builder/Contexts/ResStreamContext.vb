Imports System.IO
Imports Helper.RandomizeHelper
Imports Mono.Cecil

Namespace Core.Obfuscation.Builder

    Public Class ResStreamContext
        Inherits StubContext

#Region " Properties "
        Friend ReadOnly Property ResourceName As String
        Friend Property BinaryWriter As BinaryWriter
        Friend Property MemoryStream As MemoryStream
#End Region

#Region " Constructor "
        Public Sub New(InputA As AssemblyDefinition, PackerT As Boolean, Randomize As Randomizer, ResourceN As String)
            MyBase.New(InputA, PackerT, Randomize)
            _ResourceName = ResourceN
            _MemoryStream = New MemoryStream
            _BinaryWriter = New BinaryWriter(_MemoryStream)
        End Sub
#End Region

    End Class
End Namespace
