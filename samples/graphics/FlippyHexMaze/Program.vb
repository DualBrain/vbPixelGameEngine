Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New FlippyHexMaze
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class FlippyHexMaze
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Flippy Hex Maze"
  End Sub

  Private sc, sz, mw As Integer

  Protected Overrides Function OnUserCreate() As Boolean
    CLS()
    TURNS()
    sc = SCRW() \ 400
    sz = 12 * sc
    mw = CEIL(SCRW() / sz)
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    CLS()
    STROKE(sc)
    Dim z = 0, y = 0.0
    Do
      Dim u = z Mod mw, v = z \ mw
      Dim x = 0.5 + (v Mod 2) * sz / 2 + u * sz
      y = 0.5 + v * (sz - 1)
      Dim r = (((64 - x) ^ 2) + ((64 - y) ^ 2)) ^ 0.5
      Dim q = MSECS() / 4000 + COS(r / 80)
      For i = 0 To 2
        Dim a = (MID(1, 2 + (COS(q) * 5), 3) / 12) + (i / 3)
        Dim j = x + (COS(a) * ((sz / 2) + (sz / 13)))
        Dim k = y + (SIN(a) * ((sz / 2) + (sz / 6.1)))
        DrawLine(x, y, j, k, Presets.Black)
      Next
      z += 1
    Loop Until y > SCRH()

    Return True

  End Function

End Class