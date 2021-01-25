Imports System.ComponentModel.DataAnnotations.Schema

Public Class BankHolidayProperty
    <Column("[salesOrg]")> Public Property salesOrg As String
    <Column("[country]")> Public Property country As String
    <Column("[nationalDate]")> Public Property nationalDate As Date
End Class
