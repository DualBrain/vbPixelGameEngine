Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New BubbleUniverse
    If game.Construct(512, 512, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class BubbleUniverse
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Bubble Universe"
  End Sub

  Private x As Single = 0!
  Private v As Single = 0!
  Private t As Single = 0!
  Private hw As Single
  Private hh As Single

  Private m_t As Single
  Private ReadOnly m_delay As Single = 1 / 60.0!

  Protected Overrides Function OnUserCreate() As Boolean

    hw = ScreenWidth / 2.0!
    hh = ScreenHeight / 2.0!

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime : If m_t < m_delay Then Return True Else m_t -= m_delay

    Clear()

    Dim TAU = 6.28318548!
    Dim n = 200.0!
    Dim r = TAU / 235.0!

    For i = 0 To n
      For j = 0 To n
        Dim u = MathF.Sin(i + v) + MathF.Sin(r * i + x)
        v = MathF.Cos(i + v) + MathF.Cos(r * i + x)
        x = u + t
        Dim p = New Pixel(i, j, 99)
        Draw(CInt(Fix(hw + u * hw * 0.4)), CInt(Fix(hh + v * hh * 0.4)), p)
      Next
    Next
    t += 0.0005! ' slowed .025

    Return True

  End Function

End Class