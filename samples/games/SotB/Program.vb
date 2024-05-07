Imports VbPixelGameEngine

'NOTE: Currently doesn't work on Linux due to missing PNG file support.

Friend Module Program

  Sub Main()
    Dim game As New ShadowOfTheBeast
    If game.Construct(320, 180, 4, 4) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class ShadowOfTheBeast
  Inherits PixelGameEngine

#Region "Member Variables"

  'Private m_moon As Sprite
  Private m_blimpBig As Sprite
  Private m_blimpSmall As Sprite

  Private m_cloud1 As Sprite
  Private m_cloud2 As Sprite
  Private m_cloud3 As Sprite
  Private m_cloud4 As Sprite
  Private m_cloud5 As Sprite

  Private m_mountains As Sprite

  Private m_grass1 As Sprite
  Private m_grass2 As Sprite
  Private m_grass3 As Sprite
  Private m_grass4 As Sprite
  Private m_grass5 As Sprite

  Private m_trees As Sprite
  Private m_trees2 As Sprite
  Private m_fence As Sprite

  Private ReadOnly m_sp As New List(Of Sprite)
  Private ReadOnly m_bat As New List(Of Sprite)

  Private m_cloudX1 As Integer
  Private m_cloudX2 As Integer
  Private m_cloudX3 As Integer
  Private m_cloudX4 As Integer
  Private m_cloudX5 As Integer

  Private m_mountainsX As Integer

  Private m_grassX1 As Integer
  Private m_grassX2 As Integer
  Private m_grassX3 As Integer
  Private m_grassX4 As Integer
  Private m_grassX5 As Integer

  Private m_fenceX As Integer
  Private m_treesX1 As Integer = -20
  Private m_treesX2 As Integer = -20

  Private m_blimpBigX As Integer
  Private m_blimpSmallX As Integer

  Private c As Integer
  Private dudeflag As Single = 320.0!
  Private dudeframe As Integer = 0
  Private batx As Integer = -640
  Private baty As Integer = 60
  Private batframe As Integer = 0

