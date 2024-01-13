Imports VbPixelGameEngine
Imports QB.Video
Imports System.Timers

Friend Module Program

  Sub Main()
    Dim demo As New QBasic
    If demo.Construct(640, 400, 1, 1) Then ', False, True) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class QBasic
  Inherits PixelGameEngine

  Private m_t As Single

  Private WithEvents WorkTimer As New Timer(1)

  Friend Sub New()
    AppName = "QBasic?"
  End Sub

  Private m_workTimerActive As Boolean
  Private Sub WorkTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles WorkTimer.Elapsed

    If m_workTimerActive Then Return
    m_workTimerActive = True
    Try
      Dim value = $"{Now:HH:mm:ss}.{Now.Millisecond:000}"
      QPrintRC(value, 1, 55, 14, 8)
    Finally
      m_workTimerActive = False
    End Try

  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Init()

    Dim bg = 1

    QB.Video.COLOR(8, bg)
    CLS()
    QPrintRC("   File  Edit  View  Search  Run  Debug  Options                          Help  ", 1, 1, 0, 8)

    QPrintRC(ChrW(218), 2, 1, 8, bg)
    QPrintRC(ChrW(191), 2, 80, 8, bg)
    For r = 3 To 24
      QPrintRC(ChrW(179), r, 1, 8, bg)
      QPrintRC(ChrW(179), r, 80, 8, bg)
    Next
    For c = 2 To 79
      QPrintRC(ChrW(196), 2, c, 8, bg)
      QPrintRC(ChrW(196), 22, c, 8, bg)
    Next
    QPrintRC(ChrW(195), 22, 1, 8, bg)
    QPrintRC(ChrW(180), 22, 80, 8, bg)

    QPrintRC(ChrW(180), 2, 76, 8, bg)
    QPrintRC(ChrW(24), 2, 77, bg, 8)
    QPrintRC(ChrW(195), 2, 78, 8, bg)

    QPrintRC(ChrW(24), 3, 80, bg, 8)
    QPrintRC(ChrW(32), 4, 80, 0, 0)
    For r = 5 To 19
      QPrintRC(ChrW(177), r, 80, 0, 8)
    Next
    QPrintRC(ChrW(25), 20, 80, bg, 8)

    QPrintRC(ChrW(27), 21, 2, bg, 8)
    QPrintRC(ChrW(32), 21, 3, 0, 0)
    For c = 4 To 78
      QPrintRC(ChrW(177), 21, c, 0, 8)
    Next
    QPrintRC(ChrW(26), 21, 79, bg, 8)

    QPrintRC(" Untitled ", 2, 35, bg, 8)
    QPrintRC(" Immediate ", 22, 35, 8, bg)

    QPrintRC(" <Shift+F1=Help> <F6=Window> <F2=Subs> <F5=Run> <F8=Step>     ", 25, 1, 15, 3)
    QPrintRC(ChrW(179) & "       00002:001 ", 25, 63, 0, 3)

    LOCATE(3, 2) : PRINT("PRINT ""Hello World!""")
    LOCATE(4, 2)

    WorkTimer.Enabled = True

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime

    If m_invalidated Then
      For r = 0 To 24
        For c = 0 To 79

          Dim index = (r * 80) + c
          Dim ch = CByte(Screen0(index) And &HFF)
          Dim clr = ((Screen0(index) And &HFF00) \ 256) And &HFF
          Dim map = CharMap(m_textH, ch)

          Dim x = c * m_textW
          Dim y = r * m_textH

          Dim fg, bg As Integer
          SplitColor(clr, fg, bg)

          Dim fgc = m_palette(fg)
          Dim bgc = m_palette(bg)

          For dy = 0 To m_textH - 1
            Draw(x + 0, dy + y, If((map(dy) And 1) > 0, fgc, bgc))
            Draw(x + 1, dy + y, If((map(dy) And 2) > 0, fgc, bgc))
            Draw(x + 2, dy + y, If((map(dy) And 4) > 0, fgc, bgc))
            Draw(x + 3, dy + y, If((map(dy) And 8) > 0, fgc, bgc))
            Draw(x + 4, dy + y, If((map(dy) And 16) > 0, fgc, bgc))
            Draw(x + 5, dy + y, If((map(dy) And 32) > 0, fgc, bgc))
            Draw(x + 6, dy + y, If((map(dy) And 64) > 0, fgc, bgc))
            Draw(x + 7, dy + y, If((map(dy) And 128) > 0, fgc, bgc))
          Next

        Next
      Next
    End If

    If CInt(Fix(m_t * 8)) Mod 2 = 0 Then
      Dim x = (m_cursorCol - 1) * m_textW
      Dim y = (m_cursorRow - 1) * m_textH
      DrawLine(x, y + m_textH - 2, x + 7, y + m_textH - 2, Presets.Gray)
      DrawLine(x, y + m_textH - 1, x + 7, y + m_textH - 1, Presets.Gray)
    End If

    Return True

  End Function

  Private Shared Sub QPrintRC(text As String, row As Integer, col As Integer, fg As Integer, bg As Integer)

    Dim rr = m_cursorRow, rc = m_cursorCol
    Dim rfg = m_fgColor, rbg = m_bgColor
    m_cursorRow = row : m_cursorCol = col
    If fg <> m_fgColor OrElse bg <> m_bgColor Then
      m_fgColor = fg : m_bgColor = bg
    End If
    PRINT(text, True)
    If m_fgColor <> rfg OrElse m_bgColor <> rbg Then
      m_fgColor = rfg : m_bgColor = rbg
    End If
    m_cursorRow = rr : m_cursorCol = rc

  End Sub

End Class