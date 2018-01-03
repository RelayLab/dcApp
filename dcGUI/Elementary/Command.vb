Public Class Command
    Implements ICommand
    Private Property ActionToExecute As Action
    Public Sub New(InputAction As Action)
        ActionToExecute = InputAction
    End Sub

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return True
    End Function

    Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        ActionToExecute.Invoke()
    End Sub
End Class
