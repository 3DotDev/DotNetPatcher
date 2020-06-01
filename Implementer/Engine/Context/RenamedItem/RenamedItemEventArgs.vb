Namespace Engine.Context

#Region " Delegates "
    Public Delegate Sub RenamedItemDelegate(sender As Object, e As RenamedItemEventArgs)
#End Region

    Public NotInheritable Class RenamedItemEventArgs
        Inherits EventArgs

#Region " Constructor "
        Public Sub New(item As RenamedItem)
            _Item = item
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property Item As RenamedItem
#End Region

    End Class
End Namespace
