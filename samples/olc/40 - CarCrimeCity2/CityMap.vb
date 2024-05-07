Imports VbPixelGameEngine

Public Class CityMap

  Private m_width As Integer = 0
  Private m_height As Integer = 0
  Private m_cells() As Cell = Nothing
  Private m_nodes() As AutoNode = Nothing

  Public Sub New(w As Integer, h As Integer, mapTextures As Dictionary(Of String, Sprite), mapMesh As Dictionary(Of String, Gfx3D.Mesh), mapTransforms As Dictionary(Of String, Gfx3D.Mat4x4))
    CreateCity(w, h, mapTextures, mapMesh, mapTransforms)
  End Sub

  Public ReadOnly Property Nodes(index As Integer) As AutoNode
    Get
      Return m_nodes(index)
    End Get
  End Property

  Public Function SaveCity(filename As String) As Boolean

    If filename <> "" AndAlso m_width <> 0 Then
    End If

    'TODO:

    'Dim file As std::ofstream = New std::ofstream(sFilename, std::ios.out Or std::ios.binary)
    'If Not file.is_open() Then Return False

    'file.write(CType(m_nWidth, Char()), sizeof(Integer))
    'file.write(CType(m_nHeight, Char()), sizeof(Integer))
    'For x As Integer = 0 To m_nWidth - 1
    '    For y As Integer = 0 To m_nHeight - 1
    '        file.write(CType(Cell(x, y), Char()), sizeof(cCityCell))
    '    Next
    'Next

    Return True

  End Function

  Public Function LoadCity(filename As String) As Boolean

    If filename <> "" AndAlso m_width <> 0 Then
    End If

    'TODO:

    'Dim file As std::ifstream = New std::ifstream(sFilename, std::ios.in Or std::ios.binary)
    'If Not file.is_open() Then Return False
    'Dim w As Integer, h As Integer
    'file.read(CType(w, Char()), sizeof(Integer))
    'file.read(CType(h, Char()), sizeof(Integer))
    'CreateCity(w, h)

    'For x As Integer = 0 To m_nWidth - 1
    '    For y As Integer = 0 To m_nHeight - 1
    '        file.read(CType(Cell(x, y), Char()), sizeof(cCityCell))
    '    Next
    'Next

    Return True

  End Function

  Public Function GetWidth() As Integer
    Return m_width
  End Function

  Public Function GetHeight() As Integer
    Return m_height
  End Function

  Public Function Cell(x As Integer, y As Integer) As Cell
    If x >= 0 AndAlso x < m_width AndAlso y >= 0 AndAlso y < m_height Then
      Return m_cells(y * m_width + x)
    Else
      Return Nothing
    End If
  End Function

  Public Function Replace(x As Integer, y As Integer, cell As Cell) As Cell

    If cell Is Nothing Then
      Return Nothing
    End If

    If m_cells(y * m_width + x) IsNot Nothing Then
      m_cells(y * m_width + x) = Nothing
    End If

    m_cells(y * m_width + x) = cell

    Return cell

  End Function

  Public Function GetAutoNodeBase(x As Integer, y As Integer) As Integer 'cAuto_Node
    'Return pNodes + (y * nWidth + x) * 49
    Return (y * m_width + x) * 49
  End Function

  Public Sub RemoveAllTracks()
    For i = 0 To (m_width * m_height * 49) - 1
      m_nodes(i).m_listTracks.Clear()
    Next
  End Sub

  Private Sub CreateCity(w As Integer, h As Integer, mapTextures As Dictionary(Of String, Sprite), mapMesh As Dictionary(Of String, Gfx3D.Mesh), mapTransforms As Dictionary(Of String, Gfx3D.Mat4x4))

    ReleaseCity()
    m_width = w
    m_height = h
    ReDim m_cells(m_height * m_width - 1)

    ' Create Navigation Node Pool, assumes 5 nodes on east and south
    ' side of each cell. The City owns these nodes, and cells in the
    ' city borrow them and link to them as required
    ReDim m_nodes(m_height * m_width * 49 - 1)

    ' The cell has 49 nodes, though some are simply unused. This is less memory
    ' efficient certainly, but makes code more intuitive and easier to write

    For x = 0 To m_width - 1

      For y = 0 To m_height - 1

        ' Nodes sit between cells, therefore each create nodes along
        ' the east and southern sides of the cell. This assumes that
        ' navigation along the top and left boundaries of the map
        ' will not occur. And it shouldnt, as its water

        Dim idx = (y * m_width + x) * 49

        For dx = 0 To 6

          Dim off_x As Single
          Dim off_y As Single

          Select Case dx
            Case 0 : off_x = 0.0F
            Case 1 : off_x = 0.083F
            Case 2 : off_x = 0.333F
            Case 3 : off_x = 0.5F
            Case 4 : off_x = 0.667F
            Case 5 : off_x = 0.917F
            Case 6 : off_x = 1.0F
          End Select

          For dy = 0 To 6

            Select Case dy
              Case 0 : off_y = 0.0F
              Case 1 : off_y = 0.083F
              Case 2 : off_y = 0.333F
              Case 3 : off_y = 0.5F
              Case 4 : off_y = 0.667F
              Case 5 : off_y = 0.917F
              Case 6 : off_y = 1.0F
            End Select

            m_nodes(idx + dy * 7 + dx) = New AutoNode With {.m_pos = New Vf2d(x + off_x, y + off_y),
                                                              .m_block = False}

          Next

        Next

      Next

    Next

    ' Now create default Cell
    For x = 0 To m_width - 1
      For y = 0 To m_height - 1
        ' Default city, everything is grass
        m_cells(y * m_width + x) = New CellPlane(Me, x, y, CellPlaneType.Grass)
        ' Give the cell the opportunity to locally reference the resources it needs
        m_cells(y * m_width + x).LinkAssets(mapTextures, mapMesh, mapTransforms)
      Next
    Next

  End Sub

  Private Sub ReleaseCity()

    For x = 0 To m_width - 1
      For y = 0 To m_height - 1

        'Erase any tracks attached to nodes
        For i = 0 To 48
          Cell(x, y).m_naviNodes(i).m_listTracks.Clear()
        Next

        'Release individual cell objects
        If m_cells(y * m_width + x) IsNot Nothing Then
          m_cells(y * m_width + x) = Nothing
        End If

      Next
    Next

    'Release array of cell pointers
    If m_cells IsNot Nothing Then
      m_cells = Nothing
    End If

    'Release array of automata navigation nodes
    If m_nodes IsNot Nothing Then
      m_nodes = Nothing
    End If

    m_width = 0
    m_height = 0

  End Sub

End Class