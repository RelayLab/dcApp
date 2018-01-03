Module RunExternalPrograms
    Friend Sub RunExtProg(ProgName As String, args As String) 'откуда-то стырена функция для запуска внешнего процесса с аргументами.

        Dim ExternalProcess As Process = New Process()

        ExternalProcess.StartInfo.FileName = ProgName
        ExternalProcess.StartInfo.Arguments = args
        ExternalProcess.StartInfo.WorkingDirectory = ".\"
        ExternalProcess.StartInfo.UseShellExecute = False
        ExternalProcess.StartInfo.CreateNoWindow = True
        ExternalProcess.StartInfo.RedirectStandardInput = True
        ExternalProcess.StartInfo.RedirectStandardOutput = True
        ExternalProcess.StartInfo.RedirectStandardError = True
        ExternalProcess.Start()

        Dim stdout As System.IO.StreamReader = ExternalProcess.StandardOutput
        Dim strerr As System.IO.StreamReader = ExternalProcess.StandardError
        Dim ok_message As String = stdout.ReadToEnd()
        Dim err_message As String = stdout.ReadToEnd()

        If Not ExternalProcess.HasExited Then
            ExternalProcess.Kill()
        End If
        stdout.Close()
        strerr.Close()
        ExternalProcess.Close()
    End Sub

End Module
