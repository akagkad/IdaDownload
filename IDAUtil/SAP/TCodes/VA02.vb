Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports [lib]
Imports SAPFEWSELib

Public Class VA02
    Private ReadOnly sap As ISAPLib
    Private ReadOnly log As New IdaLog

    Enum OrderStatus
        available = 0
        blockedByUser = 1
        blockedByBatchJob = 2
        realeasedOrRejected = 4
        bothSkusAreZ4 = 5
        success = 6
        failedToSave = 7
        bothSkusAreNotApproved = 8
    End Enum

    Public Sub New(saplib As ISAPLib)
        sap = saplib
    End Sub

    Public Sub runRDDChange(orderNumber As Integer, oldRdd As Date, newRdd As Date, id As String, changeReason As String, tableName As String)
        Dim sapNewRdd As String = $"{newRdd.Day}.{newRdd.Month}.{newRdd.Year}"
        Dim sapOldRdd As String = $"{oldRdd.Day}.{oldRdd.Month}.{oldRdd.Year}"
        Dim csr As String = ""
        Dim status As OrderStatus

        log.update(
                tableName,
                {"startTime"},
                {Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
                {"orderNumber", "id"},
                {orderNumber, id}
        )

        sap.enterTCode("VA02")
        sap.setText(VA02ID.ORDER_NUNBER_INPUT_TEXT_FIELD_ID, orderNumber)
        sap.pressEnter()

        status = checkIfOrderBlocked(orderNumber)

        If status = OrderStatus.blockedByUser OrElse status = OrderStatus.blockedByBatchJob Then
            updateLog(status, tableName, orderNumber, id)
            Return
        End If

        If sap.getInfoBarMsg().Contains("exists with same PO") Then sap.pressEnter()
        sap.getRidOfPopUps()
        sap.pressEnter()

        Dim blockID As String = VA02ID.DEL_BLOCK_ID.Where(Function(x) sap.idExists(x)).First
        status = isChangeNeeded(blockID)

        If status <> OrderStatus.available Then
            updateLog(status, tableName, orderNumber, id)
            Return
        End If

        sap.getRidOfPopUps()
        sap.pressEnter()
        sap.getRidOfPopUps()

        tryToSellectAllLines()
        sap.select(VA02ID.CHANGE_RDD_BTN_ID)
        sap.setText(VA02ID.CHANGE_RDD_TEXT_FIELD, sapNewRdd)
        sap.pressBtn(VA02ID.PRESS_OK_ON_RDD_CHANGE_FIELD_BTN_ID)

        sap.getRidOfPopUps()
        sap.pressEnter()
        sap.getRidOfPopUps()

        csr = $"RDD of {orderNumber} has been changed to {sapNewRdd} from {sapOldRdd}, Reason: {changeReason}"

        While sap.isPopUp OrElse sap.idExists(VA02ID.SECOND_POPUP_WINDOW_ID)
            sap.pressEnter()
        End While

        tryToOpenCSRNotes()
        setCSRNotes(csr)
        save()
        updateOrderSavedLog(tableName, orderNumber, id)
    End Sub

    Public Sub runDeliveryBlockChange(orderNumber As Integer, id As String, changeReason As String, delBlock As String, tableName As String)
        Dim csr As String
        Dim status As OrderStatus

        log.update(
                tableName,
                {"startTime"},
                {Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
                {"orderNumber", "id"},
                {orderNumber, id}
        )

        sap.enterTCode("VA02")
        sap.setText(VA02ID.ORDER_NUNBER_INPUT_TEXT_FIELD_ID, orderNumber)
        sap.pressEnter()

        status = checkIfOrderBlocked(orderNumber)

        If status = OrderStatus.blockedByUser OrElse status = OrderStatus.blockedByBatchJob Then
            updateLog(status, tableName, orderNumber, id)
            Return
        End If

        If sap.getInfoBarMsg().Contains("exists with same PO") Then sap.pressEnter()
        sap.getRidOfPopUps()
        sap.pressEnter()

        Dim blockID As String = VA02ID.DEL_BLOCK_ID.Where(Function(x) sap.idExists(x)).First
        status = isChangeNeeded(blockID)

        If status <> OrderStatus.available Then
            updateLog(status, tableName, orderNumber, id)
            Return
        End If

        sap.getRidOfPopUps()
        sap.pressEnter()
        sap.getRidOfPopUps()

        sap.findById(blockID).key = delBlock

        csr = $"Delivery block of {orderNumber} has been changed to {delBlock}, Reason: {changeReason}"

        tryToOpenCSRNotes()
        setCSRNotes(csr)
        save()
        updateOrderSavedLog(tableName, orderNumber, id)
    End Sub

    Public Sub runRouteCodeChange(orderNumber As Integer, id As String, reason As String, routeCode As String, tableName As String)
        Dim csr = ""
        Dim status As OrderStatus

        log.update(
                tableName,
                {"startTime"},
                {Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
                {"orderNumber", "id"},
                {orderNumber, id}
        )

        sap.enterTCode("VA02")
        sap.setText(VA02ID.ORDER_NUNBER_INPUT_TEXT_FIELD_ID, orderNumber)
        sap.pressEnter()

        status = checkIfOrderBlocked(orderNumber)

        If status = OrderStatus.blockedByUser OrElse status = OrderStatus.blockedByBatchJob Then
            updateLog(status, tableName, orderNumber, id)
            Return
        End If

        If sap.getInfoBarMsg().Contains("exists with same PO") Then sap.pressEnter()
        sap.getRidOfPopUps()
        sap.pressEnter()

        Dim blockID As String = VA02ID.DEL_BLOCK_ID.Where(Function(x) sap.idExists(x)).First
        status = isChangeNeeded(blockID)

        If status <> OrderStatus.available Then
            updateLog(status, tableName, orderNumber, id)
            Return
        End If

        sap.getRidOfPopUps()
        sap.pressEnter()
        sap.getRidOfPopUps()

        csr = $"Route Code of {orderNumber} has been changed to {routeCode}, Reason: {reason}"

        sap.findById(VA02ID.SHIPPING_TAB_ID).select

        Dim table As ITable
        'sometimes table is not found when sap is slow
        Try
            table = sap.getITableObject()
        Catch ex As Exception
            Threading.Thread.Sleep(1000)
            table = sap.getITableObject()
        End Try

        table.forEachRow("Route",
                         Sub(x)
                             Try
                                 x.setCellValue("Route", $"{routeCode}")
                             Catch ex As ReadOnlyException
                             Catch exNotScrolled As COMException
                                 sap.getRidOfPopUps()
                                 x.setCellValue("Route", $"{routeCode}")
                             End Try
                         End Sub)

        sap.pressEnter()
        sap.getRidOfPopUps()

        tryToOpenCSRNotes()
        setCSRNotes(csr)
        save()
        updateOrderSavedLog(tableName, orderNumber, id)
    End Sub

    Public Function runQuantityChange(order As QtyConversionOrderProperty) As Boolean
        sap.enterTCode("VA02")
        sap.setText(SAP_ALL_ID.VA02.ORDER_FLD, order.orderNumber)
        sap.pressEnter()

        checkIfOrderBlocked(order.orderNumber)
        sap.pressEnter()
        sap.getRidOfPopUps()
        If sap.getInfoBarMsg().Contains("exists with same PO") Then sap.pressEnter()
        sap.getRidOfPopUps()

        Dim table = sap.getITableObject()
        If changeQuantityInTable(table, order) Then
            Dim csr As String = $"Order number {order.orderNumber} had qty's changed in the following - {getChangedListForCSR(order)}"

            tryToOpenCSRNotes()
            setCSRNotes(csr)
            save()
            If sap.getInfoBarMsg.Contains($"{order.orderNumber} has been saved") Then
                order.isSaved = True
                Dim itemArr As Integer() = order.documentLineChangeList.Select(Function(x) x.item).ToArray()
                log.update("QuantityConversionLog", {"isSaved"}, {1}, {"orderNumber"}, {order.orderNumber}, $"item IN ({String.Join(",", itemArr)})")
                Return True
            End If
        End If

        Return False
    End Function

    Public Function runSwitches(switchObj As SwitchesSapOrderProperty, id As String, tableName As String) As OrderStatus
        Dim csr As String = ""
        Dim status As OrderStatus

        log.update(
            tableName,
            {"startTime"},
            {Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
            {"[orderNumber]", "[id]", "[SwitchAutomatic]"},
            {switchObj.order, id, True}
        )

        sap.enterTCode("VA02")
        sap.setText(VA02ID.ORDER_NUNBER_INPUT_TEXT_FIELD_ID, switchObj.order)
        sap.pressEnter()

        status = checkIfOrderBlocked(switchObj.order)

        If status = OrderStatus.blockedByUser OrElse status = OrderStatus.blockedByBatchJob Then
            updateLog(status, tableName, switchObj.order, id)
            Return status
        End If

        If sap.getInfoBarMsg().Contains("exists with same PO") Then sap.pressEnter()
        sap.getRidOfPopUps()
        sap.pressEnter()

        Dim blockID As String = VA02ID.DEL_BLOCK_ID.Where(Function(x) sap.idExists(x)).First
        status = isChangeNeeded(blockID)

        If status <> OrderStatus.available Then
            Return status
        End If

        Dim table As ITable
        'sometimes table is not found when sap is slow
        Try
            table = sap.getITableObject()
        Catch ex As Exception
            Threading.Thread.Sleep(1000)
            table = sap.getITableObject()
        End Try

        moveRouteCodeToAvailableColumn(table) 'have to do that due to buggy GUITableControl

        For Each lineSwitch In switchObj.lineDetails
            Dim shouldChange As Boolean = True
            Dim sapLineNumber As Integer = Integer.Parse(lineSwitch.lineNumber / 10 - 1)
            Dim isChanged As Boolean = False

            If Not table.getCell(sapLineNumber, "Material").changeable Then
                lineSwitch.reason = "Line is rejected"
                shouldChange = False
            End If

            If table.getCellValue(sapLineNumber, "Material") = lineSwitch.newSku Then
                lineSwitch.reason = "Line was already switched"
                shouldChange = False
            End If

            If shouldChange Then
                Dim unitOfMeasure As String = table.getCellValue(sapLineNumber, "Un")
                Dim itemCategory As String = table.getCellValue(sapLineNumber, "ItCa")
                Dim routeCode As String = table.getCellValue(sapLineNumber, 8) 'have to use by index because the table retains its indexes by name after reordering but not actual indexes within a table
                Dim cmir As String = table.getCellValue(sapLineNumber, "Customer Material Numb")

                table.setCellValue(sapLineNumber, "Material", lineSwitch.newSku)

                sap.pressEnter()
                sap.getRidOfPopUps()

                isChanged = isLineChanged(table, lineSwitch, sapLineNumber)

                If sap.getInfoBarMsg().Contains("Z4") Then
                    Return OrderStatus.bothSkusAreZ4
                End If


                If sap.getInfoBarMsg().Contains("not approved") Then
                    Return OrderStatus.bothSkusAreNotApproved
                End If

                If unitOfMeasure <> table.getCellValue(sapLineNumber, "Un") Then
                    table.setCellValue(sapLineNumber, "Un", unitOfMeasure)
                    sap.pressEnter()
                    sap.getRidOfPopUps()
                End If

                If itemCategory <> table.getCellValue(sapLineNumber, "ItCa") Then
                    table.setCellValue(sapLineNumber, "ItCa", itemCategory)
                    sap.pressEnter()
                    sap.getRidOfPopUps()
                End If

                table.initColumnDict()
                If routeCode <> table.getCellValue(sapLineNumber, "Route") Then
                    table.setCellValue(sapLineNumber, 8, routeCode)
                    sap.pressEnter()
                    sap.getRidOfPopUps()
                End If

                If lineSwitch.isSameBarcode AndAlso cmir <> "" Then
                    table.setCellValue(sapLineNumber, "Customer Material Numb", cmir)

                    log.insert(
                        "CMIR",
                        "[salesOrg],[soldTo],[sku],[cmir]",
                        "'" & switchObj.salesOrg & "'" & "," & switchObj.soldTo & "," & lineSwitch.newSku & "," & "'" & cmir & "'")
                End If
            End If

            csr += $"sku {lineSwitch.oldSku} has{If(isChanged, " ", " not ")}been switched to {lineSwitch.newSku}. Reason: {lineSwitch.reason}{vbCr}"

            log.update(
                    tableName,
                    {"status", "reason"},
                    {If(isChanged, "success", "fail"), lineSwitch.reason},
                    {"[orderNumber]", "[id]", "[SwitchAutomatic]"},
                    {switchObj.order, id, True}
                )
        Next

        tryToOpenCSRNotes()
        setCSRNotes(csr)
        save()

        Return getOrderStatusAfterSaving()

    End Function
    Public Function runRejections(rejObj As RejectionsSapOrderProperty, id As String, tableName As String) As OrderStatus
        Dim csr As String = ""
        Dim status As OrderStatus

        log.update(
            tableName:=tableName,
            columnNames:={"startTime"},
            values:={Format(DateTime.Now, "yyyyMMdd HH:mm:ss")},
            conditionName:={"[orderNumber]", "[id]"},
            conditionValue:={rejObj.orderNumber, id}
        )

        sap.enterTCode("VA02")
        sap.setText(VA02ID.ORDER_NUNBER_INPUT_TEXT_FIELD_ID, rejObj.orderNumber)
        sap.pressEnter()

        status = checkIfOrderBlocked(rejObj.orderNumber)

        If status = OrderStatus.blockedByUser OrElse status = OrderStatus.blockedByBatchJob Then
            updateLog(status, tableName, rejObj.orderNumber, id)
            Return status
        End If

        If sap.getInfoBarMsg().Contains("exists with same PO") Then sap.pressEnter()
        sap.getRidOfPopUps()
        sap.pressEnter()

        If status <> OrderStatus.available Then
            Return status
        End If

        sap.findById(VA02ID.REJECTIONS_TAB_ID).select

        Dim table As ITable
        'sometimes table is not found when sap is slow
        Try
            table = sap.getITableObject()
        Catch ex As Exception
            Threading.Thread.Sleep(1000)
            table = sap.getITableObject()
        End Try

        For Each rejection In rejObj.lineDetails
            Dim shouldChange As Boolean = True
            Dim sapLineNumber As Integer = Integer.Parse(rejection.lineNumber / 10 - 1)

            If Not table.getCell(sapLineNumber, "Reason for rejection").changeable Then
                rejection.reason += "RRC is not changeable."
                shouldChange = False
            End If

            If table.getCellValue(sapLineNumber, "Material") <> rejection.sku Then
                rejection.reason += "RRC cannot be applied as the sku was changed."
                shouldChange = False
            End If

            If shouldChange Then
                If rejection.isReplacePartialCut Then
                    rejection.reason += $". Line was split into rejected {rejection.orderedQty - rejection.confirmedQty} cases and {rejection.confirmedQty} to be sent for customer."

                    'move to the order tab
                    'find the last line + 1 and insert sku + {rejection.orderedQty - rejection.confirmedQty
                    'put the sku number and qty
                    'go back to rejection and reject
                Else
                    table.setCellValue(sapLineNumber, "Reason for rejection", rejection.rejectionCode)
                End If

                sap.pressEnter()
                sap.getRidOfPopUps()
            End If

            csr += $"sku {rejection.sku} has{If(shouldChange, " ", " not ")}been rejected with {rejection.rejectionCode}. Reason: {rejection.reason}{vbCr}"

            log.update(
                    tableName,
                    {"status", "reason"},
                    {If(shouldChange, "success", "fail"), rejection.reason},
                    {"[orderNumber]", "[id]"},
                    {rejObj.orderNumber, id}
                )
        Next

        tryToOpenCSRNotes()
        setCSRNotes(csr)
        save()

        Return getOrderStatusAfterSaving()

    End Function


    Private Function getOrderStatusAfterSaving() As OrderStatus
        If sap.getInfoBarMsg().Contains("saved") Then
            Return OrderStatus.success
        Else
            For i = 0 To 5
                Threading.Thread.Sleep(2000)
                save()
                If sap.getInfoBarMsg().Contains("saved") Then Return OrderStatus.success
            Next

            Return OrderStatus.failedToSave
        End If
    End Function

    Private Sub moveRouteCodeToAvailableColumn(table As ITable)
        sap.findById(table.id).reorderTable("0 1 2 3 4 5 6 7 66 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 67 68 69 70 71 72 73 74 75 76 77 78 79 80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95 96 97 98 99 100 101 102")
    End Sub

    Private Function isLineChanged(table As ITable, ByRef lineSwitch As SwitchesSapLineProperty, sapLineNumber As Integer) As Boolean
        Select Case True
            Case sap.getInfoBarMsg().Contains("Future")
                lineSwitch.reason = "SKU is future status"
                putBackPreviousSku(table, lineSwitch, sapLineNumber)
                Return False
            Case sap.getInfoBarMsg().Contains("excluded")
                lineSwitch.reason = "SKU is excluded"
                putBackPreviousSku(table, lineSwitch, sapLineNumber)
                Return False
            Case sap.getInfoBarMsg().Contains("not defined")
                lineSwitch.reason = "SKU is not defined for the sales organisation"
                putBackPreviousSku(table, lineSwitch, sapLineNumber)
                Return False
            Case sap.getInfoBarMsg().Contains("not approved")
                lineSwitch.reason = "SKU is not approved for the sales organisation"
                sap.pressEnter()
                sap.getRidOfPopUps()
                putBackPreviousSku(table, lineSwitch, sapLineNumber)
                Return False
            Case Else
                Return True
        End Select
    End Function

    Private Sub putBackPreviousSku(table As ITable, lineSwitch As SwitchesSapLineProperty, sapLineNumber As Integer)
        table.setCellValue(sapLineNumber, table.columnDict("Material").First, lineSwitch.oldSku)
        sap.pressEnter()
        sap.getRidOfPopUps()
    End Sub

    Private Function getChangedListForCSR(order As QtyConversionOrderProperty) As String
        Dim text As String = ""

        For i = 0 To order.documentLineChangeList.Count - 1
            text &= $"sku: {order.documentLineChangeList(i).material}, old qty:{order.documentLineList(i).quantity}, new qty:{order.documentLineChangeList(i).quantity};" & vbCr
        Next

        Return text
    End Function

    Private Sub save()
        sap.pressSave()
        sap.getRidOfPopUps()
        If sap.idExists(SAP_ALL_ID.VA02.INCOMPLETE_DOCUMENT_MSGBOX) Then sap.pressBtn(SAP_ALL_ID.VA02.INCOMPLETE_DOCUMENT_OK_BTN)
        sap.getRidOfPopUps()
    End Sub

    Private Sub updateOrderSavedLog(tableName As String, orderNumber As Integer, id As String)
        If sap.getInfoBarMsg.Contains("saved") Then
            log.update(
                tableName,
                {"endTime", "status"},
                {Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), "success"},
                {"orderNumber", "id"},
                {orderNumber, id}
            )
        Else
            log.update(
                tableName,
                {"endTime", "status"},
                {Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), "Fail: could not save order"},
                {"orderNumber", "id"},
                {orderNumber, id}
            )
        End If
    End Sub

    Public Function changeQuantityInTable(table As ITable, order As QtyConversionOrderProperty) As Boolean
        For i = 0 To order.documentLineChangeList.Count - 1
            Dim item As Integer = order.documentLineChangeList(i).item
            Dim row As Integer = item / 10 - 1
            Dim newQuantity As Integer = order.documentLineChangeList(i).quantity
            Dim material = order.documentLineChangeList(i).material
            Try
                order.start(i)
                log.insert("QuantityConversionLog", "orderNumber, shipTo, item, material, oldQty, newQty, startTime", $"{order.orderNumber}, {order.shipTo},{order.documentLineChangeList(i).item},{order.documentLineChangeList(i).material},{order.documentLineList(i).quantity},{order.documentLineChangeList(i).quantity},'{order.documentLineChangeList(i).startTime}'")

                If table.getCellValue(row, "Material") = material Then
                    If table.setCellValue(row, "Order Quantity", newQuantity) Then
                        order.documentLineChangeList(i).actionSuccess()
                        sap.pressEnter()
                        sap.getRidOfPopUps()
                    Else
                        order.documentLineChangeList(i).actionFailed($"Failed to set value")
                    End If
                Else
                    order.documentLineChangeList(i).actionFailed($"Expected: {material} Actual: {table.getCellValue(row, "Material")}")
                End If
            Catch ex As Exception
                order.documentLineChangeList(i).actionFailed($"Error occured: {ex.Message}")
                Debug.WriteLine(ex.Source & vbCr & ex.StackTrace)
                Return False
            Finally
                order.finish(i)
                log.update("QuantityConversionLog", {"endTime", "status", "isConverted"}, {order.documentLineChangeList(i).endTime, order.documentLineChangeList(i).status, order.documentLineChangeList(i).isChanged}, {"orderNumber", "item"}, {order.orderNumber, item})
            End Try
        Next
        Return True
    End Function

    Private Sub setCSRNotes(csr As String)
        'when csr notes are greyed out need to double click them, save them and come out to populate them
        If Not sap.findById(VA02ID.CSR_FIELD_ID).Changeable Then
            sap.findById(VA02ID.CSR_FIELD_ID).doubleClick
            sap.pressSave()
            sap.goBack()
        End If

        Dim oldText As String = sap.getText(VA02ID.CSR_FIELD_ID)
        sap.setText(VA02ID.CSR_FIELD_ID, "IDA change! " & csr & vbCr & oldText)
    End Sub

    Private Sub updateLog(status As OrderStatus, tableName As String, orderNumber As String, id As String)
        Dim outcome As String = ""

        Select Case status
            Case OrderStatus.blockedByBatchJob
                outcome = "Fail: blocked by batch job"
            Case OrderStatus.blockedByUser
                outcome = "Fail: blocked by user"
            Case OrderStatus.realeasedOrRejected
                outcome = "Fail: order released or rejected"
            Case Else
                Throw New NotImplementedException
        End Select

        log.update(
                tableName,
                {"endTime", "status"},
                {Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), outcome},
                {"orderNumber", "id"},
                {orderNumber, id}
            )
    End Sub

    Private Function isChangeNeeded(blockId As String) As OrderStatus
        Select Case True
            Case Not sap.findById(blockId).Changeable
                Return OrderStatus.realeasedOrRejected
            Case Else
                Return OrderStatus.available
        End Select
    End Function

    Private Sub tryToSellectAllLines()

        Dim timeout As DateTime = DateAdd(DateInterval.Second, 10, Now)

        Dim myTable As GuiTableControl
        Dim firstRow As GuiTableRow

        While DateTime.Now < timeout
            'buggfix com objects late binding
            Threading.Thread.Sleep(2000)

            'buggfix - The data necessary to complete this operation is not yet available.
            Try
                myTable = sap.findById(VA02ID.FIRST_ROW_OF_SALES_TAB_TABLE_ID.Where(Function(x) sap.idExists(x)).First)
                firstRow = myTable.Rows(1)
                If Not firstRow.Selected Then
                    sap.pressBtn(VA02ID.SELECT_ALL_BTN_ID.Where(Function(x) sap.idExists(x)).First)
                Else
                    Exit While
                End If
            Catch ex As Exception
            End Try
        End While

    End Sub

    Private Sub tryToOpenCSRNotes()
        Dim timeout As DateTime = DateAdd(DateInterval.Second, 10, Now)

        'buggfix - The data necessary to complete this operation is not yet available.
        While DateTime.Now < timeout
            Threading.Thread.Sleep(2000)
            Try
                If Not sap.idExists(VA02ID.CSR_FIELD_ID) Then
                    sap.select(VA02ID.PRESS_OPEN_NOTES_BTN_ID)
                Else
                    Exit While
                End If
            Catch ex As Exception
            End Try
        End While
    End Sub

    Private Function checkIfOrderBlocked(orderNumber As Integer) As OrderStatus
        Dim isOrderBlocked As Boolean = sap.getInfoBarMsg().Contains("is currently being processed")

        If isOrderBlocked Then
            Dim mu As MailUtil = Create.mailUtil
            Dim guid As String = Replace(Right(sap.getInfoBarMsg(), 8), ")", "")
            Dim validGuid As Boolean = Regex.Match(guid, "[A-Z]\d{6}").Success

            If validGuid Then
                mu.mailSimple(guid, $"Please leave {orderNumber}", $"Automated IDA reply.{vbCr}You have 60 seconds to leave the order {orderNumber}")

                Dim timeout As DateTime = DateAdd(DateInterval.Second, 60, Now)
                While Now < timeout
                    If sap.getInfoBarMsg().Contains("is currently being processed") Then
                        sap.pressEnter()
                        Threading.Thread.Sleep(10000) 'that's 10 seconds Daniel...
                    Else
                        Return OrderStatus.available
                    End If
                End While
            Else
                Return OrderStatus.blockedByBatchJob
            End If
            Return OrderStatus.blockedByUser
        Else
            Return OrderStatus.available
        End If
    End Function
End Class
