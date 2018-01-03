Imports Microsoft.VisualBasic.FileIO
Imports System.Collections.Generic
Imports System.Diagnostics
Public Module GetJobs
    ''' <summary>
    ''' функция,  в которой нужно описать, как исходные данные превращаются в элементарные работы
    ''' </summary>
    ''' <param name="InitDataList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetElementaryJobs( _
                                     InitDataList As List(Of InitData)) _
                                 As List(Of ElementaryJob)

        Dim ResultList As New List(Of ElementaryJob)

        'каждая элементарная работа получает свой номер
        Dim JobNo As Integer = 1

        For Each item As InitData In InitDataList

			Dim NewJob As New ElementaryJob
			With NewJob
				.Id = JobNo
				.Name = item.VariantsNamesPath
				.AreasPath = item.MonitoredValuesPath
				.LinesPath = item.VariantsPath
				.File = item.AdditionalInfo
				.SavePath = item.SavePath 
			End With

            ResultList.Add(NewJob) 
            JobNo += 1
        Next

        Return ResultList
    End Function

    Private Function ReadFromTxt(path As String) As List(Of String())
        Try
            Dim OutputList As New List(Of String())
            Using Parser As New TextFieldParser(path, System.Text.Encoding.GetEncoding(1251))
                Parser.TextFieldType = FieldType.Delimited
                Parser.SetDelimiters(";")

                While Not Parser.EndOfData
                    Dim ItemToAdd As String()
                    ItemToAdd = Parser.ReadFields()
                    OutputList.Add(ItemToAdd)
                End While
            End Using
            Return OutputList
            
        Catch ex As Exception

            My.Application.Log.WriteException(ex, _
                                          TraceEventType.Error, _
                                          "Ошибка: " & path &
                                          ". Возможно, нет прав для доступа к файлу, его имя задано неверно или в файле содержится ошибка" &
                                          " Подробно: " & ex.Message)



            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
            Throw ex
            Return New List(Of String())
        End Try
    End Function
End Module
