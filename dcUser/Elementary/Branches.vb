Option Explicit On
Option Infer On
Option Strict On
Friend Class Branch
    Implements IWritable
    Friend ip As ULong
    Friend ip_na As ULong
    Friend iq As ULong
    Friend iq_na As ULong
    Friend d_pb As Double
    Friend Sub New(ip As ULong, _
                    ip_na As ULong, _
                    iq As ULong, _
                    iq_na As ULong, _
                    d_pb As Double)
        Me.ip = ip
        Me.ip_na = ip_na
        Me.iq = iq
        Me.iq_na = iq_na
        Me.d_pb = d_pb
    End Sub
    Friend WriteOnly Property Arguments() As String() _
                        Implements IWritable.Arguments
        Set(InputArray As String())
            Try
                Me.ip = CType(InputArray(0), ULong)
                Me.ip_na = CType(InputArray(1), ULong)
                Me.iq = CType(InputArray(2), ULong)
                Me.iq_na = CType(InputArray(3), ULong)
                Me.d_pb = CType(InputArray(4), Double)
            Catch ex As Exception
                MsgBox("Ошибка чтения ветвей из файла ветвей. Возможно, что-то задано неправильно. Подробно:  " & ex.Message)
            End Try

        End Set
    End Property
End Class
