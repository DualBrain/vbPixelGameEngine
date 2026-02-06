Imports VbPixelGameEngine
Imports System.IO
Imports System.Runtime.InteropServices

Friend Module Program

  Sub Main()
    Console.WriteLine("=== CapsLock Test Starting ===")
    Console.WriteLine($"Platform: {Environment.OSVersion}")
    
    ' Create and run the game
    Dim game As New Playground()
    If game.Construct(900, 600, 1, 1) Then
      game.Start()
    End If
    
    Console.WriteLine("=== CapsLock Test Ended ===")
  End Sub

End Module

Friend Class Playground
  Inherits PixelGameEngine
  
  Friend Sub New()
    AppName = "Playground - CapsLock Test"
    Console.WriteLine("Playground constructor called")
  End Sub
  
  Private Sub Log(message As String)
    Dim timestamp As String = DateTime.Now.ToString("HH:mm:ss.fff")
    Console.WriteLine($"[{timestamp}] {message}")
    
    ' Also write to log file
    Try
      File.AppendAllText("capslock_test.log", $"[{timestamp}] {message}{Environment.NewLine}")
    Catch ex As Exception
      ' Ignore file errors
    End Try
  End Sub

  Protected Overrides Function OnUserCreate() As Boolean

    Log("=== OnUserCreate Called ===")
    Clear(m_bg)
    
    ' Initialize CapsLock test state
    m_lastCapsLockState = CapsLock()
    Log($"Initial CapsLock LOCK state: {m_lastCapsLockState}")
    Log($"Key.CAPS_LOCK enum value: {Key.CAPS_LOCK}")
    
    ' Test basic CapsLock functionality
    Log("Testing basic CapsLock API:")
    Log($"  CapsLock() property (LOCK state): {CapsLock()}")
    Log($"  GetKey(CAPS_LOCK).Held (key press): {GetKey(Key.CAPS_LOCK).Held}")
    Log($"  GetKey(CAPS_LOCK).Pressed (key event): {GetKey(Key.CAPS_LOCK).Pressed}")
    
    Log("=== OnUserCreate Completed ===")
    Return True

  End Function
  
  Private Sub TestCapsLockState()
    Static frameCount As Integer = 0
    Static lastLogFrame As Integer = 0
    frameCount += 1
    
    ' Check current CapsLock LOCK state (not key press state)
    Dim lockState As Boolean = CapsLock()
    
    ' Check key press/release state
    Dim keyHeld As Boolean = GetKey(Key.CAPS_LOCK).Held
    Dim keyPressed As Boolean = GetKey(Key.CAPS_LOCK).Pressed
    Dim keyReleased As Boolean = GetKey(Key.CAPS_LOCK).Released
    
    ' Log every frame for debugging (but limit console spam)
    If frameCount - lastLogFrame >= 30 Then ' Every 0.5 seconds
      Log($"Frame {frameCount}: LOCK={lockState}, KeyHeld={keyHeld}, KeyPressed={keyPressed}")
      lastLogFrame = frameCount
    End If
    
    ' Detect LOCK state changes (this is what matters for CapsLock!)
    If lockState <> m_lastCapsLockState Then
      m_capsLockChangeCount += 1
      Log($"*** CAPSLOCK LOCK STATE CHANGED: {m_lastCapsLockState} -> {lockState} (Change #{m_capsLockChangeCount})")
      Log($"    Frame: {frameCount}, Time: {DateTime.Now:HH:mm:ss.fff}")
      m_lastCapsLockState = lockState
    End If
    
    ' Detect key press events (for debugging)
    If keyPressed Then
      m_capsLockKeyPressCount += 1
      Log($"*** CAPSLOCK KEY PRESSED (Press #{m_capsLockKeyPressCount}) - Lock is now: {lockState}")
      Log($"    Frame: {frameCount}, Time: {DateTime.Now:HH:mm:ss.fff}")
    End If
    
    ' Detect key release events (for debugging)
    If keyReleased Then
      Log($"*** CAPSLOCK KEY RELEASED - Lock remains: {lockState}")
      Log($"    Frame: {frameCount}, Time: {DateTime.Now:HH:mm:ss.fff}")
    End If
    
    ' Display CapsLock LOCK state in top-left corner
    Dim lockColor As Pixel = If(lockState, New Pixel(0, 255, 0, 255), New Pixel(255, 0, 0, 255))
    DrawString(10, 10, $"CapsLock LOCK: {If(lockState, "ON", "OFF")}", lockColor, 2)
    DrawString(10, 35, $"Lock Changes: {m_capsLockChangeCount}", New Pixel(255, 255, 255, 255), 1)
    DrawString(10, 50, $"Key Presses: {m_capsLockKeyPressCount}", New Pixel(255, 255, 255, 255), 1)
    DrawString(10, 65, $"Frame: {frameCount}", New Pixel(255, 255, 255, 255), 1)
    
    ' Show key status separately from lock state
    If keyHeld Then
      DrawString(10, 85, "Key Currently Held", New Pixel(255, 255, 0, 255), 1)
    ElseIf keyPressed Then
      DrawString(10, 85, "Key Just Pressed!", New Pixel(255, 165, 0, 255), 1)
    ElseIf keyReleased Then
      DrawString(10, 85, "Key Just Released!", New Pixel(255, 165, 255, 255), 1)
    Else
      DrawString(10, 85, "Key Not Pressed", New Pixel(200, 200, 200, 255), 1)
    End If
    
    ' Visual indicator for LOCK state (larger and more prominent)
    If lockState Then
      FillCircle(850, 30, 15, New Pixel(0, 255, 0, 255))
