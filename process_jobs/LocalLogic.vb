Imports System.Collections.Generic


Friend Module LocalLogic

    ''' <summary>
    ''' Использует кусочки из клиента и сервера, чтобы сама себе раздавать работы
    ''' </summary>
    ''' <param name="ThisPcInfo"></param>
    ''' <remarks></remarks>
    Sub DoLocalJob(
                  ThisPcInfo As ClientInfo,
                  AllJobs As List(Of IElementaryJob))

        Dim SingleJobs As New List(Of IElementaryJob)
        Dim AllResults As New List(Of IJobResult)
        Dim SingleJobResults As New List(Of IJobResult)

		While Not ( IsAllJobsDone(AllJobs) )  'пока есть несделанные работы...

            'запустить на локальном компьютере последовательно сначала петлю сервера, затем петлю клиента
            'петля сервера будет выбирать из списка всех работ количество несделанных по числу возможных тредов в ClientInfo
            'петля клиента будет их обрабатывать и возвращать через какое-то время
            'и так повторяется, пока все работы не будут сделаны IsDone
            SingleJobs = ServerLogic.ServerLoop(AllJobs, SingleJobResults, ThisPcInfo)
            SingleJobResults = ClientLogic.ClientLoop(SingleJobs, ThisPcInfo)
            AllResults.AddRange(SingleJobResults)
        End While

        My.Application.Log.WriteEntry("Расчеты окончены")
        Console.WriteLine("Расчеты окончены")

        'записываем полученные результаты в выходной файл results.xml
        Dim results_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of JobResult)))
        Dim writer As New System.IO.StreamWriter("results.xml")
        results_serializer.Serialize(writer, AllResults)


    End Sub

	Function IsAllJobsDone( AllJobs As List(Of IElementaryJob) ) As Boolean
		For Each Job As IElementaryJob In AllJobs
			If Job.IsDone = False Then _
				Return False
		Next
		Return True
	End Function

End Module
