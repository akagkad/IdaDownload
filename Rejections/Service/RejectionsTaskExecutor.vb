Imports IDAUtil
Imports [lib]

Public Class RejectionsTaskExecutor
    Private isLog As Boolean = True
    Private log As IServerLogger = Create.serverLogger(147)
    Private idaLog As New IdaLog
    Private salesOrg As String
    Private id As String

    Private rejectionsList As List(Of RejectionsProperty)
    Public releaseRejectionList As List(Of RejectionsProperty)
    Public afterReleaseRejectionList As List(Of RejectionsProperty)

    Public Sub New(salesOrg As String, id As String)
        Me.salesOrg = salesOrg
        Me.id = id
    End Sub

    Public Function calculateLists(dcServer As DataCollectorServer, dcSap As DataCollectorSap) As Boolean
        Dim dc As New DataCollectorServiceRejections(dcServer, dcSap)
        rejectionsList = dc.getRejectionsList(salesOrg, id)

        If rejectionsList Is Nothing Then
            Return False
        Else
            releaseRejectionList = rejectionsList.Where(Function(x) x.confirmedQty > 0)
            AfterReleaseRejectionList = rejectionsList.Where(Function(x) x.confirmedQty = 0)
            Return True
        End If
    End Function

    Public Sub populateRejectionLog(dbServer As IDBServerConnector)
        dbServer.listToServer(rejectionsList, "RejectionsLog")
    End Sub

    Public Sub endLogs(salesOrg As String, rejectionType As String, status As String)
        If isLog Then
            log.finish(status)
            idaLog.insertToActivityLog(rejectionType, status, id, salesOrg)
        End If
    End Sub

    Public Sub runVA02Rejections(sap As ISAPLib)
        Dim va02 As New VA02(sap)
    End Sub
End Class
