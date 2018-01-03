Option Explicit On
Option Infer On
Option Strict On

Friend Class Section
	Implements IComparable(Of Section) 'используем готовый интерфейс для сравнения, т.к. понадобится сортировка

    Public ip_na As ULong
    Public iq_na As ULong
    Public p As Double
    Public value_to_sort As ULong

    Friend Sub New(ip_na As ULong, _
                   iq_na As ULong, _
                   p As Double)
        Me.ip_na = ip_na
        Me.iq_na = iq_na
        Me.p = p
    End Sub

	Friend Overloads Function CompareTo(ByVal other As Section) As Integer _
		Implements IComparable(Of Section).CompareTo 'функция из интерфейса, используется во внешней функции List.Sort()
		Return Me.value_to_sort.CompareTo(other.value_to_sort) 'на самом деле, это просто обертка для сравнения нужных полей
	End Function
End Class
