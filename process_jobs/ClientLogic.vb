Imports System.Collections.Generic

Module ClientLogic
    ''' <summary>
    ''' функция, в которой отдельные работы раскидываются по тредам и выполняются
    ''' </summary>
    ''' <param name="Jobs">лист элементарных работ</param>
    ''' <param name="ThisPcInfo">информация о компьютере клиента</param>
    ''' <returns>возвращает результаты выполнения элементарных работ</returns>
    ''' <remarks></remarks>
    Friend Function ClientLoop( _
                              Jobs As List(Of IElementaryJob), _
                              ThisPcInfo As ClientInfo) _
                          As List(Of IJobResult)


        Dim OutputResults As New List(Of IJobResult)
        Dim Threads As New List(Of System.Threading.Thread)

        'для каждой работы завести отдельный тред
        For Each Job As IElementaryJob In Jobs
            'объект результата работы сразу добавить в выходной список, и затем по ссылке передать его в функцию вычисления
            'тогда не надо будет возвращать его из Thread, что сделать по-простому никак нельзя.
            Dim Result As New JobResult
            OutputResults.Add(Result)

			'настройка отдельного треда. Т.к. используется VB9, то лямбда не поддерживается, запускаем нужную 
			'функцию через AddressOf, а все параметры функции засовываем в один большой объект.
            Dim SeparateJobThread As New System.Threading.Thread(AddressOf ExecuteSingleJob)
			Dim JobParams As New JobThreadParams
			With JobParams
				.Job = Job
				.Result = Result
				.InterimSave = True
				.SendResult = True
			End With

            SeparateJobThread.Name = "Job: " & Job.Id 'назвать для дебаггинга
            SeparateJobThread.SetApartmentState(Threading.ApartmentState.STA)
            Threads.Add(SeparateJobThread) 'добавить в общий список тредов, чтобы можно потом было отследить их завершение
            SeparateJobThread.Start(JobParams)
        Next

        'после того, как все треды запущены, подождать завершения всех тредов
		For Each thread As System.Threading.Thread In Threads
            thread.Join()
        Next

        Return OutputResults
    End Function
End Module

Friend Class JobThreadParams
	Friend Job As IElementaryJob
	Friend Result As JobResult
	Friend InterimSave As Boolean
	Friend SendResult As Boolean
End Class
