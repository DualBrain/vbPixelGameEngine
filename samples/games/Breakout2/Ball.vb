Imports VbPixelGameEngine
Imports System.Drawing

Friend Class Ball
  Inherits Block

  Public Property Velocity As Point

  Public Sub New(pixel As Pixel, width As Integer, height As Integer, velocity As Point)
    MyBase.New(pixel, width, height)
    Me.Velocity = velocity
  End Sub

End Class