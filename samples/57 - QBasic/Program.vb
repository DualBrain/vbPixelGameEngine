Imports VbPixelGameEngine
Imports QB.Video
Imports System.Timers

Friend Module Program

  Sub Main()
    Dim demo As New QBasic
    If demo.Construct(640, 400, 1, 1, False, True) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class QBasic
  Inherits PixelGameEngine

  Private m_t As Single

  Private WithEvents WorkTimer As New Timer(100)

  Friend Sub New()
    AppName = "QBasic?"
  End Sub

  Private Sub WorkTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles WorkTimer.Elapsed

    LOCATE(5, 40)
    'PRINT(Now.ToString("HH:mm:ss"))

    PRINT(Now.Millisecond.ToString)

  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Init(Me)

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

    'Dim statusBackground = New Pixel(0, 170, 170)

    'QPrintRC(" Micrsoft (R) QuickBASIC 4.50 (C) Copyright Microsoft Corporation, 1985-1988    ", 25, 1, 15, 3)
    QPrintRC(" <Shift+F1=Help> <F6=Window> <F2=Subs> <F5=Run> <F8=Step>     " & ChrW(179) & "       00002:001 ", 25, 1, 0, 3)

    QPrintRC("PRINT ""Hello World!""", 3, 2, 8, bg)

    WorkTimer.Enabled = True

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime

    QB.Video.Render()

    'Dim bg = New Pixel(0, 0, 170, 255) 'Presets.DarkBlue

    'Clear(bg)

    'QPrintRC("   File  Edit  View  Search  Run  Debug  Options                          Help  ", 1, 1, Presets.Black, Presets.Gray)

    'QPrintRC(ChrW(218), 2, 1, Presets.Gray, bg)
    'QPrintRC(ChrW(191), 2, 80, Presets.Gray, bg)
    'For r = 3 To 24
    '  QPrintRC(ChrW(179), r, 1, Presets.Gray, bg)
    '  QPrintRC(ChrW(179), r, 80, Presets.Gray, bg)
    'Next
    'For c = 2 To 79
    '  QPrintRC(ChrW(196), 2, c, Presets.Gray, bg)
    '  QPrintRC(ChrW(196), 22, c, Presets.Gray, bg)
    'Next
    'QPrintRC(ChrW(195), 22, 1, Presets.Gray, bg)
    'QPrintRC(ChrW(180), 22, 80, Presets.Gray, bg)

    'QPrintRC(ChrW(180), 2, 76, Presets.Gray, bg)
    'QPrintRC(ChrW(24), 2, 77, bg, Presets.Gray)
    'QPrintRC(ChrW(195), 2, 78, Presets.Gray, bg)

    'QPrintRC(ChrW(24), 3, 80, bg, Presets.Gray)
    'QPrintRC(ChrW(32), 4, 80, Presets.Black, Presets.Black)
    'For r = 5 To 19
    '  QPrintRC(ChrW(177), r, 80, Presets.Black, Presets.Gray)
    'Next
    'QPrintRC(ChrW(25), 20, 80, bg, Presets.Gray)

    'QPrintRC(ChrW(27), 21, 2, bg, Presets.Gray)
    'QPrintRC(ChrW(32), 21, 3, Presets.Black, Presets.Black)
    'For c = 4 To 78
    '  QPrintRC(ChrW(177), 21, c, Presets.Black, Presets.Gray)
    'Next
    'QPrintRC(ChrW(26), 21, 79, bg, Presets.Gray)

    'QPrintRC(" Untitled ", 2, 35, bg, Presets.Gray)
    'QPrintRC(" Immediate ", 22, 35, Presets.Gray, bg)

    'Dim statusBackground = New Pixel(0, 170, 170)

    'QPrintRC(" Micrsoft (R) QuickBASIC 4.50 (C) Copyright Microsoft Corporation, 1985-1988    ", 25, 1, Presets.White, statusBackground)
    ''QPrintRC(" <Shift+F1=Help> <F6=Window> <F2=Subs> <F5=Run> <F8=Step>     " & ChrW(179) & "       00001:001 ", 25, 1, Presets.Black, statusBackground)

    'QPrintRC("PRINT ""Hello World!""", 3, 2, Presets.Gray, bg)

    If CInt(Fix(m_t * 8)) Mod 2 = 0 Then
      DrawLine(8, 63, 16, 63, Presets.Gray)
      DrawLine(8, 64, 16, 64, Presets.Gray)
    End If

    Return True

  End Function

  Private Sub QPrintRC(text As String, row As Integer, col As Integer, fg As Integer, bg As Integer)

    LOCATE(row, col)
    QB.Video.COLOR(fg, bg)
    PRINT(text, True)

    '  Dim charHeight = 16
    '  Dim x = (col * 8) - 8
    '  Dim y = (row * charHeight) - charHeight
    '  For Each c In text
    '    Dim map = CharMap.CharMap(charHeight, c)
    '    For cr = 0 To charHeight - 1
    '      Draw(x + 0, y + cr, If((map(cr) And 1) > 0, fg, bg))
    '      Draw(x + 1, y + cr, If((map(cr) And 2) > 0, fg, bg))
    '      Draw(x + 2, y + cr, If((map(cr) And 4) > 0, fg, bg))
    '      Draw(x + 3, y + cr, If((map(cr) And 8) > 0, fg, bg))
    '      Draw(x + 4, y + cr, If((map(cr) And 16) > 0, fg, bg))
    '      Draw(x + 5, y + cr, If((map(cr) And 32) > 0, fg, bg))
    '      Draw(x + 6, y + cr, If((map(cr) And 64) > 0, fg, bg))
    '      Draw(x + 7, y + cr, If((map(cr) And 128) > 0, fg, bg))
    '    Next
    '    x += 8
    '  Next

  End Sub

End Class