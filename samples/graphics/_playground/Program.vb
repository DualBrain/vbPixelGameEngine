Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Playground
    If game.Construct(900, 600, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Playground
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Playground"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Clear(m_bg)

    Return True

  End Function

  Private m_mt As Single

  Private m_bg As New Pixel(7, 36, 18)
  Private m_fg As New Pixel(0, 239, 130)
  Private ReadOnly m_centerX As Integer = 450
  Private ReadOnly m_centerY As Integer = 300
  Private ReadOnly m_scaleMax As Integer = 150
  Private m_scale As Integer = 1
  Private m_degree As Integer = 1

  Private m_saltX As Integer = 10
  Private m_saltY As Integer = 7

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    If GetKey(Key.SPACE).Pressed Then
      m_scale = 1
      m_degree = 1
      m_saltX = CInt(Fix(Rnd * 20))
      m_saltY = CInt(Fix(Rnd * 20))
      Clear(m_bg)
    End If

    Dim mt = CSng(1 / 60)

    m_mt += elapsedTime
    If m_mt < mt Then
      Return True
    Else
      m_mt -= mt
    End If

    If False Then

      ' one pixel per pass...

      Dim radian = m_degree * Math.PI / 180
      Dim x = (m_scaleMax + 20 + m_scale * Math.Sin(m_saltX * radian)) * Math.Cos(radian)
      Dim y = (m_scaleMax + m_scale * Math.Sin(m_saltY * radian)) * Math.Sin(radian)
      Draw(m_centerX + x, m_centerY + y, m_fg)

      If m_degree > 360 * 2 Then
        m_scale += 2
        If m_scale > m_scaleMax Then m_scale = 1 : Clear(m_bg)
        m_degree = 1
      Else
        m_degree += 1
      End If

    Else

      ' one rotation per pass...

      For degree = 1 To 360 * 2
        Dim radian = degree * Math.PI / 180
        Dim x = (m_scaleMax + 20 + m_scale * Math.Sin(m_saltX * radian)) * Math.Cos(radian)
        Dim y = (m_scaleMax + m_scale * Math.Sin(m_saltY * radian)) * Math.Sin(radian)
        Draw(m_centerX + x, m_centerY + y, m_fg)
      Next

      m_scale += 2
      If m_scale > m_scaleMax Then
        m_scale = 1
        Clear(m_bg)
        m_saltX = CInt(Fix(Rnd * 20))
        m_saltY = CInt(Fix(Rnd * 20))
      End If

    End If

    Return True

  End Function

End Class