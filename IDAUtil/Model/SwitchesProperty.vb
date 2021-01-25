Imports System.ComponentModel.DataAnnotations.Schema

Public Class SwitchesProperty
    <Column("[salesOrg]")> Public Property salesOrg As String
    <Column("[country]")> Public Property country As String
    <Column("[soldTo]")> Public Property soldTo As Long
    <Column("[switchForCustomer]")> Public Property switchForCustomer As String
    <Column("[shipTo]")> Public Property shipTo As Long
    <Column("[orderNumber]")> Public Property order As Long
    <Column("[shipToName]")> Public Property shipToName As String
    <Column("[item]")> Public Property item As Long
    <Column("[orderedQty]")> Public Property orderedQty As Double
    <Column("[confirmedQty]")> Public Property confirmedQty As Double
    <Column("[oldSku]")> Public Property oldSku As Long
    <Column("[oldSkuCaseBarcode]")> Property oldSkuCaseBarcode As String
    <Column("[oldSkuUnitBarcode]")> Property oldSkuUnitBarcode As String
    <Column("[oldSkuATP]")> Public Property oldSkuATP As Long
    <Column("[oldSkuRecoveryDate]")> Public Property oldSkuRecoveryDate As String
    <Column("[oldSkuRecoveryQty]")> Public Property oldSkuRecoveryQty As Long
    <Column("[newSku]")> Public Property newSku As Long
    <Column("[newSkuCaseBarcode]")> Property newSkuCaseBarcode As String
    <Column("[newSkuUnitBarcode]")> Property newSkuUnitBarcode As String
    <Column("[newSkuATP]")> Public Property newSkuATP As Long
    <Column("[newSkuRecoveryDate]")> Public Property newSkuRecoveryDate As String
    <Column("[newSkuRecoveryQty]")> Public Property newSkuRecoveryQty As Long
    <Column("[startDate]")> Public Property startDate As Date
    <Column("[endDate]")> Public Property endDate As Date
    <Column("[needOutOfStockToSwitch]")> Public Property needOutOfStockToSwitch As Boolean
    <Column("[SwitchAutomatic]")> Public Property switchAutomatic As Boolean
    <Column("[switchComment]")> Public Property switchComment As String
    <Column("[id]")> Public Property id As String

    Public Sub New()
    End Sub

    Public Sub New(salesOrg As String,
                   country As String,
                   soldTo As Long,
                   switchForCustomer As String,
                   shipTo As Long,
                   order As Long,
                   shipToName As String,
                   item As Long,
                   orderedQty As Double,
                   confirmedQty As Double,
                   oldSku As Long,
                   oldSkuCaseBarcode As String,
                   oldSkuUnitBarcode As String,
                   oldSkuATP As String,
                   oldSkuRecoveryDate As Date,
                   oldSkuRecoveryQty As Long,
                   newSku As Long,
                   newSkuCaseBarcode As String,
                   newSkuUnitBarcode As String,
                   newSkuATP As String,
                   newSkuRecoveryDate As Date,
                   newSkuRecoveryQty As Long,
                   startDate As Date,
                   endDate As Date,
                   needOutOfStockToSwitch As Boolean,
                   switchAutomatic As Boolean,
                   switchComment As String,
                   id As String
                   )
        Me.salesOrg = salesOrg
        Me.country = country
        Me.soldTo = soldTo
        Me.switchForCustomer = switchForCustomer
        Me.shipTo = shipTo
        Me.order = order
        Me.shipToName = shipToName
        Me.item = item
        Me.orderedQty = orderedQty
        Me.confirmedQty = confirmedQty
        Me.oldSku = oldSku
        Me.oldSkuCaseBarcode = oldSkuCaseBarcode
        Me.oldSkuUnitBarcode = oldSkuUnitBarcode
        Me.oldSkuATP = oldSkuATP
        Me.oldSkuRecoveryDate = oldSkuRecoveryDate
        Me.oldSkuRecoveryQty = oldSkuRecoveryQty
        Me.newSku = newSku
        Me.newSkuCaseBarcode = newSkuCaseBarcode
        Me.newSkuUnitBarcode = newSkuUnitBarcode
        Me.newSkuATP = newSkuATP
        Me.newSkuRecoveryDate = newSkuRecoveryDate
        Me.newSkuRecoveryQty = newSkuRecoveryQty
        Me.startDate = startDate
        Me.endDate = endDate
        Me.needOutOfStockToSwitch = needOutOfStockToSwitch
        Me.switchAutomatic = switchAutomatic
        Me.switchComment = switchComment
        Me.id = id
    End Sub
End Class
