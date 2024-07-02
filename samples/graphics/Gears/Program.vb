Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Gears
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Gears
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Gears"
  End Sub

  Private Shared Function Fna(b As Double) As Double
    Return Math.Max(Math.Min(Math.Sin(b), 0.7), -0.6)
  End Function

  Private Sub Gear(t As Single, x%, y%, n%, c%, d%)
    Dim ph = t * 20
    Dim cr = n * 6
    Dim crm = cr / 4
    Dim x1 = (x + cr + 15 * Fna(ph))
    Dim y1 = CDbl(y)
    Draw(x1, y1, SpecBAS.Palette(0))
    For i = 0 To Math.Tau + 0.02 Step 0.015
      Dim r = cr + 15 * Fna(n * i + ph)
      Dim x2 = x + r * Math.Cos(i)
      Dim y2 = y + (r * Math.Sin(i * d))
      DrawLine(x1, y1, x2, y2, SpecBAS.Palette(0)) : x1 = x2 : y1 = y2
    Next
    FloodFill(x, y, SpecBAS.Palette(c))
    FillCircle(x, y, crm, SpecBAS.Palette(c + 8))
    DrawCircle(x, y, crm, SpecBAS.Palette(0))
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single
    t += elapsedTime

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Clear(SpecBAS.Palette(28))

    Gear(t, 485, 220, 30, 1, -1) : Gear(t, 215, 240, 15, 2, 1)
    Gear(t, 234, 372, 7, 4, -1) : Gear(t, 83, 74, 20, 6, -1)
    Gear(t, 746, 414, 24, 3, 1) : Gear(t, -10, 518, 40, 5, 1)
    Gear(t, 705, 93, 12, 7, 1)

    Return True

  End Function

End Class