Imports LoginTheme
Imports LoginTheme.XertzLoginTheme

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm_Main
    Inherits Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Main))
        Me.BgwRenameTask = New System.ComponentModel.BackgroundWorker()
        Me.Frm_MainThemeContainer = New LoginTheme.XertzLoginTheme.LogInThemeContainer()
        Me.LnkLblBlogSpot = New System.Windows.Forms.LinkLabel()
        Me.BtnStart = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.PgbStart = New LoginTheme.XertzLoginTheme.TextProgressBar()
        Me.TbcTask = New LoginTheme.XertzLoginTheme.LogInTabControl()
        Me.TpAbout = New System.Windows.Forms.TabPage()
        Me.LblAboutCredits1 = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblAboutCredits = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblAboutDevelopBy = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblAboutVersion = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblDNR_DevelopBy = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblDNR_Version = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.PbxAboutLogo = New System.Windows.Forms.PictureBox()
        Me.TpVersionInfos = New System.Windows.Forms.TabPage()
        Me.PnlVersionInfosEnabled = New System.Windows.Forms.Panel()
        Me.LblVersionInfosTitle = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblVersionInfosDescription = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblVersionInfosCompany = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblVersionInfosProduct = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblVersionInfosCopyright = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblVersionInfosTrademark = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblVersionInfosVersion = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.TxbVersionInfosVersion = New System.Windows.Forms.TextBox()
        Me.TxbVersionInfosTrademark = New System.Windows.Forms.TextBox()
        Me.TxbVersionInfosCopyright = New System.Windows.Forms.TextBox()
        Me.TxbVersionInfosProduct = New System.Windows.Forms.TextBox()
        Me.TxbVersionInfosCompany = New System.Windows.Forms.TextBox()
        Me.TxbVersionInfosDescription = New System.Windows.Forms.TextBox()
        Me.TxbVersionInfosTitle = New System.Windows.Forms.TextBox()
        Me.ChbVersionInfosEnabled = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.TpManifestChanger = New System.Windows.Forms.TabPage()
        Me.PnlManifestEnabled = New System.Windows.Forms.Panel()
        Me.RdbManifestChangerHighestAvailable = New LoginTheme.XertzLoginTheme.LogInRadioButton()
        Me.RdbManifestChangerRequireAdministrator = New LoginTheme.XertzLoginTheme.LogInRadioButton()
        Me.RdbManifestChangerAsInvoker = New LoginTheme.XertzLoginTheme.LogInRadioButton()
        Me.ChbManifestEnabled = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.TpIconChanger = New System.Windows.Forms.TabPage()
        Me.PnlIconChangerEnabled = New System.Windows.Forms.Panel()
        Me.LblIconChangerSelect = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.PbxIconChangerSelect = New System.Windows.Forms.PictureBox()
        Me.TxbIconChangerSelect = New System.Windows.Forms.TextBox()
        Me.BtnIconChangerSelect = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.ChbIconChangerEnabled = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.TpDependencies = New System.Windows.Forms.TabPage()
        Me.LblDependenciesWarning = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.PnlDependenciesEnabled = New System.Windows.Forms.Panel()
        Me.CbxDependenciesEmbedded = New LoginTheme.XertzLoginTheme.LogInComboBox()
        Me.RdbDependenciesEmbedded = New LoginTheme.XertzLoginTheme.LogInRadioButton()
        Me.RdbDependenciesMerged = New LoginTheme.XertzLoginTheme.LogInRadioButton()
        Me.BtnDependenciesDelete = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.BtnDependenciesAdd = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.LbxDependenciesAdd = New LoginTheme.XertzLoginTheme.LogInListbox()
        Me.ChbDependenciesEnabled = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.TpObfuscator = New System.Windows.Forms.TabPage()
        Me.PnlObfuscatorEnabled = New System.Windows.Forms.Panel()
        Me.GbxObfuscatorRenaming = New LoginTheme.XertzLoginTheme.LogInGroupBox()
        Me.ChbObfuscatorResourcesContent = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.PnlObfuscatorNamespacesGroup = New System.Windows.Forms.Panel()
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorAttributesRP = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorFieldsRP = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorEventsRP = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorPropertiesRP = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorMethodsRP = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorTypesRP = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorNamespacesRP = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.CbxObfuscatorScheme = New LoginTheme.XertzLoginTheme.LogInComboBox()
        Me.LblObfuscatorScheme = New System.Windows.Forms.Label()
        Me.GbxObfuscatorAdvanced = New LoginTheme.XertzLoginTheme.LogInGroupBox()
        Me.ChbObfuscatorResourcesEncryption = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorControlFlow = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorAntiDumper = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorBooleanEncrypt = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorHideCalls = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorResourcesCompress = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorAntiTamper = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorAntiIlDasm = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorIntegersEncode = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorStringsEncrypt = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorAntiDebug = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.ChbObfuscatorEnabled = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.TpPacker = New System.Windows.Forms.TabPage()
        Me.LblPackerWarning = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.PnlPackerEnabled = New System.Windows.Forms.Panel()
        Me.GbxPackerLoader = New LoginTheme.XertzLoginTheme.LogInGroupBox()
        Me.TxbPackerFramework = New System.Windows.Forms.TextBox()
        Me.LblPackerFramework = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblPackerSystem = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.LblPackerPlatform = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.TxbPackerPlatform = New System.Windows.Forms.TextBox()
        Me.TxbPackerSystem = New System.Windows.Forms.TextBox()
        Me.ChbPackerEnabled = New LoginTheme.XertzLoginTheme.LogInCheckBox()
        Me.GbxDetection = New LoginTheme.XertzLoginTheme.LogInGroupBox()
        Me.PcbDetection = New System.Windows.Forms.PictureBox()
        Me.TxbDetection = New System.Windows.Forms.TextBox()
        Me.LblDetection = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.GbxSelectFile = New LoginTheme.XertzLoginTheme.LogInGroupBox()
        Me.LblType = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.TxbSelectedFile = New System.Windows.Forms.TextBox()
        Me.TxbType = New System.Windows.Forms.TextBox()
        Me.PbxSelectedFile = New System.Windows.Forms.PictureBox()
        Me.LblCpuTargetInfo = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.BtnSelectFile = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.TxbCpuTargetInfo = New System.Windows.Forms.TextBox()
        Me.LblVersionInfo = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.TxbFrameworkInfo = New System.Windows.Forms.TextBox()
        Me.TxbVersionInfo = New System.Windows.Forms.TextBox()
        Me.LblFrameworkInfo = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.Frm_MainThemeContainer.SuspendLayout()
        Me.TbcTask.SuspendLayout()
        Me.TpAbout.SuspendLayout()
        CType(Me.PbxAboutLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TpVersionInfos.SuspendLayout()
        Me.PnlVersionInfosEnabled.SuspendLayout()
        Me.TpManifestChanger.SuspendLayout()
        Me.PnlManifestEnabled.SuspendLayout()
        Me.TpIconChanger.SuspendLayout()
        Me.PnlIconChangerEnabled.SuspendLayout()
        CType(Me.PbxIconChangerSelect, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TpDependencies.SuspendLayout()
        Me.PnlDependenciesEnabled.SuspendLayout()
        Me.TpObfuscator.SuspendLayout()
        Me.PnlObfuscatorEnabled.SuspendLayout()
        Me.GbxObfuscatorRenaming.SuspendLayout()
        Me.PnlObfuscatorNamespacesGroup.SuspendLayout()
        Me.GbxObfuscatorAdvanced.SuspendLayout()
        Me.TpPacker.SuspendLayout()
        Me.PnlPackerEnabled.SuspendLayout()
        Me.GbxPackerLoader.SuspendLayout()
        Me.GbxDetection.SuspendLayout()
        CType(Me.PcbDetection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GbxSelectFile.SuspendLayout()
        CType(Me.PbxSelectedFile, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BgwRenameTask
        '
        Me.BgwRenameTask.WorkerReportsProgress = True
        Me.BgwRenameTask.WorkerSupportsCancellation = True
        '
        'Frm_MainThemeContainer
        '
        Me.Frm_MainThemeContainer.AllowClose = True
        Me.Frm_MainThemeContainer.AllowMaximize = False
        Me.Frm_MainThemeContainer.AllowMinimize = True
        Me.Frm_MainThemeContainer.BackColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Frm_MainThemeContainer.BaseColour = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Frm_MainThemeContainer.BorderColour = System.Drawing.Color.DimGray
        Me.Frm_MainThemeContainer.ContainerColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.Frm_MainThemeContainer.Controls.Add(Me.LnkLblBlogSpot)
        Me.Frm_MainThemeContainer.Controls.Add(Me.BtnStart)
        Me.Frm_MainThemeContainer.Controls.Add(Me.PgbStart)
        Me.Frm_MainThemeContainer.Controls.Add(Me.TbcTask)
        Me.Frm_MainThemeContainer.Controls.Add(Me.GbxDetection)
        Me.Frm_MainThemeContainer.Controls.Add(Me.GbxSelectFile)
        Me.Frm_MainThemeContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Frm_MainThemeContainer.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Frm_MainThemeContainer.FontSize = 12
        Me.Frm_MainThemeContainer.HoverColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Frm_MainThemeContainer.Location = New System.Drawing.Point(0, 0)
        Me.Frm_MainThemeContainer.MouseOverColour = System.Drawing.Color.BlueViolet
        Me.Frm_MainThemeContainer.Name = "Frm_MainThemeContainer"
        Me.Frm_MainThemeContainer.ShowControlBox = True
        Me.Frm_MainThemeContainer.ShowIcon = True
        Me.Frm_MainThemeContainer.ShowMaximizeButton = True
        Me.Frm_MainThemeContainer.ShowMinimizeButton = True
        Me.Frm_MainThemeContainer.Size = New System.Drawing.Size(704, 697)
        Me.Frm_MainThemeContainer.TabIndex = 0
        Me.Frm_MainThemeContainer.Text = "DotNetPatcher"
        '
        'LnkLblBlogSpot
        '
        Me.LnkLblBlogSpot.ActiveLinkColor = System.Drawing.Color.White
        Me.LnkLblBlogSpot.AutoSize = True
        Me.LnkLblBlogSpot.BackColor = System.Drawing.Color.Transparent
        Me.LnkLblBlogSpot.LinkColor = System.Drawing.Color.White
        Me.LnkLblBlogSpot.Location = New System.Drawing.Point(273, 676)
        Me.LnkLblBlogSpot.Name = "LnkLblBlogSpot"
        Me.LnkLblBlogSpot.Size = New System.Drawing.Size(161, 13)
        Me.LnkLblBlogSpot.TabIndex = 98
        Me.LnkLblBlogSpot.TabStop = True
        Me.LnkLblBlogSpot.Text = "http://3dotdevcoder.blogspot.fr/"
        Me.LnkLblBlogSpot.VisitedLinkColor = System.Drawing.Color.BlueViolet
        '
        'BtnStart
        '
        Me.BtnStart.BackColor = System.Drawing.Color.Transparent
        Me.BtnStart.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnStart.BorderColour = System.Drawing.Color.DimGray
        Me.BtnStart.Enabled = False
        Me.BtnStart.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnStart.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnStart.Location = New System.Drawing.Point(16, 627)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnStart.ProgressColour = System.Drawing.Color.BlueViolet
        Me.BtnStart.Size = New System.Drawing.Size(671, 41)
        Me.BtnStart.TabIndex = 7
        Me.BtnStart.Text = "Start"
        '
        'PgbStart
        '
        Me.PgbStart.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.PgbStart.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.PgbStart.ForeColor = System.Drawing.Color.Black
        Me.PgbStart.Location = New System.Drawing.Point(16, 627)
        Me.PgbStart.Name = "PgbStart"
        Me.PgbStart.OverLayColor = System.Drawing.Color.Black
        Me.PgbStart.Percentage = 0R
        Me.PgbStart.PercentageAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.PgbStart.ShowAText = True
        Me.PgbStart.Size = New System.Drawing.Size(671, 41)
        Me.PgbStart.TabIndex = 97
        Me.PgbStart.TextToShow = Nothing
        '
        'TbcTask
        '
        Me.TbcTask.ActiveColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.TbcTask.BackTabColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TbcTask.BaseColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.TbcTask.BorderColour = System.Drawing.Color.DimGray
        Me.TbcTask.Controls.Add(Me.TpAbout)
        Me.TbcTask.Controls.Add(Me.TpVersionInfos)
        Me.TbcTask.Controls.Add(Me.TpManifestChanger)
        Me.TbcTask.Controls.Add(Me.TpIconChanger)
        Me.TbcTask.Controls.Add(Me.TpDependencies)
        Me.TbcTask.Controls.Add(Me.TpObfuscator)
        Me.TbcTask.Controls.Add(Me.TpPacker)
        Me.TbcTask.Enabled = False
        Me.TbcTask.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.TbcTask.HorizLineColour = System.Drawing.Color.BlueViolet
        Me.TbcTask.ItemSize = New System.Drawing.Size(240, 32)
        Me.TbcTask.Location = New System.Drawing.Point(12, 255)
        Me.TbcTask.Name = "TbcTask"
        Me.TbcTask.SelectedIndex = 0
        Me.TbcTask.Size = New System.Drawing.Size(679, 366)
        Me.TbcTask.TabIndex = 16
        Me.TbcTask.TextColour = System.Drawing.Color.White
        Me.TbcTask.UpLineColour = System.Drawing.Color.BlueViolet
        '
        'TpAbout
        '
        Me.TpAbout.BackColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TpAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TpAbout.Controls.Add(Me.LblAboutCredits1)
        Me.TpAbout.Controls.Add(Me.LblAboutCredits)
        Me.TpAbout.Controls.Add(Me.LblAboutDevelopBy)
        Me.TpAbout.Controls.Add(Me.LblAboutVersion)
        Me.TpAbout.Controls.Add(Me.LblDNR_DevelopBy)
        Me.TpAbout.Controls.Add(Me.LblDNR_Version)
        Me.TpAbout.Controls.Add(Me.PbxAboutLogo)
        Me.TpAbout.Location = New System.Drawing.Point(4, 36)
        Me.TpAbout.Name = "TpAbout"
        Me.TpAbout.Padding = New System.Windows.Forms.Padding(3)
        Me.TpAbout.Size = New System.Drawing.Size(671, 326)
        Me.TpAbout.TabIndex = 0
        Me.TpAbout.Text = "About"
        '
        'LblAboutCredits1
        '
        Me.LblAboutCredits1.AutoSize = True
        Me.LblAboutCredits1.BackColor = System.Drawing.Color.Transparent
        Me.LblAboutCredits1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblAboutCredits1.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutCredits1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutCredits1.Location = New System.Drawing.Point(267, 158)
        Me.LblAboutCredits1.Name = "LblAboutCredits1"
        Me.LblAboutCredits1.Size = New System.Drawing.Size(50, 15)
        Me.LblAboutCredits1.TabIndex = 97
        Me.LblAboutCredits1.Text = "Credits :"
        '
        'LblAboutCredits
        '
        Me.LblAboutCredits.AutoSize = True
        Me.LblAboutCredits.BackColor = System.Drawing.Color.Transparent
        Me.LblAboutCredits.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblAboutCredits.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutCredits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutCredits.Location = New System.Drawing.Point(325, 158)
        Me.LblAboutCredits.Name = "LblAboutCredits"
        Me.LblAboutCredits.Size = New System.Drawing.Size(231, 45)
        Me.LblAboutCredits.TabIndex = 94
        Me.LblAboutCredits.Text = "0xd4d, yck1509, Jbevain, Aeonhack, Pl0xy, " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Xenocode, Xertz, Mono.Linker, Mirhabi" &
    ", " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Vestris ResourceLib, Paupino, Markhor"
        '
        'LblAboutDevelopBy
        '
        Me.LblAboutDevelopBy.AutoSize = True
        Me.LblAboutDevelopBy.BackColor = System.Drawing.Color.Transparent
        Me.LblAboutDevelopBy.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblAboutDevelopBy.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutDevelopBy.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutDevelopBy.Location = New System.Drawing.Point(232, 131)
        Me.LblAboutDevelopBy.Name = "LblAboutDevelopBy"
        Me.LblAboutDevelopBy.Size = New System.Drawing.Size(85, 15)
        Me.LblAboutDevelopBy.TabIndex = 92
        Me.LblAboutDevelopBy.Text = "Developed By :"
        '
        'LblAboutVersion
        '
        Me.LblAboutVersion.AutoSize = True
        Me.LblAboutVersion.BackColor = System.Drawing.Color.Transparent
        Me.LblAboutVersion.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblAboutVersion.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutVersion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblAboutVersion.Location = New System.Drawing.Point(265, 104)
        Me.LblAboutVersion.Name = "LblAboutVersion"
        Me.LblAboutVersion.Size = New System.Drawing.Size(51, 15)
        Me.LblAboutVersion.TabIndex = 91
        Me.LblAboutVersion.Text = "Version :"
        '
        'LblDNR_DevelopBy
        '
        Me.LblDNR_DevelopBy.BackColor = System.Drawing.Color.Transparent
        Me.LblDNR_DevelopBy.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblDNR_DevelopBy.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblDNR_DevelopBy.ForeColor = System.Drawing.Color.White
        Me.LblDNR_DevelopBy.Location = New System.Drawing.Point(324, 131)
        Me.LblDNR_DevelopBy.Name = "LblDNR_DevelopBy"
        Me.LblDNR_DevelopBy.Size = New System.Drawing.Size(186, 16)
        Me.LblDNR_DevelopBy.TabIndex = 79
        Me.LblDNR_DevelopBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblDNR_Version
        '
        Me.LblDNR_Version.BackColor = System.Drawing.Color.Transparent
        Me.LblDNR_Version.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblDNR_Version.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblDNR_Version.ForeColor = System.Drawing.Color.White
        Me.LblDNR_Version.Location = New System.Drawing.Point(324, 104)
        Me.LblDNR_Version.Name = "LblDNR_Version"
        Me.LblDNR_Version.Size = New System.Drawing.Size(186, 16)
        Me.LblDNR_Version.TabIndex = 78
        Me.LblDNR_Version.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'PbxAboutLogo
        '
        Me.PbxAboutLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PbxAboutLogo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PbxAboutLogo.Image = Global.DotNetPatcher.My.Resources.Resources.TDD
        Me.PbxAboutLogo.Location = New System.Drawing.Point(100, 96)
        Me.PbxAboutLogo.Name = "PbxAboutLogo"
        Me.PbxAboutLogo.Size = New System.Drawing.Size(110, 110)
        Me.PbxAboutLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PbxAboutLogo.TabIndex = 74
        Me.PbxAboutLogo.TabStop = False
        Me.PbxAboutLogo.Tag = "PbxAboutLogo.Click"
        '
        'TpVersionInfos
        '
        Me.TpVersionInfos.BackColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TpVersionInfos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TpVersionInfos.Controls.Add(Me.PnlVersionInfosEnabled)
        Me.TpVersionInfos.Controls.Add(Me.ChbVersionInfosEnabled)
        Me.TpVersionInfos.Location = New System.Drawing.Point(4, 36)
        Me.TpVersionInfos.Name = "TpVersionInfos"
        Me.TpVersionInfos.Padding = New System.Windows.Forms.Padding(3)
        Me.TpVersionInfos.Size = New System.Drawing.Size(671, 326)
        Me.TpVersionInfos.TabIndex = 1
        Me.TpVersionInfos.Text = "Version Infos Changer"
        '
        'PnlVersionInfosEnabled
        '
        Me.PnlVersionInfosEnabled.Controls.Add(Me.LblVersionInfosTitle)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.LblVersionInfosDescription)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.LblVersionInfosCompany)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.LblVersionInfosProduct)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.LblVersionInfosCopyright)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.LblVersionInfosTrademark)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.LblVersionInfosVersion)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.TxbVersionInfosVersion)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.TxbVersionInfosTrademark)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.TxbVersionInfosCopyright)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.TxbVersionInfosProduct)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.TxbVersionInfosCompany)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.TxbVersionInfosDescription)
        Me.PnlVersionInfosEnabled.Controls.Add(Me.TxbVersionInfosTitle)
        Me.PnlVersionInfosEnabled.Location = New System.Drawing.Point(6, 34)
        Me.PnlVersionInfosEnabled.Name = "PnlVersionInfosEnabled"
        Me.PnlVersionInfosEnabled.Size = New System.Drawing.Size(657, 273)
        Me.PnlVersionInfosEnabled.TabIndex = 8
        '
        'LblVersionInfosTitle
        '
        Me.LblVersionInfosTitle.AutoSize = True
        Me.LblVersionInfosTitle.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfosTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfosTitle.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosTitle.Location = New System.Drawing.Point(85, 38)
        Me.LblVersionInfosTitle.Name = "LblVersionInfosTitle"
        Me.LblVersionInfosTitle.Size = New System.Drawing.Size(35, 15)
        Me.LblVersionInfosTitle.TabIndex = 98
        Me.LblVersionInfosTitle.Text = "Title :"
        '
        'LblVersionInfosDescription
        '
        Me.LblVersionInfosDescription.AutoSize = True
        Me.LblVersionInfosDescription.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfosDescription.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfosDescription.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosDescription.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosDescription.Location = New System.Drawing.Point(48, 69)
        Me.LblVersionInfosDescription.Name = "LblVersionInfosDescription"
        Me.LblVersionInfosDescription.Size = New System.Drawing.Size(73, 15)
        Me.LblVersionInfosDescription.TabIndex = 97
        Me.LblVersionInfosDescription.Text = "Description :"
        '
        'LblVersionInfosCompany
        '
        Me.LblVersionInfosCompany.AutoSize = True
        Me.LblVersionInfosCompany.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfosCompany.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfosCompany.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosCompany.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosCompany.Location = New System.Drawing.Point(56, 100)
        Me.LblVersionInfosCompany.Name = "LblVersionInfosCompany"
        Me.LblVersionInfosCompany.Size = New System.Drawing.Size(65, 15)
        Me.LblVersionInfosCompany.TabIndex = 96
        Me.LblVersionInfosCompany.Text = "Company :"
        '
        'LblVersionInfosProduct
        '
        Me.LblVersionInfosProduct.AutoSize = True
        Me.LblVersionInfosProduct.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfosProduct.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfosProduct.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosProduct.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosProduct.Location = New System.Drawing.Point(66, 131)
        Me.LblVersionInfosProduct.Name = "LblVersionInfosProduct"
        Me.LblVersionInfosProduct.Size = New System.Drawing.Size(55, 15)
        Me.LblVersionInfosProduct.TabIndex = 95
        Me.LblVersionInfosProduct.Text = "Product :"
        '
        'LblVersionInfosCopyright
        '
        Me.LblVersionInfosCopyright.AutoSize = True
        Me.LblVersionInfosCopyright.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfosCopyright.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfosCopyright.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosCopyright.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosCopyright.Location = New System.Drawing.Point(55, 162)
        Me.LblVersionInfosCopyright.Name = "LblVersionInfosCopyright"
        Me.LblVersionInfosCopyright.Size = New System.Drawing.Size(66, 15)
        Me.LblVersionInfosCopyright.TabIndex = 94
        Me.LblVersionInfosCopyright.Text = "Copyright :"
        '
        'LblVersionInfosTrademark
        '
        Me.LblVersionInfosTrademark.AutoSize = True
        Me.LblVersionInfosTrademark.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfosTrademark.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfosTrademark.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosTrademark.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosTrademark.Location = New System.Drawing.Point(51, 193)
        Me.LblVersionInfosTrademark.Name = "LblVersionInfosTrademark"
        Me.LblVersionInfosTrademark.Size = New System.Drawing.Size(68, 15)
        Me.LblVersionInfosTrademark.TabIndex = 93
        Me.LblVersionInfosTrademark.Text = "Trademark :"
        '
        'LblVersionInfosVersion
        '
        Me.LblVersionInfosVersion.AutoSize = True
        Me.LblVersionInfosVersion.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfosVersion.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfosVersion.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosVersion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfosVersion.Location = New System.Drawing.Point(69, 224)
        Me.LblVersionInfosVersion.Name = "LblVersionInfosVersion"
        Me.LblVersionInfosVersion.Size = New System.Drawing.Size(51, 15)
        Me.LblVersionInfosVersion.TabIndex = 92
        Me.LblVersionInfosVersion.Text = "Version :"
        '
        'TxbVersionInfosVersion
        '
        Me.TxbVersionInfosVersion.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfosVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfosVersion.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfosVersion.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfosVersion.Location = New System.Drawing.Point(127, 220)
        Me.TxbVersionInfosVersion.Name = "TxbVersionInfosVersion"
        Me.TxbVersionInfosVersion.Size = New System.Drawing.Size(483, 25)
        Me.TxbVersionInfosVersion.TabIndex = 56
        Me.TxbVersionInfosVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbVersionInfosTrademark
        '
        Me.TxbVersionInfosTrademark.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfosTrademark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfosTrademark.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfosTrademark.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfosTrademark.Location = New System.Drawing.Point(127, 189)
        Me.TxbVersionInfosTrademark.Name = "TxbVersionInfosTrademark"
        Me.TxbVersionInfosTrademark.Size = New System.Drawing.Size(483, 25)
        Me.TxbVersionInfosTrademark.TabIndex = 55
        Me.TxbVersionInfosTrademark.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbVersionInfosCopyright
        '
        Me.TxbVersionInfosCopyright.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfosCopyright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfosCopyright.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfosCopyright.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfosCopyright.Location = New System.Drawing.Point(127, 158)
        Me.TxbVersionInfosCopyright.Name = "TxbVersionInfosCopyright"
        Me.TxbVersionInfosCopyright.Size = New System.Drawing.Size(483, 25)
        Me.TxbVersionInfosCopyright.TabIndex = 54
        Me.TxbVersionInfosCopyright.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbVersionInfosProduct
        '
        Me.TxbVersionInfosProduct.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfosProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfosProduct.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfosProduct.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfosProduct.Location = New System.Drawing.Point(127, 127)
        Me.TxbVersionInfosProduct.Name = "TxbVersionInfosProduct"
        Me.TxbVersionInfosProduct.Size = New System.Drawing.Size(483, 25)
        Me.TxbVersionInfosProduct.TabIndex = 53
        Me.TxbVersionInfosProduct.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbVersionInfosCompany
        '
        Me.TxbVersionInfosCompany.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfosCompany.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfosCompany.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfosCompany.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfosCompany.Location = New System.Drawing.Point(127, 96)
        Me.TxbVersionInfosCompany.Name = "TxbVersionInfosCompany"
        Me.TxbVersionInfosCompany.Size = New System.Drawing.Size(483, 25)
        Me.TxbVersionInfosCompany.TabIndex = 52
        Me.TxbVersionInfosCompany.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbVersionInfosDescription
        '
        Me.TxbVersionInfosDescription.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfosDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfosDescription.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfosDescription.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfosDescription.Location = New System.Drawing.Point(127, 65)
        Me.TxbVersionInfosDescription.Name = "TxbVersionInfosDescription"
        Me.TxbVersionInfosDescription.Size = New System.Drawing.Size(483, 25)
        Me.TxbVersionInfosDescription.TabIndex = 51
        Me.TxbVersionInfosDescription.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbVersionInfosTitle
        '
        Me.TxbVersionInfosTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfosTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfosTitle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfosTitle.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfosTitle.Location = New System.Drawing.Point(127, 34)
        Me.TxbVersionInfosTitle.Name = "TxbVersionInfosTitle"
        Me.TxbVersionInfosTitle.Size = New System.Drawing.Size(483, 25)
        Me.TxbVersionInfosTitle.TabIndex = 50
        Me.TxbVersionInfosTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ChbVersionInfosEnabled
        '
        Me.ChbVersionInfosEnabled.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbVersionInfosEnabled.BorderColour = System.Drawing.Color.DimGray
        Me.ChbVersionInfosEnabled.Checked = True
        Me.ChbVersionInfosEnabled.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbVersionInfosEnabled.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbVersionInfosEnabled.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbVersionInfosEnabled.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbVersionInfosEnabled.Location = New System.Drawing.Point(6, 6)
        Me.ChbVersionInfosEnabled.Name = "ChbVersionInfosEnabled"
        Me.ChbVersionInfosEnabled.Size = New System.Drawing.Size(86, 22)
        Me.ChbVersionInfosEnabled.TabIndex = 7
        Me.ChbVersionInfosEnabled.Tag = "Types"
        Me.ChbVersionInfosEnabled.Text = "Enabled"
        '
        'TpManifestChanger
        '
        Me.TpManifestChanger.BackColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TpManifestChanger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TpManifestChanger.Controls.Add(Me.PnlManifestEnabled)
        Me.TpManifestChanger.Controls.Add(Me.ChbManifestEnabled)
        Me.TpManifestChanger.Location = New System.Drawing.Point(4, 36)
        Me.TpManifestChanger.Name = "TpManifestChanger"
        Me.TpManifestChanger.Padding = New System.Windows.Forms.Padding(3)
        Me.TpManifestChanger.Size = New System.Drawing.Size(671, 326)
        Me.TpManifestChanger.TabIndex = 3
        Me.TpManifestChanger.Text = "Manifest Changer"
        '
        'PnlManifestEnabled
        '
        Me.PnlManifestEnabled.Controls.Add(Me.RdbManifestChangerHighestAvailable)
        Me.PnlManifestEnabled.Controls.Add(Me.RdbManifestChangerRequireAdministrator)
        Me.PnlManifestEnabled.Controls.Add(Me.RdbManifestChangerAsInvoker)
        Me.PnlManifestEnabled.Location = New System.Drawing.Point(6, 34)
        Me.PnlManifestEnabled.Name = "PnlManifestEnabled"
        Me.PnlManifestEnabled.Size = New System.Drawing.Size(657, 273)
        Me.PnlManifestEnabled.TabIndex = 41
        '
        'RdbManifestChangerHighestAvailable
        '
        Me.RdbManifestChangerHighestAvailable.BaseColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.RdbManifestChangerHighestAvailable.BorderColour = System.Drawing.Color.DimGray
        Me.RdbManifestChangerHighestAvailable.Checked = False
        Me.RdbManifestChangerHighestAvailable.CheckedColour = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.RdbManifestChangerHighestAvailable.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.RdbManifestChangerHighestAvailable.Cursor = System.Windows.Forms.Cursors.Hand
        Me.RdbManifestChangerHighestAvailable.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RdbManifestChangerHighestAvailable.HighlightColour = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(49, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.RdbManifestChangerHighestAvailable.Location = New System.Drawing.Point(241, 174)
        Me.RdbManifestChangerHighestAvailable.Name = "RdbManifestChangerHighestAvailable"
        Me.RdbManifestChangerHighestAvailable.Size = New System.Drawing.Size(160, 18)
        Me.RdbManifestChangerHighestAvailable.TabIndex = 8
        Me.RdbManifestChangerHighestAvailable.Tag = "highestAvailable"
        Me.RdbManifestChangerHighestAvailable.Text = "highestAvailable"
        '
        'RdbManifestChangerRequireAdministrator
        '
        Me.RdbManifestChangerRequireAdministrator.BaseColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.RdbManifestChangerRequireAdministrator.BorderColour = System.Drawing.Color.DimGray
        Me.RdbManifestChangerRequireAdministrator.Checked = False
        Me.RdbManifestChangerRequireAdministrator.CheckedColour = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.RdbManifestChangerRequireAdministrator.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.RdbManifestChangerRequireAdministrator.Cursor = System.Windows.Forms.Cursors.Hand
        Me.RdbManifestChangerRequireAdministrator.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RdbManifestChangerRequireAdministrator.HighlightColour = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(49, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.RdbManifestChangerRequireAdministrator.Location = New System.Drawing.Point(241, 127)
        Me.RdbManifestChangerRequireAdministrator.Name = "RdbManifestChangerRequireAdministrator"
        Me.RdbManifestChangerRequireAdministrator.Size = New System.Drawing.Size(160, 18)
        Me.RdbManifestChangerRequireAdministrator.TabIndex = 7
        Me.RdbManifestChangerRequireAdministrator.Tag = "requireAdministrator"
        Me.RdbManifestChangerRequireAdministrator.Text = "requireAdministrator"
        '
        'RdbManifestChangerAsInvoker
        '
        Me.RdbManifestChangerAsInvoker.BaseColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.RdbManifestChangerAsInvoker.BorderColour = System.Drawing.Color.DimGray
        Me.RdbManifestChangerAsInvoker.Checked = False
        Me.RdbManifestChangerAsInvoker.CheckedColour = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.RdbManifestChangerAsInvoker.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.RdbManifestChangerAsInvoker.Cursor = System.Windows.Forms.Cursors.Hand
        Me.RdbManifestChangerAsInvoker.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RdbManifestChangerAsInvoker.HighlightColour = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(49, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.RdbManifestChangerAsInvoker.Location = New System.Drawing.Point(241, 81)
        Me.RdbManifestChangerAsInvoker.Name = "RdbManifestChangerAsInvoker"
        Me.RdbManifestChangerAsInvoker.Size = New System.Drawing.Size(175, 18)
        Me.RdbManifestChangerAsInvoker.TabIndex = 6
        Me.RdbManifestChangerAsInvoker.Tag = "asInvoker"
        Me.RdbManifestChangerAsInvoker.Text = "asInvoker (Default)"
        '
        'ChbManifestEnabled
        '
        Me.ChbManifestEnabled.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbManifestEnabled.BorderColour = System.Drawing.Color.DimGray
        Me.ChbManifestEnabled.Checked = True
        Me.ChbManifestEnabled.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbManifestEnabled.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbManifestEnabled.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbManifestEnabled.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbManifestEnabled.Location = New System.Drawing.Point(6, 6)
        Me.ChbManifestEnabled.Name = "ChbManifestEnabled"
        Me.ChbManifestEnabled.Size = New System.Drawing.Size(86, 22)
        Me.ChbManifestEnabled.TabIndex = 9
        Me.ChbManifestEnabled.Tag = "Types"
        Me.ChbManifestEnabled.Text = "Enabled"
        '
        'TpIconChanger
        '
        Me.TpIconChanger.BackColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TpIconChanger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TpIconChanger.Controls.Add(Me.PnlIconChangerEnabled)
        Me.TpIconChanger.Controls.Add(Me.ChbIconChangerEnabled)
        Me.TpIconChanger.Location = New System.Drawing.Point(4, 36)
        Me.TpIconChanger.Name = "TpIconChanger"
        Me.TpIconChanger.Padding = New System.Windows.Forms.Padding(3)
        Me.TpIconChanger.Size = New System.Drawing.Size(671, 326)
        Me.TpIconChanger.TabIndex = 6
        Me.TpIconChanger.Text = "Icon Changer"
        '
        'PnlIconChangerEnabled
        '
        Me.PnlIconChangerEnabled.Controls.Add(Me.LblIconChangerSelect)
        Me.PnlIconChangerEnabled.Controls.Add(Me.PbxIconChangerSelect)
        Me.PnlIconChangerEnabled.Controls.Add(Me.TxbIconChangerSelect)
        Me.PnlIconChangerEnabled.Controls.Add(Me.BtnIconChangerSelect)
        Me.PnlIconChangerEnabled.Location = New System.Drawing.Point(6, 34)
        Me.PnlIconChangerEnabled.Name = "PnlIconChangerEnabled"
        Me.PnlIconChangerEnabled.Size = New System.Drawing.Size(657, 273)
        Me.PnlIconChangerEnabled.TabIndex = 40
        '
        'LblIconChangerSelect
        '
        Me.LblIconChangerSelect.AutoSize = True
        Me.LblIconChangerSelect.BackColor = System.Drawing.Color.Transparent
        Me.LblIconChangerSelect.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblIconChangerSelect.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblIconChangerSelect.ForeColor = System.Drawing.Color.DarkOrange
        Me.LblIconChangerSelect.Location = New System.Drawing.Point(136, 100)
        Me.LblIconChangerSelect.Name = "LblIconChangerSelect"
        Me.LblIconChangerSelect.Size = New System.Drawing.Size(379, 30)
        Me.LblIconChangerSelect.TabIndex = 95
        Me.LblIconChangerSelect.Text = "You will have to restart your explorer process to update the icon cache," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " in ord" &
    "er to see the modification of the icon made to your file !"
        Me.LblIconChangerSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LblIconChangerSelect.Visible = False
        '
        'PbxIconChangerSelect
        '
        Me.PbxIconChangerSelect.BackColor = System.Drawing.Color.Transparent
        Me.PbxIconChangerSelect.Location = New System.Drawing.Point(594, 32)
        Me.PbxIconChangerSelect.Name = "PbxIconChangerSelect"
        Me.PbxIconChangerSelect.Size = New System.Drawing.Size(48, 48)
        Me.PbxIconChangerSelect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PbxIconChangerSelect.TabIndex = 13
        Me.PbxIconChangerSelect.TabStop = False
        '
        'TxbIconChangerSelect
        '
        Me.TxbIconChangerSelect.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbIconChangerSelect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbIconChangerSelect.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbIconChangerSelect.ForeColor = System.Drawing.Color.White
        Me.TxbIconChangerSelect.Location = New System.Drawing.Point(92, 46)
        Me.TxbIconChangerSelect.Name = "TxbIconChangerSelect"
        Me.TxbIconChangerSelect.ReadOnly = True
        Me.TxbIconChangerSelect.Size = New System.Drawing.Size(484, 25)
        Me.TxbIconChangerSelect.TabIndex = 12
        Me.TxbIconChangerSelect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'BtnIconChangerSelect
        '
        Me.BtnIconChangerSelect.BackColor = System.Drawing.Color.Transparent
        Me.BtnIconChangerSelect.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnIconChangerSelect.BorderColour = System.Drawing.Color.DimGray
        Me.BtnIconChangerSelect.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.BtnIconChangerSelect.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnIconChangerSelect.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnIconChangerSelect.Location = New System.Drawing.Point(11, 46)
        Me.BtnIconChangerSelect.Name = "BtnIconChangerSelect"
        Me.BtnIconChangerSelect.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnIconChangerSelect.ProgressColour = System.Drawing.Color.BlueViolet
        Me.BtnIconChangerSelect.Size = New System.Drawing.Size(75, 25)
        Me.BtnIconChangerSelect.TabIndex = 11
        Me.BtnIconChangerSelect.Text = "Browse"
        '
        'ChbIconChangerEnabled
        '
        Me.ChbIconChangerEnabled.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbIconChangerEnabled.BorderColour = System.Drawing.Color.DimGray
        Me.ChbIconChangerEnabled.Checked = True
        Me.ChbIconChangerEnabled.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbIconChangerEnabled.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbIconChangerEnabled.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbIconChangerEnabled.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbIconChangerEnabled.Location = New System.Drawing.Point(6, 6)
        Me.ChbIconChangerEnabled.Name = "ChbIconChangerEnabled"
        Me.ChbIconChangerEnabled.Size = New System.Drawing.Size(86, 22)
        Me.ChbIconChangerEnabled.TabIndex = 8
        Me.ChbIconChangerEnabled.Tag = "Types"
        Me.ChbIconChangerEnabled.Text = "Enabled"
        '
        'TpDependencies
        '
        Me.TpDependencies.BackColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TpDependencies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TpDependencies.Controls.Add(Me.LblDependenciesWarning)
        Me.TpDependencies.Controls.Add(Me.PnlDependenciesEnabled)
        Me.TpDependencies.Controls.Add(Me.ChbDependenciesEnabled)
        Me.TpDependencies.Location = New System.Drawing.Point(4, 36)
        Me.TpDependencies.Name = "TpDependencies"
        Me.TpDependencies.Padding = New System.Windows.Forms.Padding(3)
        Me.TpDependencies.Size = New System.Drawing.Size(671, 326)
        Me.TpDependencies.TabIndex = 2
        Me.TpDependencies.Text = "Dependencies"
        '
        'LblDependenciesWarning
        '
        Me.LblDependenciesWarning.AutoSize = True
        Me.LblDependenciesWarning.BackColor = System.Drawing.Color.Transparent
        Me.LblDependenciesWarning.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblDependenciesWarning.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblDependenciesWarning.ForeColor = System.Drawing.Color.LimeGreen
        Me.LblDependenciesWarning.Location = New System.Drawing.Point(98, 9)
        Me.LblDependenciesWarning.Name = "LblDependenciesWarning"
        Me.LblDependenciesWarning.Size = New System.Drawing.Size(198, 15)
        Me.LblDependenciesWarning.TabIndex = 42
        Me.LblDependenciesWarning.Text = "(Dependencies detection is enabled)"
        '
        'PnlDependenciesEnabled
        '
        Me.PnlDependenciesEnabled.Controls.Add(Me.CbxDependenciesEmbedded)
        Me.PnlDependenciesEnabled.Controls.Add(Me.RdbDependenciesEmbedded)
        Me.PnlDependenciesEnabled.Controls.Add(Me.RdbDependenciesMerged)
        Me.PnlDependenciesEnabled.Controls.Add(Me.BtnDependenciesDelete)
        Me.PnlDependenciesEnabled.Controls.Add(Me.BtnDependenciesAdd)
        Me.PnlDependenciesEnabled.Controls.Add(Me.LbxDependenciesAdd)
        Me.PnlDependenciesEnabled.Location = New System.Drawing.Point(6, 34)
        Me.PnlDependenciesEnabled.Name = "PnlDependenciesEnabled"
        Me.PnlDependenciesEnabled.Size = New System.Drawing.Size(657, 273)
        Me.PnlDependenciesEnabled.TabIndex = 41
        '
        'CbxDependenciesEmbedded
        '
        Me.CbxDependenciesEmbedded.ArrowColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.CbxDependenciesEmbedded.BackColor = System.Drawing.Color.Transparent
        Me.CbxDependenciesEmbedded.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.CbxDependenciesEmbedded.BorderColour = System.Drawing.Color.DimGray
        Me.CbxDependenciesEmbedded.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.CbxDependenciesEmbedded.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbxDependenciesEmbedded.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CbxDependenciesEmbedded.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CbxDependenciesEmbedded.Items.AddRange(New Object() {"Nothing", "Encrypt", "Compress", "Both"})
        Me.CbxDependenciesEmbedded.LineColour = System.Drawing.Color.BlueViolet
        Me.CbxDependenciesEmbedded.Location = New System.Drawing.Point(285, 1)
        Me.CbxDependenciesEmbedded.Name = "CbxDependenciesEmbedded"
        Me.CbxDependenciesEmbedded.Size = New System.Drawing.Size(90, 24)
        Me.CbxDependenciesEmbedded.SqaureColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.CbxDependenciesEmbedded.SqaureHoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CbxDependenciesEmbedded.TabIndex = 40
        Me.CbxDependenciesEmbedded.Visible = False
        '
        'RdbDependenciesEmbedded
        '
        Me.RdbDependenciesEmbedded.BaseColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.RdbDependenciesEmbedded.BorderColour = System.Drawing.Color.DimGray
        Me.RdbDependenciesEmbedded.Checked = False
        Me.RdbDependenciesEmbedded.CheckedColour = System.Drawing.Color.FromArgb(CType(CType(173, Byte), Integer), CType(CType(173, Byte), Integer), CType(CType(174, Byte), Integer))
        Me.RdbDependenciesEmbedded.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.RdbDependenciesEmbedded.Cursor = System.Windows.Forms.Cursors.Hand
        Me.RdbDependenciesEmbedded.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RdbDependenciesEmbedded.HighlightColour = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(49, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.RdbDependenciesEmbedded.Location = New System.Drawing.Point(179, 3)
        Me.RdbDependenciesEmbedded.Name = "RdbDependenciesEmbedded"
        Me.RdbDependenciesEmbedded.Size = New System.Drawing.Size(100, 18)
        Me.RdbDependenciesEmbedded.TabIndex = 8
        Me.RdbDependenciesEmbedded.Tag = ""
        Me.RdbDependenciesEmbedded.Text = "Embedding"
        '
        'RdbDependenciesMerged
        '
        Me.RdbDependenciesMerged.BaseColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.RdbDependenciesMerged.BorderColour = System.Drawing.Color.DimGray
        Me.RdbDependenciesMerged.Checked = True
        Me.RdbDependenciesMerged.CheckedColour = System.Drawing.Color.BlueViolet
        Me.RdbDependenciesMerged.CheckState = System.Windows.Forms.CheckState.Unchecked
        Me.RdbDependenciesMerged.Cursor = System.Windows.Forms.Cursors.Hand
        Me.RdbDependenciesMerged.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RdbDependenciesMerged.HighlightColour = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(49, Byte), Integer), CType(CType(51, Byte), Integer))
        Me.RdbDependenciesMerged.Location = New System.Drawing.Point(93, 3)
        Me.RdbDependenciesMerged.Name = "RdbDependenciesMerged"
        Me.RdbDependenciesMerged.Size = New System.Drawing.Size(80, 18)
        Me.RdbDependenciesMerged.TabIndex = 7
        Me.RdbDependenciesMerged.Tag = ""
        Me.RdbDependenciesMerged.Text = "Merging"
        '
        'BtnDependenciesDelete
        '
        Me.BtnDependenciesDelete.BackColor = System.Drawing.Color.Transparent
        Me.BtnDependenciesDelete.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnDependenciesDelete.BorderColour = System.Drawing.Color.DimGray
        Me.BtnDependenciesDelete.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnDependenciesDelete.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnDependenciesDelete.Location = New System.Drawing.Point(582, 0)
        Me.BtnDependenciesDelete.Name = "BtnDependenciesDelete"
        Me.BtnDependenciesDelete.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnDependenciesDelete.ProgressColour = System.Drawing.Color.BlueViolet
        Me.BtnDependenciesDelete.Size = New System.Drawing.Size(75, 25)
        Me.BtnDependenciesDelete.TabIndex = 5
        Me.BtnDependenciesDelete.Text = "Delete"
        '
        'BtnDependenciesAdd
        '
        Me.BtnDependenciesAdd.BackColor = System.Drawing.Color.Transparent
        Me.BtnDependenciesAdd.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnDependenciesAdd.BorderColour = System.Drawing.Color.DimGray
        Me.BtnDependenciesAdd.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnDependenciesAdd.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnDependenciesAdd.Location = New System.Drawing.Point(501, 0)
        Me.BtnDependenciesAdd.Name = "BtnDependenciesAdd"
        Me.BtnDependenciesAdd.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnDependenciesAdd.ProgressColour = System.Drawing.Color.BlueViolet
        Me.BtnDependenciesAdd.Size = New System.Drawing.Size(75, 25)
        Me.BtnDependenciesAdd.TabIndex = 4
        Me.BtnDependenciesAdd.Text = "Add"
        '
        'LbxDependenciesAdd
        '
        Me.LbxDependenciesAdd.ArrowColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.LbxDependenciesAdd.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.LbxDependenciesAdd.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.LbxDependenciesAdd.BorderColour = System.Drawing.Color.DimGray
        Me.LbxDependenciesAdd.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.LbxDependenciesAdd.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.LbxDependenciesAdd.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LbxDependenciesAdd.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LbxDependenciesAdd.ForeColor = System.Drawing.Color.White
        Me.LbxDependenciesAdd.FormattingEnabled = True
        Me.LbxDependenciesAdd.IntegralHeight = False
        Me.LbxDependenciesAdd.ItemHeight = 20
        Me.LbxDependenciesAdd.LineColour = System.Drawing.Color.FromArgb(CType(CType(23, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(151, Byte), Integer))
        Me.LbxDependenciesAdd.Location = New System.Drawing.Point(3, 31)
        Me.LbxDependenciesAdd.Name = "LbxDependenciesAdd"
        Me.LbxDependenciesAdd.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.LbxDependenciesAdd.Size = New System.Drawing.Size(651, 239)
        Me.LbxDependenciesAdd.SqaureColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.LbxDependenciesAdd.SqaureHoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.LbxDependenciesAdd.TabIndex = 3
        '
        'ChbDependenciesEnabled
        '
        Me.ChbDependenciesEnabled.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbDependenciesEnabled.BorderColour = System.Drawing.Color.DimGray
        Me.ChbDependenciesEnabled.Checked = True
        Me.ChbDependenciesEnabled.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbDependenciesEnabled.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbDependenciesEnabled.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbDependenciesEnabled.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbDependenciesEnabled.Location = New System.Drawing.Point(6, 6)
        Me.ChbDependenciesEnabled.Name = "ChbDependenciesEnabled"
        Me.ChbDependenciesEnabled.Size = New System.Drawing.Size(86, 22)
        Me.ChbDependenciesEnabled.TabIndex = 10
        Me.ChbDependenciesEnabled.Tag = "Types"
        Me.ChbDependenciesEnabled.Text = "Enabled"
        '
        'TpObfuscator
        '
        Me.TpObfuscator.BackColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TpObfuscator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TpObfuscator.Controls.Add(Me.PnlObfuscatorEnabled)
        Me.TpObfuscator.Controls.Add(Me.ChbObfuscatorEnabled)
        Me.TpObfuscator.Location = New System.Drawing.Point(4, 36)
        Me.TpObfuscator.Name = "TpObfuscator"
        Me.TpObfuscator.Padding = New System.Windows.Forms.Padding(3)
        Me.TpObfuscator.Size = New System.Drawing.Size(671, 326)
        Me.TpObfuscator.TabIndex = 5
        Me.TpObfuscator.Text = "Obfuscator"
        '
        'PnlObfuscatorEnabled
        '
        Me.PnlObfuscatorEnabled.Controls.Add(Me.GbxObfuscatorRenaming)
        Me.PnlObfuscatorEnabled.Controls.Add(Me.GbxObfuscatorAdvanced)
        Me.PnlObfuscatorEnabled.Location = New System.Drawing.Point(6, 34)
        Me.PnlObfuscatorEnabled.Name = "PnlObfuscatorEnabled"
        Me.PnlObfuscatorEnabled.Size = New System.Drawing.Size(657, 284)
        Me.PnlObfuscatorEnabled.TabIndex = 38
        '
        'GbxObfuscatorRenaming
        '
        Me.GbxObfuscatorRenaming.BorderColour = System.Drawing.SystemColors.ButtonShadow
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorResourcesContent)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.PnlObfuscatorNamespacesGroup)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorAttributesRP)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorFieldsRP)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorEventsRP)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorPropertiesRP)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorMethodsRP)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorTypesRP)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.ChbObfuscatorNamespacesRP)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.CbxObfuscatorScheme)
        Me.GbxObfuscatorRenaming.Controls.Add(Me.LblObfuscatorScheme)
        Me.GbxObfuscatorRenaming.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GbxObfuscatorRenaming.HeaderColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.GbxObfuscatorRenaming.Location = New System.Drawing.Point(3, 0)
        Me.GbxObfuscatorRenaming.MainColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.GbxObfuscatorRenaming.Name = "GbxObfuscatorRenaming"
        Me.GbxObfuscatorRenaming.Size = New System.Drawing.Size(324, 281)
        Me.GbxObfuscatorRenaming.TabIndex = 56
        Me.GbxObfuscatorRenaming.Text = "Renaming"
        Me.GbxObfuscatorRenaming.TextColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'ChbObfuscatorResourcesContent
        '
        Me.ChbObfuscatorResourcesContent.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorResourcesContent.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorResourcesContent.Checked = True
        Me.ChbObfuscatorResourcesContent.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorResourcesContent.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorResourcesContent.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorResourcesContent.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorResourcesContent.Location = New System.Drawing.Point(22, 235)
        Me.ChbObfuscatorResourcesContent.Name = "ChbObfuscatorResourcesContent"
        Me.ChbObfuscatorResourcesContent.Size = New System.Drawing.Size(164, 22)
        Me.ChbObfuscatorResourcesContent.TabIndex = 56
        Me.ChbObfuscatorResourcesContent.Tag = "Resources"
        Me.ChbObfuscatorResourcesContent.Text = "Resources content"
        '
        'PnlObfuscatorNamespacesGroup
        '
        Me.PnlObfuscatorNamespacesGroup.Controls.Add(Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces)
        Me.PnlObfuscatorNamespacesGroup.Controls.Add(Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces)
        Me.PnlObfuscatorNamespacesGroup.Location = New System.Drawing.Point(46, 110)
        Me.PnlObfuscatorNamespacesGroup.Name = "PnlObfuscatorNamespacesGroup"
        Me.PnlObfuscatorNamespacesGroup.Size = New System.Drawing.Size(268, 49)
        Me.PnlObfuscatorNamespacesGroup.TabIndex = 55
        '
        'ChbObfuscatorReplaceNamespaceByEmptyNamespaces
        '
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Checked = True
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Location = New System.Drawing.Point(0, 28)
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Name = "ChbObfuscatorReplaceNamespaceByEmptyNamespaces"
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Size = New System.Drawing.Size(258, 22)
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.TabIndex = 3
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Tag = "1"
        Me.ChbObfuscatorReplaceNamespaceByEmptyNamespaces.Text = "Replace namespace(s) by empty value"
        '
        'ChbObfuscatorRenameMainNamespaceOnlyNamespaces
        '
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Checked = False
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Location = New System.Drawing.Point(0, 0)
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Name = "ChbObfuscatorRenameMainNamespaceOnlyNamespaces"
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Size = New System.Drawing.Size(245, 22)
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.TabIndex = 2
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Tag = "0"
        Me.ChbObfuscatorRenameMainNamespaceOnlyNamespaces.Text = "Rename the main namespace only "
        '
        'ChbObfuscatorAttributesRP
        '
        Me.ChbObfuscatorAttributesRP.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorAttributesRP.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorAttributesRP.Checked = True
        Me.ChbObfuscatorAttributesRP.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorAttributesRP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorAttributesRP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorAttributesRP.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorAttributesRP.Location = New System.Drawing.Point(204, 207)
        Me.ChbObfuscatorAttributesRP.Name = "ChbObfuscatorAttributesRP"
        Me.ChbObfuscatorAttributesRP.Size = New System.Drawing.Size(83, 22)
        Me.ChbObfuscatorAttributesRP.TabIndex = 10
        Me.ChbObfuscatorAttributesRP.Tag = "Attributes"
        Me.ChbObfuscatorAttributesRP.Text = "Attributes"
        '
        'ChbObfuscatorFieldsRP
        '
        Me.ChbObfuscatorFieldsRP.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorFieldsRP.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorFieldsRP.Checked = True
        Me.ChbObfuscatorFieldsRP.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorFieldsRP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorFieldsRP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorFieldsRP.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorFieldsRP.Location = New System.Drawing.Point(111, 207)
        Me.ChbObfuscatorFieldsRP.Name = "ChbObfuscatorFieldsRP"
        Me.ChbObfuscatorFieldsRP.Size = New System.Drawing.Size(83, 22)
        Me.ChbObfuscatorFieldsRP.TabIndex = 9
        Me.ChbObfuscatorFieldsRP.Tag = "Fields"
        Me.ChbObfuscatorFieldsRP.Text = "Fields"
        '
        'ChbObfuscatorEventsRP
        '
        Me.ChbObfuscatorEventsRP.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorEventsRP.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorEventsRP.Checked = True
        Me.ChbObfuscatorEventsRP.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorEventsRP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorEventsRP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorEventsRP.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorEventsRP.Location = New System.Drawing.Point(22, 207)
        Me.ChbObfuscatorEventsRP.Name = "ChbObfuscatorEventsRP"
        Me.ChbObfuscatorEventsRP.Size = New System.Drawing.Size(83, 22)
        Me.ChbObfuscatorEventsRP.TabIndex = 8
        Me.ChbObfuscatorEventsRP.Tag = "Events"
        Me.ChbObfuscatorEventsRP.Text = "Events"
        '
        'ChbObfuscatorPropertiesRP
        '
        Me.ChbObfuscatorPropertiesRP.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorPropertiesRP.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorPropertiesRP.Checked = True
        Me.ChbObfuscatorPropertiesRP.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorPropertiesRP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorPropertiesRP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorPropertiesRP.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorPropertiesRP.Location = New System.Drawing.Point(204, 179)
        Me.ChbObfuscatorPropertiesRP.Name = "ChbObfuscatorPropertiesRP"
        Me.ChbObfuscatorPropertiesRP.Size = New System.Drawing.Size(100, 22)
        Me.ChbObfuscatorPropertiesRP.TabIndex = 49
        Me.ChbObfuscatorPropertiesRP.Tag = "Properties"
        Me.ChbObfuscatorPropertiesRP.Text = "Properties"
        '
        'ChbObfuscatorMethodsRP
        '
        Me.ChbObfuscatorMethodsRP.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorMethodsRP.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorMethodsRP.Checked = True
        Me.ChbObfuscatorMethodsRP.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorMethodsRP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorMethodsRP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorMethodsRP.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorMethodsRP.Location = New System.Drawing.Point(111, 179)
        Me.ChbObfuscatorMethodsRP.Name = "ChbObfuscatorMethodsRP"
        Me.ChbObfuscatorMethodsRP.Size = New System.Drawing.Size(83, 22)
        Me.ChbObfuscatorMethodsRP.TabIndex = 6
        Me.ChbObfuscatorMethodsRP.Tag = "Methods"
        Me.ChbObfuscatorMethodsRP.Text = "Methods"
        '
        'ChbObfuscatorTypesRP
        '
        Me.ChbObfuscatorTypesRP.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorTypesRP.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorTypesRP.Checked = True
        Me.ChbObfuscatorTypesRP.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorTypesRP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorTypesRP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorTypesRP.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorTypesRP.Location = New System.Drawing.Point(22, 179)
        Me.ChbObfuscatorTypesRP.Name = "ChbObfuscatorTypesRP"
        Me.ChbObfuscatorTypesRP.Size = New System.Drawing.Size(83, 22)
        Me.ChbObfuscatorTypesRP.TabIndex = 5
        Me.ChbObfuscatorTypesRP.Tag = "Types"
        Me.ChbObfuscatorTypesRP.Text = "Types"
        '
        'ChbObfuscatorNamespacesRP
        '
        Me.ChbObfuscatorNamespacesRP.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorNamespacesRP.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorNamespacesRP.Checked = True
        Me.ChbObfuscatorNamespacesRP.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorNamespacesRP.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorNamespacesRP.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorNamespacesRP.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorNamespacesRP.Location = New System.Drawing.Point(22, 82)
        Me.ChbObfuscatorNamespacesRP.Name = "ChbObfuscatorNamespacesRP"
        Me.ChbObfuscatorNamespacesRP.Size = New System.Drawing.Size(124, 22)
        Me.ChbObfuscatorNamespacesRP.TabIndex = 4
        Me.ChbObfuscatorNamespacesRP.Tag = "Namespaces"
        Me.ChbObfuscatorNamespacesRP.Text = "Namespaces"
        '
        'CbxObfuscatorScheme
        '
        Me.CbxObfuscatorScheme.ArrowColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.CbxObfuscatorScheme.BackColor = System.Drawing.Color.Transparent
        Me.CbxObfuscatorScheme.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.CbxObfuscatorScheme.BorderColour = System.Drawing.Color.DimGray
        Me.CbxObfuscatorScheme.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.CbxObfuscatorScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbxObfuscatorScheme.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CbxObfuscatorScheme.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CbxObfuscatorScheme.Items.AddRange(New Object() {"Alphabetic", "Dots", "Invisible", "Chinese", "Japanese", "Greek", "Symbols", "Flowing"})
        Me.CbxObfuscatorScheme.LineColour = System.Drawing.Color.BlueViolet
        Me.CbxObfuscatorScheme.Location = New System.Drawing.Point(140, 40)
        Me.CbxObfuscatorScheme.Name = "CbxObfuscatorScheme"
        Me.CbxObfuscatorScheme.Size = New System.Drawing.Size(97, 24)
        Me.CbxObfuscatorScheme.SqaureColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.CbxObfuscatorScheme.SqaureHoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.CbxObfuscatorScheme.TabIndex = 39
        '
        'LblObfuscatorScheme
        '
        Me.LblObfuscatorScheme.AutoSize = True
        Me.LblObfuscatorScheme.BackColor = System.Drawing.Color.Transparent
        Me.LblObfuscatorScheme.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblObfuscatorScheme.ForeColor = System.Drawing.Color.White
        Me.LblObfuscatorScheme.Location = New System.Drawing.Point(79, 43)
        Me.LblObfuscatorScheme.Name = "LblObfuscatorScheme"
        Me.LblObfuscatorScheme.Size = New System.Drawing.Size(55, 15)
        Me.LblObfuscatorScheme.TabIndex = 40
        Me.LblObfuscatorScheme.Text = "Scheme :"
        '
        'GbxObfuscatorAdvanced
        '
        Me.GbxObfuscatorAdvanced.BorderColour = System.Drawing.SystemColors.ButtonShadow
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorResourcesEncryption)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorControlFlow)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorAntiDumper)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorBooleanEncrypt)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorHideCalls)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorResourcesCompress)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorAntiTamper)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorAntiIlDasm)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorIntegersEncode)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorStringsEncrypt)
        Me.GbxObfuscatorAdvanced.Controls.Add(Me.ChbObfuscatorAntiDebug)
        Me.GbxObfuscatorAdvanced.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GbxObfuscatorAdvanced.HeaderColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.GbxObfuscatorAdvanced.Location = New System.Drawing.Point(333, 0)
        Me.GbxObfuscatorAdvanced.MainColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.GbxObfuscatorAdvanced.Name = "GbxObfuscatorAdvanced"
        Me.GbxObfuscatorAdvanced.Size = New System.Drawing.Size(324, 281)
        Me.GbxObfuscatorAdvanced.TabIndex = 55
        Me.GbxObfuscatorAdvanced.Text = "Advanced"
        Me.GbxObfuscatorAdvanced.TextColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'ChbObfuscatorResourcesEncryption
        '
        Me.ChbObfuscatorResourcesEncryption.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorResourcesEncryption.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorResourcesEncryption.Checked = True
        Me.ChbObfuscatorResourcesEncryption.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorResourcesEncryption.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorResourcesEncryption.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorResourcesEncryption.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorResourcesEncryption.Location = New System.Drawing.Point(12, 44)
        Me.ChbObfuscatorResourcesEncryption.Name = "ChbObfuscatorResourcesEncryption"
        Me.ChbObfuscatorResourcesEncryption.Size = New System.Drawing.Size(142, 22)
        Me.ChbObfuscatorResourcesEncryption.TabIndex = 62
        Me.ChbObfuscatorResourcesEncryption.Tag = "Parameters"
        Me.ChbObfuscatorResourcesEncryption.Text = "Resources encryption"
        '
        'ChbObfuscatorControlFlow
        '
        Me.ChbObfuscatorControlFlow.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorControlFlow.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorControlFlow.Checked = True
        Me.ChbObfuscatorControlFlow.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorControlFlow.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorControlFlow.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorControlFlow.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorControlFlow.Location = New System.Drawing.Point(12, 238)
        Me.ChbObfuscatorControlFlow.Name = "ChbObfuscatorControlFlow"
        Me.ChbObfuscatorControlFlow.Size = New System.Drawing.Size(112, 22)
        Me.ChbObfuscatorControlFlow.TabIndex = 61
        Me.ChbObfuscatorControlFlow.Tag = "Parameters"
        Me.ChbObfuscatorControlFlow.Text = "Controlflow"
        '
        'ChbObfuscatorAntiDumper
        '
        Me.ChbObfuscatorAntiDumper.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorAntiDumper.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorAntiDumper.Checked = True
        Me.ChbObfuscatorAntiDumper.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorAntiDumper.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorAntiDumper.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorAntiDumper.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorAntiDumper.Location = New System.Drawing.Point(12, 193)
        Me.ChbObfuscatorAntiDumper.Name = "ChbObfuscatorAntiDumper"
        Me.ChbObfuscatorAntiDumper.Size = New System.Drawing.Size(94, 22)
        Me.ChbObfuscatorAntiDumper.TabIndex = 57
        Me.ChbObfuscatorAntiDumper.Tag = "Parameters"
        Me.ChbObfuscatorAntiDumper.Text = "Anti-Dumper"
        '
        'ChbObfuscatorBooleanEncrypt
        '
        Me.ChbObfuscatorBooleanEncrypt.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorBooleanEncrypt.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorBooleanEncrypt.Checked = True
        Me.ChbObfuscatorBooleanEncrypt.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorBooleanEncrypt.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorBooleanEncrypt.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorBooleanEncrypt.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorBooleanEncrypt.Location = New System.Drawing.Point(164, 89)
        Me.ChbObfuscatorBooleanEncrypt.Name = "ChbObfuscatorBooleanEncrypt"
        Me.ChbObfuscatorBooleanEncrypt.Size = New System.Drawing.Size(141, 22)
        Me.ChbObfuscatorBooleanEncrypt.TabIndex = 56
        Me.ChbObfuscatorBooleanEncrypt.Tag = "Parameters"
        Me.ChbObfuscatorBooleanEncrypt.Text = "Booleans encryption"
        '
        'ChbObfuscatorHideCalls
        '
        Me.ChbObfuscatorHideCalls.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorHideCalls.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorHideCalls.Checked = True
        Me.ChbObfuscatorHideCalls.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorHideCalls.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorHideCalls.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorHideCalls.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorHideCalls.Location = New System.Drawing.Point(164, 118)
        Me.ChbObfuscatorHideCalls.Name = "ChbObfuscatorHideCalls"
        Me.ChbObfuscatorHideCalls.Size = New System.Drawing.Size(91, 22)
        Me.ChbObfuscatorHideCalls.TabIndex = 55
        Me.ChbObfuscatorHideCalls.Tag = "Parameters"
        Me.ChbObfuscatorHideCalls.Text = "Hide calls"
        '
        'ChbObfuscatorResourcesCompress
        '
        Me.ChbObfuscatorResourcesCompress.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorResourcesCompress.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorResourcesCompress.Checked = True
        Me.ChbObfuscatorResourcesCompress.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorResourcesCompress.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorResourcesCompress.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorResourcesCompress.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorResourcesCompress.Location = New System.Drawing.Point(164, 44)
        Me.ChbObfuscatorResourcesCompress.Name = "ChbObfuscatorResourcesCompress"
        Me.ChbObfuscatorResourcesCompress.Size = New System.Drawing.Size(156, 22)
        Me.ChbObfuscatorResourcesCompress.TabIndex = 51
        Me.ChbObfuscatorResourcesCompress.Tag = "Parameters"
        Me.ChbObfuscatorResourcesCompress.Text = "Resources compression"
        '
        'ChbObfuscatorAntiTamper
        '
        Me.ChbObfuscatorAntiTamper.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorAntiTamper.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorAntiTamper.Checked = True
        Me.ChbObfuscatorAntiTamper.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorAntiTamper.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorAntiTamper.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorAntiTamper.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorAntiTamper.Location = New System.Drawing.Point(164, 193)
        Me.ChbObfuscatorAntiTamper.Name = "ChbObfuscatorAntiTamper"
        Me.ChbObfuscatorAntiTamper.Size = New System.Drawing.Size(91, 22)
        Me.ChbObfuscatorAntiTamper.TabIndex = 52
        Me.ChbObfuscatorAntiTamper.Tag = "Parameters"
        Me.ChbObfuscatorAntiTamper.Text = "Anti-Tamper"
        '
        'ChbObfuscatorAntiIlDasm
        '
        Me.ChbObfuscatorAntiIlDasm.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorAntiIlDasm.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorAntiIlDasm.Checked = True
        Me.ChbObfuscatorAntiIlDasm.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorAntiIlDasm.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorAntiIlDasm.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorAntiIlDasm.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorAntiIlDasm.Location = New System.Drawing.Point(164, 165)
        Me.ChbObfuscatorAntiIlDasm.Name = "ChbObfuscatorAntiIlDasm"
        Me.ChbObfuscatorAntiIlDasm.Size = New System.Drawing.Size(91, 22)
        Me.ChbObfuscatorAntiIlDasm.TabIndex = 54
        Me.ChbObfuscatorAntiIlDasm.Tag = "Parameters"
        Me.ChbObfuscatorAntiIlDasm.Text = "Anti-ILDasm"
        '
        'ChbObfuscatorIntegersEncode
        '
        Me.ChbObfuscatorIntegersEncode.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorIntegersEncode.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorIntegersEncode.Checked = True
        Me.ChbObfuscatorIntegersEncode.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorIntegersEncode.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorIntegersEncode.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorIntegersEncode.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorIntegersEncode.Location = New System.Drawing.Point(12, 118)
        Me.ChbObfuscatorIntegersEncode.Name = "ChbObfuscatorIntegersEncode"
        Me.ChbObfuscatorIntegersEncode.Size = New System.Drawing.Size(142, 22)
        Me.ChbObfuscatorIntegersEncode.TabIndex = 50
        Me.ChbObfuscatorIntegersEncode.Tag = "Parameters"
        Me.ChbObfuscatorIntegersEncode.Text = "Numerics encoding"
        '
        'ChbObfuscatorStringsEncrypt
        '
        Me.ChbObfuscatorStringsEncrypt.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorStringsEncrypt.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorStringsEncrypt.Checked = True
        Me.ChbObfuscatorStringsEncrypt.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorStringsEncrypt.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorStringsEncrypt.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorStringsEncrypt.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorStringsEncrypt.Location = New System.Drawing.Point(12, 89)
        Me.ChbObfuscatorStringsEncrypt.Name = "ChbObfuscatorStringsEncrypt"
        Me.ChbObfuscatorStringsEncrypt.Size = New System.Drawing.Size(130, 22)
        Me.ChbObfuscatorStringsEncrypt.TabIndex = 49
        Me.ChbObfuscatorStringsEncrypt.Tag = "Parameters"
        Me.ChbObfuscatorStringsEncrypt.Text = "Strings encryption"
        '
        'ChbObfuscatorAntiDebug
        '
        Me.ChbObfuscatorAntiDebug.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorAntiDebug.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorAntiDebug.Checked = True
        Me.ChbObfuscatorAntiDebug.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorAntiDebug.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorAntiDebug.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorAntiDebug.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorAntiDebug.Location = New System.Drawing.Point(12, 164)
        Me.ChbObfuscatorAntiDebug.Name = "ChbObfuscatorAntiDebug"
        Me.ChbObfuscatorAntiDebug.Size = New System.Drawing.Size(94, 22)
        Me.ChbObfuscatorAntiDebug.TabIndex = 53
        Me.ChbObfuscatorAntiDebug.Tag = "Parameters"
        Me.ChbObfuscatorAntiDebug.Text = "Anti-Debug"
        '
        'ChbObfuscatorEnabled
        '
        Me.ChbObfuscatorEnabled.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbObfuscatorEnabled.BorderColour = System.Drawing.Color.DimGray
        Me.ChbObfuscatorEnabled.Checked = True
        Me.ChbObfuscatorEnabled.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbObfuscatorEnabled.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbObfuscatorEnabled.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbObfuscatorEnabled.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbObfuscatorEnabled.Location = New System.Drawing.Point(6, 6)
        Me.ChbObfuscatorEnabled.Name = "ChbObfuscatorEnabled"
        Me.ChbObfuscatorEnabled.Size = New System.Drawing.Size(86, 22)
        Me.ChbObfuscatorEnabled.TabIndex = 37
        Me.ChbObfuscatorEnabled.Tag = "Types"
        Me.ChbObfuscatorEnabled.Text = "Enabled"
        '
        'TpPacker
        '
        Me.TpPacker.BackColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.TpPacker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TpPacker.Controls.Add(Me.LblPackerWarning)
        Me.TpPacker.Controls.Add(Me.PnlPackerEnabled)
        Me.TpPacker.Controls.Add(Me.ChbPackerEnabled)
        Me.TpPacker.Location = New System.Drawing.Point(4, 36)
        Me.TpPacker.Name = "TpPacker"
        Me.TpPacker.Padding = New System.Windows.Forms.Padding(3)
        Me.TpPacker.Size = New System.Drawing.Size(671, 326)
        Me.TpPacker.TabIndex = 4
        Me.TpPacker.Text = "Packer"
        '
        'LblPackerWarning
        '
        Me.LblPackerWarning.AutoSize = True
        Me.LblPackerWarning.BackColor = System.Drawing.Color.Transparent
        Me.LblPackerWarning.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblPackerWarning.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblPackerWarning.ForeColor = System.Drawing.Color.DarkOrange
        Me.LblPackerWarning.Location = New System.Drawing.Point(98, 9)
        Me.LblPackerWarning.Name = "LblPackerWarning"
        Me.LblPackerWarning.Size = New System.Drawing.Size(348, 15)
        Me.LblPackerWarning.TabIndex = 40
        Me.LblPackerWarning.Text = "(You must enable the detection of dependencies before packing)"
        Me.LblPackerWarning.Visible = False
        '
        'PnlPackerEnabled
        '
        Me.PnlPackerEnabled.Controls.Add(Me.GbxPackerLoader)
        Me.PnlPackerEnabled.Location = New System.Drawing.Point(6, 34)
        Me.PnlPackerEnabled.Name = "PnlPackerEnabled"
        Me.PnlPackerEnabled.Size = New System.Drawing.Size(657, 273)
        Me.PnlPackerEnabled.TabIndex = 39
        '
        'GbxPackerLoader
        '
        Me.GbxPackerLoader.BorderColour = System.Drawing.SystemColors.ButtonShadow
        Me.GbxPackerLoader.Controls.Add(Me.TxbPackerFramework)
        Me.GbxPackerLoader.Controls.Add(Me.LblPackerFramework)
        Me.GbxPackerLoader.Controls.Add(Me.LblPackerSystem)
        Me.GbxPackerLoader.Controls.Add(Me.LblPackerPlatform)
        Me.GbxPackerLoader.Controls.Add(Me.TxbPackerPlatform)
        Me.GbxPackerLoader.Controls.Add(Me.TxbPackerSystem)
        Me.GbxPackerLoader.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GbxPackerLoader.HeaderColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.GbxPackerLoader.Location = New System.Drawing.Point(0, 0)
        Me.GbxPackerLoader.MainColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.GbxPackerLoader.Name = "GbxPackerLoader"
        Me.GbxPackerLoader.Size = New System.Drawing.Size(657, 93)
        Me.GbxPackerLoader.TabIndex = 62
        Me.GbxPackerLoader.Text = "Stub"
        Me.GbxPackerLoader.TextColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'TxbPackerFramework
        '
        Me.TxbPackerFramework.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbPackerFramework.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbPackerFramework.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbPackerFramework.ForeColor = System.Drawing.Color.White
        Me.TxbPackerFramework.Location = New System.Drawing.Point(96, 47)
        Me.TxbPackerFramework.Name = "TxbPackerFramework"
        Me.TxbPackerFramework.ReadOnly = True
        Me.TxbPackerFramework.Size = New System.Drawing.Size(120, 25)
        Me.TxbPackerFramework.TabIndex = 63
        Me.TxbPackerFramework.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LblPackerFramework
        '
        Me.LblPackerFramework.AutoSize = True
        Me.LblPackerFramework.BackColor = System.Drawing.Color.Transparent
        Me.LblPackerFramework.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblPackerFramework.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblPackerFramework.ForeColor = System.Drawing.Color.White
        Me.LblPackerFramework.Location = New System.Drawing.Point(18, 51)
        Me.LblPackerFramework.Name = "LblPackerFramework"
        Me.LblPackerFramework.Size = New System.Drawing.Size(72, 15)
        Me.LblPackerFramework.TabIndex = 11
        Me.LblPackerFramework.Text = "Framework :"
        '
        'LblPackerSystem
        '
        Me.LblPackerSystem.AutoSize = True
        Me.LblPackerSystem.BackColor = System.Drawing.Color.Transparent
        Me.LblPackerSystem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblPackerSystem.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblPackerSystem.ForeColor = System.Drawing.Color.White
        Me.LblPackerSystem.Location = New System.Drawing.Point(237, 51)
        Me.LblPackerSystem.Name = "LblPackerSystem"
        Me.LblPackerSystem.Size = New System.Drawing.Size(55, 15)
        Me.LblPackerSystem.TabIndex = 12
        Me.LblPackerSystem.Text = "ExeType :"
        '
        'LblPackerPlatform
        '
        Me.LblPackerPlatform.AutoSize = True
        Me.LblPackerPlatform.BackColor = System.Drawing.Color.Transparent
        Me.LblPackerPlatform.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblPackerPlatform.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblPackerPlatform.ForeColor = System.Drawing.Color.White
        Me.LblPackerPlatform.Location = New System.Drawing.Point(445, 51)
        Me.LblPackerPlatform.Name = "LblPackerPlatform"
        Me.LblPackerPlatform.Size = New System.Drawing.Size(59, 15)
        Me.LblPackerPlatform.TabIndex = 13
        Me.LblPackerPlatform.Text = "Platform :"
        '
        'TxbPackerPlatform
        '
        Me.TxbPackerPlatform.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbPackerPlatform.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbPackerPlatform.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbPackerPlatform.ForeColor = System.Drawing.Color.White
        Me.TxbPackerPlatform.Location = New System.Drawing.Point(506, 47)
        Me.TxbPackerPlatform.Name = "TxbPackerPlatform"
        Me.TxbPackerPlatform.ReadOnly = True
        Me.TxbPackerPlatform.Size = New System.Drawing.Size(126, 25)
        Me.TxbPackerPlatform.TabIndex = 53
        Me.TxbPackerPlatform.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbPackerSystem
        '
        Me.TxbPackerSystem.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbPackerSystem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbPackerSystem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbPackerSystem.ForeColor = System.Drawing.Color.White
        Me.TxbPackerSystem.Location = New System.Drawing.Point(299, 47)
        Me.TxbPackerSystem.Name = "TxbPackerSystem"
        Me.TxbPackerSystem.ReadOnly = True
        Me.TxbPackerSystem.Size = New System.Drawing.Size(126, 25)
        Me.TxbPackerSystem.TabIndex = 52
        Me.TxbPackerSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ChbPackerEnabled
        '
        Me.ChbPackerEnabled.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.ChbPackerEnabled.BorderColour = System.Drawing.Color.DimGray
        Me.ChbPackerEnabled.Checked = False
        Me.ChbPackerEnabled.CheckedColour = System.Drawing.Color.BlueViolet
        Me.ChbPackerEnabled.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ChbPackerEnabled.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ChbPackerEnabled.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ChbPackerEnabled.Location = New System.Drawing.Point(6, 6)
        Me.ChbPackerEnabled.Name = "ChbPackerEnabled"
        Me.ChbPackerEnabled.Size = New System.Drawing.Size(86, 22)
        Me.ChbPackerEnabled.TabIndex = 8
        Me.ChbPackerEnabled.Tag = "Types"
        Me.ChbPackerEnabled.Text = "Enabled"
        '
        'GbxDetection
        '
        Me.GbxDetection.BorderColour = System.Drawing.SystemColors.ButtonShadow
        Me.GbxDetection.Controls.Add(Me.PcbDetection)
        Me.GbxDetection.Controls.Add(Me.TxbDetection)
        Me.GbxDetection.Controls.Add(Me.LblDetection)
        Me.GbxDetection.Enabled = False
        Me.GbxDetection.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GbxDetection.HeaderColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.GbxDetection.Location = New System.Drawing.Point(12, 154)
        Me.GbxDetection.MainColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.GbxDetection.Name = "GbxDetection"
        Me.GbxDetection.Size = New System.Drawing.Size(679, 95)
        Me.GbxDetection.TabIndex = 15
        Me.GbxDetection.Text = "Obfuscator/Packer detection"
        Me.GbxDetection.TextColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'PcbDetection
        '
        Me.PcbDetection.BackColor = System.Drawing.Color.Transparent
        Me.PcbDetection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PcbDetection.Location = New System.Drawing.Point(605, 36)
        Me.PcbDetection.Name = "PcbDetection"
        Me.PcbDetection.Size = New System.Drawing.Size(48, 48)
        Me.PcbDetection.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PcbDetection.TabIndex = 15
        Me.PcbDetection.TabStop = False
        '
        'TxbDetection
        '
        Me.TxbDetection.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbDetection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbDetection.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbDetection.ForeColor = System.Drawing.Color.White
        Me.TxbDetection.Location = New System.Drawing.Point(103, 46)
        Me.TxbDetection.Name = "TxbDetection"
        Me.TxbDetection.ReadOnly = True
        Me.TxbDetection.Size = New System.Drawing.Size(484, 25)
        Me.TxbDetection.TabIndex = 9
        Me.TxbDetection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LblDetection
        '
        Me.LblDetection.AutoSize = True
        Me.LblDetection.BackColor = System.Drawing.Color.Transparent
        Me.LblDetection.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblDetection.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblDetection.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblDetection.Location = New System.Drawing.Point(52, 50)
        Me.LblDetection.Name = "LblDetection"
        Me.LblDetection.Size = New System.Drawing.Size(45, 15)
        Me.LblDetection.TabIndex = 2
        Me.LblDetection.Text = "Result :"
        '
        'GbxSelectFile
        '
        Me.GbxSelectFile.BorderColour = System.Drawing.SystemColors.ButtonShadow
        Me.GbxSelectFile.Controls.Add(Me.LblType)
        Me.GbxSelectFile.Controls.Add(Me.TxbSelectedFile)
        Me.GbxSelectFile.Controls.Add(Me.TxbType)
        Me.GbxSelectFile.Controls.Add(Me.PbxSelectedFile)
        Me.GbxSelectFile.Controls.Add(Me.LblCpuTargetInfo)
        Me.GbxSelectFile.Controls.Add(Me.BtnSelectFile)
        Me.GbxSelectFile.Controls.Add(Me.TxbCpuTargetInfo)
        Me.GbxSelectFile.Controls.Add(Me.LblVersionInfo)
        Me.GbxSelectFile.Controls.Add(Me.TxbFrameworkInfo)
        Me.GbxSelectFile.Controls.Add(Me.TxbVersionInfo)
        Me.GbxSelectFile.Controls.Add(Me.LblFrameworkInfo)
        Me.GbxSelectFile.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GbxSelectFile.HeaderColour = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.GbxSelectFile.Location = New System.Drawing.Point(12, 46)
        Me.GbxSelectFile.MainColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.GbxSelectFile.Name = "GbxSelectFile"
        Me.GbxSelectFile.Size = New System.Drawing.Size(679, 102)
        Me.GbxSelectFile.TabIndex = 1
        Me.GbxSelectFile.Text = "Select .Net file (C#, VbNet)"
        Me.GbxSelectFile.TextColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        '
        'LblType
        '
        Me.LblType.AutoSize = True
        Me.LblType.BackColor = System.Drawing.Color.Transparent
        Me.LblType.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblType.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblType.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblType.Location = New System.Drawing.Point(472, 72)
        Me.LblType.Name = "LblType"
        Me.LblType.Size = New System.Drawing.Size(37, 15)
        Me.LblType.TabIndex = 13
        Me.LblType.Text = "Type :"
        '
        'TxbSelectedFile
        '
        Me.TxbSelectedFile.AllowDrop = True
        Me.TxbSelectedFile.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbSelectedFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbSelectedFile.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbSelectedFile.ForeColor = System.Drawing.Color.White
        Me.TxbSelectedFile.Location = New System.Drawing.Point(103, 37)
        Me.TxbSelectedFile.Name = "TxbSelectedFile"
        Me.TxbSelectedFile.ReadOnly = True
        Me.TxbSelectedFile.Size = New System.Drawing.Size(484, 25)
        Me.TxbSelectedFile.TabIndex = 10
        Me.TxbSelectedFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbType
        '
        Me.TxbType.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbType.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbType.ForeColor = System.Drawing.Color.White
        Me.TxbType.Location = New System.Drawing.Point(517, 68)
        Me.TxbType.Name = "TxbType"
        Me.TxbType.ReadOnly = True
        Me.TxbType.Size = New System.Drawing.Size(70, 25)
        Me.TxbType.TabIndex = 14
        Me.TxbType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PbxSelectedFile
        '
        Me.PbxSelectedFile.BackColor = System.Drawing.Color.Transparent
        Me.PbxSelectedFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PbxSelectedFile.Location = New System.Drawing.Point(605, 39)
        Me.PbxSelectedFile.Name = "PbxSelectedFile"
        Me.PbxSelectedFile.Size = New System.Drawing.Size(48, 48)
        Me.PbxSelectedFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PbxSelectedFile.TabIndex = 2
        Me.PbxSelectedFile.TabStop = False
        '
        'LblCpuTargetInfo
        '
        Me.LblCpuTargetInfo.AutoSize = True
        Me.LblCpuTargetInfo.BackColor = System.Drawing.Color.Transparent
        Me.LblCpuTargetInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblCpuTargetInfo.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblCpuTargetInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblCpuTargetInfo.Location = New System.Drawing.Point(326, 72)
        Me.LblCpuTargetInfo.Name = "LblCpuTargetInfo"
        Me.LblCpuTargetInfo.Size = New System.Drawing.Size(70, 15)
        Me.LblCpuTargetInfo.TabIndex = 6
        Me.LblCpuTargetInfo.Text = "CPU target :"
        '
        'BtnSelectFile
        '
        Me.BtnSelectFile.BackColor = System.Drawing.Color.Transparent
        Me.BtnSelectFile.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnSelectFile.BorderColour = System.Drawing.Color.DimGray
        Me.BtnSelectFile.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.BtnSelectFile.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnSelectFile.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnSelectFile.Location = New System.Drawing.Point(22, 37)
        Me.BtnSelectFile.Name = "BtnSelectFile"
        Me.BtnSelectFile.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnSelectFile.ProgressColour = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BtnSelectFile.Size = New System.Drawing.Size(75, 25)
        Me.BtnSelectFile.TabIndex = 0
        Me.BtnSelectFile.Text = "Browse"
        '
        'TxbCpuTargetInfo
        '
        Me.TxbCpuTargetInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbCpuTargetInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbCpuTargetInfo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbCpuTargetInfo.ForeColor = System.Drawing.Color.White
        Me.TxbCpuTargetInfo.Location = New System.Drawing.Point(402, 68)
        Me.TxbCpuTargetInfo.Name = "TxbCpuTargetInfo"
        Me.TxbCpuTargetInfo.ReadOnly = True
        Me.TxbCpuTargetInfo.Size = New System.Drawing.Size(65, 25)
        Me.TxbCpuTargetInfo.TabIndex = 12
        Me.TxbCpuTargetInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LblVersionInfo
        '
        Me.LblVersionInfo.AutoSize = True
        Me.LblVersionInfo.BackColor = System.Drawing.Color.Transparent
        Me.LblVersionInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblVersionInfo.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblVersionInfo.Location = New System.Drawing.Point(45, 72)
        Me.LblVersionInfo.Name = "LblVersionInfo"
        Me.LblVersionInfo.Size = New System.Drawing.Size(51, 15)
        Me.LblVersionInfo.TabIndex = 4
        Me.LblVersionInfo.Text = "Version :"
        '
        'TxbFrameworkInfo
        '
        Me.TxbFrameworkInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbFrameworkInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbFrameworkInfo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbFrameworkInfo.ForeColor = System.Drawing.Color.White
        Me.TxbFrameworkInfo.Location = New System.Drawing.Point(256, 68)
        Me.TxbFrameworkInfo.Name = "TxbFrameworkInfo"
        Me.TxbFrameworkInfo.ReadOnly = True
        Me.TxbFrameworkInfo.Size = New System.Drawing.Size(65, 25)
        Me.TxbFrameworkInfo.TabIndex = 11
        Me.TxbFrameworkInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxbVersionInfo
        '
        Me.TxbVersionInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.TxbVersionInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxbVersionInfo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxbVersionInfo.ForeColor = System.Drawing.Color.White
        Me.TxbVersionInfo.Location = New System.Drawing.Point(103, 68)
        Me.TxbVersionInfo.Name = "TxbVersionInfo"
        Me.TxbVersionInfo.ReadOnly = True
        Me.TxbVersionInfo.Size = New System.Drawing.Size(70, 25)
        Me.TxbVersionInfo.TabIndex = 10
        Me.TxbVersionInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LblFrameworkInfo
        '
        Me.LblFrameworkInfo.AutoSize = True
        Me.LblFrameworkInfo.BackColor = System.Drawing.Color.Transparent
        Me.LblFrameworkInfo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblFrameworkInfo.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblFrameworkInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblFrameworkInfo.Location = New System.Drawing.Point(178, 72)
        Me.LblFrameworkInfo.Name = "LblFrameworkInfo"
        Me.LblFrameworkInfo.Size = New System.Drawing.Size(72, 15)
        Me.LblFrameworkInfo.TabIndex = 8
        Me.LblFrameworkInfo.Text = "Framework :"
        '
        'Frm_Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 697)
        Me.Controls.Add(Me.Frm_MainThemeContainer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Frm_Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DotNet Patcher"
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.Frm_MainThemeContainer.ResumeLayout(False)
        Me.Frm_MainThemeContainer.PerformLayout()
        Me.TbcTask.ResumeLayout(False)
        Me.TpAbout.ResumeLayout(False)
        Me.TpAbout.PerformLayout()
        CType(Me.PbxAboutLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TpVersionInfos.ResumeLayout(False)
        Me.PnlVersionInfosEnabled.ResumeLayout(False)
        Me.PnlVersionInfosEnabled.PerformLayout()
        Me.TpManifestChanger.ResumeLayout(False)
        Me.PnlManifestEnabled.ResumeLayout(False)
        Me.TpIconChanger.ResumeLayout(False)
        Me.PnlIconChangerEnabled.ResumeLayout(False)
        Me.PnlIconChangerEnabled.PerformLayout()
        CType(Me.PbxIconChangerSelect, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TpDependencies.ResumeLayout(False)
        Me.TpDependencies.PerformLayout()
        Me.PnlDependenciesEnabled.ResumeLayout(False)
        Me.TpObfuscator.ResumeLayout(False)
        Me.PnlObfuscatorEnabled.ResumeLayout(False)
        Me.GbxObfuscatorRenaming.ResumeLayout(False)
        Me.GbxObfuscatorRenaming.PerformLayout()
        Me.PnlObfuscatorNamespacesGroup.ResumeLayout(False)
        Me.GbxObfuscatorAdvanced.ResumeLayout(False)
        Me.TpPacker.ResumeLayout(False)
        Me.TpPacker.PerformLayout()
        Me.PnlPackerEnabled.ResumeLayout(False)
        Me.GbxPackerLoader.ResumeLayout(False)
        Me.GbxPackerLoader.PerformLayout()
        Me.GbxDetection.ResumeLayout(False)
        Me.GbxDetection.PerformLayout()
        CType(Me.PcbDetection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GbxSelectFile.ResumeLayout(False)
        Me.GbxSelectFile.PerformLayout()
        CType(Me.PbxSelectedFile, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Frm_MainThemeContainer As LogInThemeContainer
    Friend WithEvents GbxSelectFile As LogInGroupBox
    Friend WithEvents BtnSelectFile As LogInButton
    Friend WithEvents PbxSelectedFile As System.Windows.Forms.PictureBox
    Friend WithEvents LblCpuTargetInfo As LogInLabel
    Friend WithEvents LblVersionInfo As LogInLabel
    Friend WithEvents LblFrameworkInfo As LogInLabel
    Friend WithEvents TxbCpuTargetInfo As System.Windows.Forms.TextBox
    Friend WithEvents TxbFrameworkInfo As System.Windows.Forms.TextBox
    Friend WithEvents TxbVersionInfo As System.Windows.Forms.TextBox
    Friend WithEvents TxbSelectedFile As System.Windows.Forms.TextBox
    Friend WithEvents BtnStart As LogInButton
    Friend WithEvents LblType As LogInLabel
    Friend WithEvents TxbType As System.Windows.Forms.TextBox
    Friend WithEvents BgwRenameTask As System.ComponentModel.BackgroundWorker
    Friend WithEvents GbxDetection As LogInGroupBox
    Friend WithEvents TxbDetection As System.Windows.Forms.TextBox
    Friend WithEvents LblDetection As LogInLabel
    Friend WithEvents TbcTask As LogInTabControl
    Friend WithEvents TpAbout As System.Windows.Forms.TabPage
    Friend WithEvents TpVersionInfos As System.Windows.Forms.TabPage
    Friend WithEvents TpDependencies As System.Windows.Forms.TabPage
    Friend WithEvents TpManifestChanger As System.Windows.Forms.TabPage
    Friend WithEvents TpPacker As System.Windows.Forms.TabPage
    Friend WithEvents TpObfuscator As System.Windows.Forms.TabPage
    Friend WithEvents TpIconChanger As System.Windows.Forms.TabPage
    Friend WithEvents PbxAboutLogo As System.Windows.Forms.PictureBox
    Friend WithEvents ChbVersionInfosEnabled As LogInCheckBox
    Friend WithEvents PnlVersionInfosEnabled As System.Windows.Forms.Panel
    Friend WithEvents TxbVersionInfosVersion As System.Windows.Forms.TextBox
    Friend WithEvents TxbVersionInfosTrademark As System.Windows.Forms.TextBox
    Friend WithEvents TxbVersionInfosCopyright As System.Windows.Forms.TextBox
    Friend WithEvents TxbVersionInfosProduct As System.Windows.Forms.TextBox
    Friend WithEvents TxbVersionInfosCompany As System.Windows.Forms.TextBox
    Friend WithEvents TxbVersionInfosDescription As System.Windows.Forms.TextBox
    Friend WithEvents TxbVersionInfosTitle As System.Windows.Forms.TextBox
    Friend WithEvents ChbObfuscatorEnabled As LogInCheckBox
    Friend WithEvents ChbPackerEnabled As LogInCheckBox
    Friend WithEvents ChbIconChangerEnabled As LogInCheckBox
    Friend WithEvents PnlObfuscatorEnabled As System.Windows.Forms.Panel
    Friend WithEvents ChbObfuscatorNamespacesRP As LogInCheckBox
    Friend WithEvents ChbObfuscatorTypesRP As LogInCheckBox
    Friend WithEvents ChbObfuscatorMethodsRP As LogInCheckBox
    Friend WithEvents ChbObfuscatorEventsRP As LogInCheckBox
    Friend WithEvents ChbObfuscatorFieldsRP As LogInCheckBox
    Friend WithEvents ChbObfuscatorAttributesRP As LogInCheckBox
    Friend WithEvents LblObfuscatorScheme As System.Windows.Forms.Label
    Friend WithEvents CbxObfuscatorScheme As LogInComboBox
    Friend WithEvents ChbObfuscatorResourcesCompress As LogInCheckBox
    Friend WithEvents GbxObfuscatorAdvanced As LogInGroupBox
    Friend WithEvents ChbObfuscatorAntiIlDasm As LogInCheckBox
    Friend WithEvents ChbObfuscatorStringsEncrypt As LogInCheckBox
    Friend WithEvents ChbObfuscatorAntiDebug As LogInCheckBox
    Friend WithEvents ChbObfuscatorIntegersEncode As LogInCheckBox
    Friend WithEvents ChbObfuscatorAntiTamper As LogInCheckBox
    Friend WithEvents GbxObfuscatorRenaming As LogInGroupBox
    Friend WithEvents ChbObfuscatorPropertiesRP As LogInCheckBox
    Friend WithEvents PnlObfuscatorNamespacesGroup As System.Windows.Forms.Panel
    Friend WithEvents ChbObfuscatorReplaceNamespaceByEmptyNamespaces As LogInCheckBox
    Friend WithEvents ChbObfuscatorRenameMainNamespaceOnlyNamespaces As LogInCheckBox
    Friend WithEvents ChbObfuscatorBooleanEncrypt As LogInCheckBox
    Friend WithEvents ChbObfuscatorHideCalls As LogInCheckBox
    Friend WithEvents PnlPackerEnabled As System.Windows.Forms.Panel
    Friend WithEvents PnlIconChangerEnabled As System.Windows.Forms.Panel
    Friend WithEvents LblAboutDevelopBy As LogInLabel
    Friend WithEvents LblAboutVersion As LogInLabel
    Friend WithEvents LblVersionInfosVersion As LogInLabel
    Friend WithEvents LblVersionInfosTitle As LogInLabel
    Friend WithEvents LblVersionInfosDescription As LogInLabel
    Friend WithEvents LblVersionInfosCompany As LogInLabel
    Friend WithEvents LblVersionInfosProduct As LogInLabel
    Friend WithEvents LblVersionInfosCopyright As LogInLabel
    Friend WithEvents LblVersionInfosTrademark As LogInLabel
    Friend WithEvents ChbObfuscatorAntiDumper As LogInCheckBox
    Friend WithEvents ChbObfuscatorControlFlow As LogInCheckBox
    Friend WithEvents ChbObfuscatorResourcesEncryption As XertzLoginTheme.LogInCheckBox
    Friend WithEvents LblPackerPlatform As XertzLoginTheme.LogInLabel
    Friend WithEvents LblPackerSystem As XertzLoginTheme.LogInLabel
    Friend WithEvents LblPackerFramework As XertzLoginTheme.LogInLabel
    Friend WithEvents TxbPackerPlatform As System.Windows.Forms.TextBox
    Friend WithEvents TxbPackerSystem As System.Windows.Forms.TextBox
    Friend WithEvents ChbManifestEnabled As XertzLoginTheme.LogInCheckBox
    Friend WithEvents PnlManifestEnabled As System.Windows.Forms.Panel
    Friend WithEvents RdbManifestChangerHighestAvailable As XertzLoginTheme.LogInRadioButton
    Friend WithEvents RdbManifestChangerRequireAdministrator As XertzLoginTheme.LogInRadioButton
    Friend WithEvents RdbManifestChangerAsInvoker As XertzLoginTheme.LogInRadioButton
    Friend WithEvents TxbIconChangerSelect As System.Windows.Forms.TextBox
    Friend WithEvents BtnIconChangerSelect As XertzLoginTheme.LogInButton
    Friend WithEvents PbxIconChangerSelect As System.Windows.Forms.PictureBox
    Friend WithEvents PnlDependenciesEnabled As System.Windows.Forms.Panel
    Friend WithEvents BtnDependenciesDelete As XertzLoginTheme.LogInButton
    Friend WithEvents BtnDependenciesAdd As XertzLoginTheme.LogInButton
    Friend WithEvents LbxDependenciesAdd As XertzLoginTheme.LogInListbox
    Friend WithEvents ChbDependenciesEnabled As XertzLoginTheme.LogInCheckBox
    Friend WithEvents LblDNR_DevelopBy As XertzLoginTheme.LogInLabel
    Friend WithEvents LblDNR_Version As XertzLoginTheme.LogInLabel
    Friend WithEvents PgbStart As XertzLoginTheme.TextProgressBar
    Friend WithEvents GbxPackerLoader As XertzLoginTheme.LogInGroupBox
    Friend WithEvents PcbDetection As System.Windows.Forms.PictureBox
    Friend WithEvents LnkLblBlogSpot As System.Windows.Forms.LinkLabel
    Friend WithEvents ChbObfuscatorResourcesContent As XertzLoginTheme.LogInCheckBox
    Friend WithEvents CbxDependenciesEmbedded As LoginTheme.XertzLoginTheme.LogInComboBox
    Friend WithEvents RdbDependenciesEmbedded As LoginTheme.XertzLoginTheme.LogInRadioButton
    Friend WithEvents RdbDependenciesMerged As LoginTheme.XertzLoginTheme.LogInRadioButton
    Friend WithEvents LblPackerWarning As LoginTheme.XertzLoginTheme.LogInLabel
    Friend WithEvents LblDependenciesWarning As LoginTheme.XertzLoginTheme.LogInLabel
    Friend WithEvents TxbPackerFramework As System.Windows.Forms.TextBox
    Friend WithEvents LblAboutCredits1 As LogInLabel
    Friend WithEvents LblAboutCredits As LogInLabel
    Friend WithEvents LblIconChangerSelect As LogInLabel
End Class
