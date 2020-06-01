Imports System.IO

Public Class Frm_Result

#Region " Fields "
    Private ReadOnly m_FilePath$
#End Region

#Region " Constructor "
    Sub New(Title$, Message$, FilePath$)
        InitializeComponent()
        Frm_ResultThemeContainer.Text = Title
        LblResultMessage.Text = Message
        m_FilePath = FilePath
    End Sub
#End Region

#Region " Methods "

    Private Sub Frm_Result_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If Frm_ResultThemeContainer.Text.Contains("Error") Then
            PcbResultIcon.Image = My.Resources._error
            BtnResultOpenFileDir.Visible = False
            BtnResultOpenAssemblyViewer.Visible = False
        ElseIf Frm_ResultThemeContainer.Text.Contains("Success") Then
            PcbResultIcon.Image = My.Resources.Valid
            BtnResultOpenFileDir.Visible = True
            BtnResultOpenAssemblyViewer.Visible = True
        ElseIf Frm_ResultThemeContainer.Text.Contains("Warning") Then
            PcbResultIcon.Image = My.Resources.Warning
            BtnResultOpenFileDir.Visible = False
            BtnResultOpenAssemblyViewer.Visible = False
        ElseIf Frm_ResultThemeContainer.Text.Contains("Loading") Then
            PcbResultIcon.Image = My.Resources.Loading
            BtnResultOpenFileDir.Visible = False
            BtnResultOpenAssemblyViewer.Visible = False      
        End If
        PcbResultIcon.Visible = True
    End Sub

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnResultClose.Click
        Me.Close()
    End Sub

    Private Sub BtnResultOpenFileDir_Click(sender As Object, e As EventArgs) Handles BtnResultOpenFileDir.Click
        Dim fi As New FileInfo(m_FilePath)
        Process.Start(fi.DirectoryName)
        Me.Close()
    End Sub

    Private Sub BtnResultOpenAssemblyViewer_Click(sender As Object, e As EventArgs) Handles BtnResultOpenAssemblyViewer.Click
        Dim m_exclude = New Frm_Viewer
        With m_exclude
            .Title = "Assembly Viewer"
            .FilePath = m_FilePath
            .ShowDialog()
        End With
    End Sub

#End Region

End Class