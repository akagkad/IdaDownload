Imports System.ComponentModel.DataAnnotations.Schema

Public Class ConversionMaterial
    <Column("[material]")>
    Public Property material As Integer
    <Column("[conversionIndex]")>
    Public Property conversionIndex As Integer
End Class
