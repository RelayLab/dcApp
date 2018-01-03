Option Explicit On
Option Infer Off
Option Strict On
''' <summary>
''' Класс, представляющий список отключений (ремонтов), их названий
''' </summary>
Imports System.Collections.Generic

Friend Class CommutationsHolder
    Private Commutations As List(Of Commutation)
    Private CommutationsNames As List(Of CommutationName)
    Private NumberOfVariant As ULong
    ''' <summary>
    ''' номер варианта, для которого надо получить ветви
    ''' </summary>
    Friend WriteOnly Property SetVariant() As ULong
        Set(value As ULong)
            Me.NumberOfVariant = value
        End Set
    End Property
    Friend WriteOnly Property SetCommutations() As List(Of Commutation)
        Set(value As List(Of Commutation))
            Me.Commutations = value
        End Set
    End Property
    Friend Property Names() As List(Of CommutationName)
        Set(value As List(Of CommutationName))
            Me.CommutationsNames = value
        End Set
        Get
            Return Me.CommutationsNames
        End Get
    End Property
    Friend ReadOnly Property GetVariantBranches As List(Of Commutation)
        Get
            Try
                'Dim OutputList As List(Of Commutation) = (From item As Commutation In Me.Commutations Where item.n = NumberOfVariant).ToList
                Dim OutputList As New List(Of Commutation)
				For each item as Commutation in Me.Commutations
					If item.n = NumberOfVariant Then _
						OutputList.Add(item)
				Next

                Return OutputList
            Catch ex As Exception
                Return New List(Of Commutation)
            End Try

        End Get
    End Property
    Friend ReadOnly Property GetVariantName As String
        Get
            Try
                'Return Me.CommutationsNames.Find(Function(x) _ Return x.n = Me.NumberOfVariant _ End Function).Name
				For each item As CommutationName in Me.CommutationsNames
					If item.n = Me.NumberOfVariant Then 
						Return item.Name
					End If
				Next
            Catch ex As Exception
                Return "Ошибка - номер ремонта " & NumberOfVariant & " не найден в списке названий ремонтов"
            End Try

        End Get
    End Property
    Friend ReadOnly Property GetVariantNumber As String
        Get
            Return Str(Me.NumberOfVariant)
        End Get
    End Property

End Class
