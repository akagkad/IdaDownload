Imports System.ComponentModel.DataAnnotations.Schema

Public Class ZV04PProperty
    <Column("[status]")>
    Public Property status As String
    <Column("[order]")>
    Public Property order As Long
    <Column("[item]")>
    Public Property item As String
    <Column("[delivery]")>
    Public Property delivery As String
    <Column("[shipment]")>
    Public Property shipment As String
    <Column("[docTyp]")>
    Public Property docTyp As String
    <Column("[docDate]")>
    Public Property docDate As String
    <Column("[delBlock]")>
    Public Property delBlock As String
    <Column("[reqDelDate]")>
    Public Property reqDelDate As String
    <Column("[delPriority]")>
    Public Property delPriority As String
    <Column("[pODate]")>
    Public Property pODate As String
    <Column("[loadingDate]")>
    Public Property loadingDate As String
    <Column("[material]")>
    Public Property material As Long
    <Column("[materialDescription]")>
    Public Property materialDescription As String
    <Column("[upc]")>
    Public Property upc As String
    <Column("[plant]")>
    Public Property plant As String
    <Column("[orderQty]")>
    Public Property orderQty As String
    <Column("[confirmedQty]")>
    Public Property confirmedQty As String
    <Column("[deliveryQty]")>
    Public Property deliveryQty As String
    <Column("[itemNetValue]")>
    Public Property itemNetValue As String
    <Column("[rejReason]")>
    Public Property rejReason As String
    <Column("[rejReasonDescription]")>
    Public Property rejReasonDescription As String
    <Column("[pONumber]")>
    Public Property pONumber As String
    <Column("[soldTo]")>
    Public Property soldTo As Long
    <Column("[soldToName]")>
    Public Property soldToName As String
    <Column("[salesDistrict]")>
    Public Property salesDistrict As String
    <Column("[shipTo]")>
    Public Property shipTo As Long
    <Column("[shipToName]")>
    Public Property shipToName As String
    <Column("[csrAsr]")>
    Public Property csrAsr As String
    <Column("[csrAsrName]")>
    Public Property csrAsrName As String
    <Column("[tlfpItem]")>
    Public Property tlfpItem As String
    <Column("[pltItem]")>
    Public Property pltItem As String
    <Column("[layersItem]")>
    Public Property layersItem As String
    <Column("[looseCsItem]")>
    Public Property looseCsItem As String
    <Column("[totCsItem]")>
    Public Property totCsItem As String
    <Column("[tlfpCmmtItem]")>
    Public Property tlfpCmmtItem As String
    <Column("[pltCmmtItem]")>
    Public Property pltCmmtItem As String
    <Column("[layersCmmtItem]")>
    Public Property layersCmmtItem As String
    <Column("[looseCsCmmtItem]")>
    Public Property looseCsCmmtItem As String
    <Column("[totCsCmmtItem]")>
    Public Property totCsCmmtItem As String
    <Column("[csLayer]")>
    Public Property csLayer As String
    <Column("[itemWeight]")>
    Public Property itemWeight As String
    <Column("[wtUnit]")>
    Public Property wtUnit As String
    <Column("[itemCube]")>
    Public Property itemCube As String
    <Column("[volUnit]")>
    Public Property volUnit As String
    <Column("[custExpPrice]")>
    Public Property custExpPrice As String
    <Column("[scjComparisonPrice]")>
    Public Property scjComparisonPrice As String
    <Column("[scheduleLineDeliveryDate]")>
    Public Property scheduleLineDeliveryDate As String
    <Column("[custMatNumb]")>
    Public Property custMatNumb As String
    <Column("[receivingPoint]")>
    Public Property receivingPoint As String

End Class
