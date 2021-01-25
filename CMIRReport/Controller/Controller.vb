Imports IDAUtil
Imports [lib]

Module Controller
    Public Sub executeMissingCMIRReport(salesOrg As String)
        Const isLog As Boolean = False 'when testing functions
        Dim log As IServerLogger = Create.serverLogger(140)
        Dim idaLog As New IdaLog
        Dim id As String = $"{DateTime.Now} {Environment.MachineName}"
        Dim dbServer As IDBServerConnector = Create.dbServer
        Dim sap As ISAPLib = Create.sapLib

        If isLog Then log.start()
        idaLog.insertToActivityLog("missingCMIR", "startTime", id, salesOrg)

        Dim dcServer As New DataCollectorServer(dbServer)
        Dim dcSap As New DataCollectorSap(sap, Create.exportParses)
        Dim distList As New DistributionListCalculator(dbServer)
        Dim dcService As New DataCollectorServiceCMIR(dcServer, dcSap)

        Dim missingCMIRList As List(Of MissingCMIRProperty) = dcService.getMissingCMIRS(salesOrg)

        Dim mu As IMailUtil = Create.mailUtil
        Dim email As String = distList.getDistList(salesOrg, "cmirCheck")

        If missingCMIRList Is Nothing Then
            If isLog Then log.finish("empty list")
            idaLog.insertToActivityLog("missingCMIR", "empty list", id, salesOrg)
            mu.mailSimple(email, $"{salesOrg} CMIR's missing in orders {DateTime.Now}", $"Hello<br><br><br>There are no orders in the ZV04P report<br><br>Kind Regards<br>IDA")
            Return
        End If

        If missingCMIRList.Count > 0 Then
            mu.mailSimple(email, $"{salesOrg} CMIR's missing in orders {DateTime.Now}", $"Hello<br><br><br>{mu.listToHTMLtable(missingCMIRList)}<br><br>Kind Regards<br>IDA")

            If isLog Then log.finish("success")
            idaLog.insertToActivityLog("missingCMIR", "success", id, salesOrg)
        Else
            If isLog Then log.finish("empty list")
            idaLog.insertToActivityLog("missingCMIR", "empty list", id, salesOrg)
            mu.mailSimple(email, $"{salesOrg} CMIR's missing in orders {DateTime.Now}", $"Hello<br><br><br>There are no missing CMIR's<br><br>Kind Regards<br>IDA")
        End If
    End Sub
End Module
