Imports VbPixelGameEngine

Public Enum RoadType
  H
  V
  C1
  C2
  C3
  C4
  T1
  T2
  T3
  T4
  X
End Enum

Public Class CellRoad
  Inherits Cell

  'Private m_neighboursAreRoads(3) As Boolean

  Private m_meshUnitQuad As Gfx3D.Mesh = Nothing
  Private ReadOnly m_sprRoadTex(10) As Sprite

  Private Class StopPattern
    Public [Stop](48) As Boolean
  End Class

  Private ReadOnly m_stopPattern As New List(Of StopPattern)
  Private m_currentStopPattern As Integer = 0
  Private m_stopPatternTimer As Single = 0.0F

  Public m_roadType As RoadType = RoadType.X
  Public m_safeCarTrack As AutoTrack = Nothing
  Public m_safePedestrianTrack As AutoTrack = Nothing
  Public m_safeChaseTrack As AutoTrack = Nothing

  Public Sub New(map As CityMap, x As Integer, y As Integer)
    MyBase.New(map, x, y)
    m_solid = False
    m_cellType = CellType.Road
  End Sub

  Public Overrides Sub CalculateAdjacency()

    ' Calculate suitable road junction type
    Dim r = Function(i As Integer, j As Integer) As Boolean
              Return (m_map.Cell(m_worldX + i, m_worldY + j) IsNot Nothing AndAlso m_map.Cell(m_worldX + i, m_worldY + j).m_cellType = CellType.Road)
            End Function

    If r(0, -1) AndAlso r(0, +1) AndAlso Not r(-1, 0) AndAlso Not r(+1, 0) Then m_roadType = RoadType.V
    If Not r(0, -1) AndAlso Not r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then m_roadType = RoadType.H
    If Not r(0, -1) AndAlso r(0, +1) AndAlso Not r(-1, 0) AndAlso r(+1, 0) Then m_roadType = RoadType.C1
    If Not r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then m_roadType = RoadType.T1
    If Not r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso Not r(+1, 0) Then m_roadType = RoadType.C2
    If r(0, -1) AndAlso r(0, +1) AndAlso Not r(-1, 0) AndAlso r(+1, 0) Then m_roadType = RoadType.T2
    If r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then m_roadType = RoadType.X
    If r(0, -1) AndAlso r(0, +1) AndAlso r(-1, 0) AndAlso Not r(+1, 0) Then m_roadType = RoadType.T3
    If r(0, -1) AndAlso Not r(0, +1) AndAlso Not r(-1, 0) AndAlso r(+1, 0) Then m_roadType = RoadType.C3
    If r(0, -1) AndAlso Not r(0, +1) AndAlso r(-1, 0) AndAlso r(+1, 0) Then m_roadType = RoadType.T4
    If r(0, -1) AndAlso Not r(0, +1) AndAlso r(-1, 0) AndAlso Not r(+1, 0) Then m_roadType = RoadType.C4

    ' Add navigation tracks based on type

    Dim addTrack = Function(n1 As Integer, n2 As Integer) As AutoTrack

                     If m_naviNodes(n1) Is Nothing OrElse m_naviNodes(n2) Is Nothing Then
                       ' Can't add track
                       Return Nothing
                     Else

                       ' Nodes exist so add track
                       Dim t As New AutoTrack()
                       t.m_node(0) = m_naviNodes(n1)
                       t.m_node(1) = m_naviNodes(n2)
                       t.m_cell = Me
                       t.m_trackLength = (m_naviNodes(n1).m_pos - m_naviNodes(n2).m_pos).Mag()
                       m_listTracks.Add(t)

                       ' Add pointers to track to start and end nodes
                       m_naviNodes(n1).m_listTracks.Add(m_listTracks.Last())
                       m_naviNodes(n2).m_listTracks.Add(m_listTracks.Last())

                       Return m_listTracks.Last()

                     End If

                   End Function

    ' Ensure list of tracks for this cell is clear
    m_listTracks.Clear()

    ' Add tracks depending on junction type
    m_safePedestrianTrack = Nothing
    m_safeCarTrack = Nothing
    m_safeChaseTrack = Nothing

    ' Add Pedestrian Tracks
    Select Case m_roadType
      Case RoadType.H
        m_safePedestrianTrack = addTrack(7, 13)
        addTrack(41, 35)
      Case RoadType.V
        m_safePedestrianTrack = addTrack(1, 43)
        addTrack(5, 47)
      Case RoadType.C1
        m_safePedestrianTrack = addTrack(43, 8)
        addTrack(8, 13)
        addTrack(47, 40)
        addTrack(40, 41)
      Case RoadType.C2
        addTrack(7, 12)
        addTrack(12, 47)
        m_safePedestrianTrack = addTrack(35, 36)
        addTrack(36, 43)
      Case RoadType.C3
        addTrack(1, 36)
        m_safePedestrianTrack = addTrack(36, 41)
        addTrack(5, 12)
        addTrack(12, 13)
      Case RoadType.C4
        addTrack(35, 40)
        addTrack(40, 5)
        m_safePedestrianTrack = addTrack(7, 8)
        addTrack(8, 1)

      Case RoadType.T1
        m_safePedestrianTrack = addTrack(7, 8)
        addTrack(8, 12)
        addTrack(12, 13)
        addTrack(35, 36)
        addTrack(36, 38)
        addTrack(38, 40)
        addTrack(40, 41)
        addTrack(8, 22)
        addTrack(22, 36)
        addTrack(36, 43)
        addTrack(12, 26)
        addTrack(26, 40)
        addTrack(40, 47)
      Case RoadType.T2
        m_safePedestrianTrack = addTrack(1, 8)
        addTrack(8, 36)
        addTrack(36, 43)
        addTrack(5, 12)
        addTrack(12, 26)
        addTrack(26, 40)
        addTrack(40, 47)
        addTrack(8, 10)
        addTrack(10, 12)
        addTrack(12, 13)
        addTrack(36, 38)
        addTrack(38, 40)
        addTrack(40, 41)
      Case RoadType.T3
        m_safePedestrianTrack = addTrack(5, 12)
        addTrack(12, 40)
        addTrack(40, 47)
        addTrack(1, 8)
        addTrack(8, 22)
        addTrack(22, 36)
        addTrack(36, 43)
        addTrack(12, 10)
        addTrack(10, 8)
        addTrack(8, 7)
        addTrack(40, 38)
        addTrack(38, 36)
        addTrack(36, 35)
      Case RoadType.T4
        m_safePedestrianTrack = addTrack(35, 36)
        addTrack(36, 40)
        addTrack(40, 41)
        addTrack(7, 8)
        addTrack(8, 10)
        addTrack(10, 12)
        addTrack(12, 13)
        addTrack(36, 22)
        addTrack(22, 8)
        addTrack(8, 1)
        addTrack(40, 26)
        addTrack(26, 12)
        addTrack(12, 5)
      Case RoadType.X
        addTrack(35, 36)
        addTrack(36, 38)
        addTrack(38, 40)
        addTrack(40, 41)
        addTrack(7, 8)
        addTrack(8, 10)
        addTrack(10, 12)
        addTrack(12, 13)
        addTrack(36, 22)
        addTrack(22, 8)
        addTrack(8, 1)
        addTrack(40, 26)
        addTrack(26, 12)
        addTrack(12, 5)
        m_safePedestrianTrack = addTrack(36, 43)
        addTrack(40, 47)
    End Select

    ' Add Chase Tracks
    Select Case m_roadType
      Case RoadType.H
        addTrack(21, 27)
      Case RoadType.V
        addTrack(3, 45)
      Case RoadType.C1
        addTrack(45, 24)
        addTrack(24, 27)
      Case RoadType.C2
        addTrack(21, 24)
        addTrack(24, 45)
      Case RoadType.C3
        addTrack(3, 24)
        addTrack(24, 27)
      Case RoadType.C4
        addTrack(21, 24)
        addTrack(24, 3)
      Case RoadType.T1
        addTrack(21, 24)
        addTrack(24, 27)
        addTrack(24, 45)
      Case RoadType.T2
        addTrack(3, 24)
        addTrack(24, 45)
        addTrack(24, 27)
      Case RoadType.T3
        addTrack(3, 24)
        addTrack(24, 45)
        addTrack(24, 21)
      Case RoadType.T4
        addTrack(21, 24)
        addTrack(24, 27)
        addTrack(24, 3)
      Case RoadType.X
        addTrack(3, 24)
        addTrack(27, 24)
        addTrack(45, 24)
        addTrack(21, 24)
    End Select

    ' Road traffic tracks
    Select Case m_roadType
      Case RoadType.H
        m_safeCarTrack = addTrack(14, 20)
        addTrack(28, 34)
      Case RoadType.V
        addTrack(2, 44)
        m_safeCarTrack = addTrack(4, 46)
      Case RoadType.C1
        m_safeCarTrack = addTrack(44, 16)
        addTrack(16, 20)
        addTrack(46, 32)
        addTrack(32, 34)
      Case RoadType.C2
        m_safeCarTrack = addTrack(14, 18)
        addTrack(18, 46)
        addTrack(28, 30)
        addTrack(30, 44)
      Case RoadType.C3
        addTrack(2, 30)
        addTrack(30, 34)
        m_safeCarTrack = addTrack(4, 18)
        addTrack(18, 20)
      Case RoadType.C4
        addTrack(2, 16)
        addTrack(16, 14)
        m_safeCarTrack = addTrack(4, 32)
        addTrack(32, 28)
      Case RoadType.T1
        addTrack(14, 16)
        addTrack(16, 18)
        addTrack(18, 20)
        addTrack(28, 30)
        addTrack(30, 32)
        addTrack(32, 34)
        addTrack(16, 30)
        addTrack(30, 44)
        addTrack(18, 32)
        addTrack(32, 46)
      Case RoadType.T4
        addTrack(14, 16)
        addTrack(16, 18)
        addTrack(18, 20)
        addTrack(28, 30)
        addTrack(30, 32)
        addTrack(32, 34)
        addTrack(16, 30)
        addTrack(16, 2)
        addTrack(18, 32)
        addTrack(18, 4)
      Case RoadType.T2
        addTrack(2, 16)
        addTrack(16, 30)
        addTrack(30, 44)
        addTrack(4, 18)
        addTrack(18, 32)
        addTrack(32, 46)
        addTrack(16, 18)
        addTrack(18, 20)
        addTrack(30, 32)
        addTrack(32, 34)
      Case RoadType.T3
        addTrack(2, 16)
        addTrack(16, 30)
        addTrack(30, 44)
        addTrack(4, 18)
        addTrack(18, 32)
        addTrack(32, 46)
        addTrack(14, 16)
        addTrack(16, 18)
        addTrack(28, 30)
        addTrack(30, 32)
      Case RoadType.X
        addTrack(2, 16)
        addTrack(16, 30)
        addTrack(30, 44)
        addTrack(4, 18)
        addTrack(18, 32)
        addTrack(32, 46)
        addTrack(14, 16)
        addTrack(16, 18)
        addTrack(18, 20)
        addTrack(28, 30)
        addTrack(30, 32)
        addTrack(32, 34)
    End Select

    ' Stop Patterns
    ' .PO.OP.
    ' PP.P.PP
    ' O.O.O.O
    ' .P...P.
    ' O.O.O.O
    ' PP.P.PP
    ' .PO.OP.

    ' .PO.OP.
    ' PP.P.PP
    ' O.X.X.O
    ' .P...P.
    ' O.X.X.O
    ' PP.P.PP
    ' .PO.OP.

    ' .PO.OP.
    ' PP.X.PP
    ' O.X.X.O
    ' .X...X.
    ' O.X.X.O
    ' PP.X.PP
    ' .PO.OP.

    Dim stopmap = Function(s As String) As StopPattern
                    Dim p As New StopPattern()
                    For i = 0 To s.Length - 1
                      p.Stop(i) = (s(i) = "X")
                    Next
                    Return p
                  End Function

    Select Case m_roadType
      Case RoadType.H, RoadType.V, RoadType.C1, RoadType.C2, RoadType.C3, RoadType.C4
        ' Allow all
        ' vStopPattern.push_back(stopmap(".PO.OP." "PP.P.PP" "O.O.O.O" ".P...P." "O.O.O.O" "PP.P.PP" ".PO.OP."))
      Case RoadType.X
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".P...P." &
                                         "X.X.X.X" &
                                         "PP.P.PP" &
                                         ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.X.PP" &
                                         "X.X.X.X" &
                                         ".X...X." &
                                         "X.X.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
        ' Allow West Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "O.O.O.O" &
                                         ".X...X." &
                                         "X.X.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Drain West Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.O.O" &
                                         ".X...X." &
                                         "X.X.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Allow North Traffic
        m_stopPattern.Add(stopmap(".PX.OP." &
                                         "PP.X.PP" &
                                         "X.X.O.O" &
                                         ".X...X." &
                                         "O.O.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Drain North Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.X.PP" &
                                         "X.X.O.O" &
                                         ".X...X." &
                                         "O.O.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".P...P." &
                                         "X.X.X.X" &
                                         "PP.P.PP" &
                                         ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.X.PP" &
                                         "X.X.X.X" &
                                         ".X...X." &
                                         "X.X.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
        ' Allow EAST Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.X.X" &
                                         ".X...X." &
                                         "O.O.O.O" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Drain East Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.X.X" &
                                         ".X...X." &
                                         "O.O.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Allow SOUTH Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.O.O" &
                                         ".X...X." &
                                         "O.O.X.X" &
                                         "PP.X.PP" &
                                         ".PO.XP."))
        ' Drain SOUTH Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.O.O" &
                                         ".X...X." &
                                         "O.O.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
      Case RoadType.T1
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".P...P." &
                                         "X.X.X.X" &
                                         "PP.P.PP" &
                                         ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".X...X." &
                                         "X.X.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
        ' Allow West Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "O.O.O.O" &
                                         ".X...X." &
                                         "X.X.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Drain West Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.O.O.O" &
                                         ".X...X." &
                                         "X.X.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".P...P." &
                                         "X.X.X.X" &
                                         "PP.P.PP" &
                                         ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.X.PP" &
                                         "X.X.X.X" &
                                         ".X...X." &
                                         "X.X.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
        ' Allow EAST Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".X...X." &
                                         "O.O.O.O" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Drain East Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".X...X." &
                                         "O.O.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Allow SOUTH Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.O.O.O" &
                                         ".X...X." &
                                         "O.O.X.X" &
                                         "PP.X.PP" &
                                         ".PO.XP."))
        ' Drain SOUTH Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.O.O.O" &
                                         ".X...X." &
                                         "O.O.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
      Case RoadType.T2
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".P...P." &
                                         "X.X.X.X" &
                                         "PP.P.PP" &
                                         ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.X.PP" &
                                         "X.X.X.X" &
                                         ".P...X." &
                                         "X.X.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
        ' Allow North Traffic
        m_stopPattern.Add(stopmap(".PX.OP." &
                                        "PP.X.PP" &
                                        "X.X.O.O" &
                                        ".P...X." &
                                        "X.X.O.X" &
                                        "PP.X.PP" &
                                        ".PX.OP."))
        ' Drain North Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.X.PP" &
                                         "X.X.O.O" &
                                         ".P...X." &
                                         "X.X.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.P.PP" &
                                         "X.X.X.X" &
                                         ".P...P." &
                                         "X.X.X.X" &
                                         "PP.P.PP" &
                                         ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                         "PP.X.PP" &
                                         "X.X.X.X" &
                                         ".X...X." &
                                         "X.X.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
        ' Allow EAST Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.X.X" &
                                         ".P...X." &
                                         "X.O.O.O" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Drain East Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.X.X" &
                                         ".P...X." &
                                         "X.O.O.X" &
                                         "PP.X.PP" &
                                         ".PX.OP."))
        ' Allow SOUTH Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.O.O" &
                                         ".P...X." &
                                         "X.O.X.X" &
                                         "PP.X.PP" &
                                         ".PO.XP."))
        ' Drain SOUTH Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                         "PP.X.PP" &
                                         "X.O.O.O" &
                                         ".P...X." &
                                         "X.O.X.X" &
                                         "PP.X.PP" &
                                         ".PX.XP."))
      Case RoadType.T3
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.P.PP" &
                                      "X.X.X.X" &
                                      ".P...P." &
                                      "X.X.X.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.X.PP" &
                                      "X.X.X.X" &
                                      ".X...P." &
                                      "X.X.X.X" &
                                      "PP.X.PP" &
                                      ".PX.XP."))
        ' Allow West Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "O.O.O.X" &
                                      ".X...P." &
                                      "X.X.O.X" &
                                      "PP.X.PP" &
                                      ".PX.OP."))
        ' Drain West Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "X.O.O.X" &
                                      ".X...P." &
                                      "X.X.O.X" &
                                      "PP.X.PP" &
                                      ".PX.OP."))
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.P.PP" &
                                      "X.X.X.X" &
                                      ".P...P." &
                                      "X.X.X.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.X.PP" &
                                      "X.X.X.X" &
                                      ".X...X." &
                                      "X.X.X.X" &
                                      "PP.X.PP" &
                                      ".PX.XP."))
        ' Allow North Traffic
        m_stopPattern.Add(stopmap(".PX.OP." &
                                      "PP.X.PP" &
                                      "X.X.O.X" &
                                      ".X...P." &
                                      "O.O.O.X" &
                                      "PP.X.PP" &
                                      ".PX.OP."))
        ' Drain North Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.X.PP" &
                                      "X.X.O.X" &
                                      ".X...P." &
                                      "O.O.O.X" &
                                      "PP.X.PP" &
                                      ".PX.OP."))
        ' Allow SOUTH Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "X.O.X.X" &
                                      ".X...P." &
                                      "O.O.X.X" &
                                      "PP.X.PP" &
                                      ".PO.XP."))
        ' Drain SOUTH Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "X.O.X.X" &
                                      ".X...P." &
                                      "O.O.X.X" &
                                      "PP.X.PP" &
                                      ".PX.XP."))
      Case RoadType.T4
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.P.PP" &
                                      "X.X.X.X" &
                                      ".P...P." &
                                      "X.X.X.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.X.PP" &
                                      "X.X.X.X" &
                                      ".X...X." &
                                      "X.X.X.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Allow West Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "O.O.O.O" &
                                      ".X...X." &
                                      "X.X.X.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Drain West Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "X.O.O.O" &
                                      ".X...X." &
                                      "X.X.X.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Allow North Traffic
        m_stopPattern.Add(stopmap(".PX.OP." &
                                      "PP.X.PP" &
                                      "X.X.O.O" &
                                      ".X...X." &
                                      "O.O.O.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Drain North Traffic
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.X.PP" &
                                      "X.X.O.O" &
                                      ".X...X." &
                                      "O.O.O.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Allow Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.P.PP" &
                                      "X.X.X.X" &
                                      ".P...P." &
                                      "X.X.X.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Drain Pedestrians
        m_stopPattern.Add(stopmap(".PX.XP." &
                                      "PP.X.PP" &
                                      "X.X.X.X" &
                                      ".X...X." &
                                      "X.X.X.X" &
                                      "PP.X.PP" &
                                      ".PX.XP."))
        ' Allow EAST Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "X.O.X.X" &
                                      ".X...X." &
                                      "O.O.O.O" &
                                      "PP.P.PP" &
                                      ".PX.XP."))
        ' Drain East Traffic
        m_stopPattern.Add(stopmap(".PO.XP." &
                                      "PP.X.PP" &
                                      "X.O.X.X" &
                                      ".X...X." &
                                      "O.O.O.X" &
                                      "PP.P.PP" &
                                      ".PX.XP."))

    End Select

  End Sub

  Public Overrides Function LinkAssets(mapTextures As Dictionary(Of String, Sprite), mapMesh As Dictionary(Of String, Gfx3D.Mesh), mapTransforms As Dictionary(Of String, Gfx3D.Mat4x4)) As Boolean
    m_meshUnitQuad = mapMesh("UnitQuad")
    m_sprRoadTex(RoadType.V) = mapTextures("Road_V")
    m_sprRoadTex(RoadType.H) = mapTextures("Road_H")
    m_sprRoadTex(RoadType.C1) = mapTextures("Road_C1")
    m_sprRoadTex(RoadType.T1) = mapTextures("Road_T1")
    m_sprRoadTex(RoadType.C2) = mapTextures("Road_C2")
    m_sprRoadTex(RoadType.T2) = mapTextures("Road_T2")
    m_sprRoadTex(RoadType.X) = mapTextures("Road_X")
    m_sprRoadTex(RoadType.T3) = mapTextures("Road_T3")
    m_sprRoadTex(RoadType.C3) = mapTextures("Road_C3")
    m_sprRoadTex(RoadType.T4) = mapTextures("Road_T4")
    m_sprRoadTex(RoadType.C4) = mapTextures("Road_C4")
    Return False
  End Function

  Public Overrides Function Update(elapsedTime As Single) As Boolean

    If m_stopPattern.Count = 0 Then
      Return False
    End If

    m_stopPatternTimer += elapsedTime
    If m_stopPatternTimer >= 5.0F Then
      m_stopPatternTimer -= 5.0F
      m_currentStopPattern += 1
      m_currentStopPattern = m_currentStopPattern Mod m_stopPattern.Count
      For i = 0 To 48
        If m_naviNodes(i) IsNot Nothing Then
          m_naviNodes(i).m_block = m_stopPattern(m_currentStopPattern).Stop(i)
        End If
      Next
    End If

    Return False

  End Function

  Public Overrides Function DrawBase(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean
    Dim matWorld = Gfx3D.Math.Mat_MakeTranslation(m_worldX, m_worldY, 0.0F)
    pipe.SetTransform(matWorld)
    pipe.SetTexture(m_sprRoadTex(m_roadType))
    pipe.Render(m_meshUnitQuad.Tris, Gfx3D.RenderFlags.RenderCullCw Or Gfx3D.RenderFlags.RenderDepth Or Gfx3D.RenderFlags.RenderTextured)
    Return False
  End Function

  Public Overrides Function DrawAlpha(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean
    Return False
  End Function

  Public Overrides Function DrawDebug(pge As PixelGameEngine, pipe As Gfx3D.PipeLine) As Boolean

    ' Draw Automata navigation tracks
    For Each track In m_listTracks
      Dim p1 = New Gfx3D.Vec3d(track.m_node(0).m_pos.x, track.m_node(0).m_pos.y, 0.0F)
      Dim p2 = New Gfx3D.Vec3d(track.m_node(1).m_pos.x, track.m_node(1).m_pos.y, 0.0F)
      pipe.RenderLine(p1, p2, Presets.Cyan)
    Next

    For i As Integer = 0 To 48
      If m_naviNodes(i) IsNot Nothing Then
        Dim p1 = New Gfx3D.Vec3d(m_naviNodes(i).m_pos.x, m_naviNodes(i).m_pos.y, 0.01F)
        pipe.RenderCircleXZ(p1, 0.03F, If(m_naviNodes(i).m_block, Presets.Red, Presets.Green))
      End If
    Next

    Return False

  End Function

End Class