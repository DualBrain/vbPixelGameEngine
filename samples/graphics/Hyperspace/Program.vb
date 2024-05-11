Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New Hyperspace
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Hyperspace
  Inherits PixelGameEngine

  Private z As Single = CSng(Rnd) * -2
  Private sw, sh As Single
  Private c As Integer = 0
  Private y As Single = 1.0!
  Private Const sz = 23

  Friend Sub New()
    AppName = "Hyperspace"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    sw = SCRW() / 2.0!
    sh = SCRH() / 2.0!

    Palette(1) = New Pixel(0, 0, 255, &HFE - 1)
    Palette(2) = New Pixel(0, 255, 255, &HFE - 2)
    Palette(3) = New Pixel(0, 255, 0, &HFE - 3)
    Palette(4) = New Pixel(255, 255, 0, &HFE - 4)
    Palette(5) = New Pixel(255, 0, 0, &HFE - 5)
    For i = 6 To sz
      Palette(i) = New Pixel(0, 0, i - 6, &HFE)
    Next

    CLS(0)

    Dim l = sw ^ 2 + sh ^ 2

    Dim x As Single, px As Single, py As Single
    For i = 0 To 5000
      If z <= 0 Then x = y : y = CSng(Rnd) * 50 - 25 : z = 0.2
      Dim u = x / z
      Dim v = y / z
      z = If(u ^ 2 + v ^ 2 > l, 0, z - 0.005!)
      c = (1 + c) Mod sz
      If z > 0.195 Then
        PLOT(sw + u, sh + v)
      Else
        DrawLine(px, py, sw + u, sh + v, Palette(c))
      End If
      px = sw + u : py = sh + v
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Dim d = New Dictionary(Of UInteger, Integer)
    For i = 1 To sz
      d.Add(Palette(i).N, i)
    Next

    For x = 0 To ScreenWidth - 1
      For y = 0 To ScreenHeight - 1
        Dim p = GetPixel(x, y), c = 0
        If d.TryGetValue(p.N, c) Then
          c -= 1 : If c < 1 Then c = sz
          Draw(x, y, Palette(c))
        End If
      Next
    Next

    Dim s = Palette(sz)
    For i = sz To 2 Step -1
      Palette(i) = Palette(i - 1)
    Next
    Palette(1) = s

    Return True

  End Function

End Class