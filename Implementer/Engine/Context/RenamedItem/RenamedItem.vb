Namespace Engine.Context

    Public Structure RenamedItem

#Region " Properties "
        Public ReadOnly Property ItemType As String
        Public ReadOnly Property ItemName As String
        Public ReadOnly Property ObfuscatedItemName As String
#End Region

#Region " Constructor "
        Friend Sub New(ItemType As RenamedItemType.ItemType, ItemName$, obfuscatedItemName$)
            _ItemType = ItemType
            _ItemName = ItemName
            _ObfuscatedItemName = obfuscatedItemName
        End Sub
#End Region

    End Structure
End Namespace
