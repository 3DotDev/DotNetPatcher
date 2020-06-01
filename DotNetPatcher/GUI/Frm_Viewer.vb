Imports System.IO
Imports System.ComponentModel
Imports Implementer.Core.Obfuscation.Viewer

Public Class Frm_Viewer

#Region " Fields "
    Private ReadOnly m_FilePath$ = String.Empty
    Private TreeviewHandler As TreeviewHandler
    Private TreeNode As TreeNode
    Private m_FileSize As Long
#End Region

#Region " Properties "
    Public Property Title As String
    Public Property FilePath As String
#End Region

#Region " Constructor "
    Sub New()
        InitializeComponent()
    End Sub
#End Region

#Region " Methods "
    Private Sub Frm_Exclusion_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Frm_ExclusionThemeContainer.Text = "Assembly Viewer"
        If BgwExclusion.IsBusy = False Then
            BgwExclusion.RunWorkerAsync()
        Else
            BgwExclusion.CancelAsync()
            BgwExclusion.RunWorkerAsync()
        End If
    End Sub

    Private Sub BgwExclusion_DoWork(sender As Object, e As DoWorkEventArgs) Handles BgwExclusion.DoWork
        BgwExclusion.ReportProgress(101, Nothing)
        Dim r As New Frm_Result("Loading", "Please wait while loading ...", "")
        BgwExclusion.ReportProgress(102, r)
        If New FileInfo(_FilePath).Length = m_FileSize Then
            Already(r, e)
        Else
            Success(r, e)
        End If
    End Sub

    Private Sub Success(r As Frm_Result, e As DoWorkEventArgs, Optional ByVal DeleteDirectory As Boolean = False)
        TreeviewHandler = New TreeviewHandler(_FilePath)
        BgwExclusion.ReportProgress(103, r)
        e.Result = New Object() {"Success", TreeviewHandler.LoadTreeNode, Nothing, DeleteDirectory}
        m_FileSize = New FileInfo(_FilePath).Length
    End Sub

    Private Sub Already(r As Frm_Result, e As DoWorkEventArgs, Optional ByVal DeleteDirectory As Boolean = False)
        BgwExclusion.ReportProgress(103, r)
        e.Result = New Object() {"Already", TreeNode, Nothing, DeleteDirectory}
    End Sub

    Private Sub BgwExclusion_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BgwExclusion.ProgressChanged
        If e.ProgressPercentage = 101 Then
            TvExclusion.Nodes.Clear()
        ElseIf e.ProgressPercentage = 102 Then
            TryCast(e.UserState, Frm_Result).ShowDialog()
        ElseIf e.ProgressPercentage = 103 Then
            TryCast(e.UserState, Frm_Result).Close()
        End If
    End Sub

    Private Sub BgwExclusion_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BgwExclusion.RunWorkerCompleted
        If Not e.Result Is Nothing Then
            Select Case e.Result(0)
                Case "Success", "Already"
                    TvExclusion.Nodes.Add(e.Result(1))
                    TvExclusion.Nodes(0).Expand()
                    TvExclusion.Nodes(0).Nodes(0).Expand()
                    TvExclusion.Nodes(0).Nodes(0).Nodes(0).Expand()
            End Select
        End If
    End Sub
#End Region

End Class