#Disable Warning IDE1006 ' Naming Styles
Public Class QB64
    Inherits PgeX

    Private Shared m_fg As New Pixel(128, 128, 128)
    Private Shared m_bg As New Pixel(0, 0, 0)

    Private Shared m_px As Integer
    Private Shared m_py As Integer

    'TODO: Need to determine actual (default) color palette for QB64 - the below is currently based on SpecBAS.
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

    Public Shared Sub COLOR(value As Double)
        If value > -1 AndAlso value < Palette.Count Then
            m_fg = Palette(CInt(Fix(value)))
        Else
            Stop
        End If
    End Sub

    Public Shared Sub COLOR(value As Integer)
        If value > -1 AndAlso value < Palette.Count Then
            m_fg = Palette(value)
        Else
            Stop
        End If
    End Sub

    Public Shared Sub PSET(x As Double, y As Double)
        Pge.Draw(x, y, m_fg) : m_px = CInt(Fix(x)) : m_py = CInt(Fix(y))
    End Sub

    Public Shared Sub LINE(x1 As Double, y1 As Double, x2 As Double, y2 As Double)
        Pge.DrawLine(x1, y1, x2, y2, m_fg) : m_px = CInt(Fix(x2)) : m_py = CInt(Fix(y2))
    End Sub

    Public Shared Sub LINE(x2 As Double, y2 As Double)
        Pge.DrawLine(m_px, m_py, x2, y2, m_fg) : m_px = CInt(Fix(x2)) : m_py = CInt(Fix(y2))
    End Sub

    Public Shared Function ABS(n As Double) As Double
        Return Math.Abs(n)
    End Function

    Public Shared Function ABS(n As Single) As Single
        Return MathF.Abs(n)
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

    Public Shared Function COS(n As Single) As Single
        Return MathF.Cos(n)
    End Function

    Public Shared Function COS(n As Double) As Double
        Return Math.Cos(n)
    End Function

    Public Shared Function SQR(n As Single) As Single
        Return MathF.Sqrt(n)
    End Function

    Public Shared Function SQR(n As Double) As Double
        Return Math.Sqrt(n)
    End Function

    Public Shared Function _D2R(n As Single) As Single
        Return n * MathF.PI / 180
    End Function

    Public Shared Function _D2R(n As Double) As Double
        Return n * Math.PI / 180
    End Function

    Public Shared Function _PI(muliplier As Single) As Single
        Return MathF.PI * muliplier
    End Function

    Public Shared Function SIN(n As Single) As Single
        Return MathF.Sin(n)
    End Function

    Public Shared Function SIN(n As Double) As Double
        Return Math.Sin(n)
    End Function

End Class
#Enable Warning IDE1006 ' Naming Styles