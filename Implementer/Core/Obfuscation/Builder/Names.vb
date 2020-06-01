Imports Helper.RandomizeHelper

Namespace Core.Obfuscation.Builder
    Public Class Names

#Region " Properties "
        Friend Property ClassName As String
        Friend Property Functions As List(Of String)
#End Region

#Region " Constructors "
        Public Sub New(NumberOfFunctions As Integer, Randomize As Randomizer)
            _ClassName = Randomize.GenerateNewAlphabetic
            _Functions = New List(Of String)
            For i As Integer = 0 To NumberOfFunctions - 1
                Functions.Add(Randomize.GenerateNewAlphabetic)
            Next
        End Sub

        Public Sub New(ClassName As String, Function1 As String)
            _ClassName = ClassName
            _Functions = New List(Of String) From {Function1}
        End Sub
#End Region

    End Class
End Namespace
