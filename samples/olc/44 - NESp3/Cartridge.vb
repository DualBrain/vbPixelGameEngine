Public Class Cartridge

  Private ReadOnly m_imageValid As Boolean = False
  Private ReadOnly m_mapperId As Byte = 0
  Private ReadOnly m_prgBanks As Byte = 0
  Private ReadOnly m_chrBanks As Byte = 0
  Private ReadOnly m_prgMemory As New List(Of Byte)
  Private ReadOnly m_chrMemory As New List(Of Byte)
  Private ReadOnly m_mapper As Mapper

  Private Structure Header
    Public Name As Char()
    Public PrgRomChunks As Byte
    Public ChrRomChunks As Byte
    Public Mapper1 As Byte
    Public Mapper2 As Byte
    Public PrgRamSize As Byte
    Public TvSystem1 As Byte
    Public TvSystem2 As Byte
    Public Unused As Char()
  End Structure

  Public Enum Mirrors
    Horizontal
    Vertical
    OneScreenLo
    OneScreenHi
  End Enum

  Public m_mirror As Mirrors = Mirrors.Horizontal

  Public Sub New(filename As String)

    Dim header As Header
    m_imageValid = False

    Dim ifs As New IO.FileStream(filename, IO.FileMode.Open)
    Dim br As New IO.BinaryReader(ifs)

    If ifs IsNot Nothing Then

      header.Name = br.ReadChars(4)
      header.PrgRomChunks = br.ReadByte()
      header.ChrRomChunks = br.ReadByte()
      header.Mapper1 = br.ReadByte()
      header.Mapper2 = br.ReadByte()
      header.PrgRamSize = br.ReadByte()
      header.TvSystem1 = br.ReadByte()
      header.TvSystem2 = br.ReadByte()
      header.Unused = br.ReadChars(5)

      If (header.Mapper1 And &H4) <> 0 Then
        ifs.Seek(512, IO.SeekOrigin.Current)
      End If

      m_mapperId = ((header.Mapper2 >> 4) << 4) Or (header.Mapper1 >> 4)
      m_mirror = If((header.Mapper1 And &H1) <> 0, Mirrors.Vertical, Mirrors.Horizontal)

      Dim nFileType As Byte = 1

      If nFileType = 0 Then

      End If

      If nFileType = 1 Then
        m_prgBanks = header.PrgRomChunks
        m_prgMemory.Capacity = m_prgBanks * 16384
        m_prgMemory.AddRange(br.ReadBytes(m_prgMemory.Capacity))

        m_chrBanks = header.ChrRomChunks
        m_chrMemory.Capacity = m_chrBanks * 8192
        m_chrMemory.AddRange(br.ReadBytes(m_chrMemory.Capacity))
      End If

      If nFileType = 2 Then

      End If

      Select Case m_mapperId
        Case 0 : m_mapper = New Mapper000(m_prgBanks, m_chrBanks)
      End Select

      m_imageValid = True

      ifs.Close()

    End If

  End Sub

  Public Sub New()
  End Sub

  Public Function ImageValid() As Boolean
    Return m_imageValid
  End Function

  Public Function CpuRead(addr As UShort, ByRef data As Byte) As Boolean
    Dim mapped_addr As UInteger = 0
    If m_mapper.CpuMapRead(addr, mapped_addr) Then
      data = m_prgMemory(CInt(mapped_addr))
      Return True
    Else
      Return False
    End If
  End Function

  Public Function CpuWrite(addr As UShort, data As Byte) As Boolean
    Dim mapped_addr As UInteger = 0
    If m_mapper.CpuMapWrite(addr, mapped_addr) Then
      m_prgMemory(CInt(mapped_addr)) = data
      Return True
    Else
      Return False
    End If
  End Function

  Public Function PpuRead(addr As UShort, ByRef data As Byte) As Boolean
    Dim mapped_addr As UInteger = 0
    If m_mapper.PpuMapRead(addr, mapped_addr) Then
      data = m_chrMemory(CInt(mapped_addr))
      Return True
    Else
      Return False
    End If
  End Function

  Public Function PpuWrite(addr As UShort, data As Byte) As Boolean
    Dim mapped_addr As UInteger = 0
    If m_mapper.PpuMapRead(addr, mapped_addr) Then
      m_chrMemory(CInt(mapped_addr)) = data
      Return True
    Else
      Return False
    End If
  End Function

End Class