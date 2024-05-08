Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New AmigaBall
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class AmigaBall
  Inherits PixelGameEngine

  Private c As Integer = 0

  Friend Sub New()
    AppName = "Amiga Ball"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Dim TAU = 2 * Math.PI
    Dim rad = 200, rad2 = rad ^ 2, scl = 128, scl4 = scl / 4, norm = TAU / scl, xo = ScreenWidth / 2 - rad, yo = ScreenHeight / 2 - rad

    Clear(New Pixel(128, 128, 128))

    For y = -rad To rad
      Dim y2 = y * y
      For x = -rad To rad
        If Math.Sqrt(x * x + y2) < rad Then
          Dim tmp = x / Math.Sqrt(rad2 - Math.Abs(y2))
          Dim xres = Math.Atan(tmp / Math.Sqrt(1 - tmp * tmp)) / norm
          tmp = y / rad
          Dim yres = Math.Atan(tmp / Math.Sqrt(1 - tmp * tmp)) / norm
          Dim col = CInt(xres + scl4)
          If (yres + scl4) Mod 16 < 8 Then col += 8
          col = col Mod 16
          Draw(x + rad + xo, y + rad + yo, SpecPalette(col + 4))
        End If
      Next
    Next

    SpecPalette(c + 4) = New Pixel(255, 255, 255)
    SpecPalette(4 + (c + 8) Mod 16) = New Pixel(255, 0, 0)
    c += If(c > 15, -c, 1)

    Return True

  End Function

End Class