Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New Explode
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Explode
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Explode"
  End Sub

  Private Const np As Integer = 1001
  Private ReadOnly p(np, 4) As Single

  Protected Overrides Function OnUserCreate() As Boolean
    For f = 1 To np
      p(f, 1) = SCRW() / 2.0!
      p(f, 2) = SCRH() / 2.0!
      p(f, 3) = CSng(Rnd * 4) - 2
      p(f, 4) = CSng(Rnd * 4) - 2
    Next
    PAPER(0) : INK(15)
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    CLS()

    For f = 1 To np
      PLOT(p(f, 1), p(f, 2))
      INC(p(f, 1), p(f, 3))
      INC(p(f, 2), p(f, 4))
      INC(p(f, 4), 0.01)
    Next f

    Return True

  End Function

End Class