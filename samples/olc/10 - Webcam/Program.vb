Imports VbPixelGameEngine
Imports VbPixelGameEngine.PixelType
Imports VbPixelGameEngine.Color
Imports OpenCvSharp

Module Program

  Sub Main()
    Dim game As New Webcam
    If game.Construct(320, 240, 4, 4) Then
      game.Start()
    End If
  End Sub

End Module

Class Webcam
  Inherits PixelGameEngine

  Private m_capture As VideoCapture
  Private m_frame As Mat

  Sub New()
    AppName = "Webcam"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Console.Title = "Initializing MAT"
    m_frame = New Mat()
    Console.Title = "Initializing VideoCapture"
    m_capture = New VideoCapture(0)
    Console.Title = "Configuring VideoCapture"
    m_capture.Set(VideoCaptureProperties.FrameWidth, 640) 'ScreenWidth)
    m_capture.Set(VideoCaptureProperties.FrameHeight, 480) 'ScreenHeight)
    m_capture.Set(VideoCaptureProperties.Zoom, 0)
    'm_capture.FrameHeight = ScreenHeight()
    'm_capture.FrameWidth = ScreenWidth()
    Console.Title = "Opening VideoCapture"
    m_capture?.Open(0)
    Console.Title = "Startup completed..."
    Return True

  End Function

  'Private Shared Function MatToImageArray(mat As Mat) As Integer()
  '  Dim w = mat.Width
  '  Dim h = mat.Height
  '  Dim imageData = mat.ToBytes(".bmp")
  '  Dim paddingSize = (4 - ((w * 3) Mod 4)) Mod 4
  '  Dim rgbData((w * h) - 1) As Integer
  '  Dim index = 0
  '  For y = h - 1 To 0 Step -1
  '    For x = 0 To w - 1
  '      Dim pixelIndex = (y * (w * 3 + paddingSize)) + (x * 3)
  '      Dim b = imageData(pixelIndex)
  '      Dim g = imageData(pixelIndex + 1)
  '      Dim r = imageData(pixelIndex + 2)
  '      Dim rgb = (r << 16) Or (g << 8) Or b
  '      rgbData(index) = rgb : index += 1
  '    Next
  '  Next
  '  Return rgbData
  'End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If m_capture?.IsOpened() Then

      Dim sw = ScreenWidth()
      Dim sh = ScreenHeight()

      m_capture.Read(m_frame)

      Cv2.Resize(m_frame, m_frame, New Size(sw, sh))
      Cv2.Flip(m_frame, m_frame, FlipMode.Y)

      Dim indexer = m_frame.GetGenericIndexer(Of Vec3b)

      'Dim mat3 As New Mat(Of Vec3b)(m_frame) ' not working, and don't understand why
      'Dim indexer = mat3.GetIndexer()

      Dim fw = m_frame.Width
      Dim fh = m_frame.Height

      For x = 0 To fw - 1 'sw - 1
        For y = 0 To fh - 1 'sh - 1

          ' Get Pixel

          Dim color = indexer(y, x)
          Dim r = color.Item2 '/ 255.0F
          Dim g = color.Item1 '/ 255.0F
          Dim b = color.Item0 '/ 255.0F

          ' Convert into char / color combo 

          'Dim sym = 0
          'Dim Bgcol = 0
          'Dim Fgcol = 0

          'ClassifyGray(r, g, b, sym, Bgcol, Fgcol)
          'ClassifyHSL(r, g, b, sym, Bgcol, Fgcol)
          'ClassifyOLC(r, g, b, sym, Bgcol, Fgcol)

          ' Draw Pixel
          'Draw(x, y, sym, Bgcol Or Fgcol)
          Draw(x, y, New Pixel(r, g, b)) 'Bgcol Or Fgcol)

        Next
      Next
    End If

    Return True

  End Function

  Private Shared Sub ClassifyGray(r As Single, g As Single, b As Single, ByRef ch As Integer, ByRef fg As Integer, ByRef bg As Integer)

    Dim luminance = 0.2987 * r + 0.587 * g + 0.114 * b
    Dim bw = CInt(Fix(luminance * 13))
    Select Case bw
      Case 0 : bg = BgBlack : fg = FgBlack : ch = Solid

      Case 1 : bg = BgBlack : fg = FgDarkGray : ch = Quarter
      Case 2 : bg = BgBlack : fg = FgDarkGray : ch = Half
      Case 3 : bg = BgBlack : fg = FgDarkGray : ch = ThreeQuarters
      Case 4 : bg = BgBlack : fg = FgDarkGray : ch = Solid

      Case 5 : bg = BgGray : fg = FgGray : ch = Quarter
      Case 6 : bg = BgGray : fg = FgGray : ch = Half
      Case 7 : bg = BgGray : fg = FgGray : ch = ThreeQuarters
      Case 8 : bg = BgGray : fg = FgGray : ch = Solid

      Case 9 : bg = BgGray : fg = FgWhite : ch = Quarter
      Case 10 : bg = BgGray : fg = FgWhite : ch = Half
      Case 11 : bg = BgGray : fg = FgWhite : ch = ThreeQuarters
      Case 12 : bg = BgGray : fg = FgWhite : ch = Solid

    End Select

  End Sub

  ' Define RGB class
  Private Class RGB
    Public r As Single    ' a fraction between 0 and 1
    Public g As Single    ' a fraction between 0 and 1
    Public b As Single    ' a fraction between 0 and
    Public Sub New()
    End Sub
    Public Sub New(r As Single, g As Single, b As Single)
      Me.r = r
      Me.g = g
      Me.b = b
    End Sub
  End Class

  ' Define HSV class
  Private Class HSV
    Public h As Single    ' angle in degrees
    Public s As Single    ' a fraction between 0 and 1
    Public v As Single    ' a fraction between 0 and 1
  End Class

  ' Define Rgb2hsv function
  Private Shared Function Rgb2hsv(inRgb As RGB) As HSV

    Dim outHsv As New HSV()
    Dim minVal As Single, maxVal As Single, delta As Single

    minVal = Math.Min(inRgb.r, Math.Min(inRgb.g, inRgb.b))
    maxVal = Math.Max(inRgb.r, Math.Max(inRgb.g, inRgb.b))

    outHsv.v = maxVal                               ' v
    delta = maxVal - minVal
    If delta < 0.00001F Then
      outHsv.s = 0
      outHsv.h = 0 ' undefined, maybe nan?
      Return outHsv
    End If
    If maxVal > 0.0 Then
      outHsv.s = (delta / maxVal)                  ' s
    Else
      ' if maxVal is 0, then r = g = b = 0              
      ' s = 0, h is undefined
      outHsv.s = 0.0
      outHsv.h = Single.NaN                         ' its now undefined
      Return outHsv
    End If
    If inRgb.r >= maxVal Then                           ' > is bogus, just keeps compiler happy
      outHsv.h = (inRgb.g - inRgb.b) / delta          ' between yellow & magenta
    ElseIf inRgb.g >= maxVal Then
      outHsv.h = 2.0F + (inRgb.b - inRgb.r) / delta   ' between cyan & yellow
    Else
      outHsv.h = 4.0F + (inRgb.r - inRgb.g) / delta   ' between magenta & cyan
    End If

    outHsv.h *= 60.0F                                 ' degrees

    If outHsv.h < 0.0F Then
      outHsv.h += 360.0F
    End If

    Return outHsv

  End Function

  Private Shared Sub ClassifyHSL(r As Single, g As Single, b As Single, ByRef sym As Integer, ByRef Fgcol As Integer, ByRef Bgcol As Integer)

    Dim col As HSV = Rgb2hsv(New RGB(r, g, b))

    Dim hues() As (c As Integer, fg As Integer, bg As Integer) =
    {
        (Solid, FgRed, BgRed),
        (Quarter, FgYellow, BgRed),
        (Half, FgYellow, BgRed),
        (ThreeQuarters, FgYellow, BgRed),
        (Solid, FgGreen, BgYellow),
        (Quarter, FgGreen, BgYellow),
        (Half, FgGreen, BgYellow),
        (ThreeQuarters, FgGreen, BgYellow),
        (Solid, FgCyan, BgGreen),
        (Quarter, FgCyan, BgGreen),
        (Half, FgCyan, BgGreen),
        (ThreeQuarters, FgCyan, BgGreen),
        (Solid, FgBlue, BgCyan),
        (Quarter, FgBlue, BgCyan),
        (Half, FgBlue, BgCyan),
        (ThreeQuarters, FgBlue, BgCyan),
        (Solid, FgMagenta, BgBlue),
        (Quarter, FgMagenta, BgBlue),
        (Half, FgMagenta, BgBlue),
        (ThreeQuarters, FgMagenta, BgBlue),
        (Solid, FgRed, BgMagenta),
        (Quarter, FgRed, BgMagenta),
        (Half, FgRed, BgMagenta),
        (ThreeQuarters, FgRed, BgMagenta)
    }

    Dim index = CInt(Fix((col.h / 360.0F) * 24.0F))

    If col.s > 0.2F Then
      sym = hues(index).c
      Fgcol = hues(index).fg
      Bgcol = hues(index).bg
    Else
      ClassifyGray(r, g, b, sym, Fgcol, Bgcol)
    End If

  End Sub

  Shared Sub ClassifyOLC(r As Single, g As Single, b As Single, ByRef sym As Integer, ByRef Fgcol As Integer, ByRef Bgcol As Integer)

    ' Is pixel coloured (i.e. RGB values exhibit significant variance)
    Dim mean = (r + g + b) / 3.0F
    Dim rVar = (r - mean) * (r - mean)
    Dim gVar = (g - mean) * (g - mean)
    Dim bVar = (b - mean) * (b - mean)
    Dim variance = rVar + gVar + bVar

    If variance < 0.0001F Then
      ClassifyGray(r, g, b, sym, Fgcol, Bgcol)
    Else

      ' Pixel has colour so get dominant colour
      Dim y = Math.Min(r, g)
      Dim c = Math.Min(g, b)
      Dim m = Math.Min(b, r)

      Dim mean2 = (y + c + m) / 3.0F
      Dim yVar = (y - mean2) * (y - mean2)
      Dim cVar = (c - mean2) * (c - mean2)
      Dim mVar = (m - mean2) * (m - mean2)

      Dim maxPrimaryVar = Math.Max(rVar, gVar)
      maxPrimaryVar = Math.Max(maxPrimaryVar, bVar)

      Dim maxSecondaryVar = Math.Max(cVar, yVar)
      maxSecondaryVar = Math.Max(maxSecondaryVar, mVar)

      Dim shading = 0.5F

      If rVar = maxPrimaryVar AndAlso yVar = maxSecondaryVar Then
        Compare(rVar, yVar, r, y, FgRed, FgDarkRed, BgYellow, BgDarkYellow, shading, sym, Fgcol, Bgcol)
      End If

      If rVar = maxPrimaryVar AndAlso mVar = maxSecondaryVar Then
        Compare(rVar, mVar, r, m, FgRed, FgDarkRed, BgMagenta, BgDarkMagenta, shading, sym, Fgcol, Bgcol)
      End If

      If rVar = maxPrimaryVar AndAlso cVar = maxSecondaryVar Then
        Compare(rVar, cVar, r, c, FgRed, FgDarkRed, BgCyan, BgDarkCyan, shading, sym, Fgcol, Bgcol)
      End If

      If gVar = maxPrimaryVar AndAlso yVar = maxSecondaryVar Then
        Compare(gVar, yVar, g, y, FgGreen, FgDarkGreen, BgYellow, BgDarkYellow, shading, sym, Fgcol, Bgcol)
      End If

      If gVar = maxPrimaryVar AndAlso cVar = maxSecondaryVar Then
        Compare(gVar, cVar, g, c, FgGreen, FgDarkGreen, BgCyan, BgDarkCyan, shading, sym, Fgcol, Bgcol)
      End If

      If gVar = maxPrimaryVar AndAlso mVar = maxSecondaryVar Then
        Compare(gVar, mVar, g, m, FgGreen, FgDarkGreen, BgMagenta, BgDarkMagenta, shading, sym, Fgcol, Bgcol)
      End If

      If bVar = maxPrimaryVar AndAlso mVar = maxSecondaryVar Then
        Compare(bVar, mVar, b, m, FgBlue, FgDarkBlue, BgMagenta, BgDarkMagenta, shading, sym, Fgcol, Bgcol)
      End If

      If bVar = maxPrimaryVar AndAlso cVar = maxSecondaryVar Then
        Compare(bVar, cVar, b, c, FgBlue, FgDarkBlue, BgCyan, BgDarkCyan, shading, sym, Fgcol, Bgcol)
      End If

      If bVar = maxPrimaryVar AndAlso yVar = maxSecondaryVar Then
        Compare(bVar, yVar, b, y, FgBlue, FgDarkBlue, BgYellow, BgDarkYellow, shading, sym, Fgcol, Bgcol)
      End If

    End If

  End Sub

  Private Shared Sub Compare(fV1 As Single, fV2 As Single, fC1 As Single, fC2 As Single, FgLIGHT As Integer, FgDARK As Integer, BgLIGHT As Integer, BgDARK As Integer,
                             fshading As Single, ByRef sym As Integer, ByRef Fgcol As Integer, ByRef Bgcol As Integer)
    If fV1 >= fV2 Then
      'Primary Is Dominant, Use in foreground
      Fgcol = If(fC1 > 0.5F, FgLIGHT, FgDARK)

      If fV2 < 0.0001F Then
        'Secondary is not variant, Use Grayscale in background
        If fC2 >= 0.00F AndAlso fC2 < 0.25F Then
          Bgcol = BgBlack
        End If
        If fC2 >= 0.25F AndAlso fC2 < 0.5F Then
          Bgcol = BgDarkGray
        End If
        If fC2 >= 0.5F AndAlso fC2 < 0.75F Then
          Bgcol = BgGray
        End If
        If fC2 >= 0.75F AndAlso fC2 <= 1.0F Then
          Bgcol = BgWhite
        End If
      Else
        'Secondary is variant, Use in background
        Bgcol = If(fC2 > 0.5F, BgLIGHT, BgDARK)
      End If

      'Shade dominant over background (100% -> 0%)
      fshading = ((fC1 - fC2) / 2.0F) + 0.5F
    End If

    If fshading >= 0.00F AndAlso fshading < 0.2F Then
      sym = AscW(" "c)
    End If
    If fshading >= 0.2F AndAlso fshading < 0.4F Then
      sym = Quarter
    End If
    If fshading >= 0.4F AndAlso fshading < 0.6F Then
      sym = Half
    End If
    If fshading >= 0.6F AndAlso fshading < 0.8F Then
      sym = ThreeQuarters
    End If
    If fshading >= 0.8F Then
      sym = Solid
    End If
  End Sub

  'Private Shared Function MatToBitmap(mat As Mat) As Bitmap
  '  Using ms = mat.ToMemoryStream()
  '    If OperatingSystem.IsWindows Then
  '      Return CType(Image.FromStream(ms), Bitmap)
  '    Else
  '      Return Nothing
  '    End If
  '  End Using
  'End Function

End Class
