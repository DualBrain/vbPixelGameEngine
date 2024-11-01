Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New SVirjeeFractal
    If game.Construct(800, 480, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class SVirjeeFractal
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "S Virjee Fractal"
  End Sub


  'Private xMax As Integer
  'Private yMax As Integer
  Private centerX As Single
  Private centerY As Single
  Private iter As Integer
  Private zoom As Single
  Private p As Single, q As Single
  Private oldI As Integer
  Private pqsq As Single
  Private L As Single
  Private H As Single
  Private dp As Single
  Private dq As Single
  Private reverse As Boolean = False

  Protected Overrides Function OnUserCreate() As Boolean

    centerX = CSng(ScreenWidth / 2)
    centerY = CSng(ScreenHeight / 2)
    iter = 20
    zoom = 2.8
    p = -0.745
    q = 0.113

    Clear(Presets.Black)

    oldI = 1
    pqsq = (p * p + q * q)
    L = CSng((Math.Sqrt(p * p + q * q) - 1 / iter) * (Math.Sqrt(p * p + q * q) - 1 / iter))
    H = CSng((Math.Sqrt(p * p + q * q) + 1 / iter) * (Math.Sqrt(p * p + q * q) + 1 / iter))

    dp = 0.001 : dq = -0.001 '*

    Return True

  End Function

  Private m_mt As Single

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    'If GetKey(Key.SPACE).Pressed Then
    'End If

    Dim mt = CSng(1 / 60) : m_mt += elapsedTime : If m_mt < mt Then Return True Else m_mt -= mt

    For x = -centerX To centerX
      For y = 0 To ScreenHeight
        Dim c = x / ScreenWidth * (1 - zoom * 1.5)
        Dim d = y / ScreenHeight * (1 - zoom)
        Dim ztot = 0.0
        Dim i = 1
        Dim z = 1.0
        While i < iter AndAlso z < zoom * 1.5
          Dim real = c * c - d * d + p
          Dim imag = 2 * c * d + q
          c = real / Math.Sign(d - i)
          d = imag
          z = (c * c + d * d)
          If z < H Then
            If z > L AndAlso i > 0 Then
              ztot += (1 - (Math.Abs((z - pqsq) / z) / (i / (iter))))
              oldI = i
            End If
          End If
          i += 1
        End While
        If ztot >= 0 Then i = CInt(Math.Sqrt(ztot) * 256)
        Dim blue, green, red As Integer
        Select Case i
          Case Is < 256 : red = 0 : blue = i : green = 0
          Case 256 To 512 : red = 0 : blue = 255 : green = i Mod 256
          Case 513 To 768 : red = i Mod 256 : blue = 255 : green = 255
          Case 769 To 1025 : red = 255 : blue = 255 : green = 255
          Case Is > 1026 : red = 55 : blue = 255 : green = 55
          Case Else
        End Select
        Dim gray = CInt(Int((red + blue + green) * 0.33))
        Dim hot = If(Math.Max(red, Math.Max(green, blue)) < 255, Math.Max(red, Math.Max(green, blue)), 0)
        Select Case oldI
          Case 1 : blue = hot                                              'Outer Circle 1 Figure 8
          Case 2 : green = hot                                             'Outer Circle 2
          Case 3 : blue = hot : red = gray                                 'Inner Circle Figure 8 - Yellow
          Case 4 : red = hot : green = blue : blue = gray                  'Inner to 2/Outer Circle 4 Loops top
          Case Is >= 5 : blue = CInt((hot + red + green) * 0.33) \ (oldI)  'This is main color
        End Select
        Dim pc = New Pixel(red, green, blue)
        Draw(centerX + x, centerY - y, pc)
        Draw(ScreenWidth - centerX - x, centerY + y, pc)
      Next
    Next

    'zoom = CSng(zoom * 0.999)
    'If zoom < 1.4 Then zoom = 5
    If reverse Then
      zoom = CSng(zoom * 1.001)
      If zoom > 5 Then reverse = False
    Else
      zoom = CSng(zoom * 0.999)
      If zoom < 1.4 Then reverse = True
    End If
    p += dp
    If p < -0.8 Then
      dp = -dp : p = -0.8
    ElseIf p > -0.7 Then
      dp = -dp : p = -0.7
    End If
    q += dq
    If q > 0.13 Then
      dq = -dq : q = 0.13
    ElseIf q < 0.11 Then
      dq = -dq : q = 0.11
    End If

    Return True

  End Function

End Class