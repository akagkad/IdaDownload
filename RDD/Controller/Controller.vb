Imports IDAUtil
Imports [lib]

Public Module Controller
    Public Sub executeRDDTask(salesOrg As String)
        Dim id As String = $"{DateTime.Now} {Environment.MachineName}"
        Dim dbServer As IDBServerConnector = Create.dbServer
        Dim sap As ISAPLib = Create.sapLib

        Dim dcServer As New DataCollectorServer(dbServer)
        Dim dcSap As New DataCollectorSap(sap, Create.exportParses)
        Dim distList As New DistributionListCalculator(dbServer)
        Dim dc As New DataCollectorServiceRDD(dcServer, dcSap)
        Dim email As String = distList.getDistList(salesOrg, "deliveryDateChanges")

        Dim executor As New RddTaskExecutor(salesOrg)

        Try
            executor.startLogs(salesOrg, id)
            executor.setRddList(salesOrg, id, dc)

            'check when sap is empty
            If executor.rddList Is Nothing Then
                executor.emptyListAction(salesOrg, id, email, True)
                Return
            End If

            executor.setBHList(salesOrg, dc)
            executor.prepareList()
            executor.calculateLists()

            If executor.finalList.Count > 0 Then
                executor.populateDeliveryDatesLog(dbServer)
                executor.sendEmailWithChanges(salesOrg, email)
                'TODO: move to multisession
                executor.runVA02(sap)
                executor.sendFailedRdds(salesOrg, email, dbServer)
            Else
                executor.emptyListAction(salesOrg, id, email, False)
            End If

            'If salesOrg = "IT01" Then executor.sendIT01Report(salesOrg, email, dbServer)
        Catch ex As Exception
            executor.finishLogs(salesOrg, id, "fail")
            executor.sendFailedRdds(salesOrg, email, dbServer)
            executor.handleError(salesOrg, id, ex)
            dbServer.closeConnection()
            Return
        End Try

        executor.finishLogs(salesOrg, id, "success")
        dbServer.closeConnection()
    End Sub
End Module
