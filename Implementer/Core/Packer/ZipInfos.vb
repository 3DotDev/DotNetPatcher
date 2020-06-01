Namespace Core.Packer
    Public Structure ZipInfos

#Region " Properties "
        Friend ReadOnly Property FPath As String
        Friend ReadOnly Property RefByte As Byte()
        Friend ReadOnly Property RefNewNamespaceName As String
        Friend ReadOnly Property RefNewTypeName As String
        Friend ReadOnly Property RefNewMethodName As String
#End Region

#Region " Constructor "
        Friend Sub New(filePath$, rByte As Byte(), refNewNamespaceName$, refNewTypeName$, refNewMethodName$)
            _FPath = filePath
            _RefByte = rByte
            _RefNewNamespaceName = refNewNamespaceName
            _RefNewTypeName = refNewTypeName
            _RefNewMethodName = refNewMethodName
        End Sub
#End Region

    End Structure
End Namespace
