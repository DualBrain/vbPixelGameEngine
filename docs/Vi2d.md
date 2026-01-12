# Vi2d Structure

Represents a 2D vector with integer components. Provides mathematical operations for game development.

## Fields

```vb
Public x As Integer
Public y As Integer
```

## Constructors

```vb
Public Sub New(x As Integer, y As Integer)
Public Sub New(v As Vi2d)
```

**Examples:**
```vb
Dim pos As New Vi2d(10, 20)
Dim copy As New Vi2d(pos)
```

## Properties and Methods

### Mag / Mag2
```vb
Public Function Mag() As Integer
Public Function Mag2() As Integer
```
Get the magnitude (length) of the vector.

**Returns:** Magnitude (Mag2 is squared, faster)

### Norm
```vb
Public Function Norm() As Vi2d
```
Get the normalized (unit length) vector.

**Returns:** Normalized vector

### Perp
```vb
Public Function Perp() As Vi2d
```
Get the perpendicular vector (rotated 90 degrees).

**Returns:** Perpendicular vector

### Floor / Ceil
```vb
Public Function Floor() As Vi2d
Public Function Ceil() As Vi2d
```
Round components down or up.

**Returns:** Floored/ceiled vector

### Max / Min
```vb
Public Function Max(v As Vi2d) As Vi2d
Public Function Min(v As Vi2d) As Vi2d
```
Component-wise maximum/minimum.

**Parameters:**
- `v`: Other vector

**Returns:** Component-wise max/min

### Cart / Polar
```vb
Public Function Cart() As Vi2d
Public Function Polar() As Vi2d
```
Convert between Cartesian and polar coordinates.

**Returns:** Vector in other coordinate system

### Clamp
```vb
Public Function Clamp(v1 As Vi2d, v2 As Vi2d) As Vi2d
```
Clamp components between min and max vectors.

**Parameters:**
- `v1`: Minimum values
- `v2`: Maximum values

**Returns:** Clamped vector

### Lerp
```vb
Public Function Lerp(v1 As Vi2d, t As Double) As Vi2d
```
Linear interpolation towards another vector.

**Parameters:**
- `v1`: Target vector
- `t`: Interpolation factor (0.0 to 1.0)

**Returns:** Interpolated vector

### Dot / Cross
```vb
Public Function Dot(rhs As Vi2d) As Integer
Public Function Cross(rhs As Vi2d) As Integer
```
Dot and cross products.

**Parameters:**
- `rhs`: Right-hand side vector

**Returns:** Dot/cross product result

### Str / ToString
```vb
Public Function Str() As String
Public Overrides Function ToString() As String
```
String representation.

## Operators

```vb
Public Shared Operator +(lhs As Vi2d, rhs As Vi2d) As Vi2d
Public Shared Operator +(lhs As Vi2d, rhs As Vf2d) As Vi2d
Public Shared Operator -(lhs As Vi2d, rhs As Vi2d) As Vi2d
Public Shared Operator -(lhs As Vi2d, rhs As Vf2d) As Vi2d
Public Shared Operator *(lhs As Vi2d, rhs As Integer) As Vi2d
Public Shared Operator *(lhs As Vi2d, rhs As Vi2d) As Vi2d
Public Shared Operator /(lhs As Vi2d, rhs As Integer) As Vi2d
Public Shared Operator /(lhs As Vi2d, rhs As Vi2d) As Vi2d
Public Shared Operator +(lhs As Vi2d) As Vi2d
Public Shared Operator -(lhs As Vi2d) As Vi2d
Public Shared Operator =(lhs As Vi2d, rhs As Vi2d) As Boolean
Public Shared Operator <>(lhs As Vi2d, rhs As Vi2d) As Boolean
Public Shared Widening Operator CType(v As Vi2d) As Vf2d
```

**Examples:**
```vb
Dim a As New Vi2d(10, 20)
Dim b As New Vi2d(5, 15)

' Arithmetic
Dim sum As Vi2d = a + b  ' (15, 35)
Dim diff As Vi2d = a - b ' (5, 5)
Dim scaled As Vi2d = a * 2 ' (20, 40)

' Products
Dim dot As Integer = a.Dot(b)   ' 10*5 + 20*15 = 350
Dim cross As Integer = a.Cross(b) ' 10*15 - 20*5 = 50

' Normalization
Dim unit As Vi2d = a.Norm()

' Distance
Dim distance As Integer = (a - b).Mag()
```