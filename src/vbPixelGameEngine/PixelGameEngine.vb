Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.OSPlatform
Imports System.Runtime.InteropServices.RuntimeInformation
Imports System.Threading

Public Module Singleton

  Public Property AtomActive As Boolean
  Public Property MapKeys As New Dictionary(Of Integer, Integer)
  Public Property Pge As PixelGameEngine

End Module

Public MustInherit Class PixelGameEngine

#Region "Win32"

#Region "Win32 - Const"

  Private Const VK_CAPITAL = &H14
  Private Const VK_NUMLOCK = &H90

  Private Const VK_OEM_1 = &HBA ' ;:
  Private Const VK_OEM_PLUS = &HBB ' -_
  Private Const VK_OEM_COMMA = &HBC ' ,<
  Private Const VK_OEM_MINUS = &HBD ' =+
  Private Const VK_OEM_PERIOD = 190 ' .>
  Private Const VK_OEM_2 = &HBF ' /?
  Private Const VK_OEM_3 = &HC0 ' `~
  Private Const VK_OEM_4 = &HDB ' [{
  Private Const VK_OEM_5 = &HDC ' \|
  Private Const VK_OEM_6 = &HDD ' ]}
  Private Const VK_OEM_7 = &HDE ' '"

  Private Const VK_F1 As Integer = &H70
  Private Const VK_F2 As Integer = &H71
  Private Const VK_F3 As Integer = &H72
  Private Const VK_F4 As Integer = &H73
  Private Const VK_F5 As Integer = &H74
  Private Const VK_F6 As Integer = &H75
  Private Const VK_F7 As Integer = &H76
  Private Const VK_F8 As Integer = &H77
  Private Const VK_F9 As Integer = &H78
  Private Const VK_F10 As Integer = &H79
  Private Const VK_F11 As Integer = &H7A
  Private Const VK_F12 As Integer = &H7B

  Private Const VK_DOWN As Integer = &H28
  Private Const VK_LEFT As Integer = &H25
  Private Const VK_RIGHT As Integer = &H27
  Private Const VK_UP As Integer = &H26
  Private Const VK_RETURN As Integer = &HD
  Private Const VK_BACK As Integer = &H8
  Private Const VK_ESCAPE As Integer = &H1B
  Private Const VK_PAUSE As Integer = &H13
  Private Const VK_SCROLL As Integer = &H91
  Private Const VK_TAB As Integer = &H9
  Private Const VK_DELETE As Integer = &H2E
  Private Const VK_HOME As Integer = &H24
  Private Const VK_END As Integer = &H23
  Private Const VK_PRIOR As Integer = &H21
  Private Const VK_NEXT As Integer = &H22
  Private Const VK_INSERT As Integer = &H2D
  Private Const VK_SHIFT As Integer = &H10
  Private Const VK_CONTROL As Integer = &H11
  Private Const VK_SPACE As Integer = &H20
  Private Const VK_MENU As Integer = &H12

  Private Const VK_NUMPAD0 As Integer = &H60
  Private Const VK_NUMPAD1 As Integer = &H61
  Private Const VK_NUMPAD2 As Integer = &H62
  Private Const VK_NUMPAD3 As Integer = &H63
  Private Const VK_NUMPAD4 As Integer = &H64
  Private Const VK_NUMPAD5 As Integer = &H65
  Private Const VK_NUMPAD6 As Integer = &H66
  Private Const VK_NUMPAD7 As Integer = &H67
  Private Const VK_NUMPAD8 As Integer = &H68
  Private Const VK_NUMPAD9 As Integer = &H69

  Private Const VK_MULTIPLY As Integer = &H6A
  Private Const VK_ADD As Integer = &H6B
  Private Const VK_DIVIDE As Integer = &H6F
  Private Const VK_SUBTRACT As Integer = &H6D
  Private Const VK_DECIMAL As Integer = &H6E

  'Private Const CS_USEDEFAULT As UInteger = &H80000000UI
  'Private Const CS_DBLCLKS As UInteger = 8
  Private Const CS_VREDRAW As UInteger = 1
  Private Const CS_HREDRAW As UInteger = 2
  Private Const CS_OWNDC As Integer = &H20

  'Private Const TME_LEAVE As Integer = &H2

  'Private Const COLOR_WINDOW As UInteger = 5
  Private Const COLOR_BACKGROUND As UInteger = 1

  Private Const IDI_APPLICATION As Integer = &H7F00

  'Private Const IDC_CROSS As UInteger = 32515
  Private Const IDC_ARROW As Integer = 32512

  Private Const WM_DESTROY As UInteger = 2
  'Private Const WM_PAINT As UInteger = &HF
  Private Const WM_LBUTTONUP As UInteger = &H202
  'Private Const WM_LBUTTONDBLCLK As UInteger = &H203
  Private Const WM_MOUSEMOVE As UInteger = &H200
  Private Const WM_CLOSE As UInteger = &H10
  Private Const WM_MBUTTONUP As Integer = &H208
  Private Const WM_MBUTTONDOWN As Integer = &H207
  Private Const WM_RBUTTONUP As Integer = &H205
  Private Const WM_RBUTTONDOWN As Integer = &H204
  Private Const WM_LBUTTONDOWN As Integer = &H201
  Private Const WM_KEYUP As Integer = &H101
  Private Const WM_KEYDOWN As Integer = &H100
  Private Const WM_SYSKEYDOWN As Integer = &H104
  Private Const WM_SYSKEYUP As Integer = &H105
  Private Const WM_KILLFOCUS As Integer = &H8
  Private Const WM_SETFOCUS As Integer = &H7
  Private Const WM_MOUSELEAVE As Integer = &H2A3
  Private Const WM_MOUSEWHEEL As Integer = &H20A
  Private Const WM_SIZE As Integer = &H5
  Private Const WM_CREATE As Integer = &H1
  Private Const WM_SYSCOMMAND = &H112
  'Private Const WM_GETICON As Integer = &H7F
  'Private Const WM_SETICON As Integer = &H80
  Private Const SC_KEYMENU = &HF100

  'Private Const SW_SHOWMAXIMIZED As Integer = 3
  'Private Const SW_SHOWNORMAL As Integer = 1

  Private Const WS_OVERLAPPEDWINDOW As UInteger = &HCF0000
  Private Const WS_VISIBLE As UInteger = &H10000000
  Private Const WS_EX_APPWINDOW As UInteger = &H40000UI
  Private Const WS_EX_WINDOWEDGE As UInteger = &H100
  Private Const WS_CAPTION As UInteger = &HC00000
  Private Const WS_SYSMENU As UInteger = &H80000
  Private Const WS_THICKFRAME As UInteger = &H40000
  Private Const WS_POPUP As UInteger = &H80000000UI
  Private Const WS_MINIMIZEBOX = &H20000UI
  Private Const WS_MAXIMIZEBOX = &H10000UI

  ' Window size and position flags
  'Private Const SWP_NOSIZE As Integer = &H1
  Private Const SWP_NOMOVE As Integer = &H2
  Private Const SWP_FRAMECHANGED As Integer = &H20
  Private Const SWP_NOZORDER As Integer = &H4
  Private Const SWP_NOOWNERZORDER As UInteger = &H200

  Private Const GWL_STYLE As Integer = -16

  'Private Const ICON_BIG As Integer = 1

  Private Const MONITOR_DEFAULTTONEAREST As Integer = &H2

  'Private Const SW_HIDE As Integer = 0

  Private Const PFD_DRAW_TO_WINDOW As Integer = &H4
  Private Const PFD_SUPPORT_OPENGL As Integer = &H20
  Private Const PFD_DOUBLEBUFFER As Integer = &H1
  Private Const PFD_TYPE_RGBA As Byte = 0
  Private Const PFD_MAIN_PLANE As Byte = 0

  Private Const GL_TEXTURE_2D As UInteger = &HDE1
  Private Const GL_TEXTURE_MAG_FILTER As UInteger = &H2800
  Private Const GL_TEXTURE_MIN_FILTER As UInteger = &H2801
  Private Const GL_NEAREST As UInteger = &H2600
  Private Const GL_TEXTURE_ENV As UInteger = &H2300
  Private Const GL_TEXTURE_ENV_MODE As UInteger = &H2200
  Private Const GL_DECAL As UInteger = &H2101
  Private Const GL_RGBA As UInteger = &H1908
  Private Const GL_UNSIGNED_BYTE As UInteger = &H1401
  'Private Const GL_INT As UInteger = &H1404
  Private Const GL_QUADS As UInteger = &H7
  Private Const GL_COLOR_BUFFER_BIT As UInteger = &H4000

