Public Class SpecBAS
  Inherits PgeX

  Public Const PI As Single = 3.14159274
  Public Const TAU As Single = 6.28318548

  Private Shared m_ink As Integer = 0
  Private Shared m_paper As Integer = 8
  Private Shared m_stroke As Integer = 1

  Private Shared m_mathMode As MathMode = MathMode.Radians

  Private Enum MathMode
    Radians
    Degrees
    Turns
    Gradians
  End Enum

  Public Shared Sub CLS()
    Pge.Clear(Palette(m_paper))
  End Sub

  Public Shared Sub CLS(ink As Integer)
    Pge.Clear(Palette(ink))
  End Sub

  Public Shared Sub INK(index As Integer)
    m_ink = index Mod 256
  End Sub

  Public Shared Sub PAPER(index As Integer)
    m_paper = index Mod 256
  End Sub

  Public Shared Function SCRH() As Integer
    Return Pge.ScreenHeight
  End Function

  Public Shared Function SCRW() As Integer
    Return Pge.ScreenWidth
  End Function

  Public Shared Sub STROKE(size As Double)
    m_stroke = CInt(Fix(size))
  End Sub

  Public Shared Function RGBTOINT(r As Double, g As Double, b As Double) As Integer
    Dim rr = CInt(r) : If rr < 0 Then rr = 0 Else If rr > 255 Then rr = 255
    Dim gg = CInt(g) : If gg < 0 Then gg = 0 Else If gg > 255 Then gg = 255
    Dim bb = CInt(b) : If bb < 0 Then bb = 0 Else If bb > 255 Then bb = 255
    Return rr << 16 + gg << 8 + bb
  End Function

  Public Shared Sub RADIANS()
    m_mathMode = MathMode.Radians
  End Sub

  Public Shared Sub DEGREES()
    m_mathMode = MathMode.Degrees
  End Sub

  Public Shared Sub TURNS()
    m_mathMode = MathMode.Turns
  End Sub

  Public Shared Sub GRADIANS()
    m_mathMode = MathMode.Gradians
  End Sub

  Public Shared Function DEGTORAD(n As Double) As Double
    Return n * Math.PI / 180
  End Function

  Public Shared Function DEGTORAD(n As Single) As Single
    Return n * MathF.PI / 180
  End Function

  Public Shared Function RADTODEG(n As Double) As Double
    Return n * 180 / Math.PI
  End Function

  Public Shared Function RADTODEG(n As Single) As Single
    Return n * 180 / MathF.PI
  End Function

  Private Shared Function RadToAngle(angle As Single) As Single
    Select Case m_mathMode
      Case MathMode.Degrees : Return RADTODEG(angle) ' Degrees to radians
      Case MathMode.Turns : Return angle / (PI * 2) ' Turns to radians
      Case MathMode.Gradians : Return angle / (PI / 200) ' Gradians to radians
      Case Else ' MathMode.Radians
        Return angle
    End Select
  End Function

  Private Shared Function RadToAngle(angle As Double) As Double
    Select Case m_mathMode
      Case MathMode.Degrees : Return RADTODEG(angle) ' Radians to degrees
      Case MathMode.Turns : Return angle / (PI * 2) ' Radians to turns
      Case MathMode.Gradians : Return angle / (PI / 200) ' Radians to Gradians
      Case Else ' MathMode.Radians
        Return angle
    End Select
  End Function

  Private Shared Function AngleToRad(angle As Single) As Single
    Select Case m_mathMode
      Case MathMode.Degrees : Return DEGTORAD(angle) ' Degrees to radians
      Case MathMode.Turns : Return angle * (PI * 2) ' Turns to radians
      Case MathMode.Gradians : Return angle * (PI / 200) ' Gradians to radians
      Case Else ' MathMode.Radians
        Return angle
    End Select
  End Function

  Private Shared Function AngleToRad(angle As Double) As Double
    Select Case m_mathMode
      Case MathMode.Degrees : Return DEGTORAD(angle) ' Degrees to radians
      Case MathMode.Turns : Return angle * (PI * 2) ' Turns to radians
      Case MathMode.Gradians : Return angle * (PI / 200) ' Gradians to radians
      Case Else ' MathMode.Radians
        Return angle
    End Select
  End Function

  Public Shared Function SIN(n As Single) As Single
    Dim v = AngleToRad(n)
    Return MathF.Sin(v)
  End Function

  Public Shared Function SIN(n As Double) As Double
    Dim v = AngleToRad(n)
    Return Math.Sin(v)
  End Function

  Public Shared Function COS(n As Single) As Single
    Dim v = AngleToRad(n)
    Return MathF.Cos(v)
  End Function

  Public Shared Function COS(n As Double) As Double
    Dim v = AngleToRad(n)
    Return Math.Cos(v)
  End Function

  Public Shared Function CEIL(n As Single) As Integer
    Return CInt(Fix(MathF.Ceiling(n)))
  End Function

  Public Shared Function CEIL(n As Double) As Integer
    Return CInt(Fix(Math.Ceiling(n)))
  End Function

  Public Shared Function ATN(n As Single) As Single
    Dim v = AngleToRad(n)
    Return MathF.Atan(v)
  End Function

  Public Shared Function ATN(n As Double) As Double
    Dim v = AngleToRad(n)
    Return Math.Atan(v)
  End Function

  Public Shared Function ASN(n As Single) As Single
    Dim v = AngleToRad(n)
    Return MathF.Asin(v)
  End Function

  Public Shared Function ASN(n As Double) As Double
    Dim v = AngleToRad(n)
    Return Math.Asin(v)
  End Function

  Public Shared Function TAN(n As Single) As Single
    Dim v = AngleToRad(n)
    Return MathF.Tan(v)
  End Function

  Public Shared Function TAN(n As Double) As Double
    Dim v = AngleToRad(n)
    Return Math.Tan(v)
  End Function

  Public Shared Function SQR(n As Single) As Single
    Return MathF.Sqrt(n)
  End Function

  Public Shared Function SQR(n As Double) As Double
    Return Math.Sqrt(n)
  End Function

  Public Shared Function ABS(n As Double) As Double
    Return Math.Abs(n)
  End Function

  Public Shared Function ABS(n As Single) As Single
    Return Math.Abs(n)
  End Function

  Public Shared Function ABS(n As Short) As Short
    Return Math.Abs(n)
  End Function

  Public Shared Function ABS(n As Integer) As Integer
    Return Math.Abs(n)
  End Function

  Public Shared Function ABS(n As Long) As Long
    Return Math.Abs(n)
  End Function

