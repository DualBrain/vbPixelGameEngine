Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim game As New Catrun
    If game.Construct(800, 480, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class Catrun
  Inherits PixelGameEngine

  Private ReadOnly m_data As Long() = {0, 0, 1018, 2096134, 14680065, 16777529,
                                       100663590, 2080636848, 2289827964, 260046858, 402653189, 671088640,
                                       201326592, 0, 0, 0, 4163524, 12613692,
                                       16777218, 100663665, 4194828238, 104334192, 499122460, 134217987,
                                       805306752, 192, 0, 507904, 1585160, 6295484,
                                       12582978, 18907153, 35741742, 2084708160, 6030656, 2103520,
                                       1052720, 2101256, 2052, 0, 245760, 536576,
                                       66063368, 68157976, 403833316, 3758690306, 368700, 225216,
                                       219136, 120832, 34816, 3072, 0, 0, 1057980416,
                                       1089495072, 1074796768, 2098960, 2097160, 2105584,
                                       1571584, 1577984, 1320960, 3424256, 1769472, 1879048192,
                                       251658240, 14925828, 1847326, 2100961, 4194561, 4194334,
                                       4423712, 8683472, 20447344, 20971520, 10485760, 7864320}

  Private ReadOnly m_timing As Single = 0.06!
  Private m_xc As Integer
  Private m_frame As Integer
  Private m_time As Single

  Friend Sub New()
    AppName = "Catrun"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    m_xc = 0 : m_frame = 0
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    m_time += elapsedTime
    Dim yc = ScreenHeight \ 2
    If m_frame > 77 Then m_frame = 0
    Dim frame = m_frame

    Clear()

    DrawLine(0, yc + 13, ScreenWidth, yc + 13, Presets.DarkMagenta)

    For y = yc To yc + 12
      Dim a = m_data(frame) : frame += 1
      For k = 0 To 31
        If ((a And CLng(2 ^ k)) <> 0) Then Draw(m_xc + 31 - k, y)
      Next
    Next

    If m_time > m_timing Then
      m_xc = If(m_xc < ScreenWidth, m_xc + 8, -32)
      m_time -= m_timing
      m_frame = frame
    End If

    Return True

  End Function

End Class