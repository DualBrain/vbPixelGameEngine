Imports VbPixelGameEngine

'https://www.facebook.com/groups/2057165187928233/permalink/3587586958219374/

Friend Module Program

  Sub Main()
    Dim game As New Hat
    If game.Construct(320, 240, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Hat
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Commodore PET Hat"
  End Sub

  Private m_t As Single
  Private ReadOnly m_delay As Single = 1 / 60.0!

  Protected Overrides Function OnUserCreate() As Boolean

    Dim green = New Pixel(0, 255, 0)
    Dim black = New Pixel(0, 0, 0)

    Dim scrX = ScreenWidth
    Dim scrY = ScreenHeight

    Dim p = scrX \ 2
    Dim q = scrY \ 2

    Dim xp = 144
    Dim xr = 1.5 * 3.1415927
    Dim yp = 56
    Dim yr = 1
    Dim zp = 64

    Dim xf = xr / xp
    Dim yf = yp / yr
    Dim zf = xr / zp

    Clear()

    DrawString(80, 10, "THE COMMODORE PET HAT", green)

    Dim lineRemoval = False

    For zi = -q To q
      If zi >= -zp AndAlso zi <= zp Then
        Dim zt = zi * xp / zp
        Dim zz = zi
        Dim xl = CInt(Fix(0.5 + Math.Sqrt(xp * xp - zt * zt)))
        For xi = -xl To xl + 1
          Dim xt = Math.Sqrt(xi * xi + zt * zt) * xf
          Dim xx = xi
          Dim yy = CInt(Fix((Math.Sin(xt) + 0.4 * Math.Sin(3 * xt)) * yf))
          Dim x1 = xx + zz + p
          Dim y1 = scrY - (yy - zz + q)
          Draw(x1, y1, green)
          If y1 <> 0 Then
            If lineRemoval Then DrawLine(x1, y1 + 1, x1, scrY, black)
          End If
        Next
      End If
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime : If m_t < m_delay Then Return True Else m_t -= m_delay

    Return True

  End Function

End Class