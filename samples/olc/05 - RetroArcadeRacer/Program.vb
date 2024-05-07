' Inspired by: "Code-It-Yourself! Retro Arcade Racing Game - Programming from Scratch (Quick and Simple C++)" -- @javidx9
' https://youtu.be/KkMZI5Jbf18

Imports VbPixelGameEngine

Module Program

  Sub Main()
    Dim game As New RetroArcadeRacer
    ' Note: Having to increase the resolution of the screen significantly
    '       in order to handle drawing text to the screen while still
    '       keeping the general look of the vbConsoleGameEngine example.
    If game.Construct(640, 400, 1, 1) Then
      game.Start()
    End If
  End Sub

End Module

Class RetroArcadeRacer
  Inherits PixelGameEngine

  Private m_carPos As Double = 0.0
  Private m_distance As Double = 0.0
  Private m_speed As Double = 0.0

  Private m_curvature As Double = 0.0
  Private m_trackCurvature As Double = 0.0
  Private m_playerCurvature As Double = 0.0
  Private m_trackDistance As Double = 0.0

  Private m_currentLapTime As Double = 0.0
  Private ReadOnly m_lapTimes As New List(Of Double)

  Private ReadOnly m_track As New List(Of (Curve As Single, Distance As Single))

  Friend Sub New()
    AppName = "Retro Arcade Racer"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    m_track.Add((0.0, 10.0)) ' Short section for start/finish
    m_track.Add((0.0, 200.0))
    m_track.Add((1.0, 200.0))
    m_track.Add((0.0, 400.0))
    m_track.Add((-1.0, 100.0))
    m_track.Add((0.0, 200.0))
    m_track.Add((-1.0, 200.0))
    m_track.Add((1.0, 200.0))
    m_track.Add((0.2, 500.0))
    m_track.Add((0.0, 200.0))

    For Each entry In m_track
      m_trackDistance += entry.Distance
    Next

    m_lapTimes.Add(0.0)
    m_lapTimes.Add(0.0)
    m_lapTimes.Add(0.0)
    m_lapTimes.Add(0.0)
    m_lapTimes.Add(0.0)

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.UP).Held OrElse GetKey(Key.SPACE).Held Then
      m_speed += 2.0 * elapsedTime
    Else
      m_speed -= 1.0 * elapsedTime
    End If

    Dim carDirection = 0
    If GetKey(Key.LEFT).Held Then m_playerCurvature -= 0.7 * elapsedTime : carDirection = -1
    If getkey(Key.RIGHT).Held Then m_playerCurvature += 0.7 * elapsedTime : carDirection = 1

    If Math.Abs(m_playerCurvature - m_trackCurvature) >= 0.8 Then m_speed -= 5.0

    ' Clamp Speed
    If m_speed < 0 Then m_speed = 0
    If m_speed > 1 Then m_speed = 1

    ' Move car along track according to car speed
    m_distance += (70 * m_speed) * elapsedTime

    ' Get Point on track
    Dim offset = 0.0
    Dim trackSection = 0

    m_currentLapTime += elapsedTime
    If m_distance >= m_trackDistance Then
      m_distance -= m_trackDistance
      m_lapTimes.Insert(0, m_currentLapTime)
      m_lapTimes.RemoveAt(m_lapTimes.Count - 1)
      m_currentLapTime = 0.0
    End If

    ' Find position on track (could optimise)
    While trackSection < m_track.Count AndAlso offset <= m_distance
      offset += m_track(trackSection).Distance
      trackSection += 1
    End While

    Dim targetCurvature = m_track(trackSection - 1).Curve
    Dim trackCurveDiff = (targetCurvature - m_curvature) * elapsedTime * m_speed
    m_curvature += trackCurveDiff

    m_trackCurvature += m_curvature * elapsedTime * m_speed

    ' Clear screen
    'Fill(0, 0, ScreenWidth, ScreenHeight, AscW(" "), 0)

    ' Draw Sky - light blue and dark blue
    For y = 0 To ScreenHeight() \ 2
      For x = 0 To ScreenWidth() - 1
        Draw(x, y, If(y < ScreenHeight() \ 4, Presets.DarkBlue, Presets.Blue))
      Next
    Next

    ' Draw Scenery - our hills are a rectified sine wave, where the phase
    ' accumulated track curvature
    For x = 0 To ScreenWidth() - 1
      Dim hillHeight = Math.Abs(Math.Sin(x * 0.001 + m_trackCurvature) * (ScreenHeight * 0.2))
      For y = ScreenHeight() \ 2 - CInt(hillHeight) To ScreenHeight() \ 2
        Draw(x, y, Presets.DarkYellow)
      Next
    Next

    For y = 0 To ScreenHeight() \ 2
      For x = 0 To ScreenWidth() - 1

        Dim perspective = y / (ScreenHeight() / 2)

        Dim middlePoint = 0.5 + m_curvature * Math.Pow((1.0 - perspective), 3)
        Dim roadWidth = 0.1 + (perspective * 0.8) '0.6
        Dim clipWidth = roadWidth * 0.15
        roadWidth *= 0.5

        Dim leftGrass = (middlePoint - roadWidth - clipWidth) * ScreenWidth()
        Dim leftClip = (middlePoint - roadWidth) * ScreenWidth()
        Dim rightClip = (middlePoint + roadWidth) * ScreenWidth()
        Dim rightGrass = (middlePoint + roadWidth + clipWidth) * ScreenWidth()

        Dim row = ScreenHeight() \ 2 + y

        Dim grassColor = If(Math.Sin(20.0F * Math.Pow(1.0F - perspective, 3) + m_distance * 0.1F) > 0.0F, Presets.Green, Presets.DarkGreen)
        Dim clipColor = If(Math.Sin(80.0F * Math.Pow(1.0F - perspective, 2) + m_distance) > 0.0F, Presets.Red, Presets.White)

        Dim roadColor = If(trackSection - 1 = 0, Presets.White, Presets.Gray)

        If x >= 0 AndAlso x < leftGrass Then Draw(x, row, grassColor)
        If x >= leftGrass AndAlso x < leftClip Then Draw(x, row, clipColor)
        If x >= leftClip AndAlso x < rightClip Then Draw(x, row, roadColor)
        If x >= rightClip AndAlso x < rightGrass Then Draw(x, row, clipColor)
        If x >= rightGrass AndAlso x < ScreenWidth() Then Draw(x, row, grassColor)

      Next
    Next

    ' Draw Car
    Dim hOffset = 50
    m_carPos = m_playerCurvature - m_trackCurvature
    Dim carPos = ScreenWidth() \ 2 + CInt((ScreenWidth() * m_carPos) / 2) - hOffset

    ' Note: Car is still a bit oversized when compared to the original vbConsoleGameEngine example.
    Dim yOffset = 300
    Select Case carDirection
      Case 0
        DrawString(carPos, yOffset, "   ||####||   ", Presets.Black)
        DrawString(carPos, yOffset + 10, "      ##      ", Presets.Black)
        DrawString(carPos, yOffset + 20, "     ####     ", Presets.Black)
        DrawString(carPos, yOffset + 30, "     ####     ", Presets.Black)
        DrawString(carPos, yOffset + 40, "|||  ####  |||", Presets.Black)
        DrawString(carPos, yOffset + 50, "|||########|||", Presets.Black)
        DrawString(carPos, yOffset + 60, "|||  ####  |||", Presets.Black)
      Case +1
        DrawString(carPos, yOffset, "      //####//", Presets.Black)
        DrawString(carPos, yOffset + 10, "         ##   ", Presets.Black)
        DrawString(carPos, yOffset + 20, "       ####   ", Presets.Black)
        DrawString(carPos, yOffset + 30, "      ####    ", Presets.Black)
        DrawString(carPos, yOffset + 40, "///  ####//// ", Presets.Black)
        DrawString(carPos, yOffset + 50, "//#######///O ", Presets.Black)
        DrawString(carPos, yOffset + 60, "/// #### //// ", Presets.Black)
      Case -1
        DrawString(carPos, yOffset, "\\####\\      ", Presets.Black)
        DrawString(carPos, yOffset + 10, "   ##         ", Presets.Black)
        DrawString(carPos, yOffset + 20, "   ####       ", Presets.Black)
        DrawString(carPos, yOffset + 30, "    ####      ", Presets.Black)
        DrawString(carPos, yOffset + 40, " \\\\####  \\\", Presets.Black)
        DrawString(carPos, yOffset + 50, " O\\#######\\", Presets.Black)
        DrawString(carPos, yOffset + 60, " \\\\ #### \\\", Presets.Black)
    End Select

    ' Draw Stats
    DrawString(0, 0, $"Distance        : {m_distance:F5}")
    DrawString(0, 10, $"Target Curvature: {m_curvature:F5}")
    DrawString(0, 20, $"Player Curvature: {m_playerCurvature:F5}")
    DrawString(0, 30, $"Player Speed    : {m_speed:F5}")
    DrawString(0, 40, $"Track Curvature : {m_trackCurvature:F5}")

    Dim DetermineDisplayTime = Function(t As Double) As String ' Lambda expression to turn floating point seconds into minutes:seconds:millis string
                                 Dim minutes As Double = CInt(t) \ 60
                                 t -= minutes * 60
                                 Dim seconds = CInt(Fix(t))
                                 t -= seconds
                                 Dim milliseconds = CInt(Fix(t * 1000))
                                 Return $"{minutes:00}.{seconds:00}:{milliseconds:0000}"
                               End Function

    DrawString(10, 60, DetermineDisplayTime(m_currentLapTime))

    ' Display last 5 lap times
    Dim j = 80
    For Each lapTime In m_lapTimes
      DrawString(10, j, DetermineDisplayTime(lapTime))
      j += 10
    Next

    Return True

  End Function

End Class