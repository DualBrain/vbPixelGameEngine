Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New Batoidea
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Batoidea
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Batoidea"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    For f = 0 To 255
      Palette(f) = New Pixel(f, f, 128)
    Next
    Return True
  End Function

  Private Const e As Integer = 70
  Private Const s As Integer = 273
  Private Const i As Single = TAU / 180.0!

  Private t As Single = 0!

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    'Static tt As Single : Const m_delay As Single = 1 / 60.0!
    'tt += elapsedTime : If tt < m_delay Then Return True Else tt -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Clear(Palette(1))

    For a = 0 To e
      For v = 0 To s
        Dim x = 60 + 2 * (v + a + 1)
        Dim y = 60 + 2 * (a + 119 - ((-Sin(CSng(a / e * t - PI / 2.0!)) + 1) * (-Sin(CSng(v / s * t - PI / 2.0!)) + 1) * 30))
        DrawLine(x, y, x, 438, Palette(a * 3))
      Next
    Next
    t += i

    Return True

  End Function

End Class