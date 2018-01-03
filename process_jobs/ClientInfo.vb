Imports System.Security.Cryptography
<Serializable>
Public Class ClientInfo
    Sub New(n As Integer, name As String)
        Me.MaxThreadsCount = n
        Me.ClientName = name
        'Me.Version = GetDirectoryHash()
    End Sub
    Public MaxThreadsCount As Integer
    Public ClientName As String
    Public Version As Byte()


    Private Function GetDirectoryHash() As Byte()
        Dim files As String() = IO.Directory.GetFiles(My.Application.Info.DirectoryPath)

        Dim DirectoryHash As Byte() = {}

        For Each file As String In files
            'DirectoryHash = DirectoryHash.Concat(GetFileHash(file)).ToArray
        Next
        Return DirectoryHash
    End Function

    Private Function GetFileHash(file As String) As Byte()
        Dim HashGetter As MD5 = System.Security.Cryptography.MD5.Create
        Dim Result As Byte()
        Dim sb As New System.Text.StringBuilder

        Using st As New IO.FileStream(file, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            Result = HashGetter.ComputeHash(st)
        End Using
        Return Result
    End Function
End Class
