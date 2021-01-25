Imports System.ComponentModel.DataAnnotations.Schema

Public Class MissingCustomersProperty
    <Column("[order]")> Public Property order As Long
    <Column("[soldto]")> Public Property soldto As Long
    <Column("[soldtoName]")> Public Property soldtoName As String
    <Column("[shipto]")> Public Property shipto As Long
    <Column("[shiptoName]")> Public Property shiptoName As String

    Public Sub New(order As Long, soldto As Long, soldtoName As String, shipto As Long, shiptoName As String)
        Me.order = order
        Me.soldto = soldto
        Me.soldtoName = soldtoName
        Me.shipto = shipto
        Me.shiptoName = shiptoName
    End Sub

    Public Overrides Function Equals(obj As Object) As Boolean
        Dim isEqual = (shipto = obj.shipto AndAlso shiptoName = obj.shiptoName AndAlso soldto = obj.soldto AndAlso soldtoName = obj.soldtoName)

        Return isEqual
    End Function
End Class
