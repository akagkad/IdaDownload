Imports System.ComponentModel.DataAnnotations.Schema

Public Class ConversionLog

    <Column("[orderNumber]")>
    Public Property orderNumber As Integer
    <Column("[shipTo]")>
    Public Property shipTo As Integer
    <Column("[item]")>
    Public Property item As Integer
    <Column("[material]")>
    Public Property material As Integer
    <Column("[oldQty]")>
    Public Property oldQty As Integer
    <Column("[newQty]")>
    Public Property newQty As Integer
    <Column("[isConverted]")>
    Public Property isConverted As Boolean
    <Column("[startTime]")>
    Public Property startTime As Date
    <Column("[endTime]")>
    Public Property endTime As Date
    <Column("[status]")>
    Public Property status As String
    <Column("[isSaved]")>
    Public Property isSaved As Boolean

End Class
