Imports IDAUtil
Imports [lib]

Module Controller
    Sub executeSwitchTask(salesOrg As String)
        Dim id As String = $"{DateTime.Now} {Environment.MachineName}"
        Dim dbServer As IDBServerConnector = Create.dbServer
        Dim sap As ISAPLib = Create.sapLib
        Dim mu As IMailUtil = Create.mailUtil

        Dim dcServer As New DataCollectorServer(dbServer)
        Dim dcSap As New DataCollectorSap(sap, Create.exportParses)
        Dim distList As New DistributionListCalculator(dbServer)
        Dim dc As New DataCollectorServiceSwitches(dcServer, dcSap)
        Dim executor As New SwitchesTaskExecutor(salesOrg, id)
        Dim email As String = distList.getDistList(salesOrg, "switches")

        executor.startLogs(salesOrg)

        If Not executor.calculateSwitches(dcServer, dcSap) Then
            mu.mailSimple(email, $"{salesOrg} No manual or automatic switches {DateTime.Now}", $"Hello<br><br>There are no orders in ZV04I<br><br>Kind Regards<br>IDA")

            executor.endLogs(salesOrg, "Manual Switches", "empty list")
            executor.endLogs(salesOrg, "Automatic Switches", "empty list")
            Return
        End If

        If executor.switchList.Count > 0 Then executor.populateSwitchesLog(dbServer)

        If executor.ManualSwitchesList.Count > 0 Then
            mu.mailSimple(email, $"{salesOrg} Manual Switches {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(executor.ManualSwitchesList)}<br><br>Kind Regards<br>IDA")
            executor.endLogs(salesOrg, "Manual Switches", "success")
        Else
            mu.mailSimple(email, $"{salesOrg} No Manual Switches {DateTime.Now}", $"Hello<br><br>There are no manual switches for you to action<br><br>Kind Regards<br>IDA")
            executor.endLogs(salesOrg, "Manual Switches", "empty list")
        End If

        If executor.AutomaticSwitchesList.Count > 0 Then
            mu.mailSimple(email, $"{salesOrg} Automatic Switches {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(executor.AutomaticSwitchesList)}<br><br>Kind Regards<br>IDA")

            executor.createAutomaticSwitchObjectList()

            Try
                If executor.runSwitchesInVA02() Then
                    executor.endLogs(salesOrg, "Automatic Switches", "success")
                    executor.sendFailedSwitches(salesOrg, email, dbServer, mu)
                    executor.sendReplacedCMIRs(salesOrg, email, mu, dbServer)
                Else
                    executor.sendFailedSwitches(salesOrg, email & ";DLGBFrmsOTDCENTRALISEDTEAM@scj.com", dbServer, mu)
                    mu.mailSimple(email & ";DLGBFrmsOTDCENTRALISEDTEAM@scj.com", $"{salesOrg} Too many switches to performed in this release.  {DateTime.Now}", $"Hello<br><br>There are so many switches even 3 sap sessions on one computer isn't enough. What if I told you {executor.AutomaticSwitchesList.Count} orders have switches in them? Centralised team please execute manually among yourselves as its too much to handle by one computer. <br><br>Kind Regards<br>IDA")
                End If
            Catch ex As Exception
                executor.handleError(salesOrg, mu, ex)
                executor.sendFailedSwitches(salesOrg, email & ";DLGBFrmsOTDCENTRALISEDTEAM@scj.com", dbServer, mu)
            End Try
        Else
            mu.mailSimple(email, $"{salesOrg} No Automatic Switches {DateTime.Now}", $"Hello<br><br>There are no automatic switches for IDA to action<br><br>Kind Regards<br>IDA")
            executor.endLogs(salesOrg, "Automatic Switches", "empty list")
        End If
    End Sub
End Module
