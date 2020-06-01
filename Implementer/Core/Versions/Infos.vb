Namespace Core.Versions
    Public Structure Infos

#Region " Properties "

        Public ReadOnly Property Enabled As Boolean
        Public ReadOnly Property FileDescription As String
        Public ReadOnly Property Comments As String
        Public ReadOnly Property CompanyName As String
        Public ReadOnly Property ProductName As String
        Public ReadOnly Property LegalCopyright As String
        Public ReadOnly Property LegalTrademarks As String
        Public ReadOnly Property FileVersion As String
        Public ReadOnly Property ProductVersion As String
#End Region

#Region " Constructor "
        Public Sub New(Enabl As Boolean, FileDescript$, Comment$, CompanyN$, ProductN$, LegalCopy$, LegalTrade$, FileV$, ProductV$)
            _Enabled = Enabl
            _FileDescription = FileDescript
            _Comments = Comment
            _CompanyName = CompanyN
            _ProductName = ProductN
            _LegalCopyright = LegalCopy
            _LegalTrademarks = LegalTrade
            _FileVersion = FileV
            _ProductVersion = ProductV
        End Sub

        Public Sub New(Enabl As Boolean, FilePath$)
            Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(FilePath)
            _Enabled = Enabl
            _FileDescription = fvi.FileDescription
            _Comments = fvi.Comments
            _CompanyName = fvi.CompanyName
            _ProductName = fvi.ProductName
            _LegalCopyright = fvi.LegalCopyright
            _LegalTrademarks = fvi.LegalTrademarks
            _FileVersion = fvi.FileVersion
            _ProductVersion = fvi.ProductVersion
        End Sub
#End Region
    End Structure
End Namespace
