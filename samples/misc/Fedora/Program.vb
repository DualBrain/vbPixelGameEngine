Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Empty
    If game.Construct(320, 200, 4, 4) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Empty
  Inherits PixelGameEngine

  Private ReadOnly RR(320) As Integer
  Private XP As Integer
  Private XR As Double
  Private XF As Double
  Private ZI As Integer
  Private XI As Integer

  Private m_mt As Single

  Friend Sub New()
    AppName = "Fedora"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Clear()

    For I = 0 To 320
      RR(I) = 193
    Next I

    XP = 144
    XR = 4.71238905
    XF = XR / XP
    ZI = 64

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    'm_mt += elapsedTime
    'If m_mt > 0.05 Then
    '  m_mt -= 0.001!
    'Else
    '  Return True
    'End If

    If ZI < -64 Then Return True

    Dim ZT = ZI * 2.25, ZS = ZT * ZT
    Dim XL = CInt(Fix(Math.Sqrt(20736 - ZS) + 0.5))
    Dim ZX = ZI + 160, ZY = 90 + ZI

    Dim XT = Math.Sqrt(XI * XI + ZS) * XF
    Dim YY = (Math.Sin(XT) + Math.Sin(XT * 3) * 0.4) * 56
    Dim X1 = XI + ZX, Y1 = CInt(Fix(ZY - YY))
    If RR(X1) > Y1 Then
      RR(X1) = Y1
      Draw(X1, Y1, Presets.Yellow)
    End If
    X1 = ZX - XI
    If RR(X1) > Y1 Then
      RR(X1) = Y1
      Draw(X1, Y1, Presets.Gray)
    End If

    If XI > XL Then
      XI = 0
      ZI -= 1
    Else
      XI += 1
    End If

    Return True

  End Function

End Class