Option Explicit On
Option Infer On
Option Strict On
Friend Class Commutation
    Implements IWritable
    Friend n As ULong
    Friend ip As ULong
    Friend iq As ULong
    Friend np As ULong
    Friend sta As ULong

    Friend WriteOnly Property Arguments() As String() _
                    Implements IWritable.Arguments
        Set(InputArray As String())
            Try
                Me.n = CType(InputArray(0), ULong)
                Me.ip = CType(InputArray(1), ULong)
                Me.iq = CType(InputArray(2), ULong)
                If InputArray(3) = "" Then InputArray(3) = "0"
                Me.np = CType(InputArray(3), ULong)
                If InputArray(3) = "" Then InputArray(3) = "0"
                Me.sta = CType(InputArray(4), ULong)
            Catch ex As Exception
                MsgBox("Ошибка чтения ремонта из файла ремонтов. Возможно, что-то задано неправильно. Подробно:  " & ex.Message)
            End Try

        End Set
    End Property
End Class