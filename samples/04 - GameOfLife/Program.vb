' Inspired by: "What are Cellular Automata?" -- @javidx9
' https://youtu.be/E7CxMHsYzSs

Imports VbPixelGameEngine

Module Program
  Sub Main()
    Dim game As New GameOfLife
    If game.Construct(160, 100, 8, 8) Then
      game.Start()
    End If
  End Sub

End Module

Class GameOfLife
  Inherits PixelGameEngine

  Private m_output() As Integer
  Private m_state() As Integer

  Friend Sub New()
    AppName = "Game of Life"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    ReDim m_output(ScreenWidth() * ScreenHeight())
    ReDim m_state(ScreenWidth() * ScreenHeight())

    Dim setem = Sub(x As Integer, y As Integer, s As String)
                  Dim p As Integer = 0
                  For Each c As Char In s
                    m_state(y * ScreenWidth() + x + p) = If(c = "#"c, 1, 0)
                    p += 1
                  Next
                End Sub

    '' R-Pentomino
    'setem(80, 50, "  ## ")
    'setem(80, 51, " ##  ")
    'setem(80, 52, "  #  ")

    '' Gosper Glider Gun
    'setem(60, 45, "........................#............")
    'setem(60, 46, "......................#.#............")
    'setem(60, 47, "............##......##............##.")
    'setem(60, 48, "...........#...#....##............##.")
    'setem(60, 49, "##........#.....#...##...............")
    'setem(60, 50, "##........#...#.##....#.#............")
    'setem(60, 51, "..........#.....#.......#............")
    'setem(60, 52, "...........#...#.....................")
    'setem(60, 53, "............##.......................")

    '' Infinite Growth
    'setem(20, 50, "########.#####...###......#######.#####")

    ' Random
    For i = 0 To (ScreenWidth() * ScreenHeight()) - 1
      m_state(i) = If(Rand Mod 2 = 0, 0, 1)
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Threading.Thread.Sleep(50)

    'If GetKey(Key.SPACE).Pressed Then Return True

    Dim cell As Func(Of Integer, Integer, Integer) = Function(x As Integer, y As Integer)
                                                       Return m_output(y * ScreenWidth() + x)
                                                     End Function

    ' Store output state
    For i = 0 To (ScreenWidth() * ScreenHeight()) - 1
      m_output(i) = m_state(i)
    Next

    For x = 1 To ScreenWidth() - 2
      For y = 1 To ScreenHeight() - 2

        Dim neighbors = cell(x - 1, y - 1) + cell(x + 0, y - 1) + cell(x + 1, y - 1) +
                        cell(x - 1, y + 0) + 0 + cell(x + 1, y + 0) +
                        cell(x - 1, y + 1) + cell(x + 0, y + 1) + cell(x + 1, y + 1)

        If cell(x, y) = 1 Then
          m_state(y * ScreenWidth() + x) = If(neighbors = 2 OrElse neighbors = 3, 1, 0)
        Else
          m_state(y * ScreenWidth() + x) = If(neighbors = 3, 1, 0)
        End If

        If cell(x, y) = 1 Then
          Draw(x, y, Presets.White)
        Else
          Draw(x, y, Presets.Black)
        End If

      Next
    Next


    Return True

  End Function

End Class