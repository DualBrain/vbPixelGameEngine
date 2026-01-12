# Extensions (PGEX)

vbPixelGameEngine supports extensions that add additional functionality. Extensions are implemented as separate modules that can be imported.

## Available Extensions

### QB64
Provides QBASIC/QB64 compatible graphics commands.

**Import:** `Imports VbPixelGameEngine.QB64`

**Key Functions:**
- `PSET(x, y)` - Plot a pixel
- `LINE(x1, y1, x2, y2)` - Draw a line
- `CIRCLE(x, y, radius)` - Draw a circle
- `COLOR(c)` - Set drawing color

**Example:**
```vb
Imports VbPixelGameEngine.QB64

' In OnUserUpdate
QB64.COLOR(15) ' White
QB64.PSET(100, 100)
QB64.LINE(0, 0, 100, 100)
```

### Gfx2D
2D graphics utilities including collision detection and geometric operations.

**Import:** `Imports VbPixelGameEngine.Gfx2D`

**Key Functions:**
- Triangle/quad rendering
- Collision detection (point, line, circle, triangle)
- Geometric utilities

### Gfx3D
3D graphics pipeline with meshes, cameras, and lighting.

**Import:** `Imports VbPixelGameEngine.Gfx3D`

**Key Classes:**
- `Mesh` - 3D model data
- `Camera` - View/projection matrices
- `Pipeline` - Rendering pipeline

### SpecBAS
Spectrum BASIC compatible graphics.

**Import:** `Imports VbPixelGameEngine.SpecBAS`

### Stella
Atari 2600 style graphics.

**Import:** `Imports VbPixelGameEngine.Stella`

### QuickGUI
Simple GUI components.

**Import:** `Imports VbPixelGameEngine.PGEX.QuickGui`

**Components:**
- Buttons
- Text boxes
- Sliders
- Checkboxes

**Example:**
```vb
Imports VbPixelGameEngine.PGEX.QuickGui

Dim gui As New QuickGui()
Dim button As New GuiButton("Click me", 10, 10, 100, 20)
gui.AddControl(button)
```

## Creating Extensions

Extensions inherit from `PGEX` and can access the main engine via `pge`.

```vb
Public Class MyExtension
    Inherits PGEX

    Public Sub DoSomething()
        pge.Draw(10, 10, Pixel.Red) ' Access main engine
    End Sub
End Class
```