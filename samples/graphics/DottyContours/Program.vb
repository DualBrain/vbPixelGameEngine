Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New DottyContours
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class DottyContours
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Dotty Contours"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Private Const w As Integer = 960

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    'Static tt As Single : Const m_delay As Single = 1 / 60.0!
    'tt += elapsedTime : If tt < m_delay Then Return True Else tt -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Clear(Palette(8))

    Static t As Single : t += 0.0025!
    Dim f = MAP(Sin(t), -1, 1, 64, 200)

    For y = 0 To ScreenHeight Step 8
      For x = 0 To ScreenWidth Step 8
        Dim r = OCTNOISE(CSng(x / w), CSng(y / w - t), 0, 4, 0.5)
        FillCircle(Cos(r * f) * Tau + x, Sin(r) * Tau + y, 0.5 + (Abs(Sin(r))) * 4, Palette(0))
      Next
    Next

    Return True

  End Function

End Class