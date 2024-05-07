Imports VbPixelGameEngine

'REM "A colorful level curve chart." by Luis Alberto Migliorero
'REM https://www.facebook.com/groups/2057165187928233/permalink/3587919178186152/?mibextid=uJjRxr
'SCREEN 12
'F3: LOCATE 1: PRINT "3D Graphic";
'FOR X = -300 TO 300 STEP 6
'    FOR Y = -300 TO 300 STEP 2
'        GOSUB L3
'        NY = -Y * 1 + X * 1 * .5 + 290: NZ = Z * 1 + X * 1 * .7 + 190
'        LINE (NY, NZ)-(NY, 480), 0
'        IF Y = -300 THEN
'            col = ABS(Z) * 31 / 80 + 1
'            PSET (NY, NZ), col
'        ELSE
'            col = ABS(Z) * 31 / 80 + 1
'            LINE (PY, PZ)-(NY, NZ), col
'        END IF
'        PY = NY: PZ = NZ
'    NEXT Y
'NEXT X
'END
'L3: 'Function Z(X,Y) for the figure WAVES
'A$ = INKEY$
'IF A$ = "Q" OR A$ = "q" THEN END
'Z = 100 * ABS(X) ^ .5 * Y / ((Y + 1) ^ 2 + 30) * SIN(.08 * ((X ^ 2 + Y ^ 2) ^ .5))
'CONTINUAR:
'RETURN

Friend Module Program

  Sub Main()
    Dim game As New Waves
    If game.Construct(640, 480, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Waves
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Colorful Waves"
  End Sub

  Private m_t As Single
  Private ReadOnly m_delay As Single = 1 / 60.0!

  Protected Overrides Function OnUserCreate() As Boolean

    Clear()

    Dim py As Integer
    Dim pz As Integer

    For x = -300 To 300 Step 6
      For y = -300 To 300 Step 2
        Dim z = CSng(100.0! * MathF.Abs(x) ^ 0.5! * y / ((y + 1.0!) ^ 2.0! + 30.0!) * MathF.Sin(CSng(0.08! * ((x ^ 2.0! + y ^ 2.0!) ^ 0.5!))))
        Dim ny = CInt(Fix(-y * 1 + x * 1 * 0.5 + 290))
        Dim nz = CInt(Fix(z * 1 + x * 1 * 0.7 + 190))
        DrawLine(ny, nz, ny, 480, Presets.Black)
        Dim col = MathF.Abs(z) * 31 / 80 + 1
        Dim p As Pixel = Nothing
        Select Case CInt(Fix(col)) Mod 15
          Case 0 : p = Presets.Black
          Case 1 : p = Presets.DarkBlue
          Case 2 : p = Presets.DarkGreen
          Case 3 : p = Presets.DarkCyan
          Case 4 : p = Presets.DarkRed
          Case 5 : p = Presets.DarkMagenta
          Case 6 : p = Presets.Brown
          Case 7 : p = Presets.DarkGrey
          Case 8 : p = Presets.Gray
          Case 9 : p = Presets.Blue
          Case 10 : p = Presets.Green
          Case 11 : p = Presets.Cyan
          Case 12 : p = Presets.Red
          Case 13 : p = Presets.Magenta
          Case 14 : p = Presets.Yellow
          Case 15 : p = Presets.White
          Case Else
            Stop
        End Select
        If y = -300 Then
          Draw(ny, nz, p)
        Else
          DrawLine(py, pz, ny, nz, p)
        End If
        py = ny : pz = nz
      Next
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime : If m_t < m_delay Then Return True Else m_t -= m_delay

    Return True

  End Function

End Class