' Originally by Tiffany Rayside
Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New Boxes
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Boxes
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Atlantis"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    For f = 0 To 255
      Palette(f) = New Pixel(f, f, CInt(64 + (f * 0.75)))
    Next
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static once As Boolean = False
    Static pn As Single
    Static ink As Integer

    If GetKey(Key.ESCAPE).Pressed Then Return False
    If GetKey(Key.SPACE).Pressed Then once = False

    Clear(Palette(0))

    'SetPixelMode(Function(xx As Integer, yy As Integer, desired As Pixel, current As Pixel) As Pixel
    '               Dim c = -1
    '               Dim target = If(current <> SpecPalette(0), current, desired)
    '               For i = SpecPalette.Count - 1 To 0 Step -1
    '                 If SpecPalette(i) = target Then c = i : Exit For
    '               Next
    '               c += 20 : If c > 255 Then c = 0
    '               Return SpecPalette(c)
    '             End Function)

    ' Using this instead of the above as it produces a very similar result as is much faster.
    SetPixelMode(Pixel.Mode.Alpha)

    If Not once Then
      Dim oldn = 0!, n = 0!
      Do While n = oldn
        n = (CSng(Rnd * 40) + 30) / 10
        pn = MathF.PI / n
      Loop
      ink = CInt((n * 5) + 30) '5
      once = True
    End If

    Static t As Single : t += elapsedTime
    Dim num = ScreenHeight, w = ScreenWidth / 2, h = ScreenHeight / 2

    Dim px As Single = 0!, py As Single = 0!

    Dim x = 0!
    Dim b = 0!
    For j = 0 To num - 1
      x -= 0.36!
      Dim y = x * MathF.Sin(3 * t + x / 20.9!) / 0.88!
      Dim x1 = x * MathF.Cos(b) - y * MathF.Sin(b)
      Dim y1 = x * MathF.Sin(b) + y * MathF.Cos(b)
      b = (j * 2) * pn
      DrawLine(px + w, py + h, x1 + w, y1 + h, Palette(ink)) : px = x1 : py = y1
    Next

    SetPixelMode(Pixel.Mode.Normal)

    Return True

  End Function

End Class