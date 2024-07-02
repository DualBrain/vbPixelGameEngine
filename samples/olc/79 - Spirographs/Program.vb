Imports VbPixelGameEngine
Imports System.MathF
Imports Spirographs.Palette
Imports VbPixelGameEngine.QuickGui
Imports System.Net

Friend Module Program

  Sub Main()
    Dim game As New Spirographs
    If game.Construct(768, 512, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Spirographs
  Inherits PixelGameEngine

  Private m_oldPenPoint As Vf2d
  Private m_first As Boolean
  Private m_accumulatedTime As Single

  Private m_p As Palette

  Private m_gui As New Manager
  Private m_slider1 As Slider
  Private m_slider2 As Slider
  Private m_slider3 As Slider
  Private m_button1 As Button
  Private m_button2 As Button
  Private m_checkbox As CheckBox

  Private m_output As Sprite

  Friend Sub New()
    AppName = "Tinkering with Spirographs"
  End Sub

  Private Sub Reset()
    m_first = True
    m_accumulatedTime = 0.0
    'SetDrawTarget(m_output)
    m_output = New Sprite(ScreenWidth, ScreenHeight)
    'SetDrawTarget(Nothing)
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    m_slider1 = New Slider(m_gui, New Vf2d(550, 10), New Vf2d(750, 10), 0, 256, 200)
    m_slider2 = New Slider(m_gui, New Vf2d(550, 30), New Vf2d(750, 30), -256, 256, 77)
    m_slider3 = New Slider(m_gui, New Vf2d(550, 50), New Vf2d(750, 50), 0, 256, 65)
    m_button1 = New Button(m_gui, "Clear All", New Vf2d(550, 80), New Vf2d(100, 16))
    m_checkbox = New CheckBox(m_gui, "Hide Gears", True, New Vf2d(660, 80), New Vf2d(90, 16))
    AddHandler m_checkbox.Clicked, AddressOf Checkbox_Checked
    m_button2 = New Button(m_gui, "Draw!", New Vf2d(550, 110), New Vf2d(200, 20))

    m_p = New Palette(Stock.Spectrum)

    Reset()

    Return True

  End Function

  Private Sub Checkbox_Checked(sender As Object, e As EventArgs)
    If m_checkbox.Checked Then
      m_checkbox.Text = "Hide Gears"
    Else
      m_checkbox.Text = "Show Gears"
    End If
  End Sub

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Clear()

    m_gui.Update(Me)

    If GetKey(Key.R).Pressed OrElse m_button1.Pressed Then
      Reset()
    End If

    If GetKey(Key.SPACE).Held OrElse m_button2.Held Then
      m_accumulatedTime += elapsedTime * 5
    End If

    Dim fixedGearRadius = m_slider1.Value '200.0!
    Dim movingGearRadius = m_slider2.Value '77.0!
    Dim penOffsetRadius = m_slider3.Value '65.0!

    Dim movingGearPos = New Vf2d((fixedGearRadius - movingGearRadius) * Cos(m_accumulatedTime),
                                 (fixedGearRadius - movingGearRadius) * Sin(m_accumulatedTime))

    Dim ratio = If(movingGearRadius <> 0, fixedGearRadius / movingGearRadius, 0)

    Dim penOffset = New Vf2d(penOffsetRadius * Cos(-m_accumulatedTime * ratio),
                             penOffsetRadius * Sin(-m_accumulatedTime * ratio))

    Dim fixedGearPos = New Vf2d(256.0!, 256.0!)
    Dim penPoint = fixedGearPos + movingGearPos + penOffset

    If m_first Then
      m_oldPenPoint = penPoint
      m_first = False
    End If

    If GetKey(Key.SPACE).Held OrElse m_button2.Held Then
      SetDrawTarget(m_output)
      DrawLine(m_oldPenPoint, penPoint, m_p.Sample(m_accumulatedTime / 300.0!))
      SetDrawTarget(Nothing)
    End If

    DrawSprite(0, 0, m_output)

    If m_checkbox.Checked Then
      DrawCircle(fixedGearPos, fixedGearRadius, Presets.White)
      DrawCircle(fixedGearPos + movingGearPos, Math.Abs(movingGearRadius), Presets.White)
      DrawCircle(penPoint, 4, Presets.White)
      DrawLine(fixedGearPos + movingGearPos + penOffset.Norm() * movingGearRadius,
               fixedGearPos + movingGearPos - penOffset.Norm() * movingGearRadius, Presets.White)
    End If

    m_gui.Draw(Me)

    m_oldPenPoint = penPoint

    Return True

  End Function

End Class