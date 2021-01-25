Imports System.ComponentModel.DataAnnotations.Schema

Public Class DistributionListProperty
    <Column("[name]")>
    Public Property name As String
    <Column("[salesOrg]")>
    Public Property salesOrg As String
    <Column("[country]")>
    Public Property country As String
    <Column("[address]")>
    Public Property address As String
End Class
