Namespace Core.Dependencing
    Public Class DependenciesInfos
        Implements IDisposable

#Region " Enumerations "
        Enum EncryptType
            ByDefault = 0
            ToResources = 1
        End Enum

        Enum DependenciesAddMode
            Merged = 0
            Embedded = 1
        End Enum

        Enum CompressEncryptMode
            None = 0
            Encrypt = 1
            Compress = 2
            Both = 3
        End Enum
#End Region

#Region " Properties "
        Public ReadOnly Property Enabled As Boolean
        Public ReadOnly Property Dependencies As IEnumerable(Of String)
        Public ReadOnly Property DependenciesMode As DependenciesAddMode
        Public ReadOnly Property DependenciesCompressEncryptMode As CompressEncryptMode
#End Region

#Region " Constructor "
        Public Sub New(Enable As Boolean, Dependenc As IEnumerable(Of String), DependenciesMod As Boolean, DependenciesCompressEncryptMod%)
            _Enabled = Enable
            _Dependencies = Dependenc
            _DependenciesMode = DependenciesModeValue(DependenciesMod)
            _DependenciesCompressEncryptMode = DependenciesCompressEncryptModeValue(DependenciesCompressEncryptMod)
        End Sub
#End Region

#Region " Methods "
        Private Function DependenciesModeValue(boolValue As Boolean) As DependenciesAddMode
            Return If(boolValue, DependenciesAddMode.Embedded, DependenciesAddMode.Merged)
        End Function

        Private Function DependenciesCompressEncryptModeValue(intValue%) As CompressEncryptMode
            Select Case intValue
                Case 0
                    Return CompressEncryptMode.None
                Case 1
                    Return CompressEncryptMode.Encrypt
                Case 2
                    Return CompressEncryptMode.Compress
                Case 3
                    Return CompressEncryptMode.Both
            End Select
            Return CompressEncryptMode.None
        End Function
#End Region

#Region "IDisposable Support"
        Private disposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                End If
                _Enabled = False
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
