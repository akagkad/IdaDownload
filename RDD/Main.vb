Imports IDAUtil

Module Main
    'Public Sub Main()
    '    Dim salesOrg As String = "UA01"
    Public Sub Main(args As String())
        Dim salesOrg As String = args(0)

        Try
            executeRDDTask(salesOrg)
        Catch ex As Exception
            Dim gle As New GlobalErrorHandler
            gle.handle(salesOrg, "RDD", ex)
        End Try

    End Sub
End Module