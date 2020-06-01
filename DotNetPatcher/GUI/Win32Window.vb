Public Class Win32Window
    Implements IWin32Window

    Public Sub New(handle As IntPtr)
        Me.Handle = handle
    End Sub

    Public ReadOnly Property Handle As IntPtr Implements IWin32Window.Handle

End Class


