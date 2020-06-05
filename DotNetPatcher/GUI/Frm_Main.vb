Imports System.IO
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports LoginTheme.XertzLoginTheme
Imports Implementer.Engine.Analyze
Imports Implementer.Engine.Context
Imports Implementer.Engine.Identification
Imports Implementer.Core.Dependencing
Imports Implementer.Core.ManifestRequest
Imports Implementer.Core.IconChanger
Imports Implementer.Core.Packer
Imports Implementer.Core.Obfuscation.Protections
Imports Helper.AssemblyHelper

Public Class Frm_Main

#Region " ######### FIELDS ######### "
    Private WithEvents Param As Parameters
    Private WithEvents DependenciesChecker As Checker
    Private WithEvents IconChanger As Changer
    Private ReadOnly m_rdb As LogInRadioButton()
    Private m_Context As Tasks
    Private ReadOnly m_taskArgs As TaskState
    Private ReadOnly m_controlList As List(Of Control)
    Private m_taskIsRunning As Boolean
    Private m_lastRequested As String
    Private m_assemblyHasSerializableAttributes As Boolean
#End Region

#Region " ######### FRM MAIN ######### "

    Public Sub New()
        InitializeComponent()
        Frm_MainThemeContainer.Text = "DotNetPatcher " & My.Application.Info.Version.ToString
        ShowAboutInfos()
        m_rdb = New LogInRadioButton(2) {RdbManifestChangerAsInvoker, RdbManifestChangerRequireAdministrator, RdbManifestChangerHighestAvailable}
        m_taskArgs = New TaskState
        DependenciesChecker = New Checker(LbxDependenciesAdd)
        IconChanger = New Changer
    End Sub

    Private Sub Frm_Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If m_taskIsRunning Then
            MessageBox.Show("Please wait while renaming !", "Wait ...", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            e.Cancel = True
        End If
    End Sub

#End Region

#Region " ######### SELECT ASSEMBLY TASK ######### "
    Private Sub BtnSelectFile_Click(sender As Object, e As EventArgs) Handles BtnSelectFile.Click
        Using ofd = New OpenFileDialog
            With ofd
                .Title = "Select a DotNet program (VbNet, C#)"
                .Filter = "Exe|*.exe;*.exe"
                .CheckFileExists = True
                .Multiselect = False
                If .ShowDialog() = DialogResult.OK Then
                    ShowSelectedFileInfos(.FileName)
                End If
            End With
        End Using
    End Sub

    Private Sub TxbSelectedFile_DragEnter(sender As Object, e As DragEventArgs) Handles TxbSelectedFile.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Private Sub TxbSelectedFile_DragDrop(sender As Object, e As DragEventArgs) Handles TxbSelectedFile.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim MyFiles() As String
            MyFiles = e.Data.GetData(DataFormats.FileDrop)
            TxbSelectedFile.Text = MyFiles(0)
            ShowSelectedFileInfos(TxbSelectedFile.Text)
        End If
    End Sub

    Private Sub ShowSelectedFileInfos(FilePath As String)
        Try
            Param = New Parameters(FilePath, FilePath)
            If Param.IsValidFile Then
                TxbType.Text = Param.GetModuleKind
                TxbVersionInfo.Text = Param.GetAssemblyVersion
                TxbFrameworkInfo.Text = Param.GetRuntime
                TxbCpuTargetInfo.Text = Param.GetProcessArchitecture
                PbxSelectedFile.Image = Param.GetMainIcon
                TxbSelectedFile.Text = FilePath
                TxbPackerFramework.Text = TxbFrameworkInfo.Text
                TxbPackerPlatform.Text = TxbCpuTargetInfo.Text
                TxbPackerSystem.Text = TxbType.Text
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub OnFileValidationTeminated(sender As Object, e As ValidatedFile) Handles Param.FileValidated
        If e.IsValid Then
            GbxDetection.Enabled = True
            BtnStart.Enabled = True

            Dim result = Identifier.Search(e)

            With result
                TxbDetection.Text = .ItemName & If(.ItemType = "Empty", "", " (" & .ItemType & ")")
                PcbDetection.Image = .Pic
                PcbDetection.Tag = .Pic
                TbcTask.Enabled = True

                With e.PeInfos.GetVersionInfos
                    TxbVersionInfosTitle.Text = .FileDescription
                    TxbVersionInfosDescription.Text = .Comments
                    TxbVersionInfosCompany.Text = .CompanyName
                    TxbVersionInfosProduct.Text = .ProductName
                    TxbVersionInfosCopyright.Text = .LegalCopyright
                    TxbVersionInfosTrademark.Text = .LegalTrademarks
                    TxbVersionInfosVersion.Text = .FileVersion
                End With

                m_lastRequested = Param.GetExecutionLevel()
                m_rdb.Where(Function(x) x.Tag.ToString = m_lastRequested).First.Checked = True

                Select Case .ItemType
                    Case "Packer", "Obfuscator", "Other"
                        Modified()
                        Exit Select
                    Case "Empty"
                        Dim infos As IDataFull = Loader.Full(e.Assembly.Location)
                        m_assemblyHasSerializableAttributes = infos.HasSerializableAttribute
                        UnModified()
                        Exit Select
                End Select
            End With
        Else
            GbxDetection.Enabled = False
            TbcTask.Enabled = False
            BtnStart.Enabled = False
            EmptyTextBox()
        End If
    End Sub

    Private Sub Modified()
        With TbcTask.TabPages
            .Remove(TpPacker)
            .Remove(TpObfuscator)
            .Remove(TpDependencies)
        End With

        ChbVersionInfosEnabled.Checked = True
        ChbVersionInfosEnabled.Enabled = True
        PnlVersionInfosEnabled.Enabled = True
        ChbDependenciesEnabled.Checked = False
        ChbDependenciesEnabled.Enabled = False
        PnlDependenciesEnabled.Enabled = False
        ChbManifestEnabled.Checked = True
        ChbManifestEnabled.Enabled = True
        PnlManifestEnabled.Enabled = True
        ChbObfuscatorEnabled.Checked = False
        ChbObfuscatorEnabled.Enabled = False
        PnlObfuscatorEnabled.Enabled = False
        ChbPackerEnabled.Checked = False
        ChbPackerEnabled.Enabled = False
        PnlPackerEnabled.Enabled = False
        ChbIconChangerEnabled.Checked = True
        ChbIconChangerEnabled.Enabled = True
        PnlIconChangerEnabled.Enabled = True
        CbxDependenciesEmbedded.Visible = False
        LbxDependenciesAdd.Items.Clear()
    End Sub

    Private Sub UnModified()
        With TbcTask
            If Not .TabPages.Contains(TpDependencies) Then .TabPages.Add(TpDependencies)
            If Not .TabPages.Contains(TpObfuscator) Then .TabPages.Add(TpObfuscator)
            If Not .TabPages.Contains(TpPacker) Then .TabPages.Add(TpPacker)
            .SelectedTab = TpAbout
        End With

        ChbVersionInfosEnabled.Checked = True
        ChbVersionInfosEnabled.Enabled = True
        PnlVersionInfosEnabled.Enabled = True
        ChbDependenciesEnabled.Checked = True
        ChbDependenciesEnabled.Enabled = True
        PnlDependenciesEnabled.Enabled = True
        ChbManifestEnabled.Checked = True
        ChbManifestEnabled.Enabled = True
        PnlManifestEnabled.Enabled = True
        ChbObfuscatorEnabled.Checked = True
        ChbObfuscatorEnabled.Enabled = True
        PnlObfuscatorEnabled.Enabled = True
        ChbPackerEnabled.Checked = False
        ChbPackerEnabled.Enabled = True
        PnlPackerEnabled.Enabled = False
        ChbIconChangerEnabled.Checked = True
        ChbIconChangerEnabled.Enabled = True
        PnlIconChangerEnabled.Enabled = True

        ChbObfuscatorResourcesContent.Enabled = True
        ChbObfuscatorResourcesContent.Checked = True
        ChbObfuscatorResourcesEncryption.Enabled = True
        ChbObfuscatorResourcesEncryption.Checked = True
        ChbObfuscatorResourcesCompress.Enabled = True
        ChbObfuscatorResourcesCompress.Checked = True
        ChbObfuscatorNamespacesRP.Enabled = True
        ChbObfuscatorNamespacesRP.Checked = True
        ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Enabled = True
        ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Checked = False

        CbxObfuscatorScheme.Items.Clear()
        CbxObfuscatorScheme.Items.Add("Alphabetic")
        CbxObfuscatorScheme.Items.Add("Greek")
        CbxObfuscatorScheme.Items.Add("Invisible")
        CbxObfuscatorScheme.Items.Add("Chinese")
        CbxObfuscatorScheme.Items.Add("Japanese")
        CbxObfuscatorScheme.Items.Add("Dot")
        CbxObfuscatorScheme.Items.Add("Symbols")
        CbxObfuscatorScheme.Items.Add("Flowing")

        ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Enabled = True
        ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Checked = True
    End Sub

    Private Sub EmptyTextBox()
        'Select Assembly
        TxbVersionInfo.Text = String.Empty
        TxbType.Text = String.Empty
        TxbFrameworkInfo.Text = String.Empty
        TxbCpuTargetInfo.Text = String.Empty
        TxbSelectedFile.Text = String.Empty
        PbxSelectedFile.Image = Nothing
        'Detection
        TxbDetection.Text = String.Empty
        PcbDetection.Image = Nothing
        'Version Infos
        TxbVersionInfosTitle.Text = String.Empty
        TxbVersionInfosDescription.Text = String.Empty
        TxbVersionInfosCompany.Text = String.Empty
        TxbVersionInfosProduct.Text = String.Empty
        TxbVersionInfosCopyright.Text = String.Empty
        TxbVersionInfosTrademark.Text = String.Empty
        TxbVersionInfosVersion.Text = String.Empty
        'Manifest Changer
        m_rdb.All(Function(x) x.Checked = False)
        'Icon Changer
        TxbIconChangerSelect.Text = String.Empty
        PbxIconChangerSelect.Image = Nothing
        'Dependencies
        LbxDependenciesAdd.Items.Clear()
        'Packer
        TxbPackerFramework.Text = "v4.0"
        TxbPackerPlatform.Text = String.Empty
        TxbPackerSystem.Text = String.Empty
    End Sub
#End Region

#Region " ######### CATEGORY TASK ######### "

#Region " About "

    Private Sub ShowAboutInfos()
        LblDNR_Version.Text = My.Application.Info.Version.ToString
        LblDNR_DevelopBy.Text = "3DotDev"
    End Sub

    Private Sub LblDNR_DevelopBy_MouseHover(sender As Object, e As EventArgs) Handles LblDNR_DevelopBy.MouseHover, LblDNR_Version.MouseHover, LblAboutCredits.MouseHover
        Dim lbl As Label = TryCast(sender, Label)
        lbl.ForeColor = Color.Violet
    End Sub

    Private Sub LblDNR_Version_MouseLeave(sender As Object, e As EventArgs) Handles LblDNR_DevelopBy.MouseLeave, LblDNR_Version.MouseLeave, LblAboutCredits.MouseLeave
        Dim lbl As Label = TryCast(sender, Label)
        lbl.ForeColor = Color.White
    End Sub

#End Region

#Region " Version Infos "

    Private Sub ChbVersionInfosEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles ChbVersionInfosEnabled.CheckedChanged
        PnlVersionInfosEnabled.Enabled = ChbVersionInfosEnabled.Checked
    End Sub

#End Region

#Region " Manifest "

    Private Sub ChbManifestEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles ChbManifestEnabled.CheckedChanged
        PnlManifestEnabled.Enabled = ChbManifestEnabled.Checked
    End Sub

#End Region

#Region " Icon Changer "

    Private Sub ChbIconChangerEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles ChbIconChangerEnabled.CheckedChanged
        PnlIconChangerEnabled.Enabled = ChbIconChangerEnabled.Checked
        LblIconChangerSelect.Visible = ChbIconChangerEnabled.Checked
        GrayedImage(PbxIconChangerSelect, TxbIconChangerSelect, ChbIconChangerEnabled.Checked, Nothing)
    End Sub

    Private Sub BtnIconChangerSelect_Click(sender As Object, e As EventArgs) Handles BtnIconChangerSelect.Click
        Using ofd = New OpenFileDialog
            With ofd
                .Title = "Select an icon file (*.ico)"
                .Filter = "Icon|*.ico;*.ico"
                .CheckFileExists = True
                .Multiselect = False
                If .ShowDialog() = DialogResult.OK Then
                    IconChanger.SelectingIcon(.FileName)
                End If
            End With
        End Using
    End Sub

    Private Sub OnSelectedIcon(sender As Object, e As CheckEventArgs) Handles IconChanger.CheckerResult
        If File.Exists(e.CheckedFile) Then
            TxbIconChangerSelect.Text = e.CheckedFile
            PbxIconChangerSelect.Image = Icon.ExtractAssociatedIcon(e.CheckedFile).ToBitmap
            LblIconChangerSelect.Visible = True
        Else
            LblIconChangerSelect.Visible = False
            MsgBox(e.Message, MsgBoxStyle.Exclamation, e.Title)
        End If
    End Sub


#End Region

#Region " Dependencies "

    Private Sub ChbDependenciesEnabled_click(sender As Object, e As EventArgs) Handles ChbDependenciesEnabled.Click
        PnlDependenciesEnabled.Enabled = ChbDependenciesEnabled.Checked
        ChbPackerEnabled.Enabled = ChbDependenciesEnabled.Checked

        If ChbDependenciesEnabled.Checked = True Then
            LblDependenciesWarning.ForeColor = Color.LimeGreen()
            LblDependenciesWarning.Text = "(Dependencies detection is enabled)"

            If ChbPackerEnabled.Checked Then
                ChbObfuscatorResourcesContent.Enabled = False
                ChbObfuscatorResourcesContent.Checked = False
                ChbObfuscatorResourcesEncryption.Enabled = False
                ChbObfuscatorResourcesEncryption.Checked = False
                ChbObfuscatorResourcesCompress.Enabled = False
                ChbObfuscatorResourcesCompress.Checked = False
                ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Enabled = False
                ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Checked = False
            Else
                ChbObfuscatorResourcesContent.Enabled = True
                ChbObfuscatorResourcesContent.Checked = True
                ChbObfuscatorResourcesEncryption.Enabled = True
                ChbObfuscatorResourcesEncryption.Checked = True
                ChbObfuscatorResourcesCompress.Enabled = True
                ChbObfuscatorResourcesCompress.Checked = True
                ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Enabled = True
                ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Checked = False
                GbxPackerLoader.Enabled = False
            End If
        Else
            LblDependenciesWarning.ForeColor = Color.DarkOrange
            LblDependenciesWarning.Text = "(Dependencies detection is disabled)"
            If ChbPackerEnabled.Checked Then
                ChbObfuscatorResourcesContent.Enabled = True
                ChbObfuscatorResourcesContent.Checked = True
                ChbObfuscatorResourcesEncryption.Enabled = True
                ChbObfuscatorResourcesEncryption.Checked = True
                ChbObfuscatorResourcesCompress.Enabled = True
                ChbObfuscatorResourcesCompress.Checked = True
                ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Enabled = True
                ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Checked = False
                GbxPackerLoader.Enabled = False
                ChbPackerEnabled.Checked = False
            End If
        End If
    End Sub

    Private Sub BtnDependenciesAdd_Click(sender As Object, e As EventArgs) Handles BtnDependenciesAdd.Click
        Using _ofd = New OpenFileDialog
            With _ofd
                .Title = "Select libraries (*.dll)"
                .Filter = "Libraries|*.dll;*.dll"
                .CheckFileExists = True
                .Multiselect = True
                If .ShowDialog() = DialogResult.OK Then
                    DependenciesChecker.AddReferences(.FileNames)
                End If
            End With
        End Using
    End Sub

    Private Sub BtnDependenciesDelete_Click(sender As Object, e As EventArgs) Handles BtnDependenciesDelete.Click
        DeleteReferencesItems()
    End Sub

    Private Sub DeleteReferencesItems()
        With LbxDependenciesAdd
            For i As Integer = LbxDependenciesAdd.SelectedIndices.Count - 1 To 0 Step -1
                .Items.RemoveAt(.SelectedIndices(i))
            Next
        End With
    End Sub

    Private Sub LbxDependenciesAdd_KeyUp(sender As Object, e As KeyEventArgs) Handles LbxDependenciesAdd.KeyUp
        If (e.KeyCode = Keys.Delete) Then
            DeleteReferencesItems()
        End If
    End Sub

    Private Sub OnDependenciesChecked(sender As Object, e As CheckEventArgs) Handles DependenciesChecker.CheckerResult
        If File.Exists(e.CheckedFile) Then
            If m_assemblyHasSerializableAttributes = False Then
                m_assemblyHasSerializableAttributes = DependenciesChecker.DependencyHasSerializableAttribute
            End If
            LbxDependenciesAdd.Items.Add(e.CheckedFile)
        Else
            MsgBox(e.Message, MsgBoxStyle.Exclamation, e.Title)
        End If
    End Sub

    Private Sub RdbDependenciesEmbedded_Click(sender As Object, e As EventArgs) Handles RdbDependenciesEmbedded.Click, RdbDependenciesMerged.Click
        If TryCast(sender, LogInRadioButton).Text = "Merging" Then
            CbxDependenciesEmbedded.Visible = False
        Else
            CbxDependenciesEmbedded.Visible = True
        End If
    End Sub

#End Region

#Region " Obfuscator "

    Private Sub ChbObfuscatorEnabled_CheckedChanged(sender As Object, e As EventArgs) Handles ChbObfuscatorEnabled.CheckedChanged
        PnlObfuscatorEnabled.Enabled = ChbObfuscatorEnabled.Checked
    End Sub

    Private Sub ChbObfuscatorNamespacesRP_CheckedChanged(sender As Object, e As EventArgs) Handles ChbObfuscatorNamespacesRP.CheckedChanged
        PnlObfuscatorNamespacesGroup.Enabled = ChbObfuscatorNamespacesRP.Checked
    End Sub

    Private Sub ChbObfuscatorReplaceNamespaceByEmptyNamespaces_CheckedChanged(sender As Object, e As EventArgs) Handles ChbObfuscatorReplaceNamespaceByEmptyNamespaces.CheckedChanged
        If ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Checked = True Then
            If ChbObfuscatorTypesRP.Checked = False Then
                ChbObfuscatorTypesRP.Checked = True
            End If
        End If
    End Sub

    Private Sub ChbObfuscatorTypesRP_CheckedChanged(sender As Object, e As EventArgs) Handles ChbObfuscatorTypesRP.CheckedChanged
        If ChbObfuscatorTypesRP.Checked = False Then
            If ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Checked = True Then
                ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Checked = False
            End If
        End If
    End Sub

#End Region

#Region " Packer "
    Private Sub ChbPackerEnabled_Click(sender As Object, e As EventArgs) Handles ChbPackerEnabled.Click
        PnlPackerEnabled.Enabled = ChbPackerEnabled.Checked
        Dim state As Boolean = ChbPackerEnabled.Checked
        ChbObfuscatorResourcesContent.Enabled = If(state, False, True)
        ChbObfuscatorResourcesContent.Checked = If(state, False, True)
        ChbObfuscatorResourcesEncryption.Enabled = If(state, False, True)
        ChbObfuscatorResourcesEncryption.Checked = If(state, False, True)
        ChbObfuscatorResourcesCompress.Enabled = If(state, False, True)
        ChbObfuscatorResourcesCompress.Checked = If(state, False, True)
        ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Enabled = If(state, False, True)
        ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Checked = If(state, False, False)
        ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Enabled = If(m_assemblyHasSerializableAttributes, False, If(state, False, True))
        ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Checked = If(m_assemblyHasSerializableAttributes, False, If(state, False, True))
        GbxPackerLoader.Enabled = If(state, True, False)
    End Sub

    Private Sub TpPacker_Enter(sender As Object, e As EventArgs) Handles TpPacker.Enter
        If ChbDependenciesEnabled.Checked = True Then
            LblPackerWarning.Visible = False
            ChbPackerEnabled.Enabled = True
        Else
            LblPackerWarning.Visible = True
            ChbPackerEnabled.Enabled = False
            ChbPackerEnabled.Checked = False
        End If
    End Sub

#End Region

#End Region

#Region " ######### START TASK ######### "

    Private Sub BtnStart_Click(sender As Object, e As EventArgs) Handles BtnStart.Click
        If Not BgwRenameTask.IsBusy Then
            BtnStart.Enabled = False
            m_taskIsRunning = True
            EnabledControls(False)
            GrayedImage(PbxSelectedFile, TxbSelectedFile, False, Nothing)
            GrayedImage(PcbDetection, TxbDetection, False, TryCast(PcbDetection.Tag, Image))
            GrayedImage(PbxIconChangerSelect, TxbIconChangerSelect, False, Nothing)

            BgwRenameTask.RunWorkerAsync(Tuple.Create(CbxObfuscatorScheme.SelectedIndex, CbxDependenciesEmbedded.SelectedIndex, m_assemblyHasSerializableAttributes))
        End If
    End Sub

    Private Sub EnabledControls(state As Boolean)
        GbxSelectFile.Enabled = state
        GbxDetection.Enabled = state
        TbcTask.Enabled = state
        BtnStart.Visible = state
    End Sub

    Private Sub BgwRenameTask_DoWork(sender As Object, e As DoWorkEventArgs) Handles BgwRenameTask.DoWork
        Try
            BgwRenameTask.ReportProgress(1, "Preparing tasks ...")
            Dim tups As Tuple(Of Integer, Integer, Boolean) = e.Argument
            Dim ObfuscatorRenamingSchemeIndex As Integer = tups.Item1
            Dim DependenciesEmbeddedIndex As Integer = tups.Item2
            Dim NamespaceSerialized As Boolean = tups.Item3

            Param.RenamingAccept = New RenamerState(ChbObfuscatorNamespacesRP.Checked,
                                                      ChbObfuscatorTypesRP.Checked,
                                                      ChbObfuscatorMethodsRP.Checked,
                                                      ChbObfuscatorPropertiesRP.Checked,
                                                      ChbObfuscatorFieldsRP.Checked,
                                                      ChbObfuscatorEventsRP.Checked,
                                                      ChbObfuscatorAttributesRP.Checked,
                                                      ChbObfuscatorMethodsRP.Checked,
                                                      ChbObfuscatorMethodsRP.Checked,
                                                      ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Checked,
                                                      ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Checked,
                                                      ObfuscatorRenamingSchemeIndex,
                                                      NamespaceSerialized)

            With m_taskArgs
                .DllReferences = New DependenciesInfos(ChbDependenciesEnabled.Checked,
                                                         LbxDependenciesAdd.Items.Cast(Of String).ToList,
                                                         RdbDependenciesEmbedded.Checked,
                                                         DependenciesEmbeddedIndex)

                .Obfuscation = New ObfuscationInfos(ChbObfuscatorEnabled.Checked,
                                                    ChbObfuscatorResourcesContent.Checked,
                                                    ChbObfuscatorResourcesEncryption.Checked,
                                                    ChbObfuscatorResourcesCompress.Checked,
                                                    ChbObfuscatorIntegersEncode.Checked,
                                                    ChbObfuscatorBooleanEncrypt.Checked,
                                                    ChbObfuscatorStringsEncrypt.Checked,
                                                    ChbObfuscatorAntiIlDasm.Checked,
                                                    ChbObfuscatorAntiTamper.Checked,
                                                    ChbObfuscatorAntiDebug.Checked,
                                                    ChbObfuscatorAntiDumper.Checked,
                                                    ChbObfuscatorHideCalls.Checked,
                                                    ChbObfuscatorControlFlow.Checked,
                                                    True)

                .VersionInfos = If(ChbVersionInfosEnabled.Checked, New Implementer.Core.Versions.Infos(ChbVersionInfosEnabled.Checked,
                                                                             TxbVersionInfosTitle.Text,
                                                                             TxbVersionInfosDescription.Text,
                                                                             TxbVersionInfosCompany.Text,
                                                                             TxbVersionInfosProduct.Text,
                                                                             TxbVersionInfosCopyright.Text,
                                                                             TxbVersionInfosTrademark.Text,
                                                                             TxbVersionInfosVersion.Text,
                                                                             TxbVersionInfosVersion.Text),
                                                                   New Implementer.Core.Versions.Infos(ChbVersionInfosEnabled.Checked, TxbSelectedFile.Text))

                .Manifest = New ManifestInfos(m_lastRequested, m_rdb.Where(Function(currentRequested) currentRequested.Checked).First.Tag.ToString)

                .IconChanger = New IconInfos(ChbIconChangerEnabled.Checked, TxbIconChangerSelect.Text)

                .Packer = New PackInfos(ChbPackerEnabled.Checked, If(ChbIconChangerEnabled.Checked = True AndAlso File.Exists(TxbIconChangerSelect.Text), TxbIconChangerSelect.Text, TxbSelectedFile.Text),
                                        If(m_rdb.Where(Function(y) y.Checked).First.Tag.ToString <> m_lastRequested, m_rdb.Where(Function(y) y.Checked).First.Tag.ToString, m_lastRequested))
            End With

            Param.TaskAccept = m_taskArgs

            m_Context = New Tasks(Param, BgwRenameTask)
            With m_Context
                .EmptyTemp()
                .PreparingTask(TxbSelectedFile.Text)

                If ChbDependenciesEnabled.Checked Then
                    Dim dependenciesResult = .CheckDependencies

                    If dependenciesResult.Result = String.Empty Then
                        .DependenciesTask()
                        .ManifestTask()
                        .VersionInfosTask()
                        .IconChangerTask()
                        .ObfuscationTask()
                        .PackerTask()
                        .FinalizeTask()
                    ElseIf dependenciesResult.Result.StartsWith("Error") Then
                        e.Result = New String() {"Error", dependenciesResult.Result}
                        .Clean()
                        Exit Sub
                    Else
                        If .HasObfuscationTask OrElse .HasPackerTask Then
                            .ManifestTask()
                            .VersionInfosTask()
                            .IconChangerTask()
                            .FinalizeTask()
                            e.Result = New String() {"Warning", dependenciesResult.Result}
                            Exit Sub
                        Else
                            .ManifestTask()
                            .VersionInfosTask()
                            .IconChangerTask()
                            .FinalizeTask()
                        End If
                    End If
                Else
                    .ManifestTask()
                    .VersionInfosTask()
                    .IconChangerTask()
                    .ObfuscationTask()
                    .PackerTask()
                    .FinalizeTask()
                End If

                e.Result = New String() {"Success", .Context.ProtectedFilePath, "False"}
                .Clean()
            End With
        Catch ex As Exception
            e.Result = New String() {"Error", ex.ToString}
        End Try

    End Sub

    Private Sub BgwRenameTask_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BgwRenameTask.ProgressChanged
        If Not e.UserState Is Nothing Then
            PgbStart.TextToShow = e.UserState.ToString
            PgbStart.Value = e.ProgressPercentage
        End If
    End Sub

    Private Sub BgwRenameTask_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BgwRenameTask.RunWorkerCompleted
        Try
            If Not e.Result Is Nothing Then
                Select Case e.Result(0)
                    Case "Error"
                        PgbStart.Text = e.Result(0).ToString
                        Dim r As New Frm_Result(e.Result(0), e.Result(1).ToString, "")
                        r.ShowDialog(New Win32Window(Me.Handle))
                        CleanUpMax()
                    Case "Warning"
                        PgbStart.Text = e.Result(0).ToString
                        Dim r As New Frm_Result(e.Result(0), "Following dependencies are missing : " & vbNewLine & e.Result(1).ToString, "")
                        r.ShowDialog(New Win32Window(Me.Handle))
                        CleanUpMin()
                    Case "Success"
                        PgbStart.Value = 100
                        PgbStart.Text = "Completed"
                        Dim r As New Frm_Result(e.Result(0), "Your file has been created here : " & vbNewLine & e.Result(1).ToString, e.Result(1).ToString)
                        r.ShowDialog(New Win32Window(Me.Handle))
                        CleanUpMax()
                End Select
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub CleanUpMin()
        m_taskIsRunning = False
        GbxSelectFile.Enabled = True
        GbxDetection.Enabled = True
        TbcTask.Enabled = True
        PgbStart.Text = String.Empty
        BtnStart.Visible = True
        BtnStart.Enabled = True
        PgbStart.Value = 0
        TbcTask.SelectedTab = TpDependencies
        GrayedImage(PbxSelectedFile, TxbSelectedFile, True, Nothing)
        GrayedImage(PcbDetection, TxbDetection, True, TryCast(PcbDetection.Tag, Image))
        GrayedImage(PbxIconChangerSelect, TxbIconChangerSelect, ChbIconChangerEnabled.Checked, Nothing)
    End Sub

    Private Sub CleanUpMax()
        m_taskIsRunning = False
        m_taskArgs.CleanUp()
        EmptyTextBox()
        GbxSelectFile.Enabled = True
        GbxDetection.Enabled = False
        CbxObfuscatorScheme.SelectedIndex = 0
        PgbStart.Text = String.Empty
        BtnStart.Visible = True
        PgbStart.Value = 0
        TbcTask.SelectedTab = TpAbout
    End Sub

#End Region

#Region " ######### OTHERS ######### "

    Private Sub GrayedImage(pcb As PictureBox, txb As TextBox, ChbEnabled As Boolean, OriginalImage As Image)
        Dim Img As Image
        If OriginalImage IsNot Nothing Then
            Img = OriginalImage
            ProcessGrayedImage(pcb, ChbEnabled, Img)
        Else
            If Not pcb.Image Is Nothing AndAlso File.Exists(txb.Text) Then
                Img = Icon.ExtractAssociatedIcon(txb.Text).ToBitmap
                ProcessGrayedImage(pcb, ChbEnabled, Img)
            End If
        End If
    End Sub

    Private Sub ProcessGrayedImage(pcb As PictureBox, ChbEnabled As Boolean, image As Image)
        pcb.Image = image
        If image IsNot Nothing AndAlso ChbEnabled = False Then
            Dim size As Size = image.Size
            Dim newMatrix As Single()() = New Single(4)() {}
            newMatrix(0) = New Single() {0.2125F, 0.2125F, 0.2125F, 0.0F, 0.0F}
            newMatrix(1) = New Single() {0.2577F, 0.2577F, 0.2577F, 0.0F, 0.0F}
            newMatrix(2) = New Single() {0.0361F, 0.0361F, 0.0361F, 0.0F, 0.0F}
            Dim arr As Single() = New Single(4) {}
            arr(3) = 1.0F
            newMatrix(3) = arr
            newMatrix(4) = New Single() {0.38F, 0.38F, 0.38F, 0.0F, 1.0F}
            Dim matrix As New ColorMatrix(newMatrix)
            Dim disabledAttr As New ImageAttributes()
            disabledAttr.ClearColorKey()
            disabledAttr.SetColorMatrix(matrix)
            pcb.Image = New Bitmap(image.Width, image.Height)
            Using gr As Graphics = Graphics.FromImage(pcb.Image)
                gr.DrawImage(image, New Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, disabledAttr)
            End Using
        End If
    End Sub

#End Region

End Class