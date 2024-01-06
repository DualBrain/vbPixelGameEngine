Imports VbPixelGameEngine

Friend Module Program

  Friend Sub Main()
    Dim game = New HelloWorld
    If game.Construct(160, 100, 4, 4) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class HelloWorld
  Inherits PixelGameEngine

  Private m_state As Integer = 0

  Private m_playerX As Single
  Private m_playerY As Single

  Friend Sub New()
    AppName = "Hello World"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    m_playerX = 10
    m_playerY = 10

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.SPACE).Pressed Then
      m_state += 1
    End If

    Select Case m_state

      Case 0

        ' ------
        ' Random fill
        ' ------

        For x = 0 To ScreenWidth() - 1
          For y = 0 To ScreenHeight() - 1
            Draw(x, y, Pixel.Random())
          Next
        Next

      Case 1

        ' ------
        ' Color Swatch
        ' ------

        Clear()
        For c = 0 To 15
          FillRect(0, c * 6, 5, 5, Pixel.Random)
          FillRect(6, c * 6, 5, 5, Pixel.Random)
          FillRect(12, c * 6, 5, 5, Pixel.Random)
          FillRect(18, c * 6, 5, 5, Pixel.Random)
          FillRect(24, c * 6, 5, 5, Pixel.Random)
          FillRect(30, c * 6, 5, 5, Pixel.Random)
          FillRect(36, c * 6, 5, 5, Pixel.Random)
        Next

      Case 2

        ' ------
        ' Simple character movement
        ' ------

        If GetKey(Key.LEFT).Held Then m_playerX -= 15 * elapsedTime
        If GetKey(Key.RIGHT).Held Then m_playerX += 15 * elapsedTime
        If GetKey(Key.UP).Held Then m_playerY -= 15 * elapsedTime
        If GetKey(Key.DOWN).Held Then m_playerY += 15 * elapsedTime

        Clear()

        FillRect(CInt(Fix(m_playerX)), CInt(Fix(m_playerY)), 5, 5)

      Case 3

        ' ------
        ' Mouse - https://youtu.be/tdqc9hZhHxM
        ' ------

        Clear()

        Draw(GetMouseX - 1, GetMouseY)
        Draw(GetMouseX, GetMouseY)
        Draw(GetMouseX + 1, GetMouseY)
        Draw(GetMouseX, GetMouseY - 1)
        Draw(GetMouseX, GetMouseY + 1)

        If GetMouse(0).Held Then
          FillRect(20, 20, 50, 50)
        End If

      Case Else
        m_state = 0
    End Select

    Return True

  End Function

End Class