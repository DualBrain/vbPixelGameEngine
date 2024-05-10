'NOTE: Not fully working, there appears to be a *flaw*
'      in the existing FillCircle implementation that
'      needs to be investigate. I'm guessing that it
'      isn't as efficient is it should be, where it is
'      drawing more pixels than it needs to be thus the
'      stripe problem seen in this example.

Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New Boxes
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Boxes
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Art Circles 2"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Palette(9) = New Pixel(0, 0, 0, &HFF)
    Return True
  End Function

  Private m_nextDelay As Single = 0.0!

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Static t As Single : t += elapsedTime : If t < m_nextDelay Then Return True Else t -= m_nextDelay

    Clear(Palette(8))

    Dim a = CInt(Rnd * 10) + 2, b = CInt(Rnd * 13) + 3, sc = ScreenWidth / 800.0!, sw = ScreenWidth / 2.0!, sh = ScreenHeight / 2.0!
    SetPixelMode(Function(x As Integer, y As Integer, desired As Pixel, current As Pixel) As Pixel
                   Dim c1 = If(current = Palette(8), 8, 9)
                   Dim c2 = 1
                   Dim c = c1 Xor c2
                   Return Palette(c)
                 End Function)
    For j = 1 To b
      For i = 1 To a
        FillCircle(sw + 60 * sc * MathF.Cos(360.0! / a * i) + 100 * sc * MathF.Cos(360.0! / b * j), sh + sc * 60 * MathF.Sin(360.0! / a * i) + sc * 100 * MathF.Sin(360.0! / b * j), 70 * sc, Palette(1))
      Next
    Next
    SetPixelMode(Pixel.Mode.Normal)

    m_nextDelay = 2.5

    Return True

  End Function

End Class