Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New XmasTree
    If demo.Construct(800, 600, 1, 1) Then ', False, True) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class XmasTree
  Inherits PixelGameEngine

  Private m_t As Single

  Private pi, sw, sh, d, z0, p, q As Double
  Private a, b, x, y, z, xx, yy, zz As Double

  Friend Sub New()
    AppName = "Xmas Tree"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    pi = 4 * Math.Atan(1)
    d = 700
    z0 = 2500

    sw = 800
    sh = 600

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    ' Limit to about 50fps
    m_t += elapsedTime
    If m_t > 1 / 50 Then
      m_t -= CSng(1 / 50)
    Else
      Return True
    End If

    b += 0.03

    Clear()

    For a = 0 To 20 * 2 * pi Step 0.1

      x = 5 * a * Math.Cos(a)
      y = -a * 10
      z = 5 * a * Math.Sin(a)

      yy = (y + 350) * Math.Cos(b) - z * Math.Sin(b)
      zz = (y + 350) * Math.Sin(b) + z * Math.Cos(b)
      y = yy - 350
      z = zz

      xx = x * Math.Cos(b) - z * Math.Sin(b)
      zz = x * Math.Sin(b) + z * Math.Cos(b)
      x = xx
      z = zz

      xx = x * Math.Cos(b) - (y + 350) * Math.Sin(b)
      yy = x * Math.Sin(b) + (y + 350) * Math.Cos(b)
      x = xx
      y = yy - 350

      Proj(x, y, z)

      DrawCircle(CInt(Fix(p)), CInt(Fix(q)), 1, New Pixel(0, 155, 0))

    Next

    For a = 0 To 6 * pi Step 0.1

      Dim rr = 100, r = 60

      x = (rr - r) * Math.Cos(a + pi / 4) + 70 * Math.Cos(((rr - r) / r) * a)
      y = 50 + (rr - r) * Math.Sin(a + pi / 4) - 70 * Math.Sin(((rr - r) / r) * a)
      z = 0

      yy = (y + 350) * Math.Cos(b) - z * Math.Sin(b)
      zz = (y + 350) * Math.Sin(b) + z * Math.Cos(b)
      y = yy - 350
      z = zz

      xx = x * Math.Cos(b) - z * Math.Sin(b)
      zz = x * Math.Sin(b) + z * Math.Cos(b)
      x = xx
      z = zz

      xx = x * Math.Cos(b) - (y + 350) * Math.Sin(b)
      yy = x * Math.Sin(b) + (y + 350) * Math.Cos(b)
      x = xx
      y = yy - 350

      Proj(x, y, z)

      DrawCircle(CInt(Fix(p)), CInt(Fix(q)), 1, New Pixel(255, 255, 0))

    Next

    Return True

  End Function

  Private Sub Proj(x As Double, y As Double, z As Double)
    p = sw / 2 + x * d / (z + z0)
    q = sh / 2 - (100 + y) * d / (z + z0) - 150
  End Sub

End Class