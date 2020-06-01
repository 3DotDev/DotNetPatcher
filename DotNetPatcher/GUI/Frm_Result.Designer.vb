Imports LoginTheme
Imports LoginTheme.XertzLoginTheme

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Result
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Result))
        Me.BgwRenameTask = New System.ComponentModel.BackgroundWorker()
        Me.Frm_ResultThemeContainer = New LoginTheme.XertzLoginTheme.LogInThemeContainer()
        Me.BtnResultOpenAssemblyViewer = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.BtnResultOpenFileDir = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.BtnResultClose = New LoginTheme.XertzLoginTheme.LogInButton()
        Me.LblResultMessage = New LoginTheme.XertzLoginTheme.LogInLabel()
        Me.PcbResultIcon = New System.Windows.Forms.PictureBox()
        Me.Frm_ResultThemeContainer.SuspendLayout()
        CType(Me.PcbResultIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BgwRenameTask
        '
        Me.BgwRenameTask.WorkerReportsProgress = True
        Me.BgwRenameTask.WorkerSupportsCancellation = True
        '
        'Frm_ResultThemeContainer
        '
        Me.Frm_ResultThemeContainer.AllowClose = True
        Me.Frm_ResultThemeContainer.AllowMaximize = False
        Me.Frm_ResultThemeContainer.AllowMinimize = False
        Me.Frm_ResultThemeContainer.BackColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Frm_ResultThemeContainer.BaseColour = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Frm_ResultThemeContainer.BorderColour = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.Frm_ResultThemeContainer.ContainerColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.Frm_ResultThemeContainer.Controls.Add(Me.BtnResultOpenAssemblyViewer)
        Me.Frm_ResultThemeContainer.Controls.Add(Me.BtnResultOpenFileDir)
        Me.Frm_ResultThemeContainer.Controls.Add(Me.BtnResultClose)
        Me.Frm_ResultThemeContainer.Controls.Add(Me.PcbResultIcon)
        Me.Frm_ResultThemeContainer.Controls.Add(Me.LblResultMessage)
        Me.Frm_ResultThemeContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Frm_ResultThemeContainer.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Frm_ResultThemeContainer.FontSize = 12
        Me.Frm_ResultThemeContainer.HoverColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Frm_ResultThemeContainer.Location = New System.Drawing.Point(0, 0)
        Me.Frm_ResultThemeContainer.MouseOverColour = System.Drawing.Color.BlueViolet
        Me.Frm_ResultThemeContainer.Name = "Frm_ResultThemeContainer"
        Me.Frm_ResultThemeContainer.ShowControlBox = True
        Me.Frm_ResultThemeContainer.ShowIcon = False
        Me.Frm_ResultThemeContainer.ShowMaximizeButton = False
        Me.Frm_ResultThemeContainer.ShowMinimizeButton = False
        Me.Frm_ResultThemeContainer.Size = New System.Drawing.Size(548, 278)
        Me.Frm_ResultThemeContainer.TabIndex = 0
        '
        'BtnResultOpenAssemblyViewer
        '
        Me.BtnResultOpenAssemblyViewer.BackColor = System.Drawing.Color.Transparent
        Me.BtnResultOpenAssemblyViewer.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnResultOpenAssemblyViewer.BorderColour = System.Drawing.Color.DimGray
        Me.BtnResultOpenAssemblyViewer.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnResultOpenAssemblyViewer.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnResultOpenAssemblyViewer.Location = New System.Drawing.Point(235, 235)
        Me.BtnResultOpenAssemblyViewer.Name = "BtnResultOpenAssemblyViewer"
        Me.BtnResultOpenAssemblyViewer.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnResultOpenAssemblyViewer.ProgressColour = System.Drawing.Color.BlueViolet
        Me.BtnResultOpenAssemblyViewer.Size = New System.Drawing.Size(205, 25)
        Me.BtnResultOpenAssemblyViewer.TabIndex = 13
        Me.BtnResultOpenAssemblyViewer.Text = "Open protected assembly viewer"
        '
        'BtnResultOpenFileDir
        '
        Me.BtnResultOpenFileDir.BackColor = System.Drawing.Color.Transparent
        Me.BtnResultOpenFileDir.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnResultOpenFileDir.BorderColour = System.Drawing.Color.DimGray
        Me.BtnResultOpenFileDir.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnResultOpenFileDir.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnResultOpenFileDir.Location = New System.Drawing.Point(24, 235)
        Me.BtnResultOpenFileDir.Name = "BtnResultOpenFileDir"
        Me.BtnResultOpenFileDir.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnResultOpenFileDir.ProgressColour = System.Drawing.Color.BlueViolet
        Me.BtnResultOpenFileDir.Size = New System.Drawing.Size(205, 25)
        Me.BtnResultOpenFileDir.TabIndex = 12
        Me.BtnResultOpenFileDir.Text = "Open the file directory and close"
        '
        'BtnResultClose
        '
        Me.BtnResultClose.BackColor = System.Drawing.Color.Transparent
        Me.BtnResultClose.BaseColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.BtnResultClose.BorderColour = System.Drawing.Color.DimGray
        Me.BtnResultClose.HoverColour = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.BtnResultClose.LineColour = System.Drawing.Color.BlueViolet
        Me.BtnResultClose.Location = New System.Drawing.Point(446, 235)
        Me.BtnResultClose.Name = "BtnResultClose"
        Me.BtnResultClose.PressedColour = System.Drawing.Color.FromArgb(CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(47, Byte), Integer))
        Me.BtnResultClose.ProgressColour = System.Drawing.Color.BlueViolet
        Me.BtnResultClose.Size = New System.Drawing.Size(78, 25)
        Me.BtnResultClose.TabIndex = 11
        Me.BtnResultClose.Text = "Close"
        '
        'LblResultMessage
        '
        Me.LblResultMessage.BackColor = System.Drawing.Color.Transparent
        Me.LblResultMessage.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblResultMessage.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblResultMessage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.LblResultMessage.Location = New System.Drawing.Point(94, 31)
        Me.LblResultMessage.Name = "LblResultMessage"
        Me.LblResultMessage.Size = New System.Drawing.Size(430, 201)
        Me.LblResultMessage.TabIndex = 10
        Me.LblResultMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PcbResultIcon
        '
        Me.PcbResultIcon.BackColor = System.Drawing.Color.Transparent
        Me.PcbResultIcon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PcbResultIcon.Location = New System.Drawing.Point(24, 88)
        Me.PcbResultIcon.Name = "PcbResultIcon"
        Me.PcbResultIcon.Size = New System.Drawing.Size(64, 64)
        Me.PcbResultIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PcbResultIcon.TabIndex = 9
        Me.PcbResultIcon.TabStop = False
        Me.PcbResultIcon.Visible = False
        '
        'Frm_Result
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(548, 278)
        Me.ControlBox = False
        Me.Controls.Add(Me.Frm_ResultThemeContainer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Frm_Result"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.Frm_ResultThemeContainer.ResumeLayout(False)
        CType(Me.PcbResultIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Frm_ResultThemeContainer As LogInThemeContainer
    Friend WithEvents BgwRenameTask As System.ComponentModel.BackgroundWorker
    Friend WithEvents PcbResultIcon As System.Windows.Forms.PictureBox
    Friend WithEvents BtnResultClose As XertzLoginTheme.LogInButton
    Friend WithEvents BtnResultOpenFileDir As XertzLoginTheme.LogInButton
    Friend WithEvents BtnResultOpenAssemblyViewer As LoginTheme.XertzLoginTheme.LogInButton
    Friend WithEvents LblResultMessage As LoginTheme.XertzLoginTheme.LogInLabel
End Class
