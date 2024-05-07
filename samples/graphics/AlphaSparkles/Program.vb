Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New AlphaSparkles
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class AlphaSparkles
  Inherits PixelGameEngine

  Private ReadOnly p(5) As Pixel

  Friend Sub New()
    AppName = "Alpha Sparkles"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    p(1) = New Pixel(&HFF, &HFF, &HFF, &H1A)
    p(2) = New Pixel(&HC8, &HC8, &HC8, &H1A)
    p(3) = New Pixel(&H91, &H91, &H91, &H1A)
    p(4) = New Pixel(&H91, &H91, &H91, &H1A)
    p(5) = New Pixel(&H91, &H91, &H91, &H1A)

    Clear()

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetMouse(0).Held Then
      Dim dx = GetMouseX, dy = GetMouseY
      SetPixelMode(Pixel.Mode.Alpha)
      For i = 1 To 200
        Dim rl = CSng(Rnd * 250)
        Dim ra = CSng(Rnd * 360)
        DrawLine(dx, dy, dx + rl * MathF.Cos(ra), dy + rl * MathF.Sin(ra), p(CInt((Int(Rnd * 5) + 1))))
      Next
      SetPixelMode(Pixel.Mode.Normal)
    End If

    Return True

  End Function

End Class