# Vf2d Structure

Represents a 2D vector with floating-point components. Provides mathematical operations for precise calculations.

## Fields

```vb
Public x As Single
Public y As Single
```

## Constructors

```vb
Public Sub New(x As Single, y As Single)
Public Sub New(v As Vf2d)
```

**Examples:**
```vb
Dim pos As New Vf2d(10.5F, 20.3F)
Dim copy As New Vf2d(pos)
```

## Properties and Methods

### Mag / Mag2
```vb
Public Function Mag() As Single
Public Function Mag2() As Single
```
Get the magnitude (length) of the vector.

**Returns:** Magnitude (Mag2 is squared, faster)

### Norm
```vb
Public Function Norm() As Vf2d
```
Get the normalized (unit length) vector.

**Returns:** Normalized vector

### Perp
```vb
Public Function Perp() As Vf2d
```
Get the perpendicular vector (rotated 90 degrees).

**Returns:** Perpendicular vector

### Floor / Ceil
```vb
Public Function Floor() As Vf2d
Public Function Ceil() As Vf2d
```
Round components down or up.

**Returns:** Floored/ceiled vector

### Max / Min
```vb
Public Function [Max](v As Vf2d) As Vf2d
Public Function [Min](v As Vf2d) As Vf2d
```
Component-wise maximum/minimum.

**Parameters:**
- `v`: Other vector

**Returns:** Component-wise max/min

### Cart / Polar
```vb
Public Function Cart() As Vf2d
Public Function Polar() As Vf2d
```
Convert between Cartesian and polar coordinates.

**Returns:** Vector in other coordinate system

### Clamp
```vb
Public Function Clamp(v1 As Vf2d, v2 As Vf2d) As Vf2d
```
Clamp components between min and max vectors.

**Parameters:**
- `v1`: Minimum values
- `v2`: Maximum values

**Returns:** Clamped vector

### Lerp
```vb
Public Function Lerp(v1 As Vf2d, t As Double) As Vf2d
```
Linear interpolation towards another vector.

**Parameters:**
- `v1`: Target vector
- `t`: Interpolation factor (0.0 to 1.0)

**Returns:** Interpolated vector

### Dot / Cross
```vb
Public Function Dot(rhs As Vf2d) As Integer
Public Function Cross(rhs As Vf2d) As Integer
```
Dot and cross products (returned as integers).

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
Public Shared Operator +(lhs As Vf2d, rhs As Vf2d) As Vf2d
Public Shared Operator +(left As Vf2d, right As Single) As Vf2d
Public Shared Operator +(left As Single, right As Vf2d) As Vf2d
Public Shared Operator +(left As Vf2d, right As Vi2d) As Vf2d
Public Shared Operator -(left As Vf2d, right As Vf2d) As Vf2d
Public Shared Operator -(left As Vf2d, right As Vi2d) As Vf2d
Public Shared Operator *(left As Vf2d, right As Single) As Vf2d
Public Shared Operator *(left As Single, right As Vf2d) As Vf2d
Public Shared Operator *(left As Vf2d, right As Vf2d) As Vf2d
Public Shared Operator /(left As Vf2d, right As Single) As Vf2d
Public Shared Operator /(left As Vf2d, right As Vf2d) As Vf2d
Public Shared Operator +(lhs As Vf2d) As Vf2d
Public Shared Operator -(lhs As Vf2d) As Vf2d
Public Shared Operator =(lhs As Vf2d, rhs As Vf2d) As Boolean
Public Shared Operator <>(lhs As Vf2d, rhs As Vf2d) As Boolean
Public Shared Widening Operator CType(v As Vf2d) As Vi2d
```

**Examples:**
```vb
Dim a As New Vf2d(10.5F, 20.3F)
Dim b As New Vf2d(5.2F, 15.8F)

' Arithmetic
Dim sum As Vf2d = a + b
Dim diff As Vf2d = a - b
Dim scaled As Vf2d = a * 2.0F

' Scalar operations
Dim added As Vf2d = a + 5.0F  ' Add to both components
Dim multiplied As Vf2d = 3.0F * a

' Products
Dim dot As Integer = a.Dot(b)
Dim cross As Integer = a.Cross(b)

' Normalization
Dim unit As Vf2d = a.Norm()

' Distance
Dim distance As Single = (a - b).Mag()
```