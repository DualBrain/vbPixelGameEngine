Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

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
    AppName = "6052 Boxes"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Clear()
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Dim w = ScreenHeight

    SetPixelMode(Function(x As Integer, y As Integer, desired As Pixel, current As Pixel) As Pixel
                   Dim c = -1
                   For i = Palette.Count - 1 To 0 Step -1
                     If Palette(i) = current Then c = i : Exit For
                   Next
                   c -= 1 : If c < 0 Then c = 255
                   Return Palette(c)
                 End Function)
    DrawLine(a, 0, w, a)
    DrawLine(w, a, w - a, w)
    DrawLine(w - a, w, 0, w - a)
    DrawLine(0, w - a, a, 0)
    SetPixelMode(Pixel.Mode.Normal)
    a += s : If a > w Then a = 0

    Return True

  End Function

End Class