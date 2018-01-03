Option Explicit On
Option Infer On
Option Strict On
Imports System.Collections.Generic

Friend Module MethodModule
    Friend Function CreateMskForSingleArea( _
                          Rastr As RastrObject, _
                          Area As ULong, _
                          CommutationsList As List(Of Commutation)) _
                      As List(Of Section)

        Dim Results As New ResultsHolder
        Rastr.SetArea = Area
        Results.NonDisturbedNodes = Rastr.NodesTable
        Rastr.SetDisturbanceTable = Results.GetDisturbanceArea
        Results.DisturbedBranches = Rastr.BranchesTable


        Return Results.FindNetCoeff()
    End Function
End Module