#Region "DEC"

  Public Shared Sub DEC(ByRef n As Byte, Optional amount As Byte = 1, Optional min As Byte = Byte.MinValue, Optional max As Byte = Byte.MaxValue)
    n += amount
    If n < min Then n = max
  End Sub

  Public Shared Sub DEC(ByRef n As Short, Optional amount As Short = 1, Optional min As Short = Short.MinValue, Optional max As Short = Short.MaxValue)
    n += amount
    If n < min Then n = max
  End Sub

  Public Shared Sub DEC(ByRef n As Integer, Optional amount As Integer = 1, Optional min As Integer = Integer.MinValue, Optional max As Integer = Integer.MaxValue)
    n += amount
    If n < min Then n = max
  End Sub

  Public Shared Sub DEC(ByRef n As Long, Optional amount As Long = 1, Optional min As Long = Long.MinValue, Optional max As Long = Long.MaxValue)
    n += amount
    If n < min Then n = max
  End Sub

  Public Shared Sub DEC(ByRef n As Single, Optional amount As Single = 1, Optional min As Single = Single.MinValue, Optional max As Single = Single.MaxValue)
    n += amount
    If n < min Then n = max
  End Sub

  Public Shared Sub DEC(ByRef n As Double, Optional amount As Double = 1, Optional min As Double = Double.MinValue, Optional max As Double = Double.MaxValue)
    n += amount
    If n < min Then n = max
  End Sub

#End Region

