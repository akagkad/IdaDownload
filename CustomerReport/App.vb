Imports IDAUtil

Module App
    'Public Sub Main()
    '    Dim salesOrg As String = "DE01"
    Public Sub Main(args As String())

        Dim salesOrg As String = args(0)

        Try
            executeCustomerMissingReport(salesOrg)
        Catch ex As Exception
            Dim gle As New GlobalErrorHandler
            gle.handle(salesOrg, "Missing Customers Report", ex)
        End Try
    End Sub

End Module