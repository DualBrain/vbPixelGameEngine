# PixelGameEngine Class

The main class of vbPixelGameEngine. All games inherit from this class.

## Properties

### AppName
```vb
Public Property AppName As String
```
The name of the application, displayed in the window title.

## Methods

### Construct
```vb
Public Function Construct(screen_w As UInteger, screen_h As UInteger, pixel_w As UInteger, pixel_h As UInteger, Optional full_screen As Boolean = False, Optional vsync As Boolean = False) As RCode
```
Initializes the engine with screen dimensions and pixel scaling.

**Parameters:**
- `screen_w`: Screen width in pixels
- `screen_h`: Screen height in pixels
- `pixel_w`: Pixel width scaling
- `pixel_h`: Pixel height scaling
- `full_screen`: Whether to run in fullscreen mode
- `vsync`: Whether to enable VSync

**Returns:** `RCode.Ok` on success, `RCode.Fail` on failure

**Example:**
```vb
If game.Construct(800, 600, 1, 1, False, True) Then
    game.Start()
End If
```

### Start
```vb
Public Function Start() As RCode
```
Starts the game loop. This method blocks until the game ends.

**Returns:** `RCode.Ok` on success

### OnUserCreate (Virtual)
```vb
Protected Overridable Function OnUserCreate() As Boolean
```
Called once when the game starts, before the first frame. Use for initialization.

**Returns:** `True` to continue, `False` to exit

**Example:**
```vb
Protected Overrides Function OnUserCreate() As Boolean
    ' Load resources, initialize variables
    Return True
End Function
```

### OnUserUpdate (Virtual)
```vb
Protected Overridable Function OnUserUpdate(fElapsedTime As Single) As Boolean
```
Called every frame. Contains game logic and rendering.

**Parameters:**
- `fElapsedTime`: Time elapsed since last frame in seconds

**Returns:** `True` to continue, `False` to exit

**Example:**
```vb
Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
    ' Handle input
    If GetKey(Key.ESCAPE).Pressed Then Return False

    ' Update game state

    ' Render
    Draw(100, 100, Pixel.White)

    Return True
End Function
```

### OnUserDestroy (Virtual)
```vb
Protected Overridable Sub OnUserDestroy()
```
Called when the game ends. Use for cleanup.

### IsFocused
```vb
Public Function IsFocused() As Boolean
```
Returns whether the game window is currently focused.

**Returns:** `True` if focused

### GetKey
```vb
Public Function GetKey(k As Key) As HWButton
```
Gets the state of a keyboard key.

**Parameters:**
- `k`: The key to check

**Returns:** `HWButton` with `Pressed`, `Released`, `Held` properties

**Example:**
```vb
If GetKey(Key.SPACE).Pressed Then
    ' Jump
End If
```

### GetMouse
```vb
Public Function GetMouse(b As UInteger) As HWButton
```
Gets the state of a mouse button.

**Parameters:**
- `b`: Mouse button (0=left, 1=middle, 2=right)

**Returns:** `HWButton` with `Pressed`, `Released`, `Held` properties

### GetMouseX / GetMouseY / GetMouseWheel
```vb
Public Function GetMouseX() As Integer
Public Function GetMouseY() As Integer
Public Function GetMouseWheel() As Integer
```
Get mouse position and wheel delta.

**Returns:** Mouse coordinates in pixel space, or wheel delta

### ScreenWidth / ScreenHeight
```vb
Public Function ScreenWidth() As Integer
Public Function ScreenHeight() As Integer
```
Get the screen dimensions.

**Returns:** Screen width/height in pixels

### GetDrawTargetWidth / GetDrawTargetHeight / GetDrawTarget
```vb
Public Function GetDrawTargetWidth() As Integer
Public Function GetDrawTargetHeight() As Integer
Public Function GetDrawTarget() As Sprite
```
Get information about the current drawing target.

**Returns:** Dimensions and sprite of current draw target

### SetDrawTarget
```vb
Public Sub SetDrawTarget(target As Sprite)
```
Set the drawing target. Pass `Nothing` to draw to screen.

**Parameters:**
- `target`: Sprite to draw to, or `Nothing` for screen

**Example:**
```vb
Dim buffer As New Sprite(100, 100)
SetDrawTarget(buffer)
' Draw to buffer
SetDrawTarget(Nothing) ' Back to screen
DrawSprite(0, 0, buffer)
```

### SetPixelMode
```vb
Public Sub SetPixelMode(m As Pixel.Mode)
Public Sub SetPixelMode(pixelMode As Func(Of Integer, Integer, Pixel, Pixel, Pixel))
Public Function GetPixelMode() As Pixel.Mode
```
Set the pixel blending mode for drawing operations.

**Parameters:**
- `m`: Pixel mode (Normal, Mask, Alpha, Custom)
- `pixelMode`: Custom blending function

### SetPixelBlend
```vb
Public Sub SetPixelBlend(fBlend As Single)
```
Set the blend factor for alpha blending (0.0 to 1.0).

