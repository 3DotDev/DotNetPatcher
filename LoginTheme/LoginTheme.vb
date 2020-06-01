Option Strict On

Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports System.Drawing

''     DO NOT REMOVE CREDITS! IF YOU USE PLEASE CREDIT!     ''

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''''''           Added by Sh@rp aka 3DotDev  :      ''''''
'''''' - RadioButton CheckedState Event manager     ''''''
'''''' - Enabled/Disabled control States            ''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Namespace XertzLoginTheme

    ''' <summary>
    ''' LogIn GDI+ Theme
    ''' Creator: Xertz (HF)
    ''' Version: 1.3
    ''' Control Count: 28
    ''' Date Created: 18/12/2013
    ''' Date Changed: 23/04/2014
    ''' UID: 1602992
    ''' For any bugs / errors, PM me.
    ''' </summary>
    ''' <remarks></remarks>

    Module DrawHelpers

#Region "Functions"

        Public Function RoundRectangle(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
            Dim P As GraphicsPath = New GraphicsPath()
            Dim ArcRectangleWidth As Integer = Curve * 2
            P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
            P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
            P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
            P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
            P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
            Return P
        End Function

        Public Function RoundRect(x!, y!, w!, h!, Optional r! = 0.3, Optional TL As Boolean = True, Optional TR As Boolean = True, Optional BR As Boolean = True, Optional BL As Boolean = True) As GraphicsPath
            Dim d! = Math.Min(w, h) * r, xw = x + w, yh = y + h
            RoundRect = New GraphicsPath

            With RoundRect
                If TL Then .AddArc(x, y, d, d, 180, 90) Else .AddLine(x, y, x, y)
                If TR Then .AddArc(xw - d, y, d, d, 270, 90) Else .AddLine(xw, y, xw, y)
                If BR Then .AddArc(xw - d, yh - d, d, d, 0, 90) Else .AddLine(xw, yh, xw, yh)
                If BL Then .AddArc(x, yh - d, d, d, 90, 90) Else .AddLine(x, yh, x, yh)

                .CloseFigure()
            End With
        End Function

        Enum MouseState As Byte
            None = 0
            Over = 1
            Down = 2
            Block = 3
        End Enum

#End Region

    End Module

    Public Class LogInThemeContainer
        Inherits ContainerControl

#Region "Declarations" 
        Private ReadOnly _Font As Font

        Private State As MouseState = MouseState.None
        Private MouseXLoc As Integer
        Private MouseYLoc As Integer
        Private CaptureMovement As Boolean = False
        Private Const MoveHeight As Integer = 35
        Private MouseP As Point = New Point(0, 0)   
#End Region

#Region "Properties & Events"

        <Category("Control")>
        Public Property FontSize As Integer
        <Category("Control")>
        Public Property AllowMinimize As Boolean
        <Category("Control")>
        Public Property AllowMaximize As Boolean
        <Category("Control")>
        Public Property ShowIcon As Boolean
        <Category("Control")>
        Public Property AllowClose As Boolean
        <Category("Control")>
        Public Property ShowControlBox As Boolean
        <Category("Control")>
        Public Property ShowMinimizeButton As Boolean
        <Category("Control")>
        Public Property ShowMaximizeButton As Boolean
        <Category("Colours")>
        Public Property BorderColour As Color
        <Category("Colours")>
        Public Property HoverColour As Color
        <Category("Colours")>
        Public Property BaseColour As Color
        <Category("Colours")>
        Public Property ContainerColour As Color
        <Category("Colours")>
        Public Property FontColour As Color
        <Category("Colours")>
        Public Property MouseOverColour As Color

        Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
            MyBase.OnMouseUp(e)
            CaptureMovement = False
            State = MouseState.Over
            Invalidate()
        End Sub

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            State = MouseState.Over : Invalidate()
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            State = MouseState.None : Invalidate()
        End Sub

        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            MyBase.OnMouseMove(e)
            MouseXLoc = e.Location.X
            MouseYLoc = e.Location.Y
            Invalidate()
            If CaptureMovement Then
                Parent.Location = MousePosition - CType(MouseP, Size)
            End If
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)

            If MouseXLoc > Width - 39 AndAlso MouseXLoc < Width - 16 AndAlso MouseYLoc < 22 Then
                If _ShowControlBox AndAlso _AllowClose Then
                    'Environment.Exit(0)
                    FindForm.Close()
                End If
            ElseIf MouseXLoc > Width - 64 AndAlso MouseXLoc < Width - 41 AndAlso MouseYLoc < 22 Then
                If _ShowControlBox AndAlso _AllowMaximize Then
                    Select Case FindForm.WindowState
                        Case FormWindowState.Maximized
                            FindForm.WindowState = FormWindowState.Normal
                        Case FormWindowState.Normal
                            FindForm.WindowState = FormWindowState.Maximized
                    End Select
                End If
            ElseIf MouseXLoc > Width - 89 AndAlso MouseXLoc < Width - 66 AndAlso MouseYLoc < 22 Then
                If _ShowControlBox AndAlso _AllowMinimize Then
                    Select Case FindForm.WindowState
                        Case FormWindowState.Normal
                            FindForm.WindowState = FormWindowState.Minimized
                        Case FormWindowState.Maximized
                            FindForm.WindowState = FormWindowState.Minimized
                    End Select
                End If
            ElseIf e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width - 90, MoveHeight).Contains(e.Location) Then
                CaptureMovement = True
                MouseP = e.Location
            ElseIf e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(Width - 90, 22, 75, 13).Contains(e.Location) Then
                CaptureMovement = True
                MouseP = e.Location
            ElseIf e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(Width - 15, 0, 15, MoveHeight).Contains(e.Location) Then
                CaptureMovement = True
                MouseP = e.Location
            Else
                Focus()
            End If
            State = MouseState.Down
            Invalidate()
        End Sub

#End Region

#Region "Draw Control"

        Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                    ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
            DoubleBuffered = True
            BackColor = _BaseColour
            Dock = DockStyle.Fill
            _MouseOverColour = Color.BlueViolet
            _FontSize = 12
            _Font = New Font("Segoe UI", _FontSize)
            _AllowMinimize = True
            _AllowMaximize = True
            _AllowClose = True
            _ShowControlBox = True
            _ShowMinimizeButton = True
            _ShowMaximizeButton = True
            _ShowIcon = True
            _FontColour = Color.FromArgb(255, 255, 255)
            _BaseColour = Color.FromArgb(35, 35, 35)
            _ContainerColour = Color.FromArgb(54, 54, 54)
            _BorderColour = Color.FromArgb(60, 60, 60)
            _HoverColour = Color.FromArgb(42, 42, 42)
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            ParentForm.FormBorderStyle = FormBorderStyle.None
            ParentForm.AllowTransparency = False
            ParentForm.TransparencyKey = Color.Fuchsia
            ParentForm.FindForm.StartPosition = FormStartPosition.CenterScreen
            Dock = DockStyle.Fill
            Invalidate()
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)

            Dim G = e.Graphics
            With G
                .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                .SmoothingMode = SmoothingMode.HighQuality
                .PixelOffsetMode = PixelOffsetMode.HighQuality
                .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width, Height))
                .FillRectangle(New SolidBrush(_ContainerColour), New Rectangle(2, 35, Width - 4, Height - 37))

                .DrawRectangle(New Pen(_BorderColour), New Rectangle(0, 0, Width, Height))

                If _ShowControlBox Then
                    Dim ControlBoxPoints() As Point = Nothing

                    ''Close Button
                    .DrawLine(New Pen(_FontColour), Width - 33, 6, Width - 22, 16)
                    .DrawLine(New Pen(_FontColour), Width - 33, 16, Width - 22, 6)

                    ''Minimize Button
                    If _ShowMinimizeButton Then
                        .DrawLine(New Pen(_FontColour), Width - 83, 16, Width - 72, 16)
                    End If
                    ''Maximize Button
                    If _ShowMaximizeButton Then
                        .DrawLine(New Pen(_FontColour), Width - 58, 16, Width - 47, 16)
                        .DrawLine(New Pen(_FontColour), Width - 58, 16, Width - 58, 6)
                        .DrawLine(New Pen(_FontColour), Width - 47, 16, Width - 47, 6)
                        .DrawLine(New Pen(_FontColour), Width - 58, 6, Width - 47, 6)
                        .DrawLine(New Pen(_FontColour), Width - 58, 7, Width - 47, 7)
                    End If

                    If _ShowMinimizeButton AndAlso _ShowMaximizeButton Then
                        ControlBoxPoints = {New Point(Width - 90, 0), New Point(Width - 90, 22), New Point(Width - 15, 22), New Point(Width - 15, 0)}
                        .DrawLines(New Pen(_BorderColour), ControlBoxPoints)
                        .DrawLine(New Pen(_BorderColour), Width - 65, 0, Width - 65, 22)
                    Else
                        If _ShowMinimizeButton = False AndAlso _ShowMaximizeButton = False Then
                            ControlBoxPoints = {New Point(Width - 40, 22), New Point(Width - 15, 22), New Point(Width - 15, 0)}
                            .DrawLines(New Pen(_BorderColour), ControlBoxPoints)
                        End If
                    End If

                    Select Case State
                        Case MouseState.Over
                            If MouseXLoc > Width - 39 AndAlso MouseXLoc < Width - 16 AndAlso MouseYLoc < 22 Then
                                .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(Width - 39, 0, 23, 22))
                                .DrawLine(New Pen(_MouseOverColour), Width - 33, 6, Width - 22, 16)
                                .DrawLine(New Pen(_MouseOverColour), Width - 33, 16, Width - 22, 6)
                            ElseIf MouseXLoc > Width - 64 AndAlso MouseXLoc < Width - 41 AndAlso MouseYLoc < 22 Then
                                If _ShowMaximizeButton AndAlso AllowMaximize Then
                                    .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(Width - 64, 0, 23, 22))
                                    .DrawLine(New Pen(_MouseOverColour), Width - 58, 16, Width - 47, 16)
                                    .DrawLine(New Pen(_MouseOverColour), Width - 58, 16, Width - 58, 6)
                                    .DrawLine(New Pen(_MouseOverColour), Width - 47, 16, Width - 47, 6)
                                    .DrawLine(New Pen(_MouseOverColour), Width - 58, 6, Width - 47, 6)
                                    .DrawLine(New Pen(_MouseOverColour), Width - 58, 7, Width - 47, 7)
                                End If
                            ElseIf MouseXLoc > Width - 89 AndAlso MouseXLoc < Width - 66 AndAlso MouseYLoc < 22 Then
                                If _ShowMinimizeButton AndAlso AllowMinimize Then
                                    .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(Width - 89, 0, 23, 22))
                                    .DrawLine(New Pen(_MouseOverColour), Width - 83, 16, Width - 72, 16)
                                End If
                            End If
                        Case Else
                            If MouseXLoc > Width - 39 AndAlso MouseXLoc < Width - 16 AndAlso MouseYLoc < 22 Then
                                ''Close Button
                                .DrawLine(New Pen(_FontColour), Width - 33, 6, Width - 22, 16)
                                .DrawLine(New Pen(_FontColour), Width - 33, 16, Width - 22, 6)
                            ElseIf MouseXLoc > Width - 64 AndAlso MouseXLoc < Width - 41 AndAlso MouseYLoc < 22 Then
                                If _ShowMaximizeButton Then
                                    .DrawLine(New Pen(_FontColour), Width - 58, 16, Width - 47, 16)
                                    .DrawLine(New Pen(_FontColour), Width - 58, 16, Width - 58, 6)
                                    .DrawLine(New Pen(_FontColour), Width - 47, 16, Width - 47, 6)
                                    .DrawLine(New Pen(_FontColour), Width - 58, 6, Width - 47, 6)
                                    .DrawLine(New Pen(_FontColour), Width - 58, 7, Width - 47, 7)
                                End If
                            ElseIf MouseXLoc > Width - 89 AndAlso MouseXLoc < Width - 66 AndAlso MouseYLoc < 22 Then
                                If _ShowMinimizeButton Then
                                    .DrawLine(New Pen(_FontColour), Width - 83, 16, Width - 72, 16)
                                End If
                            End If           
                    End Select
                    .DrawLine(New Pen(_BorderColour), Width - 40, 0, Width - 40, 22)

                    ''Close Button
                    '.DrawLine(New Pen(_FontColour), Width - 33, 6, Width - 22, 16)
                    '.DrawLine(New Pen(_FontColour), Width - 33, 16, Width - 22, 6)
                  
                End If

                If _ShowIcon Then
                    .DrawIcon(FindForm.Icon, New Rectangle(6, 6, 22, 22))
                End If
                .DrawString(Text, _Font, New SolidBrush(_FontColour), New RectangleF(3, 0, Width, 35), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

                Dim P() As Point = {New Point(0, 0), New Point(CInt(Me.Width), 0), New Point(CInt(Me.Width), Me.Height), _
                               New Point(Width, Me.Height), New Point(Width, Height), New Point(0, Height), New Point(0, 0)}
                .DrawLines(New Pen(Color.Gray), P)

                .InterpolationMode = CType(7, InterpolationMode)
            End With
        End Sub

#End Region

    End Class

    <DefaultEvent("CheckedChanged"), InitializationEvent("CheckedState")> _
    Public Class LogInCheckBox
        Inherits Control

#Region "Declarations"
        Private _Checked As Boolean
        Private State As MouseState = MouseState.None
#End Region

#Region "Colour & Other Properties"


        <Category("Colours")>
        Public Property BaseColour As Color = Color.FromArgb(47, 47, 47)

        <Category("Colours")>
        Public Property BorderColour As Color = Color.DimGray

        <Category("Colours")>
        Public Property CheckedColour As Color = Color.FromArgb(173, 173, 174)

        <Category("Colours")>
        Public Property FontColour As Color = Color.FromArgb(255, 255, 255)

        Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
            MyBase.OnTextChanged(e)
            Invalidate()
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            _Checked = Not _Checked
            RaiseEvent CheckedChanged(Me, EventArgs.Empty)
            MyBase.OnClick(e)
        End Sub
        Public Property Checked() As Boolean
            Get
                Return _Checked
            End Get
            Set(value As Boolean)
                _Checked = value
                RaiseEvent CheckedChanged(Me, EventArgs.Empty)
                Invalidate()
            End Set
        End Property

        Public Event CheckedChanged As EventHandler

        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            Height = 22
        End Sub
#End Region

#Region "Mouse States"

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            State = MouseState.Down : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            State = MouseState.Over : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            State = MouseState.Over : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            State = MouseState.None : Invalidate()
        End Sub

#End Region

#Region "Draw Control"
        Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or
                       ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
            DoubleBuffered = True
            Cursor = Cursors.Hand
            Size = New Size(100, 22)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g = e.Graphics
            Dim Base As New Rectangle(0, 0, 19, 19)
            With g
                .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                .SmoothingMode = SmoothingMode.HighQuality
                .Clear(Color.FromArgb(54, 54, 54))
                .FillRectangle(New SolidBrush(BaseColour), Base)
                .DrawRectangle(New Pen(BorderColour), New Rectangle(0, 0, 19, 19))
                Select Case State
                    Case MouseState.Over
                        .FillRectangle(New SolidBrush(Color.FromArgb(50, 49, 51)), Base)
                        .DrawRectangle(New Pen(BorderColour), New Rectangle(0, 0, 19, 19))
                End Select
                If Checked Then
                    Dim P() As Point = {New Point(2, 11), New Point(6, 8), New Point(9, 12), New Point(15, 3), New Point(17, 6), New Point(9, 16)}
                    CheckedColour = If(Me.Enabled = False, Color.Gray, Color.BlueViolet)
                    .FillPolygon(New SolidBrush(CheckedColour), P)
                End If
                FontColour = If(Me.Parent.Enabled = False OrElse Enabled = False, Color.Gray, Color.FromArgb(255, 255, 255))
                .DrawString(Text, Font, New SolidBrush(FontColour), New Rectangle(24, 1, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                .InterpolationMode = CType(7, InterpolationMode)
            End With
        End Sub
#End Region

    End Class

#Region "Sh@rp : RadioButton EventArgs Class Added "
    Public Class CheckedArgs
        Inherits EventArgs
        Private _Checked As Boolean
        Public Sub New(theEventChecked As Boolean)
            _Checked = theEventChecked
        End Sub
        Public Property Checked As Boolean
            Get
                Return Me._Checked
            End Get
            Set(ByVal Value As Boolean)
                _Checked = Value
            End Set
        End Property
    End Class
#End Region

    <DefaultEvent("CheckedChanged"), InitializationEvent("CheckedState")> _
    Public Class LogInRadioButton
        Inherits Control

#Region "Declarations"
        Private _Checked As Boolean
        Private State As MouseState = MouseState.None
        Private _HoverColour As Color = Color.FromArgb(50, 49, 51)
        Private _CheckedColour As Color = Color.FromArgb(173, 173, 174)
        Private _BorderColour As Color = Color.DimGray
        Private _BackColour As Color = Color.FromArgb(54, 54, 54)
        Private _TextColour As Color = Color.FromArgb(255, 255, 255)
#End Region

#Region "Colour & Other Properties"

        <Category("Colours")>
        Public Property HighlightColour As Color
            Get
                Return _HoverColour
            End Get
            Set(value As Color)
                _HoverColour = value
            End Set
        End Property

        <Category("Colours")>
        Public Property BaseColour As Color
            Get
                Return _BackColour
            End Get
            Set(value As Color)
                _BackColour = value
            End Set
        End Property

        <Category("Colours")>
        Public Property BorderColour As Color
            Get
                Return _BorderColour
            End Get
            Set(value As Color)
                _BorderColour = value
            End Set
        End Property

        <Category("Colours")>
        Public Property CheckedColour As Color
            Get
                Return _CheckedColour
            End Get
            Set(value As Color)
                _CheckedColour = value
            End Set
        End Property

        <Category("Colours")>
        Public Property FontColour As Color
            Get
                Return _TextColour
            End Get
            Set(value As Color)
                _TextColour = value
            End Set
        End Property

#Region "Sh@rp : RadioButton CheckedState Added "
        Public Delegate Sub CheckedChangedHandler(ByVal sender As Object, ByVal e As System.EventArgs)

        <Category("Configuration"), Browsable(True), Description("CheckedChanged")> _
        Public Event CheckedChanged As CheckedChangedHandler

        <Category("Configuration"), Browsable(True), Description("CheckedState")> _
        Public Event CheckedState(ByVal sender As Object)

        Protected Overridable Sub OnCheckedChanged(ByVal e As System.EventArgs)
            RaiseEvent CheckedChanged(Me, e)
        End Sub

        Private _CheckState As CheckState
        Public Property CheckState As CheckState
            Get
                Return _CheckState
            End Get
            Set(ByVal V As CheckState)
                _CheckState = V
                RaiseEvent CheckedState(Me)
                Invalidate()
            End Set
        End Property
#End Region

        Property Checked() As Boolean
            Get
                Return _Checked
            End Get
            Set(value As Boolean)
                _Checked = value
                InvalidateControls()
                RaiseEvent CheckedChanged(Me, New EventArgs)
                Invalidate()
            End Set
        End Property

        Protected Overrides Sub OnClick(e As EventArgs)
            If Not _Checked Then Checked = True
            MyBase.OnClick(e)
        End Sub

        Private Sub InvalidateControls()
            If Not IsHandleCreated OrElse Not _Checked Then Return
            For Each C As Control In Parent.Controls
                If C IsNot Me AndAlso TypeOf C Is LogInRadioButton Then
                    DirectCast(C, LogInRadioButton).Checked = False
                    Invalidate()
                End If
            Next
        End Sub
        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()
            InvalidateControls()
        End Sub
        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            Height = 18
        End Sub
#End Region

#Region "Mouse States"

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            State = MouseState.Down : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            State = MouseState.Over : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            State = MouseState.Over : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            State = MouseState.None : Invalidate()
        End Sub

#End Region

#Region "Draw Control"
        Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                       ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
            DoubleBuffered = True
            Cursor = Cursors.Hand
            Size = New Size(100, 18)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim G = e.Graphics
            Dim Base As New Rectangle(1, 1, Height - 2, Height - 2)
            Dim Circle As New Rectangle(6, 6, Height - 12, Height - 12)
            With G
                .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                .SmoothingMode = SmoothingMode.HighQuality
                .PixelOffsetMode = PixelOffsetMode.HighQuality
                .Clear(_BackColour)
                .FillEllipse(New SolidBrush(Color.FromArgb(47, 47, 47)), Base)
                .DrawEllipse(New Pen(_BorderColour, 1), Base)
                If Checked Then
                    Select Case State
                        Case MouseState.Over
                            .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                    End Select
                    _CheckedColour = If(Me.Enabled = False, Color.Gray, Color.BlueViolet)
                    .FillEllipse(New SolidBrush(_CheckedColour), Circle)
                Else
                    Select Case State
                        Case MouseState.Over
                            .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                    End Select
                End If
                _TextColour = If(Me.Parent.Enabled = False, Color.Gray, Color.FromArgb(255, 255, 255))
                .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
                .InterpolationMode = CType(7, InterpolationMode)
            End With
        End Sub
#End Region

    End Class

    Public Class LogInLabel
        Inherits Label

#Region "Declaration"
        Private _FontColour As Color = Color.FromArgb(255, 255, 255)
#End Region

#Region "Property & Event"

        <Category("Colours")>
        Public Property FontColour As Color
            Get
                Return _FontColour
            End Get
            Set(value As Color)
                _FontColour = value
            End Set
        End Property

        Protected Overrides Sub OnTextChanged(e As EventArgs)
            MyBase.OnTextChanged(e) : Invalidate()
        End Sub

#End Region

#Region "Draw Control"

        Sub New()
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
            Font = New Font("Segoe UI", 9)
            ForeColor = _FontColour
            BackColor = Color.Transparent
            Text = Text
        End Sub

#End Region

    End Class

    Public Class LogInButton
        Inherits Control

#Region "Declarations"
        Private ReadOnly _Font As New Font("Segoe UI", 9)
        Private ReadOnly m_MainColour As Color = Color.FromArgb(42, 42, 42)
        Private State As New MouseState
#End Region

#Region "Mouse States"

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            MyBase.OnMouseDown(e)
            State = MouseState.Down : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            MyBase.OnMouseUp(e)
            State = MouseState.Over : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            State = MouseState.Over : Invalidate()
        End Sub
        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            State = MouseState.None : Invalidate()
        End Sub

#End Region

#Region "Properties"

        <Category("Colours")>
        Public Property LineColour As Color
        <Category("Colours")>
        Public Property ProgressColour As Color
        <Category("Colours")>
        Public Property BorderColour As Color
        <Category("Colours")>
        Public Property FontColour As Brush
        <Category("Colours")>
        Public Property BaseColour As Color
        <Category("Colours")>
        Public Property HoverColour As Color
        <Category("Colours")>
        Public Property PressedColour As Color

#End Region

#Region "Draw Control"
        Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or
                  ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or
                  ControlStyles.SupportsTransparentBackColor, True)
            DoubleBuffered = True
            Size = New Size(75, 30)
            BackColor = Color.Transparent

            _ProgressColour = Color.DarkViolet
            _BorderColour = Color.FromArgb(25, 25, 25)
            _FontColour = Brushes.White
            _HoverColour = Color.FromArgb(52, 52, 52)
            _PressedColour = Color.FromArgb(47, 47, 47)
            _LineColour = Color.BlueViolet
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim G = e.Graphics
            With G
                .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                .Clear(BackColor)
                Dim bru As Brush = Brushes.White
                bru = If(Me.Enabled = False, Brushes.Gray, Brushes.White)
                Select Case State
                    Case MouseState.None
                        .FillRectangle(New SolidBrush(m_MainColour), New Rectangle(0, 0, Width, Height))
                        .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                        .DrawString(Text, _Font, bru, New Point(CInt(Width / 2), CInt(Height / 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Case MouseState.Over
                        .FillRectangle(New SolidBrush(_HoverColour), New Rectangle(0, 0, Width, Height))
                        .DrawRectangle(New Pen(_BorderColour, 1), New Rectangle(1, 1, Width - 2, Height - 2))
                        .DrawString(Text, _Font, bru, New Point(CInt(Width / 2), CInt(Height / 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Case MouseState.Down
                        .FillRectangle(New SolidBrush(_PressedColour), New Rectangle(0, 0, Width, Height))
                        .DrawRectangle(New Pen(_BorderColour, 1), New Rectangle(1, 1, Width - 2, Height - 2))
                        .DrawString(Text, _Font, bru, New Point(CInt(Width / 2), CInt(Height / 2)), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                End Select
                .DrawLine(New Pen(_LineColour, 2), New Point(1, CInt(Height / 2)), New Point(1 + 5, CInt(Height / 2)))
                .DrawLine(New Pen(_LineColour, 2), New Point(1, 1), New Point(1, Height - 1))

                .InterpolationMode = CType(7, InterpolationMode)
            End With

        End Sub

#End Region

    End Class

    Public Class LogInGroupBox
        Inherits ContainerControl

#Region "Properties"
        <Category("Colours")>
        Public Property BorderColour As Color
        <Category("Colours")>
        Public Property TextColour As Color
        <Category("Colours")>
        Public Property HeaderColour As Color
        <Category("Colours")>
        Public Property MainColour As Color
#End Region

#Region "Draw Control"
        Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                   ControlStyles.SupportsTransparentBackColor, True)
            DoubleBuffered = True
            Size = New Size(160, 110)
            Font = New Font("Segoe UI", 9, FontStyle.Bold)
            _MainColour = Color.FromArgb(54, 54, 54)
            _HeaderColour = Color.FromArgb(42, 42, 42)
            _TextColour = Color.FromArgb(255, 255, 255)
            _BorderColour = Color.DimGray
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g = e.Graphics
            With g
                .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                .SmoothingMode = SmoothingMode.HighQuality
                .PixelOffsetMode = PixelOffsetMode.HighQuality
                .Clear(Color.FromArgb(54, 54, 54))
                .FillRectangle(New SolidBrush(_MainColour), New Rectangle(0, 28, Width, Height))
                _TextColour = If(Me.Enabled = False, Color.Gray, Color.FromArgb(255, 255, 255))
                .FillRectangle(New SolidBrush(_HeaderColour), New Rectangle(New Point(0, 0), New Size(Width + 10, CInt(.MeasureString(Text, Font).Height + 10))))
                .DrawString(Text, Font, New SolidBrush(_TextColour), New Point(5, 5))

                Dim P() As Point = {New Point(0, 0), New Point(CInt(Me.Width), 0), New Point(CInt(Me.Width), Me.Height), _
                                 New Point(Width, Me.Height), New Point(Width, Height), New Point(0, Height), New Point(0, 0)}
                .DrawLines(New Pen(_BorderColour), P)
                .InterpolationMode = CType(7, InterpolationMode)
            End With
        End Sub
#End Region

    End Class

    Public Class LogInComboBox
        Inherits ComboBox

#Region "Declarations"
        Private State As MouseState = MouseState.None
#End Region

#Region "Properties & Events"

        <Category("Colours")>
        Public Property LineColour As Color
        <Category("Colours")>
        Public Property SqaureColour As Color
        <Category("Colours")>
        Public Property ArrowColour As Color
        <Category("Colours")>
        Public Property SqaureHoverColour As Color
        <Category("Colours")>
        Public Property BorderColour As Color
        <Category("Colours")>
        Public Property BaseColour As Color
        <Category("Colours")>
        Public Property FontColour As Color

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            MyBase.OnMouseEnter(e)
            State = MouseState.Over : Invalidate()
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            MyBase.OnMouseLeave(e)
            State = MouseState.None : Invalidate()
        End Sub

        Protected Overrides Sub OnSelectedItemChanged(e As EventArgs)
            MyBase.OnSelectedItemChanged(e)
        End Sub

        Protected Overrides Sub OnTextChanged(e As EventArgs)
            MyBase.OnTextChanged(e)
            Invalidate()
        End Sub

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            Invalidate()
            OnMouseClick(e)
        End Sub

        Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
            Invalidate()
            MyBase.OnMouseUp(e)
        End Sub

#End Region

#Region "Draw Control"

        Sub ReplaceItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
            e.DrawBackground()
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            Dim Rect As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 1, e.Bounds.Height + 1)
            With e.Graphics
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    .FillRectangle(New SolidBrush(_SqaureColour), Rect)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, New SolidBrush(_FontColour), 1, e.Bounds.Top + 2)
                Else
                    .FillRectangle(New SolidBrush(_BaseColour), Rect)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, New SolidBrush(_FontColour), 1, e.Bounds.Top + 2)
                End If
            End With
            e.DrawFocusRectangle()
            Invalidate()
        End Sub

        Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                   ControlStyles.SupportsTransparentBackColor, True)
            DoubleBuffered = True
            BackColor = Color.Transparent
            DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
            DropDownStyle = ComboBoxStyle.DropDownList
            Width = 163
            Height = 25
            Font = New Font("Segoe UI", 9)
            _BorderColour = Color.FromArgb(35, 35, 35)
            _BaseColour = Color.FromArgb(42, 42, 42)
            _FontColour = Color.FromArgb(255, 255, 255)
            _LineColour = Color.FromArgb(23, 119, 151)
            _SqaureColour = Color.FromArgb(47, 47, 47)
            _ArrowColour = Color.FromArgb(30, 30, 30)
            _SqaureHoverColour = Color.FromArgb(52, 52, 52)
        End Sub

        Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
            Dim g = e.Graphics
            With g
                .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                .SmoothingMode = SmoothingMode.HighQuality
                .PixelOffsetMode = PixelOffsetMode.HighQuality
                .Clear(BackColor)

                Dim Square As New Rectangle(Width - 25, 0, Width, Height)
                .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width - 25, Height))
                Select Case State
                    Case MouseState.None
                        .FillRectangle(New SolidBrush(_SqaureColour), Square)
                    Case MouseState.Over
                        .FillRectangle(New SolidBrush(_SqaureHoverColour), Square)
                End Select
                .DrawLine(New Pen(_LineColour, 2), New Point(Width - 26, 1), New Point(Width - 26, Height - 1))
                If Me.Parent.Enabled = False Then
                    _FontColour = Color.Gray
                Else
                    _FontColour = Color.FromArgb(255, 255, 255)
                End If
                If SelectedIndex <> -1 Then
                    .DrawString(Text, Font, New SolidBrush(_FontColour), New Rectangle(3, 0, Width - 20, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                Else
                    If Not Items Is Nothing And Items.Count > 0 Then
                        SelectedIndex = 0
                        .DrawString(Items(0).ToString, Font, New SolidBrush(_FontColour), New Rectangle(3, 0, Width - 20, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                    End If
                End If
                .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                Dim P() As Point = {New Point(Width - 17, 11), New Point(Width - 13, 5), New Point(Width - 9, 11)}
                .FillPolygon(New SolidBrush(_BorderColour), P)
                .DrawPolygon(New Pen(_ArrowColour), P)
                Dim P1() As Point = {New Point(Width - 17, 15), New Point(Width - 13, 21), New Point(Width - 9, 15)}
                .FillPolygon(New SolidBrush(_BorderColour), P1)
                .DrawPolygon(New Pen(_ArrowColour), P1)
                .InterpolationMode = CType(7, InterpolationMode)
            End With

        End Sub

#End Region

    End Class

    Public Class LogInTabControl
        Inherits TabControl

#Region "Declarations"

        Private ReadOnly CenterSF As New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}

#End Region

#Region "Properties"

        <Category("Colours")> _
        Public Property BorderColour As Color
        <Category("Colours")> _
        Public Property UpLineColour As Color
        <Category("Colours")> _
        Public Property HorizLineColour As Color
        <Category("Colours")> _
        Public Property TextColour As Color
        <Category("Colours")> _
        Public Property BackTabColour As Color
        <Category("Colours")> _
        Public Property BaseColour As Color
        <Category("Colours")> _
        Public Property ActiveColour As Color

        Protected Overrides Sub CreateHandle()
            MyBase.CreateHandle()
            Alignment = TabAlignment.Top
        End Sub

#End Region

#Region "Draw Control"

        Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                     ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
            DoubleBuffered = True
            Font = New Font("Segoe UI", 9)
            SizeMode = TabSizeMode.Normal
            ItemSize = New Size(240, 32)
            _TextColour = Color.FromArgb(255, 255, 255)
            _BackTabColour = Color.FromArgb(54, 54, 54)
            _BaseColour = Color.FromArgb(35, 35, 35)
            _ActiveColour = Color.FromArgb(47, 47, 47)
            _BorderColour = Color.FromArgb(30, 30, 30)
            _UpLineColour = Color.FromArgb(0, 160, 199)
            _HorizLineColour = Color.FromArgb(23, 119, 151)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g = e.Graphics
            With g
                .SmoothingMode = SmoothingMode.HighQuality
                .PixelOffsetMode = PixelOffsetMode.HighQuality
                .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
                .Clear(_BaseColour)
                Try : SelectedTab.BackColor = _BackTabColour : Catch : End Try
                Try : SelectedTab.BorderStyle = BorderStyle.FixedSingle : Catch : End Try
                .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                For i = 0 To TabCount - 1
                    Dim Base As New Rectangle(New Point(GetTabRect(i).Location.X, GetTabRect(i).Location.Y), New Size(GetTabRect(i).Width, GetTabRect(i).Height))
                    Dim BaseSize As New Rectangle(Base.Location, New Size(Base.Width, Base.Height))
                    If i = SelectedIndex Then
                        _TextColour = If(Me.Enabled = False, Color.Gray, Color.White)
                        .FillRectangle(New SolidBrush(_BaseColour), BaseSize)
                        .FillRectangle(New SolidBrush(_ActiveColour), New Rectangle(Base.X + 1, Base.Y - 3, Base.Width, Base.Height + 5))
                        .DrawString(TabPages(i).Text, Font, New SolidBrush(_TextColour), New Rectangle(Base.X + 7, Base.Y, Base.Width - 3, Base.Height), CenterSF)
                        .DrawLine(New Pen(_HorizLineColour, 2), New Point(Base.X + 3, CInt(Base.Height / 2 + 2)), New Point(Base.X + 9, CInt(Base.Height / 2 + 2)))
                        .DrawLine(New Pen(_UpLineColour, 2), New Point(Base.X + 3, Base.Y - 3), New Point(Base.X + 3, Base.Height + 5))
                    Else
                        _TextColour = If(Me.Enabled = False, Color.Gray, Color.White)
                        .DrawString(TabPages(i).Text, Font, New SolidBrush(_TextColour), BaseSize, CenterSF)
                    End If
                Next
                Dim P() As Point = {New Point(0, 0), New Point(CInt(Me.Width), 0), New Point(CInt(Me.Width), Me.Height), _
                               New Point(Width, Me.Height), New Point(Width, Height), New Point(0, Height), New Point(0, 0)}
                .DrawLines(New Pen(_BorderColour), P)
                .InterpolationMode = InterpolationMode.HighQualityBicubic
            End With
        End Sub

#End Region

    End Class

    Public Class LogInListbox
        Inherits ListBox

#Region " Fields "
        Private _selecteditemcolor As Color = Color.Gray
#End Region

#Region " Properties "
        <Category("Colours")>
        Public Property LineColour As Color
        <Category("Colours")>
        Public Property SqaureColour As Color
        <Category("Colours")>
        Public Property ArrowColour As Color
        <Category("Colours")>
        Public Property SqaureHoverColour As Color
        <Category("Colours")>
        Public Property BorderColour As Color
        <Category("Colours")>
        Public Property BaseColour As Color
        <Category("Colours")>
        Public Property FontColour As Color
#End Region

        Sub New()
            SetStyle(ControlStyles.DoubleBuffer, True)
            Font = New Font("Segoe UI", 9)
            BorderStyle = Windows.Forms.BorderStyle.None
            DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
            ItemHeight = 20
            ForeColor = Color.White
            BackColor = Color.FromArgb(42, 42, 42)
            IntegralHeight = False
            _BorderColour = Color.DimGray
            _BaseColour = Color.FromArgb(42, 42, 42)
            _FontColour = Color.FromArgb(255, 255, 255)
            _LineColour = Color.FromArgb(23, 119, 151)
            _SqaureColour = Color.FromArgb(47, 47, 47)
            _ArrowColour = Color.FromArgb(30, 30, 30)
            _SqaureHoverColour = Color.FromArgb(52, 52, 52)
        End Sub

        Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
            Select Case m.Msg
                Case 15
                    MyBase.WndProc(m)
                    CustomPaint()
                    Exit Select
                Case Else
                    MyBase.WndProc(m)
                    Exit Select
            End Select
        End Sub

        Private Sub CustomPaint()
            CreateGraphics.DrawRectangle(New Pen(_BorderColour), New Rectangle(0, 0, Width - 1, Height - 1))
            Dim P() As Point = {New Point(0, 0), New Point(CInt(Me.Width), 0), New Point(CInt(Me.Width), Me.Height), _
                               New Point(Width, Me.Height), New Point(Width, Height), New Point(0, Height), New Point(0, 0)}
            CreateGraphics.DrawLines(New Pen(_BorderColour), P)
        End Sub

        Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
            Try
                If e.Index < 0 Then Exit Sub
                e.DrawBackground()
                Dim rect As New Rectangle(New Point(e.Bounds.Left, e.Bounds.Top + 2), New Size(Bounds.Width, 16))
                Dim bsh As Brush = Brushes.White
                _selecteditemcolor = _SqaureColour
                If Enabled = False Then
                    bsh = Brushes.Gray
                    _selecteditemcolor = Color.FromArgb(200, _SqaureColour)
                End If
                If InStr(e.State.ToString, "Selected,") > 0 Then
                    e.Graphics.FillRectangle(Brushes.Black, e.Bounds)
                    Dim x2 As Rectangle = New Rectangle(e.Bounds.Location, New Size(e.Bounds.Width - 1, e.Bounds.Height))
                    Dim G1 As New LinearGradientBrush(New Point(x2.X, x2.Y), New Point(x2.X, x2.Y + x2.Height), _selecteditemcolor, Color.FromArgb(50, 50, 50))
                    e.Graphics.FillRectangle(G1, x2) : G1.Dispose()
                    e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, bsh, e.Bounds.X, e.Bounds.Y + 1)
                Else
                    e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, bsh, e.Bounds.X, e.Bounds.Y + 1)
                End If

                CustomPaint()

                MyBase.OnDrawItem(e)

            Catch ex As Exception : End Try
        End Sub

    End Class

    Public Class TreeViewEx
        Inherits TreeView

        <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)> _
        Private Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
        End Function

        Protected Overrides Sub CreateHandle()
            MyBase.CreateHandle()
            SetWindowTheme(Me.Handle, "explorer", Nothing)
        End Sub
    End Class

    'PROGRESSBARWITHPERCENTAGE CONTROL SOURCE FROM : http://www.codeproject.com/Articles/33971/ProgressBar-with-Percentage

    <DesignTimeVisible(True), DefaultProperty("Value"), DefaultEvent("ValueChanged"), _
    Description("Component that extends the native .net progressbar with percentage properties and the ability to overlay the percentage")> _
    Public Class TextProgressBar
        Inherits System.Windows.Forms.ProgressBar

#Region "Events"
        ''' <summary>Occurs when the value of the progress bar is changed</summary>
        <Category("Property Changed")> _
        Public Event ValueChanged As EventHandler
        ''' <summary>Occurs when the amount of decimals to be displayed in the percentage is changed</summary>
        <Category("Property Changed")> _
        Public Event PercentageDecimalsChanged As EventHandler
        ''' <summary>Occurs when the visibility of the percentage text is changed</summary>
        <Category("Property Changed")> _
        Public Event PercentageVisibleChanged As EventHandler
        ''' <summary>Occurs when the automatic updating of the percentage is turned on or off</summary>
        <Category("Property Changed")> _
        Public Event AutoUpdatePercentageChanged As EventHandler
        ''' <summary>Occurs when the OverLayColor property is changed</summary>
        <Category("Property Changed")> _
        Public Event OverLayColorChanged As EventHandler
        ''' <summary>Occurs when the align of the percentage text is changed</summary>
        <Category("Property Changed")> _
        Public Event PercentageAlignChanged As EventHandler
        ''' <summary>Occurs when the padding of the percentage text is changed</summary>
        <Category("Property Changed")> _
        Public Shadows Event PaddingChanged As EventHandler
