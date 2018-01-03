Option Explicit On
Option Infer On
Option Strict On
''' <summary>
''' класс, представляющий номер района БН
''' </summary>
Friend Class Area
    Implements IWritable
    Friend n As ULong
    Friend WriteOnly Property Arguments() As String() _
                    Implements IWritable.Arguments
        Set(InputArray As String())
            Try
                Me.n = CType(InputArray(0), ULong)
            Catch ex As Exception
                MsgBox("Ошибка чтения района из файла. Возможно, что-то задано неправильно. Подробно:  " & ex.Message)
            End Try

        End Set
    End Property
End Class
