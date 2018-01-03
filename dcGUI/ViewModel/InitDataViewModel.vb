Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports dcUser

Public Class InitDataViewModel
    Implements INotifyPropertyChanged 'это чтобы VIEW видел изменения в VIEW MODEL, смотри внизу "служебные" функции

#Region "Поля с данными для байндинга"
    Public Property FilePathsList As ObservableCollection(Of InitData) = LoadExternalFiles.UserDataGrid_Loaded_Sub() 'при открытии окна читает данные из файла конфига для основной консольной программы
#End Region

#Region "Команды для байндинга"
    Public Property MakeJobsButtonClicked As New Command(AddressOf MakeJobsButtonSub) ' привязываемся к событию VIEW
    'Public Property ExcelButtonClicked As New Command(AddressOf ExcelButtonSub)
#End Region


#Region "Функции для байндинга"
    ''' <summary>
    ''' Проверить правильность путей и содержания файлов. Если все верно, то по заданному в либе dcUSER алгоритму сделать файл с единичными работами init_data.xml
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MakeJobsButtonSub()
        Dim init_data_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of InitData)))
        Dim writer As New System.IO.StreamWriter("init_data.xml")
        init_data_serializer.Serialize(writer, FilePathsList.Cast(Of InitData).ToList) 'здесь тип ObservableCollection (Of T) нужно привести к List (Of T), 
        'т.к. Observable имеет смысл только в ViewModel, а не дальше в программе
        writer.Close()
        RunExtProg("make_jobs.exe", "") 'консольная прога, которая по алгоритму dcUser разбивает файл на работы.
        Me.UpdateStatus = "Преобразование сделано. Проверьте файл init_data.xml в папке программы"
    End Sub

#End Region

#Region "Служебные"
    Friend Event JobsUpdated As Action 'событие, чтобы обновить VM
    Friend Event StatusChanged As Action(Of String) 'событие для обновления полоски статуса MainVM из других VM
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    ''' <summary>
    ''' обновить MainVM
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Friend WriteOnly Property UpdateStatus As String
        Set(value As String)
            RaiseEvent StatusChanged(value)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(""))
        End Set
    End Property
#End Region

End Class