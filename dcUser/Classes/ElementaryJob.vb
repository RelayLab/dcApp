
<Serializable>
Public Class ElementaryJob
    Implements IElementaryJob

    Public Property ClientName As String Implements IElementaryJob.ClientName
		Set (x as String)
			ClientName=x
		End Set
		Get
			Return ClientName
		End Get
	End Property

    Public Property Id As ULong Implements IElementaryJob.Id
		Set (x as ULong)
			Id=x
		End Set
		Get
			Return Id
		End Get

	End Property
    Public Property IsDone As Boolean Implements IElementaryJob.IsDone
		Set (x as Boolean)
			IsDone=x
		End Set
		Get
			Return IsDone
		End Get

	End Property
    Public Property IsProcessing As Boolean Implements IElementaryJob.IsProcessing
			Set (x as Boolean)
			IsProcessing=x
		End Set
		Get
			Return IsProcessing
		End Get
	End Property
    Public Property Name As String Implements IElementaryJob.Name
 		Set (x as String)
			Name=x
		End Set
		Get
			Return Name
		End Get
	End Property

	Public File As String
    Public SavePath As String
    Public AreasPath As String
    Public LinesPath As String

End Class
