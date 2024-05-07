Imports System.IO
Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New Demo
    If demo.Construct(320, 200, 4, 4) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class Demo
  Inherits PixelGameEngine

  Private Shared ReadOnly m_palette13 As New List(Of Pixel) _
    From {New Pixel(0, 0, 0),
          New Pixel(0, 0, 170),
          New Pixel(0, 170, 0),
          New Pixel(0, 170, 170),
          New Pixel(170, 0, 0),
          New Pixel(170, 0, 170),
          New Pixel(170, 85, 0),
          New Pixel(170, 170, 170),
          New Pixel(85, 85, 85),
          New Pixel(85, 85, 255),
          New Pixel(85, 255, 85),
          New Pixel(85, 255, 255),
          New Pixel(255, 85, 85),
          New Pixel(255, 85, 255),
          New Pixel(255, 255, 85),
          New Pixel(255, 255, 255),
          New Pixel(0, 0, 0),
          New Pixel(20, 20, 20),
          New Pixel(32, 32, 32),
          New Pixel(44, 44, 44),
          New Pixel(56, 56, 56),
          New Pixel(68, 68, 68),
          New Pixel(80, 80, 80),
          New Pixel(97, 97, 97),
          New Pixel(113, 113, 113),
          New Pixel(129, 129, 129),
          New Pixel(145, 145, 145),
          New Pixel(161, 161, 161),
          New Pixel(182, 182, 182),
          New Pixel(202, 202, 202),
          New Pixel(226, 226, 226),
          New Pixel(255, 255, 255),
          New Pixel(0, 0, 255),
          New Pixel(64, 0, 255),
          New Pixel(125, 0, 255),
          New Pixel(190, 0, 255),
          New Pixel(255, 0, 255),
          New Pixel(255, 0, 190),
          New Pixel(255, 0, 125),
          New Pixel(255, 0, 64),
          New Pixel(255, 0, 0),
          New Pixel(255, 64, 0),
          New Pixel(255, 125, 0),
          New Pixel(255, 190, 0),
          New Pixel(255, 255, 0),
          New Pixel(190, 255, 0),
          New Pixel(125, 255, 0),
          New Pixel(64, 255, 0),
          New Pixel(0, 255, 0),
          New Pixel(0, 255, 64),
          New Pixel(0, 255, 125),
          New Pixel(0, 255, 190),
          New Pixel(0, 255, 255),
          New Pixel(0, 190, 255),
          New Pixel(0, 125, 255),
          New Pixel(0, 64, 255),
          New Pixel(125, 125, 255),
          New Pixel(157, 125, 255),
          New Pixel(190, 125, 255),
          New Pixel(222, 125, 255),
          New Pixel(255, 125, 255),
          New Pixel(255, 125, 222),
          New Pixel(255, 125, 190),
          New Pixel(255, 125, 157),
          New Pixel(255, 125, 125),
          New Pixel(255, 157, 125),
          New Pixel(255, 190, 125),
          New Pixel(255, 222, 125),
          New Pixel(255, 255, 125),
          New Pixel(222, 255, 125),
          New Pixel(190, 255, 125),
          New Pixel(157, 255, 125),
          New Pixel(125, 255, 125),
          New Pixel(125, 255, 157),
          New Pixel(125, 255, 190),
          New Pixel(125, 255, 222),
          New Pixel(125, 255, 255),
          New Pixel(125, 222, 255),
          New Pixel(125, 190, 255),
          New Pixel(125, 157, 255),
          New Pixel(182, 182, 255),
          New Pixel(198, 182, 255),
          New Pixel(218, 182, 255),
          New Pixel(234, 182, 255),
          New Pixel(255, 182, 255),
          New Pixel(255, 182, 234),
          New Pixel(255, 182, 218),
          New Pixel(255, 182, 198),
          New Pixel(255, 182, 182),
          New Pixel(255, 198, 182),
          New Pixel(255, 218, 182),
          New Pixel(255, 234, 182),
          New Pixel(255, 255, 182),
          New Pixel(234, 255, 182),
          New Pixel(218, 255, 182),
          New Pixel(198, 255, 182),
          New Pixel(182, 255, 182),
          New Pixel(182, 255, 198),
          New Pixel(182, 255, 218),
          New Pixel(182, 255, 234),
          New Pixel(182, 255, 255),
          New Pixel(182, 234, 255),
          New Pixel(182, 218, 255),
          New Pixel(182, 198, 255),
          New Pixel(0, 0, 113),
          New Pixel(28, 0, 113),
          New Pixel(56, 0, 113),
          New Pixel(85, 0, 113),
          New Pixel(113, 0, 113),
          New Pixel(113, 0, 85),
          New Pixel(113, 0, 56),
          New Pixel(113, 0, 28),
          New Pixel(113, 0, 0),
          New Pixel(113, 28, 0),
          New Pixel(113, 56, 0),
          New Pixel(113, 85, 0),
          New Pixel(113, 113, 0),
          New Pixel(85, 113, 0),
          New Pixel(56, 113, 0),
          New Pixel(28, 113, 0),
          New Pixel(0, 113, 0),
          New Pixel(0, 113, 28),
          New Pixel(0, 113, 56),
          New Pixel(0, 113, 85),
          New Pixel(0, 113, 113),
          New Pixel(0, 85, 113),
          New Pixel(0, 56, 113),
          New Pixel(0, 28, 113),
          New Pixel(56, 56, 113),
          New Pixel(68, 56, 113),
          New Pixel(85, 56, 113),
          New Pixel(97, 56, 113),
          New Pixel(113, 56, 113),
          New Pixel(113, 56, 97),
          New Pixel(113, 56, 85),
          New Pixel(113, 56, 68),
          New Pixel(113, 56, 56),
          New Pixel(113, 68, 56),
          New Pixel(113, 85, 56),
          New Pixel(113, 97, 56),
          New Pixel(113, 113, 56),
          New Pixel(97, 113, 56),
          New Pixel(85, 113, 56),
          New Pixel(68, 113, 56),
          New Pixel(56, 113, 56),
          New Pixel(56, 113, 68),
          New Pixel(56, 113, 85),
          New Pixel(56, 113, 97),
          New Pixel(56, 113, 113),
          New Pixel(56, 97, 113),
          New Pixel(56, 85, 113),
          New Pixel(56, 68, 113),
          New Pixel(80, 80, 113),
          New Pixel(89, 80, 113),
          New Pixel(97, 80, 113),
          New Pixel(105, 80, 113),
          New Pixel(113, 80, 113),
          New Pixel(113, 80, 105),
          New Pixel(113, 80, 97),
          New Pixel(113, 80, 89),
          New Pixel(113, 80, 80),
          New Pixel(113, 89, 80),
          New Pixel(113, 97, 80),
          New Pixel(113, 105, 80),
          New Pixel(113, 113, 80),
          New Pixel(105, 113, 80),
          New Pixel(97, 113, 80),
          New Pixel(89, 113, 80),
          New Pixel(80, 113, 80),
          New Pixel(80, 113, 89),
          New Pixel(80, 113, 97),
          New Pixel(80, 113, 105),
          New Pixel(80, 113, 113),
          New Pixel(80, 105, 113),
          New Pixel(80, 97, 113),
          New Pixel(80, 89, 113),
          New Pixel(0, 0, 64),
          New Pixel(16, 0, 64),
          New Pixel(32, 0, 64),
          New Pixel(48, 0, 64),
          New Pixel(64, 0, 64),
          New Pixel(64, 0, 48),
          New Pixel(64, 0, 32),
          New Pixel(64, 0, 16),
          New Pixel(64, 0, 0),
          New Pixel(64, 16, 0),
          New Pixel(64, 32, 0),
          New Pixel(64, 48, 0),
          New Pixel(64, 64, 0),
          New Pixel(48, 64, 0),
          New Pixel(32, 64, 0),
          New Pixel(16, 64, 0),
          New Pixel(0, 64, 0),
          New Pixel(0, 64, 16),
          New Pixel(0, 64, 32),
          New Pixel(0, 64, 48),
          New Pixel(0, 64, 64),
          New Pixel(0, 48, 64),
          New Pixel(0, 32, 64),
          New Pixel(0, 16, 64),
          New Pixel(32, 32, 64),
          New Pixel(40, 32, 64),
          New Pixel(48, 32, 64),
          New Pixel(56, 32, 64),
          New Pixel(64, 32, 64),
          New Pixel(64, 32, 56),
          New Pixel(64, 32, 48),
          New Pixel(64, 32, 40),
          New Pixel(64, 32, 32),
          New Pixel(64, 40, 32),
          New Pixel(64, 48, 32),
          New Pixel(64, 56, 32),
          New Pixel(64, 64, 32),
          New Pixel(56, 64, 32),
          New Pixel(48, 64, 32),
          New Pixel(40, 64, 32),
          New Pixel(32, 64, 32),
          New Pixel(32, 64, 40),
          New Pixel(32, 64, 48),
          New Pixel(32, 64, 56),
          New Pixel(32, 64, 64),
          New Pixel(32, 56, 64),
          New Pixel(32, 48, 64),
          New Pixel(32, 40, 64),
          New Pixel(44, 44, 64),
          New Pixel(48, 44, 64),
          New Pixel(52, 44, 64),
          New Pixel(60, 44, 64),
          New Pixel(64, 44, 64),
          New Pixel(64, 44, 60),
          New Pixel(64, 44, 52),
          New Pixel(64, 44, 48),
          New Pixel(64, 44, 44),
          New Pixel(64, 48, 44),
          New Pixel(64, 52, 44),
          New Pixel(64, 60, 44),
          New Pixel(64, 64, 44),
          New Pixel(60, 64, 44),
          New Pixel(52, 64, 44),
          New Pixel(48, 64, 44),
          New Pixel(44, 64, 44),
          New Pixel(44, 64, 48),
          New Pixel(44, 64, 52),
          New Pixel(44, 64, 60),
          New Pixel(44, 64, 64),
          New Pixel(44, 60, 64),
          New Pixel(44, 52, 64),
          New Pixel(44, 48, 64),
          New Pixel(0, 0, 0),
          New Pixel(0, 0, 0),
          New Pixel(0, 0, 0),
          New Pixel(0, 0, 0),
          New Pixel(0, 0, 0),
          New Pixel(0, 0, 0),
          New Pixel(0, 0, 0),
          New Pixel(0, 0, 0)}

  Friend Sub New()
    AppName = "BSave"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    Return True
  End Function

