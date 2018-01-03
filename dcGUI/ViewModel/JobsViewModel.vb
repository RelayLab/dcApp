
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Windows.Input
Imports dcUser

Public Class JobsViewModel
    Implements INotifyPropertyChanged 'это чтобы VIEW видел изменения в VIEW MODEL, смотри внизу "служебные" функции

#Region "Служебное"
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New() 'заполняет чекбокс числом логических процессоров
        For i As Integer = 0 To Environment.ProcessorCount
            Me.MaxThreadsList.Add(i)
        Next
    End Sub

#End Region

#Region "Поля с данными для байндинга"
    Public Property ElementaryJobs As ObservableCollection(Of ElementaryJob) = LoadExternalFiles.JobsPage_Loaded_Sub() 'при открытии окна прочитать данные из файла jobs.txt
    Public Property MaxThreadsList As List(Of Integer) = New List(Of Integer) 'доступный вариант чекбоксов
    Public Property MaxThreadsCount As Integer = 1 'значение в чекбоксе по умолчанию
    Public Property ServerName As String = My.Computer.Name 'имя компьютера
    Public Property IsRunning As Boolean = False 'делает неактивной кнопку запуска, если уже была нажата
#End Region

#Region "Команды для байндинга"
    Public Property StartLocalButtonClicked As New Command(AddressOf StartLocalButtonSub)
    Public Property StartClientButtonClicked As New Command(AddressOf StartClientButtonSub)
    Public Property StartServerButtonClicked As New Command(AddressOf StartServerButtonSub)
    Public Property StopButtonClicked As New Command(AddressOf StopButtonSub)
#End Region

#Region "Функции для байндинга"

    ''' <summary>
    ''' Остановить все расчеты
    ''' </summary>
    ''' <remarks>Могут останавливаться не сразу, т.к. может быть использован запрос остановки каждого треда в отдельности</remarks>
    Private Sub StopButtonSub()
        Me.IsRunning = False
        RunExtProg("taskkill /f /im process_jobs", "")
        'TODO ЗАВЕРШИТЬ ВСЕ РАСЧЕТЫ
        'Можно сделать по умному, с сигналом завершения, который будет проверяться консольной программой раз в цикл
        'можно просто taskkill
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsRunning"))
    End Sub
    ''' <summary>
    ''' Запустить вычисления локально
    ''' </summary>
    ''' <remarks>Использует одновременно части логики сервера и клиента</remarks>
    Private Sub StartLocalButtonSub()
        'запускает внешнюю консольную прогу с аргументами, которая читает файл с элементарными работами jobs.txt
        RunExtProg("process_jobs.exe", "-t" & Me.MaxThreadsCount)
        Me.IsRunning = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsRunning"))
    End Sub

    ''' <summary>
    ''' Запустить клиент
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StartClientButtonSub()
        'JobsSingleton.Instance.UpdateStatus("Клиент запущен")
        'Dim ThisPcInfo As New ClientInfo(Me.MaxThreadsCount, My.Computer.Name)
        'ClientLogic.StartClient(ThisPcInfo, ServerName)
        'Me.IsRunning = True
        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsRunning"))
    End Sub
    ''' <summary>
    ''' Запустить сервер
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StartServerButtonSub()
        'JobsSingleton.Instance.UpdateStatus("Сервер запущен")
        'dcNetwork.StartServer(AddressOf ServerLogic.ServerLoop)
        Me.IsRunning = True
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsRunning"))
    End Sub
#End Region



End Class