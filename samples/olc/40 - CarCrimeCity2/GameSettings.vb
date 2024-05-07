' This is a singleton that stores all the game's configuration settings.
' These settings are loaded on game start up and are to be considered read-only.
Public Class AssetModel
  Public Creator As String
  Public Description As String
  Public ModelObject As String
  Public Property ModelPng As String
  Public Rotate(2) As Single
  Public Scale(2) As Single
  Public Translate(2) As Single
End Class

Public Class AssetTexture
  Public Name As String
  Public File As String
End Class

Public Class GameSettings

  Public Shared Property ScreenWidth As Integer = 768
  Public Shared Property ScreenHeight As Integer = 480
  Public Shared Property PixelWidth As Integer = 2
  Public Shared Property PixelHeight As Integer = 2
  Public Shared Property FullScreen As Boolean = False

  Public Shared Property DefaultMapWidth As Integer = 64
  Public Shared Property DefaultMapHeight As Integer = 32
  Public Shared Property DefaultCityFile As String = ""

  Public Shared Property AssetTextures As New List(Of AssetTexture)()
  Public Shared Property AssetBuildings As New List(Of AssetModel)()
  Public Shared Property AssetVehicles As New List(Of AssetModel)()

  Private Sub New()
  End Sub

  Public Shared Function LoadConfigFile(filename As String) As Boolean

    Dim lua = New NLua.Lua
    lua.State.OpenLibs()

    If Not IO.File.Exists(filename) Then
      Console.WriteLine($"'{filename}' missing")
      Return False
    End If

    Try
      Dim luaResults = lua.DoFile(filename)
    Catch ex As NLua.Exceptions.LuaScriptException
      Console.WriteLine(ex.Message)
      Return False
    End Try

    Dim L = lua.State

    L.GetGlobal("PixelWidth") : If L.IsInteger(-1) Then GameSettings.PixelWidth = CInt(L.ToInteger(-1))
    L.GetGlobal("PixelHeight") : If L.IsInteger(-1) Then GameSettings.PixelHeight = CInt(L.ToInteger(-1))
    L.GetGlobal("ScreenWidth") : If L.IsInteger(-1) Then GameSettings.ScreenWidth = CInt(L.ToInteger(-1))
    L.GetGlobal("ScreenHeight") : If L.IsInteger(-1) Then GameSettings.ScreenHeight = CInt(L.ToInteger(-1))
    L.GetGlobal("DefaultMapWidth") : If L.IsInteger(-1) Then GameSettings.DefaultMapWidth = CInt(L.ToInteger(-1))
    L.GetGlobal("DefaultMapHeight") : If L.IsInteger(-1) Then GameSettings.DefaultMapHeight = CInt(L.ToInteger(-1))
    L.GetGlobal("DefaultCityFile") : If L.IsString(-1) Then GameSettings.DefaultCityFile = L.ToString(-1)
    L.GetGlobal("FullScreen") : If L.IsBoolean(-1) Then GameSettings.FullScreen = L.ToBoolean(-1)

    ' Load System Texture files

    ' Load Texture Assets
    L.GetGlobal("Textures") ' -1 Table "Teams"
    If L.IsTable(-1) Then
      L.PushNil() ' -2 Key Nil : -1 Table "Teams"

      While L.Next(-2)  ' -1 Table : -2 Key "TeamName" : -3 Table "Teams"
        Dim texture = New AssetTexture()
        Dim stage = 0
        If L.IsTable(-1) Then
          L.GetTable(-1) ' -1 Table : -2 Table Value : -3 Key "TeamName" : -4 Table "Teams" 
          L.PushNil() ' -1 Key Nil : -2 Table : -3 Table Value : -4 Key "TeamName" : -5 Table "Teams" 
          While L.Next(-2)  ' -1 Value "BotFile" : -2 Key Nil : -3 Table : -4 Table Value : -5 Key "TeamName" : -6 Table "Teams" 
            If stage = 0 Then
              texture.Name = L.ToString(-1)
            End If
            If stage = 1 Then
              texture.File = L.ToString(-1)
            End If
            L.Pop(1) ' -1 Key Nil : -2 Table : -3 Table Value : -4 Key "TeamName" : -5 Table "Teams"
            stage += 1
          End While
        End If
        L.Pop(1) ' -1 Table : -2 Table Value : -3 Key "TeamName" : -4 Table "Teams"
        AssetTextures.Add(texture)
      End While
    End If

    Dim GroupLoadAssets = Sub(group As String, vec As List(Of AssetModel))
                            L.GetGlobal(group)
                            If L.IsTable(-1) Then
                              L.PushNil()
                              While L.Next(-2)
                                Dim model As New AssetModel()
                                Dim stage = 0
                                If L.IsTable(-1) Then
                                  L.GetTable(-1)
                                  L.PushNil()
                                  While L.Next(-2)
                                    If stage = 0 Then model.Creator = L.ToString(-1)
                                    If stage = 1 Then model.Description = L.ToString(-1)
                                    If stage = 2 Then model.ModelObject = L.ToString(-1)
                                    If stage = 3 Then model.ModelPng = L.ToString(-1)
                                    If stage = 4 Then model.Rotate(0) = CSng(L.ToNumber(-1))
                                    If stage = 5 Then model.Rotate(1) = CSng(L.ToNumber(-1))
                                    If stage = 6 Then model.Rotate(2) = CSng(L.ToNumber(-1))
                                    If stage = 7 Then model.Scale(0) = CSng(L.ToNumber(-1))
                                    If stage = 8 Then model.Scale(1) = CSng(L.ToNumber(-1))
                                    If stage = 9 Then model.Scale(2) = CSng(L.ToNumber(-1))
                                    If stage = 10 Then model.Translate(0) = CSng(L.ToNumber(-1))
                                    If stage = 11 Then model.Translate(1) = CSng(L.ToNumber(-1))
                                    If stage = 12 Then model.Translate(2) = CSng(L.ToNumber(-1))
                                    L.Pop(1)
                                    stage += 1
                                  End While
                                End If
                                L.Pop(1)
                                vec.Add(model)
                              End While
                            End If
                          End Sub

    ' Load Building Assets
    GroupLoadAssets("Buildings", AssetBuildings)

    ' Load Vehicle Assets
    GroupLoadAssets("Vehicles", AssetVehicles)

    lua.Close()

    Return True

  End Function

End Class