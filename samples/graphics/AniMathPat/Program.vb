Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New AniMathPat
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class AniMathPat
  Inherits PixelGameEngine

  Private ReadOnly m_d As Integer = 15000
  Private ReadOnly m_fc As Integer = 100
  Private m_p1(,), m_p2(,), m_dd(,) As Double

  Private m_phase As Integer = 0
  Private m_counter As Integer = 1

  Friend Sub New()
    AppName = "Mathematical Patterns"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    ReDim m_p1(m_d, 3), m_p2(m_d, 3), m_dd(m_d, 2)
    Calculate() : Calculate()
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If m_phase = 0 Then
      For f = 1 To m_d
        m_dd(f, 1) = (m_p2(f, 1) - m_p1(f, 1)) / m_fc
        m_dd(f, 2) = (m_p2(f, 2) - m_p1(f, 2)) / m_fc
      Next
      PLOT(m_p2)
      m_phase += 1
    End If

    If m_phase = 1 Then
      For f = 1 To m_d
        m_p2(f, 1) -= m_dd(f, 1) : m_p2(f, 2) -= m_dd(f, 2)
      Next
      Clear()
      PLOT(m_p2)
      m_counter += 1
      If m_counter > 100 Then m_counter = 1 : m_phase = 0 : Calculate()
    End If

    Return True

  End Function

  Private Sub PLOT(a(,) As Double)
    For i = 0 To UBound(a, 1)
      Dim x = a(i, 1), y = a(i, 2), c = CInt(a(i, 3)) Mod 256
      Draw(x, y, SpecPalette(c))
    Next
  End Sub

  Private Shared Sub MAT(target(,) As Double, source(,) As Double)
    For a = 0 To UBound(target, 1)
      For b = 0 To UBound(target, 2)
        target(a, b) = source(a, b)
      Next
    Next
  End Sub

  Private Sub Calculate()
    MAT(m_p2, m_p1)
    Dim xf = ScreenWidth * 0.47, yf = ScreenHeight * 0.5, a = Rnd(), b = 0.9998, c = 2 - 2 * a, x = 0#, j = 0#, y = Rnd * 12.0 + 0.1
    For i = 1 To m_d
      Dim z = x
      x = b * y + j
      j = a * x + c * (x ^ 2) / (1 + x ^ 2)
      y = (j - z)
      m_p1(i, 1) = x * 20 + xf
      m_p1(i, 2) = y * 20 + yf
      m_p1(i, 3) = i / 1000
    Next
  End Sub

End Class