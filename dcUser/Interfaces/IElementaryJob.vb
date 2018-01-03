'PLEASE DO NOT MODIFY THIS INTERFACE
'YOU SHOULD IMPLEMENT IT IN YOR PROJECT
'IN YOUR CUSTOM "Elementary Job" CLASS
Public Interface IElementaryJob
    Property IsDone As Boolean
    Property IsProcessing As Boolean
    Property Id As ULong
    Property Name As String
    Property ClientName As String
End Interface
