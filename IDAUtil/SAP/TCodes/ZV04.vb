Imports [lib]

Public MustInherit Class ZV04

    Protected tCode As String
    Public Property soldToNumber As List(Of Integer)
    Public Property shipToNumber As List(Of Integer)
    Public Property CSRASR As Integer
    Public Property PONumber As String
    Public Property plant As List(Of String)
    Public Property docFromDate As Date
    Public Property docToDate As Date
    Public Property salesDoc As List(Of Integer)
    Public Property docType As String()
    Public Property sap As ISAPLib
    Public Property isOpenOrder As Boolean
    Public Property isCreditHold As Boolean
    Public Property isWithDelivery As Boolean
    Public Property isNotPGI As Boolean
    Public Property isNotInvoiced As Boolean
    Public Property salesOrg As String
    Public Property task As IDAEnum.Task
    'comment

    Public Sub New(sap As ISAPLib, salesOrg As String, task As IDAEnum.Task)
        Me.sap = sap
        Me.salesOrg = salesOrg
        Me.task = task
    End Sub

    Public Sub setParamsBeforeExecution()
        sap.enterTCode(tCode)
        setSalesOrg()
        setDocStatus()
        setDocType()
    End Sub

    Public Sub extractReport()
        sap.pressF8()
        sap.tableToClipboard()
    End Sub

    Public Sub exportExcel(path As String, fileName As String)
        sap.pressF8()
        sap.openExport()
        sap.exportExcel(path, fileName)
    End Sub

    Private Sub setDocStatus()

        Select Case task
            Case IDAEnum.Task.rdd, IDAEnum.Task.quantityConversion, IDAEnum.Task.switches, IDAEnum.Task.rejections
                isOpenOrder = True
                isCreditHold = True
            Case IDAEnum.Task.SOAR, IDAEnum.Task.customerMissingReport, IDAEnum.Task.missingCMIRReport
                isOpenOrder = True
                isCreditHold = True
                isNotInvoiced = True
                isNotPGI = True
                isWithDelivery = True
            Case Else
                Throw New NotImplementedException("Failed getting document statuses for ZV04")
        End Select

        sap.setCheckboxStatus(ZV04ID.OPEN_ORDER_CHECKBOX_ID, isOpenOrder)
        sap.setCheckboxStatus(ZV04ID.CREDIT_HOLD_CHECKBOX_ID, isCreditHold)
        sap.setCheckboxStatus(ZV04ID.WITH_DELIVERY_CHECKBOX_ID, isWithDelivery)
        sap.setCheckboxStatus(ZV04ID.NOT_PGI_CHECKBOX_ID, isNotPGI)
        sap.setCheckboxStatus(ZV04ID.NOT_INVOICED_CHECKBOX_ID, isNotInvoiced)
    End Sub

    Private Sub setSalesOrg()
        sap.setText(ZV04ID.SALES_ORG_TEXT_FIELD_ID, salesOrg)
    End Sub

    Private Sub setDocType()
        Dim docType As String()

        Select Case salesOrg
            Case "GB01"
                docType = {"ZOR", "ZEC"}
            Case Else
                docType = {"ZOR"}
        End Select

        sap.setMultipleSelection(docType, ZV04ID.DOCT_TYPE_MULTISELECTION_BTN_IDD)
    End Sub

End Class

Public Class ZV04HN
    Inherits ZV04

    Public Sub New(sap As ISAPLib, salesOrg As String, task As IDAEnum.Task)
        MyBase.New(sap, salesOrg, task)
        tCode = "ZV04HN"
    End Sub

End Class

Public Class ZV04I
    Inherits ZV04

    Public Property loadingDateFrom As Date
    Public Property loadingDateTo As Date
    Public Property materialNumber As List(Of Integer)
    Public Property saleDocItem As List(Of Integer)

    Public Sub New(sap As ISAPLib, salesOrg As String, task As IDAEnum.Task)
        MyBase.New(sap, salesOrg, task)
        tCode = "ZV04I"
    End Sub

End Class

Public Class ZV04P
    Inherits ZV04

    Public Sub New(sap As ISAPLib, salesOrg As String, task As IDAEnum.Task)
        MyBase.New(sap, salesOrg, task)
        tCode = "ZV04P"
    End Sub

End Class
