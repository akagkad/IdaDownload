Imports [lib]

Public Class GlobalErrorHandler
    Public Sub New()
    End Sub

    Public Sub handle(salesOrg As String, task As String, ex As Exception)
        Dim mu As IMailUtil = Create.mailUtil
        Dim f = Create.fileUtil
        Dim filepath = $"{IDAConsts.errorFilePath}\{salesOrg} {task} error {Format(DateTime.Now, "yyyy.MM.dd mm.hh")}.txt"
        f.writeToTxtFile(filepath, ex.Source & vbCr & ex.Message & vbCr & ex.StackTrace)

        mu.mailSimple($"{adminEmail};{Environment.UserName}", $"{salesOrg} {task}: Error {Err.Number}", $"{mu.getLink(filepath, "Your error info is here")}")
    End Sub

End Class
