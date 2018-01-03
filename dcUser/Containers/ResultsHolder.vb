Option Explicit On
Option Infer On
Option Strict On
Imports System.Collections.Generic
''' <summary>
''' Контейнер, который предоставляет свойства для работы с результатами, полученными из РАСТР
''' </summary>
''' <remarks></remarks>
Friend Class ResultsHolder
    Private NonDisturbedBranchesHolder As List(Of Branch)
    Private DisturbedBranchesHolder As List(Of Branch)
    Private NonDisturbedNodesHolder As List(Of Node)
    Private NetCoeff As List(Of Section)
    Friend WriteOnly Property NonDisturbedNodes() As List(Of Node)
        Set(InputList As List(Of Node))
            NonDisturbedNodesHolder = InputList
        End Set
    End Property
    Friend WriteOnly Property NonDisturbedBranches() As List(Of Branch)
        Set(InputList As List(Of Branch))
            NonDisturbedBranchesHolder = InputList
        End Set
    End Property
    Friend Overloads ReadOnly Property GetDisturbanceArea As List(Of Node)
        Get
            Dim Psum As Double = 0
                'Aggregate item As Node In NonDisturbedNodesHolder
                'Into Sum(item.pg)

			For each item As Node In Me.NonDisturbedNodesHolder
				Psum=Psum+item.pg
			Next
            For Each item As Node In NonDisturbedNodesHolder
                item.dp = item.pg / Psum
            Next
            Return NonDisturbedNodesHolder
        End Get
    End Property
    Friend WriteOnly Property DisturbedBranches() As List(Of Branch)
        Set(InputList As List(Of Branch))
            DisturbedBranchesHolder = InputList
        End Set
    End Property
    Friend Function FindNetCoeff() As List(Of Section)

        For Each item As Branch In DisturbedBranchesHolder
            If item.ip_na > item.iq_na Then
                Dim temp As ULong = item.ip_na
                item.ip_na = item.iq_na
                item.iq_na = temp
                item.d_pb = -item.d_pb
            End If
        Next

       ' Dim Sections = _
       '     From item As Branch In DisturbedBranchesHolder
       '     Group By ip_na = item.ip_na, _
       '                 iq_na = item.iq_na _
       '     Into Coeff = Sum(item.d_pb)
       '     Order By iq_na
       '     Order By ip_na
	   'сгркппировать результаты по району начала и району конца, в каждой группе просуммировать поле d_pb, уполрядочить по iq_na и ip_na

	   'сгруппировать результаты по району начала и району конца, в каждой группе просуммировать поле d_pb
		Dim Sections As New List(Of Section)
		For Each br As Branch In DisturbedBranchesHolder
			For Each sec as Section in Sections
				'если сечение с таким номером начаала и конца есть, то прибавить к нему мощность этой веетви
				If (sec.ip_na=br.ip_na And sec.iq_na=br.iq_na)
					sec.p = sec.p + br.d_pb
				Else
					'иначе добавить новое сечение с таким номером
					Dim SectionToAdd As New Section(br.ip_na,br.iq_na,br.d_pb)
					Sections.Add(SectionToAdd)
				End If
			Next
		Next

		'уполрядочить по iq_na и ip_na,чтобы сначала шли по возрастанию ip, затем iq
		'в определении класса Section функция Sort сортирует по полю value_to_sort
		For each item As Section in Sections
			item.value_to_sort=item.iq_na
		Next
		Sections.Sort()

		For each item As Section in Sections
			item.value_to_sort=item.ip_na
		Next
		Sections.Sort()

        NetCoeff = Sections
        'NetCoeff = New List(Of Section) Раньше сортировка использовала LINQ, но на mono недоступен VB9 

        'For Each item In Sections
        '    NetCoeff.Add(New Section(item.ip_na, _
        '                             item.iq_na, _
        '                             item.Coeff))
        'Next
        Return NetCoeff
    End Function
End Class
