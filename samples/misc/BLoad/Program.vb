Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New Demo
    If demo.Construct(320, 200, 4, 4) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class Demo
  Inherits PixelGameEngine

  'Private m_path As String = "content/CWE4.DAT"
  'Private m_path As String = "content/TECHNIC.FNT"
  'Private m_path As String = "content/tsbed1.dat"
  'Private m_path As String = "content/tscpanl.tdp"
  'Private m_path As String = "content/tsdemo1.img"
  'Private m_path As String = "content/tsem2.dat"
  Private m_path As String = "content/tsscene1.tdp"
  'Private m_path As String = "content/tstitle1.dat"
  'Private m_path As String = "content/turingd.tdp"
  Private m_data() As Byte
  Private m_loader As Loader
  Private m_sprite As Sprite

  Friend Sub New()
    AppName = "BLoad Demo"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    If IO.File.Exists(m_path) Then
      m_data = IO.File.ReadAllBytes(m_path)
      Try
        m_loader = New Loader(m_data)
        Debug.WriteLine(m_loader.ToString)
        m_sprite = New Sprite(m_loader.GetBitmapWidth, m_loader.GetBitmapHeight)
        m_loader.ConvertToSprite(m_sprite)
      Catch ex As Exception
        m_sprite = New Sprite(7, 7)
      End Try
    End If
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    Clear()
    DrawSprite(0, 0, m_sprite)
    Return True
  End Function

End Class