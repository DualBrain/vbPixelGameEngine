' Based on a Commodore Amiga demo by Laxity (according to Richard Russell)
' Converted to BBC BASIC by Richard Russell
' https://www.facebook.com/100000571741711/videos/422067226929333/
' Some hints on converting to VB thanks to the SpecBAS port by ZXDunny

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Laxity
    If game.Construct(512, 512, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Laxity
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Amiga Demo by Laxity"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const DELAY As Single = 1 / 60.0!
    t += elapsedTime : If t < DELAY Then Return True Else t -= DELAY

    Static sw As Integer = ScreenWidth \ 2
    Static sh As Integer = ScreenHeight \ 2
    Static sws As Single = sh * 0.9!

    Static c1 As New Pixel(255, 160, 0)
    Static c2 As New Pixel(0, 255, 255)

    Static x(697) As Integer, y(697) As Integer

    Static j As Single = 4
    Static k As Single = 2
    Static z As Single = 0

    Dim a = 0!

    For i = 0 To 697
      Dim m = MathF.PI * (MathF.Sin(a + j) + MathF.Cos(2 * a + k))
      x(i) = CInt(Fix(sw + sws * MathF.Sin(a) * MathF.Sin(m + z)))
      y(i) = CInt(Fix(sh + sws * MathF.Cos(a) * MathF.Cos(m)))
      a += 0.009!
    Next

    Clear()

    ' normal
    Dim px = x(0), py = y(0)
    For i = 1 To 697
      DrawLine(px, py, x(i), y(i), c2)
      px = x(i) : py = y(i)
    Next
    DrawLine(px, py, x(0), y(0), c2)

    ' flipped
    px = x(0) : py = y(697 - 0)
    For i = 1 To 697
      DrawLine(px, py, x(i), y(697 - i), c1)
      px = x(i) : py = y(697 - i)
    Next
    DrawLine(px, py, x(0), y(697 - 0), c1)

    'j += 0.01625!: k += 0.025!: z += 0.075!
    j += 0.008125! : k += 0.0125! : z += 0.0375!

    Return True

  End Function

End Class