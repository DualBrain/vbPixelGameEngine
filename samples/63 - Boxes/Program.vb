Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New Boxes
    If demo.Construct(800, 600, 1, 1) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class Boxes
  Inherits PixelGameEngine

  Private m_t As Single

  Private sw, sh, d, z0, p, q As Double
  Private i, j As Double

  Friend Sub New()
    AppName = "Boxes"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    d = 300
    z0 = 100
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

    Clear()

    j += 1
    For i = 2 To 10
      Box(-50 + 100 * Math.Sin((i + j) * 0.1), 100, i * 50, 50, 50, 50, New Pixel(0, 255, 0))
      Box(50 + 100 * Math.Sin((i + j) * 0.1), 100, i * 50, 50, 50, 50, New Pixel(0, 255, 0))
      Box(-50 + 100 * Math.Sin((i + j) * 0.1), 0, i * 50, 50, 50, 50, New Pixel(255, 255, 0))
      Box(50 + 100 * Math.Sin((i + j) * 0.1), 0, i * 50, 50, 50, 50, New Pixel(255, 255, 0))
      Box(-50 + 100 * Math.Sin((i + j) * 0.1), -100, i * 50, 50, 50, 50, New Pixel(0, 255, 255))
      Box(50 + 100 * Math.Sin((i + j) * 0.1), -100, i * 50, 50, 50, 50, New Pixel(0, 255, 255))
    Next

    Return True

  End Function

  Private Sub Box(x As Double, y As Double, z As Double, w As Double, l As Double, h As Double, pixel As Pixel)
    Proj(x - w / 2, y - h / 2, z - l / 2)
    Dim sx = p, sy = q
    Proj(x + w / 2, y + h / 2, z - l / 2)
    DrawRect(CInt(Fix(sx)), CInt(Fix(sy)), CInt(Fix(p - sx)), CInt(Fix(q - sy)), pixel) : sx = p : sy = q
    Proj(x + w / 2, y + h / 2, z + l / 2)
    DrawLine(CInt(Fix(sx)), CInt(Fix(sy)), CInt(Fix(p)), CInt(Fix(q)), pixel) : sx = p : sy = q
    Proj(x - w / 2, y - h / 2, z + l / 2)
    DrawRect(CInt(Fix(sx)), CInt(Fix(sy)), CInt(Fix(p - sx)), CInt(Fix(q - sy)), pixel) : sx = p : sy = q
    Proj(x - w / 2, y - h / 2, z - l / 2)
    DrawLine(CInt(Fix(sx)), CInt(Fix(sy)), CInt(Fix(p)), CInt(Fix(q)), pixel) ': sx = p : sy = q
    Proj(x - w / 2, y + h / 2, z - l / 2) : sx = p : sy = q
    Proj(x - w / 2, y + h / 2, z + l / 2)
    DrawLine(CInt(Fix(sx)), CInt(Fix(sy)), CInt(Fix(p)), CInt(Fix(q)), pixel) ': sx = p : sy = q
    Proj(x + w / 2, y - h / 2, z - l / 2) : sx = p : sy = q
    Proj(x + w / 2, y - h / 2, z + l / 2)
    DrawLine(CInt(Fix(sx)), CInt(Fix(sy)), CInt(Fix(p)), CInt(Fix(q)), pixel)
  End Sub

  Private Sub Proj(x As Double, y As Double, z As Double)
    p = sw / 2 + x * d / (z + z0)
    q = sh / 2 - y * d / (z + z0)
  End Sub

End Class