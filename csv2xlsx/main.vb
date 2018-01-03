Imports System
Imports System.Collections.Generic

Module Main

    Sub Main()

        My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
        My.Application.Log.DefaultFileLogWriter.Append = False
        My.Application.Log.DefaultFileLogWriter.AutoFlush = True
        My.Application.Log.WriteEntry("Программа запущена " & My.Computer.Clock.LocalTime.ToLongDateString & My.Computer.Clock.LocalTime.ToLongTimeString)

        Dim jobs As New List(Of String)
        jobs.Add("test")
        jobs.Add("test2")
        Dim jobs_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of String)))
        Dim writer As New System.IO.StreamWriter("jobs.xml")
        jobs_serializer.Serialize(writer, jobs)

        'сначала читаем файл paths.xml, в котором содержатся пути к цсв файлам
        Dim init_data_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of String)))
        Dim input_data As New List(Of String)
        Try
            Dim reader As New System.IO.StreamReader("csv_paths.xml")
            input_data = init_data_serializer.Deserialize(reader)
        Catch ex As Exception
            My.Application.Log.WriteException(ex, _
                                              System.Diagnostics.TraceEventType.Error, _
                                              "Ошибка чтения файла init_data.xml. Возможно, к нему нет доступа или текст содержит ошибку. Подробно: " & ex.Message)
            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
            Exit Sub
        End Try
        For Each item As String In input_data
            Csv2Xlsx(item)
        Next
    End Sub
    Private Sub Csv2Xlsx(Folder As String)
        Dim Excel As New Object
        Try
            Excel = CreateObject("Excel.Application")
            Dim CsvFiles As String() = IO.Directory.GetFiles(Folder, "*.csv")
            If (Not IO.Directory.Exists(Folder & "xlsx")) Then
                IO.Directory.CreateDirectory(Folder & "xlsx")
            End If
            Excel.DisplayAlerts = False
            For Each CsvFile As String In CsvFiles
                Dim WB As Object = Excel.Workbooks.Open(CsvFile, Local:=True, Format:=4)

                Dim path As String = Replace(CsvFile, Folder, Folder & "xlsx\")
                path = Replace(path, ".csv", ".xlsx")
                WB.SaveAs(Filename:=path, _
                          FileFormat:=51)

                WB.Close()

            Next
            Excel.Quit()
            My.Application.Log.WriteEntry("Преобразование закончено")
        Catch ex As System.Exception
            Excel.Quit()
            Excel = Nothing
            MsgBox("Ошибка преобразования файла. Возможно, нет доступа к файлам или не установлен Еxcel")
        End Try


    End Sub
End Module
