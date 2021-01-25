Imports System.ComponentModel.DataAnnotations.Schema
Imports IDAUtil

Public Class CMROutputBean

    <Column("[soldTo]")>
    Public Property soldTo As Long
    <Column("[soldToName]")>
    Public Property soldToName As String
    <Column("[shipTo]")>
    Public Property shipTo As Long
    <Column("[shipToName]")>
    Public Property shipToName As String
    <Column("[salesOrg]")>
    Public Property salesOrg As String
    <Column("[id]")>
    Public Property id As String

    Public Sub New(zv As ZV04HNProperty, id As String)
        soldTo = zv.soldto
        soldToName = zv.soldtoName
        shipTo = zv.shipto
        shipToName = zv.shiptoName
        Me.id = id
    End Sub
End Class
