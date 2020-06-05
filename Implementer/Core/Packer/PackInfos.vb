Imports Helper.UtilsHelper
Imports Implementer.Core.IconChanger
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace Core.Packer
    Public Structure PackInfos

#Region " Properties "
        Public ReadOnly Property Enabled As Boolean
        Public ReadOnly Property NewIcon As Icon
        Public ReadOnly Property RequestedLevel As String
#End Region

#Region " Constructor "
        Public Sub New(Enable As Boolean, NewIconP As String, RequestedL As String)
            _Enabled = Enable
            _NewIcon = NewIconValue(NewIconP)
            _RequestedLevel = RequestedL
        End Sub
#End Region

#Region " Methods "
        Private Function NewIconValue(fPath As String) As Icon
            If fPath.ToLower.EndsWith(".ico") Then Return New Icon(fPath)
            Return New IconInfos(fPath).NewIcon
        End Function
#End Region

    End Structure
End Namespace
