Public Module LogicModule
    Public Sub ExecuteSingleJob( _
                                 InputJob As IElementaryJob, _
                                 OutputResult As IJobResult, _
                                 Optional LogEnabled As Boolean = True, _
                                 Optional InterimSaveEnabled As Boolean = False)

        Dim Job As ElementaryJob = CType(InputJob, ElementaryJob)

        CreateMskForAllCommutations(Job.File, _
                                    Job.AreasPath, _
                                    Job.LinesPath, _
                                    Job.Name, _
                                    Job.SavePath, _
                                    HelperEnabled:=True)

        OutputResult.JobId = Job.Id



    End Sub
End Module

