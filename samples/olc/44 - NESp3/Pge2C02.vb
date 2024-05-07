Imports VbPixelGameEngine

Public Class Pge2C02

  'Private m_name(1, 1023) As Byte
  'Private m_pattern(1, 4095) As Byte
  'Private m_palette(31) As Byte

  Private ReadOnly m_screen(63) As Pixel
  Private ReadOnly m_sprScreen As Sprite
  Private ReadOnly m_sprNameTable(1) As Sprite
  Private ReadOnly m_sprPatternTable(1) As Sprite

  Public m_frameComplete As Boolean = False
  Private m_scanline As Short = 0
  Private m_cycle As Short = 0

  Private m_cart As Cartridge

  Public Sub New()

    m_screen(&H0) = New Pixel(84, 84, 84)
    m_screen(&H1) = New Pixel(0, 30, 116)
    m_screen(&H2) = New Pixel(8, 16, 144)
    m_screen(&H3) = New Pixel(48, 0, 136)
    m_screen(&H4) = New Pixel(68, 0, 100)
    m_screen(&H5) = New Pixel(92, 0, 48)
    m_screen(&H6) = New Pixel(84, 4, 0)
    m_screen(&H7) = New Pixel(60, 24, 0)
    m_screen(&H8) = New Pixel(32, 42, 0)
    m_screen(&H9) = New Pixel(8, 58, 0)
    m_screen(&HA) = New Pixel(0, 64, 0)
    m_screen(&HB) = New Pixel(0, 60, 0)
    m_screen(&HC) = New Pixel(0, 50, 60)
    m_screen(&HD) = New Pixel(0, 0, 0)
    m_screen(&HE) = New Pixel(0, 0, 0)
    m_screen(&HF) = New Pixel(0, 0, 0)

    m_screen(&H10) = New Pixel(152, 150, 152)
    m_screen(&H11) = New Pixel(8, 76, 196)
    m_screen(&H12) = New Pixel(48, 50, 236)
    m_screen(&H13) = New Pixel(92, 30, 228)
    m_screen(&H14) = New Pixel(136, 20, 176)
    m_screen(&H15) = New Pixel(160, 20, 100)
    m_screen(&H16) = New Pixel(152, 34, 32)
    m_screen(&H17) = New Pixel(120, 60, 0)
    m_screen(&H18) = New Pixel(84, 90, 0)
    m_screen(&H19) = New Pixel(40, 114, 0)
    m_screen(&H1A) = New Pixel(8, 124, 0)
    m_screen(&H1B) = New Pixel(0, 118, 40)
    m_screen(&H1C) = New Pixel(0, 102, 120)
    m_screen(&H1D) = New Pixel(0, 0, 0)
    m_screen(&H1E) = New Pixel(0, 0, 0)
    m_screen(&H1F) = New Pixel(0, 0, 0)

    m_screen(&H20) = New Pixel(236, 238, 236)
    m_screen(&H21) = New Pixel(76, 154, 236)
    m_screen(&H22) = New Pixel(120, 124, 236)
    m_screen(&H23) = New Pixel(176, 98, 236)
    m_screen(&H24) = New Pixel(228, 84, 236)
    m_screen(&H25) = New Pixel(236, 88, 180)
    m_screen(&H26) = New Pixel(236, 106, 100)
    m_screen(&H27) = New Pixel(212, 136, 32)
    m_screen(&H28) = New Pixel(160, 170, 0)
    m_screen(&H29) = New Pixel(116, 196, 0)
    m_screen(&H2A) = New Pixel(76, 208, 32)
    m_screen(&H2B) = New Pixel(56, 204, 108)
    m_screen(&H2C) = New Pixel(56, 180, 204)
    m_screen(&H2D) = New Pixel(60, 60, 60)
    m_screen(&H2E) = New Pixel(0, 0, 0)
    m_screen(&H2F) = New Pixel(0, 0, 0)

    m_screen(&H30) = New Pixel(236, 238, 236)
    m_screen(&H31) = New Pixel(168, 204, 236)
    m_screen(&H32) = New Pixel(188, 188, 236)
    m_screen(&H33) = New Pixel(212, 178, 236)
    m_screen(&H34) = New Pixel(236, 174, 236)
    m_screen(&H35) = New Pixel(236, 174, 212)
    m_screen(&H36) = New Pixel(236, 180, 176)
    m_screen(&H37) = New Pixel(228, 196, 144)
    m_screen(&H38) = New Pixel(204, 210, 120)
    m_screen(&H39) = New Pixel(180, 222, 120)
    m_screen(&H3A) = New Pixel(168, 226, 144)
    m_screen(&H3B) = New Pixel(152, 226, 180)
    m_screen(&H3C) = New Pixel(160, 214, 228)
    m_screen(&H3D) = New Pixel(160, 162, 160)
    m_screen(&H3E) = New Pixel(0, 0, 0)
    m_screen(&H3F) = New Pixel(0, 0, 0)

    m_sprScreen = New Sprite(256, 240)
    m_sprNameTable(0) = New Sprite(256, 240)
    m_sprNameTable(1) = New Sprite(256, 240)
    m_sprPatternTable(0) = New Sprite(128, 128)
    m_sprPatternTable(1) = New Sprite(128, 128)

  End Sub

  Public Function GetScreen() As Sprite
    Return m_sprScreen
  End Function

  Public Function GetNameTable(i As Byte) As Sprite
    Return m_sprNameTable(i)
  End Function

  Public Function GetPatternTable(i As Byte) As Sprite
    Return m_sprPatternTable(i)
  End Function

  Public Shared Function CpuRead(addr As UShort, Optional [readOnly] As Boolean = False) As Byte

    If [readOnly] Then
    End If

    Dim data As Byte = &H0

    Select Case addr
      Case &H0 ' Control
        ' ...
      Case &H1 ' Mask
        ' ...
      Case &H2 ' Status
        ' ...
      Case &H3 ' OAM Address
        ' ...
      Case &H4 ' OAM Data
        ' ...
      Case &H5 ' Scroll
        ' ...
      Case &H6 ' PPU Address
        ' ...
      Case &H7 ' PPU Data
        ' ...
    End Select

    Return data

  End Function

  Public Shared Sub CpuWrite(addr As UShort, data As Byte)

    If data <> 0 Then
    End If

    Select Case addr
      Case &H0 ' Control
        ' ...
      Case &H1 ' Mask
        ' ...
      Case &H2 ' Status
        ' ...
      Case &H3 ' OAM Address
        ' ...
      Case &H4 ' OAM Data
        ' ...
      Case &H5 ' Scroll
        ' ...
      Case &H6 ' PPU Address
        ' ...
      Case &H7 ' PPU Data
        ' ...
    End Select

  End Sub

  Public Function PpuRead(addr As UShort, Optional [readOnly] As Boolean = False) As Byte

    If [readOnly] Then
    End If

    Dim data As Byte = &H0
    addr = addr And &H3FFFUS

    If m_cart.PpuRead(addr, data) Then

    End If

    Return data

  End Function

  Public Sub PpuWrite(addr As UShort, data As Byte)

    addr = addr And &H3FFFUS

    If m_cart.PpuWrite(addr, data) Then

    End If

  End Sub

  Public Sub ConnectCartridge(cartridge As Cartridge)
    m_cart = cartridge
  End Sub

  Public Sub Clock()

    ' Fake some noise for now
    m_sprScreen.SetPixel(m_cycle - 1, m_scanline, m_screen(If((Rnd() >= 0.5), &H3F, &H30)))

    ' Advance renderer - it never stops, it's relentless
    m_cycle += 1S
    If m_cycle >= 341 Then
      m_cycle = 0
      m_scanline += 1S
      If m_scanline >= 261 Then
        m_scanline = -1
        m_frameComplete = True
      End If
    End If

  End Sub

End Class