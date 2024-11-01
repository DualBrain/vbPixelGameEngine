Imports VbPixelGameEngine
Imports VbPixelGameEngine.QB64

Friend Module Program

  Sub Main()
    Dim game As New FlippyHexMaze
    If game.Construct(801, 590, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class FlippyHexMaze
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Flippy Hex Maze (b+)"
  End Sub

  Private Structure Board
    Public X As Single
    Public Y As Single
    Public Flipped As Boolean
    Public Flipping As Boolean
    Public A As Single
  End Structure

  Private ReadOnly m_bX As Integer = 18
  Private ReadOnly m_bY As Integer = 16
  Private ReadOnly m_b(m_bX, m_bY) As Board
  Private ReadOnly m_cellR As Single = 25
  Private ReadOnly m_xSpacing As Single = 2 * m_cellR * COS(_D2R(30))
  Private ReadOnly m_ySpacing As Single = m_cellR * (1 + SIN(_D2R(30)))
  Private m_xOffset As Single

  Protected Overrides Function OnUserCreate() As Boolean
    Clear(New Pixel(&HAA, &HAA, &HFF))
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Clear(New Pixel(&HAA, &HAA, &HFF))
    For y = 0 To m_bY
      If y Mod 2 = 0 Then m_xOffset = 0.5! * m_xSpacing Else m_xOffset = 0
      For x = 0 To m_bX
        m_b(x, y).X = x * m_xSpacing + m_xOffset + 0.5! * m_xSpacing - 20.0!
        m_b(x, y).Y = y * m_ySpacing + 0.5! * m_ySpacing - 20.0!
        If Rnd < 0.002 Then m_b(x, y).Flipping = True
        ShowCell(x, y)
      Next
    Next

    Return True

  End Function

  Private Sub ShowCell(c As Integer, r As Integer)
    If m_b(c, r).Flipping Then
      m_b(c, r).A = m_b(c, r).A + _PI(1 / 90)
    End If
    If m_b(c, r).A >= _PI(1 / 3) Then
      m_b(c, r).Flipping = False
      m_b(c, r).A = 0
      m_b(c, r).Flipped = Not m_b(c, r).Flipped
    End If
    Dim dx = m_b(c, r).X, dy = m_b(c, r).Y
    If m_b(c, r).Flipped Then
      For a = _PI(1 / 6) To _PI(2) Step _PI(2 / 3)
        DrawLine(dx, dy, dx + (m_cellR * COS(a + m_b(c, r).A)), dy + (m_cellR * SIN(a + m_b(c, r).A)), Presets.Black)
      Next
    Else
      For a = _PI(0.5) To _PI(2) Step _PI(2 / 3)
        DrawLine(dx, dy, dx + (m_cellR * COS(a + m_b(c, r).A)), dy + (m_cellR * SIN(a + m_b(c, r).A)), Presets.Black)
      Next
    End If
  End Sub

End Class