#End Region

  Friend Sub New()
    AppName = "Shadow of the Beast (Demo)"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    ' Load assets

    'm_moon = New Sprite("assets/moon.png")
    m_blimpBig = New Sprite("assets/blimp_big.png")
    m_blimpSmall = New Sprite("assets/blimp_small.png")

    m_cloud1 = New Sprite("assets/cloud1.png")
    m_cloud2 = New Sprite("assets/cloud2.png")
    m_cloud3 = New Sprite("assets/cloud3.png")
    m_cloud4 = New Sprite("assets/cloud4.png")
    m_cloud5 = New Sprite("assets/cloud5.png")

    m_mountains = New Sprite("assets/mountains.png")

    m_grass1 = New Sprite("assets/grass1.png")
    m_grass2 = New Sprite("assets/grass2.png")
    m_grass3 = New Sprite("assets/grass3.png")
    m_grass4 = New Sprite("assets/grass4.png")
    m_grass5 = New Sprite("assets/grass5.png")

    m_trees = New Sprite("assets/trees.png")
    m_trees2 = New Sprite("assets/trees2.png")
    m_fence = New Sprite("assets/fence.png")

    For i = 1 To 6
      m_sp.Add(New Sprite($"assets/dude{i}.png"))
    Next

    For i = 1 To 5
      m_bat.Add(New Sprite($"assets/bat{i}.png"))
    Next

    Return True

  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    Static t As Single : Const DELAY = 1.0! / 60.0!
    t += elapsedTime : If t < DELAY Then Return True Else t -= DELAY

    ' Clear the screen (same color as the sky)
    Clear(New Pixel(102, 119, 136))

    ' Draw the "sunset"
    Dim r = 102.0!, g = 119.0!, b = 136.0!
    For y = 69 To 180
      DrawLine(0, y, 319, y, New Pixel(r, g, b)) : r += 1.37!
    Next

    ' Switch spritd drawing mode so that PNG transparency is honored.
    ' This is a pretty taxing overhead, so only enable if absolutely
    ' needed.. which in this case it is.
    SetPixelMode(Pixel.Mode.Mask)

    If c Mod 3 = 0 Then m_blimpBigX += 1 : If m_blimpBigX > 640 Then m_blimpBigX = 0
    If m_blimpBigX > 100 - m_blimpBig.Width AndAlso m_blimpBigX < (ScreenWidth + 100) + m_blimpBig.Width Then
      DrawSprite(m_blimpBigX - 100, 20, m_blimpBig, 1)
    End If
    If c Mod 7 = 0 Then m_blimpSmallX -= 1 : If m_blimpSmallX < 0 Then m_blimpSmallX = 640
    If m_blimpSmallX > 100 - m_blimpSmall.Width AndAlso m_blimpSmallX < (ScreenWidth + 100) + m_blimpSmall.Width Then
      DrawSprite(m_blimpSmallX - 100, 48, m_blimpSmall, 1)
    End If

    Roll(m_cloudX1, 0, m_cloud1, 1)
    Roll(m_cloudX2, 23, m_cloud2, If((c And 1) > 0, 1, 0))
    Roll(m_cloudX3, 63, m_cloud3, If((c Mod 3) = 0, 1, 0))
    Roll(m_cloudX4, 82, m_cloud4, If((c Mod 4) = 0, 1, 0))
    Roll(m_cloudX5, 91, m_cloud5, If((c Mod 5) = 0, 1, 0))
    Roll(m_mountainsX, 80, m_mountains, If((c And 1) > 0, 1, 0))
    Roll(m_grassX1, 152, m_grass1, 2)
    Roll(m_grassX2, 154, m_grass2, 3)
    Roll(m_grassX3, 157, m_grass3, 4)
    Roll(m_grassX4, 164, m_grass4, 5)
    Roll(m_grassX5, 171, m_grass5, 6)
    Roll(m_treesX2, -20, m_trees2, If((c And 3) > 0, 2, 1))
    Roll(m_treesX1, -20, m_trees, 2)
    Roll(m_fenceX, 156, m_fence, 6)

    ' character

    If dudeflag > 144 Then dudeflag -= 0.25!
    DrawSprite(CInt(Fix(dudeflag)), 105, m_sp(dudeframe), 1)
    If c Mod 6 = 0 Then dudeframe += 1 : If dudeframe > 5 Then dudeframe = 0

    ' bat

    DrawSprite(batx, baty, m_bat(batframe), 1)
    If c Mod 6 = 0 Then batframe += 1 : If batframe > 4 Then batframe = 0
    If batx < ScreenWidth + 100 Then
      batx += 3
    Else
      batx = -(640 + (320 * CInt(Rnd * 12))) : baty = Rand Mod 80
    End If

    SetPixelMode(Pixel.Mode.Normal)

    c += 1

    Return True

    End Function

  Private Sub Roll(ByRef x As Integer, ByRef y As Integer, sprite As Sprite, dirX As Integer)
    x += dirX
    'If sprite.Width <= ScreenWidth Then
    '  ' The sprite is equal or lesser than the width of the screen.
    '  If x < -ScreenWidth Then x = 0
    '  If x > ScreenWidth Then x = 0
    '  DrawSprite(x, y, sprite, 1)
    '  If x < 0 Then DrawSprite(ScreenWidth + x, y, sprite, 1)
    '  If x > 0 Then DrawSprite(x - ScreenWidth, y, sprite, 1)
    'Else
    ' The sprite is larger than the screen.
    If x > sprite.Width Then x = 0
    If x < -(sprite.Width) Then x = 0
    DrawSprite(x, y, sprite, 1)
    If x > 0 Then DrawSprite(x - sprite.Width, y, sprite, 1)
    If x < -(sprite.Width - ScreenWidth) Then DrawSprite(x + sprite.Width, y, sprite, 1)
    'End If
  End Sub

End Class