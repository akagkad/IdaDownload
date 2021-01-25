Imports IDAUtil
Imports [lib]

Public Module Main

    Sub Main()
        Dim log As IServerLogger = Create.serverLogger(135)
        Dim idaLog As New IdaLog
        Dim salesOrg As String = "ZA01"
        Dim dbServer As IDBServerConnector = Create.dbServer
        Dim id As String = $"{DateTime.Now} {Environment.MachineName}"

        idaLog.insertToActivityLog("DeliveryDatesLog", "start", id, salesOrg)
        log.start()
        Dim sapLib As ISAPLib = Create.sapLib
        Dim dataCollector As New DataCollectorSap(sapLib, Create.exportParses)
        Dim zv04IList As List(Of ZV04IProperty) = dataCollector.getZV04IList(salesOrg, IDAEnum.Task.quantityConversion)

        Dim orderList = zv04IList.Select(Function(x) x.order)
        Dim maxOrder As Long = orderList.Max
        Dim minOrder As Long = orderList.Min

        Dim conversionDAO As New ConversionDAO(dbServer)
        Dim materialConversionList As List(Of ConversionMaterial) = conversionDAO.getQuantityConversionMaterialList
        Dim shipToConversionList As List(Of ConversionShiptTo) = conversionDAO.getQuantityConversionShipToList
        Dim logConversionList As List(Of ConversionLog) = conversionDAO.getQuantityConversionLogList(minOrder, maxOrder)

        Dim dataCompareService As New DataCompareService
        Dim salesDocumetList As List(Of QtyConversionOrderProperty) = dataCompareService.getSalesDocumentToChangeQuantity(zv04IList, shipToConversionList, materialConversionList)
        dataCompareService.removeAlreadyConvertedLines(logConversionList, salesDocumetList)

        Dim mailUtil As IMailUtil = Create.mailUtil
        Dim emailService As New EmailService(mailUtil)
        Dim distList As New DistributionListCalculator(dbServer)
        Dim emails As String = distList.getDistList(salesOrg, "quantityConversions")

        If salesDocumetList.Count > 0 Then
            Try
                emailService.sendEmailNotification(emails, "", "ZA01 Kiwi conversion", salesDocumetList)
            Catch emptyEmailListEx As Exception
                mailUtil.mailSimple(IDAConsts.adminEmail, $"{salesOrg} Kiwi Conv: Error {Err.Description}", $"Empty mail list for this task")
            End Try

            Dim va02 As New VA02(sapLib)

            Try
                For Each document In salesDocumetList
                    va02.runQuantityChange(document)
                Next
            Catch ex As Exception
                Dim filepath As String = $"{IDAConsts.errorFilePath}{salesOrg} Kiwi error: {DateTime.Now}"
                Dim sTextFile As New System.Text.StringBuilder

                If Not System.IO.File.Exists(filepath) Then
                    System.IO.File.Create(filepath).Dispose()
                End If

                sTextFile.AppendLine(ex.StackTrace)
                System.IO.File.AppendAllText(filepath, sTextFile.ToString)

                mailUtil.mailSimple(IDAConsts.adminEmail, $"{salesOrg} Kiwi Conv: Error {Err.Number}", $"{mailUtil.getLink(filepath, "Your error info is here")}")
                log.finish("fail")
                idaLog.insertToActivityLog("DeliveryDatesLog", "fail", id, salesOrg)
                Return
            End Try

            log.finish("success")
            idaLog.insertToActivityLog("DeliveryDatesLog", "success", id, salesOrg)

            logConversionList = conversionDAO.getQuantityConversionLogList(minOrder, maxOrder)
            dataCompareService.removeAlreadyConvertedLines(logConversionList, salesDocumetList)
            If salesDocumetList.Count > 0 Then
                emailService.sendEmailNotification(emails, "", "ZA01 Kiwi conversion failed items", salesDocumetList)
            End If

        Else
            Try
                mailUtil.mailSimple(emails, $"ZA01 Kiwi conversion {DateTime.Now}", "No Kiwi conversions found")
            Catch emptyEmailListEx As Exception
                mailUtil.mailSimple($"{adminEmail};{Environment.UserName}", $"{salesOrg} Kiwi Conv: Error {Err.Description}", $"Empty mail list for this task")
            End Try

            idaLog.insertToActivityLog("DeliveryDatesLog", "empty list", id, salesOrg)
            log.finish("empty list")
        End If
    End Sub

End Module