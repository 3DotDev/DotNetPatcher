Namespace Core.ManifestRequest
    Public Class ManifestInfos
        Implements IDisposable

#Region " Properties "
        Public ReadOnly Property LastRequested As String
        Public ReadOnly Property NewRequested As String

        Public ReadOnly Property Modified() As Boolean
            Get
                Return If(_LastRequested <> "" AndAlso _NewRequested <> "", _LastRequested <> _NewRequested, False)
            End Get
        End Property
#End Region

#Region " Constructor "
        Public Sub New(LastRequest$, NewRequest$)
            _LastRequested = LastRequest
            _NewRequested = NewRequest
        End Sub
#End Region

#Region " Methods "
        Private Sub CleanUp()
            _LastRequested = String.Empty
            _NewRequested = String.Empty
        End Sub
#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                End If
                CleanUp()
            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

End Namespace