#End Region

#Region "Fields"
        Private Const WM_PAINT As Int32 = &HF 'hex for 15

        Private m_auto_update, m_p_visible As Boolean
        Private m_decimals As Int32
        Private m_p_align As ContentAlignment
        Private m_graphics As Graphics
        Private m_overLayFont As Color
        Private m_drawingRectangle As RectangleF
        Private ReadOnly m_strFormat As New StringFormat

        Private m_TextToShow As String
        Private m_ShowaText As Boolean
#End Region

#Region "Public methods"
        ''' <summary>Create a new instance of a ProgressbarWithPercentage</summary>
        Public Sub New()
            ' Initialize the base class
            MyBase.New()

            ' Set the default values of the properties
            Me.AutoUpdatePercentage = True
            Me.PercentageVisible = True
            Me.PercentageDecimals = 0
            Me.PercentageAlign = ContentAlignment.MiddleCenter
            Me.OverLayColor = Color.Black
            Me.ForeColor = Color.DimGray

            ' Calculate the initial gfx related values
            setGfx()
            setStringFormat()
            setDrawingRectangle()
        End Sub

        ''' <summary>Advances the current possition of the progressbar by the amount of the Step property</summary>
        Public Shadows Sub PerformStep()
            MyBase.PerformStep()
            If Me.PercentageVisible And Me.AutoUpdatePercentage Then Me.ShowPercentage()
        End Sub

        ''' <summary>Show the current percentage as text</summary>
        Public Sub ShowPercentage()
            If m_ShowaText Then
                Me.ShowText(m_TextToShow)
            Else
                Me.ShowText(Math.Round(Me.Percentage, Me.PercentageDecimals).ToString & "%")
            End If
        End Sub

        ''' <summary>Display a string on the progressbar</summary>
        ''' <param name="text">Required. String. The string to display</param>
        Public Sub ShowText(ByVal text As String)
            ' Determine the areas for the ForeColor and OverlayColor
            Dim r1 As RectangleF = Me.ClientRectangle
            r1.Width = CSng(r1.Width * Me.Value / Me.Maximum)
            Dim reg1 As New Region(r1)
            Dim reg2 As New Region(Me.ClientRectangle)
            reg2.Exclude(reg1)

            ' Draw the string
            Me.Graphics.Clip = reg1
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.OverLayColor), Me.DrawingRectangle, m_strFormat)
            Me.Graphics.Clip = reg2
            Me.Graphics.DrawString(text, Me.Font, New SolidBrush(Me.ForeColor), Me.DrawingRectangle, m_strFormat)

            reg1.Dispose()
            reg2.Dispose()
        End Sub
#End Region

#Region "Protected methods"
        Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
            MyBase.OnHandleCreated(e)
            Me.Graphics = Graphics.FromHwnd(Me.Handle)
        End Sub

        Protected Overrides Sub OnHandleDestroyed(ByVal e As System.EventArgs)
            Me.Graphics.Dispose()
            MyBase.OnHandleDestroyed(e)
        End Sub

        Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
            MyBase.WndProc(m)
            If m.Msg = WM_PAINT And Me.PercentageVisible And Me.AutoUpdatePercentage Then ShowPercentage()
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Me.AutoUpdatePercentage = False
            If disposing Then
                Me.Graphics.Dispose()
                m_strFormat.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region

#Region "Private methods"
        Private Sub ProgressbarWithPercentage_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
            setDrawingRectangle()
            setGfx()
        End Sub

        Private Sub ProgressbarWithPercentage_PaddingChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.PaddingChanged
            setDrawingRectangle()
        End Sub

        Private Sub SetGfx()
            ' Set the graphics object
            Me.Graphics = Me.CreateGraphics
        End Sub

        Private Sub SetDrawingRectangle()
            ' Determine the coordinates and size of the drawing rectangle depending on the progress bar size and padding
            Me.DrawingRectangle = New RectangleF(Me.Padding.Left,
                                       Me.Padding.Top,
                                       Me.Width - Me.Padding.Left - Me.Padding.Right,
                                       Me.Height - Me.Padding.Top - Me.Padding.Bottom)
        End Sub

        Private Sub SetStringFormat()
            ' Determine the horizontal alignment
            Select Case Me.PercentageAlign
                Case ContentAlignment.BottomCenter, ContentAlignment.BottomLeft, ContentAlignment.BottomRight
                    m_strFormat.LineAlignment = StringAlignment.Far
                Case ContentAlignment.MiddleCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight
                    m_strFormat.LineAlignment = StringAlignment.Center
                Case ContentAlignment.TopCenter, ContentAlignment.TopLeft, ContentAlignment.TopRight
                    m_strFormat.LineAlignment = StringAlignment.Near
            End Select

            ' Determine the vertical alignment
            Select Case Me.PercentageAlign
                Case ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft
                    m_strFormat.Alignment = StringAlignment.Near
                Case ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter
                    m_strFormat.Alignment = StringAlignment.Center
                Case ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight
                    m_strFormat.Alignment = StringAlignment.Far
            End Select
        End Sub
#End Region

#Region "Properties"

#Region "Appearance"
        <Browsable(True), Category("Appearance"), Description("The Text of the progressbar")> _
        Public Property TextToShow() As String
            Get
                Return m_TextToShow
            End Get
            Set(ByVal value As String)
                m_TextToShow = value
                If Me.PercentageVisible And Me.AutoUpdatePercentage Then Me.ShowPercentage()
            End Set
        End Property

        <Browsable(True), Category("Appearance"), Description("Show a Text to the progressbar")> _
        Public Property ShowAText() As Boolean
            Get
                Return m_ShowaText
            End Get
            Set(ByVal value As Boolean)
                m_ShowaText = value
            End Set
        End Property

        <Browsable(True), Category("Appearance"), Description("The value of the progressbar")> _
        Public Shadows Property Value() As Int32
            Get
                Return MyBase.Value
            End Get
            Set(ByVal value As Int32)
                If value <> Me.Value Then
                    MyBase.Value = value
                    If Me.PercentageVisible And Me.AutoUpdatePercentage Then Me.ShowPercentage()
                    RaiseEvent ValueChanged(Me, New EventArgs)
                End If
            End Set
        End Property

        <Browsable(True), Category("Appearance"), Description("The percentage of the progressbar")> _
        Public Property Percentage() As Double
            Get
                Return Me.Value / Me.Maximum * 100
            End Get
            Set(ByVal value As Double)
                If value >= 0 And value <= 100 Then
                    Me.Value = CInt(Me.Maximum * value / 100)
                    If Me.PercentageVisible And Me.AutoUpdatePercentage Then Me.ShowPercentage()
                End If
            End Set
        End Property

        <Browsable(True), Category("Appearance"), DefaultValue(0), Description("Gets or sets the amount of decimals that will be displayed in the percentage")> _
        Public Overridable Property PercentageDecimals() As Int32
            Get
                Return m_decimals
            End Get
            Set(ByVal value As Int32)
                If value > -1 And value <> Me.PercentageDecimals Then
                    m_decimals = value
                    RaiseEvent PercentageDecimalsChanged(Me, New EventArgs)
                End If
            End Set
        End Property

        <Browsable(True), Category("Appearance"), Description("Gets or sets the font of the percentage text")> _
        Public Overridable Overloads Property Font() As Font
            Get
                Return MyBase.Font
            End Get
            Set(ByVal value As Font)
                MyBase.Font = value
            End Set
        End Property

        <Browsable(True), Category("Appearance"), DefaultValue("MiddleCenter"), Description("Gets or sets if the percentage alignment")> _
        Public Overridable Property PercentageAlign() As ContentAlignment
            Get
                Return m_p_align
            End Get
            Set(ByVal value As ContentAlignment)
                If value <> Me.PercentageAlign Then
                    m_p_align = value
                    setStringFormat()
                    RaiseEvent PercentageAlignChanged(Me, New EventArgs)
                End If
            End Set
        End Property

        <Browsable(True), Category("Appearance"), Description("Gets or sets the color of the percentage text at the place of the progressbar that is indicated")> _
        Public Overridable Property OverLayColor() As Color
            Get
                Return m_overLayFont
            End Get
            Set(ByVal value As Color)
                If Me.m_overLayFont <> value Then
                    m_overLayFont = value
                    RaiseEvent OverLayColorChanged(Me, New EventArgs)
                End If
            End Set
        End Property

        <Browsable(True), Category("Appearance"), DefaultValue(True), Description("Gets or sets if the percentage should be visible")> _
        Public Overridable Property PercentageVisible() As Boolean
            Get
                Return m_p_visible
            End Get
            Set(ByVal value As Boolean)
                If value <> Me.PercentageVisible Then
                    If Not value Then Me.Graphics.Clear(Color.Transparent)
                    m_p_visible = value
                    RaiseEvent PercentageVisibleChanged(Me, New EventArgs)
                End If
            End Set
        End Property
#End Region

#Region "Behavior"
        <Browsable(True), Category("Behavior"), DefaultValue(True), Description("Gets or sets if the percentage should be auto updated")> _
        Public Overridable Property AutoUpdatePercentage() As Boolean
            Get
                Return m_auto_update
            End Get
            Set(ByVal value As Boolean)
                If value <> Me.AutoUpdatePercentage Then
                    m_auto_update = value
                    RaiseEvent AutoUpdatePercentageChanged(Me, New EventArgs)
                End If
            End Set
        End Property
#End Region

#Region "Layout"
        <Browsable(True), Category("Layout"), Description("Gets or sets if the interior spacing of the control")> _
        Public Overridable Overloads Property Padding() As Padding
            Get
                Return MyBase.Padding
            End Get
            Set(ByVal value As Padding)
                MyBase.Padding = value
            End Set
        End Property
#End Region

#Region "Misc"
        Protected Overridable Property Graphics() As Graphics
            Get
                Return m_graphics
            End Get
            Set(ByVal value As Graphics)
                If Me.Graphics IsNot Nothing Then Me.Graphics.Dispose()
                m_graphics = value
            End Set
        End Property

        Private Property DrawingRectangle() As RectangleF
            Get
                Return m_drawingRectangle
            End Get
            Set(ByVal value As RectangleF)
                m_drawingRectangle = value
            End Set
        End Property
#End Region

#End Region

    End Class

End Namespace