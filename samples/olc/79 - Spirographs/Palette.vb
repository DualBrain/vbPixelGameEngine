Imports VbPixelGameEngine

Public Class Palette

  Public Enum Stock
    Empty
    Greyscale
    ColdHot
    Spectrum
  End Enum

  Private ReadOnly m_colors As List(Of KeyValuePair(Of Double, Pixel))

  Public Sub New(Optional stock As Stock = Stock.Empty)

    m_colors = New List(Of KeyValuePair(Of Double, Pixel))()

    Select Case stock
      Case Stock.Empty
        m_colors.Clear()
      Case Stock.Greyscale
        m_colors = New List(Of KeyValuePair(Of Double, Pixel)) From {
          New KeyValuePair(Of Double, Pixel)(0.0, Presets.Black),
          New KeyValuePair(Of Double, Pixel)(1.0, Presets.White)}
      Case Stock.ColdHot
        m_colors = New List(Of KeyValuePair(Of Double, Pixel)) From {
          New KeyValuePair(Of Double, Pixel)(0.0, Presets.Cyan),
          New KeyValuePair(Of Double, Pixel)(0.5, Presets.Black),
          New KeyValuePair(Of Double, Pixel)(1.0, Presets.Yellow)}
      Case Stock.Spectrum
        m_colors = New List(Of KeyValuePair(Of Double, Pixel)) From {
          New KeyValuePair(Of Double, Pixel)(0.0 / 6.0, Presets.Red),
          New KeyValuePair(Of Double, Pixel)(1.0 / 6.0, Presets.Yellow),
          New KeyValuePair(Of Double, Pixel)(2.0 / 6.0, Presets.Green),
          New KeyValuePair(Of Double, Pixel)(3.0 / 6.0, Presets.Cyan),
          New KeyValuePair(Of Double, Pixel)(4.0 / 6.0, Presets.Blue),
          New KeyValuePair(Of Double, Pixel)(5.0 / 6.0, Presets.Magenta),
          New KeyValuePair(Of Double, Pixel)(6.0 / 6.0, Presets.Red)}
    End Select

  End Sub

  Public Function Sample(t As Double) As Pixel
    ' Return obvious sample values
    If m_colors.Count = 0 Then
      Return Presets.Black
    End If

    If m_colors.Count = 1 Then
      Return m_colors(0).Value
    End If

    ' Iterate through colour entries until we find the first entry
    ' with a location greater than our sample point
    Dim i = t Mod 1.0
    Dim it = m_colors.GetEnumerator()

    While it.MoveNext() AndAlso i > it.Current.Key
    End While

    ' If that is the first entry, just return it
    If it.Current.Key = m_colors(0).Key Then
      Return it.Current.Value
    Else
      ' else get the preceding entry, and lerp between the two proportionally
      Dim it_p = m_colors(m_colors.IndexOf(it.Current) - 1)
      Return Pixel.Lerp(it_p.Value, it.Current.Value, CSng((i - it_p.Key) / (it.Current.Key - it_p.Key)))
    End If
  End Function

  Public Sub SetColor(d As Double, col As Pixel)

    Dim i = Math.Min(Math.Max(d, 0.0), 1.0)

    ' If d already exists, replace it
    Dim it = m_colors.Find(Function(p) p.Key = i)

    If it.Key <> 0 Then 'OrElse it.Value IsNot Nothing Then
      ' Palette entry was found, so replace colour entry
      m_colors(m_colors.IndexOf(it)) = New KeyValuePair(Of Double, Pixel)(i, col)
    Else
      ' Palette entry not found, so add it, and sort palette vector
      m_colors.Add(New KeyValuePair(Of Double, Pixel)(i, col))
      m_colors.Sort(Function(p1, p2) p1.Key.CompareTo(p2.Key))
    End If

  End Sub

End Class