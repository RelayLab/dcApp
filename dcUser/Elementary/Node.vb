Option Explicit On
Option Infer On
Option Strict On

Friend Class Node
    Friend ny As ULong
    Friend na As ULong
    Friend pg As Double
    Friend sta As ULong
    Friend dp As Double

    Friend Sub New(ny As ULong, _
                    na As ULong, _
                    pg As Double, _
                    sta As ULong)
        Me.ny = ny
        Me.na = na
        Me.pg = pg
        Me.sta = sta
    End Sub

End Class
