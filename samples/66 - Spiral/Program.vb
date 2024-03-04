Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Empty
    If game.Construct(700, 700, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Empty
  Inherits PixelGameEngine

  Private m_t As Single
  Private ReadOnly m_delay As Single = 1 / 120.0!

  Private ReadOnly m_size As Integer = 7
  Private m_inc As Double

  Friend Sub New()
    AppName = "Spiral"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Dim bg = New Pixel(7, 36, 18)
    Dim fg = New Pixel(0, 239, 130)

    m_t += elapsedTime : If m_t < m_delay Then Return True Else m_t -= m_delay

    Clear(bg)

    For c = 1 To 2000
      Dim h = c + m_inc
      Dim x = Math.Sin(6 * h / Math.PI) + Math.Sin(3 * h)
      h = c + m_inc * 2
      Dim y = Math.Cos(6 * h / Math.PI) + Math.Cos(3 * h)
      FillCircle(CInt(Fix(m_size * (20 * x + 50))), CInt(Fix(m_size * (20 * y + 50))), 2, fg)
    Next
    m_inc += 0.001!

    Return True

  End Function

End Class