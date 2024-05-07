Imports VbPixelGameEngine

Public Class CellWater
  Inherits Cell

  Private m_meshUnitQuad As Gfx3D.Mesh = Nothing
  Private m_meshWalls As Gfx3D.Mesh = Nothing
  Private m_sprWater As Sprite = Nothing
  Private m_sprSides As Sprite = Nothing
  Private m_sprClouds As Sprite = Nothing
  Private ReadOnly neighboursAreWater(3) As Boolean

  Public Sub New(map As CityMap, x As Integer, y As Integer)
    MyBase.New(map, x, y)
    m_cellType = CellType.Water
    neighboursAreWater(0) = False
    neighboursAreWater(1) = False
    neighboursAreWater(2) = False
    neighboursAreWater(3) = False
  End Sub

  Public Overrides Sub CalculateAdjacency()

    Dim r = Function(i As Integer, j As Integer)
              If m_map.Cell(m_worldX + i, m_worldY + j) IsNot Nothing Then
                Return m_map.Cell(m_worldX + i, m_worldY + j).m_cellType = CellType.Water
              Else
                Return False
              End If
            End Function

    neighboursAreWater(0) = r(0, -1)
    neighboursAreWater(1) = r(+1, 0)
    neighboursAreWater(2) = r(0, +1)
    neighboursAreWater(3) = r(-1, 0)

  End Sub

  Public Overrides Function LinkAssets(mapTextures As Dictionary(Of String, Sprite), mapMesh As Dictionary(Of String, Gfx3D.Mesh), mapTransforms As Dictionary(Of String, Gfx3D.Mat4x4)) As Boolean
    m_meshUnitQuad = mapMesh("UnitQuad")
    m_meshWalls = mapMesh("WallsOut")
    m_sprWater = mapTextures("Water")
    m_sprSides = mapTextures("WaterSide")
    m_sprClouds = mapTextures("Clouds")
    Return False
  End Function

  Public Overrides Function Update(elapsedTime As Single) As Boolean
    Return False
  End Function

  Public Overrides Function DrawBase(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean
    Dim matWorld = Gfx3D.Math.Mat_MakeTranslation(m_worldX, m_worldY, 0.0F)
    pipe.SetTransform(matWorld)
    pipe.SetTexture(m_sprSides)
    If Not neighboursAreWater(1) Then pipe.Render(m_meshWalls.Tris, Gfx3D.RenderFlags.RenderLights Or Gfx3D.RenderFlags.RenderCullCcw Or Gfx3D.RenderFlags.RenderTextured Or Gfx3D.RenderFlags.RenderDepth, 0, 2)
    If Not neighboursAreWater(3) Then pipe.Render(m_meshWalls.Tris, Gfx3D.RenderFlags.RenderLights Or Gfx3D.RenderFlags.RenderCullCcw Or Gfx3D.RenderFlags.RenderTextured Or Gfx3D.RenderFlags.RenderDepth, 2, 2)
    If Not neighboursAreWater(2) Then pipe.Render(m_meshWalls.Tris, Gfx3D.RenderFlags.RenderLights Or Gfx3D.RenderFlags.RenderCullCcw Or Gfx3D.RenderFlags.RenderTextured Or Gfx3D.RenderFlags.RenderDepth, 4, 2)
    If Not neighboursAreWater(0) Then pipe.Render(m_meshWalls.Tris, Gfx3D.RenderFlags.RenderLights Or Gfx3D.RenderFlags.RenderCullCcw Or Gfx3D.RenderFlags.RenderTextured Or Gfx3D.RenderFlags.RenderDepth, 6, 2)
    Return False
  End Function

  Private Function RenderWater(x As Integer, y As Integer, pSource As Pixel, pDest As Pixel) As Pixel
    Dim a = (pSource.A / 255.0F) * 0.6F
    Dim c = 1.0F - a
    Dim r = a * pSource.R + c * pDest.R
    Dim g = a * pSource.G + c * pDest.G
    Dim b = a * pSource.B + c * pDest.B
    a = 0.4F
    c = 1.0F - a
    Dim sky = m_sprClouds.GetPixel(x, y)
    Dim sr = a * sky.R + c * r
    Dim sg = a * sky.G + c * g
    Dim sb = a * sky.B + c * b
    Return New Pixel(CByte(sr), CByte(sg), CByte(sb))
  End Function

  Public Overrides Function DrawAlpha(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean

    'Dim renderWater = Function(x As Integer, y As Integer, pSource As Pixel, pDest As Pixel) As Pixel
    '                    Dim a = CSng(pSource.A / 255.0F) * 0.6F
    '                    Dim c = 1.0F - a
    '                    Dim r = a * CSng(pSource.R) + c * CSng(pDest.R)
    '                    Dim g = a * CSng(pSource.G) + c * CSng(pDest.G)
    '                    Dim b = a * CSng(pSource.B) + c * CSng(pDest.B)
    '                    a = 0.4F
    '                    c = 1.0F - a
    '                    Dim sky = sprClouds.GetPixel(x, y)
    '                    Dim sr = a * CSng(sky.R) + c * r
    '                    Dim sg = a * CSng(sky.G) + c * g
    '                    Dim sb = a * CSng(sky.B) + c * b
    '                    Return New Pixel(CByte(sr), CByte(sg), CByte(sb))
    '                  End Function

    pge.SetPixelMode(AddressOf RenderWater)
    Dim matWorld = Gfx3D.Math.Mat_MakeTranslation(CSng(m_worldX), CSng(m_worldY), 0.07F)
    pipe.SetTransform(matWorld)
    pipe.SetTexture(m_sprWater)
    pipe.Render(m_meshUnitQuad.Tris, Gfx3D.RenderFlags.RenderCullCw Or Gfx3D.RenderFlags.RenderDepth Or Gfx3D.RenderFlags.RenderTextured)
    pge.SetPixelMode(Pixel.Mode.Normal)

    Return False

  End Function

End Class