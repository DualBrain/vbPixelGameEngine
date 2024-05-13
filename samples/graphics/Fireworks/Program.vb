Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Fireworks
    If game.Construct(1920, 1080, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Fireworks
  Inherits PixelGameEngine

  Private Const GRAVITY As Single = -10 / 60

  Private Const FRAGMENTS As Integer = 6
  Private Const STARS As Integer = 12
  Private Const X_RANGE As Integer = 8

  Private ReadOnly m_fireworks As New List(Of Firework)

  Private m_holiday As Holiday = Holiday.July4

  Friend Sub New()
    AppName = "Fireworks"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    m_fireworks.Add(New Firework(ScreenHeight, m_holiday))
    Return True
  End Function

  Private ReadOnly m_rnd As New Random()

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False
    If GetKey(Key.SPACE).Pressed Then
      For x = 1 To 10
        m_fireworks.Add(New Firework(ScreenHeight, m_holiday))
      Next
    End If

    If m_rnd.Next(1, 100) < 6 Then
      m_fireworks.Add(New Firework(ScreenHeight, m_holiday))
    End If

    Clear()

    Dim hx = ScreenWidth \ 2
    Dim hy = ScreenHeight \ 2

    SetPixelMode(Pixel.Mode.Alpha)
    For Each firework In m_fireworks
      firework.Update()
      If firework.Y < -hy Then firework.Dead = True
      If Not firework.Dead AndAlso
         firework.Vy < 0 AndAlso
         firework.Stage = Stage.Rocket Then
        For x = 1 To FRAGMENTS
          m_fireworks.Add(New Firework(firework))
        Next
        firework.Dead = True
        Exit For
      End If
      If Not firework.Dead AndAlso
         firework.Stage = Stage.Fragment Then
        If m_rnd.Next(1, 20) = 1 Then
          For x = 1 To STARS
            m_fireworks.Add(New Firework(firework, x))
          Next
          firework.Dead = True
          Exit For
        End If
      End If
      If Not firework.Dead Then
        FillCircle(firework.X + hx, ScreenHeight - (firework.Y + hy), firework.Radius, firework.Pixel)
      End If
    Next
    SetPixelMode(Pixel.Mode.Normal)

    ' Remove *dead* fireworks.
    For index = m_fireworks.Count - 1 To 0 Step -1
      If m_fireworks(index).Dead Then m_fireworks.RemoveAt(index)
    Next

    Return True

  End Function

  Private Enum Holiday
    None
    July4
  End Enum

  Private Enum Stage
    Rocket
    Fragment
    Star
  End Enum

  Private Class Firework

    Public Property X As Single
    Public Property Y As Single
    Public Property Vx As Single
    Public Property Vy As Single
    Public Property Radius As Single
    Public Property Pixel As Pixel

    Public Property Stage As Stage = Stage.Rocket
    Public Property Dead As Boolean

    Public Sub New(scrHeight As Integer, Optional holiday As Holiday = Holiday.None)

      X = 0
      Y = -(scrHeight \ 2)
      Dim rnd = New Random()
      Vx = (rnd.NextSingle() * (X_RANGE * 2)) - X_RANGE
      Vy = 15 + (rnd.NextSingle() * 4)
      Radius = 1 '6

      Dim r = rnd.Next(0, 255)
      Dim g = rnd.Next(0, 255)
      Dim b = rnd.Next(0, 255)

      Select Case holiday
        Case Holiday.July4
          Dim v = rnd.Next(100, 256)
          Select Case rnd.Next(1, 4)
            Case 1
              Pixel = New Pixel(v, 0, 0, 100)
            Case 2
              Pixel = New Pixel(v, v, v, 100)
            Case 3
              Pixel = New Pixel(0, 0, v, 100)
            Case Else
              Stop
          End Select
        Case Else
          Pixel = New Pixel(r, g, b, 100)
      End Select

    End Sub

    Public Sub New(firework As Firework)

      Stage = Stage.Fragment

      X = firework.X
      Y = firework.Y
      Radius = 3
      Dim rnd = New Random()
      Vx = (rnd.NextSingle() * 6) - 3
      Vy = rnd.NextSingle() * 8

      Pixel = New Pixel(firework.Pixel.R, firework.Pixel.G, firework.Pixel.B, 255)

    End Sub

    Public Sub New(firework As Firework, index As Integer)

      Stage = Stage.Star

      X = firework.X
      Y = firework.Y
      Radius = 2
      Dim rnd = New Random()
      Dim angleStep = 360.0! / STARS
      Dim angle = index * angleStep
      Dim starVelocity = (rnd.NextSingle() * 5) + 1
      Vx = MathF.Cos(angle * starVelocity)
      Vy = MathF.Sin(angle * starVelocity)
      Pixel = New Pixel(firework.Pixel.R, firework.Pixel.G, firework.Pixel.B, 255)

    End Sub

    Public Sub Update()
      If Not Dead Then
        X += Vx
        Y += Vy
        Vy += GRAVITY
        If Vy < 0 Then
          Dim a = Pixel.A
          If CInt(a) - CByte(5) < 0 Then
            Me.Dead = True
          Else
            a -= CByte(5)
            Pixel = New Pixel(Pixel.R, Pixel.G, Pixel.B, a)
          End If
        End If
      End If
    End Sub

  End Class

End Class