#Region "BLOAD"

  Private Shared Function BLoad(path As String) As Sprite

    Using fs = New FileStream(path, FileMode.Open)

      Try

        Dim magic = fs.ReadByte()
        If magic <> &HFD Then
          Debug.WriteLine("ERROR: Not a QuickBASIC BSAVE graphics file!")
          Return New Sprite(16, 16)
        End If

        fs.Seek(5, 0)                       ' Skip Segment/Offset data
        Dim lowByte = fs.ReadByte()
        Dim dataSize = fs.ReadByte()
        dataSize = (dataSize << 8) Or lowByte

        lowByte = fs.ReadByte()
        Dim gfxWidth = fs.ReadByte()
        gfxWidth = (gfxWidth << 8) Or lowByte
        gfxWidth = (gfxWidth >> 3)

        lowByte = fs.ReadByte()
        Dim gfxHeight = fs.ReadByte()
        gfxHeight = (gfxHeight << 8) Or lowByte

        If gfxWidth * gfxHeight <> dataSize - 4 Then
          Debug.WriteLine("ERROR: Not a Screen 13 graphics file!")
          Return New Sprite(16, 16)
        End If

        ' Read bitmap bytes into byte buffer
        Dim byteBuffer(dataSize - 1) As Byte
        Dim dataRead = 0
        While dataSize - 4 <> 0
          dataRead = fs.Read(byteBuffer, dataRead, dataSize)
          dataSize -= dataRead
        End While

        ' Convert byte array into .NET bitmap
        Dim result = New Sprite(gfxWidth, gfxHeight)
        For x = 0 To gfxWidth - 1
          For y = 0 To gfxHeight - 1
            result.SetPixel(x, y, m_palette13(Convert.ToInt32(byteBuffer(x + (y * gfxWidth)))))
          Next
        Next
        Return result

      Catch
        Debug.WriteLine("ERROR: Unknown fatal error loading file!")
        Return New Sprite(16, 16)
      End Try

    End Using

  End Function

