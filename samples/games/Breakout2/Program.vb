Imports System.Drawing
Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Breakout
    If game.Construct(320, 210, 4, 4) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Breakout
  Inherits PixelGameEngine

  Private ReadOnly RED As New Pixel(200, 72, 72)
  Private ReadOnly ORANGE As New Pixel(198, 108, 58)
  Private ReadOnly DARKYELLOWISH As New Pixel(180, 122, 48)
  Private ReadOnly YELLOWISH As New Pixel(162, 162, 42)
  Private ReadOnly GREENISH As New Pixel(72, 160, 72)
  Private ReadOnly BLUEISH As New Pixel(66, 72, 200)

  Private ReadOnly GREY As New Pixel(105, 105, 105)
  Private ReadOnly BLACK As New Pixel(0, 0, 0)
  Private ReadOnly PINK As New Pixel(168, 76, 96)
  Private ReadOnly BROWN As New Pixel(133, 107, 17)
  Private ReadOnly OTHERBROWN As New Pixel(157, 90, 48)
  Private ReadOnly GREEN As New Pixel(28, 120, 29)
  Private ReadOnly LIGHTGREEN As New Pixel(56, 141, 47)
  Private ReadOnly DARKGREEN As New Pixel(46, 137, 95)
  Private ReadOnly BLUE As New Pixel(91, 92, 214)

  Private ReadOnly WallColor As New Pixel(142, 142, 142)
  Private ReadOnly WallLeftBottomColor As New Pixel(66, 158, 130)
  Private ReadOnly BallColor As New Pixel(200, 72, 72)
  Private ReadOnly PaddleColor As New Pixel(200, 72, 72)

  Private ReadOnly m_blocksContainer As New List(Of Block)()
  Private ReadOnly m_blocks As New List(Of Block)()
  Private ReadOnly m_allSprites As New List(Of Block)()
  Private ReadOnly m_walls As New List(Of Block)()

  Private ReadOnly m_paddle As New Paddle(PaddleColor, 24, 4)
  Private ReadOnly m_ball As New Ball(BallColor, 4, 4, New Point(1, 1))

  Private m_ballDx As Single = 5
  Private m_ballDy As Single = -5
  Private m_ballSpeed As Single = 0.05
  Private m_ballSpeedMax As Single = 1.1

  Private m_score As Integer = 0
  Private m_lives As Integer = 5

  Private m_fieldLeft As Integer
  Private m_fieldTop As Integer
  Private m_fieldRight As Integer

  Friend Sub New()
    AppName = "Breakout"
  End Sub

  Private m_paddleTop As Integer

  Protected Overrides Function OnUserCreate() As Boolean

    Dim w = 320
    Dim h = 210

    Dim wt = 18
    Dim ww = 16
    Dim wh = 15

    ' Initialize paddle
    m_paddle.Rect.X = 20 '(w - 64) \ 2
    m_paddleTop = h - (m_paddle.Rect.Height * 2) - (wh) + 1
    m_paddle.Rect.Y = m_paddleTop

    ' Initialize ball
    m_ball.Rect.X = (w - 10) \ 2
    m_ball.Rect.Y = m_paddle.Rect.Y - 20

    Dim wallLeft = New Block(WallColor, 0, wt, ww, h - wh - wt)
    Dim wallRight = New Block(WallColor, w - ww, wt, ww, h - wh - wt - 2)
    Dim wallHoriz = New Block(WallColor, 0, wt, w - 1, wh)
    Dim wallLeftBottom = New Block(WallLeftBottomColor, 0, m_paddleTop, 16, 7)
    Dim wallRightBottom = New Block(RED, w - ww, m_paddleTop, 16, 6)

    m_fieldLeft = CInt(Fix(wallLeft.Rect.X + wallLeft.Rect.Width))
    m_fieldTop = CInt(Fix(wallHoriz.Rect.Y + wallHoriz.Rect.Height))
    m_fieldRight = CInt(Fix(wallRight.Rect.X))

    m_walls.Add(wallLeft)
    m_walls.Add(wallRight)
    m_walls.Add(wallHoriz)
    m_walls.Add(wallLeftBottom)
    m_walls.Add(wallRightBottom)

    Dim bh = 6
    Dim bw = 16

    For r = 0 To 5
      Dim p As Pixel
      Select Case r
        Case 0 : p = RED
        Case 1 : p = ORANGE
        Case 2 : p = DARKYELLOWISH
        Case 3 : p = YELLOWISH
        Case 4 : p = GREENISH
        Case 5 : p = BLUEISH
        Case Else
      End Select
      For c = 0 To 17
        m_blocksContainer.Add(New Block(p, (CInt(Fix(wallLeft.Rect.X)) + ww) + c * bw, 26 + (CInt(Fix(wallHoriz.Rect.Y)) + wh) + r * bh, bw, bh))
      Next
    Next

    Return True

  End Function

  Private Sub DrawNumber(ch As Char, x As Integer, y As Integer, p As Pixel)
    Dim dots() As String = Nothing
    Select Case ch
      Case ChrW(48) ' 0
        dots = {"111111111111111111111111",
                "111111111111111111111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111111111111111111111",
                "111111111111111111111111"}
      Case ChrW(49) ' 1
        dots = {"000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000",
                "000000001111111100000000"}
      Case ChrW(50) ' 2
        dots = {"111111111111111111111111",
                "111111111111111111111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "111111111111111111111111",
                "111111111111111111111111",
                "111111110000000000000000",
                "111111110000000000000000",
                "111111111111111111111111",
                "111111111111111111111111"}
      Case ChrW(51) ' 3
        dots = {"111111111111111111111111",
                "111111111111111111111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000011111111111111111111",
                "000011111111111111111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "111111111111111111111111",
                "111111111111111111111111"}
      Case ChrW(52) ' 4
        dots = {"111111110000000011111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111111111111111111111",
                "111111111111111111111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111"}
      Case ChrW(53) ' 5
        dots = {"111111111111111111111111",
                "111111111111111111111111",
                "111111110000000000000000",
                "111111110000000000000000",
                "111111111111111111111111",
                "111111111111111111111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "111111111111111111111111",
                "111111111111111111111111"}
      Case ChrW(54) ' 6
        dots = {"111111110000000000000000",
                "111111110000000000000000",
                "111111110000000000000000",
                "111111110000000000000000",
                "111111111111111111111111",
                "111111111111111111111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111111111111111111111",
                "111111111111111111111111"}
      Case ChrW(55) ' 7
        dots = {"111111111111111111111111",
                "111111111111111111111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "000000000000000011111111"}
      Case ChrW(56) ' 8
        dots = {"111111111111111111111111",
                "111111111111111111111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111111111111111111111",
                "111111111111111111111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111111111111111111111",
                "111111111111111111111111"}
      Case ChrW(57) ' 9
        dots = {"111111111111111111111111",
                "111111111111111111111111",
                "111111110000000011111111",
                "111111110000000011111111",
                "111111111111111111111111",
                "111111111111111111111111",
                "000000000000000011111111",
                "000000000000000011111111",
                "111111111111111111111111",
                "111111111111111111111111"}
      Case Else
    End Select
    If dots IsNot Nothing Then
      For r = 0 To 9
        For c = 0 To 23
          If dots(r)(c) = "1"c Then
            Draw(x + c, y + r, p)
          End If
        Next
      Next
    End If
  End Sub

  Private m_et As Single
  Private m_etBall As Single
  Private m_etPaddle As Single

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_et += elapsedTime
    m_etBall += elapsedTime

    If GetKey(Key.ESCAPE).Pressed Then
      Return False
    ElseIf GetKey(Key.RIGHT).Pressed Then
      m_etPaddle = 0
      m_paddle.Rect.X += 1
    ElseIf GetKey(Key.RIGHT).Held Then
      m_etPaddle += elapsedTime
      If m_etPaddle > 0.005 Then
        m_etPaddle -= 0.005!
        m_paddle.Rect.X += 1
      End If
    ElseIf GetKey(Key.RIGHT).Released Then
      m_etPaddle = 0
    ElseIf GetKey(Key.left).Pressed Then
      m_etPaddle = 0
      m_paddle.Rect.X -= 1
    ElseIf GetKey(Key.LEFT).Held Then
      m_etPaddle += elapsedTime
      If m_etPaddle > 0.005 Then
        m_etPaddle -= 0.005!
        m_paddle.Rect.X -= 1
      End If
    ElseIf GetKey(Key.Left).Released Then
      m_etPaddle = 0
    End If

    m_paddle.Rect.X = m_ball.Rect.X - 5

    Clear()

    Dim offset = 8
    offset += 32 : offset += 32
    ' score
    DrawNumber($"{m_score:000}"(0), offset, 5, WallColor) : offset += 32
    DrawNumber($"{m_score:000}"(1), offset, 5, WallColor) : offset += 32
    DrawNumber($"{m_score:000}"(2), offset, 5, WallColor) : offset += 32
    offset += 32
    ' lives
    DrawNumber($"{m_lives}"(0), offset, 5, WallColor) : offset += 32
    offset += 32
    DrawNumber("1"c, offset, 5, WallColor) : offset += 32

    For Each wall In m_walls
      wall.Draw(Me)
    Next

    If m_etBall > m_ballSpeed Then
      m_ball.Rect.X += m_ballDx
      m_ball.Rect.Y += m_ballDy
      m_etBall -= m_ballSpeed
    End If

    'If m_ball.Rect.Y < m_paddle.Rect.Y Then m_justBounced = False

    If m_ball.Rect.X <= m_fieldLeft Then
      ' play sound
      m_ball.Rect.X = m_fieldLeft
      m_ballDx = -m_ballDx
    End If
    If m_ball.Rect.Y <= m_fieldTop Then
      ' play sound
      m_ball.Rect.Y = m_fieldTop
      m_ballDy = -m_ballDy
    End If
    If m_ball.Rect.X > m_fieldRight - 4 Then
      ' sound
      m_ball.Rect.X = m_fieldRight - 4
      m_ballDx = -m_ballDx
    End If
    If m_ball.Rect.Y > m_paddleTop Then
      If m_ball.Rect.X + m_ball.Rect.Width - 1 >= m_paddle.Rect.X AndAlso
         m_ball.Rect.X <= m_paddle.Rect.X + m_paddle.Rect.Width - 1 Then
        ' hit the paddle

        'm_ballDy = -5

        'Dim relative = (m_paddle.Rect.X + (m_ball.Rect.Width / 2)) - m_ball.Rect.X
        'Dim normalize = (relative / (m_paddle.Rect.Width / 2))
        'm_ballDx = -CInt(Fix(normalize * 5))
        'm_ballDx = -CInt(Fix((m_ballDy / 24) * (m_paddle.Rect.X - m_ball.Rect.X)))
        'm_ballDy = -m_ballDy

        m_ball.Rect.Y = m_paddleTop

        Dim angleOfDeflection = MathF.Atan2(m_ball.Rect.Y - m_paddle.Rect.Y, m_ball.Rect.X - m_paddle.Rect.X)
        If angleOfDeflection > 0 Then
          angleOfDeflection += MathF.PI / 2
        Else
          angleOfDeflection -= MathF.PI / 2
        End If

        'm_ballDx *= MathF.Cos(angleOfDeflection)
        'm_ballDy *= MathF.Sin(angleOfDeflection)
        'm_ballDx = -m_ballDx '+ angleOfDeflection
        m_ballDx += angleOfDeflection
        m_ballDy = -m_ballDy

        'Dim center = m_paddle.Rect.X + (m_paddle.Rect.Width \ 2)
        'Dim d = center - m_ball.Rect.X
        'm_ballDx = CInt(Fix(d * -0.6))

        ''Dim angle = 1 - 2 * (m_ball.Rect.X - m_paddle.Rect.X) / m_paddle.Rect.Width
        ''Dim mult = If(m_ballDx < 0, -5, 5)
        ''m_ballDx = CInt(Fix(mult * angle))
        'm_ballDy = -m_ballDy
        'm_ball.Rect.Y = m_paddleTop

        m_score += 1

      Else
        m_ball.Rect.X = m_paddle.Rect.X
        m_ball.Rect.Y = 100
        m_ballDx = -1 * m_ballDx
        m_ballDy = 5
      End If
    End If

    For Each block In m_blocksContainer
      block.Draw(Me)
    Next

    m_ball.Draw(Me)
    m_paddle.Draw(Me)

    Return True

  End Function

End Class