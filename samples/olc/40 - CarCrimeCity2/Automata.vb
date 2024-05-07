Imports VbPixelGameEngine

Public Class AutoNode

  Public m_pos As Vf2d
  Public m_block As Boolean
  Public m_listTracks As New List(Of AutoTrack)

  Public Sub New()
    m_pos = New Vf2d(0, 0)
  End Sub

  Public Sub New(worldPos As Vf2d)
    m_pos = New Vf2d(worldPos.x, worldPos.y)
  End Sub

End Class

Public Class AutoTrack

  Public m_node(1) As AutoNode ' Two end nodes
  Public m_cell As Cell ' Pointer to host cell
  Public m_listAutos As New List(Of AutoBody)
  Public m_trackLength As Single = 1.0F

  Public Function GetPosition(t As Single, start As AutoNode) As Vf2d
    ' pStart indicates the node the automata first encounted this track
    If m_node(0) Is start Then
      Return m_node(0).m_pos + (m_node(1).m_pos - m_node(0).m_pos) * (t / m_trackLength)
    Else
      Return m_node(1).m_pos + (m_node(0).m_pos - m_node(1).m_pos) * (t / m_trackLength)
    End If
  End Function

End Class

Public Class AutoBody

  Public m_autoPos As New Vf2d(0.0F, 0.0F)
  Public m_automataPos As Single = 0.0F ' Location of automata along track
  Public m_autoLength As Single = 0.0F ' Physical length of automata
  Public m_currentTrack As AutoTrack = Nothing
  Public m_trackOriginNode As AutoNode = Nothing

  Public Sub UpdateAuto(elapsedTime As Single)

    ' Work out which node is the target destination
    Dim exitNode = m_currentTrack.m_node(0)
    If exitNode Is m_trackOriginNode Then
      exitNode = m_currentTrack.m_node(1)
    End If

    Dim automataCanMove = True
    Dim distanceToAutoInFront = 1.0F

    ' First check if the vehicle overlaps with the one in front of it

    ' Get an iterator for this automata
    Dim thisAutomata = m_currentTrack.m_listAutos.Find(Function(automata) automata Is Me)

    If thisAutomata Is Nothing Then Return

    ' If this automata is at the front of this track segment
    If thisAutomata Is m_currentTrack.m_listAutos.First() Then
      ' Then check all the following track segments. Take the position of
      ' each vehicle at the back of the track segments auto list
      For Each track In exitNode.m_listTracks

        If track IsNot m_currentTrack AndAlso track.m_listAutos.Count <> 0 Then

          ' Get Auto at back
          Dim distanceFromTrackStartToAutoRear = track.m_listAutos.Last().m_automataPos - track.m_listAutos.Last().m_autoLength

          If thisAutomata.m_automataPos < (m_currentTrack.m_trackLength + distanceFromTrackStartToAutoRear - m_autoLength) Then
            ' Move Automata along track, as there is space
            ' bAutomataCanMove = True
            distanceToAutoInFront = (m_currentTrack.m_trackLength + distanceFromTrackStartToAutoRear - 0.1F) - thisAutomata.m_automataPos
          Else
            ' No space, so do not move automata
            automataCanMove = False
          End If

        Else
          ' Track in front was empty, node is clear to pass through so
          ' bAutomataCanMove = True
        End If

      Next
    Else

      ' Get the automata in front
      ' auto itAutomataInFront = itThisAutomata;
      ' itAutomataInFront--;

      Dim automataInFront = thisAutomata
      For index = 0 To m_currentTrack.m_listAutos.Count - 1
        If m_currentTrack.m_listAutos(index) Is thisAutomata Then
          automataInFront = m_currentTrack.m_listAutos(index - 1) : Exit For
        End If
      Next

      ' If the distance between the front of the automata in front and the fornt of this automata
      ' is greater than the length of the automata in front, then there is space for this automata
      ' to enter
      If Math.Abs(automataInFront.m_automataPos - thisAutomata.m_automataPos) > (automataInFront.m_autoLength + 0.1F) Then
        ' Move Automata along track
        ' bAutomataCanMove = True
        distanceToAutoInFront = (automataInFront.m_automataPos - automataInFront.m_autoLength - 0.1F) - thisAutomata.m_automataPos
      Else
        ' No space, so do not move automata
        automataCanMove = False
      End If

    End If

    If automataCanMove Then
      If distanceToAutoInFront > m_currentTrack.m_trackLength Then distanceToAutoInFront = m_currentTrack.m_trackLength
      m_automataPos += elapsedTime * Math.Max(distanceToAutoInFront, 1.0F) * If(m_autoLength < 0.1F, 0.3F, 0.5F)
    End If

    If (m_automataPos >= m_currentTrack.m_trackLength) Then

      'Automata has reached end of current track

      'Check if it can transition beyond node
      If (Not exitNode.m_block) Then

        'It can, so reset position along track back to start
        m_automataPos -= m_currentTrack.m_trackLength

        'Choose a track from the node not equal to this one, that has an unblocked exit node
        'For now choose at random
        Dim newTrack As AutoTrack = Nothing

        If (exitNode.m_listTracks.Count = 2) Then
          'Automata is travelling along straight joined sections, one of the 
          'tracks is the track its just come in on, the other is the exit, so
          'choose the exit.
          'Dim it = pExitNode.listTracks.GetEnumerator()
          'pNewTrack = it.Current
          'If pCurrentTrack Is pNewTrack Then
          '  If it.MoveNext() Then
          '    pNewTrack = it.Current
          '  End If
          'End If
          For Each entry In exitNode.m_listTracks
            If m_currentTrack IsNot entry Then
              newTrack = entry
              Exit For
            End If
          Next
        Else
          'Automata has reached a junction with several exits
          While (newTrack Is Nothing)
            Dim i = Pge.Rand() Mod exitNode.m_listTracks.Count
            Dim j = 0
            For Each it In exitNode.m_listTracks

              Dim track = it

              'Work out which node is the target destination
              Dim pNewExitNode = track.m_node(0)
              If (pNewExitNode Is exitNode) Then
                pNewExitNode = track.m_node(1)
              End If

              If (j = i AndAlso track IsNot m_currentTrack AndAlso Not pNewExitNode.m_block) Then
                newTrack = track
                Exit For
              End If

              j += 1

            Next
          End While
        End If

        'Change to new track, the origin node of the next
        'track is the same as the exit node to the current track
        m_trackOriginNode = exitNode

        'Remove the automata from the front of the queue
        'on the current track
        m_currentTrack.m_listAutos.RemoveAt(0)

        'Switch the automatas track link to the new track
        m_currentTrack = newTrack

        'Push the automata onto the back of the new track queue
        m_currentTrack.m_listAutos.Add(Me)

      Else
        'It cant pass the node, so clamp automata at this location
        m_automataPos = m_currentTrack.m_trackLength
      End If

    Else
      'Automata is travelling
      m_autoPos = m_currentTrack.GetPosition(m_automataPos, m_trackOriginNode)
    End If

  End Sub

End Class