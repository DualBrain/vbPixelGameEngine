#Disable Warning IDE1006 ' Naming Styles
Public Class QB64
  Inherits PgeX

  Public Shared Function COS(n As Single) As Single
    Return MathF.Cos(n)
  End Function

  Public Shared Function COS(n As Double) As Double
    Return Math.Cos(n)
  End Function

  Public Shared Function _D2R(n As Single) As Single
    Return n * MathF.PI / 180
  End Function

  Public Shared Function _D2R(n As Double) As Double
    Return n * Math.PI / 180
  End Function

  Public Shared Function _PI(muliplier As Single) As Single
    Return MathF.PI * muliplier
  End Function

  Public Shared Function SIN(n As Single) As Single
    Return MathF.Sin(n)
  End Function

  Public Shared Function SIN(n As Double) As Double
    Return Math.Sin(n)
  End Function

End Class
#Enable Warning IDE1006 ' Naming Styles