' BASIC Code(optimized by Csabo And me)
' 1 REM LOXODROME
' 2 COLOR4,1:COLOR.,1:COLOR1, 6, 4: GRAPHIC1, 1
' 3 B=-1.2:W = 0.006 : D = 0.6 : SD = SIN(D) : CD = COS(D) : CB = COS(B)
' 4 FORI=.TO80STEP.1:B = B + W * CB
' 5 SB=SIN(B):CB = COS(B) : CI = COS(I) * CB
' 6 X=160+96*SIN(I)*CB
' 7 Y=96*(1-SB*CD-CI*SD)
' 8 Z=SB*SD-CI*CD
' 9 IFZ>.THENDRAW1TOX,Y:ELSEDRAW1, X, Y
' 10 NEXT

' Originally for the Commodore Plus/4

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Loxodrome
    If game.Construct(400, 280, 2, 2, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Loxodrome
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Loxodrome"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Dim ox = 40, oy = 40
    Dim b = -1.2, w = 0.006, d = 0.6, sd = Sin(d), cd = Cos(d), cb = Cos(b)
    Dim green = New Pixel(0, 239, 130)

    Dim cx = 1.0, cy = 1.0
    For i = 0 To 80 Step 0.1
      b += w * cb
      Dim sb = Sin(b) : cb = Cos(b) : Dim ci = Cos(i) * cb
      Dim x = 160 + 96 * Sin(i) * cb
      Dim y = 96 * (1 - sb * cd - ci * sd)
      Dim z = sb * sd - ci * cd
      If z > 0 Then DrawLine(ox + cx, oy + cy, ox + x, oy + y, green) Else Draw(ox + x, oy + y, green)
      cx = x : cy = y
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Return True

  End Function

End Class