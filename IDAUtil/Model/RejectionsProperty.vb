Imports System.ComponentModel.DataAnnotations.Schema

Public Class RejectionsProperty
    <Column("[salesOrg]")> Public Property salesOrg As String
    <Column("[country]")> Public Property country As String
    <Column("[soldTo]")> Public Property soldTo As Long
    <Column("[rejectionForCustomer]")> Public Property rejectionForCustomer As String
    <Column("[isReplacePartialCut]")> Public Property isReplacePartialCut As Boolean
    <Column("[shipTo]")> Public Property shipTo As Long
    <Column("[shipToName]")> Public Property shipToName As String
    <Column("[orderNumber]")> Public Property orderNumber As Long
    <Column("[item]")> Public Property item As Long
    <Column("[sku]")> Public Property sku As Long
    <Column("[rejectionReasonCode]")> Public Property rejectionReasonCode As Long
    <Column("[orderedQty]")> Public Property orderedQty As Double
    <Column("[confirmedQty]")> Public Property confirmedQty As Double
    <Column("[skuUnitBarcode]")> Property skuUnitBarcode As String
    <Column("[skuCaseBarcode]")> Property skuCaseBarcode As String
    <Column("skuATP]")> Public Property skuATP As Long
    <Column("[skuRecoveryDate]")> Public Property skuRecoveryDate As String
    <Column("[skuRecoveryQty]")> Public Property skuRecoveryQty As Long
    <Column("[startDate]")> Public Property startDate As Date
    <Column("[endDate]")> Public Property endDate As Date
    <Column("[needOutOfStockToReject]")> Public Property needOutOfStockToReject As Boolean
    <Column("[rejectionComment]")> Public Property rejectionComment As String
    <Column("[id]")> Public Property id As String

    Public Sub New()
    End Sub

    Public Sub New(salesOrg As String, country As String, soldTo As Long, rejectionForCustomer As String, isReplacePartialCut As Boolean, shipTo As Long, shipToName As String, orderNumber As Long, item As Long, sku As Long, rejectionReasonCode As Long, orderedQty As Double, confirmedQty As Double, skuUnitBarcode As String, skuCaseBarcode As String, skuATP As Long, skuRecoveryDate As String, skuRecoveryQty As Long, startDate As Date, endDate As Date, needOutOfStockToReject As Boolean, rejectionComment As String, id As String)
        Me.salesOrg = salesOrg
        Me.country = country
        Me.soldTo = soldTo
        Me.rejectionForCustomer = rejectionForCustomer
        Me.isReplacePartialCut = isReplacePartialCut
        Me.shipTo = shipTo
        Me.shipToName = shipToName
        Me.orderNumber = orderNumber
        Me.item = item
        Me.sku = sku
        Me.rejectionReasonCode = rejectionReasonCode
        Me.orderedQty = orderedQty
        Me.confirmedQty = confirmedQty
        Me.skuUnitBarcode = skuUnitBarcode
        Me.skuCaseBarcode = skuCaseBarcode
        Me.skuATP = skuATP
        Me.skuRecoveryDate = skuRecoveryDate
        Me.skuRecoveryQty = skuRecoveryQty
        Me.startDate = startDate
        Me.endDate = endDate
        Me.needOutOfStockToReject = needOutOfStockToReject
        Me.rejectionComment = rejectionComment
        Me.id = id
    End Sub
End Class
