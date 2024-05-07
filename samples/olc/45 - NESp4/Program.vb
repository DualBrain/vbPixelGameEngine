' Inspired by "NES Emulator Part #4: PPU - Background Rendering" -- @javidx9
' https://youtu.be/-THeUXqR3zY

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

  Private m_selectedPalette As Byte = 0

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

    ' Sneaky peek of controller input in next video! ;P
    m_nes.controller(0) = &H0
    If GetKey(Key.X).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H80)
    If GetKey(Key.Z).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H40)
    If GetKey(Key.A).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H20)
    If GetKey(Key.S).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H10)
    If GetKey(Key.UP).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H8)
    If GetKey(Key.DOWN).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H4)
    If GetKey(Key.LEFT).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H2)
    If GetKey(Key.RIGHT).Held Then m_nes.controller(0) = CByte(m_nes.controller(0) Or &H1)

    If GetKey(Key.SPACE).Pressed Then m_emulationRun = Not m_emulationRun
    If GetKey(Key.R).Pressed Then m_nes.Reset()
    If GetKey(Key.P).Pressed Then m_selectedPalette = CByte((m_selectedPalette + 1) And &H7)

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

    DrawCpu(516, 2)
    DrawCode(516, 72, 26)

    ' Draw Palettes & Pattern Tables ==============================================
    Dim nSwatchSize As Integer = 6
    For p = 0 To 7 ' For each palette
      For s = 0 To 3 ' For each index
        FillRect(516 + p * (nSwatchSize * 5) + s * nSwatchSize, 340, nSwatchSize, nSwatchSize, m_nes.ppu.GetColourFromPaletteRam(p, s))
      Next
    Next

    ' Draw selection reticule around selected palette
    DrawRect(516 + m_selectedPalette * (nSwatchSize * 5) - 1, 339, (nSwatchSize * 4), nSwatchSize, Presets.White)

    ' Generate Pattern Tables
    DrawSprite(516, 348, m_nes.ppu.GetPatternTable(0, m_selectedPalette))
    DrawSprite(648, 348, m_nes.ppu.GetPatternTable(1, m_selectedPalette))

    ' Draw rendered output ========================================================
    DrawSprite(0, 0, m_nes.ppu.GetScreen, 2)

    Return True

  End Function

  Public Shared Function Hex(n As UInteger, d As Byte) As String
    Return Microsoft.VisualBasic.Hex(n).PadLeft(d, "0"c)
  End Function

  Public Sub DrawRam(x As Integer, y As Integer, nAddr As UShort, nRows As Integer, nColumns As Integer)
    Dim nRamX = x, nRamY As Integer = y
    For row = 0 To nRows - 1
      Dim sOffset = "$" & Hex(nAddr, 4) & ":"
      For col = 0 To nColumns - 1
        sOffset &= " " & Hex(m_nes.CpuRead(nAddr, True), 2)
        nAddr += 1US
      Next
      DrawString(nRamX, nRamY, sOffset)
      nRamY += 10
    Next
  End Sub

  Private Sub DrawCpu(x As Integer, y As Integer)
    'Dim status = "STATUS: "
    DrawString(x, y, "STATUS:", Presets.White)
    DrawString(x + 64, y, "N", If((m_nes.cpu.status And Pge6502.Flags6502.N) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 80, y, "V", If((m_nes.cpu.status And Pge6502.Flags6502.V) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 96, y, "-", If((m_nes.cpu.status And Pge6502.Flags6502.U) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 112, y, "B", If((m_nes.cpu.status And Pge6502.Flags6502.B) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 128, y, "D", If((m_nes.cpu.status And Pge6502.Flags6502.D) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 144, y, "I", If((m_nes.cpu.status And Pge6502.Flags6502.I) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 160, y, "Z", If((m_nes.cpu.status And Pge6502.Flags6502.Z) <> 0, Presets.Green, Presets.Red))
    DrawString(x + 178, y, "C", If((m_nes.cpu.status And Pge6502.Flags6502.C) <> 0, Presets.Green, Presets.Red))
    DrawString(x, y + 10, "PC: $" & Hex(m_nes.cpu.pc, 4))
    DrawString(x, y + 20, "A: $" & Hex(m_nes.cpu.a, 2) & "  [" & m_nes.cpu.a.ToString() & "]")
    DrawString(x, y + 30, "X: $" & Hex(m_nes.cpu.x, 2) & "  [" & m_nes.cpu.x.ToString() & "]")
    DrawString(x, y + 40, "Y: $" & Hex(m_nes.cpu.y, 2) & "  [" & m_nes.cpu.y.ToString() & "]")
    DrawString(x, y + 50, "Stack P: $" & Hex(m_nes.cpu.stkp, 4))
  End Sub

  Private Sub DrawCode(x As Integer, y As Integer, nLines As Integer)

    Dim value As String = Nothing
    If m_asm.TryGetValue(m_nes.cpu.pc, value) Then
      ' Draw the middle code (current)
      Dim nLineY = (nLines \ 2) * 10 + y
      DrawString(x, nLineY, value, Presets.Cyan) : nLineY += 10
      ' Draw up to 10 lines after to the current
      Dim nextItems = m_asm.SkipWhile(Function(kvp) kvp.Key <= m_nes.cpu.pc).Take(10)
      For Each kvp In nextItems
        DrawString(x, nLineY, kvp.Value) : nLineY += 10
      Next
      ' Draw up to 10 lines prior to the current
      nLineY = (nLines \ 2) * 10 + y : nLineY -= 10
      Dim prevItems = m_asm.TakeWhile(Function(kvp) kvp.Key < m_nes.cpu.pc).Reverse.Take(10)
      For Each kvp In prevItems
        DrawString(x, nLineY, kvp.Value) : nLineY -= 10
      Next
    End If

  End Sub

End Class