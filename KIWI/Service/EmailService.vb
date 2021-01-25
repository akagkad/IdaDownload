Imports IDAUtil
Imports [lib]

Public Class EmailService

    Public mailUtil As IMailUtil

    Public Sub New(mailUtil As IMailUtil)
        Me.mailUtil = mailUtil
    End Sub

    Public Function sendEmailNotification(emailTo As String, cc As String, subject As String, salesDocumetList As List(Of QtyConversionOrderProperty))
        Dim body As String

        body = $"Hello,{vbCr}{getKIWIHTMLTable(salesDocumetList)}{vbCr}Kind regards,{vbCr}IDA"

        Return mailUtil.mailArg(emailTo, cc, subject, body)
    End Function

    Public Function getKIWIHTMLTable(salesDocumetList As List(Of QtyConversionOrderProperty)) As String
        Dim table As String
        table = "<table style = 'border: 1px solid black; border-collapse: collapse; width: 100%;'><tr style = 'border: 1px solid black;'>"

        table &= getHTMLColumnNames({"document", "item", "soldTo", "shipTo", "shipTo name", "SKU", "oldQty", "newQty", "isChanged", "status"})
        table &= "</tr>"

        For Each salesDocument In salesDocumetList
            For i = 0 To salesDocument.documentLineChangeList.Count - 1
                table &= "<tr style = 'border: 1px solid black; width:1%; white-space:nowrap;'>"
                table &= getHTMLCell(salesDocument.orderNumber)
                table &= getHTMLCell(salesDocument.documentLineChangeList(i).item)
                table &= getHTMLCell(salesDocument.soldTo)
                table &= getHTMLCell(salesDocument.shipTo)
                table &= getHTMLCell(salesDocument.shipToName)
                table &= getHTMLCell(salesDocument.documentLineChangeList(i).material)
                table &= getHTMLCell(salesDocument.documentLineList(i).quantity)
                table &= getHTMLCell(salesDocument.documentLineChangeList(i).quantity)
                table &= getHTMLCell(If(salesDocument.documentLineChangeList(i).isChanged, salesDocument.documentLineChangeList(i).isChanged, ""))
                table &= getHTMLCell(salesDocument.documentLineChangeList(i).status)
                table &= "</tr>"
            Next
        Next

        table &= "</table>"
        Return table
    End Function

    Private Function getHTMLColumnNames(columnNameArr As String()) As String
        Dim text As String = ""
        For Each name In columnNameArr
            text &= "<th style = 'border: 1px solid black;'>" & name & "</th>"
        Next
        Return text
    End Function

    Private Function getHTMLCell(text As String) As String
        Return "<td style = 'border: 1px solid black;' class='block'>" & text & "</td>"
    End Function

End Class
