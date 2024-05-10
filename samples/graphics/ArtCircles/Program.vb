'NOTE: Not fully working, there appears to be a *flaw*
'      in the existing FillCircle implementation that
'      needs to be investigate. I'm guessing that it
'      isn't as efficient is it should be, where it is
'      drawing more pixels than it needs to be thus the
'      stripe problem seen in this example.

Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New ArtCircles
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class ArtCircles
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Art Circles"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Clear(Palette(8))

    Dim data = New List(Of (X As Integer,
                            Y As Integer,
                            R As Integer)) From {(0, 0, 210),
                                                 (0, -240, 230),
                                                 (-100, -465, 250),
                                                 (-200, -740, 300),
                                                 (-300, -1100, 350),
                                                 (-400, -1500, 400)}

    Dim cx = ScreenWidth \ 2, cy = ScreenHeight \ 2, s = (ScreenHeight / 2.0!) / 1905.0!
    SetPixelMode(Function(x, y, desired, current) As Pixel
                   ' Recolor
                   Dim c = -1
                   For i = Palette.Count - 1 To 0 Step -1
                     If Palette(i) = current Then c = i : Exit For
                   Next
                   Dim r = If(c = 0, 8, 0) 'And &HFF
                   Return Palette(r)
                 End Function)
    For f = 1 To 6
      Dim x = data(f - 1).X * s, y = data(f - 1).Y * s, r = data(f - 1).R * s
      FillCircle(cx + x, cy + y, r)
      If f > 1 Then
        FillCircle(cx + x, cy - y + 1, r)
        If f > 2 Then
          FillCircle(cx - x + 1, cy + y, r)
          FillCircle(cx - x + 1, cy - y + 1, r)
        End If
      End If
    Next
    SetPixelMode(Pixel.Mode.Normal)

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    'Static t As Single : Const m_delay As Single = 1 / 60.0!
    't += elapsedTime : If t < m_delay Then Return True Else t -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Return True

  End Function

End Class