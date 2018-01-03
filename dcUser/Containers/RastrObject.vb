Option Explicit On
Option Infer Off
Option Strict Off
Imports System.Diagnostics
Imports System.Collections.Generic
Friend Class RastrObject
    Private Area As ULong
    Private RastrComObject As Object
    Friend Sub New()
        RastrComObject = CreateObject("Astra.Rastr")
    End Sub
    Friend Sub New(FilePath As String)
        Try
            RastrComObject = CreateObject("Astra.Rastr")
            With RastrComObject
                .Load(1, FilePath, "")
                .Tables("vetv").Cols.Add("ip_na", 0)
                .Tables("vetv").Cols.Add("iq_na", 0)
                .Tables("vetv").Cols("ip_na").Prop(5) = "ip.na"
                .Tables("vetv").Cols("iq_na").Prop(5) = "iq.na"
            End With
        Catch ex As Exception
            MsgBox("Ошибка. Возможно, не удается получить доступ к Растр" &
                   "или имя файла " & FilePath & "указано неправильно." &
                    "Подробно: " & ex.Message)
        End Try
    End Sub
    Friend WriteOnly Property SetArea As ULong
        Set(value As ULong)
            Area = value
        End Set
    End Property
    Friend Overloads ReadOnly Property BranchesTable As List(Of Branch)
        Get
            Try
                Dim OutputList As New List(Of Branch)
                Dim Branches As Object = RastrComObject.Tables("vetv")

                With Branches
                    .SetSel("ip.na!=iq.na")

                    Dim row As Integer = 0
                    Do

                        row = .FindNextSel(row)
                        If row = -1 Then Exit Do Else
                        OutputList.Add(New Branch(
                                                    .Cols("ip").Z(row),
                                                    .Cols("ip_na").Z(row),
                                                    .Cols("iq").Z(row),
                                                    .Cols("iq_na").Z(row),
                                                    .Cols("d_pb").Z(row)))
                    Loop
                End With

                RastrComObject.sensiv_end()

                Return OutputList
            Catch ex As Exception
                
                    My.Application.Log.WriteException(ex, TraceEventType.Error, _
                    "Ошибка. Невозможно получить доступ к таблице ветвей. Подробно: " & ex.Message)
                
                Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
                Return New List(Of Branch)
            End Try
        End Get
    End Property
    Friend Overloads ReadOnly Property NodesTable As List(Of Node)
        Get
            Try
                Dim OutputList As New List(Of Node)
                Dim Nodes As Object= RastrComObject.Tables("node")

                With Nodes
                    .SetSel("na=" + Str(Area) + "&pg>0&sta=0")

                    Dim row As Integer = 0
                    Do
                        row = .FindNextSel(row)
                        If row = -1 Then Exit Do Else
                        OutputList.Add(New Node(
                                                   .Cols("ny").Z(row),
                                                   .Cols("na").Z(row),
                                                   .Cols("pg").Z(row),
                                                   .Cols("sta").Z(row)))
                    Loop
                End With

                Return OutputList
            Catch ex As Exception
                
                    My.Application.Log.WriteException(ex, TraceEventType.Error, _
                    "Ошибка. Район не найден." & Area &
                    "Подробно: " & ex.Message)
                
                Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")
                    
                'такого района не найдено. Нельзя выводить мсгбокс, т.к. пусть работает дальше
            End Try
            Return New List(Of Node)
        End Get
    End Property
    Friend WriteOnly Property SetDisturbanceTable As List(Of Node)
        Set(InputList As List(Of Node))
            RastrComObject.sensiv_start("")

            For Each item As Node In InputList
                RastrComObject.sensiv_forw(
                                            0,
                                            item.ny,
                                            item.dp)
            Next
            RastrComObject.sensiv_write("")
        End Set
    End Property
    Friend WriteOnly Property Regim As String
        Set(value As String)
            RastrComObject.rgm(value)
        End Set
    End Property
    Friend WriteOnly Property SetCommutationsOn As List(Of Commutation)
        Set(CommutationsList As List(Of Commutation))
            Dim ExceptionCommutation As New Commutation
            Try
                Dim Branches As Object = Me.RastrComObject.Tables("vetv")

                For Each item As Commutation In CommutationsList
                    ExceptionCommutation = item
                    Branches.SetSel( _
                        "ip=" & _
                        Str(item.ip) & _
                        "iq=" & _
                        Str(item.iq) & _
                        "np=" & _
                        Str(item.np))
                    Dim row As UInteger = Branches.FindNextSel(-1)
                    Branches.Cols("sta").Z(row) = item.sta
                Next

            Catch ex As Exception
                
                    My.Application.Log.WriteException(ex, TraceEventType.Error, _
                    "Ошибка. Ветвь " &
                    ExceptionCommutation.ip & "-" &
                    ExceptionCommutation.iq & " не найдена." &
                    "Подробно: " & ex.Message)
                
                Console.Error.WriteLine("Возникла ошибка. Обратитесь к файлу лога.")

                Throw New ApplicationException
                'ветвей не найдено. продолжить работу
            End Try

        End Set
    End Property
    Friend WriteOnly Property SetCommutationsOff As List(Of Commutation)
        Set(CommutationsList As List(Of Commutation))
            For Each Item As Commutation In CommutationsList
                If Item.sta = 1 Then
                    Item.sta = 0
                Else
                    Item.sta = 1
                End If
            Next
            Me.SetCommutationsOn = CommutationsList
        End Set
    End Property
End Class
