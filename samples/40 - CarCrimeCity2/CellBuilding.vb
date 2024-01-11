Imports VbPixelGameEngine

Public Class CellBuilding
  Inherits Cell

  Private ReadOnly m_name As String
  Private m_texture As Sprite = Nothing
  Private m_mesh As Gfx3D.Mesh = Nothing
  Private m_transform As Gfx3D.Mat4x4

  Public Sub New(name As String, map As CityMap, x As Integer, y As Integer)
    MyBase.New(map, x, y)
    m_name = name
  End Sub

  'Public Overrides Sub CalculateAdjacency()
  'End Sub

  Public Overrides Function LinkAssets(mapTextures As Dictionary(Of String, Sprite), mapMesh As Dictionary(Of String, Gfx3D.Mesh), mapTransforms As Dictionary(Of String, Gfx3D.Mat4x4)) As Boolean
    m_texture = mapTextures(m_name)
    m_mesh = mapMesh(m_name)
    m_transform = mapTransforms(m_name)
    Return False
  End Function

  Public Overrides Function Update(elapsedTime As Single) As Boolean
    Return False
  End Function

  Public Overrides Function DrawBase(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean

    Dim matTranslate = Gfx3D.Math.Mat_MakeTranslation(m_worldX, m_worldY, 0.0F)
    Dim matWorld = Gfx3D.Math.Mat_MultiplyMatrix(m_transform, matTranslate)
    pipe.SetTransform(matWorld)

    If m_texture IsNot Nothing Then
      pipe.SetTexture(m_texture)
      pipe.Render(m_mesh.Tris, Gfx3D.RenderFlags.RenderCullCw Or Gfx3D.RenderFlags.RenderDepth Or Gfx3D.RenderFlags.RenderTextured Or Gfx3D.RenderFlags.RenderLights)
    Else
      pipe.Render(m_mesh.Tris, Gfx3D.RenderFlags.RenderCullCw Or Gfx3D.RenderFlags.RenderDepth Or Gfx3D.RenderFlags.RenderFlat Or Gfx3D.RenderFlags.RenderLights)
    End If

    Return False

  End Function

  Public Overrides Function DrawAlpha(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean
    Return False
  End Function

End Class