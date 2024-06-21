Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New Chip8
    If demo.Construct(64, 32, 8, 8) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class Chip8
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Chip8"
  End Sub

  Private m_machine As Machine

  Protected Overrides Function OnUserCreate() As Boolean

    Dim rom = "tests\1-chip8-logo.ch8"
    'Dim rom = "tests\2-ibm-logo.ch8"
    'Dim rom = "tests\3-corax+.ch8"
    'Dim rom = "tests\4-flags.ch8"
    'Dim rom = "tests\5-quirks.ch8"
    'Dim rom = "tests\6-keypad.ch8"
    'Dim rom = "tests\7-beep.ch8"
    'Dim rom = "tests\8-scrolling.ch8"
    m_machine = New Machine(rom)

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Dim keys = {Key.K1, Key.K2, Key.K3, Key.K4,
                Key.Q, Key.W, Key.E, Key.R,
                Key.A, Key.S, Key.D, Key.F,
                Key.Z, Key.X, Key.C, Key.V}

    For Each key In keys
      If GetKey(key).Pressed Then m_machine.PushKey(key)
      If GetKey(key).Released Then m_machine.ReleaseKey(key)
    Next

    Const CYCLE_TARGET As Single = 1 / 720.0!
    Static cycleT As Single : cycleT += elapsedTime
    If cycleT > CYCLE_TARGET Then

      m_machine.EmulateCycle()
      If m_machine.DT > 0 Then m_machine.DT -= CByte(1)
      If m_machine.ST > 0 Then m_machine.ST -= CByte(1)
      cycleT -= CYCLE_TARGET

    End If

    Static t As Single : Const DELAY As Single = 1 / 60.0!
    t += elapsedTime : If t < DELAY Then Return True Else t -= DELAY

    If m_machine.Repaint Then
      Dim fg = New Pixel(&HFF, &HCC, &H1)
      Dim bg = New Pixel(&H99, &H66, &H1)
      For y = 0 To 31
        For x = 0 To 63
          If m_machine.Display(x, y) = 1 Then
            Draw(x, y, fg)
          ElseIf m_machine.Display(x, y) = 0 Then
            Draw(x, y, bg)
          End If
        Next
      Next
      m_machine.Repaint = False
    End If

    Return True

  End Function

End Class