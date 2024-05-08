Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New AnimWave
    If game.Construct(800, 480) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class AnimWave
  Inherits PixelGameEngine

  Private ReadOnly m_maxHeight As Integer = 31
  Private ReadOnly m_xSize As Integer = 40
  Private ReadOnly m_ySize As Integer = 40
  Private ReadOnly m_xScale As Integer = 5
  Private ReadOnly m_yScale As Integer = 4
  Private ReadOnly m_xOffset As Integer = -3
  Private ReadOnly m_yOffset As Integer = 1

  Private ReadOnly m_waveTable(359) As Single
  Private ReadOnly m_heightMap(m_xSize * 2, m_ySize * 2) As Integer

  Private m_xCenter As Integer
  Private m_yCenter As Integer

  Friend Sub New()
    AppName = "Animated Sinewave"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    For angle = 0 To 359
      m_waveTable(angle) = MathF.Sin(angle * MathF.PI / 180.0!) * m_maxHeight
    Next

    For y = -m_ySize To m_ySize
      For x = -m_xSize To m_xSize
        m_heightMap(x + m_xSize, y + m_ySize) = CInt((((MathF.Cos(x / 12.0!) + 1) + (MathF.Cos(y / 12.0!) + 1)) / 4 * 359))
      Next
    Next

    m_xCenter = ScreenWidth \ 2
    m_yCenter = ScreenHeight \ 2

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const m_delay As Single = 1 / 120.0!
    t += elapsedTime : If t < m_delay Then Return True Else t -= m_delay

    If GetKey(Key.ESCAPE).Pressed Then Return False

    For y = -m_ySize To m_ySize
      For x = -m_xSize To m_xSize
        m_heightMap(x + m_xSize, y + m_ySize) -= 1 : If m_heightMap(x + m_xSize, y + m_ySize) < 0 Then m_heightMap(x + m_xSize, y + m_ySize) = 359
      Next
    Next

    Clear()

    For y = -m_ySize To m_ySize
      For x = -m_xSize To m_xSize
        Draw(x * m_xScale + y * m_xOffset + m_xCenter, ScreenHeight - (y * m_yScale + x * m_yOffset - m_waveTable(m_heightMap(x + m_xSize, y + m_ySize)) + m_yCenter))
      Next
    Next

    Return True

  End Function

End Class