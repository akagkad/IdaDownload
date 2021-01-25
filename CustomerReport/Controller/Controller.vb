Imports IDAUtil
Imports [lib]

Module Controller
    Sub executeCustomerMissingReport(salesOrg As String)
        Const isLog As Boolean = False 'when testing functions
        Dim log As IServerLogger = Create.serverLogger(139)
        Dim idaLog As New IdaLog
        Dim id As String = $"{DateTime.Now} {Environment.MachineName}"
        Dim dbServer As IDBServerConnector = Create.dbServer
        Dim sap As ISAPLib = Create.sapLib

        If isLog Then log.start()
        idaLog.insertToActivityLog("missingCustomers", "startTime", id, salesOrg)

        Dim dcServer As New DataCollectorServer(dbServer)
        Dim dcSap As New DataCollectorSap(sap, Create.exportParses)
        Dim distList As New DistributionListCalculator(dbServer)
        Dim dc As New DataCollectorServiceMCR(dcServer, dcSap)

        Dim dcService As New DataCollectorServiceMCR(dcServer, dcSap)
        Dim customerMissingList As List(Of MissingCustomersProperty) = dcService.getMissingCustomers(salesOrg)

        Dim mu As IMailUtil = Create.mailUtil
        Dim email As String = distList.getDistList(salesOrg, "missingCustomers")

        If customerMissingList Is Nothing Then
            If isLog Then log.finish("empty list")
            idaLog.insertToActivityLog("missingCustomers", "empty list", id, salesOrg)
            mu.mailSimple(email, $"{salesOrg} Customers missing in the Database {DateTime.Now}", $"Hello<br><br><br>There are no orders in ZV04HN<br><br>Kind Regards<br>IDA")
            Return
        End If

        If customerMissingList.Count > 0 Then
            mu.mailSimple(email, $"{salesOrg} Customers missing in the Database {DateTime.Now}", $"Hello<br><br><br>{mu.listToHTMLtable(customerMissingList)}<br><br>Kind Regards<br>IDA")

            If isLog Then log.finish("success")
            idaLog.insertToActivityLog("missingCustomers", "success", id, salesOrg)
        Else
            If isLog Then log.finish("empty list")
            idaLog.insertToActivityLog("missingCustomers", "empty list", id, salesOrg)
            mu.mailSimple(email, $"{salesOrg} Customers missing in the Database {DateTime.Now}", $"Hello<br><br><br>There are no missing customers in the database<br><br>Kind Regards<br>IDA")
        End If
    End Sub
End Module