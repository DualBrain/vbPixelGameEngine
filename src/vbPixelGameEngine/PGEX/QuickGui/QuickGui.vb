Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Runtime.Intrinsics.X86
Imports System.Text.RegularExpressions

Namespace QuickGui

  Public Enum State
    Disabled
    Normal
    Hover
    Click
  End Enum

  Public Enum Alignment
    Left
    Center
    Right
  End Enum

  ''' <summary>
  ''' Virtual base class for all controls
  ''' </summary>
  Public MustInherit Class BaseControl

    ''' <summary>
    ''' Controls are related to a manager, where the theme resides and control groups can be implemented
    ''' </summary>
    Protected m_manager As Manager ' Switches
    ''' <summary>
    ''' All controls exist in one of four states
    ''' </summary>
    Protected m_state As State = State.Normal

    ''' <summary>
    ''' To add a "swish" to things, controls can fade between states
    ''' </summary>
    Protected m_transition As Single = 0.0!

    Public Sub New(manager As Manager)
      m_manager = manager
      m_manager.AddControl(Me)
    End Sub

    ''' <summary>
    ''' Switches the control on/off
    ''' </summary>
    ''' <param name="enabled"></param>
    Public Overridable Sub Enable(enabled As Boolean)
      m_state = If(enabled, State.Normal, State.Disabled)
    End Sub

    ''' <summary>
    ''' Sets whether or not the control is interactive/displayed
    ''' </summary>
    ''' <returns></returns>
    Public Property Visible As Boolean = True

    ''' <summary>
    ''' True on single frame control begins being manipulated
    ''' </summary>
    ''' <returns></returns>
    Public Property Pressed As Boolean

    ''' <summary>
    ''' True on all frames control is under user manipulation
    ''' </summary>
    ''' <returns></returns>
    Public Property Held As Boolean

    ''' <summary>
    ''' True on single frame control ceases being manipulated
    ''' </summary>
    ''' <returns></returns>
    Public Property Released As Boolean

    ''' <summary>
    ''' Updates the controls behavior
    ''' </summary>
    ''' <param name="pge"></param>
    Public Overridable Sub Update(pge As PixelGameEngine)
    End Sub
    ''' <summary>
    ''' Draws the control using "sprite" based CPU operations
    ''' </summary>
    ''' <param name="pge"></param>
    Public MustOverride Sub Draw(pge As PixelGameEngine)
    ''' <summary>
    ''' Draws the control using "decal" based GPU operations
    ''' </summary>
    ''' <param name="pge"></param>
    Public MustOverride Sub DrawDecal(pge As PixelGameEngine)

  End Class

  ''' <summary>
  ''' A QuickGui.Manager acts as a convenient grouping of controls
  ''' </summary>
  Public Class Manager

    Private ReadOnly m_eraseControlsOnDestroy As Boolean
    Private ReadOnly m_controls As New List(Of BaseControl)

    ' This managers "Theme" can be set here
    ' Various element colors
    Public Property NormalColor As Pixel = Presets.DarkBlue
    Public Property HoverColor As Pixel = Presets.Blue
    Public Property ClickColor As Pixel = Presets.Cyan
    Public Property DisableColor As Pixel = Presets.DarkGrey
    Public Property BorderColor As Pixel = Presets.White
    Public Property TextColor As Pixel = Presets.White

    ''' <summary>
    ''' Speed to transition from Normal -> Hover
    ''' </summary>
    ''' <returns></returns>
    Public Property HoverSpeedOn As Single = 10.0!
    ''' <summary>
    ''' Speed to transition from Hover -> Normal
    ''' </summary>
    ''' <returns></returns>
    Public Property HoverSpeedOff As Single = 4.0!
    ''' <summary>
    ''' Size of grabe handle
    ''' </summary>
    ''' <returns></returns>
    Public Property GrabRadius As Single = 8.0!

    Public Sub CopyThemeFrom(manager As Manager)
      BorderColor = manager.BorderColor
      ClickColor = manager.ClickColor
      DisableColor = manager.DisableColor
      HoverColor = manager.HoverColor
      NormalColor = manager.NormalColor
      TextColor = manager.TextColor
      GrabRadius = manager.GrabRadius
      HoverSpeedOff = manager.HoverSpeedOff
      HoverSpeedOn = manager.HoverSpeedOn
    End Sub

    Public Sub New()
      m_eraseControlsOnDestroy = True
      m_controls = New List(Of BaseControl)()
    End Sub

    Public Sub New(cleanUpForMe As Boolean)
      m_eraseControlsOnDestroy = cleanUpForMe
      m_controls = New List(Of BaseControl)()
    End Sub

    Protected Overrides Sub Finalize()
      If m_eraseControlsOnDestroy Then
        'For Each p In m_controls
        '  p = Nothing
        'Next
        m_controls.Clear()
      End If
    End Sub

    ''' <summary>
    ''' Add a gui element derived from BaseControl to this manager
    ''' </summary>
    ''' <param name="control"></param>
    Public Sub AddControl(control As BaseControl)
      m_controls.Add(control)
    End Sub

    ''' <summary>
    ''' Updates all controls this manager operates
    ''' </summary>
    ''' <param name="pge"></param>
    Public Sub Update(pge As PixelGameEngine)
      For Each p In m_controls
        p.Update(pge)
      Next
    End Sub

    ''' <summary>
    ''' Draws as "sprite" all controls this manager operates
    ''' </summary>
    ''' <param name="pge"></param>
    Public Sub Draw(pge As PixelGameEngine)
      For Each p In m_controls
        p.Draw(pge)
      Next
    End Sub

    ''' <summary>
    ''' Draws as "decal" all controls this manager operates
    ''' </summary>
    ''' <param name="pge"></param>
    Public Sub DrawDecal(pge As PixelGameEngine)
      For Each p In m_controls
        p.DrawDecal(pge)
      Next
    End Sub

  End Class

  ''' <summary>
  ''' Creates a Label control - it's just text!
  ''' </summary>
  Public Class Label
    Inherits BaseControl

    Public Sub New(manager As Manager, text As String, pos As Vf2d, size As Vf2d)
      MyBase.New(manager)
      Me.Text = text
      Position = pos
      Me.Size = size
    End Sub

    ''' <summary>
    ''' Position of button
    ''' </summary>
    ''' <returns></returns>
    Public Property Position As Vf2d

    ''' <summary>
    ''' Size of button
    ''' </summary>
    ''' <returns></returns>
    Public Property Size As Vf2d

    ''' <summary>
    ''' Text displayed on button
    ''' </summary>
    ''' <returns></returns>
    Public Property Text As String

    ''' <summary>
    ''' Show a border?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBorder As Boolean = False

    ''' <summary>
    ''' Show a background?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBackground As Boolean = False

    ''' <summary>
    ''' Where should the text be positioned?
    ''' </summary>
    ''' <returns></returns>
    Public Property Alignment As Alignment = Alignment.Center

    Public Overrides Sub Update(pge As PixelGameEngine)
      ' Implementation here
    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)
      If Not Visible Then Return

      If HasBackground Then
        pge.FillRect(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      End If

      If HasBorder Then
        pge.DrawRect(Position, Size - New Vf2d(1, 1), m_manager.BorderColor)
      End If

      Dim vText = pge.GetTextSizeProp(Text)
      Select Case Alignment
        Case Alignment.Left
          pge.DrawStringProp(New Vf2d(Position.x + 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
        Case Alignment.Center
          pge.DrawStringProp(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)
        Case Alignment.Right
          pge.DrawStringProp(New Vf2d(Position.x + Size.x - vText.x - 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      End Select
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then Return

      'If HasBackground Then
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      'End If

      'If HasBorder Then
      '  pge.SetDecalMode(DecalMode.WIREFRAME)
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.BorderColor)
      '  pge.SetDecalMode(DecalMode.NORMAL)
      'End If

      'Dim vText = pge.GetTextSizeProp(Text)
      'Select Case Alignment
      '  Case Alignment.Left
      '    pge.DrawStringPropDecal(New Vf2d(Position.x + 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      '  Case Alignment.Center
      '    pge.DrawStringPropDecal(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)
      '  Case Alignment.Right
      '    pge.DrawStringPropDecal(New Vf2d(Position.x + Size.x - vText.x - 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      'End Select

    End Sub

  End Class

  Public Class TextBox
    Inherits Label

    Private m_textEdit As Boolean

    Public Sub New(manager As Manager, text As String, pos As Vf2d, size As Vf2d)
      MyBase.New(manager, text, pos, size)
      Alignment = Alignment.Left
      HasBorder = True
      HasBackground = False
    End Sub

    Public Overrides Sub Update(pge As PixelGameEngine)

      If m_state = State.Disabled OrElse Not Visible Then Return

      Pressed = False
      Released = False

      Dim vMouse = pge.GetMousePos()

      If vMouse.x >= Position.x AndAlso vMouse.x < Position.x + Size.x AndAlso
          vMouse.y >= Position.y AndAlso vMouse.y < Position.y + Size.y Then

        Pressed = pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed
        Released = pge.GetMouse(PixelGameEngine.Mouse.Left).Released

        If Pressed AndAlso pge.IsTextEntryEnabled() AndAlso Not m_textEdit Then
          pge.TextEntryEnable(False)
        End If

        If Pressed AndAlso Not pge.IsTextEntryEnabled() AndAlso Not m_textEdit Then
          pge.TextEntryEnable(True, Text)
          m_textEdit = True
        End If

        Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held

      Else
        Pressed = pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed
        Released = pge.GetMouse(PixelGameEngine.Mouse.Left).Released
        Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held

        If Pressed AndAlso m_textEdit Then
          Text = pge.TextEntryGetString()
          pge.TextEntryEnable(False)
          m_textEdit = False
        End If
      End If

      If m_textEdit AndAlso pge.IsTextEntryEnabled() Then
        Text = pge.TextEntryGetString()
      End If

    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)

      If Not Visible Then Return

      If HasBackground Then
        pge.FillRect(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      End If

      If HasBorder Then
        pge.DrawRect(Position, Size - New Vf2d(1, 1), m_manager.BorderColor)
      End If

      Dim vText = pge.GetTextSizeProp(Text)
      Select Case Alignment
        Case Alignment.Left
          pge.DrawStringProp(New Vf2d(Position.x + 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
        Case Alignment.Center
          pge.DrawStringProp(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)
        Case Alignment.Right
          pge.DrawStringProp(New Vf2d(Position.x + Size.x - vText.x - 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      End Select
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then Return

      'If HasBackground Then
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      'End If

      'If HasBorder Then
      '  pge.SetDecalMode(DecalMode.WIREFRAME)
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.BorderColor)
      '  pge.SetDecalMode(DecalMode.NORMAL)
      'End If

      'Dim vText = pge.GetTextSizeProp(Text)
      'Select Case Alignment
      '  Case Alignment.Left
      '    pge.DrawStringPropDecal(New Vf2d(Position.x + 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      '  Case Alignment.Centre
      '    pge.DrawStringPropDecal(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)
      '  Case Alignment.Right
      '    pge.DrawStringPropDecal(New Vf2d(Position.x + Size.x - vText.x - 2.0F, Position.y + (Size.y - vText.y) * 0.5F), Text, m_manager.TextColor)
      'End Select

    End Sub

  End Class

  ''' <summary>
  ''' Creates a Button Control - a clickable, labeled rectangle
  ''' </summary>
  Public Class Button
    Inherits BaseControl

    Public Sub New(manager As Manager, text As String, pos As Vf2d, size As Vf2d)
      MyBase.New(manager)
      Me.Text = text
      Position = pos
      Me.Size = size
    End Sub

    ''' <summary>
    ''' Position of button
    ''' </summary>
    ''' <returns></returns>
    Public Property Position As Vf2d
    ''' <summary>
    ''' Size of button
    ''' </summary>
    ''' <returns></returns>
    Public Property Size As Vf2d
    ''' <summary>
    ''' Text displayed on button
    ''' </summary>
    ''' <returns></returns>
    Public Property Text As String

    Public Event Clicked As EventHandler(Of EventArgs)

    Public Overrides Sub Update(pge As PixelGameEngine)

      If m_state = State.Disabled OrElse Not Visible Then
        Return
      End If

      Pressed = False
      Released = False
      Dim elapsedTime = pge.GetElapsedTime()

      Dim vMouse = pge.GetMousePos()
      If m_state <> State.Click Then
        If vMouse.x >= Position.x AndAlso vMouse.x < Position.x + Size.x AndAlso
           vMouse.y >= Position.y AndAlso vMouse.y < Position.y + Size.y Then
          m_transition += elapsedTime * m_manager.HoverSpeedOn
          m_state = State.Hover

          Pressed = pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed
          If Pressed Then
            m_state = State.Click
            RaiseEvent Clicked(Me, EventArgs.Empty)
          End If

          Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held
        Else
          m_transition -= elapsedTime * m_manager.HoverSpeedOff
          m_state = State.Normal
        End If
      Else
        Held = pge.GetMouse(PixelGameEngine.Mouse.Left).Held
        Released = pge.GetMouse(PixelGameEngine.Mouse.Left).Released
        If Released Then
          m_state = State.Normal
          RaiseEvent Clicked(Me, EventArgs.Empty)
        End If
      End If

      m_transition = Math.Clamp(m_transition, 0.0F, 1.0F)

    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)

      If Not Visible Then
        Return
      End If

      Select Case m_state
        Case State.Disabled
          pge.FillRect(Position, Size, m_manager.DisableColor)
        Case State.Normal, State.Hover
          pge.FillRect(Position, Size, Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
        Case State.Click
          pge.FillRect(Position, Size, m_manager.ClickColor)
      End Select

      pge.DrawRect(Position, Size - New Vf2d(1, 1), m_manager.BorderColor)
      Dim vText = pge.GetTextSizeProp(Text)
      pge.DrawStringProp(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)

    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then
      '  Return
      'End If

      'Select Case m_state
      '  Case State.Disabled
      '    pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.DisableColor)
      '  Case State.Normal, State.Hover
      '    pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
      '  Case State.Click
      '    pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.ClickColor)
      'End Select

      'pge.SetDecalMode(DecalMode.WIREFRAME)
      'pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.BorderColor)
      'pge.SetDecalMode(DecalMode.NORMAL)

      'Dim vText = pge.GetTextSizeProp(Text)
      'pge.DrawStringPropDecal(Position + (Size - vText) * 0.5F, Text, m_manager.TextColor)

    End Sub

  End Class

  Public Class ImageButton
    Inherits Button

    Public Sub New(manager As Manager, icon As Sprite, pos As Vf2d, size As Vf2d)
      MyBase.New(manager, String.Empty, pos, size)
      Me.Icon = icon
    End Sub

    Public Property Icon As Sprite 'Renderable

    Public Overrides Sub Update(pge As PixelGameEngine)
    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)
      MyBase.Draw(pge)
      pge.DrawSprite(Position + New Vi2d(4, 4), Icon) '.Sprite())
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)
      Throw New NotImplementedException
      'MyBase.DrawDecal(pge)
      'pge.DrawDecal(Position + New Vi2d(4, 4), Icon.Decal())
    End Sub

  End Class

  ''' <summary>
  ''' Creates a Button Control - a clickable, labeled rectangle
  ''' </summary>
  Public Class CheckBox
    Inherits Button

    Public Property Checked As Boolean = False

    Public Sub New(manager As Manager, text As String, check As Boolean, pos As Vf2d, size As Vf2d)
      MyBase.New(manager, text, pos, size)
      Checked = check
    End Sub

    Public Overrides Sub Update(pge As PixelGameEngine)
      If m_state = State.Disabled OrElse Not Visible Then Return
      MyBase.Update(pge)
      If Pressed Then
        Checked = Not Checked
      End If
    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)
      If Not Visible Then Return
      MyBase.Draw(pge)
      If Checked Then
        pge.DrawRect(Position + New Vf2d(2, 2), Size - New Vi2d(5, 5), m_manager.BorderColor)
      End If
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)
      Throw New NotImplementedException
    End Sub

  End Class

  Public Class ImageCheckBox
    Inherits ImageButton

    Public Property Checked As Boolean = False

    Public Sub New(manager As Manager, icon As Sprite, check As Boolean, pos As Vf2d, size As Vf2d)
      MyBase.New(manager, icon, pos, size)
      Checked = check
    End Sub

    Public Overrides Sub Update(pge As PixelGameEngine)
      If m_state = State.Disabled OrElse Not Visible Then Return
      MyBase.Update(pge)
      If Pressed Then Checked = Not Checked
    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)
      MyBase.Draw(pge)
      If Checked Then
        pge.DrawRect(Position + New Vf2d(2, 2), Size - New Vi2d(5, 5), m_manager.BorderColor)
      End If
    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)
      Throw New NotImplementedException
    End Sub

  End Class

  ''' <summary>
  ''' Creates a Slider Control - a grabbable handle that slides between two locations
  ''' </summary>
  Public Class Slider
    Inherits BaseControl

    Public Sub New(manager As Manager, posMin As Vf2d, posMax As Vf2d, valMin As Single, valMax As Single, value As Single)
      MyBase.New(manager)
      ' Initialization code here
      Me.MinimumPosition = posMin
      Me.MaximumPosition = posMax
      Me.Minimum = valMin
      Me.Maximum = valMax
      Me.Value = value
    End Sub

    ''' <summary>
    ''' Minimum value
    ''' </summary>
    ''' <returns></returns>
    Public Property Minimum As Single = -100.0F
    ''' <summary>
    ''' Maximum value
    ''' </summary>
    ''' <returns></returns>
    Public Property Maximum As Single = 100.0F
    ''' <summary>
    ''' Current value
    ''' </summary>
    ''' <returns></returns>
    Public Property Value As Single = 0.0F

    ''' <summary>
    ''' Location of minimum/start
    ''' </summary>
    ''' <returns></returns>
    Public Property MinimumPosition As Vf2d
    ''' <summary>
    ''' Location of maximum/end
    ''' </summary>
    ''' <returns></returns>
    Public Property MaximumPosition As Vf2d

    Public Overrides Sub Update(pge As PixelGameEngine)

      If m_state = State.Disabled OrElse Not Visible Then
        Return
      End If

      Dim fElapsedTime As Single = pge.GetElapsedTime()

      Dim vMouse = pge.GetMousePos()
      Held = False
      If m_state = State.Click Then
        Dim d = MaximumPosition - MinimumPosition
        Dim u As Single = d.Dot(vMouse - MinimumPosition) / d.Mag2()
        Value = u * (Maximum - Minimum) + Minimum
        Held = True
      Else
        Dim vSliderPos = MinimumPosition + (MaximumPosition - MinimumPosition) * ((Value - Minimum) / (Maximum - Minimum))
        If (vMouse - vSliderPos).Mag2() <= CInt(m_manager.GrabRadius) * CInt(m_manager.GrabRadius) Then
          m_transition += fElapsedTime * m_manager.HoverSpeedOn
          m_state = State.Hover
          If pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed Then
            m_state = State.Click
            Pressed = True
          End If
        Else
          m_state = State.Normal
        End If
      End If

      If pge.GetMouse(PixelGameEngine.Mouse.Left).Released Then
        m_state = State.Normal
        Released = True
      End If

      If m_state = State.Normal Then
        m_transition -= fElapsedTime * m_manager.HoverSpeedOff
        m_state = State.Normal
        Held = False
      End If

      Value = Math.Clamp(Value, Minimum, Maximum)
      m_transition = Math.Clamp(m_transition, 0.0F, 1.0F)

    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)

      If Not Visible Then
        Return
      End If

      pge.DrawLine(MinimumPosition, MaximumPosition, m_manager.BorderColor)
      Dim vSliderPos = MinimumPosition + (MaximumPosition - MinimumPosition) * ((Value - Minimum) / (Maximum - Minimum))

      Select Case m_state
        Case State.Disabled
          pge.FillCircle(vSliderPos, CInt(m_manager.GrabRadius), m_manager.DisableColor)
        Case State.Normal, State.Hover
          pge.FillCircle(vSliderPos, CInt(m_manager.GrabRadius), Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
        Case State.Click
          pge.FillCircle(vSliderPos, CInt(m_manager.GrabRadius), m_manager.ClickColor)
      End Select

      pge.DrawCircle(vSliderPos, CInt(m_manager.GrabRadius), m_manager.BorderColor)

    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then
      '  Return
      'End If

      'pge.DrawLineDecal(MinimumPosition, MaximumPosition, m_manager.BorderColor)
      'Dim vSliderPos = MinimumPosition + (MaximumPosition - MinimumPosition) * ((Value - Minimum) / (Maximum - Minimum))

      'Select Case m_state
      '  Case State.Disabled
      '    pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, m_manager.DisableColor)
      '  Case State.Normal, State.Hover
      '    pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, Pixel.Lerp(m_manager.NormalColor, m_manager.HoverColor, m_transition))
      '  Case State.Click
      '    pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, m_manager.ClickColor)
      'End Select

      'pge.SetDecalMode(DecalMode.WIREFRAME)
      'pge.FillRectDecal(vSliderPos - New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius), New Vf2d(m_manager.GrabRadius, m_manager.GrabRadius) * 2.0F, m_manager.BorderColor)
      'pge.SetDecalMode(DecalMode.NORMAL)

    End Sub

  End Class

  Public Class ListBox
    Inherits BaseControl

    Public Sub New(manager As Manager, list As List(Of String), pos As Vf2d, size As Vf2d)
      MyBase.New(manager)
      Me.List = list
      Group.CopyThemeFrom(m_manager)
      Position = pos
      size = size
      Slider = New Slider(Group,
                          New Vf2d(pos.x + size.x - m_manager.GrabRadius - 1, pos.y + m_manager.GrabRadius + 1),
                          New Vf2d(pos.x + size.x - m_manager.GrabRadius - 1, pos.y + size.y - m_manager.GrabRadius - 1),
                          0, CSng(list.Count), 0)
    End Sub

    ''' <summary>
    ''' Position of list
    ''' </summary>
    ''' <returns></returns>
    Public Property Position As Vf2d
    ''' <summary>
    ''' Size of list
    ''' </summary>
    ''' <returns></returns>
    Public Property Size As Vf2d
    ''' <summary>
    ''' Show a border?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBorder As Boolean = True
    ''' <summary>
    ''' Show a background?
    ''' </summary>
    ''' <returns></returns>
    Public Property HasBackground As Boolean = True

    Public Property Slider As Slider = Nothing
    Public Property Group As Manager
    Public Property VisibleItems As Integer = 0
    Public Property List As List(Of String)

    ''' <summary>
    ''' Item currently selected
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectedItem As Integer = 0
    Public Property PreviouslySelectedItem As Integer = 0
    ''' <summary>
    ''' Has selection changed?
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectionChanged As Boolean = False

    Public Overrides Sub Update(pge As PixelGameEngine)

      If m_state = State.Disabled OrElse Not Visible Then Return

      PreviouslySelectedItem = SelectedItem
      Dim vMouse = pge.GetMousePos() - Position + New Vi2d(2, 0)
      If pge.GetMouse(PixelGameEngine.Mouse.Left).Pressed Then
        If vMouse.x >= 0 AndAlso vMouse.x < Size.x - (Group.GrabRadius * 2) AndAlso vMouse.y >= 0 AndAlso vMouse.y < Size.y Then
          SelectedItem = CInt(Slider.Value + vMouse.y / 10)
        End If
      End If

      SelectedItem = Math.Clamp(SelectedItem, 0, List.Count - 1)

      SelectionChanged = SelectedItem <> PreviouslySelectedItem

      Slider.Maximum = CSng(List.Count)
      Group.Update(pge)

    End Sub

    Public Overrides Sub Draw(pge As PixelGameEngine)

      If Not Visible Then Return

      If HasBackground Then
        pge.FillRect(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      End If

      If HasBorder Then
        pge.DrawRect(Position, Size - New Vf2d(1, 1), m_manager.BorderColor)
      End If

      Dim idx0 As UInteger = CUInt(Slider.Value)
      Dim idx1 As UInteger = Math.Min(idx0 + CUInt((Size.y - 4) / 10), CUInt(List.Count))

      Dim vTextPos = Position + New Vf2d(2, 2)
      For idx As UInteger = idx0 To idx1 - 1UI
        If idx = SelectedItem Then
          pge.FillRect(vTextPos - New Vi2d(1, 1), New Vi2d(CInt(Size.x - Group.GrabRadius * 2), 10), Group.HoverColor)
        End If
        pge.DrawStringProp(vTextPos, List(CInt(idx)))
        vTextPos.y += 10
      Next

      Group.Draw(pge)

    End Sub

    Public Overrides Sub DrawDecal(pge As PixelGameEngine)

      Throw New NotImplementedException

      'If Not Visible Then Return

      'If HasBackground Then
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.NormalColor)
      'End If

      'Dim idx0 As UInteger = CUInt(Slider.Value)
      'Dim idx1 As UInteger = Math.Min(idx0 + CUInt((Size.y - 4) / 10), CUInt(List.Count))

      'Dim vTextPos = Position + New Vf2d(2, 2)
      'For idx As UInteger = idx0 To idx1 - 1UL
      '  If idx = SelectedItem Then
      '    pge.FillRectDecal(vTextPos - New Vi2d(1, 1), New Vf2d(Size.x - Group.GrabRadius * 2.0F, 10.0F), Group.HoverColor)
      '  End If
      '  pge.DrawStringPropDecal(vTextPos, List(CInt(idx)))
      '  vTextPos.y += 10
      'Next

      'If HasBorder Then
      '  pge.SetDecalMode(DecalMode.WIREFRAME)
      '  pge.FillRectDecal(Position + New Vf2d(1, 1), Size - New Vf2d(2, 2), m_manager.BorderColor)
      '  pge.SetDecalMode(DecalMode.NORMAL)
      'End If

      'Group.DrawDecal(pge)

    End Sub

  End Class

  Public Class ModalDialog
    Inherits PgeX

    Public Sub New()
      MyBase.New() 'True)

      ' Create File Open Dialog
      Dim vScreenSize = Pge.GetScreenSize()

      m_directoryListBox = New ListBox(m_fileSelectManager, m_directories, New Vf2d(20, 20), New Vf2d(300, 500))
      m_fileListBox = New ListBox(m_fileSelectManager, m_files, New Vf2d(330, 20), New Vf2d(300, 500))

      m_path = "/"
      For Each dir_entry In Directory.EnumerateFileSystemEntries(m_path)
        If Directory.Exists(dir_entry) Then
          m_directories.Add(Path.GetFileName(dir_entry))
        Else
          m_files.Add(Path.GetFileName(dir_entry))
        End If
      Next
    End Sub

    Public Sub ShowFileOpen(path As String)
      m_showDialog = True
    End Sub

    'Public Overrides Function OnBeforeUserUpdate(ByRef elapsedTime As Single) As Boolean

    '  If Not m_showDialog Then Return False

    '  m_fileSelectManager.Update(Pge)

    '  If Pge.GetKey(PixelGameEngine.Key.BACK).Pressed Then
    '    m_path = Directory.GetParent(m_path).FullName + "/"
    '    'm_directoryListBox.SelectionChanged = True
    '    'm_directoryListBox.SelectedItem = 0
    '  End If

    '  If m_directoryListBox.SelectionChanged Then
    '    Dim directory = m_directories(CInt(m_directoryListBox.SelectedItem))
    '    'If directory = ".." Then
    '    '    m_path = Directory.GetParent(m_path).FullName + "/"
    '    'Else
    '    '    m_path += directory + "/"
    '    'End If

    '    m_path += directory + "/"
    '    ' Reconstruct Lists
    '    m_directories.Clear()
    '    m_files.Clear()

    '    For Each dir_entry As String In m_directories.EnumerateFileSystemEntries(m_path)
    '      If IO.Directory.Exists(dir_entry) Then
    '        m_directories.Add(Path.GetFileName(dir_entry))
    '      Else
    '        m_files.Add(Path.GetFileName(dir_entry))
    '      End If
    '    Next

    '    'm_vDirectory.Add("..")
    '    'm_listFiles.nSelectedItem = 0
    '    'm_listDirectory.nSelectedItem = 0
    '  End If

    '  Pge.DrawStringDecal(New Vi2d(0, 0), m_path)

    '  m_fileSelectManager.DrawDecal(Pge)

    '  If Pge.GetKey(PixelGameEngine.Key.ESCAPE).Pressed Then
    '    m_showDialog = False
    '    Return False
    '  End If

    '  Return True

    'End Function

    Private m_showDialog As Boolean = False

    Private m_fileSelectManager As Manager
    Private m_volumesListBox As ListBox = Nothing
    Private m_directoryListBox As ListBox = Nothing
    Private m_fileListBox As ListBox = Nothing

    Private m_volumes As List(Of String)
    Private m_directories As List(Of String)
    Private m_files As List(Of String)
    Private m_path As String

  End Class

End Namespace