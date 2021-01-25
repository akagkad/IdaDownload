Imports System.ComponentModel.DataAnnotations.Schema

Public Class MissingCMIRProperty
    <Column("[CMIR]")>
    Public Property CMIR As String
    <Column("[material]")>
    Public Property material As Long
    <Column("[order]")>
    Public Property order As Long
    <Column("[item]")>
    Public Property item As String
    <Column("[materialDescription]")>
    Public Property materialDescription As String
    <Column("[soldTo]")>
    Public Property soldTo As Long
    <Column("[soldToName]")>
    Public Property soldToName As String

    Public Sub New(material As Long, materialDescription As String, soldTo As Long, soldToName As String, order As Long, item As String)
        Me.material = material
        Me.materialDescription = materialDescription
        Me.soldTo = soldTo
        Me.soldToName = soldToName
        Me.order = order
        Me.item = item
        CMIR = ""
    End Sub
End Class
