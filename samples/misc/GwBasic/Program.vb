Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New GwBasic
    If demo.Construct(640, 400, 2, 2) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class GwBasic
  Inherits PixelGameEngine

  Private m_t As Single

  Friend Sub New()
    AppName = "GW-BASIC?"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime

    Clear(Presets.Black)

    QPrintRC("GW-BASIC 3.23", 1, 1, Presets.Gray, Presets.Black)
    QPrintRC("(C) Copyright Microsoft 1983,1984,1985,1986,1987,1988", 2, 1, Presets.Gray, Presets.Black)
    QPrintRC("60300 Bytes free", 3, 1, Presets.Gray, Presets.Black)
    QPrintRC("Ok", 4, 1, Presets.Gray, Presets.Black)

    QPrintRC("1LIST   2RUN    3LOAD""  4SAVE""  5CONT   6,""LPT1 7TRON   8TROFF  9KEY    0SCREEN ", 25, 1, Presets.Gray, Presets.Black)
    QPrintRC("LIST", 25, 2, Presets.Black, Presets.Gray)
    QPrintRC("RUN" & ChrW(27), 25, 10, Presets.Black, Presets.Gray)
    QPrintRC("LOAD" & ChrW(27), 25, 18, Presets.Black, Presets.Gray)
    QPrintRC("SAVE" & ChrW(34), 25, 26, Presets.Black, Presets.Gray)
    QPrintRC("CONT" & ChrW(27), 25, 34, Presets.Black, Presets.Gray)
    QPrintRC("," & ChrW(34) & "LPT1", 25, 42, Presets.Black, Presets.Gray)
    QPrintRC("TRON" & ChrW(27), 25, 50, Presets.Black, Presets.Gray)
    QPrintRC("TROFF" & ChrW(27), 25, 58, Presets.Black, Presets.Gray)
    QPrintRC("KEY", 25, 66, Presets.Black, Presets.Gray)
    QPrintRC("SCREEN", 25, 74, Presets.Black, Presets.Gray)

    If CInt(Fix(m_t * 8)) Mod 2 = 0 Then
      DrawLine(0, 72, 8, 72, Presets.Gray)
    End If

    Return True

  End Function

  Private Sub QPrintRC(text As String, row As Integer, col As Integer, fg As Pixel, bg As Pixel)
    Dim charHeight = 16
    Dim x = (col * 8) - 8
    Dim y = (row * charHeight) - charHeight
    For Each c In text
      Dim map = CharMap.CharMap(charHeight, c)
      For cr = 0 To charHeight - 1
        Draw(x + 0, y + cr, If((map(cr) And 1) > 0, fg, bg))
        Draw(x + 1, y + cr, If((map(cr) And 2) > 0, fg, bg))
        Draw(x + 2, y + cr, If((map(cr) And 4) > 0, fg, bg))
        Draw(x + 3, y + cr, If((map(cr) And 8) > 0, fg, bg))
        Draw(x + 4, y + cr, If((map(cr) And 16) > 0, fg, bg))
        Draw(x + 5, y + cr, If((map(cr) And 32) > 0, fg, bg))
        Draw(x + 6, y + cr, If((map(cr) And 64) > 0, fg, bg))
        Draw(x + 7, y + cr, If((map(cr) And 128) > 0, fg, bg))
      Next
      x += 8
    Next

  End Sub

End Class