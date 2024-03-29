﻿Public Class Mapper000
  Inherits Mapper

  Public Sub New(prgBanks As Byte, chrBanks As Byte)
    MyBase.New(prgBanks, chrBanks)
  End Sub

  Public Overrides Function CpuMapRead(addr As UShort, ByRef mappedAddr As UInteger) As Boolean
    ' if PRGROM is 16KB
    '     CPU Address Bus          PRG ROM
    '     0x8000 -> 0xBFFF: Map    0x0000 -> 0x3FFF
    '     0xC000 -> 0xFFFF: Mirror 0x0000 -> 0x3FFF
    ' if PRGROM is 32KB
    '     CPU Address Bus          PRG ROM
    '     0x8000 -> 0xFFFF: Map    0x0000 -> 0x7FFF	
    If addr >= &H8000 AndAlso addr <= &HFFFF Then
      mappedAddr = addr And (If(m_prgBanks > 1, &H7FFFUI, &H3FFFUI))
      Return True
    End If
    Return False
  End Function

  Public Overrides Function CpuMapWrite(addr As UShort, ByRef mappedAddr As UInteger, data As Byte) As Boolean
    If addr >= &H8000 AndAlso addr <= &HFFFF Then
      mappedAddr = addr And (If(m_prgBanks > 1, &H7FFFUI, &H3FFFUI))
      Return True
    End If
    Return False
  End Function

  Public Overrides Function PpuMapRead(addr As UShort, ByRef mappedAddr As UInteger) As Boolean
    ' There is no mapping required for PPU
    ' PPU Address Bus          CHR ROM
    ' 0x0000 -> 0x1FFF: Map    0x0000 -> 0x1FFF
    If addr >= &H0 AndAlso addr <= &H1FFF Then
      mappedAddr = addr
      Return True
    End If
    Return False
  End Function

  Public Overrides Function PpuMapWrite(addr As UShort, ByRef mappedAddr As UInteger) As Boolean
    If addr >= &H0 AndAlso addr <= &H1FFF Then
      If m_chrBanks = 0 Then
        ' Treat as RAM
        mappedAddr = addr
        Return True
      End If
    End If
    Return False
  End Function

  Public Overrides Sub Reset()

  End Sub

End Class