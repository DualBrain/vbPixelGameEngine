Imports VbPixelGameEngine
Imports VbPixelGameEngine.QB64

Friend Module Program

  Sub Main()
    Dim game As New Shapes

    If game.Construct(1920, 1080, 1, 1, False, True) Then
      game.Start()
    End If

  End Sub

End Module

Friend Class Shapes
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Shapes"
  End Sub


  Protected Overrides Function OnUserCreate() As Boolean

    count = 500 + (Rnd * 500)

    Return True

  End Function

  'Private m_mt As Single

  Private Shared count As Double
  Private Shared pass As Integer

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    'Dim mt = CSng(1 / 60) : m_mt += elapsedTime : If m_mt < mt Then Return True Else m_mt -= mt

    If pass > count Then
      Clear()
      pass = 0
      count = 500 + (Rnd * 500)
    End If

    'Dim count = 100 + (Rnd * 100)

    'For k = 1 To count

    Dim q = Int(Rnd * 5) + 2 : If q = 7 Then q = 30
    Dim a = _D2R(360 / q), s = SIN(a), c = COS(a)
    Dim x = Int(1 + Rnd * ScreenWidth)
    Dim y = Int(1 + Rnd * ScreenHeight)
    Dim r = (Rnd * (SQR(y + x) / 20)) * 40 + 16
    For l = -r / 5 To r / 5
      QB64.COLOR(Rnd * 16) 'ABS(If(l < r, -1, 0) / 8) * Rnd * 32)
      Dim u = r + l, v = u
      PSET(x + u, y + v)
      For i = 1 To q
        Dim t = v * c - u * s
        u = v * s + u * c
        v = t
        LINE(x + u, y + v)
      Next
    Next

    'Next

    pass += 1

    Return True

  End Function

End Class