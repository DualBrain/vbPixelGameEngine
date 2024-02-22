Imports VbPixelGameEngine

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing

Structure BLOADHeader
  Public Magic As [Byte]
  Public BaseAddr As UInt16
  Public OffsetAddr As UInt16
  Public Len As UInt16
End Structure

Structure PixelFragment
  Public Width As UInt16
  Public Height As UInt16
  Public PixelData As [Byte]()
End Structure

Class Loader

  Public Enum DataFormatEnum
    Unknown
    TDP
    GET_FRAGMENT
    SINGLE_EGA_PLANE
    EGA_FNT
  End Enum

  Public Enum FragmentColorPackingEnum
    CGA
    EGA
    Unknown
  End Enum

  Private Shared ReadOnly PALETTE_EGA As Integer() = {0, 0, 0,
                                                      0, 0, 170,
                                                      0, 170, 0,
                                                      0, 170, 170,
                                                      170, 0, 0,
                                                      170, 0, 170,
                                                      170, 85, 0,
                                                      170, 170, 170,
                                                      85, 85, 85,
                                                      85, 85, 170,
                                                      85, 255, 85,
                                                      85, 255, 255,
                                                      255, 85, 85,
                                                      255, 0, 255,
                                                      255, 255, 0,
                                                      255, 255, 255}

  Private Shared ReadOnly PALETTE_CGA As Integer() = {0, 0, 0, ' type 1 (Palette 0)
                                                      0, 170, 170,
                                                      170, 0, 170,
                                                      170, 170, 170,
                                                      0, 0, 0, ' type 2 (Palette 0 High Intensity)
                                                      85, 255, 255,
                                                      255, 85, 255,
                                                      255, 255, 255,
                                                      0, 0, 0, ' type 3 (Palette 1)
                                                      0, 170, 0,
                                                      170, 0, 0,
                                                      170, 85, 0,
                                                      0, 0, 0, ' type 4 (Palette 1 High Intensity)
                                                      85, 255, 85,
                                                      255, 85, 85,
                                                      255, 255, 85,
                                                      0, 0, 0, ' type 5 (Palette 2)
                                                      0, 170, 170,
                                                      170, 0, 0,
                                                      170, 170, 170,
                                                      0, 0, 0, ' type 6 (Palette 2 High Intensity)
                                                      85, 255, 255,
                                                      255, 85, 85,
                                                      255, 255, 255}

  Public Property FragmentColorPacking As FragmentColorPackingEnum
  Public ReadOnly Property ImageType As DataFormatEnum
  Public ReadOnly Property DataLength As Integer

  Private m_header As BLOADHeader
  Private ReadOnly m_width As Integer = -1
  Private ReadOnly m_height As Integer = -1
  Private ReadOnly m_fragments As New List(Of PixelFragment)

  Private m_palette As Integer = 0
  Public WriteOnly Property Palette As Integer
    Set(value As Integer)
      m_palette = Math.Min(6, Math.Max(1, value))
    End Set
  End Property

  Public Sub New(data As Byte(),
                 Optional defaultPacking As FragmentColorPackingEnum = FragmentColorPackingEnum.Unknown,
                 Optional defaultPalette As Integer = -1)

    Dim stream As New MemoryStream(data)
    Dim reader As New BinaryReader(stream)

    m_header.Magic = reader.ReadByte()
    m_header.BaseAddr = reader.ReadUInt16()
    m_header.OffsetAddr = reader.ReadUInt16()
    m_header.Len = reader.ReadUInt16()

    ImageType = DataFormatEnum.Unknown
    m_width = -1 : m_height = -1

    FragmentColorPacking = If(defaultPacking = FragmentColorPackingEnum.Unknown, FragmentColorPackingEnum.EGA, defaultPacking)
    m_palette = If(defaultPalette = -1, 0, defaultPalette)

    If m_header.Magic <> &HFD Then
      Throw New Exception("Magic number is not 0xFD!")
    End If

    Dim actualLen As Integer = data.Length - Marshal.SizeOf(GetType(BLOADHeader))
    If actualLen < m_header.Len - 1 Then
      Throw New Exception($"Header length mismatch, expected {m_header.Len} but got {actualLen}.")
    End If
    DataLength = m_header.Len

    Dim bodyStartPos = stream.Position

    ' Assume this is a GET fragment
    Dim fragmentWidth = reader.ReadUInt16()
    Dim fragmentHeight = reader.ReadUInt16()
    Console.WriteLine($"W={fragmentWidth} H={fragmentHeight}")
    Console.WriteLine($"Data size={m_header.Len}")

    ' 
    ' Format notes:
    ' 
    ' === TDP ============================================================================================================
    ' 
    ' TDP is full-screen EGA format used in my PantherTek/Turing Degree games.
    ' 
    ' TDP stands for Turing Degree Picture.
    ' 
    ' TDP is 10 GET fragments, each 320x20 pixels.  So each GET should be 3204 bytes (320/2 = 160 bytes per scanline, 
    ' x20 scalines for 3200 bytes plus 4 for GET header).
    ' 
    ' BASIC code to load and display a TDP:
    ' DIM TDP(16020)
    ' (BLOAD it)
    ' FOR I = 0 TO 180 STEP 20
    ' PUT (0, I), TDP(((I \ 20) * 1602))
    ' NEXT
    ' 
    ' So if TDP, len should be 32040 bytes.  10 GET fragments should follow, exactly 3204 bytes each and with a pixel size
    ' of 320x20.
    ' 
    ' Why did I design this full-screen format this way?  I don't recall, other than something to do with GET/PUT causing
    ' GWBASIC heap issues if I tried to grab too much screen memory at once.
    ' 
    ' 
    ' === GET fragment ====================================================================================================
    ' 
    ' This is a standard GWBASIC/QUICKBASIC GET/PUT screen fragment, dumped straight to disk via BSAVE command.
    ' 
    ' Example on how to save one under SCREEN 7:
    ' 
    ' FragmentSize% = INT(CINT(15/2) * 15) + 4
    ' GET (0, 0)-(14, 14), Fragment%
    ' DEF SEG = VARSEG(Fragment%(0))
    ' BSAVE "Filename", VARPTR(Fragment%(0)), FragmentSize%
    ' DEF SEG
    ' 
    ' Internally, a GET fragment is just a 4-byte header (Width, height) plus raw pixel data.
    ' 
    ' Pixel format is usually straight-up dump from video RAM.  There are no indicators in the GET data
    ' structure itself to tell you what kind of format it is in.  It's just assumed you will have issued the proper
    ' SCREEN command so the video memory format matches what's in the BSAVE file.
    ' 
    ' EGA is 4-bits per pixel, 2 pixels per byte.  Data is non-linear, in 4 planes.  Each plane represents one RGBI bit.
    ' 
    ' CGA is 2-bits per pixel, 4 pixels per byte.  Data is linear.  (Internally, CGA is banked in two interlaced frames,
    ' accessible as different even/odd planes, but GWBASIC/QB seems to hide that from us and just lets us write linearly.)
    ' 
    ' Unused bits are padded to get us up to byte boundaries.  These will always be zero.
    ' 
    ' Note: Some of my array calculations were off in my original games, or I re-used arrays which were larger than
    ' strictly necessary for saving the data, so there will be extra data padding at the end of some my BSAVEs.  So the
    ' fragment length cannot be trusted to be close to the required size.  (It will always be at least the minimum
    ' required size, and this checks that.)
    ' 
    ' Another note:  CGA width is double in fragment header, so divide it by two when in CGA format.
    ' 
    ' 
    ' === EGA Plane Dump ===================================================================================================
    ' 
    ' I don't use many of these types of images, but they are B&W single-bit plane dumps from EGA memory.  They are not GET
    ' fragments.
    ' 
    ' They are decoded the same as EGA GET fragments, except that they only have just one plane's worth of data, so they
    ' have a fixed plane width of 40 bytes per scanline.
    ' 
    ' It's not easy to detect these dumps.  At the moment, they are detected if the data length is exactly 8000 bytes.  So
    ' far this simple check works, as I rarely saved very big GET fragment sprites.
    ' 
    ' 
    ' === FNT EGA font =====================================================================================================
    ' 
    ' Data length of 2340 bytes, arranged in 65 characters of 8x8 GET fragments (each 36 bytes in length).
    ' 
    ' Each GET fragment is just a 8x8 chunk of EGA memory.
    ' 
    ' I used FNTs in a lot of my early EGA games.  The character set is really simple:  Just A-Z 0-9 with punctuation and
    ' no lower case.
    ' 
    ' 
    ' === VGA Linear Dump ==================================================================================================
    ' 
    ' These are pretty straight forward.  They are just 64000 byte dumps of VGA memory at 0xA000 while in Mode 13 (ie:
    ' chained linear mode).
    ' 
    ' Palette is stored in a seperate .PAL file.
    ' 
    ' Currently not decoding these, as I can only remember one time I used this format before I switched to the
    ' PGF format with my VGA-era games.   (PGF = PantherTek Graphics Format, which was an indexed chunked binary format.)
    ' 
    ' 
    ' 

    If fragmentWidth = 320 AndAlso fragmentHeight = 20 AndAlso m_header.Len = 32040 Then

      Console.WriteLine("We think this is a TDP.")

      FragmentColorPacking = FragmentColorPackingEnum.EGA ' Force EGA packing

      ' let's verify our theory that this is an EGA TDP by examining the next GET fragment and see if it still has a size of 320x20
      stream.Position = bodyStartPos + 3200 + 4
      Dim fW = reader.ReadUInt16()
      Dim fH = reader.ReadUInt16()
      If fW <> 320 AndAlso fH <> 20 Then
        Throw New Exception($"Attempted to detect TDP, but second chunk had wrong width/height.  Expected 320,20 but got {fW},{fH}.")
      End If
      ImageType = DataFormatEnum.TDP
      m_width = 320
      m_height = 200

      ' decode these as a series of streams, we should get 10 fragments
      stream.Position = bodyStartPos ' reset stream

      For i = 0 To 10 - 1

        Dim frag As New PixelFragment With {.Width = reader.ReadUInt16(), .Height = reader.ReadUInt16()}

        If frag.Width <> 320 OrElse frag.Height <> 20 Then
          Throw New Exception($"GET fragment in TDP chunk #{i} had unusual w/h: {frag.Width} {frag.Height}")
        End If

        Dim chunkLen As Integer = CInt(CLng(Fix(Math.Round(320.0F * 20.0F / 2.0F))) Mod Integer.MaxValue)

        frag.PixelData = New [Byte](chunkLen - 1) {}

        Dim dataRead = stream.Read(frag.PixelData, 0, chunkLen)
        If dataRead <> chunkLen Then
          Throw New Exception($"Tried to read TDP fragment, but not enough data.  Expected {m_header.Len} but got {dataRead} bytes.")
        End If

        m_fragments.Add(frag)

      Next

    ElseIf m_header.Len = 8000 AndAlso ((fragmentWidth <= 0 OrElse fragmentWidth >= 320) OrElse (fragmentHeight <= 0 OrElse fragmentHeight >= 200)) Then

      ' Special case, singular EGA plane dump
      ImageType = DataFormatEnum.SINGLE_EGA_PLANE
      m_width = 320
      m_height = 200

      stream.Position = bodyStartPos ' reset to beginning

      Dim frag As New PixelFragment With {.Width = 320, .Height = 200, .PixelData = New [Byte](m_header.Len - 1) {}}
      Dim amtToRead = m_header.Len

      Dim dataRead = stream.Read(frag.PixelData, 0, amtToRead)
      If dataRead <> amtToRead Then
        Throw New Exception($"Tried to read plane dump, but not enough data.  Expected {amtToRead} but got {dataRead} bytes.")
      End If

      m_fragments.Add(frag)

    ElseIf m_header.Len = 2340 AndAlso fragmentWidth = 8 AndAlso fragmentHeight = 8 Then

      ' Special case, EGA FNT
      ' Check to see if the next chunk is also 8x8
      stream.Position = bodyStartPos + 36
      Dim fW = reader.ReadUInt16()
      Dim fH = reader.ReadUInt16()
      If fW <> 8 AndAlso fH <> 8 Then
        Throw New Exception($"Attempted to detect EGA FNT, but second chunk had wrong width/height.  Expected 8,8 but got {fW},{fH}.")
      End If

      ImageType = DataFormatEnum.EGA_FNT
      m_width = 320
      m_height = 200

      stream.Position = bodyStartPos ' reset to beginning

      For i = 0 To 65 - 1

        Dim frag As New PixelFragment With {.Width = reader.ReadUInt16(), .Height = reader.ReadUInt16()}

        If frag.Width <> 8 OrElse frag.Height <> 8 Then
          Throw New Exception($"GET fragment in EGA FNT chunk #{i} had unusual w/h: {frag.Width} {frag.Height}")
        End If

        Dim chunkLen = 32
        frag.PixelData = New [Byte](chunkLen - 1) {}

        Dim dataRead = stream.Read(frag.PixelData, 0, chunkLen)
        If dataRead <> chunkLen Then
          Throw New Exception($"Tried to read EGA FNT fragment, but not enough data.  Expected {m_header.Len} but got {dataRead} bytes.")
        End If

        m_fragments.Add(frag)

      Next

    Else

      ImageType = DataFormatEnum.GET_FRAGMENT
      m_width = fragmentWidth
      m_height = fragmentHeight

      Dim frag As New PixelFragment With {.Width = fragmentWidth, .Height = fragmentHeight, .PixelData = New Byte(m_header.Len - 1) {}}
      Dim amtToRead = m_header.Len - 4 ' minus four bytes for width/height

      Dim dataRead = stream.Read(frag.PixelData, 0, amtToRead)
      If dataRead <> amtToRead Then
        Throw New Exception($"Tried to read singular fragment, but not enough data.  Expected {amtToRead} but got {dataRead} bytes.")
      End If

      m_fragments.Add(frag)

    End If

  End Sub

  Public Shared Function GetPlaneWidth(width As Integer, format As FragmentColorPackingEnum) As Integer

    If format = FragmentColorPackingEnum.CGA Then
      ' no planes in CGA, so it's just 1:1
      Return 1
    End If
    If format = FragmentColorPackingEnum.EGA Then
      ' EGA is stored in 4 planes per scanline, so width of each plane is pixelWidth / (4 planes * 2 pixels per byte)
      Return CInt(CLng(Fix(Math.Ceiling(width / 8.0F))) Mod Integer.MaxValue)
    End If

    Throw New Exception("GetPixelsPerByte: Unknown format.")

  End Function

  Public Shared Function GetBytesPerScanline(width As Integer, format As FragmentColorPackingEnum) As Integer

    If format = FragmentColorPackingEnum.CGA Then
      Return CInt(CLng(Fix(Math.Ceiling(width / 4.0F))) Mod Integer.MaxValue) ' just width of pixels/4
    End If
    If format = FragmentColorPackingEnum.EGA Then
      Return GetPlaneWidth(width, format) * 4 ' 4 planes, so width of each plane *4
    End If

    Throw New Exception("GetPixelsPerByte: Unknown format.")

  End Function

  Public Sub RasterizeFragment(frag As PixelFragment, targetSprite As Sprite, currFormat As FragmentColorPackingEnum, offset As Point)

    ' for CGA, width is double the amount in the fragment data, so adjust accordingly
    Dim adjustedWidth = If(currFormat = FragmentColorPackingEnum.CGA, frag.Width \ 2, frag.Width)

    Dim planeWidth = GetPlaneWidth(adjustedWidth, currFormat)
    Dim bytesPerScanline = GetBytesPerScanline(adjustedWidth, currFormat)

    ' check to make sure there is enough data for this
    If frag.PixelData.Length < frag.Height * bytesPerScanline Then
      Throw New Exception($"Not enough data to unpack:  Requesting {frag.Height * bytesPerScanline} bytes for {FragmentColorPacking} format, but only have {frag.PixelData.Length} bytes.")
    End If

    If currFormat = FragmentColorPackingEnum.EGA Then
      For py = 0 To frag.Height - 1
        For px = 0 To adjustedWidth - 1

          'Dim color1 = Color.Black
          ' Each EGA byte presents two pixels, and each bit in the byte presents a single EGA plane access
          Dim bitMask = (1 << (7 - (px Mod 8)))

          ' Shuffle data from the different 4 planes (which are stored sequentially for each scanline)
          Dim data_plane0 = If((frag.PixelData((py * bytesPerScanline) + ((px \ 8)) + (planeWidth * 0)) And bitMask) > 0, 1, 0) ' Blue plane
          Dim data_plane1 = If((frag.PixelData((py * bytesPerScanline) + ((px \ 8)) + (planeWidth * 1)) And bitMask) > 0, 1, 0) ' Green plane
          Dim data_plane2 = If((frag.PixelData((py * bytesPerScanline) + ((px \ 8)) + (planeWidth * 2)) And bitMask) > 0, 1, 0) ' Red plane
          Dim data_plane3 = If((frag.PixelData((py * bytesPerScanline) + ((px \ 8)) + (planeWidth * 3)) And bitMask) > 0, 1, 0) ' Intensity plane

          ' just convert back to index color value and find in our CLUT
          Dim pixelValue = data_plane0 + (data_plane1 << 1) + (data_plane2 << 2) + (data_plane3 << 3)
          'Dim color1 = Drawing.Color.FromArgb(&HFF, PALETTE_EGA(pixelValue * 3), PALETTE_EGA(pixelValue * 3 + 1), PALETTE_EGA(pixelValue * 3 + 2))
          Dim pixel = New Pixel(PALETTE_EGA(pixelValue * 3), PALETTE_EGA(pixelValue * 3 + 1), PALETTE_EGA(pixelValue * 3 + 2), &HFF)

          targetSprite.SetPixel(offset.X + px, offset.Y + py, pixel)

        Next
      Next
    ElseIf currFormat = FragmentColorPackingEnum.CGA Then
      For py = 0 To frag.Height - 1
        For px = 0 To adjustedWidth - 1

          'Dim color1 = Color.Black
          ' Each CGA byte presents 4 pixels
          Dim bitMask = px Mod 4
          ' So access each pixel as a 2-bit value from the byte, from MSB to LSB
          Dim pixelValue = CInt(frag.PixelData((px \ 4) + (py * bytesPerScanline)) >> ((3 - bitMask) * 2))
          pixelValue = pixelValue And &H3

          ' just convert back to index color value and find in our CLUT (from 6 different CGA palettes)
          Dim palIndex = (pixelValue * 3) + (m_palette * 12)
          'Dim color1 = Color.FromArgb(&HFF, PALETTE_CGA(palIndex), PALETTE_CGA(palIndex + 1), PALETTE_CGA(palIndex + 2))
          Dim pixel = New Pixel(PALETTE_CGA(palIndex), PALETTE_CGA(palIndex + 1), PALETTE_CGA(palIndex + 2), &HFF)

          targetSprite.SetPixel(offset.X + px, offset.Y + py, pixel)

        Next
      Next
    End If

  End Sub

  Public Shared Sub RasterizeRawEGAPlaneDump(frag As PixelFragment, targetSprite As Sprite, currFormat As FragmentColorPackingEnum, offset As Point)

    ' for CGA, width is double the amount in the fragment data
    Dim bytesPerScanline = CInt(CLng(Fix(Math.Ceiling(frag.Width / 8.0F))) Mod Integer.MaxValue)

    ' check to make sure there is enough data for this
    If frag.PixelData.Length < frag.Height * bytesPerScanline Then
      Throw New Exception($"Not enough data to unpack:  Requesting {frag.Height * bytesPerScanline} bytes for planar EGA, but only have {frag.PixelData.Length} bytes.")
    End If

    For py = 0 To frag.Height - 1
      For px = 0 To frag.Width - 1

        Dim pixel = New Pixel(0, 0, 0)
        Dim bitMask = (1 << (7 - (px Mod 8)))
        Dim data_plane0 = If((frag.PixelData((py * bytesPerScanline) + ((px \ 8))) And bitMask) > 0, 1, 0) ' Blue

        If data_plane0 > 0 Then
          pixel = New Pixel(PALETTE_EGA(15 * 3), PALETTE_EGA(15 * 3 + 1), PALETTE_EGA(15 * 3 + 2), &HFF)
        End If

        targetSprite.SetPixel(offset.X + px, offset.Y + py, pixel)

      Next
    Next

  End Sub

  'Public Sub ConvertToBitmap(targetBitmap As Bitmap, Optional currFormat As FragmentColorPackingEnum = FragmentColorPackingEnum.Unknown, Optional palette1 As Integer = -1)

  '  If currFormat = FragmentColorPackingEnum.Unknown Then currFormat = FragmentColorPacking

  '  If palette1 <> -1 Then m_palette = palette1

  '  ' unpack based on format
  '  If ImageType = DataFormatEnum.GET_FRAGMENT Then
  '    Dim frag = m_fragments(0)
  '    RasterizeFragment(frag, targetBitmap, currFormat, New Point(0, 0))
  '  ElseIf ImageType = DataFormatEnum.SINGLE_EGA_PLANE Then
  '    Dim frag = m_fragments(0)
  '    RasterizeRawEGAPlaneDump(frag, targetBitmap, currFormat, New Point(0, 0))
  '  ElseIf ImageType = DataFormatEnum.EGA_FNT Then
  '    Dim x1 = 0, y1 = 0
  '    For i = 0 To 65 - 1
  '      RasterizeFragment(m_fragments(i), targetBitmap, currFormat, New Point(x1, y1))
  '      x1 += 8
  '      If x1 > 200 Then x1 = 0 : y1 += 8
  '    Next
  '  ElseIf ImageType = DataFormatEnum.TDP Then
  '    For i = 0 To 10 - 1
  '      RasterizeFragment(m_fragments(i), targetBitmap, currFormat, New Point(0, i * 20))
  '    Next
  '  End If

  'End Sub

  Public Sub ConvertToSprite(targetSprite As Sprite, Optional currFormat As FragmentColorPackingEnum = FragmentColorPackingEnum.Unknown, Optional palette1 As Integer = -1)

    If currFormat = FragmentColorPackingEnum.Unknown Then currFormat = FragmentColorPacking

    If palette1 <> -1 Then m_palette = palette1

    ' unpack based on format
    If ImageType = DataFormatEnum.GET_FRAGMENT Then
      Dim frag = m_fragments(0)
      RasterizeFragment(frag, targetSprite, currFormat, New Point(0, 0))
    ElseIf ImageType = DataFormatEnum.SINGLE_EGA_PLANE Then
      Dim frag = m_fragments(0)
      RasterizeRawEGAPlaneDump(frag, targetSprite, currFormat, New Point(0, 0))
    ElseIf ImageType = DataFormatEnum.EGA_FNT Then
      Dim x1 = 0, y1 = 0
      For i = 0 To 65 - 1
        RasterizeFragment(m_fragments(i), targetSprite, currFormat, New Point(x1, y1))
        x1 += 8
        If x1 > 200 Then x1 = 0 : y1 += 8
      Next
    ElseIf ImageType = DataFormatEnum.TDP Then
      For i = 0 To 10 - 1
        RasterizeFragment(m_fragments(i), targetSprite, currFormat, New Point(0, i * 20))
      Next
    End If

  End Sub

  Public Overrides Function ToString() As String

    Dim info = $"Size: {m_width}, {m_height}
