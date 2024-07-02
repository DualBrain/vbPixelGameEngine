Public Structure Vi2d

  ' See comments below re: Option Strict; probably fine, but wanted to note that
  ' before doing so code was "working" due to automatic type conversion; but now
  ' it is strict... so there could be differences?

  Public x As Integer
  Public y As Integer

  Public Sub New(x As Integer, y As Integer)
    Me.x = x
    Me.y = y
  End Sub

  Public Sub New(v As Vi2d)
    x = v.x
    y = v.y
  End Sub

  Public Function Mag() As Integer
    Return CInt(Fix(MathF.Sqrt(x * x + y * y)))
  End Function

  Public Function Mag2() As Integer
    Return x * x + y * y
  End Function

  Public Function Norm() As Vi2d
    Dim r = CInt(1 / Mag())
    Return New Vi2d(x * r, y * r)
  End Function

  Public Function Perp() As Vi2d
    Return New Vi2d(-y, x)
  End Function

  Public Function Floor() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Floor(x)), CInt(MathF.Floor(y)))
  End Function

  Public Function Ceil() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Ceiling(x)), CInt(MathF.Ceiling(y)))
  End Function

  Public Function [Max](v As Vi2d) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Max(x, v.x)), CInt(MathF.Max(y, v.y)))
  End Function

  Public Function [Min](v As Vi2d) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Min(x, v.x)), CInt(MathF.Min(y, v.y)))
  End Function

  Public Function Cart() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(MathF.Cos(y) * x), CInt(MathF.Sin(y) * x))
  End Function

  Public Function Polar() As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(Mag(), CInt(MathF.Atan2(y, x)))
  End Function

  Public Function Clamp(v1 As Vi2d, v2 As Vi2d) As Vi2d
    Return [Max](v1).min(v2)
  End Function

  Public Function Lerp(v1 As Vi2d, t As Double) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return Me * CInt(Fix(1.0F - t)) + (v1 * CInt(Fix(t)))
  End Function

  Public Function Dot(rhs As Vi2d) As Integer
    Return x * rhs.x + y * rhs.y
  End Function

  Public Function Cross(rhs As Vi2d) As Integer
    Return x * rhs.y - y * rhs.x
  End Function

  Public Shared Operator +(lhs As Vi2d, rhs As Vi2d) As Vi2d
    Return New Vi2d(lhs.x + rhs.x, lhs.y + rhs.y)
  End Operator

  Public Shared Operator +(lhs As Vi2d, rhs As Vf2d) As Vi2d
    Return New Vi2d(lhs.x + CInt(Fix(rhs.x)), lhs.y + CInt(Fix(rhs.y)))
  End Operator

  Public Shared Operator -(left As Vi2d, right As Vi2d) As Vi2d
    Return New Vi2d(left.x - right.x, left.y - right.y)
  End Operator

  Public Shared Operator -(left As Vi2d, right As Vf2d) As Vi2d
    Return New Vi2d(left.x - CInt(right.x), left.y - CInt(right.y))
  End Operator

  Public Shared Operator *(left As Vi2d, right As Integer) As Vi2d
    Return New Vi2d(left.x * right, left.y * right)
  End Operator

  Public Shared Operator *(left As Vi2d, right As Vi2d) As Vi2d
    Return New Vi2d(left.x * right.x, left.y * right.y)
  End Operator

  Public Shared Operator /(left As Vi2d, right As Integer) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(Fix(left.x / right)), CInt(Fix(left.y / right)))
  End Operator

  Public Shared Operator /(left As Vi2d, right As Vi2d) As Vi2d ' Turned on project-wide Option Strict; had to add CInt()?
    Return New Vi2d(CInt(Fix(left.x / right.x)), CInt(Fix(left.y / right.y)))
  End Operator

  'Public Shared Operator +=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x += right.x
  '  left.y += right.y
  '  Return left
  'End Operator

  'Public Shared Operator -=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x -= right.x
  '  left.y -= right.y
  '  Return left
  'End Operator

  'Public Shared Operator *=(left As Vi2d, right As Integer) As Vi2d
  '  left.x *= right
  '  left.y *= right
  '  Return left
  'End Operator

  'Public Shared Operator /=(left As Vi2d, right As Integer) As Vi2d
  '  left.x /= right
  '  left.y /= right
  '  Return left
  'End Operator

  'Public Shared Operator *=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x *= right.x
  '  left.y *= right.y
  '  Return left
  'End Operator

  'Public Shared Operator /=(left As Vi2d, right As Vi2d) As Vi2d
  '  left.x /= right.x
  '  left.y /= right.y
  '  Return left
  'End Operator

  Public Shared Operator +(lhs As Vi2d) As Vi2d
    Return New Vi2d(+lhs.x, +lhs.y)
  End Operator

  Public Shared Operator -(lhs As Vi2d) As Vi2d
    Return New Vi2d(-lhs.x, -lhs.y)
  End Operator

  Public Shared Operator =(lhs As Vi2d, rhs As Vi2d) As Boolean
    Return (lhs.x = rhs.x AndAlso lhs.y = rhs.y)
  End Operator

  Public Shared Operator <>(lhs As Vi2d, rhs As Vi2d) As Boolean
    Return (lhs.x <> rhs.x OrElse lhs.y <> rhs.y)
  End Operator

  Public Function Str() As String
    Return $"({x},{y})"
  End Function

  Public Overrides Function ToString() As String
    Return Str()
  End Function

  'Public Shared Widening Operator CType(v As Vi2d) As Vi2d
  '  Return New Vi2d(v.x, v.y)
  'End Operator

  Public Shared Widening Operator CType(v As Vi2d) As Vf2d
    Return New Vf2d(v.x, v.y)
  End Operator

  'Public Shared Widening Operator CType(v As Vi2d) As v2d_generic(Of Double)
  '  Return New v2d_generic(Of Double)(v.x, v.y)
  'End Operator

End Structure