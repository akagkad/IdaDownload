Imports IDAUtil

Public Class DataCollectorServiceCMIR
    Private ReadOnly dataCollectorServer As DataCollectorServer
    Private ReadOnly dataCollectorSap As DataCollectorSap

    Public Sub New(dataCollectorServer As DataCollectorServer, dataCollectorSap As DataCollectorSap)
        Me.dataCollectorServer = dataCollectorServer
        Me.dataCollectorSap = dataCollectorSap
    End Sub

    Public Function getMissingCMIRS(salesOrg As String) As List(Of MissingCMIRProperty)
        Dim PList As List(Of ZV04PProperty) = dataCollectorSap.getZV04PList(salesOrg)
        Dim customerDataList As List(Of CustomerDataProperty) = dataCollectorServer.getCustomerDataList(salesOrg)

        If PList Is Nothing Then
            Return Nothing
        End If

        Dim query As List(Of MissingCMIRProperty) =
        (From p In PList
         Join cm In customerDataList
                On p.soldTo Equals cm.soldTo And p.shipTo Equals cm.shipTo
         Where (p.custMatNumb Is Nothing OrElse p.custMatNumb = "") AndAlso cm.cmirCheckAllowed AndAlso p.rejReason = ""
         Select New MissingCMIRProperty(p.material, p.materialDescription, p.soldTo, p.soldToName, p.order, p.item)).ToList

        Return query
    End Function
End Class
