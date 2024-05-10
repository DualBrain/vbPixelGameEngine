Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New BamCymbal
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class BamCymbal
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Bam Cymbal"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Clear()

    For f = 0 To 255
      Palette(f) = New Pixel(0, 0, f, f)
    Next

    Return True

  End Function

  Private c As Integer = 0, r As Single = 0!

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const m_delay As Single = 1 / 1200.0!
    t += elapsedTime : If t < m_delay Then Return True Else t -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    SetPixelMode(Pixel.Mode.Alpha)
    For a = 0! To Tau Step 0.00125!
      Dim p = Palette((CInt(a * 80) + c) Mod 256)
      Draw(Cos(a) * r + 400, Sin(a) * r + 240, p)
    Next
    SetPixelMode(Pixel.Mode.Normal)
    c += 1
    r = CSng(Rnd * 240)

    Return True

  End Function

End Class