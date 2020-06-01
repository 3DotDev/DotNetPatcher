Imports LoginTheme.XertzLoginTheme

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Viewer
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Viewer))
        Me.BgwRenameTask = New System.ComponentModel.BackgroundWorker()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.BgwExclusion = New System.ComponentModel.BackgroundWorker()
        Me.Frm_ExclusionThemeContainer = New LoginTheme.XertzLoginTheme.LogInThemeContainer()
        Me.TvExclusion = New LoginTheme.XertzLoginTheme.TreeViewEx()
        Me.Frm_ExclusionThemeContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'BgwRenameTask
        '
        Me.BgwRenameTask.WorkerReportsProgress = True
        Me.BgwRenameTask.WorkerSupportsCancellation = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Assembly.png")
        Me.ImageList1.Images.SetKeyName(1, "Library.png")
        Me.ImageList1.Images.SetKeyName(2, "NameSpace.png")
        Me.ImageList1.Images.SetKeyName(3, "Class.png")
        Me.ImageList1.Images.SetKeyName(4, "Constructor.png")
        Me.ImageList1.Images.SetKeyName(5, "Delegate.png")
        Me.ImageList1.Images.SetKeyName(6, "Enum.png")
        Me.ImageList1.Images.SetKeyName(7, "EnumValue.png")
        Me.ImageList1.Images.SetKeyName(8, "Event.png")
        Me.ImageList1.Images.SetKeyName(9, "Field.png")
        Me.ImageList1.Images.SetKeyName(10, "Interface.png")
        Me.ImageList1.Images.SetKeyName(11, "Method.png")
        Me.ImageList1.Images.SetKeyName(12, "PInvokeMethod.png")
        Me.ImageList1.Images.SetKeyName(13, "Property.png")
        Me.ImageList1.Images.SetKeyName(14, "StaticClass.png")
        '
        'BgwExclusion
        '
        Me.BgwExclusion.WorkerReportsProgress = True
        Me.BgwExclusion.WorkerSupportsCancellation = True
        '
        'Frm_ExclusionThemeContainer
        '
        Me.Frm_ExclusionThemeContainer.AllowClose = True
        Me.Frm_ExclusionThemeContainer.AllowMaximize = False
        Me.Frm_ExclusionThemeContainer.AllowMinimize = False
        Me.Frm_ExclusionThemeContainer.BackColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Frm_ExclusionThemeContainer.BaseColour = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Frm_ExclusionThemeContainer.BorderColour = System.Drawing.Color.DimGray
        Me.Frm_ExclusionThemeContainer.ContainerColour = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.Frm_ExclusionThemeContainer.Controls.Add(Me.TvExclusion)
        Me.Frm_ExclusionThemeContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Frm_ExclusionThemeContainer.FontColour = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Frm_ExclusionThemeContainer.FontSize = 12
        Me.Frm_ExclusionThemeContainer.HoverColour = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.Frm_ExclusionThemeContainer.Location = New System.Drawing.Point(0, 0)
        Me.Frm_ExclusionThemeContainer.MouseOverColour = System.Drawing.Color.BlueViolet
        Me.Frm_ExclusionThemeContainer.Name = "Frm_ExclusionThemeContainer"
        Me.Frm_ExclusionThemeContainer.ShowControlBox = True
        Me.Frm_ExclusionThemeContainer.ShowIcon = True
        Me.Frm_ExclusionThemeContainer.ShowMaximizeButton = False
        Me.Frm_ExclusionThemeContainer.ShowMinimizeButton = False
        Me.Frm_ExclusionThemeContainer.Size = New System.Drawing.Size(704, 697)
        Me.Frm_ExclusionThemeContainer.TabIndex = 0
        Me.Frm_ExclusionThemeContainer.Text = "Assembly Viewer"
        '
        'TvExclusion
        '
        Me.TvExclusion.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TvExclusion.ImageIndex = 0
        Me.TvExclusion.ImageList = Me.ImageList1
        Me.TvExclusion.Location = New System.Drawing.Point(3, 38)
        Me.TvExclusion.Name = "TvExclusion"
        Me.TvExclusion.SelectedImageIndex = 0
        Me.TvExclusion.Size = New System.Drawing.Size(698, 656)
        Me.TvExclusion.TabIndex = 4
        '
        'Frm_Exclusion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 697)
        Me.ControlBox = False
        Me.Controls.Add(Me.Frm_ExclusionThemeContainer)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Frm_Exclusion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TransparencyKey = System.Drawing.Color.Fuchsia
        Me.Frm_ExclusionThemeContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Frm_ExclusionThemeContainer As LogInThemeContainer
    Friend WithEvents BgwRenameTask As System.ComponentModel.BackgroundWorker
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents BgwExclusion As System.ComponentModel.BackgroundWorker
    Friend WithEvents TvExclusion As TreeViewEx
End Class
