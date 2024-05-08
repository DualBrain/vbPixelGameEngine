Imports VbPixelGameEngine
Imports System.MathF

Friend Module Program

  Sub Main()
    Dim game As New ArchSpiral
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class ArchSpiral
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Archimedes Spiral"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Clear()
    For t = 0 To 1325.21! Step 0.001!
      Dim r = 1.5 + 0.35 * t
      Draw(400 + r * Cos(t), 240 + r * Sin(t))
    Next
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    If GetKey(Key.ESCAPE).Pressed Then Return False
    Return True
  End Function

End Class