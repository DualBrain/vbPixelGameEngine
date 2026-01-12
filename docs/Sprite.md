# Sprite Class

Represents a 2D image or drawing surface. Sprites can be loaded from files or created programmatically.

## Constructors

```vb
Public Sub New()
Public Sub New(imageFile As String, Optional pack As ResourcePack = Nothing)
Public Sub New(w As Integer, h As Integer)
```

**Examples:**
```vb
' Empty sprite
Dim sprite As New Sprite()

' Load from file
Dim sprite As New Sprite("image.png")

' Create blank sprite
Dim sprite As New Sprite(100, 100)
```

## Properties

### Width / Height
```vb
Public Property Width As Integer
Public Property Height As Integer
```
The dimensions of the sprite in pixels.

## Methods

### LoadFromFile
```vb
Public Function LoadFromFile(imageFile As String, Optional pack As ResourcePack = Nothing) As PixelGameEngine.RCode
```
Load an image file into the sprite.

**Parameters:**
- `imageFile`: Path to the image file
- `pack`: Optional resource pack

**Returns:** `RCode.Ok` on success

**Supported formats:** PNG (on Windows), custom PGE format

### LoadFromPGESprFile / SaveToPGESprFile
```vb
Public Function LoadFromPGESprFile(imageFile As String, Optional pack As ResourcePack = Nothing) As PixelGameEngine.RCode
Public Function SaveToPGESprFile(imageFile As String) As PixelGameEngine.RCode
```
Load/save sprites in PGE's native format.

**Parameters:**
- `imageFile`: Path to the sprite file
- `pack`: Optional resource pack

**Returns:** `RCode.Ok` on success

### GetPixel / SetPixel
```vb
Public Function GetPixel(x As Integer, y As Integer) As Pixel
Public Function SetPixel(x As Integer, y As Integer, p As Pixel) As Boolean
```
Get or set the color of a pixel.

**Parameters:**
- `x`, `y`: Pixel coordinates
- `p`: Pixel color

**Returns:** For `GetPixel`, the pixel color. For `SetPixel`, `True` if successful.

**Example:**
```vb
' Set a pixel
sprite.SetPixel(10, 10, Pixel.Red)

' Get a pixel
Dim color As Pixel = sprite.GetPixel(10, 10)
```

### Sample / SampleBL
```vb
Public Function Sample(x As Single, y As Single) As Pixel
Public Function SampleBL(u As Single, v As Single) As Pixel
```
Sample the sprite at normalized coordinates.

**Parameters:**
- `x`, `y`: Normalized coordinates (0.0 to 1.0)
- `u`, `v`: Texture coordinates for bilinear sampling

**Returns:** Sampled pixel color

### GetData
```vb
Public ReadOnly Property GetData() As Pixel()
```
Get direct access to the pixel data array.

**Returns:** Array of pixels in row-major order

**Example:**
```vb
Dim pixels As Pixel() = sprite.GetData()
For i = 0 To pixels.Length - 1
    pixels(i) = Pixel.Random()
Next
```

## Mode Enum

```vb
Public Enum Mode
    Normal   ' Clamp to edges
    Periodic ' Wrap around
End Enum
```

Controls how sampling behaves at edges.