#End Region

#Region "BSAVE"

  Private Shared Sub BSave(sprite As Sprite, path As String)

    ' Get dimensions from sprite
    Dim gfxWidth = sprite.Width
    Dim gfxHeight = sprite.Height
    Dim dataSize = gfxWidth * gfxHeight + 4

    If gfxWidth > 320 Then
      Debug.WriteLine("ERROR: Graphics cannot exceed 320 pixels wide!" & vbLf & "Please resize your image and try again...")
      Return
    End If
    If gfxHeight > 200 Then
      Debug.WriteLine("ERROR: Graphics cannot exceed 200 pixels high!" & vbLf & "Please resize your image and try again...")
      Return
    End If

    ' Generate file header
    Dim header(6) As Byte
    header(0) = &HFD                      ' Magic
    header(1) = &H9C
    header(2) = &H0                       ' Segment
    header(3) = &H0
    header(4) = &H0                       ' Offset
    header(5) = CByte(dataSize And &HFF)  ' Low byte of data size
    header(6) = CByte(dataSize >> 8)      ' High byte of data size

    ' Generate bitmap data
    Dim gfx(dataSize - 1) As Byte
    For x = 0 To gfxWidth - 1
      For y = 0 To gfxHeight - 1
        gfx(x + (y * gfxWidth) + 4) = CByte(NearestColor(sprite.GetPixel(x, y)))
      Next
    Next

    gfxWidth <<= 3                                        ' SCREEN 13 - QB stores Width * 8
    gfx(0) = CByte(gfxWidth And &HFF)                     ' Low byte of GFX Width
    gfx(1) = CByte(gfxWidth >> 8)                         ' High byte of GFX Width
    gfx(2) = CByte(gfxHeight And &HFF)                    ' Low byte of GFX Height
    gfx(3) = CByte(gfxHeight >> 8)                        ' High byte of GFX Height

    Using fs = New FileStream(path, FileMode.CreateNew)
      fs.Write(header, 0, 7)         ' Write header to file
      fs.Write(gfx, 0, dataSize)     ' Write bitmap to file
    End Using

  End Sub

#End Region

  Private Shared Function NearestColor(pixel As Pixel) As Integer

    Dim result = &H0
    Dim smallest = &H7FFFFFFF

    For c = 0 To 256 - 1
      Dim diffR = pixel.R - m_palette13(c).R, diffG = pixel.G - m_palette13(c).G, diffB = pixel.B - m_palette13(c).B
      Dim diff = (diffR * diffR) + (diffB * diffB) + (diffG * diffG)
      If diff = 0 Then Return c
      If diff < smallest Then smallest = diff : result = c
    Next

    Return result

  End Function

End Class