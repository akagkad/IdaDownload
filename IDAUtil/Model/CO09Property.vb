Imports System.ComponentModel.DataAnnotations.Schema
<DebuggerDisplay("sku: {sku}, salesOrg: {salesOrg}, ATP: {ATP}")>
Public Class CO09Property
    <Column("[sku]")> Public Property sku As Long
    <Column("[salesOrg]")> Public Property salesOrg As String
    <Column("[plant]")> Public Property plant As Long
    <Column("[ATP]")> Public Property ATP As Long
    <Column("[recoveryDate]")> Public Property recoveryDate As String
    <Column("[recoveryQty]")> Public Property recoveryQty As String
End Class
