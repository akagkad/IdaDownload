Imports System.ComponentModel.DataAnnotations.Schema

Public Class ZV04HNProperty
    <Column("[status]")>
    Public Property status As Long
    <Column("[order]")>
    Public Property order As Integer
    <Column("[delivery]")>
    Public Property delivery As Long
    <Column("[shipment]")>
    Public Property shipment As String
    <Column("[docTyp]")>
    Public Property docTyp As String
    <Column("[docDate]")>
    Public Property docDate As Date
    <Column("[delBlock]")>
    Public Property delBlock As String
    <Column("[delBlockDesc]")>
    Public Property delBlockDesc As String
    <Column("[ordNetValue]")>
    Public Property ordNetValue As Double
    <Column("[reqDelDate]")>
    Public Property reqDelDate As Date
    <Column("[pricingDate]")>
    Public Property pricingDate As String
    <Column("[pODate]")>
    Public Property pODate As String
    <Column("[loadingDate]")>
    Public Property loadingDate As Date
    <Column("[plant]")>
    Public Property plant As String
    <Column("[orderQty]")>
    Public Property orderQty As Double
    <Column("[confirmedQty]")>
    Public Property confirmedQty As Double
    <Column("[deliveryQty]")>
    Public Property deliveryQty As Double
    <Column("[deliveryNetValue]")>
    Public Property deliveryNetValue As Double
    <Column("[FillRate]")>
    Public Property FillRate As Double
    <Column("[pONumber]")>
    Public Property pONumber As String
    <Column("[soldto]")>
    Public Property soldto As Long
    <Column("[soldtoName]")>
    Public Property soldtoName As String
    <Column("[salesOffice]")>
    Public Property salesOffice As String
    <Column("[salesGroup]")>
    Public Property salesGroup As String
    <Column("[salesDistrict]")>
    Public Property salesDistrict As String
    <Column("[backorderIndicator]")>
    Public Property backorderIndicator As String
    <Column("[shipto]")>
    Public Property shipto As long
    <Column("[shiptoName]")>
    Public Property shiptoName As String
    <Column("[cSRASR]")>
    Public Property cSRASR As String
    <Column("[csrasrName]")>
    Public Property csrasrName As String
    <Column("[scjAgent]")>
    Public Property scjAgent As String
    <Column("[scjAgentName]")>
    Public Property scjAgentName As String
    <Column("[scjAgentReg]")>
    Public Property scjAgentReg As String
    <Column("[scjAgentRegName]")>
    Public Property scjAgentRegName As String
    <Column("[orderTlfp]")>
    Public Property orderTlfp As Double
    <Column("[orderPlt]")>
    Public Property orderPlt As Double
    <Column("[orderLayers]")>
    Public Property orderLayers As Double
    <Column("[orderLooseCs]")>
    Public Property orderLooseCs As Double
    <Column("[orderTotCs]")>
    Public Property orderTotCs As Double
    <Column("[tlfpCmmt]")>
    Public Property tlfpCmmt As Double
    <Column("[pltCmmt]")>
    Public Property pltCmmt As Double
    <Column("[layersCmmt]")>
    Public Property layersCmmt As Double
    <Column("[looseCsCmmt]")>
    Public Property looseCsCmmt As Double
    <Column("[totCsCmmt]")>
    Public Property totCsCmmt As Double
    <Column("[headerWeight]")>
    Public Property headerWeight As Double
    <Column("[wtUnit]")>
    Public Property wtUnit As String
    <Column("[headerCube]")>
    Public Property headerCube As Double
    <Column("[volUnit]")>
    Public Property volUnit As String
    <Column("[ediSummaryData]")>
    Public Property ediSummaryData As String
    <Column("[zv04Report]")>
    Public Property zv04Report As String
    <Column("[ordDesc]")>
    Public Property ordDesc As String
    <Column("[description]")>
    Public Property description As String
    <Column("[createdBy]")>
    Public Property createdBy As String
    <Column("[route]")>
    Public Property route As String
    <Column("[shipToPostalCode]")>
    Public Property shipToPostalCode As String
    <Column("[shipToRegion]")>
    Public Property shipToRegion As String
    <Column("[shipToRegionName]")>
    Public Property shipToRegionName As String
    <Column("[shipToCity]")>
    Public Property shipToCity As String
    <Column("[shipToStreet]")>
    Public Property shipToStreet As String
    <Column("[soldToPostalCode]")>
    Public Property soldToPostalCode As String
    <Column("[soldToRegion]")>
    Public Property soldToRegion As String
    <Column("[soldToRegionName]")>
    Public Property soldToRegionName As String
    <Column("[soldToCity]")>
    Public Property soldToCity As String
    <Column("[soldToStreet]")>
    Public Property soldToStreet As String
    <Column("[realOpenOrderQty]")>
    Public Property realOpenOrderQty As String
    <Column("[realOpenOrderNetValue]")>
    Public Property realOpenOrderNetValue As Double
    <Column("[appointmentTimes]")>
    Public Property appointmentTimes As String
    <Column("[csrNotes]")>
    Public Property csrNotes As String
    <Column("[ediTransmittedText]")>
    Public Property ediTransmittedText As String
    <Column("[deliveryInstructions]")>
    Public Property deliveryInstructions As String
    <Column("[deliveryNote2]")>
    Public Property deliveryNote2 As String
    <Column("[carrierInstructions]")>
    Public Property carrierInstructions As String
End Class