DrawString(825, 55, "LOCKED", New Pixel(0, 255, 0, 255), 1)
    Else
      DrawString(825, 55, "UNLOCKED", New Pixel(255, 0, 0, 255), 1)
    End If
    
    ' Visual indicator for key press state (smaller, secondary)
    If keyHeld Then
      FillCircle(750, 30, 8, New Pixel(255, 255, 0, 255))
      DrawString(730, 45, "KEY", New Pixel(255, 255, 0, 255), 1)
    Else
      DrawCircle(750, 30, 8, New Pixel(100, 100, 100, 255))
      DrawString(730, 45, "KEY", New Pixel(100, 100, 100, 255), 1)
    End If
    
    ' Instructions
    DrawString(10, 110, "CAPS LOCK TEST:", New Pixel(255, 255, 255, 255), 2)
    DrawString(10, 135, "Large circle = LOCK state (persists)", New Pixel(200, 200, 200, 255), 1)
    DrawString(10, 150, "Small circle = Key press state", New Pixel(200, 200, 200, 255), 1)
    DrawString(10, 165, "Press CapsLock to toggle LOCK", New Pixel(200, 200, 200, 255), 1)
    DrawString(10, 180, "Press ESC to exit", New Pixel(200, 200, 200, 255), 1)
    
    ' Test other keys for comparison
    If frameCount Mod 120 = 0 Then ' Every 2 seconds
      Log($"Other keys - SPACE Held: {GetKey(Key.SPACE).Held}, A Held: {GetKey(Key.A).Held}")
    End If
  End Sub

  Private m_mt As Single

  Private m_bg As New Pixel(7, 36, 18)
  Private m_fg As New Pixel(0, 239, 130)
  Private ReadOnly m_centerX As Integer = 450
  Private ReadOnly m_centerY As Integer = 300
  Private ReadOnly m_scaleMax As Integer = 150
  Private m_scale As Integer = 1
  Private m_degree As Integer = 1

  Private m_saltX As Integer = 10
  Private m_saltY As Integer = 7
  
  ' CapsLock testing variables
  Private m_lastCapsLockState As Boolean = False
  Private m_capsLockChangeCount As Integer = 0
  Private m_capsLockKeyPressCount As Integer = 0

  Protected Overrides Function OnUserUpdate(elapsedTime As Single) As Boolean

    If GetKey(Key.ESCAPE).Pressed Then Return False
    
    ' Test CapsLock functionality
    TestCapsLockState()

    If GetKey(Key.SPACE).Pressed Then
      m_scale = 1
      m_degree = 1
      m_saltX = CInt(Fix(Rnd * 20))
      m_saltY = CInt(Fix(Rnd * 20))
      Clear(m_bg)
    End If

    Dim mt = CSng(1 / 60)

    m_mt += elapsedTime
    If m_mt < mt Then
      Return True
    Else
      m_mt -= mt
    End If

    If False Then

      ' one pixel per pass...

      Dim radian = m_degree * Math.PI / 180
      Dim x = (m_scaleMax + 20 + m_scale * Math.Sin(m_saltX * radian)) * Math.Cos(radian)
      Dim y = (m_scaleMax + m_scale * Math.Sin(m_saltY * radian)) * Math.Sin(radian)
      Draw(m_centerX + x, m_centerY + y, m_fg)

      If m_degree > 360 * 2 Then
        m_scale += 2
        If m_scale > m_scaleMax Then m_scale = 1 : Clear(m_bg)
        m_degree = 1
      Else
        m_degree += 1
      End If

    Else

      ' one rotation per pass...

      For degree = 1 To 360 * 2
        Dim radian = degree * Math.PI / 180
        Dim x = (m_scaleMax + 20 + m_scale * Math.Sin(m_saltX * radian)) * Math.Cos(radian)
        Dim y = (m_scaleMax + m_scale * Math.Sin(m_saltY * radian)) * Math.Sin(radian)
        Draw(m_centerX + x, m_centerY + y, m_fg)
      Next

      m_scale += 2
      If m_scale > m_scaleMax Then
        m_scale = 1
        Clear(m_bg)
        m_saltX = CInt(Fix(Rnd * 20))
        m_saltY = CInt(Fix(Rnd * 20))
      End If

    End If

    Return True

  End Function

End Class