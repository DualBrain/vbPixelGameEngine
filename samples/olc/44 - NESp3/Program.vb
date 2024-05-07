' Inspired by "NES Emulator Part #3: Buses, RAMs, ROMs & Mappers" -- @javidx9
' https://youtu.be/8XmxKPJDGU0

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New Demo2C02
    If demo.Construct(780, 480, 2, 2) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class Demo2C02
  Inherits PixelGameEngine

  ' The NES
  Private ReadOnly m_nes As New Bus
  Private m_cart As Cartridge
  Private m_emulationRun As Boolean = False
  Private m_residualTime As Single = 0.0F

  ' Support Utilities
  Private m_asm As New Dictionary(Of UShort, String)

  Friend Sub New()
    AppName = "2C02 Demonstration"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    ' Load the cartridge
    m_cart = New Cartridge("assets/nestest.nes")
    If Not m_cart.ImageValid Then Return False

    ' Insert into NES
    m_nes.InsertCartridge(m_cart)

    ' Extract dissassembly
    m_asm = m_nes.cpu.Disassemble(&H0, &HFFFF)

    ' Reset NES
    m_nes.Reset()

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Clear(Presets.DarkBlue)

    If m_emulationRun Then
      If m_residualTime > 0.0F Then
        m_residualTime -= elapsedTime
      Else
        m_residualTime += (1.0F / 60.0F) - elapsedTime
        Do
          m_nes.Clock()
        Loop While Not m_nes.ppu.m_frameComplete
        m_nes.ppu.m_frameComplete = False
      End If
    Else

      ' Emulate code step-by-step
      If GetKey(Key.C).Pressed Then

        ' Clock enough times to execute a whole CPU instruction
        Do
          m_nes.Clock()
        Loop While Not m_nes.cpu.Complete

        ' CPU clock runs slower than system clock, so it may be
        ' complete for additional system clock cycles. Drain
        ' those out
        Do
          m_nes.Clock()
        Loop While m_nes.cpu.Complete()

      End If

      ' Emulate the whole frame
      If GetKey(Key.F).Pressed Then

        ' Clock enough times to draw a single frame
        Do
          m_nes.Clock()
        Loop While Not m_nes.ppu.m_frameComplete

        ' Use residual clock cycles to complete current instruction
        Do
          m_nes.Clock()
        Loop While Not m_nes.cpu.Complete()

        ' Reset frame completion flag
        m_nes.ppu.m_frameComplete = False

      End If

    End If

    If GetKey(Key.SPACE).Pressed Then m_emulationRun = Not m_emulationRun
    If GetKey(Key.R).Pressed Then m_nes.Reset()

    DrawCpu(516, 2)
    DrawCode(516, 72, 26)

    DrawSprite(0, 0, m_nes.ppu.GetScreen, 2)

    'If GetKey(Key.SPACE).Pressed Then
    '  Do
    '    nes.cpu.Clock()
    '  Loop While Not nes.cpu.Complete()
    'End If

    'If GetKey(Key.R).Pressed Then nes.cpu.Reset()
    'If GetKey(Key.I).Pressed Then nes.cpu.Irq()
    'If GetKey(Key.N).Pressed Then nes.cpu.Nmi()

    '' Draw Ram Page 0x00		
    'DrawRam(2, 2, &H0, 16, 16)
    'DrawRam(2, 182, &H8000, 16, 16)
    'DrawCpu(448, 2)
    'DrawCode(448, 72, 26)

    'DrawString(10, 370, "SPACE = Step Instruction    R = RESET    I = IRQ    N = NMI")

    Return True

  End Function

  Public Shared Function Hex(n As UInteger, d As Byte) As String
    Return Microsoft.VisualBasic.Hex(n).PadLeft(d, "0"c)
  End Function

  Public Sub DrawRam(x As Integer, y As Integer, addr As UShort, rows As Integer, columns As Integer)
    Dim ramX = x, ramY As Integer = y
    For row = 0 To rows - 1
      Dim sOffset = "$" & Hex(addr, 4) & ":"
      For col = 0 To columns - 1
        sOffset &= " " & Hex(m_nes.CpuRead(addr, True), 2)
        addr += 1US
      Next
      DrawString(ramX, ramY, sOffset)
      ramY += 10
    Next
  End Sub

  Private Sub DrawCpu(x As Integer, y As Integer)
    'Dim status = "STATUS: "
    DrawString(x, y, "STATUS:", Presets.White)
    DrawString(x + 64, y, "N", If((m_nes.cpu.status And Pge6502.FLAGS6502.N) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 80, y, "V", If((m_nes.cpu.status And Pge6502.FLAGS6502.V) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 96, y, "-", If((m_nes.cpu.status And Pge6502.FLAGS6502.U) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 112, y, "B", If((m_nes.cpu.status And Pge6502.FLAGS6502.B) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 128, y, "D", If((m_nes.cpu.status And Pge6502.FLAGS6502.D) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 144, y, "I", If((m_nes.cpu.status And Pge6502.FLAGS6502.I) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 160, y, "Z", If((m_nes.cpu.status And Pge6502.FLAGS6502.Z) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 178, y, "C", If((m_nes.cpu.status And Pge6502.FLAGS6502.C) <> 0, Presets.Green, Presets.Red))
    DrawString(x, y + 10, "PC: $" & Hex(m_nes.cpu.pc, 4))
    DrawString(x, y + 20, "A: $" & Hex(m_nes.cpu.a, 2) & "  [" & m_nes.cpu.a.ToString() & "]")
    DrawString(x, y + 30, "X: $" & Hex(m_nes.cpu.x, 2) & "  [" & m_nes.cpu.x.ToString() & "]")
    DrawString(x, y + 40, "Y: $" & Hex(m_nes.cpu.y, 2) & "  [" & m_nes.cpu.y.ToString() & "]")
    DrawString(x, y + 50, "Stack P: $" & Hex(m_nes.cpu.stkp, 4))
  End Sub

  Private Sub DrawCode(x As Integer, y As Integer, lines As Integer)

    Dim value As String = Nothing
    If m_asm.TryGetValue(m_nes.cpu.pc, value) Then
      ' Draw the middle code (current)
      Dim nLineY = (lines \ 2) * 10 + y
      DrawString(x, nLineY, value, Presets.Cyan) : nLineY += 10
      ' Draw up to 10 lines after to the current
      Dim nextItems = m_asm.SkipWhile(Function(kvp) kvp.Key <= m_nes.cpu.pc).Take(10)
      For Each kvp In nextItems
        DrawString(x, nLineY, kvp.Value) : nLineY += 10
      Next
      ' Draw up to 10 lines prior to the current
      nLineY = (lines \ 2) * 10 + y : nLineY -= 10
      Dim prevItems = m_asm.TakeWhile(Function(kvp) kvp.Key < m_nes.cpu.pc).Reverse.Take(10)
      For Each kvp In prevItems
        DrawString(x, nLineY, kvp.Value) : nLineY -= 10
      Next
    End If

  End Sub

End Class