#Region "INC"

  Public Shared Sub INC(ByRef n As Byte, Optional amount As Byte = 1, Optional min As Byte = Byte.MinValue, Optional max As Byte = Byte.MaxValue)
    n += amount
    If n > max Then n = min
  End Sub

  Public Shared Sub INC(ByRef n As Short, Optional amount As Short = 1, Optional min As Short = Short.MinValue, Optional max As Short = Short.MaxValue)
    n += amount
    If n > max Then n = min
  End Sub

  Public Shared Sub INC(ByRef n As Integer, Optional amount As Integer = 1, Optional min As Integer = Integer.MinValue, Optional max As Integer = Integer.MaxValue)
    n += amount
    If n > max Then n = min
  End Sub

  Public Shared Sub INC(ByRef n As Long, Optional amount As Long = 1, Optional min As Long = Long.MinValue, Optional max As Long = Long.MaxValue)
    n += amount
    If n > max Then n = min
  End Sub

  Public Shared Sub INC(ByRef n As Single, Optional amount As Single = 1, Optional min As Single = Single.MinValue, Optional max As Single = Single.MaxValue)
    n += amount
    If n > max Then n = min
  End Sub

  Public Shared Sub INC(ByRef n As Double, Optional amount As Double = 1, Optional min As Double = Double.MinValue, Optional max As Double = Double.MaxValue)
    n += amount
    If n > max Then n = min
  End Sub

