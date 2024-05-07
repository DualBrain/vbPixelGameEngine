Imports VbPixelGameEngine

Friend Class Rectangle

  Public Property X As Integer
  Public Property Y As Integer
  Public Property Width As Integer
  Public Property Height As Integer

  Public Sub New(x As Integer, y As Integer, w As Integer, h As Integer)
    Me.X = x
    Me.Y = y
    Width = w
    Height = h
  End Sub

End Class

Friend Class Block

  Public Property Rect As Rectangle
  Public Property Pixel As Pixel

  Public Sub New(pixel As Pixel, x As Integer, y As Integer, width As Integer, height As Integer)
    Me.Pixel = pixel
    Me.Rect = New Rectangle(x, y, width, height)
  End Sub

  Public Sub New(pixel As Pixel, width As Integer, height As Integer)
    Me.Pixel = pixel
    Me.Rect = New Rectangle(0, 0, width, height)
  End Sub

  Public Sub Draw(pge As PixelGameEngine)
    pge.FillRect(Rect.X, Rect.Y, Rect.Width, Rect.Height, Pixel)
  End Sub

End Class