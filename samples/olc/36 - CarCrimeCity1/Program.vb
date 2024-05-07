' Inspired by "BIG PROJECT! Top Down City Based Car Crime Game #1" -- @javidx9
' https://youtu.be/mD6b_hP17WI

Imports VbPixelGameEngine
Imports VbPixelGameEngine.Gfx3D
Imports VbPixelGameEngine.Gfx3D.Math

Friend Module Program

  Sub Main()
    Dim game As New CarCrime
    If game.Construct(768, 480, 2, 2) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class CarCrime
  Inherits PixelGameEngine

  Private ReadOnly m_meshCube As New Mesh
  Private ReadOnly m_meshFlat As New Mesh
  Private ReadOnly m_meshWallsOut As New Mesh

  Private ReadOnly m_up As New Vec3d(0, 1, 0)
  Private m_eye As New Vec3d(0, 0, -10) '4)
  Private m_lookDir As New Vec3d(0, 0, 1)

  Private ReadOnly m_pipeRender As New PipeLine

  Private m_sprAll As Sprite
  Private m_sprGround As Sprite
  Private m_sprRoof As Sprite
  Private m_sprFrontage As Sprite
  Private m_sprWindows As Sprite
  Private ReadOnly m_sprRoad(11) As Sprite
  Private m_sprCar As Sprite

  'Private m_theta As Single

  ' Define the cell
  Private Class Cell
    Public Height As Integer = 0
    Public WorldX As Integer = 0
    Public WorldY As Integer = 0
    Public Road As Boolean = False
    Public Building As Boolean = True
  End Class

  ' Map variables
  Private m_mapWidth As Integer
  Private m_mapHeight As Integer
  Private m_map() As Cell

  Private m_cameraX As Single = 0.0F
  Private m_cameraY As Single = 0.0F
  Private m_cameraZ As Single = -10.0F

  Private m_carAngle As Single = 0.0F
  Private ReadOnly m_carSpeed As Single = 2.0F
  Private m_carVel As New Vec3d(0, 0, 0)
  Private ReadOnly m_carPos As New Vec3d(0, 0, 0)

  Private m_mouseWorldX As Integer = 0
  Private m_mouseWorldY As Integer = 0
  Private m_matProj As Mat4x4

  Private ReadOnly m_setSelectedCells As New HashSet(Of Cell)

  Private m_viewWorldTopLeft As Vec3d
  Private m_viewWorldBottomRight As Vec3d

  Friend Sub New()
    AppName = "Car Crime City"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    ' Load Sprite Sheet
    m_sprAll = New Sprite("assets/City_Roads1_mip0.png")

    ' Here we break up the sprite sheet into individual textures. This is more
    ' out of convenience than anything else, as it keeps the texture coordinates
    ' easy to manipulate

    ' Building Lowest Floor
    m_sprFrontage = New Sprite(32, 96)
    SetDrawTarget(m_sprFrontage)
    DrawPartialSprite(0, 0, m_sprAll, 288, 64, 32, 96)

    ' Building Windows
    m_sprWindows = New Sprite(32, 96)
    SetDrawTarget(m_sprWindows)
    DrawPartialSprite(0, 0, m_sprAll, 320, 64, 32, 96)

    ' Plain Grass Field
    m_sprGround = New Sprite(96, 96)
    SetDrawTarget(m_sprGround)
    DrawPartialSprite(0, 0, m_sprAll, 192, 0, 96, 96)

    ' Building Roof
    m_sprRoof = New Sprite(96, 96)
    SetDrawTarget(m_sprRoof)
    DrawPartialSprite(0, 0, m_sprAll, 352, 64, 96, 96)

    ' There are 12 Road Textures, arranged in a 3x4 grid
    For r = 0 To 11
      m_sprRoad(r) = New Sprite(96, 96)
      SetDrawTarget(m_sprRoad(r))
      DrawPartialSprite(0, 0, m_sprAll, (r Mod 3) * 96, (r \ 3) * 96, 96, 96)
    Next

    ' Don't forget to set the draw target back to being the main screen (been there... wasted 1.5 hours :| )
    SetDrawTarget(Nothing)

    ' The Yellow Car
    m_sprCar = New Sprite("assets/car_top.png")

    ' A Full cube - Always useful for debugging
    m_meshCube.Tris = New List(Of Triangle) From {New Triangle({0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F}), ' SOUTH
                                                  New Triangle({0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F}),
                                                                                                                                                                                               _
                                                  New Triangle({1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F}), ' EAST
                                                  New Triangle({1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F}), _
                                                                                                                                                                                                _
                                                  New Triangle({1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F}), ' NORTH
                                                  New Triangle({1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F}), _
                                                                                                                                                                                                _
                                                  New Triangle({0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F}), ' WEST
                                                  New Triangle({0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F}), _
                                                                                                                                                                                                _
                                                  New Triangle({0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F}), ' TOP
                                                  New Triangle({0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F}), _
                                                                                                                                                                                                _
                                                  New Triangle({1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F}), ' BOTTOM
                                                  New Triangle({1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 1.0F})}

    ' A Flat quad
    m_meshFlat.Tris = New List(Of Triangle) From {New Triangle({0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F}),
                                                  New Triangle({0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F})}

    ' The four outer walls of a cell
    m_meshWallsOut.Tris = New List(Of Triangle) From {New Triangle({1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F}), ' EAST
                                                      New Triangle({1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 0.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F}),
                                                                                                                                                                                                   _
                                                      New Triangle({0.0F, 0.0F, 0.2F, 1.0F, 0.0F, 1.0F, 0.2F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F}), ' WEST
                                                      New Triangle({0.0F, 0.0F, 0.2F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F}), _
                                                                                                                                                                                                    _
                                                      New Triangle({0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F}), ' TOP
                                                      New Triangle({0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F}), _
                                                                                                                                                                                                    _
                                                      New Triangle({1.0F, 0.0F, 0.2F, 1.0F, 0.0F, 0.0F, 0.2F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F}), ' BOTTOM
                                                      New Triangle({1.0F, 0.0F, 0.2F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F})}

    ' Initialize the 3D Graphics PGE Extension. This is required
    ' to setup internal buffers to the same size as the main output
    ConfigureDisplay()

    ' Configure the rendering pipeline with projection and viewport properties
    m_pipeRender.SetProjection(90.0F, CSng(ScreenHeight / ScreenWidth), 0.1F, 1000.0F, 0.0F, 0.0F, ScreenWidth, ScreenHeight)

    m_matProj = Mat_MakeProjection(90.0F, CSng(ScreenHeight / ScreenWidth), 0.1F, 1000.0F)

    ' Define the city map, a 64x32 array of Cells. Initialize cells to be just grass fields.
    m_mapWidth = 64
    m_mapHeight = 32
    ReDim m_map(m_mapWidth * m_mapHeight)
    For x = 0 To m_mapWidth - 1
      For y = 0 To m_mapHeight - 1
        m_map(y * m_mapWidth + x) = New Cell With {.Height = 0, .Road = False, .WorldX = x, .WorldY = y}
      Next
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    'fTheta += elapsedTime

    'If GetKey(Key.W).Held Then fCameraY -= 2.0F * elapsedTime
    'If GetKey(Key.S).Held Then fCameraY += 2.0F * elapsedTime
    'If GetKey(Key.A).Held Then fCameraX -= 2.0F * elapsedTime
    'If GetKey(Key.D).Held Then fCameraX += 2.0F * elapsedTime
    If GetKey(Key.X).Held Then m_cameraZ -= 5.0F * elapsedTime
    If GetKey(Key.Z).Held Then m_cameraZ += 5.0F * elapsedTime

    If GetKey(Key.LEFT).Held Then m_carAngle -= 4.0F * elapsedTime
    If GetKey(Key.RIGHT).Held Then m_carAngle += 4.0F * elapsedTime

    Dim a = New Vec3d(1, 0, 0)
    Dim m = Mat_MakeRotationZ(m_carAngle)
    m_carVel = Mat_MultiplyVector(m, a)

    If GetKey(Key.UP).Held Then
      m_carPos.X += m_carVel.X * m_carSpeed * elapsedTime
      m_carPos.Y += m_carVel.Y * m_carSpeed * elapsedTime
    End If

    If m_mouseWorldX >= 0 AndAlso m_mouseWorldX < m_mapWidth AndAlso m_mouseWorldY >= 0 AndAlso m_mouseWorldY < m_mapHeight Then

      ' Press "R" to toggle a Road flag for selected cell(s)
      If GetKey(Key.R).Pressed Then
        If m_setSelectedCells.Count <> 0 Then
          For Each cell In m_setSelectedCells
            cell.Road = Not cell.Road
          Next
        Else
          m_map(m_mouseWorldY * m_mapWidth + m_mouseWorldX).Road = Not m_map(m_mouseWorldY * m_mapWidth + m_mouseWorldX).Road
        End If
      End If

      ' Press "T" to raise a building...
      If GetKey(Key.T).Pressed Then
        If m_setSelectedCells.Count <> 0 Then
          For Each cell In m_setSelectedCells
            cell.Height += 1
          Next
        Else
          m_map(m_mouseWorldY * m_mapWidth + m_mouseWorldX).Height += 1
        End If
      End If

      ' Press "E" to lower a building...
      If GetKey(Key.E).Pressed Then
        If m_setSelectedCells.Count <> 0 Then
          For Each cell In m_setSelectedCells
            cell.Height -= 1
          Next
        Else
          m_map(m_mouseWorldY * m_mapWidth + m_mouseWorldX).Height -= 1
        End If
      End If

    End If

    Clear(Presets.Blue)
    ClearDepth()

    Dim vLookTarget = Vec_Add(m_eye, m_lookDir)

    ' Setup the camera properties for the pipeline - aka "view" transform
    m_cameraX = m_carPos.X
    m_cameraY = m_carPos.Y
    m_eye = New Vec3d(m_cameraX, m_cameraY, m_cameraZ)
    m_pipeRender.SetCamera(m_eye, vLookTarget, m_up)

    ' Create a point at matrix, if you recall, this is the inverse of the look at matrix
    ' used by the camera
    Dim matView = Mat_PointAt(m_eye, vLookTarget, m_up)

    ' Assume the origin of the mouse ray is the middle of the screen...
    Dim vecMouseOrigin = New Vec3d(0.0F, 0.0F, 0.0F)

    ' ... and that a ray is cast to the mouse location from the origin. Here we translate
    ' the mouse coordinates into viewport coordinates
    Dim vecMouseDir = New Vec3d(2.0F * (CSng(GetMouseX() / ScreenWidth) - 0.5F) / m_matProj.M(0, 0),
                                2.0F * (CSng(GetMouseY() / ScreenHeight) - 0.5F) / m_matProj.M(1, 1),
                                1.0F,
                                0.0F)

    ' Now transform the origin point and ray direction by the inverse of the camera
    vecMouseOrigin = Mat_MultiplyVector(matView, vecMouseOrigin)
    vecMouseDir = Mat_MultiplyVector(matView, vecMouseDir)

    ' Extend the mouse ray to a large length
    vecMouseDir = Vec_Mul(vecMouseDir, 1000.0F)

    ' Offset the mouse ray by the mouse origin
    vecMouseDir = Vec_Add(vecMouseOrigin, vecMouseDir)

    ' All of our intersections for mouse checks occur in the ground plane (z=0), so
    ' define a plane at that location
    Dim plane_p = New Vec3d(0.0F, 0.0F, 0.0F)
    Dim plane_n = New Vec3d(0.0F, 0.0F, 1.0F)

    ' Calculate Mouse Location in plane, by doing a line/plane intersection test
    Dim t = 0.0F
    Dim mouse3d = Vec_IntersectPlane(plane_p, plane_n, vecMouseOrigin, vecMouseDir, t)

    m_mouseWorldX = CInt(Fix(mouse3d.X))
    m_mouseWorldY = CInt(Fix(mouse3d.Y))

    If GetMouse(0).Held Then
      m_setSelectedCells.Add(m_map(m_mouseWorldY * m_mapWidth + m_mouseWorldX))
    End If
    If GetMouse(1).Released Then
      m_setSelectedCells.Clear()
    End If

    ' Work out the Top Left Ground Cell
    vecMouseDir = New Vec3d(-1.0F / m_matProj.M(0, 0), -1.0F / m_matProj.M(1, 1), 1.0F, 0.0F)
    vecMouseDir = Mat_MultiplyVector(matView, vecMouseDir)
    vecMouseDir = Vec_Mul(vecMouseDir, 1000.0F)
    vecMouseDir = Vec_Add(vecMouseOrigin, vecMouseDir)
    m_viewWorldTopLeft = Vec_IntersectPlane(plane_p, plane_n, vecMouseOrigin, vecMouseDir, t)

    ' Work out the Bottom Right Ground Cell
    vecMouseDir = New Vec3d(1.0F / m_matProj.M(0, 0), 1.0F / m_matProj.M(1, 1), 1.0F, 0.0F)
    vecMouseDir = Mat_MultiplyVector(matView, vecMouseDir)
    vecMouseDir = Vec_Mul(vecMouseDir, 1000.0F)
    vecMouseDir = Vec_Add(vecMouseOrigin, vecMouseDir)
    m_viewWorldBottomRight = Vec_IntersectPlane(plane_p, plane_n, vecMouseOrigin, vecMouseDir, t)

    Dim nStartX = Integer.Max(0, CInt(Fix(m_viewWorldTopLeft.X - 1)))
    Dim nEndX = Integer.Min(m_mapWidth, CInt(Fix(m_viewWorldBottomRight.X + 1)))
    Dim nStartY = Integer.Max(0, CInt(Fix(m_viewWorldTopLeft.Y - 1)))
    Dim nEndY = Integer.Min(m_mapHeight, CInt(Fix(m_viewWorldBottomRight.Y + 1)))

    For x = nStartX To nEndX - 1

      Dim ix = x

      For y = nStartY To nEndY - 1

        Dim iy = y

        If m_map(y * m_mapWidth + x).Road Then

          Dim road = 0

          Dim r = Function(i As Integer, j As Integer) As Boolean
                    Return m_map((iy + j) * m_mapWidth + (ix + i)).Road
                  End Function

          If r(0, -1) AndAlso r(0, +1) AndAlso Not r(-1, 0) AndAlso Not r(+1, 0) Then road = 0
          If Not r(0, -1) AndAlso Not r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then road = 1

          If Not r(0, -1) AndAlso r(0, +1) AndAlso Not r(-1, 0) AndAlso r(+1, 0) Then road = 3
          If Not r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then road = 4

          If Not r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso Not r(+1, 0) Then road = 5

          If r(0, -1) AndAlso r(0, +1) AndAlso Not r(-1, 0) AndAlso r(+1, 0) Then road = 6
          If r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then road = 7
          If r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso Not r(+1, 0) Then road = 8

          If r(0, -1) AndAlso Not r(0, +1) AndAlso Not r(-1, 0) AndAlso r(+1, 0) Then road = 9
          If r(0, -1) AndAlso Not r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then road = 10
          If r(0, -1) AndAlso Not r(0, +1) AndAlso r(-1, 0) AndAlso Not r(+1, 0) Then road = 11

          ' Set the appropriate texture to use
          m_pipeRender.SetTexture(m_sprRoad(road))
          Dim matWorld = Mat_MakeTranslation(x, y, 0.0F)
          m_pipeRender.SetTransform(matWorld)
          m_pipeRender.Render(m_meshFlat.Tris)

        Else

          If m_map(y * m_mapWidth + x).Height < 0 Then
            ' Water?
          End If

          If m_map(y * m_mapWidth + x).Height = 0 Then
            ' Cell is ground, draw a flat grass quad at height 0
            Dim matWorld = Mat_MakeTranslation(x, y, 0.0F)
            m_pipeRender.SetTransform(matWorld)
            m_pipeRender.SetTexture(m_sprGround)
            m_pipeRender.Render(m_meshFlat.Tris)
          End If

          If m_map(y * m_mapWidth + x).Height > 0 Then

            ' Cell is a Building, for now, we'll draw each story as a seperate mesh
            For h = 0 To m_map(y * m_mapWidth + x).Height - 1

              ' Create a transform that positions the story according to its height
              Dim matWorld = Mat_MakeTranslation(x, y, -(h + 1) * 0.2F)
              m_pipeRender.SetTransform(matWorld)

              ' Choose a texture, if its ground level, use the "street level front", otherwise use windows
              m_pipeRender.SetTexture(If(h = 0, m_sprFrontage, m_sprWindows))
              m_pipeRender.Render(m_meshWallsOut.Tris)

            Next

            If True Then
              ' Top the building off with a roof
              Dim h = m_map(y * m_mapWidth + x).Height
              Dim matworld = Mat_MakeTranslation(x, y, -(h) * 0.2F)
              m_pipeRender.SetTransform(matworld)
              m_pipeRender.SetTexture(m_sprRoof)
              m_pipeRender.Render(m_meshFlat.Tris)
            End If

          End If

        End If
      Next
    Next

    ' Draw Selected Cells, iterate through the set of cells, and draw a wireframe quad at ground level
    ' to indicate it is in the selection set
    For Each cell In m_setSelectedCells
      Dim matWorld = Mat_MakeTranslation(cell.WorldX, cell.WorldY, 0.0F)
      m_pipeRender.SetTransform(matWorld)
      m_pipeRender.Render(m_meshFlat.Tris, RenderFlags.RenderWire)
    Next

    ' Draw Car, a few transforms required for this

    ' 1) Offset the car to the middle of the quad
    Dim matCarOffset = Mat_MakeTranslation(-0.5F, -0.5F, -0.0F)
    ' 2) The quad is currently unit square, scale it to be more rectangular and smaller than the cells
    Dim matCarScale = Mat_MakeScale(0.4F, 0.2F, 1.0F)
    ' 3) Combine into matrix
    Dim matCar = Mat_MultiplyMatrix(matCarOffset, matCarScale)
    ' 4) Rotate the car around its offset origin, according to its angle
    Dim matCarRot = Mat_MakeRotationZ(m_carAngle)
    matCar = Mat_MultiplyMatrix(matCar, matCarRot)
    ' 5) Translate the car into its position in the world. Give it a little elevation so its above the ground
    Dim matCarTrans = Mat_MakeTranslation(m_carPos.X, m_carPos.Y, -0.01F)
    matCar = Mat_MultiplyMatrix(matCar, matCarTrans)

    ' Apply "world" transform to pipeline
    m_pipeRender.SetTransform(matCar)
    ' Set the car texture to the pipeline
    m_pipeRender.SetTexture(m_sprCar)

    ' The car has transparency, so enable it
    SetPixelMode(Pixel.Mode.Alpha)
    ' Render the quad
    m_pipeRender.Render(m_meshFlat.Tris)
    ' Set transparency back to none to optimise drawing other pixels
    SetPixelMode(Pixel.Mode.Normal)

    Return True

  End Function

End Class