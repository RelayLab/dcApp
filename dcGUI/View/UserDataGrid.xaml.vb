Imports Microsoft.Win32
Public Class UserDataGrid

    Private Sub ChoosePath_Executed(sender As Object, e As ExecutedRoutedEventArgs)
        Dim FD As New OpenFileDialog()
        FD.Multiselect = False
        FD.Title = "Выберите путь к файлу"
        FD.CheckFileExists = True
        FD.CheckPathExists = True
        FD.ShowDialog()
        CType(e.OriginalSource, System.Windows.Controls.TextBox).Text = FD.FileName
    End Sub
End Class
