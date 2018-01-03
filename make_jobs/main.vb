Imports System.Collections.Generic
Imports System.Diagnostics

Module Main

     Sub Main()

        'настройка лога, чтобы он сохранялся в той же папке, что и программа
        My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
        My.Application.Log.DefaultFileLogWriter.Append = False
        My.Application.Log.DefaultFileLogWriter.AutoFlush = True
        My.Application.Log.WriteEntry("Программа запущена " & My.Computer.Clock.LocalTime.ToLongDateString & My.Computer.Clock.LocalTime.ToLongTimeString)

        'сначала читаем файл init_data.xml, в котором содержатся названия файлов с вариантами
        Dim init_data_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of InitData)))
        Dim input_data As New List(Of InitData)
        Try
            Dim reader As New System.IO.StreamReader("init_data.xml")
            input_data = init_data_serializer.Deserialize(reader)
        Catch ex As Exception
            My.Application.Log.WriteException(ex, _
                                              TraceEventType.Error, _
                                              "Ошибка чтения файла init_data.xml. Возможно, к нему нет доступа или текст содержит ошибку. Подробно: " & ex.Message)
            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
            Exit Sub
        End Try


        'теперь названия файлов с вариантами посылаем в функцию для разбиения на элементарные работы
        'эта функция объявлена во внешней длл и её должен определить пользователь
        Dim jobs As List(Of ElementaryJob) = GetElementaryJobs(input_data) 

        'записываем полученные элементарные работы в выходной файл jobs.xml
        Dim jobs_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of ElementaryJob)))
        Try
            Dim writer As New System.IO.StreamWriter("jobs.xml")
            jobs_serializer.Serialize(writer, jobs)
        Catch ex As Exception
            My.Application.Log.WriteException(ex, _
                                              TraceEventType.Error, _
                                              "Ошибка записи файла init_data.xml. Возможно, к нему нет доступа. Подробно: " & ex.Message)
            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")

            Exit Sub
        End Try

    End Sub

End Module
