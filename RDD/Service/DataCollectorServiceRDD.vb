Imports IDAUtil

Public Class DataCollectorServiceRDD
    Private ReadOnly dataCollectorServer As DataCollectorServer
    Private ReadOnly dataCollectorSap As DataCollectorSap

    Public Sub New(dataCollectorServer As DataCollectorServer, dataCollectorSap As DataCollectorSap)
        Me.dataCollectorServer = dataCollectorServer
        Me.dataCollectorSap = dataCollectorSap
    End Sub

    Public Function GetRddList(salesOrg As String, id As String) As List(Of RddOutputBean)

        Dim ZVList As List(Of ZV04HNProperty)
        ZVList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.rdd)

        If ZVList Is Nothing Then
            Return Nothing
        Else
            ZVList = ZVList.Where(Function(x) getZVCondition(x, salesOrg)).ToList
        End If

        Dim customerDataList As List(Of CustomerDataProperty) = dataCollectorServer.getCustomerDataList(salesOrg)
        Dim query As List(Of RddOutputBean)

        query = (
                From zv In ZVList
                Join cm In customerDataList On zv.shipto Equals cm.shipTo And zv.soldto Equals cm.soldTo
                Select New RddOutputBean(zv, cm, id)
                ).ToList()

        Return query
    End Function

    Public Function getBHList(salesOrg As String) As List(Of BankHolidayProperty)
        Return dataCollectorServer.getBHList(salesOrg)
    End Function

    Private Function getZVCondition(zv As ZV04HNProperty, salesOrg As String) As Boolean
        Dim flag As Boolean = zv.delBlock <> IDAConsts.bypassOrderChangesBlock

        Select Case salesOrg
            Case "ZA01", "NG01", "KE02"
                flag = flag AndAlso zv.delBlock <> "Z4" AndAlso zv.delBlock <> "04"
            Case "ES01", "PT01"
                flag = flag AndAlso zv.delBlock <> "ZG"
            Case Else
        End Select

        Return flag
    End Function
End Class
