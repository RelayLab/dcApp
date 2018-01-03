''' <summary>
''' Интерфейс, который используется для чтения из CSV файла в List(of T)
''' </summary>
''' <remarks></remarks>
Public Interface IWritable
    ''' <summary>
    ''' Принимает массив строк, которые будут использоваться в классе, реализующем интерфейс
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    WriteOnly Property Arguments() As String()
End Interface
