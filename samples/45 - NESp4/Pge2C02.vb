Imports System.Collections.Specialized
Imports System.Runtime.InteropServices
Imports VbPixelGameEngine

Public Class Pge2C02

  Private ReadOnly m_tblName(1, 1023) As Byte
  Private ReadOnly m_tblPattern(1, 4095) As Byte
  Private ReadOnly m_tblPalette(31) As Byte

  Private ReadOnly m_palScreen(63) As Pixel
  Private ReadOnly m_sprScreen As Sprite
  Private ReadOnly m_sprNameTable(1) As Sprite
  Private ReadOnly m_sprPatternTable(1) As Sprite

  Public m_frameComplete As Boolean = False

  Public Class StatusBits

    Public Property Reg As Byte

    Public Property SpriteOverflow As Boolean
      Get
        Return ((Reg And &H20) >> 5) = 1
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H20)
        Else
          Reg = CByte(Reg And Not &H20)
        End If
      End Set
    End Property

    Public Property SpriteZeroHit As Boolean
      Get
        Return ((Reg And &H40) >> 6) = 1
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H40)
        Else
          Reg = CByte(Reg And Not &H40)
        End If
      End Set
    End Property

    Public Property VerticalBlank As Boolean
      Get
        Return ((Reg And &H80) >> 7) = 1
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H80)
        Else
          Reg = CByte(Reg And Not &H80)
        End If
      End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(reg As Byte)
      Me.Reg = reg
    End Sub

  End Class

  Private ReadOnly m_status As New StatusBits()

  Public Class MaskBits

    Public Property Reg As Byte

    Public Property Grayscale() As Boolean
      Get
        Return (Reg And &H1) = &H1
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H1)
        Else
          Reg = CByte(Reg And Not &H1)
        End If
      End Set
    End Property

    Public Property RenderBackgroundLeft() As Boolean
      Get
        Return (Reg And &H2) = &H2
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H2)
        Else
          Reg = CByte(Reg And Not &H2)
        End If
      End Set
    End Property

    Public Property RenderSpritesLeft() As Boolean
      Get
        Return (Reg And &H4) = &H4
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H4)
        Else
          Reg = CByte(Reg And Not &H4)
        End If
      End Set
    End Property

    Public Property RenderBackground() As Boolean
      Get
        Return (Reg And &H8) = &H8
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H8)
        Else
          Reg = CByte(Reg And Not &H8)
        End If
      End Set
    End Property

    Public Property RenderSprites() As Boolean
      Get
        Return (Reg And &H10) = &H10
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H10)
        Else
          Reg = CByte(Reg And Not &H10)
        End If
      End Set
    End Property

    Public Property EnhanceRed() As Boolean
      Get
        Return (Reg And &H20) = &H20
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H20)
        Else
          Reg = CByte(Reg And Not &H20)
        End If
      End Set
    End Property

    Public Property EnhanceGreen() As Boolean
      Get
        Return (Reg And &H40) = &H40
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H40)
        Else
          Reg = CByte(Reg And Not &H40)
        End If
      End Set
    End Property

    Public Property EnhanceBlue() As Boolean
      Get
        Return (Reg And &H80) = &H80
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H80)
        Else
          Reg = CByte(Reg And Not &H80)
        End If
      End Set
    End Property

  End Class

  Private ReadOnly m_mask As New MaskBits()

  Public Structure PpuCtrl

    Public Property Reg As Byte

    Public Property NameTableX As Boolean
      Get
        Return (Reg And &H1) = &H1
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H1)
        Else
          Reg = CByte(Reg And &HFE)
        End If
      End Set
    End Property

    Public Property NameTableY As Boolean
      Get
        Return (Reg And &H2) = &H2
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H2)
        Else
          Reg = CByte(Reg And &HFD)
        End If
      End Set
    End Property

    Public Property IncrementMode As Boolean
      Get
        Return (Reg And &H4) = &H4
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H4)
        Else
          Reg = CByte(Reg And &HB)
        End If
      End Set
    End Property

    Public Property PatternSprite As Boolean
      Get
        Return (Reg And &H8) = &H8
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H8)
        Else
          Reg = CByte(Reg And &HF7)
        End If
      End Set
    End Property

    Public Property PatternBackground As Boolean
      Get
        Return (Reg And &H10) = &H10
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H10)
        Else
          Reg = CByte(Reg And &HEF)
        End If
      End Set
    End Property

    Public Property SpriteSize As Boolean
      Get
        Return (Reg And &H20) = &H20
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H20)
        Else
          Reg = CByte(Reg And &HDF)
        End If
      End Set
    End Property

    Public Property SlaveMode As Boolean ' unused
      Get
        Return (Reg And &H40) = &H40
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H40)
        Else
          Reg = CByte(Reg And &HB)
        End If
      End Set
    End Property

    Public Property EnableNmi As Boolean
      Get
        Return (Reg And &H80) = &H80
      End Get
      Set(value As Boolean)
        If value Then
          Reg = CByte(Reg Or &H80)
        Else
          Reg = CByte(Reg And &H7F)
        End If
      End Set
    End Property

  End Structure

  Private m_control As New PpuCtrl()

  Public Class LoopyRegister

    Public Property Reg As UShort = &H0

    Public Property CoarseX As UShort
      Get
        Return (Reg And &HFUS)
      End Get
      Set(value As UShort)
        Reg = (Reg And &HFFE0US) Or (value And &HFUS)
      End Set
    End Property

    Public Property CoarseY As UShort
      Get
        Return ((Reg >> 5) And &HFUS)
      End Get
      Set(value As UShort)
        Reg = (Reg And &HFC1FUS) Or ((value And &HFUS) << 5)
      End Set
    End Property

    Public Property NameTableX As UShort
      Get
        Return ((Reg >> 10) And &H1US)
      End Get
      Set(value As UShort)
        Reg = (Reg And &HFBFFUS) Or ((value And &H1US) << 10)
      End Set
    End Property

    Public Property NameTableY As UShort
      Get
        Return ((Reg >> 11) And &H1US)
      End Get
      Set(value As UShort)
        Reg = (Reg And &HFBFFUS) Or ((value And &H1US) << 11)
      End Set
    End Property

    Public Property FineY As UShort
      Get
        Return ((Reg >> 12) And &H7US)
      End Get
      Set(value As UShort)
        Reg = (Reg And &H8FFFUS) Or ((value And &H7US) << 12)
      End Set
    End Property

    Public Property Unused As UShort
      Get
        Return ((Reg >> 15) And &H1US)
      End Get
      Set(value As UShort)
        Reg = (Reg And &H7FFFUS) Or ((value And &H1US) << 15)
      End Set
    End Property

  End Class

  Private m_vramAddr As New LoopyRegister() ' Active "pointer" address into nametable to extract background tile info
  Private ReadOnly m_tramAddr As New LoopyRegister() ' Temporary store of information to be "transferred" into "pointer" at various times

  ' Pixel offset horizontally
  Private m_fineX As Byte = &H0

  ' Internal communications
  Private m_addressLatch As Byte = &H0
  Private m_ppuDataBuffer As Byte = &H0

  ' Pixel "dot" position information
  Private m_scanline As Short = 0
  Private m_cycle As Short = 0

  ' Background rendering
  Private m_bgNextTileId As Byte = &H0
  Private m_bgNextTileAttrib As Byte = &H0
  Private m_bgNextTileLsb As Byte = &H0
  Private m_bgNextTileMsb As Byte = &H0
  Private m_bgShifterPatternLo As UShort = &H0
  Private m_bgShifterPatternHi As UShort = &H0
  Private m_bgShifterAttribLo As UShort = &H0
  Private m_bgShifterAttribHi As UShort = &H0

  ' The Cartridge or "GamePak"
  Private m_cart As Cartridge

  ' Interface
  Public m_nmi As Boolean = False

  Public Sub New()

    m_palScreen(&H0) = New Pixel(84, 84, 84)
    m_palScreen(&H1) = New Pixel(0, 30, 116)
    m_palScreen(&H2) = New Pixel(8, 16, 144)
    m_palScreen(&H3) = New Pixel(48, 0, 136)
    m_palScreen(&H4) = New Pixel(68, 0, 100)
    m_palScreen(&H5) = New Pixel(92, 0, 48)
    m_palScreen(&H6) = New Pixel(84, 4, 0)
    m_palScreen(&H7) = New Pixel(60, 24, 0)
    m_palScreen(&H8) = New Pixel(32, 42, 0)
    m_palScreen(&H9) = New Pixel(8, 58, 0)
    m_palScreen(&HA) = New Pixel(0, 64, 0)
    m_palScreen(&HB) = New Pixel(0, 60, 0)
    m_palScreen(&HC) = New Pixel(0, 50, 60)
    m_palScreen(&HD) = New Pixel(0, 0, 0)
    m_palScreen(&HE) = New Pixel(0, 0, 0)
    m_palScreen(&HF) = New Pixel(0, 0, 0)

    m_palScreen(&H10) = New Pixel(152, 150, 152)
    m_palScreen(&H11) = New Pixel(8, 76, 196)
    m_palScreen(&H12) = New Pixel(48, 50, 236)
    m_palScreen(&H13) = New Pixel(92, 30, 228)
    m_palScreen(&H14) = New Pixel(136, 20, 176)
    m_palScreen(&H15) = New Pixel(160, 20, 100)
    m_palScreen(&H16) = New Pixel(152, 34, 32)
    m_palScreen(&H17) = New Pixel(120, 60, 0)
    m_palScreen(&H18) = New Pixel(84, 90, 0)
    m_palScreen(&H19) = New Pixel(40, 114, 0)
    m_palScreen(&H1A) = New Pixel(8, 124, 0)
    m_palScreen(&H1B) = New Pixel(0, 118, 40)
    m_palScreen(&H1C) = New Pixel(0, 102, 120)
    m_palScreen(&H1D) = New Pixel(0, 0, 0)
    m_palScreen(&H1E) = New Pixel(0, 0, 0)
    m_palScreen(&H1F) = New Pixel(0, 0, 0)

    m_palScreen(&H20) = New Pixel(236, 238, 236)
    m_palScreen(&H21) = New Pixel(76, 154, 236)
    m_palScreen(&H22) = New Pixel(120, 124, 236)
    m_palScreen(&H23) = New Pixel(176, 98, 236)
    m_palScreen(&H24) = New Pixel(228, 84, 236)
    m_palScreen(&H25) = New Pixel(236, 88, 180)
    m_palScreen(&H26) = New Pixel(236, 106, 100)
    m_palScreen(&H27) = New Pixel(212, 136, 32)
    m_palScreen(&H28) = New Pixel(160, 170, 0)
    m_palScreen(&H29) = New Pixel(116, 196, 0)
    m_palScreen(&H2A) = New Pixel(76, 208, 32)
    m_palScreen(&H2B) = New Pixel(56, 204, 108)
    m_palScreen(&H2C) = New Pixel(56, 180, 204)
    m_palScreen(&H2D) = New Pixel(60, 60, 60)
    m_palScreen(&H2E) = New Pixel(0, 0, 0)
    m_palScreen(&H2F) = New Pixel(0, 0, 0)

    m_palScreen(&H30) = New Pixel(236, 238, 236)
    m_palScreen(&H31) = New Pixel(168, 204, 236)
    m_palScreen(&H32) = New Pixel(188, 188, 236)
    m_palScreen(&H33) = New Pixel(212, 178, 236)
    m_palScreen(&H34) = New Pixel(236, 174, 236)
    m_palScreen(&H35) = New Pixel(236, 174, 212)
    m_palScreen(&H36) = New Pixel(236, 180, 176)
    m_palScreen(&H37) = New Pixel(228, 196, 144)
    m_palScreen(&H38) = New Pixel(204, 210, 120)
    m_palScreen(&H39) = New Pixel(180, 222, 120)
    m_palScreen(&H3A) = New Pixel(168, 226, 144)
    m_palScreen(&H3B) = New Pixel(152, 226, 180)
    m_palScreen(&H3C) = New Pixel(160, 214, 228)
    m_palScreen(&H3D) = New Pixel(160, 162, 160)
    m_palScreen(&H3E) = New Pixel(0, 0, 0)
    m_palScreen(&H3F) = New Pixel(0, 0, 0)

    m_sprScreen = New Sprite(256, 240)
    m_sprNameTable(0) = New Sprite(256, 240)
    m_sprNameTable(1) = New Sprite(256, 240)
    m_sprPatternTable(0) = New Sprite(128, 128)
    m_sprPatternTable(1) = New Sprite(128, 128)

  End Sub

  Public Function GetScreen() As Sprite
    ' Simply returns the current sprite holding the rendered screen
    Return m_sprScreen
  End Function

  Public Function GetPatternTable(i As Byte, palette As Integer) As Sprite

    ' This function draw the CHR ROM for a given pattern table into
    ' an olc:Sprite, using a specified palette. Pattern tables consist
    ' of 16x16 "tiles or characters". It Is independent of the running
    ' emulation And using it does Not change the systems state, though
    ' it gets all the data it needs from the live system. Consequently,
    ' if the game has Not yet established palettes Or mapped to relevant
    ' CHR ROM banks, the sprite may look empty. This approach permits a 
    ' "live" extraction of the pattern table exactly how the NES, And 
    ' ultimately the player would see it.
    '
    ' A tile consists of 8x8 pixels. On the NES, pixels are 2 bits, which
    ' gives an index into 4 different colours of a specific palette. There
    ' are 8 palettes to choose from. Colour "0" in each palette Is effectively
    ' considered transparent, as those locations in memory "mirror" the global
    ' background colour being used. This mechanics of this are shown in 
    ' detail in ppuRead() & ppuWrite()
    '
    ' Characters on NES
    ' ~~~~~~~~~~~~~~~~~
    ' The NES stores characters using 2-bit pixels. These are Not stored sequentially
    ' but in singular bit planes. For example:
    '
    ' 2-Bit Pixels       LSB Bit Plane     MSB Bit Plane
    ' 0 0 0 0 0 0 0 0	   0 0 0 0 0 0 0 0   0 0 0 0 0 0 0 0
    ' 0 1 1 0 0 1 1 0	   0 1 1 0 0 1 1 0   0 0 0 0 0 0 0 0
    ' 0 1 2 0 0 2 1 0	   0 1 1 0 0 1 1 0   0 0 1 0 0 1 0 0
    ' 0 0 0 0 0 0 0 0 =  0 0 0 0 0 0 0 0 + 0 0 0 0 0 0 0 0
    ' 0 1 1 0 0 1 1 0	   0 1 1 0 0 1 1 0   0 0 0 0 0 0 0 0
    ' 0 0 1 1 1 1 0 0	   0 0 1 1 1 1 0 0   0 0 0 0 0 0 0 0
    ' 0 0 0 2 2 0 0 0	   0 0 0 1 1 0 0 0   0 0 0 1 1 0 0 0
    ' 0 0 0 0 0 0 0 0	   0 0 0 0 0 0 0 0   0 0 0 0 0 0 0 0
    '
    ' The planes are stored as 8 bytes of LSB, followed by 8 bytes of MSB

    ' Loop through all 16x16 tiles
    For nTileY = 0 To 15
      For nTileX = 0 To 15

        ' Convert the 2D title coordinate into a 1D offset into the pattern table memory
        Dim nOffset = nTileY * 256US + nTileX * 16US

        ' Now loop through 8 rows of 8 pixels
        For row = 0 To 7

          ' For each row, we need to read both bit planes of the character
          ' in order to extract the least significant And most significant 
          ' bits of the 2 bit pixel value. in the CHR ROM, each character
          ' Is stored as 64 bits of lsb, followed by 64 bits of msb. This
          ' conveniently means that two corresponding rows are always 8
          ' bytes apart in memory.
          Dim tileLsb = PpuRead(CUShort(i * &H1000US + nOffset + row + &H0US))
          Dim tileMsb = PpuRead(CUShort(i * &H1000US + nOffset + row + &H8US))

          ' Now we have a single row of the two bit planes for the character
          ' we need to iterate through the 8-bit words, combining them to give
          ' us the final pixel index
          For col = 0 To 7

            ' We can get the index value by simply adding the bits together
            ' but we're only interested in the lsb of the row words because...
            Dim pixel = (tileLsb And &H1) + (tileMsb And &H1)

            ' ...we will shift the row words 1 bit right for each column of
            ' the character
            tileLsb >>= 1 : tileMsb >>= 1

            ' Now we know the location and NES pixel value for a specific location
            ' in the pattern table, we can translate that to a screen colour, and an
            ' (x,y) location in the sprite
            m_sprPatternTable(i).SetPixel(nTileX * 8 + (7 - col), ' Because we are using the lsb of the row word first we are effectively reading the row from right to left, so we need to draw the row "backwards"
                                        nTileY * 8 + row,
                                        GetColourFromPaletteRam(palette, pixel))

          Next
        Next
      Next
    Next

    ' Finally return the updated sprite representing the pattern table
    Return m_sprPatternTable(i)

  End Function

  Public Function GetColourFromPaletteRam(palette As Integer, pixel As Integer) As Pixel

    ' This is a convenience function that takes a specified palette and pixel
    ' index and returns the appropriate screen colour.
    ' "0x3F00"       - Offset into PPU addressable range where palettes are stored
    ' "palette << 2" - Each palette is 4 bytes in size
    ' "pixel"        - Each pixel index is either 0, 1, 2 or 3
    ' "& 0x3F"       - Stops us reading beyond the bounds of the palScreen array
    Return m_palScreen(PpuRead(&H3F00US + CUShort(palette << 2) + CUShort(pixel)) And &H3F)

    ' Note: We don't access tblPalette directly here, instead we know that ppuRead()
    ' will map the address onto the separate small RAM attached to the PPU bus.

  End Function

  Public Function GetNameTable(i As Byte) As Sprite
    ' As of now unused, but a placeholder for nametable visualisation in the future
    Return m_sprNameTable(i)
  End Function

  Public Function CpuRead(addr As UShort, Optional rdonly As Boolean = False) As Byte

    Dim data As Byte = &H0

    If rdonly Then
      ' Reading from PPU registers can affect their contents
      ' so this read only option is used for examining the
      ' state of the PPU without changing its state. This is
      ' really only used in debug mode.
      Select Case addr
        Case &H0 ' Control
          data = m_control.Reg
        Case &H1 ' Mask
          data = m_mask.Reg
        Case &H2 ' Status
          data = m_status.Reg
        Case &H3 ' OAM Address
        Case &H4 ' OAM Data
        Case &H5 ' Scroll
        Case &H6 ' PPU Address
        Case &H7 ' PPU Data
      End Select

    Else

      Select Case addr

        Case &H0 ' Control - Not readable

        Case &H1 ' Mask - Not Readable

        Case &H2 ' Status

          ' Reading from the status register has the effect of resetting
          ' different parts of the circuit. Only the top three bits
          ' contain status information, however it is possible that
          ' some "noise" gets picked up on the bottom 5 bits which 
          ' represent the last PPU bus transaction. Some games "may"
          ' use this noise as valid data (even though they probably
          ' shouldn't)
          data = CByte((m_status.Reg And &HE0) Or (m_ppuDataBuffer And &H1F))

          ' Clear the vertical blanking flag
          m_status.VerticalBlank = False

          ' Reset Loopy's Address latch flag
          m_addressLatch = 0

        Case &H3 ' OAM Address

        Case &H4 ' OAM Data

        Case &H5 ' Scroll - Not Readable

        Case &H6 ' PPU Address - Not Readable

        Case &H7 ' PPU Data

          ' Reads from the NameTable ram get delayed one cycle, 
          ' so output buffer which contains the data from the 
          ' previous read request
          data = m_ppuDataBuffer
          ' then update the buffer for next time
          m_ppuDataBuffer = PpuRead(m_vramAddr.Reg)
          ' However, if the address was in the palette range, the
          ' data is not delayed, so it returns immediately
          If m_vramAddr.Reg >= &H3F00 Then data = m_ppuDataBuffer
          ' All reads from PPU data automatically increment the nametable
          ' address depending upon the mode set in the control register.
          ' If set to vertical mode, the increment is 32, so it skips
          ' one whole nametable row; in horizontal mode it just increments
          ' by 1, moving to the next column
          m_vramAddr.Reg += If(m_control.IncrementMode, 32US, 1US)

      End Select

    End If

    Return data

  End Function

  Public Sub CpuWrite(addr As UShort, data As Byte)

    Select Case addr
      Case &H0 ' Control
        m_control.Reg = data
        m_tramAddr.NameTableX = If(m_control.NameTableX, 1US, 0US)
        m_tramAddr.NameTableY = If(m_control.NameTableY, 1US, 0US)
      Case &H1 ' Mask
        m_mask.Reg = data
      Case &H2 ' Status
      Case &H3 ' OAM Address
      Case &H4 ' OAM Data
      Case &H5 ' Scroll
        If m_addressLatch = 0 Then
          ' First write to scroll register contains X offset in pixel space
          ' which we split into coarse And fine x values
          m_fineX = CByte(data And &H7)
          m_tramAddr.CoarseX = data >> 3
          m_addressLatch = 1
        Else
          ' First write to scroll register contains Y offset in pixel space
          ' which we split into coarse And fine Y values
          m_tramAddr.FineY = CByte(data And &H7)
          m_tramAddr.CoarseY = data >> 3
          m_addressLatch = 0
        End If
      Case &H6 ' PPU Address
        If m_addressLatch = 0 Then
          ' PPU address bus can be accessed by CPU via the ADDR And DATA
          ' registers. The fisrt write to this register latches the high byte
          ' of the address, the second Is the low byte. Note the writes
          ' are stored in the tram register...
          m_tramAddr.Reg = CUShort(((data And &H3F) << 8) Or (m_tramAddr.Reg And &HFF))
          m_addressLatch = 1
        Else
          ' ...when a whole address has been written, the internal vram address
          ' buffer Is updated. Writing to the PPU Is unwise during rendering
          ' as the PPU will maintam the vram address automatically whilst
          ' rendering the scanline position.
          m_tramAddr.Reg = CUShort((m_tramAddr.Reg And &HFF00) Or data)
          m_vramAddr = m_tramAddr
          m_addressLatch = 0
        End If
      Case &H7 ' PPU Data
        PpuWrite(m_vramAddr.Reg, data)
        ' All writes from PPU data automatically increment the nametable
        ' address depending upon the mode set in the control register.
        ' If set to vertical mode, the increment Is 32, so it skips
        ' one whole nametable row; in horizontal mode it just increments
        ' by 1, moving to the next column
        m_vramAddr.Reg += If(m_control.IncrementMode, 32US, 1US)
    End Select

  End Sub

  Public Function PpuRead(addr As UShort, Optional rdonly As Boolean = False) As Byte

    If rdonly Then
    End If

    Dim data As Byte = &H0
    addr = addr And &H3FFFUS

    If m_cart.PpuRead(addr, data) Then

    ElseIf addr >= &H0 AndAlso addr <= &H1FFF Then

      ' If the cartridge cant map the address, have
      ' a physical location ready here
      data = m_tblPattern((addr And &H1000US) >> 12, addr And &HFFF)

    ElseIf addr >= &H2000 AndAlso addr <= &H3EFF Then

      addr = addr And &HFFFUS
      If m_cart.m_mirror = Cartridge.Mirrors.Vertical Then
        ' Vertical
        If addr >= &H0 AndAlso addr <= &H3FF Then data = m_tblName(0, addr And &H3FF)
        If addr >= &H400 AndAlso addr <= &H7FF Then data = m_tblName(1, addr And &H3FF)
        If addr >= &H800 AndAlso addr <= &HBFF Then data = m_tblName(0, addr And &H3FF)
        If addr >= &HC00 AndAlso addr <= &HFFF Then data = m_tblName(1, addr And &H3FF)
      ElseIf m_cart.m_mirror = Cartridge.Mirrors.Horizontal Then
        ' Horizontal
        If addr >= &H0 AndAlso addr <= &H3FF Then data = m_tblName(0, addr And &H3FF)
        If addr >= &H400 AndAlso addr <= &H7FF Then data = m_tblName(0, addr And &H3FF)
        If addr >= &H800 AndAlso addr <= &HBFF Then data = m_tblName(1, addr And &H3FF)
        If addr >= &HC00 AndAlso addr <= &HFFF Then data = m_tblName(1, addr And &H3FF)
      End If

    ElseIf addr >= &H3F00 AndAlso addr <= &H3FFF Then

      addr = addr And &H1FUS
      If addr = &H10 Then addr = &H0
      If addr = &H14 Then addr = &H4
      If addr = &H18 Then addr = &H8
      If addr = &H1C Then addr = &HC
      data = CByte(m_tblPalette(addr) And (If(m_mask.Grayscale, &H30, &H3F)))

    End If

    Return data

  End Function

  Public Sub PpuWrite(addr As UShort, data As Byte)

    addr = addr And &H3FFFUS

    If m_cart.PpuWrite(addr, data) Then

    ElseIf addr >= &H0 AndAlso addr <= &H1FFF Then

      m_tblPattern((addr And &H1000US) >> 12, addr And &HFFF) = data

    ElseIf addr >= &H2000 AndAlso addr <= &H3EFF Then

      addr = addr And &HFFFUS
      If m_cart.m_mirror = Cartridge.Mirrors.Vertical Then
        ' Vertical
        If addr >= &H0 AndAlso addr <= &H3FF Then m_tblName(0, addr And &H3FF) = data
        If addr >= &H400 AndAlso addr <= &H7FF Then m_tblName(1, addr And &H3FF) = data
        If addr >= &H800 AndAlso addr <= &HBFF Then m_tblName(0, addr And &H3FF) = data
        If addr >= &HC00 AndAlso addr <= &HFFF Then m_tblName(1, addr And &H3FF) = data
      ElseIf m_cart.m_mirror = Cartridge.Mirrors.Horizontal Then
        ' Horizontal
        If addr >= &H0 AndAlso addr <= &H3FF Then m_tblName(0, addr And &H3FF) = data
        If addr >= &H400 AndAlso addr <= &H7FF Then m_tblName(0, addr And &H3FF) = data
        If addr >= &H800 AndAlso addr <= &HBFF Then m_tblName(1, addr And &H3FF) = data
        If addr >= &HC00 AndAlso addr <= &HFFF Then m_tblName(1, addr And &H3FF) = data
      End If

    ElseIf addr >= &H3F00 AndAlso addr <= &H3FFF Then

      addr = addr And &H1FUS
      If addr = &H10 Then addr = &H0
      If addr = &H14 Then addr = &H4
      If addr = &H18 Then addr = &H8
      If addr = &H1C Then addr = &HC
      m_tblPalette(addr) = data

    End If

  End Sub

  Public Sub ConnectCartridge(cartridge As Cartridge)
    m_cart = cartridge
  End Sub

  Public Sub Reset()
    m_fineX = &H0
    m_addressLatch = &H0
    m_ppuDataBuffer = &H0
    m_scanline = 0
    m_cycle = 0
    m_bgNextTileId = &H0
    m_bgNextTileAttrib = &H0
    m_bgNextTileLsb = &H0
    m_bgNextTileMsb = &H0
    m_bgShifterPatternLo = &H0
    m_bgShifterPatternHi = &H0
    m_bgShifterAttribLo = &H0
    m_bgShifterAttribHi = &H0
    m_status.Reg = &H0
    m_mask.Reg = &H0
    m_control.Reg = &H0
    m_vramAddr.Reg = &H0
    m_tramAddr.Reg = &H0
  End Sub

  Public Sub Clock()

    ' As we progress through scanlines and cycles, the PPU is effectively
    ' a state machine going through the motions of fetching background 
    ' information and sprite information, compositing them into a pixel
    ' to be output.

    ' The lambda functions (functions inside functions) contain the various
    ' actions to be performed depending upon the output of the state machine
    ' for a given scanline/cycle combination

    ' ==============================================================================
    ' Increment the background tile "pointer" one tile/column horizontally
    Dim incrementScrollX = Sub()

                             ' Note: pixel perfect scrolling horizontally is handled by the 
                             ' data shifters. Here we are operating in the spatial domain of 
                             ' tiles, 8x8 pixel blocks.

                             ' Only if rendering is enabled
                             If m_mask.RenderBackground OrElse m_mask.RenderSprites Then
                               ' A single name table is 32x30 tiles. As we increment horizontally
                               ' we may cross into a neighbouring nametable, or wrap around to
                               ' a neighbouring nametable
                               If m_vramAddr.CoarseX = 31 Then
                                 ' Leaving nametable so wrap address round
                                 m_vramAddr.CoarseX = 0
                                 ' Flip target nametable bit
                                 m_vramAddr.NameTableX = Not m_vramAddr.NameTableX
                               Else
                                 ' Staying in current nametable, so just increment
                                 m_vramAddr.CoarseX += 1US
                               End If
                             End If

                           End Sub

    ' Increment the background tile "pointer" one scanline vertically
    Dim incrementScrollY = Sub()

                             ' Incrementing vertically is more complicated. The visible nametable
                             ' is 32x30 tiles, but in memory there is enough room for 32x32 tiles.
                             ' The bottom two rows of tiles are in fact not tiles at all, they
                             ' contain the "attribute" information for the entire table. This is
                             ' information that describes which palettes are used for different 
                             ' regions of the nametable.

                             ' In addition, the NES doesnt scroll vertically in chunks of 8 pixels
                             ' i.e. the height of a tile, it can perform fine scrolling by using
                             ' the fine_y component of the register. This means an increment in Y
                             ' first adjusts the fine offset, but may need to adjust the whole
                             ' row offset, since fine_y is a value 0 to 7, and a row is 8 pixels high

                             ' Only if rendering is enabled
                             If m_mask.RenderBackground OrElse m_mask.RenderSprites Then
                               ' If possible, just increment the fine y offset
                               If m_vramAddr.FineY < 7 Then
                                 m_vramAddr.FineY += 1US
                               Else
                                 ' If we have gone beyond the height of a row, we need to
                                 ' increment the row, potentially wrapping into neighbouring
                                 ' vertical nametables. Dont forget however, the bottom two rows
                                 ' do not contain tile information. The coarse y offset is used
                                 ' to identify which row of the nametable we want, and the fine
                                 ' y offset is the specific "scanline"

                                 ' Reset fine y offset
                                 m_vramAddr.FineY = 0

                                 ' Check if we need to swap vertical nametable targets
                                 If m_vramAddr.CoarseY = 29 Then
                                   ' We do, so reset coarse y offset
                                   m_vramAddr.CoarseY = 0
                                   ' And flip the target nametable bit
                                   m_vramAddr.NameTableY = Not m_vramAddr.NameTableY
                                 ElseIf m_vramAddr.CoarseY = 31 Then
                                   ' In case the pointer is in the attribute memory, we
                                   ' just wrap around the current nametable
                                   m_vramAddr.CoarseY = 0
                                 Else
                                   ' None of the above boundary/wrapping conditions apply
                                   ' so just increment the coarse y offset
                                   m_vramAddr.CoarseY += 1US
                                 End If
                               End If
                             End If

                           End Sub

    ' ==============================================================================
    ' Transfer the temporarily stored horizontal nametable access information
    ' into the "pointer". Note that fine x scrolling is not part of the "pointer"
    ' addressing mechanism
    Dim transferAddressX = Sub()
                             ' Only if rendering is enabled
                             If m_mask.RenderBackground OrElse m_mask.RenderSprites Then
                               m_vramAddr.NameTableX = m_tramAddr.NameTableX
                               m_vramAddr.CoarseX = m_tramAddr.CoarseX
                             End If
                           End Sub

    ' ==============================================================================
    ' Transfer the temporarily stored vertical nametable access information
    ' into the "pointer". Note that fine y scrolling is part of the "pointer"
    ' addressing mechanism
    Dim transferAddressY = Sub()
                             ' Only if rendering is enabled
                             If m_mask.RenderBackground OrElse m_mask.RenderSprites Then
                               m_vramAddr.FineY = m_tramAddr.FineY
                               m_vramAddr.NameTableY = m_tramAddr.NameTableY
                               m_vramAddr.CoarseY = m_tramAddr.CoarseY
                             End If
                           End Sub

    ' ==============================================================================
    ' Prime the "in-effect" background tile shifters ready for outputting next
    ' 8 pixels in scanline.
    Dim loadBackgroundShifters = Sub()
                                   ' Each PPU update we calculate one pixel. These shifters shift 1 bit along
                                   ' feeding the pixel compositor with the binary information it needs. Its
                                   ' 16 bits wide, because the top 8 bits are the current 8 pixels being drawn
                                   ' and the bottom 8 bits are the next 8 pixels to be drawn. Naturally this means
                                   ' the required bit is always the MSB of the shifter. However, "fine x" scrolling
                                   ' plays a part in this too, whcih is seen later, so in fact we can choose
                                   ' any one of the top 8 bits.
                                   m_bgShifterPatternLo = (m_bgShifterPatternLo And &HFF00US) Or m_bgNextTileLsb
                                   m_bgShifterPatternHi = (m_bgShifterPatternHi And &HFF00US) Or m_bgNextTileMsb
                                   ' Attribute bits do not change per pixel, rather they change every 8 pixels
                                   ' but are synchronised with the pattern shifters for convenience, so here
                                   ' we take the bottom 2 bits of the attribute word which represent which 
                                   ' palette is being used for the current 8 pixels and the next 8 pixels, and 
                                   ' "inflate" them to 8 bit words.
                                   m_bgShifterAttribLo = (m_bgShifterAttribLo And &HFF00US) Or If((m_bgNextTileAttrib And &B1) = &B1, &HFFUS, &H0US)
                                   m_bgShifterAttribHi = (m_bgShifterAttribHi And &HFF00US) Or If((m_bgNextTileAttrib And &B10) = &B10, &HFFUS, &H0US)
                                 End Sub

    ' ==============================================================================
    ' Every cycle the shifters storing pattern and attribute information shift
    ' their contents by 1 bit. This is because every cycle, the output progresses
    ' by 1 pixel. This means relatively, the state of the shifter is in sync
    ' with the pixels being drawn for that 8 pixel section of the scanline.
    Dim UpdateShifters = Sub()
                           If m_mask.RenderBackground Then
                             ' Shifting background tile pattern row
                             m_bgShifterPatternLo <<= 1
                             m_bgShifterPatternHi <<= 1
                             ' Shifting palette attributes by 1
                             m_bgShifterAttribLo <<= 1
                             m_bgShifterAttribHi <<= 1
                           End If
                         End Sub

    ' All but 1 of the secanlines is visible to the user. The pre-render scanline
    ' at -1, is used to configure the "shifters" for the first visible scanline, 0.
    If m_scanline >= -1 AndAlso m_scanline < 240 Then

      If m_scanline = 0 AndAlso m_cycle = 0 Then
        ' "Odd Frame" cycle skip
        m_cycle = 1
      End If

      If m_scanline = -1 AndAlso m_cycle = 1 Then
        ' Effectively start of new frame, so clear vertical blank flag
        m_status.VerticalBlank = False
      End If

      If (m_cycle >= 2 AndAlso m_cycle < 258) OrElse (m_cycle >= 321 AndAlso m_cycle < 338) Then
        UpdateShifters()

        ' In these cycles we are collecting and working with visible data
        ' The "shifters" have been preloaded by the end of the previous
        ' scanline with the data for the start of this scanline. Once we
        ' leave the visible region, we go dormant until the shifters are
        ' preloaded for the next scanline.

        ' Fortunately, for background rendering, we go through a fairly
        ' repeatable sequence of events, every 2 clock cycles.
        Select Case (m_cycle - 1) Mod 8
          Case 0
            ' Load the current background tile pattern and attributes into the "shifter"
            loadBackgroundShifters()

            ' Fetch the next background tile ID
            ' "(vram_addr.reg And &H0FFF)" : Mask to 12 bits that are relevant
            ' "| &H2000"                 : Offset into nametable space on PPU address bus
            m_bgNextTileId = PpuRead(&H2000US Or (m_vramAddr.Reg And &HFFFUS))

            ' Explanation:
            ' The bottom 12 bits of the loopy register provide an index into
            ' the 4 nametables, regardless of nametable mirroring configuration.
            ' nametable_y(1) nametable_x(1) coarse_y(5) coarse_x(5)
            '
            ' Consider a single nametable is a 32x32 array, and we have four of them
            '   0                1
            ' 0 +----------------+----------------+
            '   |                |                |
            '   |                |                |
            '   |    (32x32)     |    (32x32)     |
            '   |                |                |
            '   |                |                |
            ' 1 +----------------+----------------+
            '   |                |                |
            '   |                |                |
            '   |    (32x32)     |    (32x32)     |
            '   |                |                |
            '   |                |                |
            '   +----------------+----------------+
            '
            ' This means there are 4096 potential locations in this array, which 
            ' just so happens to be 2^12!
          Case 2
            ' Fetch the next background tile attribute. OK, so this one is a bit
            ' more involved :P

            ' Recall that each nametable has two rows of cells that are not tile
            ' information, instead they represent the attribute information that
            ' indicates which palettes are applied to which area on the screen.
            ' Importantly (and frustratingly) there is not a 1 to 1 correspondance
            ' between background tile and palette. Two rows of tile data holds
            ' 64 attributes. Therfore we can assume that the attributes affect
            ' 8x8 zones on the screen for that nametable. Given a working resolution
            ' of 256x240, we can further assume that each zone is 32x32 pixels
            ' in screen space, or 4x4 tiles. Four system palettes are allocated
            ' to background rendering, so a palette can be specified using just
            ' 2 bits. The attribute byte therefore can specify 4 distinct palettes.
            ' Therefore we can even further assume that a single palette is
            ' applied to a 2x2 tile combination of the 4x4 tile zone. The very fact
            ' that background tiles "share" a palette locally is the reason why
            ' in some games you see distortion in the colours at screen edges.

            ' As before when choosing the tile ID, we can use the bottom 12 bits of
            ' the loopy register, but we need to make the implementation "coarser"
            ' because instead of a specific tile, we want the attribute byte for a
            ' group of 4x4 tiles, or in other words, we divide our 32x32 address
            ' by 4 to give us an equivalent 8x8 address, and we offset this address
            ' into the attribute section of the target nametable.

            ' Reconstruct the 12 bit loopy address into an offset into the
            ' attribute memory

            ' "(vram_addr.coarse_x >> 2)" : integer divide coarse x by 4,
            ' from 5 bits to 3 bits
            ' "((vram_addr.coarse_y >> 2) << 3)" : integer divide coarse y by 4,
            ' from 5 bits to 3 bits,
            ' shift to make room for coarse x

            ' Result so far: YX00 00yy yxxx

            ' All attribute memory begins at 0x03C0 within a nametable, so OR with
            ' result to select target nametable, and attribute byte offset. Finally
            ' OR with 0x2000 to offset into nametable address space on PPU bus.
            m_bgNextTileAttrib = PpuRead(&H23C0US Or (m_vramAddr.NameTableY << 11) Or (m_vramAddr.NameTableX << 10) Or ((m_vramAddr.CoarseY >> 2) << 3) Or (m_vramAddr.CoarseX >> 2))

            ' Right we've read the correct attribute byte for a specified address,
            ' but the byte itself is broken down further into the 2x2 tile groups
            ' in the 4x4 attribute zone.

            ' The attribute byte is assembled thus: BR(76) BL(54) TR(32) TL(10)
            '
            ' +----+----+ +----+----+
            ' | TL | TR | | ID | ID |
            ' +----+----+ where TL = +----+----+
            ' | BL | BR | | ID | ID |
            ' +----+----+ +----+----+
            '
            ' Since we know we can access a tile directly from the 12 bit address, we
            ' can analyse the bottom bits of the coarse coordinates to provide us with
            ' the correct offset into the 8-bit word, to yield the 2 bits we are
            ' actually interested in which specifies the palette for the 2x2 group of
            ' tiles. We know if "coarse y % 4" < 2 we are in the top half else bottom half.
            ' Likewise if "coarse x % 4" < 2 we are in the left half else right half.
            ' Ultimately we want the bottom two bits of our attribute word to be the
            ' palette selected. So shift as required...
            If (m_vramAddr.CoarseY And &H2) <> 0 Then
              m_bgNextTileAttrib >>= 4
            End If
            If (m_vramAddr.CoarseX And &H2) <> 0 Then
              m_bgNextTileAttrib >>= 2
            End If
            m_bgNextTileAttrib = m_bgNextTileAttrib And CByte(&H3)

            ' Compared to the last two, the next two are the easy ones... :P

          Case 4

            ' Fetch the next background tile LSB bit plane from the pattern memory
            ' The Tile ID has been read from the nametable. We will use this id to 
            ' index into the pattern memory to find the correct sprite (assuming
            ' the sprites lie on 8x8 pixel boundaries in that memory, which they do
            ' even though 8x16 sprites exist, as background tiles are always 8x8).
            '
            ' Since the sprites are effectively 1 bit deep, but 8 pixels wide, we 
            ' can represent a whole sprite row as a single byte, so offsetting
            ' into the pattern memory is easy. In total there is 8KB so we need a 
            ' 13 bit address.

            ' "(control.pattern_background << 12)"  : the pattern memory selector 
            '                                         from control register, either 0K
            '                                         or 4K offset
            ' "((uint16_t)bg_next_tile_id << 4)"    : the tile id multiplied by 16, as
            '                                         2 lots of 8 rows of 8 bit pixels
            ' "(vram_addr.fine_y)"                  : Offset into which row based on
            '                                         vertical scroll offset
            ' "+ 0"                                 : Mental clarity for plane offset
            ' Note: No PPU address bus offset required as it starts at 0x0000
            m_bgNextTileLsb = PpuRead(CUShort((If(m_control.PatternBackground, 1, 0) << 12) + (CUShort(m_bgNextTileId) << 4) + (m_vramAddr.FineY) + 0))

          Case 6

            ' Fetch the next background tile MSB bit plane from the pattern memory
            ' This is the same as above, but has a +8 offset to select the next bit plane
            m_bgNextTileMsb = PpuRead((If(m_control.PatternBackground, 1US, 0US) << 12) + ((CUShort(m_bgNextTileId) << 4)) + (m_vramAddr.FineY) + 8US)

          Case 7

            ' Increment the background tile "pointer" to the next tile horizontally
            ' in the nametable memory. Note this may cross nametable boundaries which
            ' is a little complex, but essential to implement scrolling
            incrementScrollX()

        End Select

      End If

      ' End of a visible scanline, so increment downwards...
      If m_cycle = 256 Then
        incrementScrollY()
      End If

      ' ...and reset the x position
      If m_cycle = 257 Then
        loadBackgroundShifters()
        transferAddressX()
      End If

      ' Superfluous reads of tile id at end of scanline
      If m_cycle = 338 Or m_cycle = 340 Then
        m_bgNextTileId = PpuRead(&H2000US Or (m_vramAddr.Reg And &HFFFUS))
      End If

      If m_scanline = -1 AndAlso m_cycle >= 280 AndAlso m_cycle < 305 Then
        ' End of vertical blank period so reset the Y address ready for rendering
        transferAddressY()
      End If

    End If

    If m_scanline = 240 Then
      ' Post Render Scanline - Do Nothing!
    End If

    If m_scanline >= 241 AndAlso m_scanline < 261 Then
      If m_scanline = 241 AndAlso m_cycle = 1 Then
        ' Effectively end of frame, so set vertical blank flag
        m_status.VerticalBlank = True

        ' If the control register tells us to emit a NMI when
        ' entering vertical blanking period, do it! The CPU
        ' will be informed that rendering is complete so it can
        ' perform operations with the PPU knowing it wont
        ' produce visible artefacts
        If m_control.EnableNmi Then
          m_nmi = True
        End If
      End If
    End If

    ' Composition - We now have background pixel information for this cycle
    ' At this point we are only interested in background

    Dim bg_pixel As Byte = &H0 ' The 2-bit pixel to be rendered
    Dim bg_palette As Byte = &H0 ' The 3-bit index of the palette the pixel indexes

    ' We only render backgrounds if the PPU is enabled to do so. Note if
    ' background rendering is disabled, the pixel and palette combine
    ' to form 0x00. This will fall through the colour tables to yield
    ' the current background colour in effect
    If m_mask.RenderBackground Then
      ' Handle Pixel Selection by selecting the relevant bit
      ' depending upon fine x scolling. This has the effect of
      ' offsetting ALL background rendering by a set number
      ' of pixels, permitting smooth scrolling
      Dim bit_mux As UInt16 = &H8000US >> m_fineX
      ' Select Plane pixels by extracting from the shifter 
      ' at the required location. 
      Dim p0_pixel As Byte = CByte(If((m_bgShifterPatternLo And bit_mux) > 0, 1, 0))
      Dim p1_pixel As Byte = CByte(If((m_bgShifterPatternHi And bit_mux) > 0, 1, 0))

      ' Combine to form pixel index
      bg_pixel = (p1_pixel << 1) Or p0_pixel

      ' Get palette
      Dim bg_pal0 As Byte = CByte(If((m_bgShifterAttribLo And bit_mux) > 0, 1, 0))
      Dim bg_pal1 As Byte = CByte(If((m_bgShifterAttribHi And bit_mux) > 0, 1, 0))
      bg_palette = (bg_pal1 << 1) Or bg_pal0
    End If

    ' Now we have a final pixel colour, and a palette for this cycle
    ' of the current scanline. Let's at long last, draw that ^&%*er :P
    m_sprScreen.SetPixel(m_cycle - 1, m_scanline, GetColourFromPaletteRam(bg_palette, bg_pixel))

    ' Fake some noise for now
    'sprScreen.SetPixel(cycle - 1, scanline, palScreen(If(Rnd() >= 0.5, &H3F, &H30)))

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

    '' Fake some noise for now
    'sprScreen.SetPixel(cycle - 1, scanline, palScreen(If((Rnd() >= 0.5), &H3F, &H30)))

    '' Advance renderer - it never stops, it's relentless
    'cycle += 1S
    'If cycle >= 341 Then
    '  cycle = 0
    '  scanline += 1S
    '  If scanline >= 261 Then
    '    scanline = -1
    '    frame_complete = True
    '  End If
    'End If

  End Sub

End Class