Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Circles
    If game.Construct(800, 600, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Circles
  Inherits PixelGameEngine

  Private t As Integer
  Private sw As Integer
  Private sh As Integer
  Private i As Integer
  Private l As Integer

  Friend Sub New()
    AppName = "1500 Circles"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    t = 1500
    sw = ScreenWidth \ 2
    sh = ScreenHeight \ 2
    i = 1
    Clear()

    Return True

  End Function

  Private m_mt As Single

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Dim mt = CSng(1 / 60)

    m_mt += elapsedTime
    If m_mt < mt Then
      Return True
    Else
      m_mt -= mt
    End If

    If l > t Then
      Return True
    Else

      Dim x = CSng(MathF.Cos((10.0! * MathF.PI * l) / t) * (1.0! - 0.5! * (MathF.Cos((16.0! * MathF.PI * l) / t)) ^ 2.0!))
      Dim y = CSng(MathF.Sin((10.0! * MathF.PI * l) / t) * (1.0! - 0.5! * (MathF.Cos((16.0! * MathF.PI * l) / t)) ^ 2.0!))
      Dim r = CSng((1.0! / 100.0!) + (1.0! / 10.0!) * (MathF.Sin((52.0! * MathF.PI * l) / t)) ^ 4.0!)
      If MathF.Abs(r) < 0.010001! Then i = ((i + 1) Mod 7) + 1

      Dim p As Pixel
      Select Case i
        Case 0 : p = New Pixel(0, 0, 0)
        Case 1 : p = New Pixel(1, 0, &HCE)
        Case 2 : p = New Pixel(&HCF, 1, 0)
        Case 3 : p = New Pixel(&HCF, 1, &HCE)
        Case 4 : p = New Pixel(0, &HCF, &H15)
        Case 5 : p = New Pixel(1, &HCF, &HCF)
        Case 6 : p = New Pixel(&HCF, &HCF, &H15)
        Case 7 : p = New Pixel(&HCF, &HCF, &HCF)
        Case Else
      End Select

      Dim z = 280 '160

      DrawCircle(CInt(Fix((x * z) + sw)), CInt(Fix((y * z) + sh)), CInt(Fix(r * 200)), p)

      l += 1

    End If

    Return True

  End Function

End Class