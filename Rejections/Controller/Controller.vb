Imports IDAUtil
Imports [lib]

Module Controller
    Sub executeRejectionsDuringReleaseTask(salesOrg As String, isRunDuringRelease As Boolean)
        Dim id As String = $"{DateTime.Now} {Environment.MachineName}"
        Dim dbServer As IDBServerConnector = Create.dbServer
        Dim sap As ISAPLib = Create.sapLib
        Dim mu As IMailUtil = Create.mailUtil

        Dim dcServer As New DataCollectorServer(dbServer)
        Dim dcSap As New DataCollectorSap(sap, Create.exportParses)

        Dim dlc As New DistributionListCalculator(dbServer)
        Dim email As String = dlc.getDistList(salesOrg, "rejections")

        Dim executor As New RejectionsTaskExecutor(salesOrg, id)

        If Not executor.calculateLists(dcServer, dcSap) Then
            executor.endLogs(salesOrg, "Release Rejection", "Empty List")

            mu.mailSimple(email, $"{salesOrg} No Rejections {DateTime.Now}", $"Hello<br><br>There are no orders in ZV04I<br><br>Kind Regards<br>IDA")
            Return
        End If

        executor.populateRejectionLog(dbServer)

        If executor.afterReleaseRejectionList.Count > 0 Then
            mu.mailSimple(email, $"{salesOrg} After release rejections {DateTime.Now}", $"Hello<br><br>Here are the rejections that will be executed after last release today:<br><br>{mu.listToHTMLtable(executor.afterReleaseRejectionList)}<br><br>Kind Regards<br>IDA")
        End If

        If executor.releaseRejectionList.Count > 0 Then
            mu.mailSimple(email, $"{salesOrg} Release rejections {DateTime.Now}", $"Hello<br><br>Here are the rejections during release:<br><br>{mu.listToHTMLtable(executor.releaseRejectionList)}<br><br>Kind Regards<br>IDA")

            executor.runVA02Rejections(sap)
        End If
    End Sub
End Module
