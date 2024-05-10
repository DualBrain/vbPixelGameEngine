Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New Banthrope
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Banthrope
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Banthorpe Plot"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Private dv As Single = 4.0!

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const m_delay As Single = 1 / 60.0!
    t += elapsedTime : If t < m_delay Then Return True Else t -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Clear(Palette(8))

    Dim m1 = 720, v = 620, x1 = m1 / 2.0!, x2 = CSng(x1 ^ 2), y1 = v / 2.0!, y2 = v / 4

    For x5 = 0 To x1
      Dim x4 = CSng(x5 ^ 2.0), m = -y1, a = SQR(x2 - x4)
      For i1 = -a To a Step 7
        Dim r1 = SQR(CSng(x4 + i1 ^ 2)) / x1
        Dim f = (r1 - 1) * Sin(r1 * 12)
        Dim r = CSng(CInt(i1 / dv + f * y2))
        If r > m Then
          m = r : r = y1 - r
          Draw(x1 - x5 + 60, r, Palette(0))
          Draw(x1 + x5 + 60, r, Palette(0))
        End If
      Next
    Next

    dv -= 0.01!

    Return True

  End Function

End Class