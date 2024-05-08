Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New AreaFill
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class AreaFill
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Random Area Filling"
  End Sub

  Private m_pass As Integer = 0

  Protected Overrides Function OnUserCreate() As Boolean

    ' Random Area Filling

    Clear()

    For f = 1 To 255
      SpecPalette(f) = New Pixel(128 + f / 2.0!, 64 + f / 1.5!, f)
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    'Static t As Single : Const m_delay As Single = 1 / 120.0!
    't += elapsedTime : If t < m_delay Then Return True Else t -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False
    If GetKey(Key.SPACE).Pressed Then Clear() : m_pass = If(m_pass = 0, 1, 0)

    Dim r = 3 + CSng(Rnd * 100)
    Dim xc = CSng(Rnd * (ScreenWidth - (r * 2))) + r
    Dim yc = CSng(Rnd * (ScreenHeight - (r * 2))) + r
    Dim c = CInt(Rnd * 254) + 1

    If m_pass = 0 Then
      r += 1
      For y = -r To r
        Dim x1 = Int(MathF.Sqrt(r * r - y * y))
        For x = -x1 To x1
          If GetPixel(x + xc, y + yc) <> Presets.Black Then
            Return True
          End If
        Next x
      Next y
      r -= 1
      For y = -r To r
        Dim x1 = Int(MathF.Sqrt(r * r - y * y))
        For x = -x1 To x1
          Draw(x + xc, y + yc, SpecPalette(c))
        Next
      Next
    ElseIf m_pass = 1 Then
      r += 1
      For y = yc - r To yc + r
        For x = xc - r To xc + r
          If GetPixel(x, y) <> Presets.Black Then
            Return True
          End If
        Next
      Next
      r -= 1
      For y = yc - r To yc + r
        For x = xc - r To xc + r
          Draw(x, y, SpecPalette(c))
        Next
      Next
    Else
      Return False
    End If

    Return True

  End Function

End Class