Option Explicit On
Option Infer On
Option Strict On
Imports System.IO
Imports System.Diagnostics
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.FileIO
Imports System.Collections.Generic

Friend Module FileIO

    ''' <summary>
    ''' Обобщенная функция для чтения из CSV в экземпляр класса, реализующего интерфейс IWritable
    ''' </summary>
    ''' <typeparam name="T">класс, реализующий интерфейс</typeparam>
    ''' <param name="file">путь к файлу, который необходимо прочитать</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function ReadFromCsv(file As String) As List(Of String())
        Try
            Dim OutputList As New List(Of String())
            Using Parser As New TextFieldParser(file, GetEncoding(1251))
                Parser.TextFieldType = FieldType.Delimited
                Parser.SetDelimiters(";")

                While Not Parser.EndOfData
                    OutputList.Add(Parser.ReadFields())
                End While
            End Using
            Return OutputList
        Catch ex As Exception
            
                My.Application.Log.WriteException( _
                ex, _
                TraceEventType.Error, _
                "Ошибка: " & file &
                ". Возможно, нет прав для доступа к файлу, его имя задано неверно или есть ошибки в файле" &
                " Подробно: " & ex.Message)
            
            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")

            Return New List(Of String())
        End Try
    End Function

    ''' <summary>
    ''' Сохраняет МСК в CSV файл
    ''' </summary>
    ''' <param name="file">название, с которым будет сохранена МСК</param>
    ''' <param name="InputDict">сохраняемая МСК</param>
    ''' <remarks></remarks>
    Friend Sub WriteToCsv(
                   file As String,
                    InputDict As Dictionary(
                                            Of ULong,
                                            List(Of Section)))
        Try
            Using SW As StreamWriter = New StreamWriter(file, False, GetEncoding(1251))

			'Это то, как получаем указатель на ппервый элемент, по документации МДСН
			Dim enumer As Dictionary(Of ULong, List(Of Section)).Enumerator = InputDict.GetEnumerator()
			enumer.MoveNext()
			Dim FirstElement As KeyValuePair(Of ULong,List(Of Section)) = enumer.Current

                For Each item As Section In FirstElement 'записать первую строчку с ip.na
                    SW.Write(";")
                    SW.Write(item.ip_na)
                Next
                SW.WriteLine(";;")

                For Each item As Section In FirstElement	'записать вторую строчку с iq.na
                    SW.Write(";")
                    SW.Write(item.iq_na)
                Next
                SW.WriteLine(";;")

                For Each key As ULong In InputDict.Keys 'записать все остальные строчки значениями коэфф
                    SW.Write(key)
                    For Each item As Section In InputDict.Item(key)
                        SW.Write(";")
                        SW.Write(item.p)
                    Next
                    SW.WriteLine(";;")
                Next

                SW.Close()

            End Using
        Catch ex As Exception
            
                My.Application.Log.WriteException( _
                ex, _
                TraceEventType.Error, _
                "Ошибка: " & file &
                ". Возможно, нет прав для доступа к файлу или его имя задано неверно" &
                " Подробно: " & ex.Message)
            
            Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
            Exit Sub
        End Try
    End Sub
    ' ''' <summary>
    ' ''' Записывает строку в файл лога
    ' ''' </summary>
    ' ''' <param name="TextToLog">Строка для лога</param>
    ' ''' <param name="LogFilePath">Путь к файлу лога</param>
    'Friend Sub Log(TextToLog As String, LogFilePath As String)
    '    Try
    '        Using SW As StreamWriter = New StreamWriter(LogFilePath, True, GetEncoding(1251))
    '            SW.WriteLineAsync(TextToLog)
    '        End Using
    '    Catch ex As Exception
    '        MsgBox("Ошибка: " & LogFilePath &
    '               ". Возможно, нет прав для доступа к файлу или его имя задано неверно" &
    '               " Подробно: " & ex.Message)
    '        Exit Sub
    '    End Try
    'End Sub
End Module

