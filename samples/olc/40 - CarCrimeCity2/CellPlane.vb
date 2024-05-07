Imports VbPixelGameEngine

Public Enum CellPlaneType
  Grass
  Asphalt
End Enum

Public Class CellPlane
  Inherits Cell

  Protected m_type As CellPlaneType = CellPlaneType.Grass

  Private m_meshUnitQuad As Gfx3D.Mesh = Nothing
  Private m_sprGrass As Sprite = Nothing
  Private m_sprPavement As Sprite = Nothing

  Public Sub New(map As CityMap, x As Integer, y As Integer, type As CellPlaneType)
    MyBase.New(map, x, y)
    m_solid = False
    m_type = type
    If m_type = CellPlaneType.Grass Then m_cellType = CellType.Grass
    If m_type = CellPlaneType.Asphalt Then m_cellType = CellType.Pavement
  End Sub

  Public Overrides Function LinkAssets(mapTextures As Dictionary(Of String, Sprite), mapMesh As Dictionary(Of String, Gfx3D.Mesh), mapTransforms As Dictionary(Of String, Gfx3D.Mat4x4)) As Boolean
    m_sprGrass = mapTextures("Grass")
    m_sprPavement = mapTextures("Pavement")
    m_meshUnitQuad = mapMesh("UnitQuad")
    Return True
  End Function

  Public Overrides Function Update(elapsedTime As Single) As Boolean
    Return False
  End Function

  Public Overrides Function DrawBase(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean

    Dim matWorld = Gfx3D.Math.Mat_MakeTranslation(m_worldX, m_worldY, 0.0F)
    pipe.SetTransform(matWorld)

    If m_type = CellPlaneType.Grass Then
      pipe.SetTexture(m_sprGrass)
    Else
      pipe.SetTexture(m_sprPavement)
    End If

    pipe.Render(m_meshUnitQuad.Tris, Gfx3D.RenderFlags.RenderCullCw Or Gfx3D.RenderFlags.RenderDepth Or Gfx3D.RenderFlags.RenderTextured)

    Return False

  End Function

  Public Overrides Function DrawAlpha(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean
    Return False
  End Function

End Class