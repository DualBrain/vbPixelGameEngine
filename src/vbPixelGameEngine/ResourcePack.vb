Imports System.IO
Imports System.Runtime.InteropServices

'=============================================================
' Resource Packs - Allows you to store files in one large 
' scrambled file - Thanks MaGetzUb for debugging a null char in std:stringstream bug
Public Class ResourcePack

  Private Structure ResourceFile
    Public Size As Integer
    Public Offset As Integer
  End Structure

  Private m_baseFile As FileStream
  Private ReadOnly m_mapFiles As New Dictionary(Of String, ResourceFile)

  Public Sub New()
  End Sub

  'Protected Overrides Sub Finalize()
  '  MyBase.Finalize()
  '  m_baseFile.Close()
  'End Sub

  Public Function AddFile(filename As String) As Boolean

    Dim file = MakePosix(filename)

    If IO.File.Exists(file) Then
      Dim e = New ResourceFile()
      Dim fileInfo = New IO.FileInfo(file)
      e.Size = CInt(fileInfo.Length)
      e.Offset = 0 ' Unknown at this stage
      m_mapFiles(file) = e
      Return True
    End If

    Return False

  End Function

  Public Function LoadPack(filename As String, key As String) As Boolean

    ' Open the resource file
    m_baseFile = New IO.FileStream(filename, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
    If Not m_baseFile.CanRead Then Return False

    ' 1) Read Scrambled index
    Dim indexSize As Integer = 0
    m_baseFile.Read(BitConverter.GetBytes(indexSize), 0, 4)

    Dim buffer(indexSize - 1) As Byte
    m_baseFile.Read(buffer, 0, indexSize)

    Dim decoded = Scramble(buffer, key)
    Dim pos As Integer = 0
    Dim read = Sub(dst As Byte(), size As Integer)
                 Array.Copy(decoded, pos, dst, 0, size)
                 pos += size
               End Sub

    Dim [get] = Function() As Integer
                  Dim c = New Byte(0) {}
                  read(c, 1)
                  Return c(0)
                End Function

    ' 2) Read Map
    Dim mapEntries = 0
    read(BitConverter.GetBytes(mapEntries), 4)

    For i = 0 To mapEntries - 1

      Dim filePathSize = 0
      read(BitConverter.GetBytes(filePathSize), 4)

      Dim useFileName = ""
      For j = 0 To filePathSize - 1
        useFileName &= ChrW([get]())
      Next

      Dim e = New ResourceFile()
      read(BitConverter.GetBytes(e.Size), 4)
      read(BitConverter.GetBytes(e.Offset), 4)
      m_mapFiles(useFileName) = e

    Next

    ' Don't close base file! we will provide a stream
    ' pointer when the file is requested
    Return True

  End Function

  Public Function SavePack(filename As String, key As String) As Boolean
    ' Create/Overwrite the resource file
    Dim ofs = New FileStream(filename, IO.FileMode.Create)
    If Not ofs.CanWrite Then Return False
    ' Iterate through map
    Dim indexSize = 0 ' Unknown for now
    Dim indexSizeBytes = BitConverter.GetBytes(indexSize)
    ofs.Write(indexSizeBytes, 0, indexSizeBytes.Length)

    Dim mapSize = m_mapFiles.Count
    Dim mapSizeBytes = BitConverter.GetBytes(mapSize)
    ofs.Write(mapSizeBytes, 0, mapSizeBytes.Length)

    For Each pair In m_mapFiles

      Dim e = pair.Value

      ' Write the path of the file
      Dim pathSize = pair.Key.Length
      Dim pathSizeBytes = BitConverter.GetBytes(pathSize)
      ofs.Write(pathSizeBytes, 0, pathSizeBytes.Length)
      Dim pathBytes = Text.Encoding.ASCII.GetBytes(pair.Key)
      ofs.Write(pathBytes, 0, pathBytes.Length)

      ' Write the file entry properties
      Dim sizeBytes = BitConverter.GetBytes(e.Size)
      ofs.Write(sizeBytes, 0, sizeBytes.Length)

      Dim offsetBytes = BitConverter.GetBytes(e.Offset)
      ofs.Write(offsetBytes, 0, offsetBytes.Length)

    Next

    ' 2) Write the individual Data
    Dim offset = ofs.Position
    'indexSize = CInt(offset)

    For Each pair In m_mapFiles

      Dim e = pair.Value

      ' Store beginning of file offset within resource pack file
      e.Offset = CInt(offset)

      ' Load the file to be added
      Dim vBuffer(e.Size - 1) As Byte
      Using i = New System.IO.FileStream(pair.Key, IO.FileMode.Open)
        i.Read(vBuffer, 0, e.Size)
      End Using

      ' Write the loaded file into resource pack file
      ofs.Write(vBuffer, 0, e.Size)
      offset += e.Size

    Next

    ' 3) Scramble Index
    Dim stream = New List(Of Byte)()
    Dim write As Action(Of Byte(), Integer) = Sub(data, size)
                                                Dim sizeNow = stream.Count()
                                                stream.AddRange(data.Take(size))
                                              End Sub

    ' Iterate through map
    Dim mapSizeBytes2 = BitConverter.GetBytes(mapSize)
    write(mapSizeBytes2, Marshal.SizeOf(GetType(Integer)))

    For Each pair In m_mapFiles

      Dim e = pair.Value

      ' Write the path of the file
      Dim pathSizeBytes2 = BitConverter.GetBytes(pair.Key.Length)
      write(pathSizeBytes2, Marshal.SizeOf(GetType(Integer)))
      Dim pathBytes2 = Text.Encoding.ASCII.GetBytes(pair.Key)
      write(pathBytes2, pair.Key.Length)

      ' Write the file entry properties
      Dim sizeBytes2 = BitConverter.GetBytes(e.Size)
      write(sizeBytes2, Marshal.SizeOf(GetType(Integer)))

      Dim offsetBytes2 = BitConverter.GetBytes(e.Offset)
      write(offsetBytes2, Marshal.SizeOf(GetType(Integer)))

    Next

    Dim indexString = Scramble(stream.ToArray, key)
    Dim indexStringLen = indexString.Length

    ' 4) Rewrite Map (it has been updated with offsets now) at start of file
    ofs.Seek(0, SeekOrigin.Begin)
    ofs.Write(BitConverter.GetBytes(indexStringLen), 0, Marshal.SizeOf(GetType(Integer)))
    ofs.Write(indexString.ToArray(), 0, indexString.Length)
    ofs.Close()

    Return True

  End Function

  Friend Function GetFileBuffer(filename As String) As ResourceBuffer
    Dim e = m_mapFiles(filename)
    Return New ResourceBuffer(m_baseFile, e.Offset, e.Size)
  End Function

  Public Function Loaded() As Boolean
    Return m_baseFile IsNot Nothing
  End Function

  Private Shared Function Scramble(data As Byte(), key As String) As Byte()
    If String.IsNullOrEmpty(key) Then Return data
    Dim result = New List(Of Byte)
    Dim c = 0
    For Each s In data
      result.Add(s Xor CByte(AscW(key(c Mod key.Length))))
      c += 1
    Next
    Return result.ToArray
  End Function

  Private Shared Function MakePosix(path As String) As String
    Dim result = ""
    For Each c In path
      result += If(c = "\", "/", c)
    Next
    Return result
  End Function

End Class