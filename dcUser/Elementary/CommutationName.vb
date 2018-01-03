Option Explicit On
Option Infer On
Option Strict On
Friend Class CommutationName
    Implements IWritable
    Friend n As ULong
    Friend Name As String
    Friend WriteOnly Property Arguments() As String() _
                    Implements IWritable.Arguments
        Set(InputArray As String())
            Try
                Me.n = CType(InputArray(0), ULong)
                Me.Name = InputArray(1)
            Catch ex As Exception
                MsgBox("Ошибка чтения названия ремонта из файла названий ремонтов. Возможно, что-то задано неправильно. Подробно:  " & ex.Message)
            End Try

        End Set
    End Property
End Class
