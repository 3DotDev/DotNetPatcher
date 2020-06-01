Imports System.IO
Imports System.Drawing
Imports dnlib

Namespace Core.IconChanger
    Public Class IconInfos
        Implements IDisposable

#Region " Fields "
        Private m_tmpExePath As String
#End Region

#Region " Properties "
        Public ReadOnly Property Enabled As Boolean
        Public ReadOnly Property NewIcon As Icon
#End Region

#Region " Constructor "
        Public Sub New(Enable As Boolean, NewIconPath$)
            _Enabled = Enable
            _NewIcon = If(File.Exists(NewIconPath), New Icon(NewIconPath), Nothing)
            _Enabled = CheckNewIconExists()
        End Sub

        Public Sub New(FilePath As String)
            _NewIcon = FromExeFile(FilePath)
        End Sub
#End Region

#Region " Methods "
        Private Function FromExeFile(FilePath As String) As Icon
            If File.Exists(FilePath) Then
                m_tmpExePath = Path.Combine(Path.GetTempPath, Guid.NewGuid.ToString.Replace("-", "") & ".exe")
                File.Copy(FilePath, m_tmpExePath, True)
                Dim ic As New PeReader(m_tmpExePath)
                Return ic.GetMainIcon
            End If
            _Enabled = False
            Return Nothing
        End Function

        Private Function CheckNewIconExists() As Boolean
            If _Enabled Then
                If _NewIcon IsNot Nothing Then
                    Return True
                End If
            End If
            Return False
        End Function
#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                End If
                _Enabled = False
                _NewIcon = Nothing
                Try
                    File.Delete(m_tmpExePath)
                Catch ex As Exception
                End Try
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
