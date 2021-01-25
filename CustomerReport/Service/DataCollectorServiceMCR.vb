Imports IDAUtil
Imports [lib]

Public Class DataCollectorServiceMCR
    Private ReadOnly dataCollectorServer As DataCollectorServer
    Private ReadOnly dataCollectorSap As DataCollectorSap

    Public Sub New(dataCollectorServer As DataCollectorServer, dataCollectorSap As DataCollectorSap)
        Me.dataCollectorServer = dataCollectorServer
        Me.dataCollectorSap = dataCollectorSap
    End Sub

    Public Function getMissingCustomers(salesOrg As String) As List(Of MissingCustomersProperty)
        Dim ZVList As List(Of ZV04HNProperty) = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.rdd)
        Dim customerDataList As List(Of CustomerDataProperty) = dataCollectorServer.getCustomerDataList(salesOrg)

        If ZVList Is Nothing Then
            Return Nothing
        End If

        Dim query As List(Of MissingCustomersProperty) =
            (From cml In (ZVList.Where(Function(x) customerDataList.All(Function(y) y.shipTo <> x.shipto OrElse x.soldto <> y.soldTo)).ToList)
             Select New MissingCustomersProperty(cml.order, cml.soldto, cml.soldtoName, cml.shipto, cml.shiptoName)).Distinct(Function(x, y) x.soldto = y.soldto AndAlso x.shipto = y.shipto).ToList
        Return query
    End Function
End Class