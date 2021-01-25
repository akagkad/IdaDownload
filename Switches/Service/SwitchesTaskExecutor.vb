Imports IDAUtil
Imports [lib]

Public Class SwitchesTaskExecutor
    Const isLog As Boolean = True 'when testing functions
    Private log As IServerLogger = Create.serverLogger(144)
    Private idaLog As New IdaLog

    Private ReadOnly salesOrg As String
    Private ReadOnly id As String

    Public switchList As New List(Of SwitchesProperty)
    Public AutomaticSwitchesList As New List(Of SwitchesProperty)
    Public ManualSwitchesList As New List(Of SwitchesProperty)
    Public automaticSwitchObjectList As New List(Of SwitchesSapOrderProperty)

    Public Sub New(salesOrg As String, id As String)
        Me.salesOrg = salesOrg
        Me.id = id
    End Sub

    Public Sub startLogs(salesOrg As String)
        If isLog Then
            log.start()
            idaLog.insertToActivityLog("Switches", "startTime", id, salesOrg)
        End If
    End Sub

    Public Sub populateSwitchesLog(dbserver As IDBServerConnector)
        dbserver.listToServer(switchList, "SwitchLog")
    End Sub

    Public Function calculateSwitches(dcServer As DataCollectorServer, dcSap As DataCollectorSap) As Boolean
        Dim DCSS As New DataCollectorServiceSwitches(dcServer, dcSap)
        Dim list As List(Of SwitchesProperty) = DCSS.getSwitchesList(salesOrg, id)

        If list Is Nothing Then
            Return False
        Else
            switchList = list
            AutomaticSwitchesList = switchList.Where(Function(x) x.switchAutomatic = True).ToList
            ManualSwitchesList = switchList.Where(Function(x) x.switchAutomatic = False).ToList
            Return True
        End If
    End Function

    Public Sub endLogs(salesOrg As String, switchType As String, status As String)
        If isLog Then
            log.finish(status)
            idaLog.insertToActivityLog(switchType, status, id, salesOrg)
        End If
    End Sub

    Public Sub createAutomaticSwitchObjectList()
        Dim switchOrderObj As New SwitchesOrderPropertyFactory
        automaticSwitchObjectList = switchOrderObj.getSapSwitchObjectList(AutomaticSwitchesList)
    End Sub

    Public Function runSwitchesInVA02() As Boolean

        Dim tableName As String = "SwitchLog"

        Select Case automaticSwitchObjectList.Count
            Case 1
                '1 session
                runExecution(1, tableName)
                Return True
            Case 2
                '2 sessions
                runExecution(2, tableName)
                Return True
            Case 3 To 100
                '3 sessions
                runExecution(3, tableName)
                Return True
            Case Else
                idaLog.update(
                    tableName,
                    {"startTime", "status", "reason", "endTime"},
                    {Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), "fail", $"Too many orders for switches - {automaticSwitchObjectList.Count}", Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
                    {"[id]", "[SwitchAutomatic]"},
                    {id, True}
                )
                Return False
        End Select
    End Function

    Public Sub runExecution(session As Byte, tableName As String)
        Dim taskList As New List(Of Task)
        Dim listList = automaticSwitchObjectList.SplitOnAmountOfLists(session)
        Dim realSession As Byte = (session - 1)

        For i As Byte = 0 To realSession
            Dim q = i 'multithreading issue solver
            taskList.Add(Task.Run(Sub() runSapSession(q, listList(q), tableName)))
        Next

        Try
            Task.WaitAll(taskList.ToArray)
        Catch ae As AggregateException
            Dim gle As New GlobalErrorHandler

            For Each innerX In ae.InnerExceptions
                gle.handle(salesOrg, "Automatic Switches", innerX)
            Next
        End Try


        While realSession > 0
            Dim sap As ISAPLib = Create.sapLib(realSession)
            sap.closeSessionWindow()
            realSession -= 1
        End While
    End Sub

    Private Sub runSapSession(session As Byte, list As List(Of SwitchesSapOrderProperty), tableName As String)
        Dim sap As ISAPLib

        If session > 0 Then
            sap = Create.sapLib()
            sap.createSession()
            Try
                sap = Create.sapLib(session)
            Catch ex As Exception
                Threading.Thread.Sleep(3000)
                sap = Create.sapLib(session)
            End Try
        Else
            sap = Create.sapLib(session)
        End If

        Dim va02 As New VA02(sap)

        For Each switchObj In list
            Dim status = va02.runSwitches(switchObj, id, tableName)
            updateOrderLog(tableName, switchObj, status)
        Next

    End Sub

    Private Sub updateOrderLog(tableName As String, switchObj As SwitchesSapOrderProperty, status As VA02.OrderStatus)
        Select Case status
            Case VA02.OrderStatus.success
                idaLog.update(
                    tableName,
                    {"endTime"},
                    {Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
                    {"[orderNumber]", "[id]", "[SwitchAutomatic]"},
                    {switchObj.order, id, True}
                )
            Case VA02.OrderStatus.failedToSave
                updateFailedOrderLog("Failed to save order", tableName, switchObj.order)
            Case VA02.OrderStatus.bothSkusAreZ4
                updateFailedOrderLog("Old and new skus are in Z4", tableName, switchObj.order)
            Case VA02.OrderStatus.blockedByBatchJob
                updateFailedOrderLog("Blocked by batch job", tableName, switchObj.order)
            Case VA02.OrderStatus.blockedByUser
                updateFailedOrderLog("Blocked by user", tableName, switchObj.order)
            Case VA02.OrderStatus.realeasedOrRejected
                updateFailedOrderLog("Order released or rejected", tableName, switchObj.order)
            Case VA02.OrderStatus.bothSkusAreNotApproved
                updateFailedOrderLog("Old and new skus are not approved", tableName, switchObj.order)
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub

    Private Sub updateFailedOrderLog(reason As String, tableName As String, orderNumber As Long)
        idaLog.update(
            tableName,
            {"status", "reason", "endTime"},
            {"fail", reason, Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
            {"[orderNumber]", "[id]", "[SwitchAutomatic]"},
            {orderNumber, id, True}
        )
    End Sub

    Public Sub sendFailedSwitches(salesOrg As String, email As String, dbServer As IDBServerConnector, mu As IMailUtil)
        Dim failedListQuery As String = $"Select * from SwitchLog where [id] = '{AutomaticSwitchesList.Item(0).id}' And (status is null or status <> 'success') AND SwitchAutomatic = 1"
        Dim rs As ADODB.Recordset = dbServer.executeQuery(failedListQuery)

        If Not rs.EOF Then mu.mailSimple(email, $"{salesOrg} Failed Switches {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items if needed<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA")
    End Sub

    Public Sub sendReplacedCMIRs(salesOrg As String, email As String, mu As IMailUtil, dbServer As IDBServerConnector)
        Dim failedListQuery As String = $"Select distinct * from CMIR where [salesOrg] = '{salesOrg}'"
        Dim rs As ADODB.Recordset = dbServer.executeQuery(failedListQuery)

        If Not rs.EOF Then mu.mailArg("DLGBFrmsOTDCENTRALISEDTEAM@scj.com", email, $"{salesOrg} Replaced CMIR's during switches {DateTime.Now}", $"Hello <br>Dear Centralised Team representative of the sales organisation: {salesOrg}<br>Please do the below cmir links that were discovered from the switch task<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA")

        dbServer.executeQuery($"Delete from CMIR where [salesOrg] = '{salesOrg}'")
    End Sub

    Sub handleError(salesOrg As String, mu As IMailUtil, ex As Exception)
        Dim filepath As String = $"{IDAConsts.errorFilePath}\{salesOrg} Switches error {Format(DateTime.Now, "yyyy.MM.dd mm.hh")}.txt"
        Dim f = Create.fileUtil
        f.writeToTxtFile(filepath, ex.Source & vbCr & ex.Message & vbCr & ex.StackTrace)

        mu.mailSimple($"{adminEmail};{Environment.UserName}", $"{salesOrg} Switches: Error {Err.Number}", $"{mu.getLink(filepath, "Your error info is here")}")
    End Sub
End Class
