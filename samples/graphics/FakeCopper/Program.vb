Imports VbPixelGameEngine
Imports VbPixelGameEngine.SpecBAS

Friend Module Program

  Sub Main()
    Dim game As New FakeCopper
    If game.Construct(800, 480, False, True) Then
      game.Start()
    End If
  End Sub

End Module

Friend Class FakeCopper
  Inherits PixelGameEngine

  Private ReadOnly bs As Integer = 32
  Private ReadOnly numOfStars As Integer = 1000
  Private ReadOnly s(numOfStars, 3) As Single
  Private a As Single = 255.0!
  Private i(3) As Single

  Friend Sub New()
    AppName = "Fake Copper Effects"
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    CLS(0)
    DEGREES()

    For f = 1 To bs
      Palette(f) = New Pixel(a, a, a)
      a -= 255.0! / bs
    Next

    i = {0, bs * 0.75!, bs * 0.5!, bs * 0.125!}

    For f = 1 To numOfStars
      s(f, 1) = CSng(Rnd * SCRW())
      s(f, 2) = CSng(Rnd * SCRH())
      s(f, 3) = CSng(Rnd * 2 + 1)
    Next f

    '30 screen lock:CLS 0: degrees
    '40 numofbars=15,bs=32,numofstars=1000,sh=(scrh/2)-(bs/2),sc=bs/15,swc=scrh/400
    '45 a=255: For f = 1 To bs : Palette f, a, a, a: a -= 255 / bs : Next f
    Return True
  End Function

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False

    Dim numOfBars = 5
    Dim sh = (SCRH() / 2.0!) - (bs / 2.0!)
    'Dim sc = bs / 15.0!
    Dim swc = SCRH() / 400.0!

    '60 CLS 0: For t = 1 To numofstars : PLOT INK i(s(t, 3));s(t,1),s(t,2): s(t, 1) -= s(t, 3) : If s(t, 1) < 0 Then s(t, 1) = SCRW,s(t,2)=rnd*Scrh,s(t,3)=rnd*2+1
    '70 next t

    CLS(0)

    For t = 1 To numOfStars
      PLOT(i(CInt(s(t, 3))), s(t, 1), s(t, 2))
      s(t, 1) -= s(t, 3)
      If s(t, 1) < 0 Then
        s(t, 1) = SCRW()
        s(t, 2) = CSng(Rnd * SCRH())
        s(t, 3) = (CSng(Rnd) * 2) + 1
      End If
    Next

    Dim yy As Single
    For b = 1 To numOfBars
      Dim by = sh + SIN(a + (b * 10 * swc)) * 50 * swc
      yy = by
      For c = bs To 1 Step -2
        DrawLine(0, yy, SCRW, yy, Palette(c))
        yy += 1
      Next
      For c = 1 To bs Step 2
        DrawLine(0, yy, SCRW, yy, Palette(c))
        yy += 1
      Next
    Next
    a += 4

    Return True

  End Function

End Class