#End Region

  Public Shared ReadOnly Palette As New List(Of Pixel) From {New Pixel(&H0, &H0, &H0, &HFF), ' Black
                                                             New Pixel(&H0, &H0, &HB0, &HFF), ' Blue
                                                             New Pixel(&HB0, &H0, &H0, &HFF), ' Red
                                                             New Pixel(&HB0, &H0, &HB0, &HFF), ' Magenta
                                                             New Pixel(&HB0, &HB0, &H0, &HFF), ' Green
                                                             New Pixel(&H0, &HB0, &HB0, &HFF), ' Cyan
                                                             New Pixel(&HB0, &HB0, &H0, &HFF), ' Yellow
                                                             New Pixel(&HC0, &HC0, &HC0, &HFF), ' White
                                                             New Pixel(&H80, &H80, &H80, &HFF),
                                                             Presets.Blue,
                                                             Presets.Red,
                                                             Presets.Magenta,
                                                             Presets.Green,
                                                             Presets.Cyan,
                                                             Presets.Yellow,
                                                             Presets.White,
                                                             New Pixel() With {.B = &H33, .G = &H0, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H0, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H0, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H0, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H33, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H33, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H33, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H33, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H33, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H33, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H66, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H66, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H66, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H66, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H66, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H66, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H99, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H99, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H99, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H99, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H99, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H99, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HCC, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HCC, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HCC, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HCC, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HCC, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HCC, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HFF, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HFF, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HFF, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HFF, .R = &H0, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H0, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H0, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H0, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H0, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H0, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H0, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H33, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H33, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H33, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H33, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H33, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H33, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H66, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H66, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H66, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H66, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H66, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H66, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H99, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H99, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H99, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H99, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H99, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H99, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HCC, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HCC, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HCC, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HCC, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HCC, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HCC, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HFF, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HFF, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HFF, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HFF, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HFF, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HFF, .R = &H33, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H0, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H0, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H0, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H0, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H0, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H0, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H33, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H33, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H33, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H33, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H33, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H33, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H66, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H66, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H66, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H66, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H66, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H66, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H99, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H99, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H99, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H99, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H99, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H99, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HCC, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HCC, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HCC, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HCC, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HCC, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HCC, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HFF, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HFF, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HFF, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HFF, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HFF, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HFF, .R = &H66, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H0, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H0, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H0, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H0, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H0, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H0, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H33, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H33, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H33, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H33, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H33, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H33, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H66, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H66, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H66, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H66, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H66, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H66, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H99, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H99, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H99, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H99, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H99, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H99, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HCC, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HCC, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HCC, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HCC, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HCC, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HCC, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HFF, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HFF, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HFF, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HFF, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HFF, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HFF, .R = &H99, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H0, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H0, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H0, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H0, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H0, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H0, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H33, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H33, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H33, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H33, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H33, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H33, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H66, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H66, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H66, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H66, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H66, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H66, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H99, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H99, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H99, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H99, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H99, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H99, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HCC, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HCC, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HCC, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HCC, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HCC, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HCC, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HFF, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HFF, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HFF, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HFF, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HFF, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HFF, .R = &HCC, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H0, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H0, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H0, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H0, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H33, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H33, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H33, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H33, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H33, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H33, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H66, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H66, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H66, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H66, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H66, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H66, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H99, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &H99, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &H99, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &H99, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &H99, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &H99, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &HCC, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HCC, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HCC, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HCC, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HCC, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HFF, .G = &HCC, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H33, .G = &HFF, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H66, .G = &HFF, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H99, .G = &HFF, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &HCC, .G = &HFF, .R = &HFF, .A = &HFF},
                                                             New Pixel() With {.B = &H0, .G = &H0, .R = &H0, .A = &HFE},
                                                             New Pixel() With {.B = &H8, .G = &H8, .R = &H8, .A = &HFF},
                                                             New Pixel() With {.B = &H10, .G = &H10, .R = &H10, .A = &HFF},
                                                             New Pixel() With {.B = &H18, .G = &H18, .R = &H18, .A = &HFF},
                                                             New Pixel() With {.B = &H20, .G = &H20, .R = &H20, .A = &HFF},
                                                             New Pixel() With {.B = &H28, .G = &H28, .R = &H28, .A = &HFF},
                                                             New Pixel() With {.B = &H30, .G = &H30, .R = &H30, .A = &HFF},
                                                             New Pixel() With {.B = &H38, .G = &H38, .R = &H38, .A = &HFF},
                                                             New Pixel() With {.B = &H40, .G = &H40, .R = &H40, .A = &HFF},
                                                             New Pixel() With {.B = &H48, .G = &H48, .R = &H48, .A = &HFF},
                                                             New Pixel() With {.B = &H50, .G = &H50, .R = &H50, .A = &HFF},
                                                             New Pixel() With {.B = &H58, .G = &H58, .R = &H58, .A = &HFF},
                                                             New Pixel() With {.B = &H60, .G = &H60, .R = &H60, .A = &HFF},
                                                             New Pixel() With {.B = &H68, .G = &H68, .R = &H68, .A = &HFF},
                                                             New Pixel() With {.B = &H70, .G = &H70, .R = &H70, .A = &HFF},
                                                             New Pixel() With {.B = &H78, .G = &H78, .R = &H78, .A = &HFF},
                                                             New Pixel() With {.B = &H80, .G = &H80, .R = &H80, .A = &HFE},
                                                             New Pixel() With {.B = &H88, .G = &H88, .R = &H88, .A = &HFF},
                                                             New Pixel() With {.B = &H90, .G = &H90, .R = &H90, .A = &HFF},
                                                             New Pixel() With {.B = &H98, .G = &H98, .R = &H98, .A = &HFF},
                                                             New Pixel() With {.B = &HA0, .G = &HA0, .R = &HA0, .A = &HFF},
                                                             New Pixel() With {.B = &HA8, .G = &HA8, .R = &HA8, .A = &HFF},
                                                             New Pixel() With {.B = &HB0, .G = &HB0, .R = &HB0, .A = &HFF},
                                                             New Pixel() With {.B = &HB8, .G = &HB8, .R = &HB8, .A = &HFF},
                                                             New Pixel() With {.B = &HC0, .G = &HC0, .R = &HC0, .A = &HFF},
                                                             New Pixel() With {.B = &HC8, .G = &HC8, .R = &HC8, .A = &HFF},
                                                             New Pixel() With {.B = &HD0, .G = &HD0, .R = &HD0, .A = &HFF},
                                                             New Pixel() With {.B = &HD8, .G = &HD8, .R = &HD8, .A = &HFF},
                                                             New Pixel() With {.B = &HE0, .G = &HE0, .R = &HE0, .A = &HFF},
                                                             New Pixel() With {.B = &HE8, .G = &HE8, .R = &HE8, .A = &HFF},
                                                             New Pixel() With {.B = &HF0, .G = &HF0, .R = &HF0, .A = &HFF},
                                                             New Pixel() With {.B = &HF8, .G = &HF8, .R = &HF8, .A = &HFF}}

  Private Shared Function Polyterm(x As Single, p As Single, q As Single, r As Single, n As Integer) As Single

    Dim acc = 0!
    Dim mul = 1.0!

    For s = 1 To n - 1
      Dim harm = MathF.Log(s)
      Dim zeta = If(r = 0, 1, MathF.Exp(harm * r))
      harm = If(p = 0, 1, MathF.Exp(harm * p))
      mul = mul * x * harm
      acc += mul * zeta * MathF.Cos(q * (s - 1))
    Next

    Return acc

  End Function

  Private Shared Function Under(x As Single, p As Single, q As Single, r As Single, n As Integer) As Single

    Dim acc = 0!
    For s = 1 To n - 1
      Dim harm = x / s
      acc = acc + harm + Polyterm(harm, p, q, r, n)
    Next

    Return Polyterm(acc, r, q, p, n)

  End Function

  'TODO: SP_CompSimpson

  Private Shared Function Mandel(x As Single, y As Single, MaxIters As Integer) As Integer

    Dim p2y = y * y
    Dim q = ((x - 0.25!) * (x - 0.25!)) + p2y
    If (((x + 1) * (x + 1)) + p2y < 0.0625) Or (q * (q + (x - 0.25)) < p2y / 4) Then
      Return 0
    End If

    Dim zr = x
    Dim zi = y
    Dim p = 0
    Dim ptot = 8

    Do

      Dim ckr = zr
      Dim cki = zi
      ptot += ptot
      If ptot > MaxIters Then ptot = MaxIters

      Do
        p += 1
        Dim tmp = (zr * zr) - (zi * zi) + x
        zi = (zi * 2 * zr) + y
        zr = tmp

        If (zr * zr) + (zi * zi) > 4 Then
          Return p
        ElseIf (zr = ckr) And (zi = cki) Then
          Return 0
        End If

      Loop Until p >= ptot

    Loop Until ptot = MaxIters

    Return 0

  End Function

  ' Perlin noise

  Private Shared ReadOnly p2 As New List(Of Integer) _
    From {151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23,
          190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
          77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
          135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
          223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228,
          251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
          138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180, 151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23,
          190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166,
          77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196,
          135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42,
          223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228,
          251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254,
          138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180}

  Private Shared Function Lerp(a As Single, b As Single, x As Single) As Single
    Return a + x * (b - a)
  End Function

  Private Shared Function Fade(t As Single) As Single
    Return t * t * t * (t * (t * 6 - 15) + 10)
  End Function

  Private Shared Function Grad(hash As Integer, x As Single, y As Single, z As Single) As Single

    Select Case hash And &HF
      Case 0 : Return x + y
      Case 1 : Return -x + y
      Case 2 : Return x - y
      Case 3 : Return -x - y
      Case 4 : Return x + z
      Case 5 : Return -x + z
      Case 6 : Return x - z
      Case 7 : Return -x - z
      Case 8 : Return y + z
      Case 9 : Return -y + z
      Case &HA : Return y - z
      Case &HB : Return -y - z
      Case &HC : Return y + x
      Case &HD : Return -y + z
      Case &HE : Return y - x
      Case &HF : Return -y - z
      Case Else : Return 0
    End Select

  End Function

  Public Shared Function NOISE(x As Single, y As Single, z As Single) As Single
    ' Internally calls Perlin, but the keyword is NOISE

    x = MathF.Abs(x)
    y = MathF.Abs(y)
    z = MathF.Abs(z)

    Dim t = CInt(MathF.Truncate(x))
    Dim xi = t And &HFF
    Dim xf = x - t

    t = CInt(MathF.Truncate(y))
    Dim yi = t And &HFF
    Dim yf = y - t

    t = CInt(Math.Truncate(z))
    Dim zi = t And &HFF
    Dim zf = z - t

    Dim u = Fade(xf)
    Dim v = Fade(yf)
    Dim w = Fade(zf)

    Dim p = p2(xi)
    Dim aaa = p2(p2(p + yi) + zi)
    Dim aba = p2(p2(p + yi + 1) + zi)
    Dim aab = p2(p2(p + yi) + zi + 1)
    Dim abb = p2(p2(p + yi + 1) + zi + 1)

    p = p2(xi + 1)
    Dim baa = p2(p2(p + yi) + zi)
    Dim bba = p2(p2(p + yi + 1) + zi)
    Dim bab = p2(p2(p + yi) + zi + 1)
    Dim bbb = p2(p2(p + yi + 1) + zi + 1)

    Dim x1 = Lerp(Grad(aaa, xf, yf, zf), Grad(baa, xf - 1, yf, zf), u)
    Dim x2 = Lerp(Grad(aba, xf, yf - 1, zf), Grad(bba, xf - 1, yf - 1, zf), u)
    Dim y1 = Lerp(x1, x2, v)
    x1 = Lerp(Grad(aab, xf, yf, zf - 1), Grad(bab, xf - 1, yf, zf - 1), u)
    x2 = Lerp(Grad(abb, xf, yf - 1, zf - 1), Grad(bbb, xf - 1, yf - 1, zf - 1), u)
    Dim y2 = Lerp(x1, x2, v)

    Return Lerp(y1, y2, w) + 0.5!

  End Function

  Public Shared Function OCTNOISE(x As Single, y As Single, z As Single, octaves As Integer, persistence As Single) As Single
    ' Internally calls OctavePerlin, but the keyword is OCTNOISE

    Dim total = 0!
    Dim frequency = 1.0!
    Dim amplitude = 1.0!
    Dim maxvalue = 0!

    For i = 0 To octaves - 1
      total += NOISE(x * frequency, y * frequency, z * frequency) * amplitude
      maxvalue += amplitude
      amplitude *= persistence
      frequency *= 2
    Next

    Return total / maxvalue

  End Function

  ' -----

  ''' <summary>
  ''' MAP(n,a1,a2,b1,b2) where a and b are ranges, n gets mapped from range a to range b
  ''' </summary>
  ''' <param name="n"></param>
  ''' <param name="rMin"></param>
  ''' <param name="rMax"></param>
  ''' <param name="mMin"></param>
  ''' <param name="mMax"></param>
  ''' <returns></returns>
  Public Shared Function MAP(n As Single, rMin As Single, rMax As Single, mMin As Single, mMax As Single) As Single
    Return (((n - rMin) / (rMax - rMin)) * (mMax - mMin)) + mMin
  End Function

  Public Shared Sub PLOT(x As Double, y As Double)
    Pge.Draw(x, y, Palette(m_ink))
  End Sub

  Public Shared Sub PLOT(ink As Double, x As Double, y As Double)
    Dim c = CInt(ink) Mod 256
    Pge.Draw(x, y, Palette(c))
  End Sub

  Public Shared Sub PLOT(a(,) As Double)
    For i = 0 To UBound(a, 1)
      Dim x = a(i, 1), y = a(i, 2)
      Dim c = If(UBound(a, 2) > 2, CInt(a(i, 3)) Mod 256, m_ink)
      Pge.Draw(x, y, Palette(c))
    Next
  End Sub

  Public Shared Sub MAT(target(,) As Integer, source(,) As Integer)
    For a = 0 To UBound(target, 1)
      For b = 0 To UBound(target, 2)
        target(a, b) = source(a, b)
      Next
    Next
  End Sub

  Public Shared Sub MAT(target(,) As Long, source(,) As Long)
    For a = 0 To UBound(target, 1)
      For b = 0 To UBound(target, 2)
        target(a, b) = source(a, b)
      Next
    Next
  End Sub

  Public Shared Sub MAT(target(,) As Single, source(,) As Single)
    For a = 0 To UBound(target, 1)
      For b = 0 To UBound(target, 2)
        target(a, b) = source(a, b)
      Next
    Next
  End Sub

  Public Shared Sub MAT(target(,) As Double, source(,) As Double)
    For a = 0 To UBound(target, 1)
      For b = 0 To UBound(target, 2)
        target(a, b) = source(a, b)
      Next
    Next
  End Sub

  Public Shared Function MID(a As Single, b As Single, c As Single) As Single
    Return MathF.Max(MathF.Min(a, b), MathF.Min(MathF.Max(a, b), c))
  End Function

  Public Shared Function MID(a As Double, b As Double, c As Double) As Double
    Return Math.Max(Math.Min(a, b), Math.Min(Math.Max(a, b), c))
  End Function

  Public Shared Function MSECS() As Integer
    Return CInt(Fix((Microsoft.VisualBasic.DateAndTime.Timer * 1000)))
  End Function

End Class