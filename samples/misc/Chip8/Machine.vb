Imports System.IO
Imports VbPixelGameEngine.PixelGameEngine

Public Class Machine

  Private Const vF As Integer = &HF
  Private Const ZERO As Byte = 0
  Private Const ONE As Byte = 1

  Private ReadOnly m_rng As New Random

  Private ReadOnly m_rom As String
  Private ReadOnly m_ram(&HFFF) As Byte
  Private ReadOnly Registers(&HF) As Byte
  Private m_i As New UShort
  Private m_pc As New UShort
  Private m_sp As New Byte
  Private ReadOnly m_stack(&HF) As UShort

  Public Display(63, 31) As Integer
  Public Keys(&HF) As Boolean

  Public Property ST As Byte
  Public Property DT As Byte
  Public Property Repaint As Boolean = False
  Public Property Emulating As Boolean = False

  Public Sub PushKey(key As Key)
    Select Case key
      Case Key.K1 : Keys(&H1) = True
      Case Key.K2 : Keys(&H2) = True
      Case Key.K3 : Keys(&H3) = True
      Case Key.K4 : Keys(&HC) = True
      Case Key.Q : Keys(&H4) = True
      Case Key.W : Keys(&H5) = True
      Case Key.E : Keys(&H6) = True
      Case Key.R : Keys(&HD) = True
      Case Key.A : Keys(&H7) = True
      Case Key.S : Keys(&H8) = True
      Case Key.D : Keys(&H9) = True
      Case Key.F : Keys(&HE) = True
      Case Key.Z : Keys(&HA) = True
      Case Key.X : Keys(&H0) = True
      Case Key.C : Keys(&HB) = True
      Case Key.V : Keys(&HF) = True
      Case Else
    End Select
  End Sub

  Public Sub ReleaseKey(key As Key)
    Select Case key
      Case Key.K1 : Keys(&H1) = False
      Case Key.K2 : Keys(&H2) = False
      Case Key.K3 : Keys(&H3) = False
      Case Key.K4 : Keys(&HC) = False
      Case Key.Q : Keys(&H4) = False
      Case Key.W : Keys(&H5) = False
      Case Key.E : Keys(&H6) = False
      Case Key.R : Keys(&HD) = False
      Case Key.A : Keys(&H7) = False
      Case Key.S : Keys(&H8) = False
      Case Key.D : Keys(&H9) = False
      Case Key.F : Keys(&HE) = False
      Case Key.Z : Keys(&HA) = False
      Case Key.X : Keys(&H0) = False
      Case Key.C : Keys(&HB) = False
      Case Key.V : Keys(&HF) = False
      Case Else
    End Select
  End Sub

  Public Sub EmulateCycle()

    Dim b1 = m_ram(m_pc)
    Dim nn = m_ram(m_pc + 1) ' b2

    Dim n1 = m_ram(m_pc) \ 16
    Dim n2 = m_ram(m_pc) And &HF
    Dim n3 = m_ram(m_pc + 1) \ 16
    Dim n4 = m_ram(m_pc + 1) And &HF

    Dim addr = CUShort(n2 * 256 + n3 * 16 + n4)

    Select Case n1
      Case &H0
        Select Case n3
          Case &HC ' Scroll down N lines (Schip)
            Stop
          Case &HE
            Select Case n4
              Case &H0 ' Erase the screen.
                For a = 0 To 63
                  For b = 0 To 31
                    Display(a, b) = 0
                  Next
                Next
                m_pc += CByte(2) : Repaint = True
              Case &HE ' Return from a CHIP-8 sub-routine
                m_sp -= CByte(1) : m_pc = m_stack(m_sp) : m_pc += CByte(2)
              Case Else
                Stop
            End Select
          Case &HF
            Select Case n4
              Case &HB ' Scroll 4 pixels right (Schip)
                Stop
              Case &HC ' Scroll 4 pixels left (Schip)
                Stop
              Case &HD ' Quit the emulator
                Stop
              Case &HE ' Set CHIP-8 graphics mode (Schip)
                Stop
              Case &HF ' Set SCHIP graphics mode (Schip)
                Stop
              Case Else
                Stop
            End Select
          Case Else
            Stop
        End Select
      Case &H1 ' Jump to NNN
        m_pc = addr
      Case &H2 ' Call CHIP-8 sub-routine at NNN (16 successive calls max)
        m_stack(m_sp) = m_pc
        m_sp += CByte(1)
        m_pc = addr
      Case &H3 ' Skip next instruction if VX = KK
        m_pc += If(Registers(n2) = nn, CByte(4), CByte(2))
      Case &H4 ' Skip next instruction if VX <> KK
        m_pc += If(Not Registers(n2) = nn, CByte(4), CByte(2))
      Case &H5 ' Skip next instruction if VX = VY
        m_pc += If(Registers(n2) = Registers(n3), CByte(4), CByte(2))
      Case &H6 ' VX = KK
        Registers(n2) = nn : m_pc += CByte(2)
      Case &H7 ' VX = VX + KK
        Registers(n2) = CByte((CInt(Registers(n2)) + nn) And &HFF) : m_pc += CByte(2)
      Case &H8
        Select Case n4
          Case &H0 ' VX = VY
            Registers(n2) = Registers(n3) : m_pc += CByte(2)
          Case &H1 ' VX = VX OR VY
            Registers(n2) = Registers(n2) Or Registers(n3) : Registers(vF) = ZERO : m_pc += CByte(2)
          Case &H2 ' VX = VX AND VY
            Registers(n2) = Registers(n2) And Registers(n3) : Registers(vF) = ZERO : m_pc += CByte(2)
          Case &H3 ' VX = VX XOR VY
            Registers(n2) = Registers(n2) Xor Registers(n3) : Registers(vF) = ZERO : m_pc += CByte(2)
          Case &H4 ' VX = VX + VY, VF = carry
            Dim tmp = CInt(Registers(n2)) + Registers(n3)
            Registers(n2) = CByte(tmp And &HFF)
            Registers(vF) = If(tmp > &HFF, ONE, ZERO)
            m_pc += CByte(2)
          Case &H5 ' VX = VX - VY, VF = not borrow
            Dim flag = Registers(n2) >= Registers(n3)
            Dim tmp = CInt(Registers(n2)) - Registers(n3)
            Registers(n2) = CByte(tmp And &HFF)
            Registers(vF) = If(flag, ONE, ZERO)
            m_pc += CByte(2)
          Case &H6 ' VX = VX SHR 1 (VX=VX/2), VF = carry
            Dim flag = (Registers(n2) And 1) > 0
            Registers(n2) = Registers(n3) >> 1
            'Registers(n2) = Registers(n2) >> 1
            Registers(vF) = If(flag, ONE, ZERO)
            m_pc += CByte(2)
          Case &H7 ' VX = VY - VX, VF = not borrow
            Dim flag = Registers(n3) >= Registers(n2)
            Dim tmp = CInt(Registers(n3)) - Registers(n2)
            Registers(n2) = CByte(tmp And &HFF)
            Registers(vF) = If(flag, ONE, ZERO)
            m_pc += CByte(2)
          Case &HE ' VX = VX SHL 1 (VX=VX*2), VF = carry
            Dim flag = (Registers(n3) And &H80) > 0
            Dim tmp = (Registers(n3) << 1)
            Registers(n2) = CByte(tmp And &HFF)
            Registers(vF) = If(flag, ONE, ZERO)
            m_pc += CByte(2)
          Case Else
            Stop
        End Select
      Case &H9 ' Skip next instruction if VX != VY
        m_pc += If(Not Registers(n2) = Registers(n3), CByte(4), CByte(2))
      Case &HA ' I = NNN
        m_i = addr : m_pc += CByte(2)
      Case &HB ' Jump to NNN + V0
        m_pc = addr + Registers(0)
      Case &HC ' VX = Random number and KK
        Dim random = CByte(m_rng.Next(0, 255))
        Registers(n2) = nn And random : m_pc += CByte(2)
      Case &HD ' Draws a sprite at (VX,VY) starting at M(I). VF = collission. If N=0, draws the 16x16 sprite, otherwise an 8xN sprite.
        Dim nibble = n4
        Registers(vF) = 0
        For ypos = 0 To nibble - 1
          Dim memoryY = (Registers(n3) + ypos) Mod 64
          Dim line = m_ram(m_i + ypos)
          For xpos = 0 To 7
            Dim memoryX = (Registers(n2) + xpos) Mod 64
            Dim active = ((line >> (7 - xpos)) And 1) <> 0
            'Dim active = (line And (1 << (7 - xpos))) <> 0
            If active Then
              Display(memoryX Mod 64, memoryY Mod 32) = Display(memoryX Mod 64, memoryY Mod 32) Xor 1
              If Display(memoryX Mod 64, memoryY Mod 32) = 0 Then
                Registers(vF) = 1
              End If
            End If
          Next
        Next
        Repaint = True
        m_pc += CByte(2)
      Case &HE
        Select Case n3
          Case &H9 ' Skip next instruction if key VX pressed... ? 9E?
            m_pc += If(Keys(Registers(n2)), CByte(4), CByte(2))
          Case &HA ' Skip next instruction if key VX not pressed... ? A1?
            m_pc += If(Not Keys(Registers(n2)), CByte(4), CByte(2))
          Case Else
            Stop
        End Select
      Case &HF
        Select Case n3
          Case &H0
            Select Case n4
              Case &H7 ' VX = Delay timer
                Registers(n2) = DT : m_pc += CByte(2)
              Case &HA ' Waits a keypress and stores it in VX
                For index = 0 To 15
                  If Keys(index) Then
                    Registers(n2) = CByte(index)
                    m_pc += CByte(2)
                    Exit For
                  End If
                Next
              Case Else
                Stop
            End Select
          Case &H1
            Select Case n4
              Case &H5 ' Delay timer = VX
                DT = Registers(n2) : m_pc += CByte(2)
              Case &H8 ' Sound timer = VX
                ST = Registers(n2) : m_pc += CByte(2)
              Case &HE ' i = i + VX
                m_i += Registers(n2) : m_pc += CByte(2)
              Case Else
                Stop
            End Select
          Case &H2 ' I points to the 4x5 font sprite of hex char in VX
            Select Case n4
              Case &H9
                Select Case Registers(n2)
                  Case 0 : m_i = 0
                  Case 1 : m_i = 5
                  Case 2 : m_i = 10
                  Case 3 : m_i = 15
                  Case 4 : m_i = 20
                  Case 5 : m_i = 25
                  Case 6 : m_i = 30
                  Case 7 : m_i = 35
                  Case 8 : m_i = 40
                  Case 9 : m_i = 45
                  Case &HA : m_i = 50
                  Case &HB : m_i = 55
                  Case &HC : m_i = 60
                  Case &HD : m_i = 65
                  Case &HE : m_i = 70
                  Case &HF : m_i = 75
                End Select
                m_pc += CByte(2)
              Case Else
                Stop
            End Select
          Case &H3 ' Store BCD representation of VX in M(I)...M(I+2)
            Select Case n4
              Case &H3
                m_ram(m_i) = Registers(n2) \ CByte(100)
                m_ram(m_i + 1) = (Registers(n2) \ CByte(10)) Mod CByte(10)
                m_ram(m_i + 2) = (Registers(n2) Mod CByte(100)) Mod CByte(10)
                m_pc += CByte(2)
              Case Else
                Stop
            End Select
          Case &H5 ' Save V0...VX in memory starting at M(I)
            Select Case n4
              Case &H5
                For a = m_i To m_i + n2
                  m_ram(a) = Registers(a - m_i)
                Next
                m_pc += CByte(2)
              Case Else
                Stop
            End Select
          Case &H6 ' Load V0...VX from memory starting at M(I)
            Select Case n4
              Case &H5
                For a = m_i To m_i + n2
                  Registers(a - m_i) = m_ram(a)
                Next
                m_pc += CByte(2)
              Case Else
                Stop
            End Select
          Case &H7 ' Save V0...VX (X<8) in the HP48 flats (Schip)
            Stop
          Case &H8 ' Load V0...VX (X<8) from the HP48 flags (Schip)
            Stop
          Case Else
            Stop
        End Select
      Case Else
        Stop
    End Select

  End Sub

  Public Sub New(rom As String)
    If rom = Nothing OrElse rom = String.Empty Then
      Throw New Exception("No ROM loaded!")
    End If
    m_pc = &H200
    m_i = 0
    m_sp = 0
    ST = 0
    DT = 0
    For a = 0 To 63
      For b = 0 To 31
        Display(a, b) = 0
      Next
    Next
    Repaint = True
    For a = 0 To &HF
      m_stack(a) = 0
      Registers(a) = 0
      Keys(a) = False
    Next
    Dim characters As Byte() = {&HF0, &H90, &H90, &H90, &HF0,
                                &H20, &H60, &H20, &H20, &H70,
                                &HF0, &H10, &HF0, &H80, &HF0,
                                &HF0, &H10, &HF0, &H10, &HF0,
                                &H90, &H90, &HF0, &H10, &H10,
                                &HF0, &H80, &HF0, &H10, &HF0,
                                &HF0, &H80, &HF0, &H90, &HF0,
                                &HF0, &H10, &H20, &H40, &H40,
                                &HF0, &H90, &HF0, &H90, &HF0,
                                &HF0, &H90, &HF0, &H10, &HF0,
                                &HF0, &H90, &HF0, &H90, &H90,
                                &HE0, &H90, &HE0, &H90, &HE0,
                                &HF0, &H80, &H80, &H80, &HF0,
                                &HE0, &H90, &H90, &H90, &HE0,
                                &HF0, &H80, &HF0, &H80, &HF0,
                                &HF0, &H80, &HF0, &H80, &H80}
    For a = 0 To 79
      m_ram(a) = characters(a)
    Next
    m_rom = rom
    Using fs As New FileStream(m_rom, FileMode.Open, FileAccess.Read, FileShare.Read)
      fs.Seek(0, SeekOrigin.Begin)
      For a = 0 To fs.Length - 1
        m_ram(&H200 + CInt(a)) = CByte(fs.ReadByte)
      Next
    End Using
  End Sub

  Private Function OpcodeError(ByVal opcode As String) As String
    Return $"Invalid opcode: {opcode}
PC: {m_pc}
SP: {m_sp}
Stack({m_sp}): {m_stack(m_sp)}
I: {m_i}
V(0): {Registers(0)}
V(1): {Registers(1)}
V(2): {Registers(2)}
V(3): {Registers(3)}
V(4): {Registers(4)}
V(5): {Registers(5)}
V(6): {Registers(6)}
V(7): {Registers(7)}
V(8): {Registers(8)}
V(9): {Registers(9)}
V(A): {Registers(&HA)}
V(B): {Registers(&HB)}
V(C): {Registers(&HC)}
V(D): {Registers(&HD)}
V(E): {Registers(&HE)}
V(F): {Registers(vF)}"
  End Function

End Class