#End Region

  Private Class Win32

    Delegate Function WndProc(hWnd As IntPtr, msg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr

#Region "Win32 - Structure"

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure WINDOWPLACEMENT
      Public Length As UInteger
      Public Flags As UInteger
      Public ShowCmd As UInteger
      Public MinPosition As Point
      Public MaxPosition As Point
      Public NormalPosition As RECT
      'Public Device As RECT ' only on Mac?
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Class CREATESTRUCT
      Public lpCreateParams As IntPtr
      Public hInstance As IntPtr
      Public hMenu As IntPtr
      Public hwndParent As IntPtr
      Public cy As Integer
      Public cx As Integer
      Public y As Integer
      Public x As Integer
      Public style As Integer
      Public lpszName As String
      Public lpszClass As String
      Public dwExStyle As UInteger
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure Point
      Public X As Integer
      Public Y As Integer
      Public Sub New(x As Integer, y As Integer)
        Me.X = x
        Me.Y = y
      End Sub
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure RECT
      Public Left As Integer
      Public Top As Integer
      Public Right As Integer
      Public Bottom As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure MONITORINFO
      Public cbSize As Integer
      Public rcMonitor As RECT
      Public rcWork As RECT
      Public dwFlags As UInt32
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure MSG
      Public hWnd As IntPtr
      Public message As UInteger
      Public wParam As IntPtr
      Public lParam As IntPtr
      Public time As Integer
      Public pt As Point
    End Structure

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Friend Structure WNDCLASS
      <MarshalAs(UnmanagedType.U4)> Public Style As Integer
      Public WndProc As IntPtr
      Public ClsExtra As Integer
      Public WndExtra As Integer
      Public hInstance As IntPtr
      Public hIcon As IntPtr
      Public hCursor As IntPtr
      Public hBackground As IntPtr
      Public MenuName As String
      Public ClassName As String
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure TRACKMOUSEEVENTSTRUCT
      <MarshalAs(UnmanagedType.U4)> Public cbSize As Integer
      <MarshalAs(UnmanagedType.U4)> Public dwFlags As Integer
      Public hWnd As IntPtr
      <MarshalAs(UnmanagedType.U4)> Public dwHoverTime As Integer
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Friend Structure PIXELFORMATDESCRIPTOR
      Public nSize As UShort
      Public nVersion As UShort
      Public dwFlags As UInteger
      Public iPixelType As Byte
      Public cColorBits As Byte
      Public cRedBits As Byte
      Public cRedShift As Byte
      Public cGreenBits As Byte
      Public cGreenShift As Byte
      Public cBlueBits As Byte
      Public cBlueShift As Byte
      Public cAlphaBits As Byte
      Public cAlphaShift As Byte
      Public cAccumBits As Byte
      Public cAccumRedBits As Byte
      Public cAccumGreenBits As Byte
      Public cAccumBlueBits As Byte
      Public cAccumAlphaBits As Byte
      Public cDepthBits As Byte
      Public cStencilBits As Byte
      Public cAuxBuffers As Byte
      Public iLayerType As Byte
      Public bReserved As Byte
      Public dwLayerMask As UInteger
      Public dwVisibleMask As UInteger
      Public dwDamageMask As UInteger
    End Structure

#End Region

#Region "Win32 - P/Invoke"

    Friend Declare Function wglCreateContext Lib "opengl32" (hdc As IntPtr) As IntPtr
#Disable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function wglGetProcAddress Lib "opengl32" Alias "wglGetProcAddress" (<MarshalAs(UnmanagedType.LPStr)> lpProcName As String) As IntPtr
#Enable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function wglMakeCurrent Lib "opengl32" (hdc As IntPtr, hglrc As IntPtr) As Integer
    Friend Declare Sub glViewport Lib "opengl32" (x As Integer, y As Integer, width As Integer, height As Integer)
    Friend Declare Sub glEnable Lib "opengl32.dll" (cap As UInteger)
    Friend Declare Sub glGenTextures Lib "opengl32.dll" (n As Integer, ByRef textures As UInteger)
    Friend Declare Sub glBindTexture Lib "opengl32.dll" (target As UInteger, texture As UInteger)
    Friend Declare Sub glTexParameteri Lib "opengl32.dll" (target As UInteger, pname As UInteger, param As Integer)
    Friend Declare Sub glTexEnvf Lib "opengl32.dll" (target As UInteger, pname As UInteger, param As Single)
    Friend Declare Sub glTexImage2D Lib "opengl32.dll" (target As UInteger, level As Integer, internalformat As Integer, width As Integer, height As Integer, border As Integer, format As UInteger, type As UInteger, data As IntPtr)
    Friend Declare Sub glTexSubImage2D Lib "opengl32.dll" (target As UInteger, level As Integer, xoffset As Integer, yoffset As Integer, width As Integer, height As Integer, format As UInteger, type As UInteger, pixels As IntPtr)
    Friend Declare Sub glBegin Lib "opengl32.dll" (mode As UInteger)
    Friend Declare Sub glTexCoord2f Lib "opengl32.dll" (s As Single, t As Single)
    Friend Declare Sub glEnd Lib "opengl32.dll" ()
    Friend Declare Sub glVertex3f Lib "opengl32.dll" (x As Single, y As Single, z As Single)
    Friend Declare Function wglDeleteContext Lib "opengl32.dll" (hglrc As IntPtr) As Boolean
    Friend Declare Sub glClear Lib "opengl32.dll" (mask As UInteger)

    Friend Declare Function AdjustWindowRectEx Lib "user32.dll" (ByRef lpRect As RECT, dwStyle As UInteger, bMenu As Boolean, dwExStyle As UInteger) As Boolean
    Friend Declare Function CreateWindowEx Lib "user32.dll" Alias "CreateWindowExW" (exStyle As UInteger,
                                                                                     atom As UShort,
                                                                                     <MarshalAs(UnmanagedType.LPWStr)> windowName As String,
                                                                                     style As UInteger,
                                                                                     x As Integer,
                                                                                     y As Integer,
                                                                                     width As Integer,
                                                                                     height As Integer,
                                                                                     wndParent As IntPtr,
                                                                                     menu As IntPtr,
                                                                                     hInstance As IntPtr,
                                                                                     lpParam As IntPtr) As IntPtr
    Friend Declare Function DefWindowProc Lib "user32.dll" Alias "DefWindowProcA" (hWnd As IntPtr, msg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr
    Friend Declare Function DestroyWindow Lib "user32.dll" (hWnd As IntPtr) As Boolean
    Friend Declare Function DispatchMessage Lib "user32.dll" Alias "DispatchMessageA" (ByRef lpMsg As MSG) As Integer
    Friend Declare Function FindWindow Lib "user32.dll" (<MarshalAs(UnmanagedType.LPWStr)> lpClassName As String,
                                                         <MarshalAs(UnmanagedType.LPWStr)> lpWindowName As String) As Boolean
    Friend Declare Function GetDC Lib "user32" (hWnd As IntPtr) As IntPtr
    Friend Declare Function GetDesktopWindow Lib "user32.dll" () As IntPtr
    Friend Declare Function GetKeyState Lib "user32.dll" (virtKey As Integer) As Short
    Friend Declare Function GetMessage Lib "user32.dll" Alias "GetMessageA" (ByRef lpMsg As MSG, hWnd As IntPtr, wMsgFilterMin As UInteger, wMsgFilterMax As UInteger) As Integer
    Friend Declare Function GetMonitorInfo Lib "user32.dll" Alias "GetMonitorInfoA" (hMonitor As IntPtr, ByRef lpmi As MONITORINFO) As Boolean
    Friend Declare Function GetWindowLong Lib "user32.dll" Alias "GetWindowLongA" (hWnd As IntPtr, index As Integer) As Long
    Friend Declare Function GetWindowPlacement Lib "user32.dll" (hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean
    Friend Declare Function LoadCursor Lib "user32.dll" Alias "LoadCursorA" (hInstance As IntPtr, cursorName As Integer) As IntPtr
    Friend Declare Function LoadIcon Lib "user32.dll" Alias "LoadIconA" (hInstance As IntPtr, lpIconName As Integer) As IntPtr
    Friend Declare Function MonitorFromWindow Lib "user32.dll" (hwnd As IntPtr, dwFlags As UInteger) As IntPtr
    Friend Declare Sub PostQuitMessage Lib "user32.dll" (exitCode As Integer)
    Friend Declare Function PostMessage Lib "user32.dll" Alias "PostMessageA" (hwnd As IntPtr, wMsg As UInteger, wParam As IntPtr, lParam As IntPtr) As Boolean
    Friend Declare Function RegisterClass Lib "user32.dll" Alias "RegisterClassA" (ByRef lpWndClass As WNDCLASS) As UShort
    Friend Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (hWnd As IntPtr, msg As UInteger, wParam As IntPtr, lParam As IntPtr) As IntPtr
    Friend Declare Function SetWindowLong Lib "user32.dll" Alias "SetWindowLongA" (hWnd As IntPtr, index As Integer, newLong As Long) As Long
    Friend Declare Function SetWindowPlacement Lib "user32.dll" (hWnd As IntPtr, ByRef lpwndpl As WINDOWPLACEMENT) As Boolean
    Friend Declare Function SetWindowPos Lib "user32.dll" (hWnd As IntPtr, hWndInsertAfter As IntPtr, x As Integer, y As Integer, cx As Integer, cy As Integer, flags As UInteger) As Boolean
    Friend Declare Function SetWindowText Lib "user32.dll" Alias "SetWindowTextW" (hwnd As IntPtr, <MarshalAs(UnmanagedType.LPWStr)> lpString As String) As Boolean
    Friend Declare Function ShowWindow Lib "user32.dll" (hWnd As IntPtr, cmdShow As Integer) As <MarshalAs(UnmanagedType.Bool)> Boolean
    Friend Declare Function TrackMouseEvent Lib "user32.dll" (ByRef tme As TRACKMOUSEEVENTSTRUCT) As Boolean
    Friend Declare Function TranslateMessage Lib "user32.dll" (ByRef lpMsg As MSG) As Boolean
    Friend Declare Function UpdateWindow Lib "user32.dll" (hWnd As IntPtr) As Boolean

    Friend Declare Function FreeConsole Lib "kernel32.dll" () As Boolean
    Friend Declare Function GetConsoleWindow Lib "kernel32.dll" () As IntPtr
    Friend Declare Function GetLastError Lib "kernel32.dll" () As UInteger
    Friend Declare Function GetModuleHandle Lib "kernel32.dll" Alias "GetModuleHandleA" (lpModuleName As IntPtr) As IntPtr

    Friend Declare Function ChoosePixelFormat Lib "gdi32" (hdc As IntPtr, ByRef pfd As PIXELFORMATDESCRIPTOR) As Integer
    Friend Declare Function SetPixelFormat Lib "gdi32" (hdc As IntPtr, iPixelFormat As Integer, ByRef pfd As PIXELFORMATDESCRIPTOR) As Integer
    Friend Declare Function SwapBuffers Lib "gdi32.dll" (hdc As IntPtr) As Boolean

#End Region

  End Class

  ' Windows
  Private ReadOnly m_delegWndProc As Win32.WndProc = AddressOf Pge_WindowEvent
  Private Delegate Function wglSwapInterval_t(interval As Integer) As Integer
  Private wglSwapInterval As wglSwapInterval_t

#End Region

#Region "Linux"

#Region "Linux - Const"

  Private Const XK_F1 As Integer = &HFFBE
  Private Const XK_F2 As Integer = &HFFBF
  Private Const XK_F3 As Integer = &HFFC0
  Private Const XK_F4 As Integer = &HFFC1
  Private Const XK_F5 As Integer = &HFFC2
  Private Const XK_F6 As Integer = &HFFC3
  Private Const XK_F7 As Integer = &HFFC4
  Private Const XK_F8 As Integer = &HFFC5
  Private Const XK_F9 As Integer = &HFFC6
  Private Const XK_F10 As Integer = &HFFC7
  Private Const XK_F11 As Integer = &HFFC8
  Private Const XK_F12 As Integer = &HFFC9

  Private Const XK_Down As Integer = &HFF54
  Private Const XK_Left As Integer = &HFF51
  Private Const XK_Right As Integer = &HFF53
  Private Const XK_Up As Integer = &HFF52
  Private Const XK_KP_Enter As Integer = &HFF8D
  Private Const XK_Return As Integer = &HFF0D

  Private Const XK_BackSpace As Integer = &HFF08
  Private Const XK_Escape As Integer = &HFF1B
  Private Const XK_Linefeed As Integer = &HFF0A
  Private Const XK_Pause As Integer = &HFF13
  Private Const XK_Scroll_Lock As Integer = &HFF14
  Private Const XK_Tab As Integer = &HFF09
  Private Const XK_Delete As Integer = &HFF9F
  Private Const XK_Home As Integer = &HFF50
  Private Const XK_End As Integer = &HFF57
  Private Const XK_Page_Up As Integer = &HFF55
  Private Const XK_Page_Down As Integer = &HFF56
  Private Const XK_Insert As Integer = &HFF63
  Private Const XK_Shift_L As Integer = &HFFE1
  Private Const XK_Shift_R As Integer = &HFFE2
  Private Const XK_Control_L As Integer = &HFFE3
  Private Const XK_Control_R As Integer = &HFFE4
  Private Const XK_Alt_L As Integer = &HFFE9
  Private Const XK_Alt_R As Integer = &HFFEA
  Private Const XK_space As Integer = &H20

  Private Const XK_0 As Integer = &H30
  Private Const XK_1 As Integer = &H31
  Private Const XK_2 As Integer = &H32
  Private Const XK_3 As Integer = &H33
  Private Const XK_4 As Integer = &H34
  Private Const XK_5 As Integer = &H35
  Private Const XK_6 As Integer = &H36
  Private Const XK_7 As Integer = &H37
  Private Const XK_8 As Integer = &H38
  Private Const XK_9 As Integer = &H39

  Private Const XK_KP_0 As Integer = &HFFB0
  Private Const XK_KP_1 As Integer = &HFFB1
  Private Const XK_KP_2 As Integer = &HFFB2
  Private Const XK_KP_3 As Integer = &HFFB3
  Private Const XK_KP_4 As Integer = &HFFB4
  Private Const XK_KP_5 As Integer = &HFFB5
  Private Const XK_KP_6 As Integer = &HFFB6
  Private Const XK_KP_7 As Integer = &HFFB7
  Private Const XK_KP_8 As Integer = &HFFB8
  Private Const XK_KP_9 As Integer = &HFFB9

  Private Const XK_KP_Multiply As Integer = &HFFAA '&H1008FFAA
  Private Const XK_KP_Add As Integer = &HFFAB '&H1008FFAB
  Private Const XK_KP_Divide As Integer = &HFFAF '&H1008FFAF
  Private Const XK_KP_Subtract As Integer = &HFFAD '&H1008FFAD
  Private Const XK_KP_Decimal As Integer = &HFFAE '&H1008FFAE

  Private Const GLX_RGBA As Integer = 4
  Private Const GLX_DEPTH_SIZE As Integer = 12
  Private Const GLX_DOUBLEBUFFER As Integer = 5

  Private Const AllocNone As Integer = 0
  Private Const None As Integer = 0
  'Private Const ExposureMask As Integer = &H8000
  'Private Const KeyPressMask As Integer = &H1
  'Private Const KeyReleaseMask As Integer = &H2
  'Private Const ButtonPressMask As Integer = &H4
  'Private Const ButtonReleaseMask As Integer = &H8
  'Private Const PointerMotionMask As Integer = &H200
  'Private Const FocusChangeMask As Integer = &H20000
  'Private Const StructureNotifyMask As Integer = &H20000
  Private Const InputOutput As Integer = 1

  'Private Const WEColormap As UInteger = &H2

  'Private Const CWEventMask As UInteger = &H80
  'Private Const CWColormap As UInteger = &H4

  'Private Const XInternalAtom As UInteger = &H400000
  'Private Const XMapWindowConst As Integer = 18

  'Private Const ClientMessage As Integer = 3

  Private Const GL_TRUE As Integer = 1

#End Region

  Private Class X11

    Private Sub New()
    End Sub

#Region "Structure"

    Public Enum XKeySym

      NoSymbol = 0

      ' * TTY function keys, cleverly chosen to map to ASCII, for convenience of
      ' * programming, but could have been arbitrary (at the cost of lookup
      ' * tables in client code).

      ''' <summary>
      ''' Back space, back char 
      ''' </summary>
      XK_BackSpace = &HFF08
      XK_Tab = &HFF09
      ''' <summary>
      ''' Linefeed, LF
      ''' </summary>
      XK_Linefeed = &HFF0A
      XK_Clear = &HFF0B
      ''' <summary>
      ''' Return, enter
      ''' </summary>
      XK_Return = &HFF0D
      ''' <summary>
      ''' Pause, hold 
      ''' </summary>
      XK_Pause = &HFF13
      XK_Scroll_Lock = &HFF14
      XK_Sys_Req = &HFF15
      XK_Escape = &HFF1B
      ''' <summary>
      ''' Delete, rubout
      ''' </summary>
      XK_Delete = &HFFFF

      ' * International & multi-key character composition

      ' * Japanese keyboard support

      ' * 0xff31 thru 0xff3f are under XK_KOREAN

      ' * Cursor control & motion

      XK_Home = &HFF50
      ''' <summary>
      ''' Move left, left arrow
      ''' </summary>
      XK_Left = &HFF51
      ''' <summary>
      ''' Move up, up arrow
      ''' </summary>
      XK_Up = &HFF52
      ''' <summary>
      ''' Move right, right arrow
      ''' </summary>
      XK_Right = &HFF53
      ''' <summary>
      ''' Move down, down arrow
      ''' </summary>
      XK_Down = &HFF54
      ''' <summary>
      ''' Prior, previous
      ''' </summary>
      XK_Page_Up = &HFF55
      'XK_Prior = &HFF55
      ''' <summary>
      ''' Next
      ''' </summary>
      XK_Page_Down = &HFF56
      'XK_Next = &HFF56
      ''' <summary>
      ''' EOL
      ''' </summary>
      XK_End = &HFF57
      ''' <summary>
      ''' BOL
      ''' </summary>
      XK_Begin = &HFF58

      ' * Misc functions

      ' * Keypad functions, keypad numbers cleverly chosen to map to ASCII

      ''' <summary>
      ''' Space
      ''' </summary>
      XK_KP_Space = &HFF80
      XK_KP_Tab = &HFF89
      ''' <summary>
      ''' Enter
      ''' </summary>
      XK_KP_Enter = &HFF8D
      ''' <summary>
      ''' PF1, KP_A, ...
      ''' </summary>
      XK_KP_F1 = &HFF91
      XK_KP_F2 = &HFF92
      XK_KP_F3 = &HFF93
      XK_KP_F4 = &HFF94
      XK_KP_Home = &HFF95
      XK_KP_Left = &HFF96
      XK_KP_Up = &HFF97
      XK_KP_Right = &HFF98
      XK_KP_Down = &HFF99
      'XK_KP_Prior = &HFF9A
      XK_KP_Page_Up = &HFF9A
      'XK_KP_Next = &HFF9B
      XK_KP_Page_Down = &HFF9B
      XK_KP_End = &HFF9C
      XK_KP_Begin = &HFF9D
      XK_KP_Insert = &HFF9E
      XK_KP_Delete = &HFF9F
      ''' <summary>
      ''' Equals
      ''' </summary>
      XK_KP_Equal = &HFFBD
      XK_KP_Multiply = &HFFAA
      XK_KP_Add = &HFFAB
      ''' <summary>
      ''' Separator, often comma
      ''' </summary>
      XK_KP_Separator = &HFFAC
      XK_KP_Subtract = &HFFAD
      XK_KP_Decimal = &HFFAE
      XK_KP_Divide = &HFFAF

      XK_KP_0 = &HFFB0
      XK_KP_1 = &HFFB1
      XK_KP_2 = &HFFB2
      XK_KP_3 = &HFFB3
      XK_KP_4 = &HFFB4
      XK_KP_5 = &HFFB5
      XK_KP_6 = &HFFB6
      XK_KP_7 = &HFFB7
      XK_KP_8 = &HFFB8
      XK_KP_9 = &HFFB9

      ' * Auxiliary functions; note the duplicate definitions for left and right
      ' * function keys;  Sun keyboards And a few other manufacturers have such
      ' * function key groups on the left And/Or right sides of the keyboard.
      ' * We've not found a keyboard with more than 35 function keys total.

      XK_F1 = &HFFBE
      XK_F2 = &HFFBF
      XK_F3 = &HFFC0
      XK_F4 = &HFFC1
      XK_F5 = &HFFC2
      XK_F6 = &HFFC3
      XK_F7 = &HFFC4
      XK_F8 = &HFFC5
      XK_F9 = &HFFC6
      XK_F10 = &HFFC7
      XK_F11 = &HFFC8
      'XK_L1 = &HFFC8
      XK_F12 = &HFFC9
      'XK_L2 = &HFFC9
      XK_F13 = &HFFCA
      'XK_L3 = &HFFCA
      XK_F14 = &HFFCB
      'XK_L4 = &HFFCB
      XK_F15 = &HFFCC
      'XK_L5 = &HFFCC
      XK_F16 = &HFFCD
      'XK_L6 = &HFFCD
      XK_F17 = &HFFCE
      'XK_L7 = &HFFCE
      XK_F18 = &HFFCF
      'XK_L8 = &HFFCF
      XK_F19 = &HFFD0
      'XK_L9 = &HFFD0
      XK_F20 = &HFFD1
      'XK_L10 = &HFFD1
      XK_F21 = &HFFD2
      'XK_R1 = &HFFD2
      XK_F22 = &HFFD3
      'XK_R2 = &HFFD3
      XK_F23 = &HFFD4
      'XK_R3 = &HFFD4
      XK_F24 = &HFFD5
      'XK_R4 = &HFFD5
      XK_F25 = &HFFD6
      'XK_R5 = &HFFD6
      XK_F26 = &HFFD7
      'XK_R6 = &HFFD7
      XK_F27 = &HFFD8
      'XK_R7 = &HFFD8
      XK_F28 = &HFFD9
      'XK_R8 = &HFFD9
      XK_F29 = &HFFDA
      'XK_R9 = &HFFDA
      XK_F30 = &HFFDB
      'XK_R10 = &HFFDB
      XK_F31 = &HFFDC
      'XK_R11 = &HFFDC
      XK_F32 = &HFFDD
      'XK_R12 = &HFFDD
      XK_F33 = &HFFDE
      'XK_R13 = &HFFDE
      XK_F34 = &HFFDF
      'XK_R14 = &HFFDF
      XK_F35 = &HFFE0
      'XK_R15 = &HFFE0

      ' * Modifiers

      ''' <summary>
      ''' Left shift
      ''' </summary>
      XK_Shift_L = &HFFE1
      ''' <summary>
      ''' Right shift
      ''' </summary>
      XK_Shift_R = &HFFE2
      ''' <summary>
      ''' Left control
      ''' </summary>
      XK_Control_L = &HFFE3
      ''' <summary>
      ''' Right control
      ''' </summary>
      XK_Control_R = &HFFE4
      ''' <summary>
      ''' Caps lock
      ''' </summary>
      XK_Caps_Lock = &HFFE5
      ''' <summary>
      ''' Shift lock
      ''' </summary>
      XK_Shift_Lock = &HFFE6
      ''' <summary>
      ''' Left meta
      ''' </summary>
      XK_Meta_L = &HFFE7
      ''' <summary>
      ''' Right meta
      ''' </summary>
      XK_Meta_R = &HFFE8
      ''' <summary>
      ''' Left alt
      ''' </summary>
      XK_Alt_L = &HFFE9
      ''' <summary>
      ''' Right alt
      ''' </summary>
      XK_Alt_R = &HFFEA
      ''' <summary>
      ''' Left super
      ''' </summary>
      XK_Super_L = &HFFEB
      ''' <summary>
      ''' Right super
      ''' </summary>
      XK_Super_R = &HFFEC
      ''' <summary>
      ''' Left hyper
      ''' </summary>
      XK_Hyper_L = &HFFED
      ''' <summary>
      ''' Right hyper
      ''' </summary>
      XK_Hyper_R = &HFFEE

      ' * Latin 1
      ' * (ISO/IEC 8859-1 = Unicode U+0020..U+00FF)
      ' * Byte 3 = 0

      ''' <summary>
      ''' U+0020 SPACE
      ''' </summary>
      XK_space = &H20
      ''' <summary>
      ''' U+0021 EXCLAMATION MARK 
      ''' </summary>
      XK_exclam = &H21
      ''' <summary>
      ''' U+0022 QUOTATION MARK
      ''' </summary>
      XK_quotedbl = &H22
      ''' <summary>
      ''' U+0023 NUMBER SIGN
      ''' </summary>
      XK_numbersign = &H23
      ''' <summary>
      ''' U+0024 DOLLAR SIGN
      ''' </summary>
      XK_dollar = &H24
      ''' <summary>
      ''' U+0025 PERCENT SIGN
      ''' </summary>
      XK_percent = &H25
      ''' <summary>
      ''' U+0026 AMPERSAND
      ''' </summary>
      XK_ampersand = &H26
      ''' <summary>
      ''' U+0027 APOSTROPHE
      ''' </summary>
      XK_apostrophe = &H27
      '''' <summary>
      '''' deprecated
      '''' </summary>
      'XK_quoteright = &H27
      ''' <summary>
      ''' U+0028 LEFT PARENTHESIS
      ''' </summary>
      XK_parenleft = &H28
      ''' <summary>
      ''' U+0029 RIGHT PARENTHESIS
      ''' </summary>
      XK_parenright = &H29
      ''' <summary>
      ''' U+002A ASTERISK
      ''' </summary>
      XK_asterisk = &H2A
      ''' <summary>
      ''' U+002B PLUS SIGN
      ''' </summary>
      XK_plus = &H2B
      ''' <summary>
      ''' U+002C COMMA
      ''' </summary>
      XK_comma = &H2C
      ''' <summary>
      ''' U+002D HYPHEN-MINUS
      ''' </summary>
      XK_minus = &H2D
      ''' <summary>
      ''' U+002E FULL STOP
      ''' </summary>
      XK_period = &H2E
      ''' <summary>
      ''' U+002F SOLIDUS
      ''' </summary>
      XK_slash = &H2F
      ''' <summary>
      ''' U+0030 DIGIT ZERO
      ''' </summary>
      XK_0 = &H30
      ''' <summary>
      ''' U+0031 DIGIT ONE
      ''' </summary>
      XK_1 = &H31
      ''' <summary>
      ''' U+0032 DIGIT TWO
      ''' </summary>
      XK_2 = &H32
      ''' <summary>
      ''' U+0033 DIGIT THREE
      ''' </summary>
      XK_3 = &H33
      ''' <summary>
      ''' U+0034 DIGIT FOUR
      ''' </summary>
      XK_4 = &H34
      ''' <summary>
      ''' U+0035 DIGIT FIVE
      ''' </summary>
      XK_5 = &H35
      ''' <summary>
      ''' U+0036 DIGIT SIX
      ''' </summary>
      XK_6 = &H36
      ''' <summary>
      ''' U+0037 DIGIT SEVEN
      ''' </summary>
      XK_7 = &H37
      ''' <summary>
      ''' U+0038 DIGIT EIGHT
      ''' </summary>
      XK_8 = &H38
      ''' <summary>
      ''' U+0039 DIGIT NINE
      ''' </summary>
      XK_9 = &H39
      ''' <summary>
      ''' U+003A COLON
      ''' </summary>
      XK_colon = &H3A
      ''' <summary>
      ''' U+003B SEMICOLON
      ''' </summary>
      XK_semicolon = &H3B
      ''' <summary>
      ''' U+003C LESS-THAN SIGN
      ''' </summary>
      XK_less = &H3C
      ''' <summary>
      ''' U+003D EQUALS SIGN
      ''' </summary>
      XK_equal = &H3D
      ''' <summary>
      ''' U+003E GREATER-THAN SIGN
      ''' </summary>
      XK_greater = &H3E
      ''' <summary>
      ''' U+003F QUESTION MARK
      ''' </summary>
      XK_question = &H3F
      ''' <summary>
      ''' U+0040 COMMERCIAL AT
      ''' </summary>
      XK_at = &H40
      ''' <summary>
      ''' U+0041 LATIN CAPITAL LETTER A
      ''' </summary>
      XK_c_A = &H41
      ''' <summary>
      ''' U+0042 LATIN CAPITAL LETTER B
      ''' </summary>
      XK_c_B = &H42
      ''' <summary>
      ''' U+0043 LATIN CAPITAL LETTER C
      ''' </summary>
      XK_c_C = &H43
      ''' <summary>
      ''' U+0044 LATIN CAPITAL LETTER D
      ''' </summary>
      XK_c_D = &H44
      ''' <summary>
      ''' U+0045 LATIN CAPITAL LETTER E
      ''' </summary>
      XK_c_E = &H45
      ''' <summary>
      ''' U+0046 LATIN CAPITAL LETTER F
      ''' </summary>
      XK_c_F = &H46
      ''' <summary>
      ''' U+0047 LATIN CAPITAL LETTER G
      ''' </summary>
      XK_c_G = &H47
      ''' <summary>
      ''' U+0048 LATIN CAPITAL LETTER H
      ''' </summary>
      XK_c_H = &H48
      ''' <summary>
      ''' U+0049 LATIN CAPITAL LETTER I
      ''' </summary>
      XK_c_I = &H49
      ''' <summary>
      ''' U+004A LATIN CAPITAL LETTER J
      ''' </summary>
      XK_c_J = &H4A
      ''' <summary>
      ''' U+004B LATIN CAPITAL LETTER K
      ''' </summary>
      XK_c_K = &H4B
      ''' <summary>
      ''' U+004C LATIN CAPITAL LETTER L
      ''' </summary>
      XK_c_L = &H4C
      ''' <summary>
      ''' U+004D LATIN CAPITAL LETTER M
      ''' </summary>
      XK_c_M = &H4D
      ''' <summary>
      ''' U+004E LATIN CAPITAL LETTER N
      ''' </summary>
      XK_c_N = &H4E
      ''' <summary>
      ''' U+004F LATIN CAPITAL LETTER O
      ''' </summary>
      XK_c_O = &H4F
      ''' <summary>
      ''' U+0050 LATIN CAPITAL LETTER P
      ''' </summary>
      XK_c_P = &H50
      ''' <summary>
      ''' U+0051 LATIN CAPITAL LETTER Q
      ''' </summary>
      XK_c_Q = &H51
      ''' <summary>
      ''' U+0052 LATIN CAPITAL LETTER R
      ''' </summary>
      XK_c_R = &H52
      ''' <summary>
      ''' U+0053 LATIN CAPITAL LETTER S
      ''' </summary>
      XK_c_S = &H53
      ''' <summary>
      ''' U+0054 LATIN CAPITAL LETTER T
      ''' </summary>
      XK_c_T = &H54
      ''' <summary>
      ''' U+0055 LATIN CAPITAL LETTER U
      ''' </summary>
      XK_c_U = &H55
      ''' <summary>
      ''' U+0056 LATIN CAPITAL LETTER V
      ''' </summary>
      XK_c_V = &H56
      ''' <summary>
      ''' U+0057 LATIN CAPITAL LETTER W
      ''' </summary>
      XK_c_W = &H57
      ''' <summary>
      ''' U+0058 LATIN CAPITAL LETTER X
      ''' </summary>
      XK_c_X = &H58
      ''' <summary>
      ''' U+0059 LATIN CAPITAL LETTER Y
      ''' </summary>
      XK_c_Y = &H59
      ''' <summary>
      ''' U+005A LATIN CAPITAL LETTER Z
      ''' </summary>
      XK_c_Z = &H5A
      ''' <summary>
      ''' U+005B LEFT SQUARE BRACKET
      ''' </summary>
      XK_bracketleft = &H5B
      ''' <summary>
      ''' U+005C REVERSE SOLIDUS
      ''' </summary>
      XK_backslash = &H5C
      ''' <summary>
      ''' U+005D RIGHT SQUARE BRACKET
      ''' </summary>
      XK_bracketright = &H5D
      ''' <summary>
      ''' U+005E CIRCUMFLEX ACCENT
      ''' </summary>
      XK_asciicircum = &H5E
      ''' <summary>
      ''' U+005F LOW LINE
      ''' </summary>
      XK_underscore = &H5F
      ''' <summary>
      ''' U+0060 GRAVE ACCENT
      ''' </summary>
      XK_grave = &H60
      '''' <summary>
      '''' deprecated
      '''' </summary>
      'XK_quoteleft = &H60
      ''' <summary>
      ''' U+0061 LATIN SMALL LETTER A
      ''' </summary>
      XK_a = &H61
      ''' <summary>
      ''' U+0062 LATIN SMALL LETTER B
      ''' </summary>
      XK_b = &H62
      ''' <summary>
      ''' U+0063 LATIN SMALL LETTER C
      ''' </summary>
      XK_c = &H63
      ''' <summary>
      ''' U+0064 LATIN SMALL LETTER D
      ''' </summary>
      XK_d = &H64
      ''' <summary>
      ''' U+0065 LATIN SMALL LETTER E
      ''' </summary>
      XK_e = &H65
      ''' <summary>
      ''' U+0066 LATIN SMALL LETTER F
      ''' </summary>
      XK_f = &H66
      ''' <summary>
      ''' U+0067 LATIN SMALL LETTER G
      ''' </summary>
      XK_g = &H67
      ''' <summary>
      ''' U+0068 LATIN SMALL LETTER H
      ''' </summary>
      XK_h = &H68
      ''' <summary>
      ''' U+0069 LATIN SMALL LETTER I
      ''' </summary>
      XK_i = &H69
      ''' <summary>
      ''' U+006A LATIN SMALL LETTER J
      ''' </summary>
      XK_j = &H6A
      ''' <summary>
      ''' U+006B LATIN SMALL LETTER K
      ''' </summary>
      XK_k = &H6B
      ''' <summary>
      ''' U+006C LATIN SMALL LETTER L
      ''' </summary>
      XK_l = &H6C
      ''' <summary>
      ''' U+006D LATIN SMALL LETTER M
      ''' </summary>
      XK_m = &H6D
      ''' <summary>
      ''' U+006E LATIN SMALL LETTER N
      ''' </summary>
      XK_n = &H6E
      ''' <summary>
      ''' U+006F LATIN SMALL LETTER O
      ''' </summary>
      XK_o = &H6F
      ''' <summary>
      ''' U+0070 LATIN SMALL LETTER P
      ''' </summary>
      XK_p = &H70
      ''' <summary>
      ''' U+0071 LATIN SMALL LETTER Q
      ''' </summary>
      XK_q = &H71
      ''' <summary>
      ''' U+0072 LATIN SMALL LETTER R
      ''' </summary>
      XK_r = &H72
      ''' <summary>
      ''' U+0073 LATIN SMALL LETTER S
      ''' </summary>
      XK_s = &H73
      ''' <summary>
      ''' U+0074 LATIN SMALL LETTER T
      ''' </summary>
      XK_t = &H74
      ''' <summary>
      ''' U+0075 LATIN SMALL LETTER U
      ''' </summary>
      XK_u = &H75
      ''' <summary>
      ''' U+0076 LATIN SMALL LETTER V
      ''' </summary>
      XK_v = &H76
      ''' <summary>
      ''' U+0077 LATIN SMALL LETTER W
      ''' </summary>
      XK_w = &H77
      ''' <summary>
      ''' U+0078 LATIN SMALL LETTER X
      ''' </summary>
      XK_x = &H78
      ''' <summary>
      ''' U+0079 LATIN SMALL LETTER Y
      ''' </summary>
      XK_y = &H79
      ''' <summary>
      ''' U+007A LATIN SMALL LETTER Z
      ''' </summary>
      XK_z = &H7A
      ''' <summary>
      ''' U+007B LEFT CURLY BRACKET
      ''' </summary>
      XK_braceleft = &H7B
      ''' <summary>
      ''' U+007C VERTICAL LINE
      ''' </summary>
      XK_bar = &H7C
      ''' <summary>
      ''' U+007D RIGHT CURLY BRACKET
      ''' </summary>
      XK_braceright = &H7D
      ''' <summary>
      ''' U+007E TILDE
      ''' </summary>
      XK_asciitilde = &H7E
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure XMotionEvent
      Public Type As XEventType
      Public Serial As Long
      Public Send_Event As Boolean
      Public Display As IntPtr
      Public Window As IntPtr
      Public Root As IntPtr
      Public Subwindow As IntPtr
      Public Time As Long
      Public X, Y As Integer
      Public X_Root, Y_Root As Integer
      Public State As Integer
      Public Is_Hint As Char
      Public Same_Screen As Boolean
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure XButtonEvent
      Public Type As XEventType
      Public Serial As Long
      Public Send_Event As Boolean
      Public Display As IntPtr
      Public Window As IntPtr
      Public Root As IntPtr
      Public Subwindow As IntPtr
      Public Time As Long
      Public X, Y As Integer
      Public X_Root, Y_Root As Integer
      Public State As Integer
      Public Button As Integer
      Public Same_Screen As Boolean
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure XClientMessageEvent
      Public Type As XEventType
      Public Serial As Long
      Public Send_Event As Boolean
      Public Display As IntPtr
      Public Window As IntPtr
      Public Message_Type As ULong 'XAtom
      Public Format As Integer
      'Public Data As XClientMessageEventData
      Public L0 As Long
      Public L1 As Long
      Public L2 As Long
      Public L3 As Long
      Public L4 As Long
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure XConfigureEvent
      Public Type As XEventType
      Public Serial As Long
      Public Send_Event As Boolean
      Public Display As IntPtr
      Public _Event As Integer
      Public Window As IntPtr
      Public X, Y As Integer
      Public Width, Height As Integer
      Public Border_Width As Integer
      Public Above As Integer
      Public Override_Redirect As Integer
    End Structure

    ''' <summary>
    ''' Information used by the visual utility routines to find desired visual
    ''' type from the many visuals a display may support.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure XVisualInfo
      Public Visual As IntPtr ' Visual*
      Public VisualId As ULong ' VisualID: unsigned long OR CARD32, BITS32 
      Public Screen As Integer ' int
      Public Depth As Integer ' int
      Public [Class] As Integer ' int
      Public RedMask As ULong ' unsigned long
      Public GreenMask As ULong ' unsigned long
      Public BlueMask As ULong ' unsigned long
      Public ColormapSize As Integer ' int
      Public BitsPerRgb As Integer ' int
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure XKeyEvent
      Public Type As XEventType
      Public Serial As Long
      Public Send_Event As Boolean
      Public Display As IntPtr
      Public Window As IntPtr
      Public Root As IntPtr
      Public Subwindow As IntPtr
      Public Time As Long
      Public X, Y As Integer
      Public X_Root, Y_Root As Integer
      Public State As Integer
      Public Keycode As Integer
      Public Same_Screen As Boolean
    End Structure

    ''' <summary>
    ''' Data structure for setting window attributes.
    ''' </summary>
    <StructLayout(LayoutKind.Sequential)>
    Public Structure XSetWindowAttributes
      ''' <summary>
      ''' background or None or ParentRelative
      ''' </summary>
      Public Background_Pixmap As IntPtr ' Pixmap
      ''' <summary>
      ''' background pixel
      ''' </summary>
      Public Background_Pixel As Long ' unsigned long
      ''' <summary>
      ''' order of the window
      ''' </summary>
      Public Border_Pixmap As IntPtr ' Pixmap
      ''' <summary>
      ''' border pixel value
      ''' </summary>
      Public BorderPixel As Long ' unsigned long
      ''' <summary>
      ''' one of bit gravity values
      ''' </summary>
      Public Bit_Gravity As Integer ' int
      ''' <summary>
      ''' one of the window gravity values
      ''' </summary>
      Public Win_Gravity As Integer ' int
      ''' <summary>
      ''' NotUseful, WhenMapped, Always
      ''' </summary>
      Public Backing_Store As Integer ' int
      ''' <summary>
      ''' planes to be preserved if possible
      ''' </summary>
      Public Backing_Planes As Long ' unsigned long
      ''' <summary>
      ''' value to use in restoring planes
      ''' </summary>
      Public Backing_Pixel As Long ' unsigned long
      ''' <summary>
      ''' should bits under be saved? (popups)
      ''' </summary>
      Public Save_Under As Boolean ' Bool
      ''' <summary>
      ''' set of events that should be saved
      ''' </summary>
      Public EventMask As Long 'XEventMask ' long
      ''' <summary>
      ''' set of events that should not propagate
      ''' </summary>
      Public Do_Not_Propogate_Mask As Long 'XEventMask ' long
      ''' <summary>
      ''' boolean value for override-redirect
      ''' </summary>
      Public Override_Redirect As Boolean ' Bool
      ''' <summary>
      ''' color map to be associated with window
      ''' </summary>
      Public Colormap As IntPtr ' Colormap
      ''' <summary>
      ''' cursor to be displayed (or None)
      ''' </summary>
      Public Cursor As IntPtr ' Cursor
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure XWindowAttributes
      ''' <summary>
      ''' location of window
      ''' </summary>
      Public X, Y As Integer ' int
      ''' <summary>
      ''' width and height of window
      ''' </summary>
      Public Width, Height As Integer ' int
      ''' <summary>
      ''' border width of window
      ''' </summary>
      Public Border_Width As Integer ' int
      ''' <summary>
      ''' depth of window
      ''' </summary>
      Public Depth As Integer ' int
      ''' <summary>
      ''' the associated visual structure
      ''' </summary>
      Public Visual As IntPtr ' Visual *
      ''' <summary>
      ''' root of screen containing window
      ''' </summary>
      Public Root As IntPtr ' Window
      ''' <summary>
      ''' InputOutput, InputOnly
      ''' </summary>
      Public [Class] As Integer ' int
      ''' <summary>
      ''' one of bit gravity values
      ''' </summary>
      Public Bit_Gravity As Integer ' int
      ''' <summary>
      ''' one of the window gravity values
      ''' </summary>
      Public Win_Gravity As Integer ' int
      ''' <summary>
      ''' NotUseful, WhenMapped, Always
      ''' </summary>
      Public Backing_Store As Integer ' int
      ''' <summary>
      ''' planes to be preserved if possible
      ''' </summary>
      Public Backing_Planes As Long ' unsigned long
      ''' <summary>
      ''' value to be used when restoring planes
      ''' </summary>
      Public Backing_Pixel As Long ' unsigned long
      ''' <summary>
      ''' boolean, should bits under be saved?
      ''' </summary>
      Public Save_Under As Boolean ' Bool
      ''' <summary>
      ''' color map to be associated with window
      ''' </summary>
      Public Colormap As IntPtr ' Colormap
      ''' <summary>
      ''' boolean, is color map currently installed
      ''' </summary>
      Public Map_Installed As Boolean ' Bool
      ''' <summary>
      ''' IsUnmapped, IsUnviewable, IsViewable
      ''' </summary>
      Public Map_State As XMapState ' int
      ''' <summary>
      ''' set of events all people have interest in
      ''' </summary>
      Public All_Event_Masks As Long ' long
      ''' <summary>
      ''' my event mask
      ''' </summary>
      Public Your_Event_Mask As Long ' long
      ''' <summary>
      ''' set of events that should not propagate
      ''' </summary>
      Public Do_Not_Propagate_Mask As Long ' long
      ''' <summary>
      ''' boolean value for override-redirect
      ''' </summary>
      Public Override_Redirect As Boolean ' Bool
      ''' <summary>
      ''' back pointer to correct screen
      ''' </summary>
      Public Screen As IntPtr ' Screen *
    End Structure

    <StructLayout(LayoutKind.Sequential, Size:=192)>
    Public Structure XEvent
      Public Type As XEventType
    End Structure

#End Region

#Region "Enum"

    Public Enum XMapState
      IsUnmapped = 0
      IsUnviewable = 1
      IsViewable = 2
    End Enum

    <Flags()>
    Public Enum XEventType
      KeyPress = 2
      KeyRelease = 3
      ButtonPress = 4
      ButtonRelease = 5
      MotionNotify = 6
      EnterNotify = 7
      LeaveNotify = 8
      FocusIn = 9
      FocusOut = 10
      KeymapNotify = 11
      Expose = 12
      GraphicsExpose = 13
      NoExpose = 14
      VisibilityNotify = 15
      CreateNotify = 16
      DestroyNotify = 17
      UnmapNotify = 18
      MapNotify = 19
      MapRequest = 20
      ReparentNotify = 21
      ConfigureNotify = 22
      ConfigureRequest = 23
      GravityNotify = 24
      ResizeRequest = 25
      CirculateNotify = 26
      CirculateRequest = 27
      PropertyNotify = 28
      SelectionClear = 29
      SelectionRequest = 30
      SelectionNotify = 31
      ColormapNotify = 32
      ClientMessage = 33
      MappingNotify = 34
    End Enum

    <Flags()>
    Public Enum XWindowAttributeFlags
      CWBackPixel = (1 << 1)
      CWBorderPixmap = (1 << 2)
      CWBorderPixel = (1 << 3)
      CWBitGravity = (1 << 4)
      CWWinGravity = (1 << 5)
      CWBackingStore = (1 << 6)
      CWBackingPlanes = (1 << 7)
      CWBackingPixel = (1 << 8)
      CWOverrideRedirect = (1 << 9)
      CWSaveUnder = (1 << 10)
      CWEventMask = (1 << 11)
      CWDontPropagate = (1 << 12)
      CWColormap = (1 << 13)
      CWCursor = (1 << 14)
    End Enum

    <Flags()>
    Public Enum XEventMask
      KeyPressMask = (1 << 0)
      KeyReleaseMask = (1 << 1)
      ButtonPressMask = (1 << 2)
      ButtonReleaseMask = (1 << 3)
      EnterWindowMask = (1 << 4)
      LeaveWindowMask = (1 << 5)
      PointerMotionMask = (1 << 6)
      PointerMotionHintMask = (1 << 7)
      Button1MotionMask = (1 << 8)
      Button2MotionMask = (1 << 9)
      Button3MotionMask = (1 << 10)
      Button4MotionMask = (1 << 11)
      Button5MotionMask = (1 << 12)
      ButtonMotionMask = (1 << 13)
      KeymapStateMask = (1 << 14)
      ExposureMask = (1 << 15)
      VisibilityChangeMask = (1 << 16)
      StructureNotifyMask = (1 << 17)
      ResizeRedirectMask = (1 << 18)
      SubstructureNotifyMask = (1 << 19)
      SubstructureRedirectMask = (1 << 20)
      FocusChangeMask = (1 << 21)
      PropertyChangeMask = (1 << 22)
      ColormapChangeMask = (1 << 23)
    End Enum

#End Region

#Region "OpenGL"

    Friend Declare Function glXChooseVisual Lib "libGL.so.1" (display As IntPtr,
                                                             screen As Integer,
                                                             ByRef attribList() As Integer) As IntPtr
    Friend Declare Function glXCreateContext Lib "libGL.so.1" (display As IntPtr, vi As IntPtr, shareList As IntPtr, direct As Boolean) As IntPtr
#Disable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function glXGetProcAddress Lib "libGL.so.1" (<MarshalAs(UnmanagedType.LPStr)> procname As String) As IntPtr
#Enable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function glXMakeCurrent Lib "libGL.so.1" (display As IntPtr, draw As IntPtr, context As IntPtr) As Integer
    Friend Declare Sub glViewport Lib "libGL.so.1" (x As Integer, y As Integer, width As Integer, height As Integer)

    Friend Declare Sub glEnable Lib "libGL.so.1" (cap As UInteger)
    Friend Declare Sub glGenTextures Lib "libGL.so.1" (n As Integer, ByRef textures As UInteger)
    Friend Declare Sub glBindTexture Lib "libGL.so.1" (target As UInteger, texture As UInteger)
    Friend Declare Sub glTexParameteri Lib "libGL.so.1" (target As UInteger, pname As UInteger, param As Integer)
    Friend Declare Sub glTexEnvf Lib "libGL.so.1" (target As UInteger, pname As UInteger, param As Single)
    Friend Declare Sub glTexImage2D Lib "libGL.so.1" (target As UInteger, level As Integer, internalformat As Integer, width As Integer, height As Integer, border As Integer, format As UInteger, type As UInteger, data As IntPtr)
    Friend Declare Sub glVertex3f Lib "libGL.so.1" (x As Single, y As Single, z As Single)
    Friend Declare Sub glBegin Lib "libGL.so.1" (mode As UInteger)
    Friend Declare Sub glTexCoord2f Lib "libGL.so.1" (s As Single, t As Single)
    Friend Declare Sub glEnd Lib "libGL.so.1" ()
    Friend Declare Sub glClear Lib "libGL.so.1" (mask As UInteger)
    Friend Declare Sub glXSwapBuffers Lib "libGL.so.1" (display As IntPtr, drawable As IntPtr)
    Friend Declare Sub glTexSubImage2D Lib "libGL.so.1" (target As UInteger, level As Integer, xoffset As Integer, yoffset As Integer, width As Integer, height As Integer, format As UInteger, type As UInteger, pixels As IntPtr)
    Friend Declare Function glXDestroyContext Lib "libGL.so.1" (display As IntPtr, ctx As IntPtr) As Integer

#End Region

#Region "Linux P/Invoke - X11"

    Friend Declare Sub XCloseDisplay Lib "libX11.so.6" (display As IntPtr)
    Friend Declare Function XCreateColormap Lib "libX11.so.6" (display As IntPtr,
                                                               window As IntPtr,
                                                               visual As IntPtr,
                                                               alloc As Integer) As IntPtr
    Friend Declare Function XCreateWindow Lib "libX11.so.6" (display As IntPtr, ' Display*
                                                             parent As IntPtr, ' Window
                                                             x As Integer, ' int
                                                             y As Integer, ' int
                                                             width As Integer, ' unsigned int
                                                             height As Integer, ' unsigned int
                                                             borderWidth As Integer, ' unsigned int
                                                             depth As Integer, ' int
                                                             [class] As Integer, ' unsigned int
                                                             visual As IntPtr, ' Visual*
                                                             valueMask As XWindowAttributeFlags, ' unsigned long
                                                             ByRef attributes As XSetWindowAttributes ' XSetWindowAttributes*
                                                             ) As IntPtr ' Window
    Friend Declare Function XDefaultRootWindow Lib "libX11.so.6" (display As IntPtr) As IntPtr
    Friend Declare Function XDestroyWindow Lib "libX11.so.6" (display As IntPtr, window As IntPtr) As Integer
    Friend Declare Function XFlush Lib "libX11.so.6" (display As IntPtr) As Integer
    Friend Declare Function XGetWindowAttributes Lib "libX11.so.6" (display As IntPtr, w As IntPtr, <Out> ByRef windowAttributesReturn As XWindowAttributes) As Integer
    Friend Declare Function XInitThreads Lib "libX11.so.6" () As Integer
#Disable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function XInternAtom Lib "libX11.so.6" (display As IntPtr, <MarshalAs(UnmanagedType.LPStr)> atomName As String, onlyIfExists As Boolean) As Integer
#Enable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function XLookupKeysym Lib "libX11.so.6" (ByRef keyEvent As XKeyEvent, index As Integer) As XKeySym
    Friend Declare Sub XMapWindow Lib "libX11.so.6" (display As IntPtr, window As IntPtr)
    Friend Declare Function XNextEvent Lib "libX11.so.6" (display As IntPtr, handle As IntPtr) As Integer
#Disable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function XOpenDisplay Lib "libX11.so.6" (<MarshalAs(UnmanagedType.LPStr)> display As String) As IntPtr
#Enable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Function XPending Lib "libX11.so.6" (display As IntPtr) As Integer
    Friend Declare Function XSendEvent Lib "libX11.so.6" (display As IntPtr, ' Display *
                                                          window As IntPtr, ' Window
                                                          propagate As Boolean, ' Bool
                                                          eventMask As Long, ' long
                                                          eventSend As IntPtr ' XEvent *
                                                          ) As Integer ' Status
    Friend Declare Sub XSetWMProtocols Lib "libX11.so.6" (display As IntPtr, window As IntPtr,
                                                          protocols As Integer(), count As Integer)
#Disable Warning CA2101 ' Specify marshaling for P/Invoke string arguments
    Friend Declare Sub XStoreName Lib "libX11.so.6" (display As IntPtr, window As IntPtr, <MarshalAs(UnmanagedType.LPStr)> title As String)
#Enable Warning CA2101 ' Specify marshaling for P/Invoke string arguments

#End Region

  End Class

  Private Delegate Function glSwapInterval_t(display As IntPtr, drawable As IntPtr, interval As Integer) As Integer
  Private glSwapIntervalEXT As glSwapInterval_t
  Private pge_Display As IntPtr
  Private pge_WindowRoot As IntPtr
  Private pge_Window As IntPtr
  Private pge_VisualInfo As IntPtr
  Private pge_ColorMap As IntPtr
  Private pge_SetWindowAttribs As X11.XSetWindowAttributes

#End Region

  Public Delegate Function PixelModeDelegate(x As Integer, y As Integer, p1 As Pixel, p2 As Pixel) As Pixel
  Private funcPixelMode As PixelModeDelegate

  Protected Property AppName As String

  Protected m_hWnd As IntPtr
  Private m_glBuffer As UInteger

  Private m_pixelMode As Pixel.Mode
  Private m_blendFactor As Single = 1.0F

  Private m_screenWidth As Integer
  Private m_screenHeight As Integer
  Private m_pixelWidth As Integer
  Private m_pixelHeight As Integer
  Private m_fullScreen As Boolean
  Private m_enableVSYNC As Boolean
  Private m_pixelX As Single
  Private m_pixelY As Single
  Private m_defaultDrawTarget As Sprite
  Private m_drawTarget As Sprite

  'Private Shared m_shutdown As Boolean
  Private m_windowWidth As Integer
  Private m_windowHeight As Integer
  Private m_hasInputFocus As Boolean
  Private m_hasMouseFocus As Boolean
  Private m_frameTimer As Single = 1.0F
  Private m_frameCount As Integer
  Private m_totalFrameCount As Integer
  'Private m_totalFrames As Integer

  Private ReadOnly m_keyNewState(255) As Boolean
  Private ReadOnly m_keyOldState(255) As Boolean
  Private ReadOnly m_keyboardState(255) As HwButton

  Private ReadOnly m_mouseNewState(4) As Boolean
  Private ReadOnly m_mouseOldState(4) As Boolean
  Private ReadOnly m_mouseState(4) As HwButton
  Private m_mousePosXcache As Integer
  Private m_mousePosYcache As Integer
  Private m_mousePosX As Integer
  Private m_mousePosY As Integer
  Private m_mouseWheelDelta As Integer
  Private m_mouseWheelDeltaCache As Integer
  Private m_viewX As Integer
  Private m_viewY As Integer
  Private m_viewW As Integer
  Private m_viewH As Integer

  Private m_glRenderContext As IntPtr
  Private m_glDeviceContext As IntPtr

  Private m_subPixelOffsetX As Single
  Private m_subPixelOffsetY As Single

  Protected Structure HwButton
    Public Pressed As Boolean ' Set once during the frame the event occurs
    Public Released As Boolean ' Set once during the frame the event occurs
    Public Held As Boolean ' Set true for all frames between pressed and released events
    Public ElapsedTime As Single
  End Structure

  Public Enum RCode
    Ok
    Fail
    NoFile
  End Enum

  Public Enum Key
    NONE
    A
    B
    C
    D
    E
    F
    G
    H
    I
    J
    K
    L
    M
    N
    O
    P
    Q
    R
    S
    T
    U
    V
    W
    X
    Y
    Z
    K0
    K1
    K2
    K3
    K4
    K5
    K6
    K7
    K8
    K9
    F1
    F2
    F3
    F4
    F5
    F6
    F7
    F8
    F9
    F10
    F11
    F12
    UP
    DOWN
    LEFT
    RIGHT
    SPACE
    TAB
    SHIFT
    CTRL
    INS
    DEL
    HOME
    [END]
    PGUP
    PGDN
    BACK
    ESCAPE
    [RETURN]
    ENTER
    PAUSE
    SCROLL
    NP0
    NP1
    NP2
    NP3
    NP4
    NP5
    NP6
    NP7
    NP8
    NP9
    NP_MUL
    NP_DIV
    NP_ADD
    NP_SUB
    NP_DECIMAL
    PERIOD
    EQUALS
    COMMA
    MINUS
    OEM_1
    OEM_2
    OEM_3
    OEM_4
    OEM_5
    OEM_6
    OEM_7
    OEM_8
    CAPS_LOCK
    ENUM_END
    ALT
  End Enum

  Private m_fontSprite As Sprite

  Protected Friend Sub New()
    AppName = "Undefined"
    Singleton.Pge = Me
  End Sub

  Private m_wpPrev As Win32.WINDOWPLACEMENT

  Public Function GetScreenSize() As (w As Integer, h As Integer)

    If IsOSPlatform(Windows) Then
      Dim mi = New Win32.MONITORINFO With {.cbSize = Marshal.SizeOf(GetType(Win32.MONITORINFO))}
      If Win32.GetMonitorInfo(Win32.MonitorFromWindow(m_hWnd, 0), mi) Then
        Return (mi.rcMonitor.Right - mi.rcMonitor.Left, mi.rcMonitor.Bottom - mi.rcMonitor.Top)
      End If
    Else
      'Throw New NotImplementedException()
      Return (1920, 1080)
    End If

  End Function

  Public ReadOnly Property IsFullScreen As Boolean
    Get
      If IsOSPlatform(Windows) Then
        Dim currentStyle = Win32.GetWindowLong(m_hWnd, GWL_STYLE)
        Return Not ((currentStyle And WS_OVERLAPPEDWINDOW) = WS_OVERLAPPEDWINDOW)
      Else
        'Throw New NotImplementedException()
      End If
      Return False
    End Get
  End Property

  Public Sub ToggleFullScreen()

    If IsOSPlatform(Windows) Then

      ' Get the current window style
      Dim currentStyle = Win32.GetWindowLong(m_hWnd, GWL_STYLE)

      ' Check if the window is currently in full screen mode
      'Dim isFullScreen = (currentStyle And WS_POPUP) = WS_POPUP
      Dim isFullScreen = Not ((currentStyle And WS_OVERLAPPEDWINDOW) = WS_OVERLAPPEDWINDOW)

      If isFullScreen Then
        ' Restore to normal window
        'Win32.SetWindowLong(m_hWnd, GWL_STYLE, currentStyle And Not WS_POPUP)
        'Win32.SetWindowPos(m_hWnd, IntPtr.Zero, 100, 100, 800, 600, SWP_NOZORDER Or SWP_NOMOVE Or SWP_NOSIZE Or SWP_FRAMECHANGED)
        'Win32.ShowWindow(m_hWnd, SW_SHOWNORMAL)

        Win32.SetWindowLong(m_hWnd, GWL_STYLE, currentStyle Or WS_OVERLAPPEDWINDOW)
        Win32.SetWindowPlacement(m_hWnd, m_wpPrev)
        'Win32.SetWindowPos(m_hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOOWNERZORDER Or SWP_FRAMECHANGED)

        'm_iconHandle = Win32.SendMessage(m_hWnd, WM_GETICON, CType(ICON_BIG, IntPtr), IntPtr.Zero)
        'If m_iconHandle <> IntPtr.Zero Then
        '  Win32.SendMessage(m_hWnd, WM_SETICON, CType(ICON_BIG, IntPtr), m_iconHandle)
        'End If

      Else
        'Win32.SetWindowLong(m_hWnd, GWL_STYLE, currentStyle Or WS_POPUP)
        'Win32.SetWindowPos(m_hWnd, Win32.GetDesktopWindow(), 0, 0, 0, 0, SWP_NOZORDER Or SWP_NOMOVE Or SWP_NOSIZE Or SWP_FRAMECHANGED)
        'Win32.ShowWindow(m_hWnd, SW_SHOWMAXIMIZED)

        'm_iconHandle = Win32.SendMessage(m_hWnd, WM_GETICON, CType(ICON_BIG, IntPtr), IntPtr.Zero)
        m_wpPrev = New Win32.WINDOWPLACEMENT With {.Length = CUInt(Marshal.SizeOf(GetType(Win32.WINDOWPLACEMENT)))}
        Dim mi = New Win32.MONITORINFO With {.cbSize = Marshal.SizeOf(GetType(Win32.MONITORINFO))}

        If Win32.GetWindowPlacement(m_hWnd, m_wpPrev) AndAlso
           Win32.GetMonitorInfo(Win32.MonitorFromWindow(m_hWnd, 0), mi) Then
          Win32.SetWindowLong(m_hWnd, GWL_STYLE, currentStyle And (Not WS_OVERLAPPEDWINDOW))
          Win32.SetWindowPos(m_hWnd, IntPtr.Zero,
                             mi.rcMonitor.Left, mi.rcMonitor.Top,
                             mi.rcMonitor.Right - mi.rcMonitor.Left,
                             mi.rcMonitor.Bottom - mi.rcMonitor.Top,
                             SWP_NOOWNERZORDER Or SWP_FRAMECHANGED)
        End If

      End If
    Else
      'TODO: Linux?
    End If
  End Sub

  Public Sub DecreasePixelSize()
    If m_pixelWidth > 1 AndAlso m_pixelHeight > 1 Then
      If IsOSPlatform(Windows) Then
        m_pixelWidth -= 1
        m_pixelHeight -= 1
        m_windowWidth = m_screenWidth * m_pixelWidth
        m_windowHeight = m_screenHeight * m_pixelHeight
        Dim dwExStyle = WS_EX_APPWINDOW Or WS_EX_WINDOWEDGE
        Dim dwStyle = WS_CAPTION Or WS_SYSMENU Or WS_VISIBLE Or WS_THICKFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX
        Dim rWndRect = New Win32.RECT With {.Left = 0, .Top = 0, .Right = m_windowWidth, .Bottom = m_windowHeight}
        Win32.AdjustWindowRectEx(rWndRect, dwStyle, False, dwExStyle)
        Dim width = rWndRect.Right - rWndRect.Left
        Dim height = rWndRect.Bottom - rWndRect.Top
        Win32.SetWindowPos(m_hWnd, IntPtr.Zero, 0, 0, width, height, SWP_NOOWNERZORDER Or SWP_NOMOVE Or SWP_NOZORDER Or SWP_FRAMECHANGED)
      Else
        'TODO: Linux?
      End If
    End If
  End Sub

  Public Sub IncreasePixelSize()
    'TODO: Possibly limit to a maximum?
    If IsOSPlatform(Windows) Then
      m_pixelWidth += 1
      m_pixelHeight += 1
      m_windowWidth = m_screenWidth * m_pixelWidth
      m_windowHeight = m_screenHeight * m_pixelHeight
      Dim dwExStyle = WS_EX_APPWINDOW Or WS_EX_WINDOWEDGE
      Dim dwStyle = WS_CAPTION Or WS_SYSMENU Or WS_VISIBLE Or WS_THICKFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX
      Dim rWndRect = New Win32.RECT With {.Left = 0, .Top = 0, .Right = m_windowWidth, .Bottom = m_windowHeight}
      Win32.AdjustWindowRectEx(rWndRect, dwStyle, False, dwExStyle)
      Dim width = rWndRect.Right - rWndRect.Left
      Dim height = rWndRect.Bottom - rWndRect.Top
      Win32.SetWindowPos(m_hWnd, IntPtr.Zero, 0, 0, width, height, SWP_NOOWNERZORDER Or SWP_NOMOVE Or SWP_NOZORDER Or SWP_FRAMECHANGED)
    Else
      'TODO: Linux?
    End If
  End Sub

  Public Function CapsLock() As Boolean
    If IsOSPlatform(Windows) Then
      Return (Win32.GetKeyState(VK_CAPITAL) And 1) <> 0
    Else
      If pge_Display <> IntPtr.Zero Then
        ' Tried XkbGetIndicatorState (along with XkbQueryExtension), XGetModifierMapping and XQueryKeymap
        ' with no success; not sure what the right approach is supposed to be here.
      End If
      Return False
    End If
  End Function

  Public Function NumLock() As Boolean
    If IsOSPlatform(Windows) Then
      Return (Win32.GetKeyState(VK_NUMLOCK) And 1) <> 0
    Else
      If pge_Display <> IntPtr.Zero Then
        ' See CapsLock for more details.
      End If
      Return False
    End If
  End Function

  Public Function Construct(screenW As Integer, screenH As Integer, pixelW As Integer, pixelH As Integer, Optional fullScreen As Boolean = False, Optional vsync As Boolean = False) As Boolean 'RCode

    m_screenWidth = screenW
    m_screenHeight = screenH
    m_pixelWidth = pixelW
    m_pixelHeight = pixelH
    m_fullScreen = fullScreen
    m_enableVSYNC = vsync

    m_pixelX = 2.0F / m_screenWidth
    m_pixelY = 2.0F / m_screenHeight

    If m_pixelWidth = 0 OrElse m_pixelHeight = 0 OrElse m_screenWidth = 0 OrElse m_screenHeight = 0 Then
      Return False 'RCode.FAIL
    End If

    ' Load the default font sheet
    Pge_ConstructFontSheet()

    ' Create a sprite that represents the primary drawing target
    m_defaultDrawTarget = New Sprite(m_screenWidth, m_screenHeight)
    SetDrawTarget(Nothing)

    Return True 'RCode.OK

  End Function

  Protected Sub SetScreenSize(w As Integer, h As Integer)

    m_defaultDrawTarget = Nothing '.Dispose()
    m_screenWidth = w
    m_screenHeight = h
    m_defaultDrawTarget = New Sprite(m_screenWidth, m_screenHeight)
    SetDrawTarget(Nothing)

    If IsOSPlatform(Windows) Then
      Win32.glClear(GL_COLOR_BUFFER_BIT)
      Win32.SwapBuffers(m_glDeviceContext)
      Win32.glClear(GL_COLOR_BUFFER_BIT)
    Else
      X11.glClear(GL_COLOR_BUFFER_BIT)
      X11.glXSwapBuffers(pge_Display, pge_Window)
      X11.glClear(GL_COLOR_BUFFER_BIT)
    End If

    Pge_UpdateViewport()

  End Sub

  <DebuggerNonUserCode, DebuggerStepThrough>
  Public Function Start() As RCode

    ' Construct the window
    If IsOSPlatform(Windows) Then
      If WindowCreateWindows() = IntPtr.Zero Then Return RCode.Fail
    ElseIf IsOSPlatform(Linux) Then
      If WindowCreateLinux() = IntPtr.Zero Then Return RCode.Fail
    Else
      Return RCode.Fail
    End If

    If IsOSPlatform(Windows) Then
      'FreeConsole() ' doesn't always work??????
      Dim ptr = Win32.GetConsoleWindow
      Win32.ShowWindow(ptr, 0)
    End If

    ' Start the thread
    Singleton.AtomActive = True
    Dim t As New Thread(AddressOf EngineThread)
    t.Start()

    If IsOSPlatform(Windows) Then
      ' Handle Windows Message Loop
      Dim m = New Win32.MSG With {.message = 0, ' Set the message parameter to zero to retrieve any message.
                                  .hWnd = IntPtr.Zero, ' Set the window handle parameter to zero to retrieve messages for any window.
                                  .wParam = IntPtr.Zero, ' Set the wParam parameter to zero.
                                  .lParam = IntPtr.Zero, ' Set the lParam parameter to zero.
                                  .time = 0, ' Set the time parameter to zero.
                                  .pt = New Win32.Point(0, 0)} ' Set the cursor position to (0,0).
      While Win32.GetMessage(m, IntPtr.Zero, 0, 0) > 0
        Win32.TranslateMessage(m)
        Dim unused = Win32.DispatchMessage(m)
      End While
    End If

    ' Wait for thread to be exited

    Return RCode.Ok

  End Function

  Public Sub SetDrawTarget(target As Sprite)
    If target IsNot Nothing Then
      m_drawTarget = target
    Else
      m_drawTarget = m_defaultDrawTarget
    End If
  End Sub

  Friend ReadOnly Property GetDrawTarget() As Sprite
    Get
      Return m_drawTarget
    End Get
  End Property

  Private Protected ReadOnly Property GetDrawTargetWidth() As Integer
    Get
      If m_drawTarget IsNot Nothing Then
        Return m_drawTarget.Width
      Else
        Return 0
      End If
    End Get
  End Property

  Private Protected ReadOnly Property GetDrawTargetHeight() As Integer
    Get
      If m_drawTarget IsNot Nothing Then
        Return m_drawTarget.Height
      Else
        Return 0
      End If
    End Get
  End Property

  Protected ReadOnly Property IsFocused() As Boolean
    Get
      Return m_hasInputFocus
    End Get
  End Property

  Protected ReadOnly Property GetKey(k As Key) As HwButton
    Get
      Return m_keyboardState(k)
    End Get
  End Property

  Protected ReadOnly Property GetMouse(b As Integer) As HwButton
    Get
      Return m_mouseState(b)
    End Get
  End Property

  Protected ReadOnly Property GetMouseX() As Integer
    Get
      Return m_mousePosX
    End Get
  End Property

  Protected ReadOnly Property GetMouseY() As Integer
    Get
      Return m_mousePosY
    End Get
  End Property

  Protected ReadOnly Property GetMouseWheel() As Integer
    Get
      Return m_mouseWheelDelta
    End Get
  End Property

  Public ReadOnly Property ScreenWidth As Integer
    Get
      Return m_screenWidth
    End Get
  End Property

  Public ReadOnly Property ScreenHeight As Integer
    Get
      Return m_screenHeight
    End Get
  End Property

  Protected Function Draw(pos As Vi2d) As Boolean
    Return Draw(pos.x, pos.y, Presets.White)
  End Function

  Protected Function Draw(pos As Vi2d, p As Pixel) As Boolean
    Return Draw(pos.x, pos.y, p)
  End Function

  Protected Overridable Function Draw(x As Integer, y As Integer) As Boolean
    Return Draw(x, y, Presets.White)
  End Function

  Public Function Draw(x As Integer, y As Integer, p As Pixel) As Boolean

    If m_drawTarget Is Nothing Then
      Return False
    End If

    If m_pixelMode = Pixel.Mode.Normal Then
      Return m_drawTarget.SetPixel(x, y, p)
    End If

    If m_pixelMode = Pixel.Mode.Mask Then
      If p.A = 255 Then
        Return m_drawTarget.SetPixel(x, y, p)
      End If
    End If

    If m_pixelMode = Pixel.Mode.Alpha Then
      Dim d = m_drawTarget.GetPixel(x, y)
      Dim a = (p.A / 255.0F) * m_blendFactor
      Dim c = 1.0F - a
      Dim r = a * p.R + c * d.R
      Dim g = a * p.G + c * d.G
      Dim b = a * p.B + c * d.B
      Return m_drawTarget.SetPixel(x, y, New Pixel(CByte(r), CByte(g), CByte(b)))
    End If

    If m_pixelMode = Pixel.Mode.Custom Then
      Return m_drawTarget.SetPixel(x, y, funcPixelMode(x, y, p, m_drawTarget.GetPixel(x, y)))
    End If

    Return False

  End Function

  Protected Sub SetSubPixelOffset(ox As Single, oy As Single)
    m_subPixelOffsetX = ox * m_pixelX
    m_subPixelOffsetY = oy * m_pixelY
  End Sub

  Protected Sub DrawLine(pos1 As Vi2d, pos2 As Vi2d)
    DrawLine(pos1.x, pos1.y, pos2.x, pos2.y, Presets.White, &HFFFFFFFFUI)
  End Sub

  Protected Sub DrawLine(pos1 As Vi2d, pos2 As Vi2d, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)
    DrawLine(pos1.x, pos1.y, pos2.x, pos2.y, p, pattern)
  End Sub

  Protected Sub DrawLine(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer)
    DrawLine(x1, y1, x2, y2, Presets.White, &HFFFFFFFFUI)
  End Sub

  'Public Sub DrawLine(x1 As Single, y1 As Single, x2 As Single, y2 As Single, p As Pixel)
  '  DrawLine(CInt(Fix(x1)), CInt(Fix(y1)), CInt(Fix(x2)), CInt(Fix(y2)), p, &HFFFFFFFFUI)
  'End Sub

  Public Sub DrawLine(x1 As Double, y1 As Double, x2 As Double, y2 As Double, p As Pixel)
    DrawLine(CInt(Fix(x1)), CInt(Fix(y1)), CInt(Fix(x2)), CInt(Fix(y2)), p, &HFFFFFFFFUI)
  End Sub

  Public Sub DrawLine(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, p As Pixel, Optional pattern As UInteger = &HFFFFFFFFUI)

    Dim dx = x2 - x1
    Dim dy = y2 - y1

    Dim rol = New Func(Of Boolean)(Function()
                                     pattern = (pattern << 1) Or (pattern >> 31)
                                     Return CInt(pattern And 1) <> 0
                                   End Function)

    ' straight line idea by gurkanctn
    If dx = 0 Then ' Line is vertical
      If y2 < y1 Then Swap(y1, y2)
      For y = y1 To y2
        If rol() Then Draw(x1, y, p)
      Next
      Return
    End If

    If dy = 0 Then ' Line is horizontal
      If x2 < x1 Then Swap(x1, x2)
      For x = x1 To x2
        If rol() Then Draw(x, y1, p)
      Next
      Return
    End If

    ' Line is Funk-aye
    Dim dx1 = MathF.Abs(dx) : Dim dy1 = MathF.Abs(dy)
    Dim px = 2 * dy1 - dx1 : Dim py = 2 * dx1 - dy1

    If dy1 <= dx1 Then

      Dim x, y As Integer
      Dim xe As Integer

      If dx >= 0 Then
        x = x1 : y = y1 : xe = x2
      Else
        x = x2 : y = y2 : xe = x1
      End If
      If rol() Then Draw(x, y, p)

      For i = 0 To xe - x
        x += 1
        If px < 0 Then
          px += 2 * dy1
        Else
          If (dx < 0 AndAlso dy < 0) OrElse (dx > 0 AndAlso dy > 0) Then
            y += 1
          Else
            y -= 1
          End If
          px += 2 * (dy1 - dx1)
        End If
        If rol() Then Draw(x, y, p)
      Next

    Else

      Dim x, y As Integer
      Dim ye As Integer

      If dy >= 0 Then
        x = x1 : y = y1 : ye = y2
      Else
        x = x2 : y = y2 : ye = y1
      End If
      If rol() Then Draw(x, y, p)

      For i = 0 To ye - y
        y += 1
        If py <= 0 Then
          py += 2 * dx1
        Else
          If (dx < 0 AndAlso dy < 0) OrElse (dx > 0 AndAlso dy > 0) Then
            x += 1
          Else
            x -= 1
          End If
          py += 2 * (dx1 - dy1)
        End If
        If rol() Then Draw(x, y, p)
      Next

    End If

  End Sub

  Protected Sub DrawCircle(pos As Vi2d, radius As Integer)
    DrawCircle(pos.x, pos.y, radius, Presets.White, &HFF)
  End Sub

  Protected Sub DrawCircle(pos As Vi2d, radius As Integer, p As Pixel, Optional mask As Byte = &HFF)
    DrawCircle(pos.x, pos.y, radius, p, mask)
  End Sub

  Protected Sub DrawCircle(x As Integer, y As Integer, radius As Integer)
    DrawCircle(x, y, radius, Presets.White, &HFF)
  End Sub

  Public Sub DrawCircle(x As Integer, y As Integer, radius As Integer, p As Pixel, Optional mask As Byte = &HFF)

    Dim x0 = 0
    Dim y0 = radius
    Dim d = 3 - 2 * radius
    If radius = 0 Then Return

    While y0 >= x0 ' only formulate 1/8 of circle
      If (mask And &H1) <> 0 Then Draw(x + x0, y - y0, p)
      If (mask And &H2) <> 0 Then Draw(x + y0, y - x0, p)
      If (mask And &H4) <> 0 Then Draw(x + y0, y + x0, p)
      If (mask And &H8) <> 0 Then Draw(x + x0, y + y0, p)
      If (mask And &H10) <> 0 Then Draw(x - x0, y + y0, p)
      If (mask And &H20) <> 0 Then Draw(x - y0, y + x0, p)
      If (mask And &H40) <> 0 Then Draw(x - y0, y - x0, p)
      If (mask And &H80) <> 0 Then Draw(x - x0, y - y0, p)
      If d < 0 Then
        d += 4 * x0 + 6 : x0 += 1
      Else
        d += 4 * (x0 - y0) + 10 : x0 += 1 : y0 -= 1
      End If
    End While

  End Sub

  Protected Sub FillCircle(pos As Vi2d, radius As Integer)
    FillCircle(pos.x, pos.y, radius, Presets.White)
  End Sub

  Protected Sub FillCircle(pos As Vi2d, radius As Integer, p As Pixel)
    FillCircle(pos.x, pos.y, radius, p)
  End Sub

  Public Sub FillCircle(x As Single, y As Single, radius As Single, p As Pixel)
    FillCircle(CInt(Fix(x)), CInt(Fix(y)), CInt(Fix(radius)), p)
  End Sub

  Public Sub FillCircle(x As Integer, y As Integer, radius As Integer, p As Pixel)

    Dim x0 = 0
    Dim y0 = radius
    Dim d = 3 - 2 * radius
    If radius = 0 Then Return

    Dim drawLine = Sub(sx As Integer, ex As Integer, ny As Integer)
                     For i = sx To ex
                       Draw(i, ny, p)
                     Next
                   End Sub

    While y0 >= x0
      drawLine(x - x0, x + x0, y - y0)
      drawLine(x - y0, x + y0, y - x0)
      drawLine(x - x0, x + x0, y + y0)
      drawLine(x - y0, x + y0, y + x0)
      If d < 0 Then
        d += 4 * x0 + 6 : x0 += 1
      Else
        d += 4 * (x0 - y0) + 10 : x0 += 1 : y0 -= 1
      End If
    End While

  End Sub

  Protected Sub DrawRect(pos As Vi2d, size As Vi2d)
    DrawRect(pos.x, pos.y, size.x, size.y, Presets.White)
  End Sub

  Protected Sub DrawRect(pos As Vi2d, size As Vi2d, p As Pixel)
    DrawRect(pos.x, pos.y, size.x, size.y, p)
  End Sub

  Protected Sub DrawRect(x As Integer, y As Integer, w As Integer, h As Integer)
    DrawRect(x, y, w, h, Presets.White)
  End Sub

  Public Sub DrawRect(x As Integer, y As Integer, w As Integer, h As Integer, p As Pixel)
    DrawLine(x, y, x + w, y, p)
    DrawLine(x + w, y, x + w, y + h, p)
    DrawLine(x + w, y + h, x, y + h, p)
    DrawLine(x, y + h, x, y, p)
  End Sub

  Protected Sub Clear()
    Clear(Presets.Black)
  End Sub

  Protected Sub Clear(p As Pixel)
    Dim pixels = GetDrawTargetWidth() * GetDrawTargetHeight()
    Dim m() = GetDrawTarget().GetData()
    For i = 0 To pixels - 1
      m(i) = p
    Next
#If PGE_DBG_OVERDRAW Then
    Sprite.nOverdrawCount += pixels
#End If
  End Sub

  Protected Sub FillRect(pos As Vi2d, size As Vi2d)
    FillRect(pos.x, pos.y, size.x, size.y, Presets.White)
  End Sub

  Protected Sub FillRect(pos As Vi2d, size As Vi2d, p As Pixel)
    FillRect(pos.x, pos.y, size.x, size.y, p)
  End Sub

  Protected Sub FillRect(x As Integer, y As Integer, w As Integer, h As Integer)
    FillRect(x, y, w, h, Presets.White)
  End Sub

  Public Sub FillRect(x As Integer, y As Integer, w As Integer, h As Integer, p As Pixel)

    Dim x2 = x + w
    Dim y2 = y + h

    If x < 0 Then x = 0
    If x >= GetDrawTargetWidth() Then x = GetDrawTargetWidth()
    If y < 0 Then y = 0
    If y >= GetDrawTargetHeight() Then y = GetDrawTargetHeight()

    If x2 < 0 Then x2 = 0
    If x2 >= GetDrawTargetWidth() Then x2 = GetDrawTargetWidth()
    If y2 < 0 Then y2 = 0
    If y2 >= GetDrawTargetHeight() Then y2 = GetDrawTargetHeight()

    For i = x To x2 - 1
      For j = y To y2 - 1
        Draw(i, j, p)
      Next
    Next

  End Sub

  Protected Sub DrawTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d)
    DrawTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, Presets.White)
  End Sub

  Protected Sub DrawTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d, p As Pixel)
    DrawTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, p)
  End Sub

  Protected Sub DrawTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer)
    DrawTriangle(x1, y1, x2, y2, x3, y3, Presets.White)
  End Sub

  Public Sub DrawTriangle(x1 As Single, y1 As Single, x2 As Single, y2 As Single, x3 As Single, y3 As Single, p As Pixel)
    DrawTriangle(CInt(Fix(x1)), CInt(Fix(y1)), CInt(Fix(x2)), CInt(Fix(y2)), CInt(Fix(x3)), CInt(Fix(y3)), p)
  End Sub

  Public Sub DrawTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer, p As Pixel)
    DrawLine(x1, y1, x2, y2, p)
    DrawLine(x2, y2, x3, y3, p)
    DrawLine(x3, y3, x1, y1, p)
  End Sub

  Protected Sub FillTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d)
    FillTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, Presets.White)
  End Sub

  Protected Sub FillTriangle(pos1 As Vi2d, pos2 As Vi2d, pos3 As Vi2d, p As Pixel)
    FillTriangle(pos1.x, pos1.y, pos2.x, pos2.y, pos3.x, pos3.y, p)
  End Sub

  Protected Sub FillTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer)
    FillTriangle(x1, y1, x2, y2, x3, y3, Presets.White)
  End Sub

  Public Sub FillTriangle(x1 As Single, y1 As Single, x2 As Single, y2 As Single, x3 As Single, y3 As Single, p As Pixel)
    FillTriangle(CInt(Fix(x1)), CInt(Fix(y1)), CInt(Fix(x2)), CInt(Fix(y2)), CInt(Fix(x3)), CInt(Fix(y3)), p)
  End Sub

  Public Sub FillTriangle(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, x3 As Integer, y3 As Integer, p As Pixel)

    Dim drawline = Sub(sx As Integer, ex As Integer, ny As Integer)
                     For i = sx To ex
                       Draw(i, ny, p)
                     Next
                   End Sub

    Dim t1x, t2x, y, minx, maxx, t1xp, t2xp As Integer
    Dim changed1, changed2 As Boolean
    Dim signx1, signx2, dx1, dy1, dx2, dy2 As Integer
    Dim e1, e2 As Integer
    ' Sort vertices
    If y1 > y2 Then Swap(y1, y2) : Swap(x1, x2)
    If y1 > y3 Then Swap(y1, y3) : Swap(x1, x3)
    If y2 > y3 Then Swap(y2, y3) : Swap(x2, x3)

    t1x = x1 : t2x = x1 : y = y1 ' Starting points
    dx1 = x2 - x1
    If dx1 < 0 Then
      dx1 = -dx1
      signx1 = -1
    Else
      signx1 = 1
    End If
    dy1 = y2 - y1

    dx2 = x3 - x1
    If dx2 < 0 Then
      dx2 = -dx2
      signx2 = -1
    Else
      signx2 = 1
    End If
    dy2 = y3 - y1

    If dy1 > dx1 Then ' swap values
      Swap(dx1, dy1) : changed1 = True
    End If
    If dy2 > dx2 Then ' swap values
      Swap(dy2, dx2) : changed2 = True
    End If

    e2 = dx2 >> 1
    ' Flat top, just process the second half
    If y1 = y2 Then GoTo nextx
    e1 = dx1 >> 1

    For i = 0 To dx1 - 1
      t1xp = 0 : t2xp = 0

      If t1x < t2x Then
        minx = t1x : maxx = t2x
      Else
        minx = t2x : maxx = t1x
      End If

      ' process first line until y value is about to change
      While i < dx1
        'i += 1
        e1 += dy1
        While e1 >= dx1
          e1 -= dx1
          If changed1 Then
            t1xp = signx1
          Else
            GoTo next1
          End If
        End While
        If changed1 Then Exit While
        t1x += signx1
      End While
      ' Move line
next1:
      ' Process second line until y value is about to change
      While True
        e2 += dy2
        While e2 >= dx2
          e2 -= dx2
          If changed2 Then
            t2xp = signx2 ' t2x += signx2
          Else
            GoTo next2
          End If
        End While
        If changed2 Then Exit While
        t2x += signx2
      End While
next2:
      If minx > t1x Then minx = t1x
      If minx > t2x Then minx = t2x
      If maxx < t1x Then maxx = t1x
      If maxx < t2x Then maxx = t2x
      drawline(minx, maxx, y)    ' Draw line from min to max points found on the y
      ' Now increase y
      If Not changed1 Then t1x += signx1
      t1x += t1xp
      If Not changed2 Then t2x += signx2
      t2x += t2xp
      y += 1
      If y = y2 Then Exit For
    Next
nextx:
    ' Second half
    dx1 = x3 - x2
    If dx1 < 0 Then
      dx1 = -dx1 : signx1 = -1
    Else
      signx1 = 1
    End If
    dy1 = y3 - y2 : t1x = x2

    If dy1 > dx1 Then ' swap values
      Swap(dy1, dx1) : changed1 = True
    Else
      changed1 = False
    End If

    e1 = dx1 >> 1

    For i = 0 To dx1
      t1xp = 0
      t2xp = 0
      If t1x < t2x Then
        minx = t1x : maxx = t2x
      Else
        minx = t2x : maxx = t1x
      End If
      ' process first line until y value is about to change
      While i < dx1
        e1 += dy1
        While e1 >= dx1
          e1 -= dx1
          If changed1 Then t1xp = signx1 : Exit While ' t1x += signx1
          GoTo next3
        End While
        If changed1 Then Exit While
        t1x += signx1
        If i < dx1 Then i += 1
      End While
next3:
      ' process second line until y value is about to change
      While t2x <> x3
        e2 += dy2
        While e2 >= dx2
          e2 -= dx2
          If changed2 Then
            t2xp = signx2
          Else
            GoTo next4
          End If
        End While
        If changed2 Then Exit While
        t2x += signx2
      End While
next4:

      If minx > t1x Then minx = t1x
      If minx > t2x Then minx = t2x
      If maxx < t1x Then maxx = t1x
      If maxx < t2x Then maxx = t2x
      drawline(minx, maxx, y)
      If Not changed1 Then t1x += signx1
      t1x += t1xp
      If Not changed2 Then t2x += signx2
      t2x += t2xp
      y += 1
      If y > y3 Then Return

    Next

  End Sub

  Protected Sub DrawSprite(pos As Vi2d, sprite As Sprite, Optional scale As Integer = 1)
    DrawSprite(pos.x, pos.y, sprite, scale)
  End Sub

  Public Sub DrawSprite(x As Integer, y As Integer, sprite As Sprite, Optional scale As Integer = 1)

    If sprite Is Nothing Then Return

    If scale > 1 Then
      For i = 0 To sprite.Width - 1
        For j = 0 To sprite.Height - 1
          For iIs = 0 To scale - 1
            For js = 0 To scale - 1
              Dim dx = x + (i * scale) + iIs
              Dim dy = y + (j * scale) + js
              If dx < 0 OrElse dy < 0 OrElse dx > ScreenWidth - 1 OrElse dy > ScreenHeight - 1 Then Continue For
              Draw(dx, dy, sprite.GetPixel(i, j))
            Next
          Next
        Next
      Next
    Else
      For i = 0 To sprite.Width - 1
        For j = 0 To sprite.Height - 1
          Dim dx = x + i, dy = y + j
          If dx < 0 OrElse dy < 0 OrElse dx > ScreenWidth - 1 OrElse dy > ScreenHeight - 1 Then Continue For
          Draw(dx, dy, sprite.GetPixel(i, j))
        Next
      Next
    End If

  End Sub

  Protected Sub DrawPartialSprite(pos As Vi2d, sprite As Sprite, sourcepos As Vi2d, size As Vi2d, Optional scale As Integer = 1)
    DrawPartialSprite(pos.x, pos.y, sprite, sourcepos.x, sourcepos.y, size.x, size.y, scale)
  End Sub

  Protected Sub DrawPartialSprite(x As Integer, y As Integer, sprite As Sprite, ox As Integer, oy As Integer, w As Integer, h As Integer, Optional scale As Integer = 1)

    If sprite Is Nothing Then
      Return
    End If

    If scale > 1 Then
      For i = 0 To w - 1
        For j = 0 To h - 1
          For iIs = 0 To scale - 1
            For js = 0 To scale - 1
              Draw(x + (i * scale) + iIs, y + (j * scale) + js, sprite.GetPixel(i + ox, j + oy))
            Next
          Next
        Next
      Next
    Else
      For i = 0 To w - 1
        For j = 0 To h - 1
          Draw(x + i, y + j, sprite.GetPixel(i + ox, j + oy))
        Next
      Next
    End If

  End Sub

  Protected Sub DrawString(pos As Vi2d, sText As String)
    DrawString(pos.x, pos.y, sText, Presets.White, 1)
  End Sub

  Protected Sub DrawString(pos As Vi2d, sText As String, col As Pixel, Optional scale As Integer = 1)
    DrawString(pos.x, pos.y, sText, col, scale)
  End Sub

  Protected Sub DrawString(x As Integer, y As Integer, sText As String)
    DrawString(x, y, sText, Presets.White, 1)
  End Sub

  Protected Sub DrawString(x As Integer, y As Integer, sText As String, col As Pixel, Optional scale As Integer = 1)

    Dim sx = 0
    Dim sy = 0
    Dim m = m_pixelMode

    If col.A <> 255 Then
      SetPixelMode(Pixel.Mode.Alpha)
    Else
      SetPixelMode(Pixel.Mode.Mask)
    End If

    For Each c In sText
      If c = vbLf Then
        sx = 0
        sy += 8 * scale
      Else
        Dim ox = (Asc(c) - 32) Mod 16
        Dim oy = (Asc(c) - 32) \ 16
        If scale > 1 Then
          For i = 0 To 7
            For j = 0 To 7
              If m_fontSprite.GetPixel(i + ox * 8, j + oy * 8).R > 0 Then
                For iIs = 0 To scale - 1
                  For js = 0 To scale - 1
                    Draw(x + sx + (i * scale) + iIs, y + sy + (j * scale) + js, col)
                  Next
                Next
              End If
            Next
          Next
        Else
          For i = 0 To 7
            For j = 0 To 7
              If m_fontSprite.GetPixel(i + ox * 8, j + oy * 8).R > 0 Then
                Draw(x + sx + i, y + sy + j, col)
              End If
            Next
          Next
        End If
        sx += 8 * scale
      End If

    Next

    SetPixelMode(m)

  End Sub

  Public Sub SetPixelMode(m As Pixel.Mode)
    m_pixelMode = m
  End Sub

  Protected ReadOnly Property GetPixelMode() As Pixel.Mode
    Get
      Return m_pixelMode
    End Get
  End Property

  Public Sub SetPixelMode(pixelMode As PixelModeDelegate)
    funcPixelMode = pixelMode
    m_pixelMode = Pixel.Mode.Custom
  End Sub

  Protected Sub SetPixelBlend(blend As Single)
    m_blendFactor = blend
    If m_blendFactor < 0.0F Then m_blendFactor = 0.0F
    If m_blendFactor > 1.0F Then m_blendFactor = 1.0F
  End Sub

  Protected Overridable Function OnUserCreate() As Boolean
    Return True
  End Function

  Protected MustOverride Function OnUserUpdate(elapsedTime As Single) As Boolean

  Private Protected Overridable Function OnUserDestroy() As Boolean
    Return True
  End Function

  Private Sub Pge_UpdateViewport()

    Dim ww = m_screenWidth * m_pixelWidth
    Dim wh = m_screenHeight * m_pixelHeight
    Dim wasp = CSng(ww / wh)

    m_viewW = m_windowWidth
    m_viewH = CInt(Fix(m_viewW / wasp))

    If m_viewH > m_windowHeight Then
      m_viewH = m_windowHeight
      m_viewW = CInt(Fix(m_viewH * wasp))
    End If

    m_viewX = (m_windowWidth - m_viewW) \ 2
    m_viewY = (m_windowHeight - m_viewH) \ 2

  End Sub

  Private Sub Pge_UpdateWindowSize(x As Integer, y As Integer)
    m_windowWidth = x
    m_windowHeight = y
    Pge_UpdateViewport()
  End Sub

  Private Sub Pge_UpdateMouseWheel(delta As Integer)
    m_mouseWheelDeltaCache += delta
  End Sub

  Public Property ShowEngineName As Boolean = True
  Public Property ShowIPS As Boolean = True

  Private Sub Pge_UpdateMouse(x As Integer, y As Integer)

    ' Mouse coords come in screen space
    ' But leave in pixel space

    ' Full Screen mode may have a weird viewport we must clamp to
    x -= m_viewX
    y -= m_viewY

    m_mousePosXcache = CInt(Fix((x / (m_windowWidth - (m_viewX * 2))) * m_screenWidth))
    m_mousePosYcache = CInt(Fix((y / (m_windowHeight - (m_viewY * 2))) * m_screenHeight))

    If m_mousePosXcache >= m_screenWidth Then m_mousePosXcache = m_screenWidth - 1
    If m_mousePosYcache >= m_screenHeight Then m_mousePosYcache = m_screenHeight - 1

    If m_mousePosXcache < 0 Then m_mousePosXcache = 0
    If m_mousePosYcache < 0 Then m_mousePosYcache = 0

  End Sub

  Private Sub EngineThread()

    If IsOSPlatform(Windows) Then
      ' Start OpenGL, the context is owned by the game thread
      Pge_OpenGLCreate_Windows()
      ' Create Screen Texture - disable filtering
      Win32.glEnable(GL_TEXTURE_2D)
      Win32.glGenTextures(1, m_glBuffer)
      Win32.glBindTexture(GL_TEXTURE_2D, m_glBuffer)
      Win32.glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST)
      Win32.glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST)
      Win32.glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL)
    ElseIf IsOSPlatform(Linux) Then
      ' Start OpenGL, the context is owned by the game thread
      Pge_OpenGLCreate_Linux()
      ' Create Screen Texture - disable filtering
      X11.glEnable(GL_TEXTURE_2D)
      X11.glGenTextures(1, m_glBuffer)
      X11.glBindTexture(GL_TEXTURE_2D, m_glBuffer)
      X11.glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST)
      X11.glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST)
      X11.glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL)
    End If

    'If True Then

    Dim pixels = GCHandle.Alloc(m_defaultDrawTarget.GetData, GCHandleType.Pinned)
    Try
      If IsOSPlatform(Linux) Then
        X11.glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, m_screenWidth, m_screenHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, pixels.AddrOfPinnedObject)
      Else
        Win32.glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, m_screenWidth, m_screenHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, pixels.AddrOfPinnedObject)
      End If
    Finally
      pixels.Free()
    End Try

    'Dim data = m_defaultDrawTarget.GetData
    'Dim sz = Marshal.SizeOf(GetType(Pixel))
    'Dim ts As Integer = sz * data.Length
    'Dim b(ts - 1) As Byte
    'Dim handle As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
    'Try
    '  Marshal.Copy(handle.AddrOfPinnedObject(), b, 0, ts)
    'Finally
    '  handle.Free()
    'End Try
    'Dim ptr = Marshal.AllocHGlobal(b.Length)
    'Marshal.Copy(b, 0, ptr, b.Length)
    'If IsOSPlatform(Linux) Then
    '  X11.glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, m_screenWidth, m_screenHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, ptr)
    'Else
    '  Win32.glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, m_screenWidth, m_screenHeight, 0, GL_RGBA, GL_UNSIGNED_BYTE, ptr)
    'End If
    'Marshal.FreeHGlobal(ptr)
    'End If

    ' Create user resources as part of this thread
    If Not OnUserCreate() Then
      Singleton.AtomActive = False
    End If

    Dim tp1 = Now()
    'Dim tp2 = Now()

    Dim xe = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(X11.XEvent)))

    Try

      While Singleton.AtomActive

        ' Run as fast as possible
        While Singleton.AtomActive

          ' Handle Timing
          Dim tp2 = Now
          Dim difference = tp2 - tp1
          tp1 = tp2

          Dim elapsedTime = CSng(difference.TotalSeconds)

          If IsOSPlatform(Linux) Then

            ' Handle Xlib Message Loop - we do this in the
            ' same thread that OpenGL was created so we don't
            ' need to worry too much about multithreading with X11
            While X11.XPending(pge_Display) > 0

              Dim status = X11.XNextEvent(pge_Display, xe)
              Dim xevent = CType(Marshal.PtrToStructure(xe, GetType(X11.XEvent)), X11.XEvent)

              Select Case xevent.Type
                Case X11.XEventType.Expose
                  Dim gwa As X11.XWindowAttributes
                  Dim rslt = X11.XGetWindowAttributes(pge_Display, pge_Window, gwa)
                  m_windowWidth = gwa.Width
                  m_windowHeight = gwa.Height
                  Pge_UpdateViewport()
                  X11.glClear(GL_COLOR_BUFFER_BIT) ' Thanks Benedani!
                Case X11.XEventType.ConfigureNotify
                  Dim xce = CType(Marshal.PtrToStructure(xe, GetType(X11.XConfigureEvent)), X11.XConfigureEvent)
                  m_windowWidth = xce.Width
                  m_windowHeight = xce.Height
                Case X11.XEventType.KeyPress
                  Dim key = CType(Marshal.PtrToStructure(xe, GetType(X11.XKeyEvent)), X11.XKeyEvent)
                  Dim sym = X11.XLookupKeysym(key, 0)
                  If MapKeys.ContainsKey(sym) Then
                    m_keyNewState(MapKeys(sym)) = True
                  End If
                'Dim sym As X11.XKeySym = X11.XLookupKeysym(xev.xkey, 0)
                'pKeyNewState(MapKeys(sym)) = True
                'Dim e As X11.XKeyEvent = CType(xev, X11.XKeyEvent) ' Because DragonEye loves numpads
                'X11.XLookupString(e, Nothing, 0, sym, Nothing)
                'pKeyNewState(MapKeys(sym)) = True
                Case X11.XEventType.KeyRelease
                  Dim key = CType(Marshal.PtrToStructure(xe, GetType(X11.XKeyEvent)), X11.XKeyEvent)
                  Dim sym = X11.XLookupKeysym(key, 0)
                  If MapKeys.ContainsKey(sym) Then
                    m_keyNewState(MapKeys(sym)) = False
                  End If
                'Dim sym As X11.XKeySym = X11.XLookupKeysym(xev.xkey, 0)
                'pKeyNewState(MapKeys(sym)) = False
                'Dim e As X11.XKeyEvent = CType(xev, X11.XKeyEvent)
                'X11.XLookupString(e, Nothing, 0, sym, Nothing)
                'pKeyNewState(MapKeys(sym)) = False
                Case X11.XEventType.ButtonPress
                  Dim btn = CType(Marshal.PtrToStructure(xe, GetType(X11.XButtonEvent)), X11.XButtonEvent)
                  Select Case btn.Button
                    Case 1 : m_mouseNewState(0) = True
                    Case 2 : m_mouseNewState(2) = True
                    Case 3 : m_mouseNewState(1) = True
                    Case 4 : Pge_UpdateMouseWheel(120)
                    Case 5 : Pge_UpdateMouseWheel(-120)
                    Case Else
                  End Select
                Case X11.XEventType.ButtonRelease
                  Dim btn = CType(Marshal.PtrToStructure(xe, GetType(X11.XButtonEvent)), X11.XButtonEvent)
                  Select Case btn.Button
                    Case 1 : m_mouseNewState(0) = False
                    Case 2 : m_mouseNewState(2) = False
                    Case 3 : m_mouseNewState(1) = False
                    Case Else
                  End Select
                Case X11.XEventType.MotionNotify
                  Dim motion = CType(Marshal.PtrToStructure(xe, GetType(X11.XMotionEvent)), X11.XMotionEvent)
                  Pge_UpdateMouse(motion.X, motion.Y)
                Case X11.XEventType.FocusIn
                  m_hasInputFocus = True
                Case X11.XEventType.FocusOut
                  m_hasInputFocus = False
                Case X11.XEventType.ClientMessage
                  Singleton.AtomActive = False
                Case Else
              End Select
            End While

          End If

          ' Handle User Input - Keyboard
          For i = 0 To 255
            m_keyboardState(i).Pressed = False
            m_keyboardState(i).Released = False
            If m_keyNewState(i) <> m_keyOldState(i) Then
              If m_keyNewState(i) Then
                m_keyboardState(i).Pressed = Not m_keyboardState(i).Held
                m_keyboardState(i).Held = True
              Else
                m_keyboardState(i).Released = True
                m_keyboardState(i).Held = False
              End If
            End If
            m_keyOldState(i) = m_keyNewState(i)
            ' Handle Keyboard Repeat (Rate)
            ' Setting to approximately 2-3 10th's of a second
            ' delay (for the initial delay) with a little less
            ' than 10th of a second for continued repeats...
            If m_keyboardState(i).Pressed OrElse m_keyboardState(i).Held Then
              m_keyboardState(i).ElapsedTime += elapsedTime
              If m_keyboardState(i).Held AndAlso
                 m_keyboardState(i).ElapsedTime > 0.275 Then
                m_keyboardState(i).ElapsedTime -= 0.075!
                m_keyboardState(i).Pressed = True
              End If
            Else
              m_keyboardState(i).ElapsedTime = 0
            End If
          Next

          ' Handle User Input - Mouse
          For i = 0 To 4
            m_mouseState(i).Pressed = False
            m_mouseState(i).Released = False
            If m_mouseNewState(i) <> m_mouseOldState(i) Then
              If m_mouseNewState(i) Then
                m_mouseState(i).Pressed = Not m_mouseState(i).Held
                m_mouseState(i).Held = True
              Else
                m_mouseState(i).Released = True
                m_mouseState(i).Held = False
              End If
            End If
            m_mouseOldState(i) = m_mouseNewState(i)
          Next

          ' Cache mouse coordinates so they remain
          ' consistent during frame
          m_mousePosX = m_mousePosXcache
          m_mousePosY = m_mousePosYcache

          m_mouseWheelDelta = m_mouseWheelDeltaCache
          m_mouseWheelDeltaCache = 0

          ' Handle F11 key
          If GetKey(Key.F11).Pressed Then
            ToggleFullScreen()
          End If

          ' Handle Frame Update
          If Not OnUserUpdate(elapsedTime) Then
            Singleton.AtomActive = False
          End If

          'If True Then

          If IsOSPlatform(Windows) Then
            Win32.glViewport(m_viewX, m_viewY, m_viewW, m_viewH)
            pixels = GCHandle.Alloc(m_defaultDrawTarget.GetData, GCHandleType.Pinned)
            Try
              Win32.glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_screenWidth, m_screenHeight, GL_RGBA, GL_UNSIGNED_BYTE, pixels.AddrOfPinnedObject())
            Finally
              pixels.Free() : pixels = Nothing
            End Try
            ' Display texture on screen
            Win32.glBegin(GL_QUADS)
            Win32.glTexCoord2f(0.0, 1.0) : Win32.glVertex3f(-1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
            Win32.glTexCoord2f(0.0, 0.0) : Win32.glVertex3f(-1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
            Win32.glTexCoord2f(1.0, 0.0) : Win32.glVertex3f(1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
            Win32.glTexCoord2f(1.0, 1.0) : Win32.glVertex3f(1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
            Win32.glEnd()
            ' Preset Graphics to screen
            Win32.SwapBuffers(m_glDeviceContext)
          Else
            X11.glViewport(m_viewX, m_viewY, m_viewW, m_viewH)
            Dim pixel = GCHandle.Alloc(m_defaultDrawTarget.GetData, GCHandleType.Pinned)
            Try
              X11.glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_screenWidth, m_screenHeight, GL_RGBA, GL_UNSIGNED_BYTE, pixel.AddrOfPinnedObject())
            Finally
              pixel.Free()
            End Try
            ' Display texture on screen
            X11.glBegin(GL_QUADS)
            X11.glTexCoord2f(0.0, 1.0) : X11.glVertex3f(-1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
            X11.glTexCoord2f(0.0, 0.0) : X11.glVertex3f(-1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
            X11.glTexCoord2f(1.0, 0.0) : X11.glVertex3f(1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
            X11.glTexCoord2f(1.0, 1.0) : X11.glVertex3f(1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
            X11.glEnd()
            ' Preset Graphics to screen
            X11.glXSwapBuffers(pge_Display, pge_Window)
          End If

          'Else

          '  ' Display Graphics
          '  If IsOSPlatform(Windows) Then
          '    Win32.glViewport(m_viewX, m_viewY, m_viewW, m_viewH)
          '  Else
          '    X11.glViewport(m_viewX, m_viewY, m_viewW, m_viewH)
          '  End If

          '  ' Copy pixel array into texture
          '  Dim data = m_defaultDrawTarget.GetData
          '  Dim sz = Marshal.SizeOf(GetType(Pixel))
          '  Dim ts As Integer = sz * data.Length
          '  Dim b(ts - 1) As Byte
          '  Dim handle As GCHandle = GCHandle.Alloc(data, GCHandleType.Pinned)
          '  Try
          '    Marshal.Copy(handle.AddrOfPinnedObject(), b, 0, ts)
          '  Finally
          '    handle.Free()
          '  End Try
          '  Dim ptr = Marshal.AllocHGlobal(b.Length)
          '  Marshal.Copy(b, 0, ptr, b.Length)
          '  If IsOSPlatform(Windows) Then
          '    Win32.glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_screenWidth, m_screenHeight, GL_RGBA, GL_UNSIGNED_BYTE, ptr)
          '  Else
          '    X11.glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, m_screenWidth, m_screenHeight, GL_RGBA, GL_UNSIGNED_BYTE, ptr)
          '  End If
          '  Marshal.FreeHGlobal(ptr)

          '  If IsOSPlatform(Windows) Then
          '    ' Display texture on screen
          '    Win32.glBegin(GL_QUADS)
          '    Win32.glTexCoord2f(0.0, 1.0) : Win32.glVertex3f(-1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
          '    Win32.glTexCoord2f(0.0, 0.0) : Win32.glVertex3f(-1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
          '    Win32.glTexCoord2f(1.0, 0.0) : Win32.glVertex3f(1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
          '    Win32.glTexCoord2f(1.0, 1.0) : Win32.glVertex3f(1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
          '    Win32.glEnd()
          '    ' Preset Graphics to screen
          '    Win32.SwapBuffers(m_glDeviceContext)
          '  ElseIf IsOSPlatform(Linux) Then
          '    ' Display texture on screen
          '    X11.glBegin(GL_QUADS)
          '    X11.glTexCoord2f(0.0, 1.0) : X11.glVertex3f(-1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
          '    X11.glTexCoord2f(0.0, 0.0) : X11.glVertex3f(-1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
          '    X11.glTexCoord2f(1.0, 0.0) : X11.glVertex3f(1.0F + m_subPixelOffsetX, 1.0F + m_subPixelOffsetY, 0.0F)
          '    X11.glTexCoord2f(1.0, 1.0) : X11.glVertex3f(1.0F + m_subPixelOffsetX, -1.0F + m_subPixelOffsetY, 0.0F)
          '    X11.glEnd()
          '    ' Preset Graphics to screen
          '    X11.glXSwapBuffers(pge_Display, pge_Window)
          '  End If

          'End If

          ' Update Title Bar
          m_frameTimer += elapsedTime
          m_frameCount += 1

          If m_frameTimer >= 1.0F Then

            If m_totalFrameCount + m_frameCount > Integer.MaxValue Then
              m_totalFrameCount = m_frameCount
              'm_totalFrames = 1
            Else
              m_totalFrameCount += m_frameCount
              'm_totalFrames += 1
            End If

            'Dim avg = m_totalFrameCount \ m_totalFrames

            m_frameTimer -= 1.0F
            Dim title As String = Nothing
            'If Not ShowEngineName Then title = If(ShowIPS, $"{AppName} - IPS: {m_frameCount} ({avg})", AppName)
            'If ShowEngineName Then title = If(ShowIPS, $"vbPixelGameEngine v0.0.1 - {AppName} - IPS: {m_frameCount} ({avg})", $"vbPixelGameEngine v0.0.1 - {AppName}")
            If Not ShowEngineName Then title = If(ShowIPS, $"{AppName} - IPS: {m_frameCount}", AppName)
            If ShowEngineName Then title = If(ShowIPS, $"vbPixelGameEngine v0.0.1 - {AppName} - IPS: {m_frameCount}", $"vbPixelGameEngine v0.0.1 - {AppName}")
            m_frameCount = 0

            If IsOSPlatform(Windows) Then
              Win32.SetWindowText(m_hWnd, title)
            ElseIf IsOSPlatform(Linux) Then
              X11.XStoreName(pge_Display, pge_Window, title)
            End If

          End If

        End While

        ' Allow the user to free resources if they have overridden the destroy function
        If OnUserDestroy() Then
          ' User has permitted destroy, so exit and clean up
        Else
          ' User denied destroy for some reason, so continue running
          Singleton.AtomActive = True
        End If

      End While

    Finally
      Marshal.FreeHGlobal(xe)
    End Try

    If IsOSPlatform(Windows) Then
      Win32.wglDeleteContext(m_glRenderContext)
      Win32.PostMessage(m_hWnd, WM_DESTROY, IntPtr.Zero, IntPtr.Zero)
    ElseIf IsOSPlatform(Linux) Then
      Dim hrslt = X11.glXMakeCurrent(pge_Display, IntPtr.Zero, Nothing)
      hrslt = X11.glXDestroyContext(pge_Display, m_glDeviceContext)
      hrslt = X11.XDestroyWindow(pge_Display, pge_Window)
      X11.XCloseDisplay(pge_Display)
    End If

  End Sub

  Private Sub Pge_ConstructFontSheet()

    Dim data As String = ""
    data &= "?Q`0001oOch0o01o@F40o0<AGD4090LAGD<090@A7ch0?00O7Q`0600>00000000"
    data &= "O000000nOT0063Qo4d8>?7a14Gno94AA4gno94AaOT0>o3`oO400o7QN00000400"
    data &= "Of80001oOg<7O7moBGT7O7lABET024@aBEd714AiOdl717a_=TH013Q>00000000"
    data &= "720D000V?V5oB3Q_HdUoE7a9@DdDE4A9@DmoE4A;Hg]oM4Aj8S4D84@`00000000"
    data &= "OaPT1000Oa`^13P1@AI[?g`1@A=[OdAoHgljA4Ao?WlBA7l1710007l100000000"
    data &= "ObM6000oOfMV?3QoBDD`O7a0BDDH@5A0BDD<@5A0BGeVO5ao@CQR?5Po00000000"
    data &= "Oc``000?Ogij70PO2D]??0Ph2DUM@7i`2DTg@7lh2GUj?0TO0C1870T?00000000"
    data &= "70<4001o?P<7?1QoHg43O;`h@GT0@:@LB@d0>:@hN@L0@?aoN@<0O7ao0000?000"
    data &= "OcH0001SOglLA7mg24TnK7ln24US>0PL24U140PnOgl0>7QgOcH0K71S0000A000"
    data &= "00H00000@Dm1S007@DUSg00?OdTnH7YhOfTL<7Yh@Cl0700?@Ah0300700000000"
    data &= "<008001QL00ZA41a@6HnI<1i@FHLM81M@@0LG81?O`0nC?Y7?`0ZA7Y300080000"
    data &= "O`082000Oh0827mo6>Hn?Wmo?6HnMb11MP08@C11H`08@FP0@@0004@000000000"
    data &= "00P00001Oab00003OcKP0006@6=PMgl<@440MglH@000000`@000001P00000000"
    data &= "Ob@8@@00Ob@8@Ga13R@8Mga172@8?PAo3R@827QoOb@820@0O`0007`0000007P0"
    data &= "O`000P08Od400g`<3V=P0G`673IP0`@3>1`00P@6O`P00g`<O`000GP800000000"
    data &= "?P9PL020O`<`N3R0@E4HC7b0@ET<ATB0@@l6C4B0O`H3N7b0?P01L3R000000020"

    m_fontSprite = New Sprite(128, 48)

    Dim px = 0, py = 0
    For b = 0 To 1023 Step 4

      Dim sym1 = AscW(data(b + 0)) - 48
      Dim sym2 = AscW(data(b + 1)) - 48
      Dim sym3 = AscW(data(b + 2)) - 48
      Dim sym4 = AscW(data(b + 3)) - 48
      Dim r = sym1 << 18 Or sym2 << 12 Or sym3 << 6 Or sym4

      For i = 0 To 23
        Dim k = If((r And (1 << i)) <> 0, 255, 0)
        m_fontSprite.SetPixel(px, py, New Pixel(k, k, k, k))
        If System.Threading.Interlocked.Increment(py) = 48 Then
          px += 1 : py = 0
        End If
      Next

    Next

  End Sub

#Region "Windows"

  Private Function WindowCreateWindows() As IntPtr
    Dim wc As New Win32.WNDCLASS With {
      .hIcon = Win32.LoadIcon(IntPtr.Zero, IDI_APPLICATION),
      .hCursor = Win32.LoadCursor(IntPtr.Zero, IDC_ARROW),
      .Style = CS_HREDRAW Or CS_VREDRAW Or CS_OWNDC,
      .hInstance = Win32.GetModuleHandle(Nothing),
      .WndProc = Marshal.GetFunctionPointerForDelegate(m_delegWndProc),
      .ClsExtra = 0,
      .WndExtra = 0,
      .MenuName = Nothing,
      .hBackground = CType(COLOR_BACKGROUND, IntPtr) + 1, 'Nothing
      .ClassName = "VB_PIXEL_GAME_ENGINE"}

    Dim atom = Win32.RegisterClass(wc)
    If atom = 0 Then
      Dim er = Win32.GetLastError
      Return IntPtr.Zero
    End If

    m_windowWidth = m_screenWidth * m_pixelWidth
    m_windowHeight = m_screenHeight * m_pixelHeight

    ' Define window furniture
    Dim dwExStyle = WS_EX_APPWINDOW Or WS_EX_WINDOWEDGE
    Dim dwStyle = WS_CAPTION Or WS_SYSMENU Or WS_VISIBLE Or WS_THICKFRAME Or WS_MINIMIZEBOX Or WS_MAXIMIZEBOX

    Dim nCosmeticOffset = 30
    m_viewW = m_windowWidth
    m_viewH = m_windowHeight

    ' Handle Fullscreen
    If m_fullScreen Then
      dwExStyle = 0
      dwStyle = WS_VISIBLE Or WS_POPUP
      nCosmeticOffset = 0
      Dim hmon = Win32.MonitorFromWindow(m_hWnd, MONITOR_DEFAULTTONEAREST)
      Dim mi = New Win32.MONITORINFO With {.cbSize = Marshal.SizeOf(GetType(Win32.MONITORINFO))}
      If Not Win32.GetMonitorInfo(hmon, mi) Then Return Nothing
      m_windowWidth = mi.rcMonitor.Right
      m_windowHeight = mi.rcMonitor.Bottom
    End If

    Pge_UpdateViewport()

    ' Keep client size as requested
    Dim rWndRect = New Win32.RECT With {.Left = 0, .Top = 0, .Right = m_windowWidth, .Bottom = m_windowHeight}
    Win32.AdjustWindowRectEx(rWndRect, dwStyle, False, dwExStyle)
    Dim width = rWndRect.Right - rWndRect.Left
    Dim height = rWndRect.Bottom - rWndRect.Top

    'Singleton.Pge = Me
    m_hWnd = Win32.CreateWindowEx(dwExStyle, atom, "", dwStyle,
                                  nCosmeticOffset, nCosmeticOffset, width, height, Nothing, Nothing,
                                  Win32.GetModuleHandle(Nothing), IntPtr.Zero)

    'Dim tme = New TRACKMOUSEEVENTSTRUCT
    'tme.cbSize = Marshal.SizeOf(GetType(TRACKMOUSEEVENTSTRUCT))
    'tme.dwFlags = TME_LEAVE
    'tme.hWnd = pge_hWnd
    'TrackMouseEvent(tme)

    ' Create Keyboard Mapping
    Singleton.MapKeys(&H0) = Key.NONE
    Singleton.MapKeys(&H41) = Key.A : Singleton.MapKeys(&H42) = Key.B : Singleton.MapKeys(&H43) = Key.C : Singleton.MapKeys(&H44) = Key.D : Singleton.MapKeys(&H45) = Key.E
    Singleton.MapKeys(&H46) = Key.F : Singleton.MapKeys(&H47) = Key.G : Singleton.MapKeys(&H48) = Key.H : Singleton.MapKeys(&H49) = Key.I : Singleton.MapKeys(&H4A) = Key.J
    Singleton.MapKeys(&H4B) = Key.K : Singleton.MapKeys(&H4C) = Key.L : Singleton.MapKeys(&H4D) = Key.M : Singleton.MapKeys(&H4E) = Key.N : Singleton.MapKeys(&H4F) = Key.O
    Singleton.MapKeys(&H50) = Key.P : Singleton.MapKeys(&H51) = Key.Q : Singleton.MapKeys(&H52) = Key.R : Singleton.MapKeys(&H53) = Key.S : Singleton.MapKeys(&H54) = Key.T
    Singleton.MapKeys(&H55) = Key.U : Singleton.MapKeys(&H56) = Key.V : Singleton.MapKeys(&H57) = Key.W : Singleton.MapKeys(&H58) = Key.X : Singleton.MapKeys(&H59) = Key.Y
    Singleton.MapKeys(&H5A) = Key.Z

    Singleton.MapKeys(VK_F1) = Key.F1 : Singleton.MapKeys(VK_F2) = Key.F2 : Singleton.MapKeys(VK_F3) = Key.F3 : Singleton.MapKeys(VK_F4) = Key.F4
    Singleton.MapKeys(VK_F5) = Key.F5 : Singleton.MapKeys(VK_F6) = Key.F6 : Singleton.MapKeys(VK_F7) = Key.F7 : Singleton.MapKeys(VK_F8) = Key.F8
    Singleton.MapKeys(VK_F9) = Key.F9 : Singleton.MapKeys(VK_F10) = Key.F10 : Singleton.MapKeys(VK_F11) = Key.F11 : Singleton.MapKeys(VK_F12) = Key.F12

    Singleton.MapKeys(VK_DOWN) = Key.DOWN : Singleton.MapKeys(VK_LEFT) = Key.LEFT : Singleton.MapKeys(VK_RIGHT) = Key.RIGHT : Singleton.MapKeys(VK_UP) = Key.UP
    Singleton.MapKeys(VK_RETURN) = Key.ENTER 'mapKeys(VK_RETURN) = Key.RETURN

    Singleton.MapKeys(VK_BACK) = Key.BACK : Singleton.MapKeys(VK_ESCAPE) = Key.ESCAPE : Singleton.MapKeys(VK_RETURN) = Key.ENTER : Singleton.MapKeys(VK_PAUSE) = Key.PAUSE
    Singleton.MapKeys(VK_SCROLL) = Key.SCROLL : Singleton.MapKeys(VK_TAB) = Key.TAB : Singleton.MapKeys(VK_DELETE) = Key.DEL : Singleton.MapKeys(VK_HOME) = Key.HOME
    Singleton.MapKeys(VK_END) = Key.END : Singleton.MapKeys(VK_PRIOR) = Key.PGUP : Singleton.MapKeys(VK_NEXT) = Key.PGDN : Singleton.MapKeys(VK_INSERT) = Key.INS
    Singleton.MapKeys(VK_SHIFT) = Key.SHIFT : Singleton.MapKeys(VK_CONTROL) = Key.CTRL
    Singleton.MapKeys(VK_SPACE) = Key.SPACE
    Singleton.MapKeys(VK_MENU) = Key.ALT

    Singleton.MapKeys(&H30) = Key.K0 : Singleton.MapKeys(&H31) = Key.K1 : Singleton.MapKeys(&H32) = Key.K2 : Singleton.MapKeys(&H33) = Key.K3 : Singleton.MapKeys(&H34) = Key.K4
    Singleton.MapKeys(&H35) = Key.K5 : Singleton.MapKeys(&H36) = Key.K6 : Singleton.MapKeys(&H37) = Key.K7 : Singleton.MapKeys(&H38) = Key.K8 : Singleton.MapKeys(&H39) = Key.K9

    Singleton.MapKeys(VK_NUMPAD0) = Key.NP0 : Singleton.MapKeys(VK_NUMPAD1) = Key.NP1 : Singleton.MapKeys(VK_NUMPAD2) = Key.NP2 : Singleton.MapKeys(VK_NUMPAD3) = Key.NP3 : Singleton.MapKeys(VK_NUMPAD4) = Key.NP4
    Singleton.MapKeys(VK_NUMPAD5) = Key.NP5 : Singleton.MapKeys(VK_NUMPAD6) = Key.NP6 : Singleton.MapKeys(VK_NUMPAD7) = Key.NP7 : Singleton.MapKeys(VK_NUMPAD8) = Key.NP8 : Singleton.MapKeys(VK_NUMPAD9) = Key.NP9
    Singleton.MapKeys(VK_MULTIPLY) = Key.NP_MUL : Singleton.MapKeys(VK_ADD) = Key.NP_ADD : Singleton.MapKeys(VK_DIVIDE) = Key.NP_DIV : Singleton.MapKeys(VK_SUBTRACT) = Key.NP_SUB : Singleton.MapKeys(VK_DECIMAL) = Key.NP_DECIMAL

    Singleton.MapKeys(VK_OEM_1) = Key.OEM_1
    Singleton.MapKeys(VK_OEM_COMMA) = Key.COMMA
    Singleton.MapKeys(VK_OEM_MINUS) = Key.MINUS
    Singleton.MapKeys(VK_OEM_PLUS) = Key.EQUALS
    Singleton.MapKeys(VK_OEM_PERIOD) = Key.PERIOD
    Singleton.MapKeys(VK_OEM_2) = Key.OEM_2
    Singleton.MapKeys(VK_OEM_3) = Key.OEM_3
    Singleton.MapKeys(VK_OEM_4) = Key.OEM_4
    Singleton.MapKeys(VK_OEM_5) = Key.OEM_5
    Singleton.MapKeys(VK_OEM_6) = Key.OEM_6
    Singleton.MapKeys(VK_OEM_7) = Key.OEM_7

    Return m_hWnd

  End Function

  Private Function Pge_OpenGLCreate_Windows() As Boolean

    ' Create Device Context
    m_glDeviceContext = Win32.GetDC(m_hWnd)
    Dim pfd As New Win32.PIXELFORMATDESCRIPTOR With {.nSize = CUShort(Marshal.SizeOf(GetType(Win32.PIXELFORMATDESCRIPTOR))),
                                           .nVersion = 1,
                                           .dwFlags = PFD_DRAW_TO_WINDOW Or PFD_SUPPORT_OPENGL Or PFD_DOUBLEBUFFER,
                                           .iPixelType = PFD_TYPE_RGBA,
                                           .cColorBits = 32,
                                           .cRedBits = 0,
                                           .cRedShift = 0,
                                           .cGreenBits = 0,
                                           .cGreenShift = 0,
                                           .cBlueBits = 0,
                                           .cBlueShift = 0,
                                           .cAlphaBits = 0,
                                           .cAlphaShift = 0,
                                           .cAccumBits = 0,
                                           .cAccumRedBits = 0,
                                           .cAccumGreenBits = 0,
                                           .cAccumBlueBits = 0,
                                           .cAccumAlphaBits = 0,
                                           .cDepthBits = 24, '0,
                                           .cStencilBits = 8, '0,
                                           .cAuxBuffers = 0,
                                           .iLayerType = PFD_MAIN_PLANE,
                                           .bReserved = 0,
                                           .dwLayerMask = 0,
                                           .dwVisibleMask = 0,
                                           .dwDamageMask = 0}

    Dim pf = Win32.ChoosePixelFormat(m_glDeviceContext, pfd) : If pf = 0 Then Return False
    Dim unused = Win32.SetPixelFormat(m_glDeviceContext, pf, pfd)

    m_glRenderContext = Win32.wglCreateContext(m_glDeviceContext) : If m_glRenderContext = IntPtr.Zero Then Return False
    Dim unused2 = Win32.wglMakeCurrent(m_glDeviceContext, m_glRenderContext)

    Win32.glViewport(m_viewX, m_viewY, m_viewW, m_viewH)

    ' Remove Frame cap
    wglSwapInterval = CType(Marshal.GetDelegateForFunctionPointer(Win32.WglGetProcAddress("wglSwapIntervalEXT"), GetType(wglSwapInterval_t)), wglSwapInterval_t)
    If wglSwapInterval IsNot Nothing AndAlso Not m_enableVSYNC Then wglSwapInterval(0)

    Return True

  End Function

  ' Windows Event Handler
  Private Shared Function Pge_WindowEvent(hWnd As IntPtr, uMsg As UInt32, wParam As IntPtr, lParam As IntPtr) As IntPtr
    'Static sge As PixelGameEngine = Nothing
    Select Case uMsg
      Case WM_CREATE
        'NOTE: swapped out trying to get a reference to the PGE by passing it through CreateWindowEx and instead
        '      modified the code (at CreateWindowEx) so that a shared reference to self (Me) is created there.
        'Dim createStruct = Marshal.PtrToStructure(Of CREATESTRUCT)(lParam)
        'sge = CType(Marshal.PtrToStructure(createStruct.lpCreateParams, GetType(PixelGameEngine)), PixelGameEngine)
        Return IntPtr.Zero
      Case WM_MOUSEMOVE
        If Not Singleton.Pge.m_hasMouseFocus Then
          ' Set the first time we detect mouse movement.
          Singleton.Pge.m_hasMouseFocus = True
        End If
        Dim v = CInt(lParam)
        Dim x = v And &HFFFF
        Dim y = (v >> 16) And &HFFFF
        Dim ix = BitConverter.ToInt16(BitConverter.GetBytes(x), 0)
        Dim iy = BitConverter.ToInt16(BitConverter.GetBytes(y), 0)
        Singleton.Pge.Pge_UpdateMouse(ix, iy)
        Return IntPtr.Zero
      Case WM_SIZE
        Dim v = CInt(lParam)
        Singleton.Pge.Pge_UpdateWindowSize(v And &HFFFF, (v >> 16) And &HFFFF)
        Return IntPtr.Zero
      Case WM_MOUSEWHEEL
        Singleton.Pge.Pge_UpdateMouseWheel(GET_WHEEL_DELTA_WPARAM(wParam))
        Return IntPtr.Zero
      Case WM_MOUSELEAVE
        'TODO: WM_MOUSELEAVE is working *once*, not sure why...
        Singleton.Pge.m_hasMouseFocus = False
        Return IntPtr.Zero
      Case WM_SETFOCUS
        Singleton.Pge.m_hasInputFocus = True
        Return IntPtr.Zero
      Case WM_KILLFOCUS
        Singleton.Pge.m_hasInputFocus = False
        Return IntPtr.Zero
      Case WM_SYSCOMMAND
        If (wParam.ToInt32 And &HFFF0) = SC_KEYMENU Then
          Return IntPtr.Zero
        End If
      Case WM_SYSKEYDOWN
        Dim value As Integer
        If MapKeys.TryGetValue(wParam.ToInt32(), value) Then
          Pge.m_keyNewState(value) = True
        End If
        Return IntPtr.Zero
      Case WM_SYSKEYUP
        Dim value As Integer
        If MapKeys.TryGetValue(wParam.ToInt32(), value) Then
          Pge.m_keyNewState(value) = False
        End If
        Return IntPtr.Zero
      Case WM_KEYDOWN
        Dim value As Integer
        If MapKeys.TryGetValue(wParam.ToInt32(), value) Then
          Pge.m_keyNewState(value) = True
        End If
        Return IntPtr.Zero
      Case WM_KEYUP
        Dim value As Integer
        If MapKeys.TryGetValue(wParam.ToInt32(), value) Then
          Pge.m_keyNewState(value) = False
        End If
        Return IntPtr.Zero
      Case WM_LBUTTONDOWN
        Singleton.Pge.m_mouseNewState(0) = True
        Return IntPtr.Zero
      Case WM_LBUTTONUP
        Singleton.Pge.m_mouseNewState(0) = False
        Return IntPtr.Zero
      Case WM_RBUTTONDOWN
        Singleton.Pge.m_mouseNewState(1) = True
        Return IntPtr.Zero
      Case WM_RBUTTONUP
        Singleton.Pge.m_mouseNewState(1) = False
        Return IntPtr.Zero
      Case WM_MBUTTONDOWN
        Singleton.Pge.m_mouseNewState(2) = True
        Return IntPtr.Zero
      Case WM_MBUTTONUP
        Singleton.Pge.m_mouseNewState(2) = False
        Return IntPtr.Zero
      Case WM_CLOSE
        Singleton.AtomActive = False
        Return IntPtr.Zero
      Case WM_DESTROY
        Win32.PostQuitMessage(0)
        Return IntPtr.Zero
    End Select
    Return Win32.DefWindowProc(hWnd, uMsg, wParam, lParam)
  End Function

#End Region

#Region "Linux"

  ' Do the Linux stuff!
  Private Function WindowCreateLinux() As IntPtr

    Dim rslt = X11.XInitThreads()

    ' Grab the deafult display and window
    pge_Display = X11.XOpenDisplay(Nothing)
    pge_WindowRoot = X11.XDefaultRootWindow(pge_Display)

    ' Based on the display capabilities, configure the appearance of the window
    Dim pge_GLAttribs() = {GLX_RGBA, GLX_DEPTH_SIZE, 24, GLX_DOUBLEBUFFER, None}
    pge_VisualInfo = X11.glXChooseVisual(pge_Display, 0, pge_GLAttribs)
    Dim vi = Marshal.PtrToStructure(Of X11.XVisualInfo)(pge_VisualInfo)
    pge_ColorMap = X11.XCreateColormap(pge_Display, pge_WindowRoot, vi.Visual, AllocNone)
    pge_SetWindowAttribs.Colormap = pge_ColorMap

    ' Register which events we are interested in receiving
    pge_SetWindowAttribs.EventMask = X11.XEventMask.ExposureMask Or
                                     X11.XEventMask.KeyPressMask Or
                                     X11.XEventMask.KeyReleaseMask Or
                                     X11.XEventMask.ButtonPressMask Or
                                     X11.XEventMask.ButtonReleaseMask Or
                                     X11.XEventMask.PointerMotionMask Or
                                     X11.XEventMask.FocusChangeMask Or
                                     X11.XEventMask.StructureNotifyMask

    ' Create the window
    pge_Window = X11.XCreateWindow(pge_Display, pge_WindowRoot, 30, 30, m_screenWidth * m_pixelWidth, m_screenHeight * m_pixelHeight, 0, vi.Depth, InputOutput, vi.Visual, X11.XWindowAttributeFlags.CWColormap Or X11.XWindowAttributeFlags.CWEventMask, pge_SetWindowAttribs)

    Dim wmDelete = X11.XInternAtom(pge_Display, "WM_DELETE_WINDOW", True)
    X11.XSetWMProtocols(pge_Display, pge_Window, {wmDelete}, 1)

    X11.XMapWindow(pge_Display, pge_Window)
    X11.XStoreName(pge_Display, pge_Window, "vbPixelGameEngine")

    If m_fullScreen Then ' Thanks DragonEye, again :D
      Dim wm_state = X11.XInternAtom(pge_Display, "_NET_WM_STATE", False)
      Dim fullscreen = CByte(X11.XInternAtom(pge_Display, "_NET_WM_STATE_FULLSCREEN", False))
      Dim xev As X11.XClientMessageEvent ' = Nothing
      xev.Type = X11.XEventType.ClientMessage
      xev.Window = pge_Window
      xev.Message_Type = CULng(wm_state)
      xev.Format = 32
      xev.L0 = If(m_fullScreen, 1, 0) ' the action (0: off, 1: on, 2: toggle)
      xev.L1 = fullscreen ' first property to alter
      xev.L2 = 0 ' second property to alter
      xev.L3 = 0 ' source indication
      X11.XMapWindow(pge_Display, pge_Window)
      Dim eventSend = Marshal.AllocHGlobal(Marshal.SizeOf(xev))
      Marshal.StructureToPtr(xev, eventSend, False)
      rslt = X11.XSendEvent(pge_Display, X11.XDefaultRootWindow(pge_Display), False, X11.XEventMask.SubstructureRedirectMask Or X11.XEventMask.SubstructureNotifyMask, eventSend)
      Marshal.FreeHGlobal(eventSend)
      rslt = X11.XFlush(pge_Display)
      Dim gwa As X11.XWindowAttributes
      rslt = X11.XGetWindowAttributes(pge_Display, pge_Window, gwa)
      m_windowWidth = gwa.Width
      m_windowHeight = gwa.Height
      Pge_UpdateViewport()
    End If

    ' Create Keyboard Mapping
    Singleton.MapKeys(&H0) = Key.NONE
    Singleton.MapKeys(&H61) = Key.A : Singleton.MapKeys(&H62) = Key.B : Singleton.MapKeys(&H63) = Key.C : Singleton.MapKeys(&H64) = Key.D : Singleton.MapKeys(&H65) = Key.E
    Singleton.MapKeys(&H66) = Key.F : Singleton.MapKeys(&H67) = Key.G : Singleton.MapKeys(&H68) = Key.H : Singleton.MapKeys(&H69) = Key.I : Singleton.MapKeys(&H6A) = Key.J
    Singleton.MapKeys(&H6B) = Key.K : Singleton.MapKeys(&H6C) = Key.L : Singleton.MapKeys(&H6D) = Key.M : Singleton.MapKeys(&H6E) = Key.N : Singleton.MapKeys(&H6F) = Key.O
    Singleton.MapKeys(&H70) = Key.P : Singleton.MapKeys(&H71) = Key.Q : Singleton.MapKeys(&H72) = Key.R : Singleton.MapKeys(&H73) = Key.S : Singleton.MapKeys(&H74) = Key.T
    Singleton.MapKeys(&H75) = Key.U : Singleton.MapKeys(&H76) = Key.V : Singleton.MapKeys(&H77) = Key.W : Singleton.MapKeys(&H78) = Key.X : Singleton.MapKeys(&H79) = Key.Y
    Singleton.MapKeys(&H7A) = Key.Z

    Singleton.MapKeys(XK_F1) = Key.F1 : Singleton.MapKeys(XK_F2) = Key.F2 : Singleton.MapKeys(XK_F3) = Key.F3 : Singleton.MapKeys(XK_F4) = Key.F4
    Singleton.MapKeys(XK_F5) = Key.F5 : Singleton.MapKeys(XK_F6) = Key.F6 : Singleton.MapKeys(XK_F7) = Key.F7 : Singleton.MapKeys(XK_F8) = Key.F8
    Singleton.MapKeys(XK_F9) = Key.F9 : Singleton.MapKeys(XK_F10) = Key.F10 : Singleton.MapKeys(XK_F11) = Key.F11 : Singleton.MapKeys(XK_F12) = Key.F12

    Singleton.MapKeys(XK_Down) = Key.DOWN : Singleton.MapKeys(XK_Left) = Key.LEFT : Singleton.MapKeys(XK_Right) = Key.RIGHT : Singleton.MapKeys(XK_Up) = Key.UP
    Singleton.MapKeys(XK_KP_Enter) = Key.ENTER : Singleton.MapKeys(XK_Return) = Key.ENTER

    Singleton.MapKeys(XK_BackSpace) = Key.BACK : Singleton.MapKeys(XK_Escape) = Key.ESCAPE : Singleton.MapKeys(XK_Linefeed) = Key.ENTER : Singleton.MapKeys(XK_Pause) = Key.PAUSE
    Singleton.MapKeys(XK_Scroll_Lock) = Key.SCROLL : Singleton.MapKeys(XK_Tab) = Key.TAB : Singleton.MapKeys(XK_Delete) = Key.DEL : Singleton.MapKeys(XK_Home) = Key.HOME
    Singleton.MapKeys(XK_End) = Key.END : Singleton.MapKeys(XK_Page_Up) = Key.PGUP : Singleton.MapKeys(XK_Page_Down) = Key.PGDN : Singleton.MapKeys(XK_Insert) = Key.INS
    Singleton.MapKeys(XK_Shift_L) = Key.SHIFT : Singleton.MapKeys(XK_Shift_R) = Key.SHIFT : Singleton.MapKeys(XK_Control_L) = Key.CTRL : Singleton.MapKeys(XK_Control_R) = Key.CTRL
    Singleton.MapKeys(XK_Alt_L) = Key.ALT : Singleton.MapKeys(XK_Alt_R) = Key.ALT
    Singleton.MapKeys(XK_space) = Key.SPACE

    Singleton.MapKeys(XK_0) = Key.K0 : Singleton.MapKeys(XK_1) = Key.K1 : Singleton.MapKeys(XK_2) = Key.K2 : Singleton.MapKeys(XK_3) = Key.K3 : Singleton.MapKeys(XK_4) = Key.K4
    Singleton.MapKeys(XK_5) = Key.K5 : Singleton.MapKeys(XK_6) = Key.K6 : Singleton.MapKeys(XK_7) = Key.K7 : Singleton.MapKeys(XK_8) = Key.K8 : Singleton.MapKeys(XK_9) = Key.K9

    Singleton.MapKeys(XK_KP_0) = Key.NP0 : Singleton.MapKeys(XK_KP_1) = Key.NP1 : Singleton.MapKeys(XK_KP_2) = Key.NP2 : Singleton.MapKeys(XK_KP_3) = Key.NP3 : Singleton.MapKeys(XK_KP_4) = Key.NP4
    Singleton.MapKeys(XK_KP_5) = Key.NP5 : Singleton.MapKeys(XK_KP_6) = Key.NP6 : Singleton.MapKeys(XK_KP_7) = Key.NP7 : Singleton.MapKeys(XK_KP_8) = Key.NP8 : Singleton.MapKeys(XK_KP_9) = Key.NP9
    Singleton.MapKeys(XK_KP_Multiply) = Key.NP_MUL : Singleton.MapKeys(XK_KP_Add) = Key.NP_ADD : Singleton.MapKeys(XK_KP_Divide) = Key.NP_DIV : Singleton.MapKeys(XK_KP_Subtract) = Key.NP_SUB : Singleton.MapKeys(XK_KP_Decimal) = Key.NP_DECIMAL

    Singleton.MapKeys(X11.XKeySym.XK_semicolon) = Key.OEM_1
    Singleton.MapKeys(X11.XKeySym.XK_equal) = Key.EQUALS
    Singleton.MapKeys(X11.XKeySym.XK_comma) = Key.COMMA
    Singleton.MapKeys(X11.XKeySym.XK_minus) = Key.MINUS
    Singleton.MapKeys(X11.XKeySym.XK_period) = Key.PERIOD
    Singleton.MapKeys(X11.XKeySym.XK_slash) = Key.OEM_2
    Singleton.MapKeys(X11.XKeySym.XK_grave) = Key.OEM_3
    Singleton.MapKeys(X11.XKeySym.XK_bracketleft) = Key.OEM_4
    Singleton.MapKeys(X11.XKeySym.XK_backslash) = Key.OEM_5
    Singleton.MapKeys(X11.XKeySym.XK_bracketright) = Key.OEM_6
    Singleton.MapKeys(X11.XKeySym.XK_apostrophe) = Key.OEM_7

    Return pge_Display

  End Function

  Function Pge_OpenGLCreate_Linux() As Boolean

    Dim glDeviceContext = X11.glXCreateContext(pge_Display, pge_VisualInfo, IntPtr.Zero, GL_TRUE = 1)
    Dim rslt = X11.glXMakeCurrent(pge_Display, pge_Window, glDeviceContext)

    Dim gwa As X11.XWindowAttributes
    Dim hrslt = X11.XGetWindowAttributes(pge_Display, pge_Window, gwa)
    X11.glViewport(0, 0, gwa.Width, gwa.Height)

    'Dim glSwapIntervalEXT As glSwapInterval_t = Nothing
    glSwapIntervalEXT = CType(Marshal.GetDelegateForFunctionPointer(X11.glXGetProcAddress("glXSwapIntervalEXT"), GetType(glSwapInterval_t)), glSwapInterval_t)

    If glSwapIntervalEXT Is Nothing AndAlso Not m_enableVSYNC Then
      Console.WriteLine("NOTE: Could not disable VSYNC, glXSwapIntervalEXT() was not found!")
      Console.WriteLine("      Don't worry though, things will still work, it's just the")
      Console.WriteLine("      frame rate will be capped to your monitors refresh rate - javidx9")
    End If

    If glSwapIntervalEXT IsNot Nothing AndAlso Not m_enableVSYNC Then
      glSwapIntervalEXT(pge_Display, pge_Window, 0)
    End If

    Return True

  End Function

#End Region

#Region "Additional"

  Private Shared Function GET_WHEEL_DELTA_WPARAM(wParam As IntPtr) As Integer
    Dim l = wParam.ToInt64
    Dim v = CInt(If(l > Integer.MaxValue, l - UInteger.MaxValue, l))
    Return CShort(v >> 16)
  End Function

  Public Shared Sub Swap(ByRef a As Integer, ByRef b As Integer)
    Dim t = a
    a = b
    b = t
  End Sub

  Public Shared Sub Swap(ByRef a As Single, ByRef b As Single)
    Dim t = a
    a = b
    b = t
  End Sub

  Public Shared Sub Swap(ByRef p1 As Pixel, ByRef p2 As Pixel)
    Dim n = p1.N
    p1.N = p2.N
    p2.N = n
  End Sub

#End Region

#Region "C++'isms"

  ' Since a lot of the olcPGE examples use `rand`,
  ' including similar functionality here to reduce
  ' constantly trying to translate C++ code to VB
  ' for this common scenario.

  Private ReadOnly m_random As New Random
  Protected Const RAND_MAX As Integer = 2147483647

  Protected ReadOnly Property Rnd As Double
    Get
      Return m_random.NextDouble
    End Get
  End Property

  ' Provide for something *similar* to C++.
  Public ReadOnly Property Rand As Integer
    Get
      Return CInt(Fix(m_random.NextDouble * RAND_MAX))
    End Get
  End Property

#End Region

#Region "CGE"

  ' I've added these so that compatibility with CGE can be retained.

  Private Shared Function ConsoleColor2PixelColor(c As Color) As Pixel
    Select Case c
      Case Color.FgBlack : Return Presets.Black
      Case Color.FgDarkBlue : Return Presets.DarkBlue
      Case Color.FgDarkGreen : Return Presets.DarkGreen
      Case Color.FgDarkCyan : Return Presets.DarkCyan
      Case Color.FgDarkRed : Return Presets.DarkRed
      Case Color.FgDarkMagenta : Return Presets.DarkMagenta
      Case Color.FgDarkYellow : Return Presets.DarkYellow
      Case Color.FgGray : Return Presets.Gray
      Case Color.FgDarkGray : Return Presets.DarkGrey
      Case Color.FgBlue : Return Presets.Blue
      Case Color.FgGreen : Return Presets.Green
      Case Color.FgCyan : Return Presets.Cyan
      Case Color.FgRed : Return Presets.Red
      Case Color.FgMagenta : Return Presets.Magenta
      Case Color.FgYellow : Return Presets.Yellow
      Case Color.FgWhite : Return Presets.White
      Case Color.BgBlack : Return Presets.Black
      Case Color.BgDarkBlue : Return Presets.DarkBlue
      Case Color.BgDarkGreen : Return Presets.DarkGreen
      Case Color.BgDarkCyan : Return Presets.DarkCyan
      Case Color.BgDarkRed : Return Presets.DarkRed
      Case Color.BgDarkMagenta : Return Presets.DarkMagenta
      Case Color.BgDarkYellow : Return Presets.DarkYellow
      Case Color.BgGray : Return Presets.Gray
      Case Color.BgDarkGray : Return Presets.DarkGrey
      Case Color.BgBlue : Return Presets.Blue
      Case Color.BgGreen : Return Presets.Green
      Case Color.BgCyan : Return Presets.Cyan
      Case Color.BgRed : Return Presets.Red
      Case Color.BgMagenta : Return Presets.Magenta
      Case Color.BgYellow : Return Presets.Yellow
      Case Color.BgWhite : Return Presets.White
      Case Else
        Return Presets.White
    End Select
  End Function

  Public Function ConstructConsole(w As Integer, h As Integer, pw As Integer, ph As Integer) As Boolean
    Return Construct(w, h, pw, ph)
  End Function

  Protected Sub Fill(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    Dim w = (x2 - x1) + 1
    Dim h = (y2 - y1) + 1
    FillRect(x1, y1, w, h, ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub FillCircle(x As Single, y As Single, radius As Single, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    FillCircle(CInt(Fix(x)), CInt(Fix(y)), CInt(Fix(radius)), ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub FillCircle(x As Integer, y As Integer, radius As Single, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    FillCircle(x, y, CInt(Fix(radius)), ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub DrawLine(x1 As Single, y1 As Single, x2 As Single, y2 As Single, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    DrawLine(CInt(Fix(x1)), CInt(Fix(y1)), CInt(Fix(x2)), CInt(Fix(y2)), ConsoleColor2PixelColor(c))
  End Sub

  Protected Sub DrawLine(x1 As Integer, y1 As Integer, x2 As Integer, y2 As Integer, dummy As PixelType, c As Color)
    If dummy = PixelType.Half Then
    End If
    DrawLine(x1, y1, x2, y2, ConsoleColor2PixelColor(c))
  End Sub

#End Region

End Class

Public MustInherit Class PgeX

  'Public Shared Property Pge As PixelGameEngine

End Class

#Region "CGE"

Public Enum Color As Short
  FgBlack = &H0
  FgDarkBlue = &H1
  FgDarkGreen = &H2
  FgDarkCyan = &H3
  FgDarkRed = &H4
  FgDarkMagenta = &H5
  FgDarkYellow = &H6
  FgGray = &H7
  FgDarkGray = &H8
  FgBlue = &H9
  FgGreen = &HA
  FgCyan = &HB
  FgRed = &HC
  FgMagenta = &HD
  FgYellow = &HE
  FgWhite = &HF
#Disable Warning CA1069 ' Enums values should not be duplicated
  BgBlack = &H0
#Enable Warning CA1069 ' Enums values should not be duplicated
  BgDarkBlue = &H10
  BgDarkGreen = &H20
  BgDarkCyan = &H30
  BgDarkRed = &H40
  BgDarkMagenta = &H50
  BgDarkYellow = &H60
  BgGray = &H70
  BgDarkGray = &H80
  BgBlue = &H90
  BgGreen = &HA0
  BgCyan = &HB0
  BgRed = &HC0
  BgMagenta = &HD0
  BgYellow = &HE0
  BgWhite = &HF0
End Enum

Public Enum PixelType As Short
  Solid = &H2588
  ThreeQuarters = &H2593
  Half = &H2592
  Quarter = &H2591
End Enum

#End Region