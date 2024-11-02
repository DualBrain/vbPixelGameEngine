Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New TenPrint
    If game.Construct(320, 200, 4, 4, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class TenPrint
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "10 PRINT CHR$(205.5+RND(1));:GOTO 10"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    m_display = New Sprite(ScreenWidth, ScreenHeight)
    SetDrawTarget(m_display)
    Clear(bColor)
    SetDrawTarget(Nothing)
    Return True
  End Function

  Private m_display As Sprite
  Private bColor As New Pixel(0, 0, 170)

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static tt As Single : Const DELAY As Single = 5 / 60.0!
    tt += elapsedTime : If tt < DELAY Then Return True Else tt -= DELAY

    Static y As Integer
    Static fColor As Pixel = Presets.White
    Const SEGMENT_LENGTH As Integer = 8

    Const PROBABILITY As Double = 0.43

    ' Draw to buffer...
    SetDrawTarget(m_display)
    Dim ys = y + SEGMENT_LENGTH - 2
    For x = 0 To ScreenWidth - 1 Step SEGMENT_LENGTH
      Dim t = Rnd <= PROBABILITY
      Dim x1 = x + If(Not t, SEGMENT_LENGTH - 2, 0)
      Dim x2 = x + If(t, SEGMENT_LENGTH - 2, 0)
      Dim y1 = y
      Dim y2 = ys
      DrawLine(x1, y1, x2, y2, fColor) ' \
    Next
    If y + SEGMENT_LENGTH > ScreenHeight Then
      y -= SEGMENT_LENGTH
      DrawPartialSprite(New Vi2d(0, 0),
                        m_display,
                        New Vi2d(0, SEGMENT_LENGTH),
                        New Vi2d(ScreenWidth, ScreenHeight - SEGMENT_LENGTH))
      FillRect(0, y, ScreenWidth, SEGMENT_LENGTH, bColor)
    Else
      y += SEGMENT_LENGTH
    End If

    ' Write buffer to screen.
    SetDrawTarget(Nothing)
    DrawSprite(0, 0, m_display)

    Return True

  End Function

End Class