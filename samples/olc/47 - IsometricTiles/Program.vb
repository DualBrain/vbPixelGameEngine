' Inspired by "Coding Quickie: Isometric Tiles" -- @javidx9
' https://youtu.be/ukkbNKTgf5U

Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New IsometricDemo
    If demo.Construct(512, 480, 2, 2) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class IsometricDemo
  Inherits PixelGameEngine

  Friend Sub New()
    AppName = "Coding Quickie: Isometric Tiles"
  End Sub

  Private m_worldSize As New Vi2d(14, 10)
  Private m_tileSize As New Vi2d(40, 20)
  Private m_origin As New Vi2d(5, 1)
  Private m_sprIsom As Sprite = Nothing
  Private m_world As Integer() = Nothing

  Protected Overrides Function OnUserCreate() As Boolean
    m_sprIsom = New Sprite("assets/isometric_demo.png")
    m_world = Enumerable.Repeat(0, m_worldSize.x * m_worldSize.y).ToArray()
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Clear(Presets.White)
    Dim vMouse As New Vi2d(GetMouseX(), GetMouseY())
    Dim vCell As New Vi2d(vMouse.x \ m_tileSize.x, vMouse.y \ m_tileSize.y)
    Dim vOffset As New Vi2d(vMouse.x Mod m_tileSize.x, vMouse.y Mod m_tileSize.y)
    Dim col = m_sprIsom.GetPixel(3 * m_tileSize.x + vOffset.x, vOffset.y)

    Dim vSelected As New Vi2d((vCell.y - m_origin.y) + (vCell.x - m_origin.x),
                              (vCell.y - m_origin.y) - (vCell.x - m_origin.x))

    If col = Presets.Red Then vSelected += New Vi2d(-1, 0)
    If col = Presets.Blue Then vSelected += New Vi2d(0, -1)
    If col = Presets.Green Then vSelected += New Vi2d(0, 1)
    If col = Presets.Yellow Then vSelected += New Vi2d(1, 0)

    If GetMouse(0).Pressed Then
      If vSelected.x >= 0 AndAlso vSelected.x < m_worldSize.x AndAlso
         vSelected.y >= 0 AndAlso vSelected.y < m_worldSize.y Then
        m_world(vSelected.y * m_worldSize.x + vSelected.x) += 1
        m_world(vSelected.y * m_worldSize.x + vSelected.x) = m_world(vSelected.y * m_worldSize.x + vSelected.x) Mod 6
      End If
    End If

    Dim ToScreen = Function(x As Integer, y As Integer) As Vi2d
                     Return New Vi2d(CInt(Fix(m_origin.x * m_tileSize.x + (x - y) * (m_tileSize.x / 2))),
                                     CInt(Fix(m_origin.y * m_tileSize.y + (x + y) * (m_tileSize.y / 2))))
                   End Function

    SetPixelMode(Pixel.Mode.Mask)

    ' (0,0) is at top, defined by vOrigin, so draw from top to bottom
    ' to ensure tiles closest to camera are drawn last
    For y = 0 To m_worldSize.y - 1
      For x = 0 To m_worldSize.x - 1

        ' Convert cell coordinate to world space
        Dim vWorld = ToScreen(x, y)

        Select Case m_world(y * m_worldSize.x + x)
          Case 0
            ' Invisble Tile
            DrawPartialSprite(vWorld.x, vWorld.y, m_sprIsom, 1 * m_tileSize.x, 0, m_tileSize.x, m_tileSize.y)
          Case 1
            ' Visible Tile
            DrawPartialSprite(vWorld.x, vWorld.y, m_sprIsom, 2 * m_tileSize.x, 0, m_tileSize.x, m_tileSize.y)
          Case 2
            ' Tree
            DrawPartialSprite(vWorld.x, vWorld.y - m_tileSize.y, m_sprIsom, 0 * m_tileSize.x, 1 * m_tileSize.y, m_tileSize.x, m_tileSize.y * 2)
          Case 3
            ' Spooky Tree
            DrawPartialSprite(vWorld.x, vWorld.y - m_tileSize.y, m_sprIsom, 1 * m_tileSize.x, 1 * m_tileSize.y, m_tileSize.x, m_tileSize.y * 2)
          Case 4
            ' Beach
            DrawPartialSprite(vWorld.x, vWorld.y - m_tileSize.y, m_sprIsom, 2 * m_tileSize.x, 1 * m_tileSize.y, m_tileSize.x, m_tileSize.y * 2)
          Case 5
            ' Water
            DrawPartialSprite(vWorld.x, vWorld.y - m_tileSize.y, m_sprIsom, 3 * m_tileSize.x, 1 * m_tileSize.y, m_tileSize.x, m_tileSize.y * 2)
        End Select
      Next
    Next

    ' Draw Selected Cell - Has varying alpha components
    SetPixelMode(Pixel.Mode.Alpha)

    ' Convert selected cell coordinate to world space
    Dim vSelectedWorld = ToScreen(vSelected.x, vSelected.y)

    ' Draw "highlight" tile
    DrawPartialSprite(vSelectedWorld.x, vSelectedWorld.y, m_sprIsom, 0 * m_tileSize.x, 0, m_tileSize.x, m_tileSize.y)

    ' Go back to normal drawing with no expected transparency
    SetPixelMode(Pixel.Mode.Normal)

    ' Draw Hovered Cell Boundary
    'DrawRect(vCell.x * vTileSize.x, vCell.y * vTileSize.y, vTileSize.x, vTileSize.y, Presets.Red)

    ' Draw Debug Info
    DrawString(4, 4, "Mouse   : " & vMouse.x.ToString() & ", " & vMouse.y.ToString(), Presets.Black)
    DrawString(4, 14, "Cell    : " & vCell.x.ToString() & ", " & vCell.y.ToString(), Presets.Black)
    DrawString(4, 24, "Selected: " & vSelected.x.ToString() & ", " & vSelected.y.ToString(), Presets.Black)

    Return True

  End Function

End Class