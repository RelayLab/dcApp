Imports System.Collections.Generic

Friend Module ServerLogic
    ''' <summary>
    ''' функция, принимающая работы, отмечающая их как сделанные и выдающая новые по числу тредов у клиента
    ''' </summary>
    ''' <param name="Jobs">список все работ</param>
    ''' <param name="Results">входные результаты, которые будут отмечены как сделанные</param>
    ''' <param name="ClientInfoInst">инфа о клиенте</param>
    ''' <returns>возвращает новые работы на основе инфы о клиенте</returns>
    ''' <remarks></remarks>
    Friend Function ServerLoop(
                              Jobs As List(Of IElementaryJob), _
                              Results As List(Of IJobResult), _
                              ClientInfoInst As ClientInfo) _
                          As List(Of IElementaryJob)

        MarkAsCompleted(Jobs, Results) 'пометить как сделанные

        'на основе инфы о клиенте взять новые несделанные работы
 '       Dim OutputJobs As List(Of IElementaryJob) = (From job In Jobs
 '                                                   Where job.IsDone = False
 '                                                   Take ClientInfoInst.MaxThreadsCount).ToList
		Dim OutputJobs As New List(Of IElementaryJob) 
		Dim m As Integer = 0
		For each job as IElementaryJob In Jobs

			If job.IsDone = False

				If ClientInfoInst.MaxThreadsCount > m Then
					OutputJobs.Add(job)
					m = m + 1
				End If

			End If

		Next


	   'пометить эти работы как находящиеся в обработке у клиента
		For each job As IElementaryJob in OutputJobs
			job.IsProcessing = True
			job.ClientName = ClientInfoInst.ClientName
		Next

        Return OutputJobs
    End Function

    ''' <summary>
    ''' пометить работы как сделанные
    ''' </summary   >
    ''' <param name="Results">сделанные работы</param>
    ''' <remarks></remarks>
    Friend Sub MarkAsCompleted(AllJobs As List(Of IElementaryJob), Results As List(Of IJobResult))
        If Results IsNot Nothing Then
            For Each ResultsTable As IJobResult In Results

				For Each job As IElementaryJob in AllJobs 'пометить работы как сделанные
					if job.Id = ResultsTable.JobId then _
						job.IsDone = True
				Next

            Next
        End If
    End Sub
End Module
