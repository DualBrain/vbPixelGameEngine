' fake-voronoi-plasma by Dav OCT/2023
Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New FakeVoronoiPlasma
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class FakeVoronoiPlasma
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Fake Voronoi Plasma"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    CLS(0)
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Static t As Integer = 4096
    Const s = 3

    SetPixelMode(Pixel.Mode.Alpha)
    For x = 0 To SCRW() Step s
      For y = 0 To SCRH() Step s
        Dim xy = x - y, yx = y - x
        Dim d = SQR(xy * xy + t + yx * yx + t)
        Dim c = New Pixel(CInt(d + x + t) Mod 256, CInt(d + y + t) Mod 256, CInt(d + t) Mod 256, &HA)
        FillRect(x, y, s, s, c)
      Next
    Next
    SetPixelMode(Pixel.Mode.Normal)
    t += 1

    Return True

  End Function

End Class