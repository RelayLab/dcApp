Imports System.Collections.Generic
Imports System.Diagnostics

Module main

    Sub Main(argc as Integer, argv As String())

        'это для тестирования прямо из студии, передает аргумент в программу, обозначающий работу в двух потоках
#If DEBUG Then
        Dim tmp As String() = {"-t2"}
        argv = tmp
#End If

        'входной арумент -tN показаывает, сколько тредов запустить. N - натуральное число
        'Здесь берём этот входной аргумент и записываем его в MaxThreadsCount 
        Dim MaxThreadsCount As Integer
        'If argv.Count = 1 AndAlso Left(argv(0), 2) = "-t" Then
        If argc= 1 AndAlso Left(argv(0), 2) = "-t" Then
            MaxThreadsCount = Integer.Parse(argv(0)(2))
        Else
            MaxThreadsCount = 1
        End If

        '*********************Настройка записи в лог**************************
        My.Application.Log.DefaultFileLogWriter.BaseFileName = "thread_main"    'название файла лога
        My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory  'сохранять лог в папку с программой
        My.Application.Log.DefaultFileLogWriter.Append = False  'перезаписать старый лог
        My.Application.Log.DefaultFileLogWriter.AutoFlush = True 'добавлять каждую новую запись сразу в файл, а не только в буфер записи
        '*********************Настройка записи в лог**************************

        My.Application.Log.WriteEntry("Программа запущена " & My.Computer.Clock.LocalTime.ToLongDateString & My.Computer.Clock.LocalTime.ToLongTimeString)
        My.Application.Log.WriteEntry(My.Computer.Clock.LocalTime.ToLongTimeString & " " & "Запущено потоков вычислений: " & MaxThreadsCount)
        Console.WriteLine("Запущено потоков вычислений: " & MaxThreadsCount)

        'сначала читаем файл jobs.xml, в котором содержатся элементарные работы
        Dim jobs_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of ElementaryJob)))
        Dim jobs As List(Of ElementaryJob)
        Try
            Dim reader As New System.IO.StreamReader("jobs.xml")
            jobs = jobs_serializer.Deserialize(reader)
        Catch ex As Exception
            My.Application.Log.WriteException(ex, _
                                              TraceEventType.Error, _
                                              "Ошибка чтения файла jobs.xml. Возможно, к нему нет доступа или текст содержит ошибку. Подробно: " & ex.Message)
            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
            Exit Sub
        End Try

        'создать массив с информацией о данном компьютере
        Dim ThisPcInfo As New ClientInfo(MaxThreadsCount, My.Computer.Name)

        'Запустить работу!
        LocalLogic.DoLocalJob( _
            ThisPcInfo, _
            jobs)
    End Sub
    
End Module
