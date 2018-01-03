Imports System.ComponentModel


Public Class MainViewModel
    Implements INotifyPropertyChanged

    '<Поля с данными для байндинга>
    Public Property Status As String = "Готов"
    '</Поля с данными для байндинга>

    '<Обновление из синглтона>
    'Friend WithEvents Singleton As JobsSingleton = JobsSingleton.Instance
    'Private Sub UpdateStatus(InputStatus As String) Handles Singleton.StatusUpdated
    'Status = InputStatus
    'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Status"))
    'End Sub
    '</Обновление из синглтона>


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

