Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New RaytracingTex
    If game.Construct(800, 400, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class RaytracingTex
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Raytracing Tex"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Clear()

    Dim spheres = 6
    Dim w = ScreenWidth / 2
    Dim h = ScreenHeight / 2
    Dim s = 0.0
    Dim cl() = {New Pixel(&H0, &H0, &H0),
                New Pixel(&HA0, &HA0, &HA0),
                New Pixel(&HF, &HF, &HF),
                New Pixel(&HFF, &HFF, &HFF),
                New Pixel(&H99, &H99, &H99)}

    Dim c As New List(Of List(Of Single))
    Dim r(6) As Single
    Dim q(6) As Single
    c.Add({0!, 0!, 0!, 0!}.ToList)
    c.Add({0!, -0.3!, -0.8!, 3.0!}.ToList) : r(1) = 0.6! : q(1) = r(1) * r(1)
    c.Add({0!, 0.9!, -1.4!, 3.5!}.ToList) : r(2) = 0.35! : q(2) = r(2) * r(2)
    c.Add({0!, 0.7!, -0.45!, 2.5!}.ToList) : r(3) = 0.4! : q(3) = r(3) * r(3)
    c.Add({0!, -0.5!, -0.3!, 1.5!}.ToList) : r(4) = 0.15! : q(4) = r(4) * r(4)
    c.Add({0!, 1.0!, -0.2!, 1.5!}.ToList) : r(5) = 0.1! : q(5) = r(5) * r(5)
    c.Add({0!, -0.1!, -0.2!, 1.25!}.ToList) : r(6) = 0.2! : q(6) = r(6) * r(6)

    For i = 1 To ScreenHeight
      For j = 0 To ScreenWidth - 1
        Dim x = 0.3, y = -0.5, z = 0.0, ba = 3
        Dim dx = j - w, dy = h - i, dz = (ScreenHeight / 480) * 600
        Dim dd = dx * dx + dy * dy + dz * dz
        Do
          Dim n = CInt((y >= 0 OrElse dy <= 0))
          If n = 0 Then s = (y / dy) * -1
          For k = 1 To spheres
            Dim px = c(k)(1) - x : Dim py = c(k)(2) - y : Dim pz = c(k)(3) - z
            Dim pp = px * px + py * py + pz * pz
            Dim sc = px * dx + py * dy + pz * dz
            If sc > 0 Then
              Dim bb = sc * sc / dd
              Dim aa = q(k) - pp + bb
              If aa > 0 Then
                sc = (Math.Sqrt(bb) - Math.Sqrt(aa)) / Math.Sqrt(dd)
                If sc < s OrElse n <> 0 Then
                  n = k
                  s = sc
                End If
              End If
            End If
          Next k
          If n < 0 Then ' sky
            Dim rval = CInt(178 * (ScreenHeight - i) / ScreenHeight + 128 * (dy * dy / dd)) - 10
            Dim gval = CInt(178 * (ScreenHeight - i) / ScreenHeight + 128 * (dy * dy / dd)) - 10
            Dim bval = CInt(178 + 55 * (dy * dy / dd))
            Dim p = New Pixel(rval, gval, bval)
            Draw(j, ScreenHeight - i, p)
            Exit Do
          Else
            dx *= s : dy *= s : dz *= s : dd = dd * s * s
            x += dx : y += dy : z += dz
            If n = 0 Then
              For k = 1 To spheres
                Dim u = c(k)(1) - x
                Dim v = c(k)(3) - z
                If u * u + v * v <= q(k) Then
                  ba = 1 : k = spheres + 1
                End If
              Next
              If (x - Int(x) > 0.5) = (z - Int(z) > 0.5) Then
                Draw(j, ScreenHeight - i, cl(ba))
              Else
                Draw(j, ScreenHeight - i, cl(ba + 1))
              End If
              Exit Do
            Else
              Dim nx = x - c(n)(1), ny = y - c(n)(2), nz = z - c(n)(3)
              Dim nn = nx * nx + ny * ny + nz * nz
              Dim l = 2 * (dx * nx + dy * ny + dz * nz) / nn
              dx -= nx * l : dy -= ny * l : dz -= nz * l
            End If
          End If
        Loop
      Next
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const DELAY = 1.0! / 60.0!
    t += elapsedTime : If t < DELAY Then Return True Else t -= DELAY

    Return True

  End Function

End Class