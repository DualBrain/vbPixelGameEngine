# Pixel Structure

Represents a 32-bit RGBA color. Pixels can be manipulated with arithmetic operations and converted between different color spaces.

## Fields

```vb
Public N As UInteger  ' Packed 32-bit value (0xAABBGGRR)
Public R As Byte      ' Red component (0-255)
Public G As Byte      ' Green component (0-255)
Public B As Byte      ' Blue component (0-255)
Public A As Byte      ' Alpha component (0-255)
```

## Constructors

```vb
Public Sub New(red As Single, green As Single, blue As Single, Optional alpha As Single = 255.0F)
Public Sub New(red As Integer, green As Integer, blue As Integer, Optional alpha As Integer = 255)
Public Sub New(p As UInteger) ' From packed value
```

**Examples:**
```vb
' RGB values
Dim white As New Pixel(255, 255, 255)
Dim red As New Pixel(255, 0, 0)

' RGBA values
Dim transparent As New Pixel(255, 0, 0, 128)

' Packed value
Dim color As New Pixel(&HFFFF0000UI) ' Red
```

## Static Methods

### Random / RandomAlpha
```vb
Public Shared Function Random() As Pixel
Public Shared Function RandomAlpha() As Pixel
```
Generate random colors.

**Returns:** Random pixel (opaque or with random alpha)

### FromRgb
```vb
Public Shared Function FromRgb(rgb As UInteger) As Pixel
```
Create pixel from packed RGB value.

**Parameters:**
- `rgb`: Packed color value

**Returns:** Pixel with alpha=255

### FromHsv
```vb
Public Shared Function FromHsv(h As Single, s As Single, v As Single) As Pixel
```
Create pixel from HSV color space.

**Parameters:**
- `h`: Hue (0-360)
- `s`: Saturation (0-1)
- `v`: Value/Brightness (0-1)

**Returns:** Pixel in RGB space

**Example:**
```vb
Dim red As Pixel = Pixel.FromHsv(0, 1, 1)
Dim green As Pixel = Pixel.FromHsv(120, 1, 1)
Dim blue As Pixel = Pixel.FromHsv(240, 1, 1)
```

### Lerp
```vb
Public Shared Function Lerp(p1 As Pixel, p2 As Pixel, t As Single) As Pixel
```
Linear interpolation between two pixels.

**Parameters:**
- `p1`, `p2`: Pixels to interpolate between
- `t`: Interpolation factor (0.0 to 1.0)

**Returns:** Interpolated pixel

## Operators

```vb
Public Shared Operator =(p1 As Pixel, p2 As Pixel) As Boolean
Public Shared Operator <>(p1 As Pixel, p2 As Pixel) As Boolean
Public Shared Operator +(p1 As Pixel, p2 As Pixel) As Pixel
Public Shared Operator *(p1 As Pixel, p2 As Pixel) As Pixel
Public Shared Operator *(p As Pixel, t As Single) As Pixel
```

**Examples:**
```vb
Dim color1 As Pixel = Pixel.Red
Dim color2 As Pixel = Pixel.Blue

' Add colors
Dim mixed As Pixel = color1 + color2

' Scale brightness
Dim dimmed As Pixel = color1 * 0.5F

' Interpolate
Dim gradient As Pixel = Pixel.Lerp(color1, color2, 0.5F)
```

## Presets

```vb
Public Enum Presets As UInteger
    White = &HFFFFFF
    Gray = &HA9A9A9
    Red = &HFF0000
    Yellow = &HFFFF00
    Green = &HFF00
    Cyan = &HFFFF
    Blue = &HFF
    Magenta = &HFF00FF
    Brown = &H9A6324
    Orange = &HF58231
    Purple = &H911EB4
    Lime = &HBFEF45
    Pink = &HFABEBE
    Snow = &HFFFAFA
    Teal = &H469990
    Lavender = &HE6BEFF
    Beige = &HFFFAC8
    Maroon = &H800000
    Mint = &HAAFFC3
    Olive = &H808000
    Apricot = &HFFD8B1
    Navy = &H75
    Black = &HFF000000UI
    DarkGrey = &H8B8B8B
    DarkRed = &H8B0000
    DarkYellow = &H8B8B00
    DarkGreen = &H8B00
    DarkCyan = &H8B8B
    DarkBlue = &H8B
    DarkMagenta = &H8B008B
End Enum
```

**Example:**
```vb
Dim color As Pixel = Pixel.Red
Dim dark As Pixel = Pixel.DarkRed
```

## Mode Enum

```vb
Public Enum Mode
    Normal  ' Default blending
    Mask    ' Only draw if alpha > 0
    Alpha   ' Alpha blending
    Custom  ' Custom blend function
End Enum
```