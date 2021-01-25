Imports System.ComponentModel.DataAnnotations.Schema

Public Class SwitchesDataProperty

    <Column("[salesOrg]")> Property salesOrg As String
    <Column("[country]")> Property country As String
    <Column("[soldTo]")> Property soldTo As Long
    <Column("[oldSku]")> Property oldSku As Long
    <Column("[oldSkuDescription]")> Property oldSkuDescription As String
    <Column("[oldSkuCaseBarcode]")> Property oldSkuCaseBarcode As String
    <Column("[oldSkuUnitBarcode]")> Property oldSkuUnitBarcode As String
    <Column("[startDate]")> Property startDate As String
    <Column("[endDate]")> Property endDate As String
    <Column("[needOutOfStockToSwitch]")> Property needOutOfStockToSwitch As Boolean
    <Column("[newSku]")> Property newSku As Long
    <Column("[newSkuDescription]")> Property newSkuDescription As String
    <Column("[newSkuCaseBarcode]")> Property newSkuCaseBarcode As String
    <Column("[newSkuUnitBarcode]")> Property newSkuUnitBarcode As String
    <Column("[switchAutomatic]")> Property switchAutomatic As Boolean
    <Column("[switchComment]")> Property switchComment As String

End Class
