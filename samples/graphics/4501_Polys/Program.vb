Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New Polys
    If game.Construct(800, 600, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Polys
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "1401 Polys"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    'ORIGIN 0, 0 To 250, 150
    Dim c = 180 / MathF.PI

    Dim bg = New Pixel(7, 36, 18)
    Dim fg = New Pixel(0, 239, 130)

    Clear(bg)

    For n = 3 To 19
      Dim a1 = (150 + 40 * n) / c
      Dim x1 = 150 + 4 * n * MathF.Cos(a1)
      Dim y1 = 85 + 4 * n * MathF.Sin(a1)
      For i = 0 To n - 1
        For j = i + 1 To n
          Dim r = 1.2 * n
          Dim a = CSng(i * (360 / n) / c)
          Dim x = CInt(Fix(x1 + r * MathF.Cos(a)))
          Dim y = CInt(Fix(y1 + r * MathF.Sin(a)))
          a = CSng(j * (360 / n) / c)
          Dim xx = CInt(Fix(x1 + r * MathF.Cos(a)))
          Dim yy = CInt(Fix(y1 + r * MathF.Sin(a)))
          DrawLine(x * 3, y * 3, xx * 3, yy * 3, fg)
        Next j
      Next i

    Next n

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Return True

  End Function

End Class