Imports System.IO
Imports System.Drawing

Public Class Sprite

#If PGE_DBG_OVERDRAW Then
  Private Shared nOverdrawCount As Integer
#End If

  Private m_pixelColData As Pixel()
  Private ReadOnly m_modeSample As Mode

  Public Enum Mode
    Normal
    Periodic
  End Enum

  Public Property Width As Integer
  Public Property Height As Integer

  Public Sub New()
    m_pixelColData = Nothing
    Width = 0
    Height = 0
  End Sub

  Public Sub New(imageFile As String, Optional pack As ResourcePack = Nothing)
    LoadFromFile(imageFile, pack)
  End Sub

  Public Sub New(w As Integer, h As Integer)
    If m_pixelColData IsNot Nothing Then
      Erase m_pixelColData
    End If
    Width = w
    Height = h
    m_pixelColData = New Pixel(Width * Height - 1) {}
    For i = 0 To Width * Height - 1
      m_pixelColData(i) = New Pixel(&H0, &H0, &H0, &HFF)
    Next
  End Sub

  Protected Overrides Sub Finalize()
    If m_pixelColData IsNot Nothing Then
      Erase m_pixelColData
    End If
  End Sub

  Public Function LoadFromPGESprFile(imageFile As String, Optional pack As ResourcePack = Nothing) As PixelGameEngine.RCode

    If m_pixelColData IsNot Nothing Then
      Erase m_pixelColData
    End If

    Dim ReadData As Action(Of Stream) = Sub(iis As Stream)
                                          Dim bytes = New Byte(3) {}
                                          iis.Read(bytes, 0, 4)
                                          Width = BitConverter.ToInt32(bytes, 0)
                                          iis.Read(bytes, 0, 4)
                                          Height = BitConverter.ToInt32(bytes, 0)
                                          m_pixelColData = New Pixel(Width * Height - 1) {}
                                          bytes = New Byte(Width * Height * 4 - 1) {}
                                          iis.Read(bytes, 0, Width * Height * 4)
                                          Buffer.BlockCopy(bytes, 0, m_pixelColData, 0, Width * Height * 4)
                                        End Sub

    If pack Is Nothing Then
      Using ifs = New FileStream(imageFile, FileMode.Open, FileAccess.Read)
        If ifs IsNot Nothing Then
          ReadData(ifs)
          Return PixelGameEngine.RCode.Ok
        Else
          Return PixelGameEngine.RCode.Fail
        End If
      End Using
    Else
      Dim rb = pack.GetFileBuffer(imageFile)
      Dim iss = New MemoryStream(rb.Data)
      ReadData(iss)
    End If

    Return PixelGameEngine.RCode.Fail

  End Function

  Public Function SaveToPGESprFile(imageFile As String) As PixelGameEngine.RCode

    If m_pixelColData Is Nothing Then
      Return PixelGameEngine.RCode.Fail
    End If

    Using ofs = New FileStream(imageFile, FileMode.Create, FileAccess.Write)
      ofs.Write(BitConverter.GetBytes(Width), 0, 4)
      ofs.Write(BitConverter.GetBytes(Height), 0, 4)
      Dim bytes = New Byte(Width * Height * 4 - 1) {}
      Buffer.BlockCopy(m_pixelColData, 0, bytes, 0, Width * Height * 4)
      ofs.Write(bytes, 0, Width * Height * 4)
    End Using

    Return PixelGameEngine.RCode.Ok

  End Function

  Function LoadFromFile(imageFile As String, Optional pack As ResourcePack = Nothing) As PixelGameEngine.RCode

    If String.IsNullOrWhiteSpace(imageFile) Then Return PixelGameEngine.RCode.Ok

    Dim bmp As Bitmap = Nothing

    If OperatingSystem.IsWindowsVersionAtLeast(7) Then ' IsOSPlatform(Windows) Then
      If pack IsNot Nothing Then
        ' Load sprite from input stream
        Dim rb = pack.GetFileBuffer(imageFile)
        Dim ms = New MemoryStream(rb.vMemory.ToArray())
        'bmp = DirectCast(Bitmap.FromStream(ms), Bitmap)
        bmp = DirectCast(Bitmap.FromStream(ms), Bitmap)
      Else
        ' Load sprite from file
        bmp = DirectCast(Bitmap.FromFile(imageFile), Bitmap)
      End If
    End If

    If bmp Is Nothing Then Return PixelGameEngine.RCode.NoFile
    If OperatingSystem.IsWindowsVersionAtLeast(7) Then
      Width = bmp.Width
      Height = bmp.Height
    End If
    m_pixelColData = New Pixel(Width * Height - 1) {}

    For x = 0 To Width - 1
      For y = 0 To Height - 1
        Dim c = Drawing.Color.Black
        If OperatingSystem.IsWindowsVersionAtLeast(7) Then
          c = bmp.GetPixel(x, y)
        End If
        SetPixel(x, y, New Pixel(c.R, c.G, c.B, c.A))
      Next
    Next

    If OperatingSystem.IsWindowsVersionAtLeast(7) Then
      bmp.Dispose()
    End If

    Return PixelGameEngine.RCode.Ok

  End Function

  Public Function SetPixel(x As Integer, y As Integer, p As Pixel) As Boolean
    If x >= 0 AndAlso x < Width AndAlso y >= 0 AndAlso y < Height Then
      If OperatingSystem.IsLinux Then Dim t = p.R : p.R = p.B : p.B = t
      m_pixelColData(y * Width + x) = p
      Return True
    Else
      Return False
    End If
  End Function

  Public Function GetPixel(x As Integer, y As Integer) As Pixel
    If m_modeSample = Mode.Normal Then
      If x >= 0 AndAlso x < Width AndAlso y >= 0 AndAlso y < Height Then
        Dim p = m_pixelColData(y * Width + x)
        If OperatingSystem.IsLinux Then Dim t = p.R : p.R = p.B : p.B = t
        Return p
      Else
        Return New Pixel(0, 0, 0, &HFF)
      End If
    Else
      Dim p = m_pixelColData(Math.Abs(y Mod Height) * Width + Math.Abs(x Mod Width))
      If OperatingSystem.IsLinux Then Dim t = p.R : p.R = p.B : p.B = t
      Return p
    End If
  End Function

  Public Function Sample(x As Single, y As Single) As Pixel
    Dim sx = Math.Min(CInt(Fix(x * Width)), Width - 1)
    Dim sy = Math.Min(CInt(Fix(y * Height)), Height - 1)
    Return GetPixel(sx, sy)
  End Function

  Public Function SampleBL(u As Single, v As Single) As Pixel

    u = u * Width - 0.5F
    v = v * Height - 0.5F
    Dim x = CInt(Fix(u))
    Dim y = CInt(Fix(v))
    Dim uRatio = u - x
    Dim vRatio = v - y
    Dim uOpposite = 1.0F - uRatio
    Dim vOpposite = 1.0F - vRatio

    Dim p1 = GetPixel(Math.Max(x, 0), Math.Max(y, 0))
    Dim p2 = GetPixel(Math.Min(x + 1, Width - 1), Math.Max(y, 0))
    Dim p3 = GetPixel(Math.Max(x, 0), Math.Min(y + 1, Height - 1))
    Dim p4 = GetPixel(Math.Min(x + 1, Width - 1), Math.Min(y + 1, Height - 1))

    Return New Pixel(CByte((p1.R * uOpposite + p2.R * uRatio) * vOpposite + (p3.R * uOpposite + p4.R * uRatio) * vRatio),
                     CByte((p1.G * uOpposite + p2.G * uRatio) * vOpposite + (p3.G * uOpposite + p4.G * uRatio) * vRatio),
                     CByte((p1.B * uOpposite + p2.B * uRatio) * vOpposite + (p3.B * uOpposite + p4.B * uRatio) * vRatio))

  End Function

  Public ReadOnly Property GetData() As Pixel()
    Get
      Return m_pixelColData
    End Get
  End Property

End Class