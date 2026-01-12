# ResourcePack Class

Manages collections of files stored in a single scrambled pack file for distribution and loading.

## Methods

### AddFile
```vb
Public Function AddFile(filename As String) As Boolean
```
Add a file to the pack (for saving).

**Parameters:**
- `filename`: Path to the file to add

**Returns:** `True` if file exists and was added

### LoadPack
```vb
Public Function LoadPack(filename As String, key As String) As Boolean
```
Load a resource pack from file.

**Parameters:**
- `filename`: Path to the pack file
- `key`: Decryption key

**Returns:** `True` on success

### SavePack
```vb
Public Function SavePack(filename As String, key As String) As Boolean
```
Save the current file collection to a pack file.

**Parameters:**
- `filename`: Path for the pack file
- `key`: Encryption key

**Returns:** `True` on success

**Example:**
```vb
Dim pack As New ResourcePack()
pack.AddFile("sprite.png")
pack.AddFile("sound.wav")
pack.SavePack("game.pak", "mypassword")
```

### GetFileBuffer
```vb
Friend Function GetFileBuffer(filename As String) As ResourceBuffer
```
Get a buffer for reading a file from the pack.

**Parameters:**
- `filename`: Name of the file in the pack

**Returns:** `ResourceBuffer` for reading the file

### Loaded
```vb
Public Function Loaded() As Boolean
```
Check if a pack is currently loaded.

**Returns:** `True` if a pack is loaded

## ResourceBuffer Structure

Provides stream access to packed file data.

```vb
Public Class ResourceBuffer
    Inherits MemoryStream

    Public Sub New(baseFile As FileStream, offset As Integer, size As Integer)
```

**Example usage with Sprite:**
```vb
Dim pack As New ResourcePack()
pack.LoadPack("game.pak", "mypassword")
Dim sprite As New Sprite()
sprite.LoadFromFile("sprite.png", pack)
```