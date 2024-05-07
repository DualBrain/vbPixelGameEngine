Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Plinko
    If game.Construct(800, 600, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Plinko
  Inherits PixelGameEngine

  Private Const n = 5000
  Private ReadOnly p(n, 5) As Single

  Friend Sub New()
    AppName = "Plinko"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    For i = 1 To n
      p(i, 1) = -i / 8.0!
      p(i, 2) = 300
      p(i, 3) = 1
      p(i, 4) = 0
      p(i, 5) = -i / 8.0!
    Next
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    Clear()
    For i = 1 To n
      Draw(p(i, 1), p(i, 2))
      p(i, 5) += 1
      If p(i, 5) >= 16 Then
        p(i, 4) = 2 * Coin - 1
        p(i, 5) = 0
      Else
        p(i, 1) += p(i, 3)
        p(i, 2) += p(i, 4)
      End If
    Next
    Return True
  End Function

End Class