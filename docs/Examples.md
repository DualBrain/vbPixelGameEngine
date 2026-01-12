# Examples

The vbPixelGameEngine repository includes numerous examples demonstrating various features and techniques.

## Example Categories

### Graphics
Located in `samples/graphics/`

- **Shapes** - Basic drawing primitives (lines, circles, rectangles)
- **1500_Circles** - Circle rendering and patterns
- **4501_Polys** - Polygon drawing
- **6502_Boxes** - 3D box rendering
- **AlphaSparkles** - Alpha blending effects
- **ArchSpiral** - Mathematical patterns
- **Fireworks** - Particle systems
- **Hyperspace** - Starfield effects

### Games
Located in `samples/games/`

- Complete game implementations
- Platformers, puzzles, arcade games
- Demonstrate full game architecture

### 3D
Located in `samples/3d/`

- 3D graphics examples
- Mesh rendering, cameras, lighting
- OpenGL integration

### Math
Located in `samples/math/`

- Mathematical visualizations
- Fractals, curves, transformations

### Miscellaneous
Located in `samples/misc/`

- Utility examples
- File I/O, networking, etc.

### OLC (OneLoneCoder)
Located in `samples/olc/`

- Ported examples from the original olcPixelGameEngine
- Direct compatibility demonstrations

## Running Examples

1. Open the VB.NET project file (.vbproj) in Visual Studio
2. Build and run
3. Most examples use keyboard controls (see console output or source comments)

## Key Example Patterns

### Basic Game Structure
```vb
Friend Class MyExample
    Inherits PixelGameEngine

    Friend Sub New()
        AppName = "My Example"
    End Sub

    Protected Overrides Function OnUserCreate() As Boolean
        ' Initialization
        Return True
    End Function

    Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
        ' Input handling
        If GetKey(Key.ESCAPE).Pressed Then Return False

        ' Update logic

        ' Render
        Clear(Pixel.Black)
        DrawString(10, 10, "Hello World", Pixel.White)

        Return True
    End Function
End Class
```

### Sprite Usage
```vb
Dim sprite As New Sprite("image.png")

' In OnUserUpdate
DrawSprite(100, 100, sprite)
```

### Input Handling
```vb
' Keyboard
If GetKey(Key.SPACE).Pressed Then
    ' Jump
End If

' Mouse
Dim mouseX = GetMouseX()
Dim mouseY = GetMouseY()
If GetMouse(0).Held Then ' Left mouse button
    Draw(mouseX, mouseY, Pixel.Red)
End If
```

### Timing
```vb
Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    ' elapsedTime is seconds since last frame
    Dim fps = 1.0F / elapsedTime

    ' Smooth movement
    position += velocity * elapsedTime
End Function
```

## Learning Path

1. Start with **Shapes** - Basic drawing
2. Try **AlphaSparkles** - Pixel manipulation
3. Explore **Fireworks** - Particle systems
4. Check out complete games in `samples/games/`
5. Experiment with 3D in `samples/3d/`

Each example includes source code with comments explaining the techniques used.