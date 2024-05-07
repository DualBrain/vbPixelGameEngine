Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New QBasic
    If demo.Construct(256, 192, 4, 4, False, True) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class QBasic
  Inherits PixelGameEngine

  Private ReadOnly m_charWidth As Integer = 8
  Private ReadOnly m_charHeight As Integer = 12
  Private ReadOnly m_colors As New List(Of Pixel)
  Private m_cursorColor As Integer = Colors.Black
  Private ReadOnly m_fg As Colors = Colors.Black
  Private ReadOnly m_bg As Colors = Colors.Green
  Private m_col As Integer = 1
  Private m_row As Integer = 1
  Private m_t As Single

  Friend Sub New()
    AppName = "Color Computer Simulator"
  End Sub

  Private Enum Colors
    Black
    Green
    Red
    Blue
    Gray
    White
    Cyan
    Magenta
    Orange
  End Enum

  Protected Overrides Function OnUserCreate() As Boolean

    ' Build a table of the standard 9 colors...
    ' Note: These colors are based on testing using the Xroar emulator set to RGB output.
    '       Meaning that they might not be totally correct and should be verified on real hardward. 
    m_colors.Add(New Pixel(2, 2, 2)) ' BLACK (0)
    m_colors.Add(New Pixel(2, 253, 2)) ' GREEN (1)
    m_colors.Add(New Pixel(253, 2, 2)) ' RED (2)
    m_colors.Add(New Pixel(2, 74, 253)) ' BLUE (3) - looks "lighter" blue to me
    m_colors.Add(New Pixel(74, 74, 74)) ' GRAY (4)
    m_colors.Add(New Pixel(253, 253, 253)) ' WHITE (5)
    m_colors.Add(New Pixel(74, 253, 253)) ' CYAN (6)
    m_colors.Add(New Pixel(2, 2, 253)) ' MAGENTA (7) - looks blue to me
    m_colors.Add(New Pixel(253, 74, 2)) ' ORANGE (8)

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_t += elapsedTime

    Cls()

    Print("DISK EXTENDED COLOR BASIC 2.1")
    Print("COPR. 1982, 1986 BY TANDY")
    Print("UNDER LICENSE FROM MICROSOFT")
    Print("AND MICROWARE SYSTEMS CORP.")
    Print()
    Print("OK")

    HandleCursor()

    Return True

  End Function

  Private Sub HandleCursor()

    Dim x = (m_col * m_charWidth) - m_charWidth
    Dim y = (m_row * m_charHeight) - m_charHeight

    FillRect(x, y, m_charWidth - 1, m_charHeight - 1, m_colors(m_cursorColor))
    If m_t > 0.1 Then ' approximately 10 times a second, change the cursor color.
      m_t -= 0.1!
      m_cursorColor += 1 : If m_cursorColor > 8 Then m_cursorColor = 0
    End If

  End Sub

#Region "CoCo Inspired API"

  Private Sub Cls(Optional c As Integer = -1)

    If c > -1 Then
    ElseIf c > 8 Then
      c = 1
    Else
      c = m_bg
    End If

    Clear(m_colors(c)) : m_row = 1 : m_col = 1

  End Sub

  'Private Sub Color(fg As Integer, bg As Integer)
  '  If fg > -1 AndAlso fg < 9 Then m_fg = CType(fg, Colors)
  '  If fg > -1 AndAlso fg < 9 Then m_bg = CType(bg, Colors)
  'End Sub

  Private Sub Print(Optional text As String = "", Optional noLinefeed As Boolean = False)

    'TODO: Raw write to the screen, does not currently operate in a "console" style.
    '      So should really rethink about how this works; probably write to a "screen buffer"
    '      where OnUserUpdate will then write the buffer to the actual screen.
    '      This backing buffer could then "scroll" as needed.
    '      This buffer will also be needed in order to "read" a character from the screen (at some point).

    Dim x = (m_col * m_charWidth) - m_charWidth
    Dim y = (m_row * m_charHeight) - m_charHeight

    ' Note: A CoCo has special characters that are also different colors, will need to handle these...
    '       They are at 128+
    For Each c In text
      Dim map = CharMap.CharMap(m_charHeight, c)
      For cr = 0 To m_charHeight - 1
        Draw(x + 0, y + cr, If((map(cr) And 1) > 0, m_colors(m_fg), m_colors(m_bg)))
        Draw(x + 1, y + cr, If((map(cr) And 2) > 0, m_colors(m_fg), m_colors(m_bg)))
        Draw(x + 2, y + cr, If((map(cr) And 4) > 0, m_colors(m_fg), m_colors(m_bg)))
        Draw(x + 3, y + cr, If((map(cr) And 8) > 0, m_colors(m_fg), m_colors(m_bg)))
        Draw(x + 4, y + cr, If((map(cr) And 16) > 0, m_colors(m_fg), m_colors(m_bg)))
        Draw(x + 5, y + cr, If((map(cr) And 32) > 0, m_colors(m_fg), m_colors(m_bg)))
        Draw(x + 6, y + cr, If((map(cr) And 64) > 0, m_colors(m_fg), m_colors(m_bg)))
        Draw(x + 7, y + cr, If((map(cr) And 128) > 0, m_colors(m_fg), m_colors(m_bg)))
      Next
      x += m_charWidth : m_col += 1
      If x > ScreenWidth Then
        x = 0 : y += m_charHeight : m_row += 1 : m_col = 1
      End If
    Next

    If Not noLinefeed Then
      m_row += 1 : m_col = 1
    End If

  End Sub

#End Region

End Class