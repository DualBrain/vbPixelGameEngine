Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New KimmieFish
    If game.Construct(640, 360, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class KimmieFish
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Kimmie Fish 2"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const m_delay As Single = 1 / 60.0!
    t += elapsedTime : If t < m_delay Then Return True Else t -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Clear()

    Static a As Integer = 690
    Static b As Integer = 300
    Static c As Integer = -100
    Static d As Integer = 100

    Static color1 As Integer = 1
    Static color2 As Integer = 5

    Static counter As Integer = 0
    Static moveRight As Integer = 0
    Static moveRight2 As Integer = 0

    Line(a + 13, b + 0, a + 18, b + 2, color1)
    Line(a + 8, b + 2, a + 13, b + 4, color1)
    Line(a + 18, b + 2, a + 23, b + 4, color1)
    counter += 1 : If counter = 21 Then counter = 0
    If counter < 20 * 0.7 Then Line(a + 33, b + 2, a + 35, b + 4, color1)
    Line(a + 5, b + 4, a + 8, b + 6, color1)
    Line(a + 23, b + 4, a + 25, b + 6, color1)
    Line(a + 30, b + 4, a + 33, b + 6, color1)
    Line(a + 3, b + 6, a + 5, b + 8, color1)
    Line(a + 9, b + 6, a + 11, b + 8, color1)
    Line(a + 23, b + 6, a + 30, b + 8, color1)
    Line(a + 0, b + 8, a + 3, b + 10, color1)
    If counter < 20 * 0.5 Then Line(a + 15, b + 8, a + 18, b + 10, color1)
    Line(a + 28, b + 8, a + 30, b + 10, color1)
    Line(a + 0, b + 10, a + 3, b + 12, color1)
    Line(a + 13, b + 10, a + 15, b + 12, color1)
    If counter < 20 * 0.5 Then Line(a + 15, b + 10, a + 18, b + 12, color1)
    Line(a + 28, b + 10, a + 30, b + 12, color1)
    Line(a + 3, b + 12, a + 8, b + 14, color1)
    Line(a + 23, b + 12, a + 30, b + 14, color1)
    Line(a + 5, b + 14, a + 8, b + 16, color1)
    Line(a + 23, b + 14, a + 25, b + 16, color1)
    Line(a + 30, b + 14, a + 33, b + 16, color1)
    Line(a + 8, b + 16, a + 13, b + 18, color1)
    Line(a + 18, b + 16, a + 23, b + 18, color1)
    If counter < 20 * 0.7 Then Line(a + 33, b + 16, a + 35, b + 18, color1)
    Line(a + 13, b + 18, a + 18, b + 20, color1)

    If a > 640 Then moveRight = -1
    If moveRight = -1 Then a -= 1
    If a = -100 Then a += 800
    If a = -99 Then color1 += 1
    If color1 = 16 Then color1 = 1
    If a = -98 Then b = CInt(Rnd * 280 + 20)

    Line(c + 22, d + 0, c + 17, d + 2, color2)
    Line(c + 27, d + 2, c + 22, d + 4, color2)
    Line(c + 17, d + 2, c + 12, d + 4, color2)
    counter += 1
    If counter = 21 Then counter = 0
    If counter < 20 * 0.7 Then Line(c + 2, d + 2, c + 0, d + 4, color2)
    Line(c + 30, d + 4, c + 27, d + 6, color2)
    Line(c + 12, d + 4, c + 10, d + 6, color2)
    Line(c + 5, d + 4, c + 2, d + 6, color2)
    Line(c + 32, d + 6, c + 30, d + 8, color2)
    Line(c + 26, d + 6, c + 24, d + 8, color2)
    Line(c + 12, d + 6, c + 5, d + 8, color2)
    Line(c + 35, d + 8, c + 32, d + 10, color2)
    If counter < 20 * 0.5 Then Line(c + 20, d + 8, c + 17, d + 10, color2)
    Line(c + 7, d + 8, c + 5, d + 10, color2)
    Line(c + 35, d + 10, c + 32, d + 12, color2)
    Line(c + 22, d + 10, c + 20, d + 12, color2)
    If counter < 20 * 0.5 Then Line(c + 20, d + 10, c + 17, d + 12, color2)
    Line(c + 7, d + 10, c + 5, d + 12, color2)
    Line(c + 32, d + 12, c + 27, d + 14, color2)
    Line(c + 12, d + 12, c + 5, d + 14, color2)
    Line(c + 30, d + 14, c + 27, d + 16, color2)
    Line(c + 12, d + 14, c + 10, d + 16, color2)
    Line(c + 5, d + 14, c + 2, d + 16, color2)
    Line(c + 27, d + 16, c + 22, d + 18, color2)
    Line(c + 17, d + 16, c + 12, d + 18, color2)
    If counter < 20 * 0.7 Then Line(c + 2, d + 16, c + 0, d + 18, color2)
    Line(c + 22, d + 18, c + 17, d + 20, color2)

    If c < -99 Then moveRight2 = 1
    If moveRight2 = 1 Then c += 1
    If c = 740 Then c -= 840
    If c = -98 Then color2 += 1
    If color2 = 16 Then color2 = 1
    If c = -97 Then d = CInt(Rnd * 280 + 20)

    Return True

  End Function

  Private Sub Line(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, c As Integer)
    Dim w = Math.Abs(x2 - x1)
    Dim h = y2 - y1
    Dim x = x1 : If x1 > x2 Then x = x2
    FillRect(x, y1, w, h, QBColor(c))
  End Sub

  Private Shared Function QBColor(c As Integer) As Pixel
    Select Case c
      Case 0 : Return Presets.Black
      Case 1 : Return Presets.DarkBlue
      Case 2 : Return Presets.DarkGreen
      Case 3 : Return Presets.DarkCyan
      Case 4 : Return Presets.DarkRed
      Case 5 : Return Presets.DarkMagenta
      Case 6 : Return Presets.Brown
      Case 7 : Return Presets.DarkGrey
      Case 8 : Return Presets.Gray
      Case 9 : Return Presets.Blue
      Case 10 : Return Presets.Green
      Case 11 : Return Presets.Cyan
      Case 12 : Return Presets.Red
      Case 13 : Return Presets.Magenta
      Case 14 : Return Presets.Yellow
      Case 15 : Return Presets.White
      Case Else
        Stop
    End Select
  End Function

End Class