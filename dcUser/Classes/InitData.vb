
<Serializable>
Public Class InitData
    Implements IInitData

    Public Property AdditionalInfo As String Implements IInitData.AdditionalInfo
		Set(x as String)
			AdditionalInfo=x
		End Set
		Get
			Return AdditionalInfo
		End Get
	End Property

    Public Property MonitoredValuesPath As String Implements IInitData.MonitoredValuesPath
		Set(x as String)
			MonitoredValuesPath=x
		End Set
		Get
			Return MonitoredValuesPath
		End Get
	End Property

    Public Property SavePath As String Implements IInitData.SavePath
		Set(x as String)
			SavePath=x
		End Set
		Get
			Return SavePath
		End Get
	End Property

    Public Property VariantsNamesPath As String Implements IInitData.VariantsNamesPath
		Set(x as String)
			VariantsNamesPath=x
		End Set
		Get
			Return VariantsNamesPath
		End Get
	End Property

    Public Property VariantsPath As String Implements IInitData.VariantsPath
		Set(x as String)
			VariantsPath=x
		End Set
		Get
			Return VariantsPath
		End Get
	End Property

End Class
