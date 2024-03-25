Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Screen
    If game.Construct(640, 400, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Screen
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "SCREEN 2 (QBasic) Demo"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    m_row = 1
    m_col = 1
    m_fg = Presets.White
    m_bg = Presets.Black
    Return True
  End Function

  Private m_row As Integer, m_col As Integer
  Private m_fg As Pixel, m_bg As Pixel

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const DELAY As Single = 1 / 60.0!
    t += elapsedTime : If t < DELAY Then Return True Else t -= DELAY

    Static m_option As Integer
    Static m_generated As Integer
    Static m_found As Integer
    Static m_searched As Integer

    If GetKey(Key.SPACE).Pressed Then
      m_option += 1 : If m_option > 2 Then m_option = 0
    End If

    m_generated += 1
    If m_generated Mod 1000 = 0 Then m_found += 1
    If m_generated Mod 3333 = 0 Then m_searched += 1

    Clear()
    Locate(2, 33) : Print("Primality Test")
    Locate(3, 14) : Print("Prime: YES or NO - test numbers up to 9 Quadrillion")
    Locate(5, 3) : Print("Options:    SINGLE   RANDOM   RANGE")
    Locate(5, 42) : Print($"Count Searched : {m_searched}")
    Locate(7, 3) : Print($"Randomly generated (n): {m_generated:N0}")
    Locate(7, 42) : Print($"Primes found: {m_found}")
    Locate(10, 31) : Print("xxx NOT Prime xxx")
    Locate(13, 3) : Print("Floored Square Root (n):    824")

    Select Case m_option
      Case 0 : DrawRect(13 * 8, 32 - 2, 64, 8 + 2)
      Case 1 : DrawRect(22 * 8, 32 - 2, 64, 8 + 2)
      Case 2 : DrawRect(31 * 8, 32 - 2, 64 - 8, 8 + 2)
      Case Else
    End Select

    Return True

  End Function

  Private Sub Locate(row As Integer, col As Integer)
    m_row = row : m_col = col
  End Sub

  Private Sub Print(text As String)
    Dim charHeight = 8
    Dim x = (m_col * 8) - 8
    Dim y = (m_row * charHeight) - charHeight
    For Each c In text
      Dim map = CharMap.CharMap(charHeight, c)
      For cr = 0 To charHeight - 1
        Draw(x + 0, y + cr, If((map(cr) And 1) > 0, m_fg, m_bg))
        Draw(x + 1, y + cr, If((map(cr) And 2) > 0, m_fg, m_bg))
        Draw(x + 2, y + cr, If((map(cr) And 4) > 0, m_fg, m_bg))
        Draw(x + 3, y + cr, If((map(cr) And 8) > 0, m_fg, m_bg))
        Draw(x + 4, y + cr, If((map(cr) And 16) > 0, m_fg, m_bg))
        Draw(x + 5, y + cr, If((map(cr) And 32) > 0, m_fg, m_bg))
        Draw(x + 6, y + cr, If((map(cr) And 64) > 0, m_fg, m_bg))
        Draw(x + 7, y + cr, If((map(cr) And 128) > 0, m_fg, m_bg))
      Next
      x += 8
    Next
    m_row += 1
  End Sub

End Class