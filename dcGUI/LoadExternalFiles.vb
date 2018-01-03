Imports dcUser
'Сюда отдельно вынесены функции, которые используются VIEW MODEL для чтения файлов конфига и отображения их содержимого в VIEW
Module LoadExternalFiles
    ''' <summary>
    ''' Эта функция читает файл init_data.xml и загружает данные из него на вкладку "Исходные данные"
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Содержание функции очевидное</remarks>
    Friend Function UserDataGrid_Loaded_Sub() As System.Collections.ObjectModel.ObservableCollection(Of InitData)
        Dim init_data_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of InitData)))
        Dim input_data As New List(Of InitData)
        Try

            Dim reader As New System.IO.StreamReader("init_data.xml")
            Dim tmp As Object = init_data_serializer.Deserialize(reader)
            input_data = TryCast(tmp, List(Of InitData))
            reader.Close()
            Return New System.Collections.ObjectModel.ObservableCollection(Of InitData)(input_data) 'вот здесь возвращается не просто Лист, а Observable... т.к. именно такой тип нужен VIEW MODEL

        Catch ex As Exception
            MsgBox(
                "Ошибка чтения файла init_data.xml. Возможно, к нему нет доступа или текст содержит ошибку. Подробно: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Эта функция читает файл jobs.xml и загружает данные из него на вкладку "Единичные работы"
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function JobsPage_Loaded_Sub() As System.Collections.ObjectModel.ObservableCollection(Of ElementaryJob)
        Dim init_data_serializer As New System.Xml.Serialization.XmlSerializer(GetType(List(Of ElementaryJob)))
        Dim input_data As New List(Of ElementaryJob)
        Try

            Dim reader As New System.IO.StreamReader("jobs.xml")
            Dim tmp As Object = init_data_serializer.Deserialize(reader)
            input_data = TryCast(tmp, List(Of ElementaryJob))
            reader.Close()
            Return New System.Collections.ObjectModel.ObservableCollection(Of ElementaryJob)(input_data)

        Catch ex As Exception
            MsgBox(
                "Ошибка чтения файла jobs.xml. Возможно, к нему нет доступа или текст содержит ошибку. Подробно: " & ex.Message)
            Return Nothing
        End Try
    End Function
End Module


