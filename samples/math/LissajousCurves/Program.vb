' https://www.youtube.com/watch?v=cM8ZYifXfHE

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New LissajousCurves
    If game.Construct(320, 240, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class LissajousCurves
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Lissajous Curves"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Clear()
    Dim c = 1, dx = 35, dy = 35, pi = 3.1415926535
    Dim xs, ys As Double
    For a = 1 To 4
      For b = 1 To 3
        c += 1
        For t = 0 To 2 * pi Step 0.02
          Dim x = dx * (2 * a - 1) + dx * Math.Sin(a * t + t + 1) * 0.75 + 15
          Dim y = dy * (2 * b - 1) + dy * Math.Sin(b * t + t) * 0.75 + 15
          If t = 0 Then Draw(x, y, QBColor(c)) : xs = x : ys = y : Continue For
          DrawLine(xs, ys, x, y, QBColor(c)) : xs = x : ys = y
        Next
      Next
    Next
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    If GetKey(Key.ESCAPE).Pressed Then Return False
    Return True
  End Function

End Class