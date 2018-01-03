Imports System.ComponentModel
Imports System.IO
Imports System.Text.Encoding


Public Class LogViewModel
    Implements INotifyPropertyChanged 'это чтобы VIEW видел изменения в VIEW MODEL, смотри внизу "служебные" функции


#Region "Поля с данными для байндинга"
    Public Property LogText As String 'текст всех файлов лога, который выводится в окне
#End Region

#Region "Команды для байндинга"
    Public Property RenewButtonClicked As New Command(AddressOf RenewButtonSub) 'нажатие кнопки обновить
    Public Property ClearButtonClicked As New Command(AddressOf ClearButtonSub) 'нажатие кнопки удалить
#End Region

#Region "Функции для байндинга"
    Private Sub RenewButtonSub() 'читает все файлы с расширением "log" в папке , записывает в текст
        Try
            Me.LogText = "" 'в этой переменной текст для вывода
            Dim log_files As String() = System.IO.Directory.GetFiles(My.Application.Info.DirectoryPath, "*.log")
            For Each File As String In log_files
                Me.LogText = Me.LogText & "#### Файл: " & File & "####" & vbNewLine 'просто форматирование для красоты
                Using SR As New StreamReader(File, GetEncoding(1251)) 'читать каждый файл
                    Me.LogText = Me.LogText & SR.ReadToEnd() & vbNewLine
                End Using
            Next

        Catch ex As Exception 'если что-то не так, то вывести ошибку
            MsgBox("Ошибка: " &
                   "Файлы логов в папке " &
                   My.Application.Info.DirectoryPath &
                   ". Возможно, нет прав для доступа к файлу или его имя задано неверно" &
                   " Подробно: " & ex.Message)
            Exit Sub
        End Try
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("")) 'обновить VIEW
    End Sub

    Private Sub ClearButtonSub() 'при нажатии кнопки "удаление" удаляет все файлы "log" в папке.
        Try
            Me.LogText = ""
            Dim log_files As String() = System.IO.Directory.GetFiles(My.Application.Info.DirectoryPath, "*.log")
            For Each File As String In log_files
                System.IO.File.Delete(File)
            Next

        Catch ex As Exception
            MsgBox("Ошибка: " &
                   "Файлы логов в папке " &
                   My.Application.Info.DirectoryPath &
                   ". Возможно, нет прав для доступа к файлу или его имя задано неверно" &
                   " Подробно: " & ex.Message)
            Exit Sub
        End Try
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("")) 'обновить VIEW
    End Sub
#End Region

#Region "Служебное"
    Public Sub New() 'при открытии окна загружает данные из файла лога
        RenewButtonSub()
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged 'для обновления VIEW
#End Region


End Class
