Imports System.ComponentModel.DataAnnotations.Schema

Public Class CustomerDataProperty
    <Column("[salesOrg]")>
    Public Property salesOrg As String
    <Column("[distributionChannel]")>
    Public Property distributionChannel As Long
    <Column("[country]")>
    Public Property country As String
    <Column("[soldTo]")>
    Public Property soldTo As Long
    <Column("[soldToName]")>
    Public Property soldToName As String
    <Column("[shipTo]")>
    Public Property shipTo As Long
    <Column("[shipToName]")>
    Public Property shipToName As String
    <Column("[cmirCheckAllowed]")>
    Public Property cmirCheckAllowed As Boolean
    <Column("[replaceObsoletePartialCutsAllowed]")>
    Public Property replaceObsoletePartialCutsAllowed As Boolean
    <Column("[sendOrderConfirmationAllowed]")>
    Public Property sendOrderConfirmationAllowed As Boolean
    <Column("[sendDeliveryConfirmationAllowed]")>
    Public Property sendDeliveryConfirmationAllowed As Boolean
    <Column("[changeRouteCodeActionAllowed]")>
    Public Property changeRouteCodeActionAllowed As Boolean
    <Column("[changeRDDActionAllowed]")>
    Public Property changeRDDActionAllowed As Boolean
    <Column("[oneDayLeadTimeAllowed]")>
    Public Property oneDayLeadTimeAllowed As Boolean
    <Column("[leadTime]")>
    Public Property leadTime As Integer
    <Column("[deliveryDay]")>
    Public Property deliveryDay As String
    <Column("[sendDailyCutsAllowed]")>
    Public Property sendDailyCutsAllowed As Boolean
    <Column("[accountManager]")>
    Public Property accountManager As String
    <Column("[salesDistrict]")>
    Public Property salesDistrict As Long
    <Column("[csrNumber]")>
    Public Property csrNumber As Long
    <Column("[csrName]")>
    Public Property csrName As String
    <Column("[minimumOrderCaseQuantity]")>
    Public Property minimumOrderCaseQuantity As Long
    <Column("[minimumOrderPalletQuantity]")>
    Public Property minimumOrderPalletQuantity As Long
    <Column("[minimumOrderValue]")>
    Public Property minimumOrderValue As Long
    <Column("[truckOverflowPalletQuantity]")>
    Public Property truckOverflowPalletQuantity As Long
    <Column("[truckOverflowValue]")>
    Public Property truckOverflowValue As Long
    <Column("[truckOverflowWeight]")>
    Public Property truckOverflowWeight As Long
    <Column("[markedForDelitionZA01Only]")>
    Public Property markedForDelitionZA01Only As Boolean

End Class
