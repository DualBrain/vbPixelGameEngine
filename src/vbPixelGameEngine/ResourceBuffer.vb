Friend Class ResourceBuffer

    Friend vMemory As Byte()

    Public ReadOnly Property Data As Byte()
        Get
            Return vMemory
        End Get
    End Property

    Sub New(ifs As IO.FileStream, offset As Integer, size As Integer)
        vMemory = New Byte(size - 1) {}
        ifs.Seek(offset, IO.SeekOrigin.Begin)
        ifs.ReadExactly(vMemory)
    End Sub

End Class
