' Inspired by the article found at: https://migeel.sk/blog/2024/01/02/building-a-self-contained-game-in-csharp-under-2-kilobytes/
' The original C# version of this can be found at: https://github.com/MichalStrehovsky/minimaze

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Minimaze
    If game.Construct(640, 480, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Minimaze
  Inherits PixelGameEngine

  Private Const WIDTH = 640
  Private Const HEIGHT = 480
  'Private Const MAP_WIDTH = 24
  Private Const MAP_HEIGHT = 24

  Private m_posX As Double = 22, m_posY As Double = 12  'x and y start position
  Private m_dirX As Double = -1, m_dirY As Double = 0 'initial direction vector
  Private m_planeX As Double = 0, m_planeY As Double = 0.66 'the 2d raycaster version of camera plane
  Private m_keyState As KeyState

  Friend Sub New()
    AppName = "Minimaze"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_keyState = If(GetKey(Key.A).Held OrElse GetKey(Key.LEFT).Held, KeyState.Left, KeyState.None) Or
                 If(GetKey(Key.S).Held OrElse GetKey(Key.DOWN).Held, KeyState.Down, KeyState.None) Or
                 If(GetKey(Key.D).Held OrElse GetKey(Key.RIGHT).Held, KeyState.Right, KeyState.None) Or
                 If(GetKey(Key.W).Held OrElse GetKey(Key.UP).Held, KeyState.Up, KeyState.None)

    Dim skyColor = New Pixel(&H11, 0, 0)
    Dim groundColor = New Pixel(&H33, &H33, &H33)

    FillRect(0, 0, WIDTH, HEIGHT \ 2, skyColor)
    FillRect(0, HEIGHT \ 2, WIDTH, HEIGHT \ 2, groundColor)

    For x = 0 To WIDTH - 1

      'calculate ray position and direction
      Dim cameraX = 2 * x / WIDTH - 1 'x-coordinate in camera space
      Dim rayDirX = m_dirX + m_planeX * cameraX
      Dim rayDirY = m_dirY + m_planeY * cameraX

      'which box of the map we're in
      Dim mapX = CInt(CLng(Fix(m_posX)) Mod Integer.MaxValue)
      Dim mapY = CInt(CLng(Fix(m_posY)) Mod Integer.MaxValue)

      'length of ray from one x or y-side to next x or y-side
      'these are derived as:
      'deltaDistX = sqrt(1 + (rayDirY * rayDirY) / (rayDirX * rayDirX))
      'deltaDistY = sqrt(1 + (rayDirX * rayDirX) / (rayDirY * rayDirY))
      'which can be simplified to abs(|rayDir| / rayDirX) and abs(|rayDir| / rayDirY)
      'where |rayDir| is the length of the vector (rayDirX, rayDirY). Its length,
      'unlike (dirX, dirY) is not 1, however this does not matter, only the
      'ratio between deltaDistX and deltaDistY matters, due to the way the DDA
      'stepping further below works. So the values can be computed as below.
      ' Division through zero is prevented, even though technically that's not
      ' needed in C++ with IEEE 754 floating point values.
      Dim deltaDistX = If((rayDirX = 0), 1.0E+30, Math.Abs(1 / rayDirX))
      Dim deltaDistY = If((rayDirY = 0), 1.0E+30, Math.Abs(1 / rayDirY))

      Dim perpWallDist As Double

      'what direction to step in x or y-direction (either +1 or -1)
      Dim stepX As Integer
      Dim stepY As Integer

      Dim hit = 0 'was there a wall hit?
      Dim side = 0 'was a NS or a EW wall hit?
      'length of ray from current position to next x or y-side
      Dim sideDistX As Double
      Dim sideDistY As Double
      'calculate step and initial sideDist
      If rayDirX < 0 Then
        stepX = -1 : sideDistX = (m_posX - mapX) * deltaDistX
      Else
        stepX = 1 : sideDistX = (mapX + 1.0 - m_posX) * deltaDistX
      End If
      If rayDirY < 0 Then
        stepY = -1 : sideDistY = (m_posY - mapY) * deltaDistY
      Else
        stepY = 1 : sideDistY = (mapY + 1.0 - m_posY) * deltaDistY
      End If
      'perform DDA
      While hit = 0
        'jump to next map square, either in x-direction, or in y-direction
        If sideDistX < sideDistY Then
          sideDistX += deltaDistX
          mapX += stepX
          side = 0
        Else
          sideDistY += deltaDistY
          mapY += stepY
          side = 1
        End If
        'Check if ray has hit a wall
        If WorldMap(mapX * MAP_HEIGHT + mapY) > 0 Then hit = 1
      End While
      'Calculate distance projected on camera direction. This is the shortest distance from the point where the wall is
      'hit to the camera plane. Euclidean to center camera point would give fisheye effect!
      'This can be computed as (mapX - posX + (1 - stepX) / 2) / rayDirX for side == 0, or same formula with Y
      'for size == 1, but can be simplified to the code below thanks to how sideDist and deltaDist are computed:
      'because they were left scaled to |rayDir|. sideDist is the entire length of the ray above after the multiple
      'steps, but we subtract deltaDist once because one step more into the wall was taken above.
      If side = 0 Then
        perpWallDist = (sideDistX - deltaDistX)
      Else
        perpWallDist = (sideDistY - deltaDistY)
      End If

      'Calculate height of line to draw on screen
      Dim lineHeight = CInt(CLng(Fix((HEIGHT / perpWallDist))) Mod Integer.MaxValue)

      'calculate lowest and highest pixel to fill in current stripe
      Dim drawStart = -lineHeight \ 2 + HEIGHT \ 2 : If drawStart < 0 Then drawStart = 0
      Dim drawEnd = lineHeight \ 2 + HEIGHT \ 2 : If drawEnd >= HEIGHT Then drawEnd = HEIGHT - 1

      '      Dim tempVar As Integer
      Dim color As Pixel = Presets.Yellow

      Select Case WorldMap(mapX * MAP_HEIGHT + mapY)
        Case = 1 : color = Presets.Blue
        Case = 2 : color = Presets.Green
        Case = 3 : color = Presets.Red
        Case = 4 : color = Presets.White
        Case Else
      End Select

      If side = 1 Then
        color = New Pixel(color.R And Not &H80, color.G And Not &H80, color.B And Not &H80)
      End If

      'draw the pixels of the stripe as a vertical line
      For y = drawStart To drawEnd - 1
        Draw(x, y, color)
      Next

      Dim frameTime = elapsedTime 'frameTime is the time this frame has taken, in seconds

      'speed modifiers
      Dim moveSpeed = frameTime * 0.015
      Dim rotSpeed = frameTime * 0.005

      'move forward or backwards if no wall
      If (m_keyState And (KeyState.Up Or KeyState.Down)) <> 0 Then
        If (m_keyState And KeyState.Down) <> 0 Then moveSpeed = -moveSpeed
        If WorldMap(CInt(CLng(Fix((m_posX + m_dirX * moveSpeed))) Mod Integer.MaxValue) * MAP_HEIGHT + CInt(CLng(Fix(m_posY)) Mod Integer.MaxValue)) = 0 Then
          m_posX += m_dirX * moveSpeed
        End If
        If WorldMap(CInt(CLng(Fix(m_posX)) Mod Integer.MaxValue) * MAP_HEIGHT + CInt(CLng(Fix((m_posY + m_dirY * moveSpeed))) Mod Integer.MaxValue)) = 0 Then
          m_posY += m_dirY * moveSpeed
        End If
      End If
      'rotate to the right or left
      If (m_keyState And (KeyState.Right Or KeyState.Left)) <> 0 Then
        If (m_keyState And KeyState.Right) <> 0 Then rotSpeed = -rotSpeed
        'both camera direction and camera plane must be rotated
        Dim oldDirX = m_dirX
        m_dirX = m_dirX * Math.Cos(rotSpeed) - m_dirY * Math.Sin(rotSpeed)
        m_dirY = oldDirX * Math.Sin(rotSpeed) + m_dirY * Math.Cos(rotSpeed)
        Dim oldPlaneX = m_planeX
        m_planeX = m_planeX * Math.Cos(rotSpeed) - m_planeY * Math.Sin(rotSpeed)
        m_planeY = oldPlaneX * Math.Sin(rotSpeed) + m_planeY * Math.Cos(rotSpeed)
      End If

    Next

    Return True

  End Function

  Private Enum KeyState
    None = 0
    Left = 1
    Up = 2
    Right = 4
    Down = 8
  End Enum

  Private Shared ReadOnly Property WorldMap As Byte()
    Get
      Return {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 3, 0, 3, 0, 3, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 2, 2, 0, 2, 2, 0, 0, 0, 0, 3, 0, 3, 0, 3, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 4, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 4, 0, 0, 0, 0, 5, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 4, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 4, 0, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
              1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    End Get
  End Property

End Class