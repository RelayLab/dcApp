Option Explicit On
Option Infer On
Option Strict Off
Imports System.Collections.Generic
Imports System.Diagnostics
Public Module MskCreator

	''' <summary>
    ''' Функция для проверки того, можно ли прочитать заданные файлы
    ''' </summary>
    ''' <param name="AreasPath">Файл районов</param>
    ''' <param name="CommutationsPath">Файл ремонтов-ветвей</param>
    ''' <param name="CommutationNamesPath">файл названий ремонтов</param>
    ''' <summary>
    ''' Создает МСК для всех коммутаций, указанных в файле CommutationsPath
    ''' </summary>

    Sub CheckFiles( _
					AreasPath As String,
					CommutationsPath As String,
					CommutationNamesPath As String, _
					Optional AreasList As List(Of Area) = Nothing, _
					Optional CommList As List (Of Commutation) = Nothing, _
					Optional NamesList As List (Of CommutationName) = Nothing )

		Dim AreasFromFile As List(Of String()) = ReadFromCsv(AreasPath)
        AreasList = New List(Of Area)(AreasFromFile.Count)
		For i=0 To AreasList.Count-1
			AreasList.Item(i).Arguments = AreasFromFile.Item(i)
		Next

		Dim CommutationsFromFile As List(Of String()) = ReadFromCsv(CommutationsPath)
        CommList = New List(Of Commutation)(CommutationsFromFile.Count)
		For i=0 To AreasList.Count-1
			CommList.Item(i).Arguments = CommutationsFromFile.Item(i)
		Next

		Dim NamesFromFile As List(Of String()) = ReadFromCsv(CommutationNamesPath)
        NamesList = New List(Of CommutationName)(NamesFromFile.Count)
		For i=0 To AreasList.Count-1
			NamesList.Item(i).Arguments = NamesFromFile.Item(i)
		Next

    End Sub

    ''' <param name="Rg2Path">файл режима, для которого будет создаваться МСК</param>
    ''' <param name="AreasPath">файл с районами для возмущений</param>
    ''' <param name="CommutationsPath">файл со списком ремонтов</param>
    ''' <param name="CommutationNamesPath">файл с названиями ремонтов</param>
    ''' <param name="SavePath">путь к папке для сохранения МСК</param>
    ''' <param name="HelperEnabled">вывод параметров расчета в протокол</param>
    ''' <remarks></remarks>
    Public Sub CreateMskForAllCommutations(Rg2Path As String,
                                            AreasPath As String,
                                            CommutationsPath As String,
                                            CommutationNamesPath As String,
                                            SavePath As String,
                                            HelperEnabled As Boolean)
        Try

			Dim AreasList As New List(Of Area)
			Dim CommutationsInst As New CommutationsHolder
			Dim CommList As New List(Of Commutation)
			Dim NamesList As New List(Of CommutationName)

			CheckFiles(AreasPath, CommutationsPath, CommutationNamesPath, AreasList, CommList , NamesList)

			CommutationsInst.SetCommutations = CommList
			CommutationsInst.Names = NamesList

            For Each item As CommutationName In CommutationsInst.Names
                CommutationsInst.SetVariant = CULng(item.n)

                Dim raschet_info As String = "Расчет: " &
                                                    Rg2Path &
                                                    "_" &
                                                    CommutationsInst.GetVariantNumber &
                                                    "_" &
                                                    CommutationsInst.GetVariantName

                'Лог
                My.Application.Log.DefaultFileLogWriter.BaseFileName = Threading.Thread.CurrentThread.Name
                My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
                My.Application.Log.DefaultFileLogWriter.AutoFlush = True

                My.Application.Log.WriteEntry(System.Threading.Thread.CurrentThread.Name.ToString & _
                raschet_info)


                Console.WriteLine(raschet_info)


                InitMksForSpecifiedCommutations( _
                    SavePath,
                    Rg2Path,
                    AreasList,
                    CommutationsInst.GetVariantBranches,
                    CommutationsInst.GetVariantNumber &
                    "_" &
                    CommutationsInst.GetVariantName)
            Next

        Catch ex As Exception

            My.Application.Log.WriteException(ex, _
                                              TraceEventType.Error, _
                                              "Ошибка функции расчета МСК CreateMskForManyCommutations. Подробно: " & ex.Message)

            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")

        End Try
    End Sub
    ''' <summary>
    ''' Начинает создание одной МСК для заданного режима и ремонтов
    ''' </summary>
    ''' <param name="PathToSave">путь для сохранения</param>
    ''' <param name="Rg2Name">путь к файлу рж2</param>
    ''' <param name="AreasList">список районов для формирования МСК</param>
    ''' <param name="Commutations">список коммутаций для этой МСК</param>
    ''' <param name="VariantName">имя выходного файла, который будет сохранен туда же, куда рж2</param>
    ''' <remarks></remarks>
    Friend Sub InitMksForSpecifiedCommutations(
                 PathToSave As String,
                 Rg2Name As String,
                 AreasList As List(Of Area),
                 Commutations As List(Of Commutation),
                 VariantName As String)
        Try
            If Commutations Is Nothing Or Commutations.Count = 0 Then
                My.Application.Log.WriteEntry(
                    "Для заданного варианта нет ветвей")
                Exit Sub
            End If
            Dim ResultsDict As Dictionary(
                                     Of ULong, 
                                     List(Of Section)) =
                                 GetMksForSpecifiedCommutations(
                                     Rg2Name,
                                     AreasList,
                                     Commutations)
            VariantName = (VariantName &
                           "_" &
                           System.IO.Path.GetFileName(Rg2Name)).
                       TrimEnd("."c, "r"c, "g"c, "2"c)
            WriteToCsv(
                PathToSave &
                VariantName &
                ".csv",
                ResultsDict)
        Catch ExceptionSetCommutationsOn As ApplicationException
            Exit Sub
        Catch ex As Exception

            My.Application.Log.WriteException(ex, _
                                          TraceEventType.Error, _
                                          "Ошибка функции расчета InitMksForSpecifiedCommutations. Подробно: " & ex.Message)

            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")

        End Try

    End Sub
    ''' <summary>
    ''' Главная функция, которая возвращает МСК для рассчитываемого режима
    ''' </summary>
    ''' <param name="RastrPath">Путь к рассчитываемому режиму</param>
    ''' <param name="AreasList">Список районов, в которых будет рассчитываться МСК</param>
    ''' <param name="Commutations">Список отключений, которые надо сделать в этом режиме</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetMksForSpecifiedCommutations(
                                    RastrPath As String,
                                    AreasList As List(Of Area),
                                    Commutations As List(Of Commutation)
                                    ) As Dictionary(
                                                   Of ULong, 
                                                   List(Of Section))
        Dim ExceptionAreaN As New ULong
        Try
            Dim ResultsDict As New Dictionary(Of _
                                          ULong,  _
                                          List(Of Section))
            Dim Rastr As New RastrObject(RastrPath)
            Rastr.SetCommutationsOn = Commutations
            Rastr.Regim = ""
            'Rastr.SetCommutationsOff = CommList

            For Each item As Area In AreasList
                ExceptionAreaN = item.n
                ResultsDict.Add(
                    item.n,
                    CreateMskForSingleArea(
                        Rastr,
                        item.n,
                        Commutations))
            Next
            Return ResultsDict
        Catch ExceptionSetCommutationsOn As ApplicationException
            Throw ExceptionSetCommutationsOn
            Return New Dictionary(Of ULong, List(Of Section))
        Catch ex As Exception

            My.Application.Log.WriteException(ex, _
                                          TraceEventType.Error, _
            "Ошибка функции расчета МСК GetMksForSpecifiedCommutations. Возможно, не удается установить связь с Растр или ошибка в файле районов. Или отсутствует район " & ExceptionAreaN &
                "Подробно: " & ex.Message)


            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
            Return New Dictionary(Of ULong, List(Of Section))
        End Try
    End Function

	

End Module
