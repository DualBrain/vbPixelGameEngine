Imports System.Math

'TEKTRONIX 4051 by David Emmons
'100 PAGE
'110 SET DEGREES
'120 FOR A=0 TO 3600 STEP 7
'130 X=65+25*COS(A)
'140 Y=50+25*SIN(A)
'150 ROTATE 1.7*A
'160 MOVE X,Y
'170 REDRAW 0,10
'180 NEXT A

'BBC BASIC by Richard Russell 
'100 MODE 0
'110 COLOUR 1,2
'120 FOR A = 0 TO 3600 STEP 7
'130 X = 640 + 250*COSRAD(A)
'140 Y = 512 + 250*SINRAD(A)
'150 MOVE X,Y
'160 DRAW BY 100*COSRAD(1.7*A),100*SINRAD(1.7*A)
'170 NEXT A

'SpecBAS by Paul Dunn
'10 FOR A = 0 TO TAU*10 STEP .1221
'20 X=400+100*COS(A): Y=240+100*SIN(A)
'30 DRAW X,Y TO X+40*COS(1.7*A),Y+40*SIN(1.7*A)
'40 NEXT A

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New Spirograph
    'If demo.Construct(400, 300, 1, 1) Then
    If demo.Construct(800, 480, 1, 1) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class Spirograph
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Spirograph"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Dim bg = New Pixel(7, 36, 18)
    Dim fg = New Pixel(0, 239, 130)

    Clear(bg)

    Dim d = 1.7
    Dim r = 100
    Dim cx = 400
    Dim cy = 240
    Dim v = 40
    If False Then
      ' 'BBC BASIC' conversion
      Dim s = 7
      For a = 0 To 3600 Step s
        Dim x = cx + r * Cos(DegreesToRadians(a))
        Dim y = cy + r * Sin(DegreesToRadians(a))
        DrawLine(x, y, x + v * Cos(DegreesToRadians(d * a)), y + v * Sin(DegreesToRadians(d * a)), fg)
      Next
    Else
      ' 'SpecBAS' conversion
      For a = 0 To (2 * PI) * 10 Step 0.1221
        Dim x = cx + r * Cos(a), y = cy + r * Sin(a)
        DrawLine(x, y, x + v * Cos(d * a), y + v * Sin(d * a), fg)
      Next
    End If

    Return True

  End Function

  Private Shared Function DegreesToRadians(degrees As Double) As Double
    Return degrees * Math.PI / 180
  End Function

End Class