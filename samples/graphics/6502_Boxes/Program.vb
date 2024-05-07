Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Boxes
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Boxes
  Inherits PixelGameEngine

  Private Const s As Single = 0.5!
  Private a As Single = 0!

  Friend Sub New()
    AppName = "Boxes"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Clear()
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Dim w = ScreenHeight

    SetPixelMode(Function(x As Integer, y As Integer, desired As Pixel, current As Pixel) As Pixel
                   Dim r = CInt(current.R) - CInt(desired.R)
                   Dim g = CInt(desired.G)
                   Dim b = CInt(desired.B)
                   If r < 0 Then r = 255 + r
                   If r > 255 Then r -= 255
                   Return New Pixel(r, g, b)
                 End Function)
    DrawLine(a, 0, w, a, Presets.Gray)
    DrawLine(w, a, w - a, w, Presets.Gray)
    DrawLine(w - a, w, 0, w - a, Presets.Gray)
    DrawLine(0, w - a, a, 0, Presets.Gray)
    SetPixelMode(Pixel.Mode.Normal)
    a += s : If a > w Then a = 0

    Return True

  End Function

End Class