### Draw
```vb
Public Function Draw(x As Integer, y As Integer, p As Pixel) As Boolean
Public Function Draw(pos As Vi2d, p As Pixel) As Boolean
```
Draw a single pixel.

**Parameters:**
- `x`, `y`: Position
- `pos`: Position vector
- `p`: Pixel color

**Returns:** `True` if pixel was drawn

**Example:**
```vb
Draw(10, 10, Pixel.White)
Draw(New Vi2d(20, 20), Pixel.Red)
```

### DrawLine
```vb
Public Sub DrawLine(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)
Public Sub DrawLine(pos1 As Vi2d, pos2 As Vi2d, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)
```
Draw a line between two points.

**Parameters:**
- `x1`, `y1`, `x2`, `y2`: Start and end coordinates
- `pos1`, `pos2`: Start and end vectors
- `p`: Color
- `pattern`: Bit pattern for dashed lines

### DrawCircle / FillCircle
```vb
Public Sub DrawCircle(x As Integer, y As Integer, radius As Integer, p As Pixel, Optional mask As Byte = &HFF)
Public Sub DrawCircle(pos As Vi2d, radius As Integer, p As Pixel, Optional mask As Byte = &HFF)
Public Sub FillCircle(x As Integer, y As Integer, radius As Integer, p As Pixel)
Public Sub FillCircle(pos As Vi2d, radius As Integer, p As Pixel)
```
Draw or fill circles.

**Parameters:**
- `x`, `y`: Center position
- `pos`: Center vector
- `radius`: Circle radius
- `p`: Color
- `mask`: Octant mask for partial circles

### DrawRect / FillRect
```vb
Public Sub DrawRect(x As Integer, y As Integer, w As Integer, h As Integer, p As Pixel)
Public Sub DrawRect(pos As Vi2d, size As Vi2d, p As Pixel)
Public Sub FillRect(x As Integer, y As Integer, w As Integer, h As Integer, p As Pixel)
Public Sub FillRect(pos As Vi2d, size As Vi2d, p As Pixel)
```
Draw or fill rectangles.

### DrawTriangle / FillTriangle
```vb
Public Sub DrawTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer, p As Pixel)
Public Sub DrawTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d, p As Pixel)
Public Sub FillTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer, p As Pixel)
Public Sub FillTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d, p As Pixel)
```
Draw or fill triangles.

### DrawSprite
```vb
Public Sub DrawSprite(x As Integer, y As Integer, sprite As Sprite, Optional scale As UInteger = 1)
Public Sub DrawSprite(pos As Vi2d, sprite As Sprite, Optional scale As UInteger = 1)
```
Draw a sprite at the specified position.

**Parameters:**
- `x`, `y`: Position
- `pos`: Position vector
- `sprite`: Sprite to draw
- `scale`: Scale factor

### DrawPartialSprite
```vb
Public Sub DrawPartialSprite(x As Integer, y As Integer, sprite As Sprite, ox As Integer, oy As Integer, w As Integer, h As Integer, Optional scale As UInteger = 1)
Public Sub DrawPartialSprite(pos As Vi2d, sprite As Sprite, sourcepos As Vi2d, size As Vi2d, Optional scale As UInteger = 1)
```
Draw a portion of a sprite.

### DrawString
```vb
Public Sub DrawString(x As Integer, y As Integer, sText As String, col As Pixel, Optional scale As UInteger = 1)
Public Sub DrawString(pos As Vi2d, sText As String, col As Pixel, Optional scale As UInteger = 1)
```
Draw text at the specified position.

### Clear
```vb
Public Sub Clear(Optional p As Pixel = Nothing)
```
Clear the screen to a color.

**Parameters:**
- `p`: Color to clear to (default black)

### SetScreenSize
```vb
Public Sub SetScreenSize(w As Integer, h As Integer)
```
Change the screen size at runtime.

## Enums

### Key
```vb
Public Enum Key
    NONE
    A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    K0, K1, K2, K3, K4, K5, K6, K7, K8, K9
    F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12
    UP, DOWN, LEFT, RIGHT
    SPACE, TAB, SHIFT, CTRL, INS, DEL, HOME, END, PGUP, PGDN
    BACK, ESCAPE, RETURN, ENTER, PAUSE, SCROLL
    NP0, NP1, NP2, NP3, NP4, NP5, NP6, NP7, NP8, NP9
    NP_MUL, NP_DIV, NP_ADD, NP_SUB, NP_DECIMAL
End Enum
```

### RCode
```vb
Public Enum RCode
    Fail = 0
    Ok = 1
    NoFile = -1
End Enum
```

## Structures

### HWButton
```vb
Public Structure HWButton
    Public Pressed As Boolean  ' Set once during frame event occurs
    Public Released As Boolean ' Set once during frame event occurs
    Public Held As Boolean     ' Set true for all frames between pressed and released
End Structure
```