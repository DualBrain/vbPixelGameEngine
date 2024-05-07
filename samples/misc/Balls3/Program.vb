Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Balls
    If game.Construct(640, 360, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Balls
  Inherits PixelGameEngine

  Private ReadOnly m_balls As Integer = 20

  Friend Sub New()
    AppName = "Bouncing Balls 3"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    For a = 1 To m_balls
      m_x(a) = 30
      m_y(a) = 30
      m_h(a) = CSng((Rnd * 2) - 1)
      m_v(a) = CSng((Rnd * 2) - 1)
    Next

    Return True

  End Function

  Private ReadOnly m_x(m_balls) As Single
  Private ReadOnly m_y(m_balls) As Single
  Private ReadOnly m_h(m_balls) As Single
  Private ReadOnly m_v(m_balls) As Single

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const m_delay As Single = 1 / 60.0!
    t += elapsedTime : If t < m_delay Then Return True Else t -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Clear()

    For a = 1 To m_balls
      DrawCircle(CInt(Fix(m_x(a))), CInt(Fix(m_y(a))), 15, Presets.Green)
      m_x(a) = m_x(a) + m_h(a)
      m_y(a) = m_y(a) + m_v(a)
      If m_x(a) > 624 Then m_h(a) = -m_h(a)
      m_x(a) = m_x(a) - 1
      If m_x(a) < 16 Then m_h(a) = -m_h(a)
      m_x(a) = m_x(a) + 1
      If m_y(a) > 343 Then m_v(a) = -m_v(a)
      m_y(a) = m_y(a) - 1
      If m_y(a) < 16 Then m_v(a) = -m_v(a)
      m_y(a) = m_y(a) + 1
    Next

    Return True

  End Function

End Class