Imports System.ComponentModel.DataAnnotations.Schema

Public Class RejectionsDataProperty
    <Column("[salesOrg]")> Public Property salesOrg As String
    <Column("[country]")> Public Property country As String
    <Column("[soldTo]")> Public Property soldTo As Long
    <Column("[sku]")> Public Property sku As Long
    <Column("[skuDescription]")> Public Property skuDescription As String
    <Column("[skuUnitBarcode]")> Public Property skuUnitBarcode As String
    <Column("[skuCaseBarcode]")> Public Property skuCaseBarcode As String
    <Column("[rejectionReasonCode]")> Public Property rejectionReasonCode As String
    <Column("[needOutOfStockToReject]")> Public Property needOutOfStockToReject As Boolean
    <Column("[startDate]")> Public Property startDate As String
    <Column("[endDate]")> Public Property endDate As String
    <Column("[rejectionComment]")> Public Property rejectionComment As String
End Class
