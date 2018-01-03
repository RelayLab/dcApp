
<Serializable>
Public Class JobResult
    Implements IJobResult

    Public Property JobId As ULong Implements IJobResult.JobId
		Set(x as ULong)
			JobId = x
		End Set
		Get
			Return JobId
		End Get
	End Property

    Public Property Result As Object Implements IJobResult.Result
		Set(x as Object)
			Result = x
		End Set
		Get
			Return Result
		End Get
	End Property

End Class
