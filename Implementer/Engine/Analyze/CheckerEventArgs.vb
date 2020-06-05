Namespace Engine.Analyze

#Region " Delegates "
    Public Delegate Sub Check(sender As Object, e As CheckEventArgs)
#End Region

    Public NotInheritable Class CheckEventArgs
        Inherits EventArgs

#Region " Properties "
        Public ReadOnly Property Message As String
        Public ReadOnly Property Title As String
        Public ReadOnly Property CheckedFile As String
#End Region

#Region " Constructor "
        Public Sub New(message As String, title As String, checkedFile As String)
            _Message = message
            _Title = title
            _CheckedFile = checkedFile
        End Sub
#End Region

    End Class
End Namespace
