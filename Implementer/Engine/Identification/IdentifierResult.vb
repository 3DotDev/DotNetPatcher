Imports System.Drawing

Namespace Engine.Identification

    Public NotInheritable Class IdentifierResult

#Region " Fields "
        Private ReadOnly m_itemName As IdentifierEnum.ResultName
        Private ReadOnly m_itemType As IdentifierEnum.ResultType
#End Region

#Region " Constructor "
        Friend Sub New(ItemName As IdentifierEnum.ResultName, ItemType As IdentifierEnum.ResultType, pic As Bitmap)
            m_itemName = ItemName
            m_itemType = ItemType
            Me.Pic = pic
        End Sub
#End Region

#Region " Properties "
        Public ReadOnly Property ItemType As String
            Get
                Return ReturnType(m_itemType)
            End Get
        End Property

        Public ReadOnly Property ItemName As String
            Get
                Return ReturnName(m_itemName)
            End Get
        End Property

        Public ReadOnly Property Pic As Bitmap
#End Region

#Region " Methods "
        Private Function ReturnName(itName As IdentifierEnum.ResultName) As String
            Select Case itName
                Case 0
                    Return "Unknown"
                Case 1
                    Return "Confuser"
                Case 2
                    Return "Rpx"
                Case 3
                    Return "DotNetPatcher"
                Case 4
                    Return "MPress"
                Case 5
                    Return "Netz"
                Case 6
                    Return "ILProtector"
                Case 7
                    Return "NetPack"
                Case 8
                    Return "NetShrink"
                Case 9
                    Return "DotBundle"
                Case 10
                    Return "SmartAssembly"
                Case 11
                    Return "BabelObfuscator"
                Case 12
                    Return "AgileDotNet"
                Case 13
                    Return "CodeVeil"
                Case 14
                    Return "CryptoObfuscator"
                Case 15
                    Return "DotFuscator"
                Case 16
                    Return "GoliathDotNet"
                Case 17
                    Return "SpicesDotNet"
                Case 18
                    Return "XenocodeObfuscator"
                Case 19
                    Return "SkaterDotNetObfuscator"
                Case 20
                    Return "MancoDotNetObfuscator"
                Case 21
                    Return "CodeFortObfuscator"
                Case 22
                    Return "PhoenixProtector"
                Case 23
                    Return "MacrobjectObfuscator"
                Case 24
                    Return "EazfuscatorDotNet"
                Case 25
                    Return "DotWall"
                Case 26
                    Return "Unidentified"
            End Select
            Return "Unknown"
        End Function

        Private Function ReturnType(itType As IdentifierEnum.ResultType) As String
            Select Case itType
                Case 0
                    Return "Other"
                Case 1
                    Return "Packer"
                Case 2
                    Return "Obfuscator"
                Case 3
                    Return "Empty"
            End Select
            Return "Other"
        End Function
#End Region

    End Class
End Namespace
