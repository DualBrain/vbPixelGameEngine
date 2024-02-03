Imports VbPixelGameEngine

Friend Module Program

  Sub Main()
    Dim demo As New BoingBall
    If demo.Construct(640, 480, 1, 1, False, True) Then
      demo.Start()
    End If
  End Sub

End Module

Friend Class BoingBall
  Inherits PixelGameEngine

  Private Class Pt
    Public X As Double
    Public Y As Double
    Sub New(x As Double, y As Double)
      Me.X = x : Me.Y = y
    End Sub
  End Class

  Private BLACK As Pixel = New Pixel(0, 0, 0)
  Private GRAY As Pixel = New Pixel(102, 102, 102)
  Private LIGHTGRAY As Pixel = New Pixel(170, 170, 170)
  Private WHITE As Pixel = New Pixel(255, 255, 255)
  Private RED As Pixel = New Pixel(255, 26, 1)
  Private PURPLE As Pixel = New Pixel(183, 45, 168)

  Private done As Boolean = False
  Private phase As Double = 0.0
  Private dp As Double = 2.5
  Private x As Double = 320
  Private dx As Double = 2.1
  Private right As Boolean = True
  Private yAng As Double = 0.0

  Friend Sub New()
    AppName = "Boing"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Clear(Presets.Gray)

    phase = (phase + If(right, 45.0 - dp, dp)) Mod 45.0
    x += If(right, dx, -dx)

    If x >= 505 Then
      right = False
    ElseIf x <= 135 Then
      right = True
    End If

    yAng = (yAng + 1.5) Mod 360.0
    Dim yValue = 350.0 - 200.0 * Math.Abs(Math.Cos(yAng * Math.PI / 180.0))

    CalcAndDraw(phase, 120.0, x, yValue)

    Return True

  End Function

  Private Shared Function GetLat(phase As Double, i As Integer) As Double
    If i = 0 Then
      Return -90.0
    ElseIf i = 9 Then
      Return 90.0
    Else
      Return -90.0 + phase + (i - 1) * 22.5
    End If
  End Function

  Private Shared Function CalcPoints(phase As Double) As List(Of List(Of Pt))

    Dim points As New List(Of List(Of Pt))

    For i = 0 To 9

      points.Add(New List(Of Pt))
      Dim lat = GetLat(phase, i)
      Dim sinLat = Math.Sin(lat * Math.PI / 180.0)

      For j = 0 To 8
        Dim lon = -90.0 + j * 22.5
        Dim y = Math.Sin(lon * Math.PI / 180.0)
        Dim l = Math.Cos(lon * Math.PI / 180.0)
        Dim x = sinLat * l
        points(i).Add(New Pt(x, y))
      Next

    Next

    Return points

  End Function

  Private Shared Sub TiltSphere(points As List(Of List(Of Pt)), ang As Double)

    Dim st = MathF.Sin(ang * Math.PI / 180.0)
    Dim ct = MathF.Cos(ang * Math.PI / 180.0)

    For i = 0 To 9
      For j = 0 To 8
        Dim point = points(i)(j)
        Dim x = point.X
        Dim y = point.Y
        x = x * ct - y * st
        y = x * st + y * ct
        points(i)(j) = New Pt(x, y)
      Next
    Next

  End Sub

  Private Shared Sub ScaleAndTranslate(points As List(Of List(Of Pt)), s As Double, tx As Double, ty As Double)
    For i = 0 To 9
      For j = 0 To 8
        Dim point = points(i)(j)
        Dim x = point.X
        Dim y = point.Y
        x = x * s + tx
        y = y * s + ty
        points(i)(j) = New Pt(x, y)
      Next
    Next
  End Sub

  Private Shared Sub Transform(points As List(Of List(Of Pt)), s As Double, tx As Double, ty As Double)
    TiltSphere(points, 17.0)
    ScaleAndTranslate(points, s, tx, ty)
  End Sub

  Private Sub DrawMeridians(points As List(Of List(Of Pt)))
    For i = 0 To 9
      For j = 0 To 7
        Dim p1 = points(i)(j)
        Dim p2 = points(i)(j + 1)
        DrawLine(p1.X, p1.Y, p2.X, p2.Y, BLACK)
      Next
    Next
  End Sub

  Private Sub DrawParabels(points As List(Of List(Of Pt)))
    For i = 0 To 6
      Dim p1 = points(0)(i + 1)
      Dim p2 = points(9)(i + 1)
      DrawLine(p1.X, p1.Y, p2.X, p2.Y, BLACK)
    Next
  End Sub

  Private Sub FillTiles(points As List(Of List(Of Pt)), alter As Boolean)
    For j = 0 To 7
      For i = 0 To 8
        Dim p1 = points(i)(j)
        Dim p2 = points(i + 1)(j)
        Dim p3 = points(i + 1)(j + 1)
        Dim p4 = points(i)(j + 1)
        'DrawPolygon({p1, p2, p3, p4}.ToList, If(alter, RED, WHITE))
        FillPolygon({p1, p2, p3, p4}.ToList, If(alter, RED, WHITE))
        alter = Not alter
      Next
    Next
  End Sub

  Private Sub DrawShadow(points As List(Of List(Of Pt)))

    Dim ps As New List(Of Pt)()

    For i = 0 To 8
      Dim point = points(0)(i)
      ps.Add(New Pt(point.X + 50, point.Y))
    Next

    For i = 0 To 7
      Dim point = points(9)(7 - i)
      ps.Add(New Pt(point.X + 50, point.Y))
    Next

    'DrawPolygon(ps, GRAY)
    FillPolygon(ps, GRAY)

  End Sub

  Private Sub DrawPolygon(points As List(Of Pt), c As Pixel)
    For i = 0 To points.Count - 2
      DrawLine(points(i).X, points(i).Y, points(i + 1).X, points(i + 1).Y, c)
    Next
    DrawLine(points(points.Count - 1).X, points(points.Count - 1).Y, points(0).X, points(0).Y, c)
  End Sub

  ' Fill a polygon using a list of points
  Private Sub FillPolygon(points As List(Of Pt), color As Pixel)

    ' Find the minimum and maximum Y coordinates to determine the bounding box
    Dim minY = points.Min(Function(p) p.Y)
    Dim maxY = points.Max(Function(p) p.Y)

    ' For each scanline within the bounding box, find the intersections with the polygon edges
    For y = minY To maxY

      Dim intersections As New List(Of Integer)

      ' Find intersections with each edge
      For i = 0 To points.Count - 1

        Dim nextIndex = If(i < points.Count - 1, i + 1, 0)
        Dim y1 = points(i).Y
        Dim y2 = points(nextIndex).Y

        ' Check if the current scanline is within the edge's Y range
        If (y1 <= y AndAlso y2 > y) OrElse (y2 <= y AndAlso y1 > y) Then
          ' Calculate the X coordinate of the intersection point
          Dim xIntersection = CInt(points(i).X + (y - y1) / (y2 - y1) * (points(nextIndex).X - points(i).X))
          intersections.Add(xIntersection)
        End If
      Next

      ' Sort the intersections in ascending order
      intersections.Sort()

      ' Draw horizontal line segments between pairs of intersections
      For i = 0 To intersections.Count - 2 Step 2
        DrawLine(intersections(i), y, intersections(i + 1), y, color)
      Next

    Next

  End Sub

  Private Sub DrawWireframe()

    For i = 0 To 12
      Dim p1 As New Pt(50, i * 36)
      Dim p2 As New Pt(590, i * 36)
      DrawLine(p1.X, p1.Y, p2.X, p2.Y, PURPLE)
    Next

    For i = 0 To 15
      Dim p1 As New Pt(50 + i * 36, 0)
      Dim p2 As New Pt(50 + i * 36, 432)
      DrawLine(p1.X, p1.Y, p2.X, p2.Y, PURPLE)
    Next

    For i = 0 To 15
      Dim p1 As New Pt(50 + i * 36, 432)
      Dim p2 As New Pt(CInt(i * 42.666), 480)
      DrawLine(p1.X, p1.Y, p2.X, p2.Y, PURPLE)
    Next

    Dim ys() = {442, 454, 468}
    For i = 0 To 2
      Dim y = ys(i)
      Dim x1 = 50 - 50.0 * (y - 432) / (480.0 - 432.0)
      Dim p1 As New Pt(CInt(x1), y)
      Dim p2 As New Pt(640 - CInt(x1), y)
      DrawLine(p1.X, p1.Y, p2.X, p2.Y, PURPLE)
    Next

  End Sub

  Private Sub CalcAndDraw(phase As Double, scale As Double, x As Double, y As Double)
    Dim points = CalcPoints(phase Mod 22.5)
    Transform(points, scale, x, y)
    DrawShadow(points)
    DrawWireframe()
    FillTiles(points, phase >= 22.5)
    'DrawMeridians(points)
    'DrawParabels(points)
  End Sub

End Class