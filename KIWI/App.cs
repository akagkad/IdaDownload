using IDAUtil;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.SAP.TCode;
using IDAUtil.SAP.TCode.Runners;
using IDAUtil.Service;
using KIWI.Model;
using lib;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace KIWI {
    public static class App {
        public static void Main() {
            var log = Create.serverLogger(135);
            var idaLog = new IdaLog();
            string salesOrg = "ZA01";
            IDBServerConnector dbServer = Create.dbServer();
            string id = $"{DateTime.Now} {Environment.MachineName}";
            idaLog.insertToActivityLog("KiwiConversions", "start", id, salesOrg);
            log.start();
            ISAPLib sapLib = Create.sapLib();
            DataCollectorSap dataCollector = new DataCollectorSap(sapLib, Create.exportParses());
            List<ZV04IProperty> zv04IList = dataCollector.getZV04IList(salesOrg, IDAEnum.Task.quantityConversion);
            KiwiConversionException kiwiExceptionObj = new KiwiConversionException();

            //removes lines that match exception criteria from zv04iList for furthur calculation
            zv04IList = zv04IList.Where(x => !(kiwiExceptionObj.material.Contains(x.material) && kiwiExceptionObj.soldTo.Contains(x.soldTo))).ToList();

            var orderList = zv04IList.Select(x => x.order);
            long maxOrder = orderList.Max();
            long minOrder = orderList.Min();
            var conversionDAO = new ConversionDAO(dbServer);
            var materialConversionList = conversionDAO.getQuantityConversionMaterialList();
            var shipToConversionList = conversionDAO.getQuantityConversionShipToList();
            var logConversionList = conversionDAO.getQuantityConversionLogList(minOrder, maxOrder);
            var dataCompareService = new DataCompareService();
            var salesDocumetList = dataCompareService.getSalesDocumentToChangeQuantity(zv04IList, shipToConversionList, materialConversionList);
            dataCompareService.removeAlreadyConvertedLines(logConversionList, ref salesDocumetList);
            var mailUtil = Create.mailUtil();
            var emailService = new EmailService(mailUtil);
            var distList = new DistributionListCalculator(dbServer);
            string emails = distList.getDistList(salesOrg, "quantityConversions");

            if (salesDocumetList.Count > 0) {
                try {
                    emailService.sendEmailNotification(emails, "", "ZA01 Kiwi conversion", salesDocumetList);
                } catch (Exception) {
                    mailUtil.mailSimple(IDAConsts.adminEmail, $"{salesOrg} Kiwi Conv: Error {Information.Err().Description}", $"Empty mail list for this task");
                }
                IVA02 va02 = new VA02(sapLib, idaLog);
                QuantityConversionVA02Runner va02Runner = new QuantityConversionVA02Runner(sapLib, idaLog, va02);

                try {
                    foreach (var document in salesDocumetList)
                        va02Runner.runQuantityChange(document);
                } catch (Exception ex) {
                    string filepath = $"{IDAConsts.Paths.errorFilePath}{salesOrg} Kiwi error: {DateTime.Now}";
                    var sTextFile = new System.Text.StringBuilder();
                    if (!System.IO.File.Exists(filepath)) {
                        System.IO.File.Create(filepath).Dispose();
                    }

                    sTextFile.AppendLine(ex.StackTrace);
                    System.IO.File.AppendAllText(filepath, sTextFile.ToString());
                    mailUtil.mailSimple(IDAConsts.adminEmail, $"{salesOrg} Kiwi Conv: Error {Information.Err().Number}", $"{mailUtil.getLink(filepath, "Your error info is here")}");
                    log.finish("fail");
                    idaLog.insertToActivityLog("KiwiConversions", "fail", id, salesOrg);
                    return;
                }

                log.finish("success");
                idaLog.insertToActivityLog("KiwiConversions", "success", id, salesOrg);
                logConversionList = conversionDAO.getQuantityConversionLogList(minOrder, maxOrder);
                dataCompareService.removeAlreadyConvertedLines(logConversionList, ref salesDocumetList);
                if (salesDocumetList.Count > 0) {
                    emailService.sendEmailNotification(emails, "", "ZA01 Kiwi conversion failed items", salesDocumetList);
                }
            } else {
                try {
                    mailUtil.mailSimple(emails, $"ZA01 Kiwi conversion {DateTime.Now}", "No Kiwi conversions found");
                } catch (Exception) {
                    mailUtil.mailSimple($"{IDAConsts.adminEmail};{Environment.UserName}", $"{salesOrg} Kiwi Conv: Error {Information.Err().Description}", $"Empty mail list for this task");
                }

                idaLog.insertToActivityLog("KiwiConversions", "empty list", id, salesOrg);
                log.finish("empty list");
            }
        }
    }
}