Imports VbPixelGameEngine

Public Class CarCrimeCity
  Inherits PixelGameEngine

  Private ReadOnly m_mapAssetTextures As New Dictionary(Of String, Sprite)
  Private ReadOnly m_mapAssetMeshes As New Dictionary(Of String, Gfx3D.Mesh)
  Private ReadOnly m_mapAssetTransform As New Dictionary(Of String, Gfx3D.Mat4x4)

  ' Camera variables
  Private ReadOnly m_camera As New Gfx3D.Vec3d(0.0F, 0.0F, -3.0F)
  Private ReadOnly m_up As New Gfx3D.Vec3d(0.0F, 1.0F, 0.0F)
  Private m_eye As New Gfx3D.Vec3d(0.0F, 0.0F, -3.0F)
  Private m_lookDir As New Gfx3D.Vec3d(0.0F, 0.0F, 1.0F)

  ' Ray Casting Parameters
  Private m_viewWorldTopLeft As Vf2d
  Private m_viewWorldBottomRight As Vf2d

  ' Cloud movement variables
  'Private m_cloudOffsetX As Single = 0.0F
  'Private m_cloudOffsetY As Single = 0.0F

  ' Mouse Control
  Private m_offset As New Vf2d(0.0F, 0.0F)
  Private m_startPan As New Vf2d(0.0F, 0.0F)
  'Private m_mouseOnGround As New Vf2d(0.0F, 0.0F)
  'Private m_scale As Single = 1.0F

  Private ReadOnly m_listAutomata As New List(Of AutoBody)()

  Private m_city As CityMap = Nothing

  'Private m_globalTime As Single = 0.0F

  ' Editing Utilities
  Private m_editMode As Boolean = True
  Private m_mouseX As Integer = 0
  Private m_mouseY As Integer = 0

  Structure CellLoc
    Public X As Integer
    Public Y As Integer
  End Structure

  Private ReadOnly m_setSelectedCells As New HashSet(Of Integer)

  Private m_carVel As Vf2d
  Private m_carPos As Vf2d
  'Private m_speed As Single = 0.0F
  Private m_angle As Single = 0.0F

  'Private goCar As cGameObjectQuad = Nothing
  'Private goObstacle As cGameObjectQuad = Nothing

  'Private vecObstacles As New List(Of cGameObjectQuad)

  'Private nTrafficState As Integer = 0

  Friend Sub New()
    AppName = "Car Crime City"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    ' Initialise PGEX 3D
    Gfx3D.ConfigureDisplay()

    ' Load fixed system assets, i.e. those need to simply do anything
    If Not LoadAssets() Then
      Return False
    End If

    ' Create Default city
    m_city = New CityMap(GameSettings.DefaultMapWidth, GameSettings.DefaultMapHeight, m_mapAssetTextures, m_mapAssetMeshes, m_mapAssetTransform)

    ' If a city map file has been specified, then load it
    If Not String.IsNullOrEmpty(GameSettings.DefaultCityFile) Then
      If Not m_city.LoadCity(GameSettings.DefaultCityFile) Then
        Console.WriteLine("Failed to load '{0}'", GameSettings.DefaultCityFile)
        Return False
      End If
    End If

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    'm_globalTime += elapsedTime

    If (GetKey(Key.TAB).Released) Then m_editMode = Not m_editMode

    If (m_editMode) Then ' Use mouse to pan and zoom, and place objects

      m_eye = m_camera
      Dim mouseScreen = New Vf2d(GetMouseX(), GetMouseY())
      Dim mouseOnGroundBeforeZoom = GetMouseOnGround(mouseScreen)

      m_offset = New Vf2d(0, 0)

      If (IsFocused()) Then

        If (GetMouse(2).Pressed) Then m_startPan = mouseOnGroundBeforeZoom
        If (GetMouse(2).Held) Then m_offset = (m_startPan - mouseOnGroundBeforeZoom)

        If (GetMouseWheel() > 0) Then
          m_camera.Z *= 0.5F
        End If

        If (GetMouseWheel() < 0) Then
          m_camera.Z *= 1.5F
        End If
      End If

      m_eye = m_camera
      Dim mouseOnGroundAfterZoom = GetMouseOnGround(mouseScreen)
      m_offset += (mouseOnGroundBeforeZoom - mouseOnGroundAfterZoom)
      m_camera.X += m_offset.x
      m_camera.Y += m_offset.y
      m_eye = m_camera

      ' Get Integer versions of mouse coords in world space
      m_mouseX = CInt(Fix(mouseOnGroundAfterZoom.x))
      m_mouseY = CInt(Fix(mouseOnGroundAfterZoom.y))

      DoEditMode(elapsedTime)

    Else

      ' Not in edit mode, so camera follows player
      If (GetKey(Key.LEFT).Held) Then m_angle += -2.5F * elapsedTime
      If (GetKey(Key.RIGHT).Held) Then m_angle += 2.5F * elapsedTime
      If (GetKey(Key.UP).Held) Then
        m_carVel = New Vf2d(MathF.Cos(m_angle), MathF.Sin(m_angle))
        m_carPos += m_carVel * 2.0F * elapsedTime
      End If

      m_camera.X = m_carPos.x
      m_camera.Y = m_carPos.y
      m_eye = m_camera

    End If

    ' Calculate Visible ground plane dimensions
    m_viewWorldTopLeft = GetMouseOnGround(New Vf2d(0.0F, 0.0F))
    m_viewWorldBottomRight = GetMouseOnGround(New Vf2d(CSng(ScreenWidth()), CSng(ScreenHeight())))

    ' Calculate visible world extents
    Dim startX = Integer.Max(0, CInt(m_viewWorldTopLeft.x) - 1)
    Dim endX = Integer.Min(m_city.GetWidth(), CInt(m_viewWorldBottomRight.x) + 1)
    Dim startY = Integer.Max(0, CInt(m_viewWorldTopLeft.y) - 1)
    Dim endY = Integer.Min(m_city.GetHeight(), CInt(m_viewWorldBottomRight.y) + 1)

    ' Only update automata for cells near player
    Dim automStartX = Integer.Max(0, CInt(m_viewWorldTopLeft.x) - 3)
    Dim automEndX = Integer.Min(m_city.GetWidth(), CInt(m_viewWorldBottomRight.x) + 3)
    Dim automStartY = Integer.Max(0, CInt(m_viewWorldTopLeft.y) - 3)
    Dim automEndY = Integer.Min(m_city.GetHeight(), CInt(m_viewWorldBottomRight.y) + 3)

    Dim localStartX = Integer.Max(0, CInt(m_camera.X) - 3)
    Dim localEndX = Integer.Min(m_city.GetWidth(), CInt(m_camera.X) + 3)
    Dim localStartY = Integer.Max(0, CInt(m_camera.Y) - 3)
    Dim localEndY = Integer.Min(m_city.GetHeight(), CInt(m_camera.Y) + 3)

    ' Update Cells
    For x = startX To endX - 1
      For y = startY To endY - 1
        m_city.Cell(x, y).Update(elapsedTime)
      Next
    Next

    '' Update Automata
    'For Each a In listAutomata

    '  a.UpdateAuto(elapsedTime)

    '  ' If automata is too far from camera, remove it
    '  If (a.vAutoPos - New Vf2d(vCamera.X, vCamera.Y)).Mag() > 5.0F Then
    '    ' Despawn automata

    '    ' 1) Disconnect it from track
    '    a.pCurrentTrack.listAutos.Remove(a)

    '    ' 2) Erase it from memory
    '    a = Nothing
    '  End If

    'Next

    '' Remove dead automata, their pointer has been set to Nothing in the list
    'listAutomata.RemoveAll(Function(a) a Is Nothing)

    '' Maintain a certain level of automata in vicinity of player
    'If listAutomata.Count < 20 Then

    '  Dim bSpawnOK = False
    '  Dim nSpawnAttempt = 20

    '  While Not bSpawnOK AndAlso nSpawnAttempt > 0

    '    ' Find random cell on edge of vicinity, which is out of view of the player
    '    Dim fRandomAngle As Single = (CSng(Rnd()) * 2.0F * 3.14159F)
    '    Dim nRandomCellX As Integer = CInt(vCamera.X + MathF.Cos(fRandomAngle) * 3.0F)
    '    Dim nRandomCellY As Integer = CInt(vCamera.Y + MathF.Sin(fRandomAngle) * 3.0F)

    '    nSpawnAttempt -= 1

    '    If pCity.Cell(nRandomCellX, nRandomCellY) IsNot Nothing AndAlso pCity.Cell(nRandomCellX, nRandomCellY).nCellType = CellType.CELL_ROAD Then

    '      bSpawnOK = True

    '      ' Add random automata
    '      If CInt(Rnd() * 100) < 50 Then
    '        ' Spawn Pedestrian
    '        SpawnPedestrian(nRandomCellX, nRandomCellY)
    '      Else
    '        ' Spawn Vehicle
    '        SpawnVehicle(nRandomCellX, nRandomCellY)
    '        ' TODO: Get % chance of vehicle spawn from lua script
    '      End If

    '    End If

    '  End While

    'End If

    ' Render Scene
    Clear(Presets.Blue)
    Gfx3D.ClearDepth()

    ' Create rendering pipeline
    Dim pipe As New Gfx3D.PipeLine()
    pipe.SetProjection(90.0F, CSng(ScreenHeight() / ScreenWidth()), 0.1F, 1000.0F, 0.0F, 0.0F, ScreenWidth(), ScreenHeight())
    Dim lookTarget = Gfx3D.Math.Vec_Add(m_eye, m_lookDir)
    pipe.SetCamera(m_eye, lookTarget, m_up)

    ' Add global illumination vector (sunlight)
    Dim lightDir As New Gfx3D.Vec3d(1.0F, 1.0F, -1.0F)
    pipe.SetLightSource(0, Gfx3D.Light.Ambient, New Pixel(100, 100, 100), New Gfx3D.Vec3d(0, 0, 0), lightDir)
    pipe.SetLightSource(1, Gfx3D.Light.Directional, Presets.White, New Gfx3D.Vec3d(0, 0, 0), lightDir)

    ' RENDER CELL CONTENTS

    ' Render Base Objects (those without alpha components)
    For x = startX To endX - 1
      For y = startY To endY - 1
        m_city.Cell(x, y).DrawBase(Me, pipe)
      Next
    Next

    ' Render Upper Objects (those with alpha components)
    For x = startX To endX - 1
      For y = startY To endY - 1
        m_city.Cell(x, y).DrawAlpha(Me, pipe)
      Next
    Next

    If m_editMode Then
      ' Render additional per cell debug information
      For x = startX To endX - 1
        For y = startY To endY - 1
          m_city.Cell(x, y).DrawDebug(Me, pipe)
        Next
      Next
    End If

    If m_editMode Then
      ' Draw Selections
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        Dim matWorld1 = Gfx3D.Math.Mat_MakeTranslation(CSng(x), CSng(y), 0.01F)
        pipe.SetTransform(matWorld1)
        pipe.Render(m_mapAssetMeshes("UnitQuad").Tris, Gfx3D.RenderFlags.RenderWire)
      Next
    End If

    ' RENDER AUTOMATA

    Dim test() = {"Sedan", "SUV", "TruckCab", "TruckTrailer", "UTE", "Wagon"}
    'Dim i = 0
    For Each a In m_listAutomata

      Dim v = New Gfx3D.Vec3d(a.m_autoPos.x, a.m_autoPos.y, 0.0F)

      'Dim matWorld As GFX3D.mat4x4 = GFX3D.Math.Mat_MakeTranslation(a.vAutoPos.x, a.vAutoPos.y, 0.01F)
      'matWorld = GFX3D.Math.Mat_MultiplyMatrix(mapAssetTransform(test(i)), matWorld)
      'pipe.SetTransform(matWorld)
      'pipe.SetTexture(mapAssetTextures(test(i)))
      'pipe.Render(mapAssetMeshes(test(i)).tris, GFX3D.RENDER_CULL_CW Or GFX3D.RENDER_DEPTH Or GFX3D.RENDER_TEXTURED Or GFX3D.RENDER_LIGHTS)
      'i += 1
      'i = i Mod 6

      pipe.RenderCircleXZ(v, If(a.m_autoLength < 0.1F, 0.05F, 0.07F), If(a.m_autoLength < 0.1F, Presets.Magenta, Presets.Yellow))

    Next

    ' Draw Player Vehicle
    Dim matRotateZ = Gfx3D.Math.Mat_MakeRotationZ(m_angle)
    Dim matTranslate = Gfx3D.Math.Mat_MakeTranslation(m_carPos.x, m_carPos.y, 0.01F)
    Dim matWorld = Gfx3D.Math.Mat_MultiplyMatrix(m_mapAssetTransform("Sedan"), matRotateZ)
    matWorld = Gfx3D.Math.Mat_MultiplyMatrix(matWorld, matTranslate)
    pipe.SetTransform(matWorld)
    pipe.SetTexture(m_mapAssetTextures("Sedan"))
    pipe.Render(m_mapAssetMeshes("Sedan").Tris, Gfx3D.RenderFlags.RenderCullCw Or Gfx3D.RenderFlags.RenderDepth Or Gfx3D.RenderFlags.RenderTextured Or Gfx3D.RenderFlags.RenderLights)

    DrawString(10, 10, "Automata: " & m_listAutomata.Count.ToString(), Presets.White)
    DrawString(10, 20, $"Car: {m_carPos.x},{m_carPos.y}", Presets.White)

    If GetKey(Key.ESCAPE).Pressed Then
      Return False
    End If

    Return True

  End Function

  Sub SpawnPedestrian(x As Integer, y As Integer)
    Dim cell = m_city.Cell(x, y)
    Dim t = TryCast(cell, CellRoad)?.m_safePedestrianTrack
    If t Is Nothing Then Return
    Dim a = New AutoBody With {.m_autoLength = 0.05F, .m_currentTrack = t}
    t.m_listAutos.Add(a)
    a.m_trackOriginNode = t.m_node(0)
    a.UpdateAuto(0.0F)
    m_listAutomata.Add(a)
  End Sub

  Sub SpawnVehicle(x As Integer, y As Integer)
    Dim cell = m_city.Cell(x, y)
    Dim t = TryCast(cell, CellRoad)?.m_safeCarTrack
    If t Is Nothing Then Return
    Dim a = New AutoBody With {.m_autoLength = 0.2F, .m_currentTrack = t}
    t.m_listAutos.Add(a)
    a.m_trackOriginNode = t.m_node(0)
    a.UpdateAuto(0.0F)
    m_listAutomata.Add(a)
  End Sub

  Sub DoEditMode(elapsedTime As Single)

    If elapsedTime > 0 Then
    End If

    ' Get cell under mouse cursor
    Dim mcell = m_city.Cell(m_mouseX, m_mouseY)
    Dim tempCellAdded = False

    ' Left click and drag adds cells
    If mcell IsNot Nothing AndAlso GetMouse(0).Held Then
      m_setSelectedCells.Add(m_mouseY * m_city.GetWidth() + m_mouseX)
    End If

    ' Right click clears selection
    If GetMouse(1).Released Then
      m_setSelectedCells.Clear()
    End If

    If m_setSelectedCells.Count = 0 Then
      ' If nothing can be edited validly then just exit
      If mcell Is Nothing Then
        Return
      End If
      ' else set is empty, so temporarily add current cell to it
      m_setSelectedCells.Add(m_mouseY * m_city.GetWidth() + m_mouseX)
      tempCellAdded = True
    End If

    ' If the map changes, we will need to update
    ' the automata, and adjacency
    Dim mapChanged = False

    ' Press "G" to apply grass
    If GetKey(Key.G).Pressed Then
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        Dim cell = m_city.Replace(x, y, New CellPlane(m_city, x, y, CellPlaneType.Grass))
        cell.LinkAssets(m_mapAssetTextures, m_mapAssetMeshes, m_mapAssetTransform)
      Next
      mapChanged = True
    End If

    ' Press "P" to apply Pavement
    If GetKey(Key.P).Pressed Then
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        Dim cell = m_city.Replace(x, y, New CellPlane(m_city, x, y, CellPlaneType.Asphalt))
        cell.LinkAssets(m_mapAssetTextures, m_mapAssetMeshes, m_mapAssetTransform)
      Next
      mapChanged = True
    End If

    ' Press "W" to apply Water
    If GetKey(Key.W).Pressed Then
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        Dim cell = m_city.Replace(x, y, New CellWater(m_city, x, y))
        cell.LinkAssets(m_mapAssetTextures, m_mapAssetMeshes, m_mapAssetTransform)
      Next
      mapChanged = True
    End If

    ' Press "Q" to apply Buildings
    If GetKey(Key.Q).Pressed Then
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        Dim cell = m_city.Replace(x, y, New CellBuilding("Apartments_1", m_city, x, y))
        cell.LinkAssets(m_mapAssetTextures, m_mapAssetMeshes, m_mapAssetTransform)
      Next
      mapChanged = True
    End If

    ' Press "R" to apply Roads
    If GetKey(Key.R).Pressed Then
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        Dim cell = m_city.Replace(x, y, New CellRoad(m_city, x, y))
        cell.LinkAssets(m_mapAssetTextures, m_mapAssetMeshes, m_mapAssetTransform)
      Next
      mapChanged = True
    End If

    If GetKey(Key.C).Pressed Then
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        SpawnVehicle(x, y)
      Next
    End If

    If GetKey(Key.V).Pressed Then
      For Each c In m_setSelectedCells
        Dim x = c Mod m_city.GetWidth()
        Dim y = c \ m_city.GetWidth()
        SpawnPedestrian(x, y)
      Next
    End If

    If mapChanged Then
      ' The navigation nodes may have tracks attached to them, so get rid of them
      ' all. Below we will reconstruct all tracks because city has changed
      m_city.RemoveAllTracks()
      'For Each a In m_listAutomata
      '  a = Nothing '.Dispose()
      'Next
      For i = 0 To m_listAutomata.Count - 1
        m_listAutomata(i) = Nothing
      Next
      m_listAutomata.Clear()
      For x = 0 To m_city.GetWidth() - 1
        For y = 0 To m_city.GetHeight() - 1
          Dim c = m_city.Cell(x, y)
          ' Update adjacency information, i.e. those cells whose
          ' state changes based on neighbouring cells
          c.CalculateAdjacency()
        Next
      Next
    End If

    ' To facilitate "edit under cursor" we added a temporary cell
    ' which needs to be removed now
    If tempCellAdded Then
      m_setSelectedCells.Clear()
    End If

  End Sub

  Function GetMouseOnGround(mouseScreen As Vf2d) As Vf2d
    Dim lookTarget = Gfx3D.Math.Vec_Add(m_eye, m_lookDir)
    Dim matProj = Gfx3D.Math.Mat_MakeProjection(90.0F, CSng(ScreenHeight()) / CSng(ScreenWidth()), 0.1F, 1000.0F)
    Dim matView = Gfx3D.Math.Mat_PointAt(m_eye, lookTarget, m_up)
    Dim mouseDir As New Gfx3D.Vec3d(2.0F * ((mouseScreen.x / CSng(ScreenWidth())) - 0.5F) / matProj.M(0, 0),
                                 2.0F * ((mouseScreen.y / CSng(ScreenHeight())) - 0.5F) / matProj.M(1, 1),
                                 1.0F, 0.0F)
    Dim mouseOrigin As New Gfx3D.Vec3d(0.0F, 0.0F, 0.0F)
    mouseOrigin = Gfx3D.Math.Mat_MultiplyVector(matView, mouseOrigin)
    mouseDir = Gfx3D.Math.Mat_MultiplyVector(matView, mouseDir)
    mouseDir = Gfx3D.Math.Vec_Mul(mouseDir, 1000.0F)
    mouseDir = Gfx3D.Math.Vec_Add(mouseOrigin, mouseDir)
    Dim plane_p As New Gfx3D.Vec3d(0.0F, 0.0F, 0.0F)
    Dim plane_n As New Gfx3D.Vec3d(0.0F, 0.0F, 1.0F)
    Dim t = 0.0F
    Dim mouse3d = Gfx3D.Math.Vec_IntersectPlane(plane_p, plane_n, mouseOrigin, mouseDir, t)
    Return New Vf2d(mouse3d.X, mouse3d.Y)
  End Function

  Function LoadAssets() As Boolean
    ' Game Settings should have loaded all the relevant file information
    ' to start loading asset information. Game assets will be stored in
    ' a map structure. Maps can have slightly longer access times, so each
    ' in game object will have facility to extract required resources once
    ' when it is created, meaning no map search during normal use  End Function

    ' System Meshes
    ' A simple flat unit quad
    Dim meshQuad As New Gfx3D.Mesh With {
      .Tris = New List(Of Gfx3D.Triangle) From {New Gfx3D.Triangle({0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F}, Presets.White, Presets.White, Presets.White),
                                                New Gfx3D.Triangle({0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F}, Presets.White, Presets.White, Presets.White)}
    }
    m_mapAssetMeshes("UnitQuad") = meshQuad

    ' The four outer walls of a cell
    Dim meshWallsOut As New Gfx3D.Mesh With {
      .Tris = New List(Of Gfx3D.Triangle) From {New Gfx3D.Triangle({1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F}, Presets.White, Presets.White, Presets.White), ' EAST
                                                    New Gfx3D.Triangle({1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 0.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F}, Presets.White, Presets.White, Presets.White),
                                                                                                                                                                                                                                                    _
                                                    New Gfx3D.Triangle({0.0F, 0.0F, 0.2F, 1.0F, 0.0F, 1.0F, 0.2F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F}, Presets.White, Presets.White, Presets.White), ' WEST
                                                    New Gfx3D.Triangle({0.0F, 0.0F, 0.2F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F}, Presets.White, Presets.White, Presets.White), _
                                                                                                                                                                                                                                                     _
                                                    New Gfx3D.Triangle({0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F}, Presets.White, Presets.White, Presets.White), ' TOP
                                                    New Gfx3D.Triangle({0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 1.0F, 0.2F, 1.0F, 1.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F}, Presets.White, Presets.White, Presets.White), _
                                                                                                                                                                                                                                                     _
                                                    New Gfx3D.Triangle({1.0F, 0.0F, 0.2F, 1.0F, 0.0F, 0.0F, 0.2F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 0.0F}, Presets.White, Presets.White, Presets.White), ' BOTTOM
                                                    New Gfx3D.Triangle({1.0F, 0.0F, 0.2F, 1.0F, 0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F}, Presets.White, Presets.White, Presets.White)}
    }
    m_mapAssetMeshes("WallsOut") = meshWallsOut

    ' System Textures
    For Each asset In GameSettings.AssetTextures
      Dim sprAsset As New Sprite()
      If sprAsset.LoadFromFile(asset.File) = RCode.Ok Then
        m_mapAssetTextures(asset.Name) = sprAsset
      Else
        Console.WriteLine("Failed to load " & asset.Name)
        Return False
      End If
    Next

    ' Break up roads sprite into individual sprites. Why? Its easier to maintain
    ' the roads sprite as a single image, but easier to use if they are all individual.
    ' Breaking it up manually in the image editing software is time consuming so just
    ' do it here
    Dim nRoadTexSize = 256 ' In pixels in base texture
    Dim nRoadTexOffset = 64 ' There exists a 64 pixel offset from top left of source image
    For r = 0 To 11
      Dim road As New Sprite(nRoadTexSize, nRoadTexSize)
      SetDrawTarget(road)
      DrawPartialSprite(0, 0, m_mapAssetTextures("AllRoads"), ((r Mod 3) * nRoadTexSize) + nRoadTexOffset, ((r \ 3) * nRoadTexSize) + nRoadTexOffset, nRoadTexSize, nRoadTexSize)
      Select Case r
        Case 0 : m_mapAssetTextures("Road_V") = road
        Case 1 : m_mapAssetTextures("Road_H") = road
        Case 2 : m_mapAssetTextures("Pavement") = road
        Case 3 : m_mapAssetTextures("Road_C1") = road
        Case 4 : m_mapAssetTextures("Road_T1") = road
        Case 5 : m_mapAssetTextures("Road_C2") = road
        Case 6 : m_mapAssetTextures("Road_T2") = road
        Case 7 : m_mapAssetTextures("Road_X") = road
        Case 8 : m_mapAssetTextures("Road_T3") = road
        Case 9 : m_mapAssetTextures("Road_C3") = road
        Case 10 : m_mapAssetTextures("Road_T4") = road
        Case 11 : m_mapAssetTextures("Road_C4") = road
      End Select
    Next
    SetDrawTarget(Nothing)

    ' Load Buildings
    For Each asset In GameSettings.AssetBuildings
      m_mapAssetMeshes(asset.Description) = New Gfx3D.Mesh()
      m_mapAssetMeshes(asset.Description).LoadOBJFile(asset.ModelObject)
      m_mapAssetTextures(asset.Description) = New Sprite(asset.ModelPng)
      Dim matScale = Gfx3D.Math.Mat_MakeScale(asset.Scale(0), asset.Scale(1), asset.Scale(2))
      Dim matTranslate = Gfx3D.Math.Mat_MakeTranslation(asset.Translate(0), asset.Translate(1), asset.Translate(2))
      Dim matRotateX = Gfx3D.Math.Mat_MakeRotationX(asset.Rotate(0))
      Dim matRotateY = Gfx3D.Math.Mat_MakeRotationY(asset.Rotate(1))
      Dim matRotateZ = Gfx3D.Math.Mat_MakeRotationZ(asset.Rotate(2))
      Dim matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTranslate, matScale)
      matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTransform, matRotateX)
      matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTransform, matRotateY)
      matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTransform, matRotateZ)
      m_mapAssetTransform(asset.Description) = matTransform
    Next

    ' Load Vehicles
    For Each asset In GameSettings.AssetVehicles
      m_mapAssetMeshes(asset.Description) = New Gfx3D.Mesh()
      m_mapAssetMeshes(asset.Description).LoadOBJFile(asset.ModelObject)
      m_mapAssetTextures(asset.Description) = New Sprite(asset.ModelPng)
      Dim matScale = Gfx3D.Math.Mat_MakeScale(asset.Scale(0), asset.Scale(1), asset.Scale(2))
      Dim matTranslate = Gfx3D.Math.Mat_MakeTranslation(asset.Translate(0), asset.Translate(1), asset.Translate(2))
      Dim matRotateX = Gfx3D.Math.Mat_MakeRotationX(asset.Rotate(0))
      Dim matRotateY = Gfx3D.Math.Mat_MakeRotationY(asset.Rotate(1))
      Dim matRotateZ = Gfx3D.Math.Mat_MakeRotationZ(asset.Rotate(2))
      Dim matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTranslate, matScale)
      matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTransform, matRotateX)
      matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTransform, matRotateY)
      matTransform = Gfx3D.Math.Mat_MultiplyMatrix(matTransform, matRotateZ)
      m_mapAssetTransform(asset.Description) = matTransform
    Next

    Return True

  End Function

  Private Class GameObjectQuad

    'Private m_meshTris As List(Of Gfx3D.Triangle)
    Private ReadOnly m_pointsModel As List(Of Gfx3D.Vec3d)
    Private ReadOnly m_pointsWorld As List(Of Gfx3D.Vec3d)
    Private ReadOnly m_pos As Gfx3D.Vec3d

    Private ReadOnly m_width As Single
    Private ReadOnly m_height As Single
    'Private m_originX As Single
    'Private m_originY As Single
    Private ReadOnly m_angle As Single

    Structure Vec2d
      Public X As Single
      Public Y As Single
      Public Sub New(x As Single, y As Single)
        Me.x = x
        Me.y = y
      End Sub
    End Structure

    Public Sub New(w As Single, h As Single)

      m_width = w
      m_height = h
      m_angle = 0.0F

      ' Construct Model Quad Geometry
      m_pointsModel = New List(Of Gfx3D.Vec3d) From {
          New Gfx3D.Vec3d(-m_width / 2.0F, -m_height / 2.0F, -0.01F, 1.0F),
          New Gfx3D.Vec3d(-m_width / 2.0F, +m_height / 2.0F, -0.01F, 1.0F),
          New Gfx3D.Vec3d(+m_width / 2.0F, +m_height / 2.0F, -0.01F, 1.0F),
          New Gfx3D.Vec3d(+m_width / 2.0F, -m_height / 2.0F, -0.01F, 1.0F)}

      m_pointsWorld = New List(Of Gfx3D.Vec3d)(m_pointsModel.Count - 1) ' {}
      TransformModelToWorld()

    End Sub

    Public Sub TransformModelToWorld()
      For i = 0 To m_pointsModel.Count - 1
        m_pointsWorld(i) = New Gfx3D.Vec3d(
            m_pointsModel(i).X * MathF.Cos(m_angle) - m_pointsModel(i).Y * MathF.Sin(m_angle) + m_pos.X,
            m_pointsModel(i).X * MathF.Sin(m_angle) + m_pointsModel(i).Y * MathF.Cos(m_angle) + m_pos.Y,
            m_pointsModel(i).Z,
            m_pointsModel(i).W)
      Next
    End Sub

    Public Function GetTriangles() As List(Of Gfx3D.Triangle)
      ' Return triangles based upon this quad
      Return New List(Of Gfx3D.Triangle) From {
          New Gfx3D.Triangle(m_pointsWorld(0), m_pointsWorld(1), m_pointsWorld(2), 0.0F, 0.0F, 0.0F, 0.0F, 1.0F, 0.0F, 1.0F, 1.0F, 0.0F, Presets.Red, Presets.Red, Presets.Red),
          New Gfx3D.Triangle(m_pointsWorld(0), m_pointsWorld(2), m_pointsWorld(3), 0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.0F, 1.0F, 0.0F, 0.0F, Presets.Red, Presets.Red, Presets.Red)}
    End Function

    ' Use rectangle edge intersections.
    Public Function StaticCollisionWith(ByRef r2 As GameObjectQuad, Optional resolveStatic As Boolean = False) As Boolean

      Dim collision = False

      ' Check diagonals of R1 against edges of R2
      For p = 0 To m_pointsWorld.Count - 1

        Dim line_r1s As New Vec2d With {.X = m_pos.X, .Y = m_pos.Y}
        Dim line_r1e As New Vec2d With {.X = m_pointsWorld(p).X, .Y = m_pointsWorld(p).Y}

        Dim displacement As New Vec2d With {.X = 0, .Y = 0}

        For q = 0 To r2.m_pointsWorld.Count - 1

          Dim line_r2s As New Vec2d With {.X = r2.m_pointsWorld(q).X, .Y = r2.m_pointsWorld(q).Y}
          Dim line_r2e As New Vec2d With {.X = r2.m_pointsWorld((q + 1) Mod r2.m_pointsWorld.Count).X, .Y = r2.m_pointsWorld((q + 1) Mod r2.m_pointsWorld.Count).Y}

          ' Standard "off the shelf" line segment intersection
          Dim h = (line_r2e.X - line_r2s.X) * (line_r1s.Y - line_r1e.Y) - (line_r1s.X - line_r1e.X) * (line_r2e.Y - line_r2s.Y)
          Dim t1 = ((line_r2s.Y - line_r2e.Y) * (line_r1s.X - line_r2s.X) + (line_r2e.X - line_r2s.X) * (line_r1s.Y - line_r2s.Y)) / h
          Dim t2 = ((line_r1s.Y - line_r1e.Y) * (line_r1s.X - line_r2s.X) + (line_r1e.X - line_r1s.X) * (line_r1s.Y - line_r2s.Y)) / h

          If t1 >= 0.0F AndAlso t1 <= 1.0F AndAlso t2 >= 0.0F AndAlso t2 <= 1.0F Then
            If resolveStatic Then
              displacement.X += (1.0F - t1) * (line_r1e.X - line_r1s.X)
              displacement.Y += (1.0F - t1) * (line_r1e.Y - line_r1s.Y)
              collision = True
            Else
              Return True
            End If
          End If
        Next

        m_pos.X -= displacement.X
        m_pos.Y -= displacement.Y

      Next

      ' Check diagonals of R2 against edges of R1
      For p = 0 To r2.m_pointsWorld.Count - 1

        Dim line_r1s As New Vec2d(r2.m_pos.X, r2.m_pos.Y)
        Dim line_r1e As New Vec2d(r2.m_pointsWorld(p).X, r2.m_pointsWorld(p).Y)

        Dim displacement As New Vec2d(0, 0)

        For q = 0 To m_pointsWorld.Count - 1

          Dim line_r2s As New Vec2d(m_pointsWorld(q).X, m_pointsWorld(q).Y)
          Dim line_r2e As New Vec2d(m_pointsWorld((q + 1) Mod m_pointsWorld.Count).X, m_pointsWorld((q + 1) Mod m_pointsWorld.Count).Y)

          ' Standard "off the shelf" line segment intersection
          Dim h = (line_r2e.X - line_r2s.X) * (line_r1s.Y - line_r1e.Y) - (line_r1s.X - line_r1e.X) * (line_r2e.Y - line_r2s.Y)
          Dim t1 = ((line_r2s.Y - line_r2e.Y) * (line_r1s.X - line_r2s.X) + (line_r2e.X - line_r2s.X) * (line_r1s.Y - line_r2s.Y)) / h
          Dim t2 = ((line_r1s.Y - line_r1e.Y) * (line_r1s.X - line_r2s.X) + (line_r1e.X - line_r1s.X) * (line_r1s.Y - line_r2s.Y)) / h

          If t1 >= 0.0F AndAlso t1 <= 1.0F AndAlso t2 >= 0.0F AndAlso t2 <= 1.0F Then
            If resolveStatic Then
              displacement.X += (1.0F - t1) * (line_r1e.X - line_r1s.X)
              displacement.Y += (1.0F - t1) * (line_r1e.Y - line_r1s.Y)
              collision = True
            Else
              Return True
            End If
          End If

        Next

        m_pos.X += displacement.X
        m_pos.Y += displacement.Y

      Next

      Return collision

    End Function

  End Class

End Class