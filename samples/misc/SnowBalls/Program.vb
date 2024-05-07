Imports VbPixelGameEngine

' Found on the Discord...

' Notes:
'
'  - missing snowflake.png
'  - currently do not support `Renderable` (probably part of 2.x)
'  - also do not currently support `DrawRotatedDecal` (probably part of 2.x)

Friend Module Program

  Sub Main()
    Dim demo As New SnowBalls()
    If demo.Construct(1280, 720, 1, 1, False, True) Then
      demo.Start()
    End If
  End Sub

End Module

Class SnowBalls
  Inherits PixelGameEngine

  Public Sub New()
    AppName = "Snow Balls"
  End Sub

  Private Class SnowFlake
    Public Sub New()
    End Sub
    Public Sub New(position As Vf2d, depth As Single)
      Me.Position = position
      Me.Depth = depth
    End Sub
    Public Sub New(position As Vf2d, velocity As Vf2d, depth As Single)
      Me.Position = position
      Me.Velocity = velocity
      Me.Depth = depth
    End Sub
    Public Property Position As Vf2d
    Public Property Velocity As Vf2d
    Public Property Depth As Single
  End Class

  Private ReadOnly m_flakes As New List(Of SnowFlake)
  Private m_screen As Vf2d
  'Private ReadOnly m_flake As New Sprite ' Renderable
  Private m_wind As Vf2d
  Private m_windAngle, m_windAngleTarget As Single
  Private m_windChangeCountdown As Single = 10.0F

  Private Function MyRnd(min As Single, max As Single) As Single
    Return CSng((Rand / RAND_MAX) * (max - min) + min)
  End Function

  Protected Overrides Function OnUserCreate() As Boolean

    m_screen = New Vf2d(ScreenWidth(), ScreenHeight())
    'm_flake.Load("snowflake.png")
    'm_flake.LoadFromFile("snowflake.png")

    For i = 0 To 1024
      m_flakes.Add(New SnowFlake(New Vf2d(MyRnd(-200.0F, m_screen.x + 200.0F),
                                                       MyRnd(-100.0F, m_screen.y)),
                                      MyRnd(1.0F, 5.0F)))
    Next

    m_windAngleTarget = 1.0F * 2.0F * 3.14159F

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    elapsedTime *= If(GetKey(Key.SPACE).Held, 10.0F, 1.0F)

    m_windChangeCountdown -= elapsedTime
    If m_windChangeCountdown <= 0.0F Then
      m_windChangeCountdown = MyRnd(4.0F, 20.0F)
      m_windAngleTarget = CSng((0.75 + MyRnd(-0.25F, +0.25F)) * 2.0F * 3.14159F)
    End If

    m_windAngle += (m_windAngleTarget - m_windAngle) * 0.8F

    m_wind = New Vf2d(CSng(Math.Cos(m_windAngle)), CSng(Math.Sin(m_windAngle)))

    ' physics init

    For Each flake In m_flakes

      flake.Velocity = New Vf2d(0.0F, 10.0F / flake.Depth) + m_wind
      flake.Position += flake.Velocity * elapsedTime * 10.0F

      If flake.Position.y >= m_screen.y + 50.0F OrElse
         flake.Position.x >= m_screen.x + 50.0F OrElse
         flake.Position.x <= -50.0F Then
        flake.Position = New Vf2d(MyRnd(-200.0F, m_screen.x + 200.0F),
                                  MyRnd(-153.0F, -77.0F))
        flake.Depth = MyRnd(1.0F, 5.0F)
      End If

    Next

    ' drawing stuff

    Clear()

    For Each flake In m_flakes

      Dim c = 1.0F - ((flake.Depth - 1.0F) / 5.0F)
      Dim s = 0.1F / (flake.Depth / 2.0F)

      'DrawRotatedDecal(flake.Position, m_flake.Decal(), 0.0F,
      '                 New Vf2d(32.0F, 32.0F),
      '                 New Vf2d(s, s),
      '                 New Pixel(c, c, c, 1.0F))

      'DrawCircle(flake.Position, c * 10, New Pixel(c * 255, c * 255, c * 255, c * 255))
      FillCircle(flake.Position, CInt(c * 5), New Pixel(c * 255, c * 255, c * 255))

    Next

    Return True

  End Function

End Class