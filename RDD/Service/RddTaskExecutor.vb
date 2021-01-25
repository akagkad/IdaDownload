Imports IDAUtil
Imports [lib]
Public Class RddTaskExecutor
    Const isLog As Boolean = False 'when testing functions
    Private log As IServerLogger = Create.serverLogger(138)
    Private idaLog As New IdaLog

    Private ReadOnly mu As IMailUtil = Create.mailUtil
    Private salesOrg As String

    Public rddList As New List(Of RddOutputBean)
    Private bhList As New List(Of BankHolidayProperty)
    Private calculatedRDDList As New List(Of CalculatedRddOutputBean)
    Private calculatedDelBlockList As New List(Of CalculatedRddOutputBean)
    Private calculatedRouteCodeList As New List(Of CalculatedRddOutputBean)
    Private calculatedList As New List(Of CalculatedRddOutputBean)

    Public preparedList As New List(Of CalculatedRddOutputBean)
    Public finalList As New List(Of CalcModel)

    Public Sub New(salesOrg As String)
        Me.salesOrg = salesOrg
    End Sub

    Public Sub startLogs(salesOrg As String, id As String)
        If isLog Then log.start()
        idaLog.insertToActivityLog("DeliveryDateChanges", "startTime", id, salesOrg)
    End Sub

    Public Sub setBHList(salesOrg As String, dc As DataCollectorServiceRDD)
        bhList = dc.getBHList(salesOrg)
    End Sub

    Public Sub setRddList(salesOrg As String, id As String, dc As DataCollectorServiceRDD)
        rddList = dc.GetRddList(salesOrg, id)
    End Sub

    Public Sub prepareList()
        Dim rddCalc As New RddDataCalculator(bhList, BHUtil.isSkipWeekend(salesOrg))
        Dim list As New List(Of CalculatedRddOutputBean)
        Select Case salesOrg
            Case "ZA01", "KE02", "NG01"
                list = rddCalc.getCalculatedRDDList(rddList).Where(Function(x) x.oldRdd <> x.newRecommendedRdd OrElse (x.oldRdd = x.newRecommendedRdd AndAlso (x.delBlock <> "" OrElse x.newRecommendedRouteCode <> ""))).ToList()
            Case Else
                list = rddCalc.getCalculatedRDDList(rddList).Where(Function(x) x.oldRdd < x.newRecommendedRdd OrElse (x.oldRdd = x.newRecommendedRdd AndAlso (x.delBlock <> "" OrElse x.newRecommendedRouteCode <> ""))).ToList()
        End Select

        Dim sortedList As New List(Of CalculatedRddOutputBean)
        sortedList = list.OrderBy(Function(x) x.delBlock).ToList()

        preparedList = sortedList
    End Sub

    Public Sub calculateLists()
        'separating into calculated lists
        calculatedRDDList = preparedList.Where(Function(x) x.delBlock = "" AndAlso x.isRddChangeAllowed).ToList()
        calculatedDelBlockList = preparedList.Where(Function(x) x.delBlock <> "").ToList()
        calculatedRouteCodeList = preparedList.Where(Function(x) x.route <> x.newRecommendedRouteCode AndAlso x.newRecommendedRouteCode <> "" AndAlso x.isRouteCodeChangeAllowed).ToList()

        'list to populate the log with
        calculatedList.AddRange(calculatedRDDList)
        calculatedList.AddRange(calculatedDelBlockList)
        calculatedList.AddRange(calculatedRouteCodeList)

        'creating a list objects with default execute implementation for all tasks
        finalList.AddRange(From q In calculatedRDDList Select New RDDModel(q))
        finalList.AddRange(From q In calculatedDelBlockList Select New DelBlockModel(q))
        finalList.AddRange(From q In calculatedRouteCodeList Select New RouteCodeModel(q))
    End Sub

    Public Sub runVA02(sap As ISAPLib)
        Dim va02 As New VA02(sap)

        For Each item In finalList
            item.execute(va02)
        Next
    End Sub

    Public Sub populateDeliveryDatesLog(dbserver As IDBServerConnector)
        dbserver.listToServer(calculatedList, "DeliveryDatesLog")
    End Sub
    Public Sub sendEmailWithChanges(salesOrg As String, email As String)
        mu.mailSimple(email, $"{salesOrg} RDD changes {DateTime.Now}", $"Hello<br><br><br>{mu.listToHTMLtable(calculatedList)}<br><br>Kind Regards<br>IDA")
    End Sub
    Public Sub emptyListAction(salesOrg As String, id As String, email As String, IsEmptySap As Boolean)
        idaLog.insertToActivityLog("DeliveryDateChanges", "empty list", id, salesOrg)
        If isLog Then log.finish("empty list")
        mu.mailSimple(email, $"{salesOrg} No RDD changes {DateTime.Now}", $"Hello<br>{If(IsEmptySap, "No orders in ZV04HN", "All RDD's are correct")} <br><br>Kind regards<br>IDA")
    End Sub

    Public Sub sendFailedRdds(salesOrg As String, email As String, dbServer As IDBServerConnector)
        Dim failedListQuery As String = $"Select * from DeliveryDatesLog where [id] = '{calculatedList.Item(0).id}' And (status is null or status <> 'success')"
        Dim rs As ADODB.Recordset = dbServer.executeQuery(failedListQuery)

        If Not rs.EOF Then mu.mailSimple(email, $"{salesOrg} Failed RDD items {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items manually<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA")
    End Sub

    Public Sub handleError(salesOrg As String, id As String, ex As Exception)
        Dim filepath As String = $"{IDAConsts.errorFilePath}\{salesOrg} RDD error {Format(DateTime.Now, "yyyy.MM.dd mm.hh")}.txt"
        Dim f = Create.fileUtil
        f.writeToTxtFile(filepath, ex.Source & vbCr & ex.Message & vbCr & ex.StackTrace)

        mu.mailSimple(IDAConsts.adminEmail, $"{salesOrg} RDD: Error {Err.Number}", $"{mu.getLink(filepath, "Your error info is here")}")

        If isLog Then log.finish("fail")
        idaLog.insertToActivityLog("DeliveryDateChanges", "fail", id, salesOrg)
    End Sub

    Public Sub finishLogs(salesOrg As String, id As String, status As String)
        If isLog Then log.finish(status)
        idaLog.insertToActivityLog("DeliveryDateChanges", status, id, salesOrg)
    End Sub

    Public Sub sendIT01Report(salesOrg As String, email As String, dbServer As IDBServerConnector)
        Dim changedOrdersToReleaseTodayList As String = $"Select * from DeliveryDatesLog where salesOrg = '{salesOrg}' [startTime] > GETDATE() AND status = 'success'"
        Dim rs As ADODB.Recordset = dbServer.executeQuery(changedOrdersToReleaseTodayList)

        mu.mailSimple(email, $"{salesOrg} Failed RDD items {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items manually<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA")
    End Sub
End Class