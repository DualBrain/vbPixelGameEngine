# vbPixelGameEngine API Documentation

vbPixelGameEngine is a VB.NET port of the olcPixelGameEngine, a fast and simple 2D graphics framework for rapid prototyping and game development.

## Overview

The engine provides a pixel-perfect, retro-style graphics API with hardware acceleration support. It's designed for simplicity and performance, allowing developers to focus on game logic rather than low-level graphics programming.

## Key Features

- **Pixel-based Graphics**: Direct pixel manipulation with 32-bit color support
- **Hardware Accelerated**: OpenGL backend for fast rendering
- **Cross-platform**: Supports Windows and Linux
- **Simple API**: Easy to learn and use for rapid prototyping
- **Extensible**: Supports extensions (PGEX) for additional functionality

## Getting Started

To create a game with vbPixelGameEngine:

1. Create a class that inherits from `PixelGameEngine`
2. Override `OnUserCreate()` for initialization
3. Override `OnUserUpdate(fElapsedTime)` for game loop
4. Call `Construct()` and `Start()`

```vb
Friend Class MyGame
    Inherits PixelGameEngine

    Friend Sub New()
        AppName = "My Game"
    End Sub

    Protected Overrides Function OnUserCreate() As Boolean
        ' Initialize game state
        Return True
    End Function

    Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean
        ' Game logic and rendering
        Draw(10, 10, Pixel.White)
        Return True
    End Function
End Class

Sub Main()
    Dim game As New MyGame()
    If game.Construct(800, 600, 1, 1) Then
        game.Start()
    End If
End Sub
```

## API Reference

- [PixelGameEngine Class](PixelGameEngine.md) - Main engine class
- [Pixel Structure](Pixel.md) - Color and pixel data
- [Sprite Class](Sprite.md) - Image handling
- [Vi2d Structure](Vi2d.md) - Integer 2D vector
- [Vf2d Structure](Vf2d.md) - Float 2D vector
- [ResourcePack Class](ResourcePack.md) - Resource management
- [Extensions (PGEX)](Extensions.md) - Additional functionality
- [Examples](Examples.md) - Sample code and usage patterns

## License

vbPixelGameEngine is licensed under the OLC-3 license, matching the original olcPixelGameEngine.