Imports VbPixelGameEngine

Public Enum CellType
  Blank
  Grass
  Concrete
  Water
  Building
  Road
  Pavement
End Enum

Public Class Cell

  Protected m_map As CityMap = Nothing

  Public m_worldX As Integer = 0
  Public m_worldY As Integer = 0
  Public m_solid As Boolean = False
  Public m_cellType As CellType = CellType.Blank

  ' This cell may actually be occupied by a multi-cell body
  ' so this pointer points to the host cell that contains
  ' that body
  Public m_hostCell As Cell = Nothing

  ' Each cell links to 20 automata transport nodes, 5 on each side
  Public m_naviNodes(48) As AutoNode

  ' Each cell can have a number of automata transport tracks, it owns them
  ' These connect nodes together as determined by the cell
  Public m_listTracks As New List(Of AutoTrack)

  Public Sub New()
    ' Cells own a list of automata navigation tracks
    ' but this will be destroyed when the cell Is deleted
  End Sub

  Public Sub New(map As CityMap, x As Integer, y As Integer)

    m_map = [map]
    m_worldX = x
    m_worldY = y
    m_cellType = CellType.Blank

    ' Connect internal nodes
    For i = 0 To 48
      m_naviNodes(i) = m_map.Nodes(m_map.GetAutoNodeBase(x, y) + i)
    Next

    ' Link cell into maps node pool
    If y > 0 Then
      For i = 0 To 6
        m_naviNodes(i) = m_map.Nodes(m_map.GetAutoNodeBase(x, y - 1) + 42 + i)
      Next
    Else
      For i = 0 To 6
        m_naviNodes(i) = Nothing
      Next
    End If

    If x > 0 Then
      ' Link West side
      For i = 0 To 6
        m_naviNodes(i * 7) = m_map.Nodes(m_map.GetAutoNodeBase(x - 1, y) + 6 + i * 7)
      Next
    Else
      For i = 0 To 6
        m_naviNodes(i * 7) = Nothing
      Next
    End If

    ' South Side
    If y < m_map.GetHeight() - 1 Then

    Else
      For i = 0 To 6
        m_naviNodes(42 + i) = Nothing
      Next
    End If

    ' East Side
    If x < m_map.GetWidth() - 1 Then

    Else
      For i = 0 To 6
        m_naviNodes(6 + i * 7) = Nothing
      Next
    End If

    ' Unused Nodes
    m_naviNodes(9) = Nothing
    m_naviNodes(11) = Nothing
    m_naviNodes(15) = Nothing
    m_naviNodes(19) = Nothing
    m_naviNodes(29) = Nothing
    m_naviNodes(33) = Nothing
    m_naviNodes(37) = Nothing
    m_naviNodes(39) = Nothing
    m_naviNodes(0) = Nothing
    m_naviNodes(6) = Nothing
    m_naviNodes(42) = Nothing
    m_naviNodes(48) = Nothing

  End Sub

  Public Overridable Function LinkAssets(mapTextures As Dictionary(Of String, Sprite),
                                         mapMesh As Dictionary(Of String, Gfx3D.Mesh),
                                         mapTransforms As Dictionary(Of String, Gfx3D.Mat4x4)) As Boolean
    Return False
  End Function

  Public Overridable Function Update(fElapsedTime As Single) As Boolean
    Return False
  End Function

  Public Overridable Function DrawBase(pge As PixelGameEngine,
                                       pipe As Gfx3D.PipeLine) As Boolean
    Return False
  End Function

  Public Overridable Function DrawAlpha(pge As PixelGameEngine,
                                        pipe As Gfx3D.PipeLine) As Boolean
    Return False
  End Function

  Public Overridable Function DrawDebug(pge As PixelGameEngine,
                                        pipe As Gfx3D.PipeLine) As Boolean
    Return False
  End Function

  Public Overridable Sub CalculateAdjacency()

  End Sub

End Class