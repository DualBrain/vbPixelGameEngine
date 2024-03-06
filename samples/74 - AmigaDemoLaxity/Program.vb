' Based on a Commodore Amiga demo by Laxity (according to Richard Russell)
' Converted to BBC BASIC by Richard Russell
' https://www.facebook.com/100000571741711/videos/422067226929333/
' Some hints on converting to VB thanks to the SpecBAS port by ZXDunny

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Hat
    If game.Construct(512, 512, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Hat
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Amiga Demo by Laxity"
  End Sub

  Private m_t As Single
  Private ReadOnly m_delay As Single = 1 / 60.0!

  Private sw, sh As Integer
  Private sws As Single

  Private ReadOnly x(697) As Integer, y(697) As Integer

  Private j As Single = 4
  Private k As Single = 2
  Private z As Single = 0

  Private c1 As New Pixel(255, 160, 0)
  Private c2 As New Pixel(0, 255, 255)

  Protected Overrides Function OnUserCreate() As Boolean

    sw = ScreenWidth \ 2
    sh = ScreenHeight \ 2
    sws = sh * 0.9!

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime : If m_t < m_delay Then Return True Else m_t -= m_delay

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