BSAVE format: {ImageType}
Color Packing: {FragmentColorPacking}"
    If FragmentColorPacking = FragmentColorPackingEnum.CGA Then
      info &= $"{vbLf}CGA Palette: {m_palette + 1}"
    End If
    info &= $"{vbLf}GET_FRAGMENTS: {m_fragments.Count}
BSAVE data length: {DataLength}"

    Return info

  End Function

  Public Function GetBitmapWidth(Optional currFormat As FragmentColorPackingEnum = FragmentColorPackingEnum.Unknown) As Integer

    If currFormat = FragmentColorPackingEnum.Unknown Then
      currFormat = FragmentColorPacking
    End If

    If ImageType = DataFormatEnum.SINGLE_EGA_PLANE OrElse
       ImageType = DataFormatEnum.TDP OrElse
       ImageType = DataFormatEnum.EGA_FNT Then
      Return 320
    End If

    If currFormat = FragmentColorPackingEnum.CGA Then
      Return m_width \ 2
    Else
      Return m_width
    End If

  End Function

  Public Function GetBitmapHeight(Optional currFormat As FragmentColorPackingEnum = FragmentColorPackingEnum.Unknown) As Integer

    If currFormat = FragmentColorPackingEnum.Unknown Then
      currFormat = FragmentColorPacking
    End If

    If ImageType = DataFormatEnum.SINGLE_EGA_PLANE OrElse
       ImageType = DataFormatEnum.TDP OrElse
       ImageType = DataFormatEnum.EGA_FNT Then
      Return 200
    End If

    Return m_height

  End Function

End Class