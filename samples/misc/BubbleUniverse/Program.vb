Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New BubbleUniverse
    If game.Construct(512, 512, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class BubbleUniverse
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Bubble Universe"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const DELAY As Single = 1 / 60.0!
    t += elapsedTime : If t < DELAY Then Return True Else t -= DELAY

    Static x As Single = 0!
    Static v As Single = 0!
    Static tm As Single = 0!

    Clear()

    Dim TAU = MathF.PI * 2
    Dim n = 200.0!
    Dim r = TAU / 235.0!

    Dim hw = ScreenWidth \ 2
    Dim hh = ScreenHeight \ 2

    For i = 0 To n
      For j = 0 To n
        Dim u = MathF.Sin(i + v) + MathF.Sin(r * i + x)
        v = MathF.Cos(i + v) + MathF.Cos(r * i + x)
        x = u + tm
        Dim p = New Pixel(i, j, 99)
        Draw(CInt(Fix(hw + u * hw * 0.4)), CInt(Fix(hh + v * hh * 0.4)), p)
      Next
    Next
    tm += 0.0005! ' slowed .025

    Return True

  End Function

End Class