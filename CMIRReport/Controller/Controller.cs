using IDAUtil;
using IDAUtil.Service;
using lib;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;

namespace CMIRReport {
    static class Controller {
        public static void executeMissingCMIRReport(string salesOrg) {
            var idaLog = new IdaLog();
            string id = $"{DateTime.Now} {Environment.MachineName}";

           // idaLog.insertToActivityLog("missingCMIR", "startTime", id, salesOrg);

            var dbServer = Create.dbServer();
            var sap = Create.sapLib();

            DataCollectorServer dcServer = new DataCollectorServer(dbServer);
            DataCollectorSap dcSap = new DataCollectorSap(sap, Create.exportParses());
            DistributionListCalculator distList = new DistributionListCalculator(dbServer);
            DataCollectorServiceCMIR dcService = new DataCollectorServiceCMIR(dcServer, dcSap);
            List<MissingCMIRProperty> missingCMIRList = dcService.getMissingCMIRS(salesOrg);

            IMailUtil mu = Create.mailUtil();
            string email = distList.getDistList(salesOrg, "cmirCheck");

            if (missingCMIRList is null) {
                idaLog.insertToActivityLog("missingCMIR", "empty list", id, salesOrg);
                mu.mailSimple(email, $"{salesOrg} CMIR's missing in orders {DateTime.Now}", $"Hello<br><br><br>There are no orders in the ZV04P report<br><br>Kind Regards<br>IDA");
                return;
            }

            if (missingCMIRList.Count > 0) {
                sendEmailWithExcelFile(salesOrg, missingCMIRList, mu, email);
                idaLog.insertToActivityLog("missingCMIR", "success", id, salesOrg);
            } else {
                idaLog.insertToActivityLog("missingCMIR", "empty list", id, salesOrg);
                mu.mailSimple(email, $"{salesOrg} CMIR's missing in orders {DateTime.Now}", $"Hello<br><br><br>There are no missing CMIR's<br><br>Kind Regards<br>IDA");
            }
        }

        private static void sendEmailWithExcelFile(string salesOrg, List<MissingCMIRProperty> missingCMIRList, IMailUtil mu, string email) {
            IDBConnector dbxl = Create.dbXl();
            Workbook wb;

            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{salesOrg} {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}hour {DateTime.Now.Minute}minute Missing CMIR Report.xlsx";

            dbxl.listToExcel(list: missingCMIRList, path: path);

            using (IExcelUtil xl = Create.xlUtil()) {
                try {
                    wb = xl.getWorkbook(path: path);
                } catch (Exception) {
                    System.Threading.Thread.Sleep(5000);
                    wb = xl.getWorkbook(path: path);
                }

                Worksheet ws = wb.Worksheets[1] as Worksheet;

                try {
                    IdaExcelService.prettifyExcel(worksheet: ws, excelUtilObj: xl);
                } catch (Exception) {
                    System.Threading.Thread.Sleep(2000);
                    IdaExcelService.prettifyExcel(worksheet: ws, excelUtilObj: xl);
                }

                wb.Save();

                mu.mailOneAttachment(emailTo: email, cc: "", subject: $"{salesOrg} Missing CMIR Report", body: $"{mu.listToHTMLtable(missingCMIRList)}", path: path);
                dbxl.closeConnection();
                xl.closeWBAndAllInstances(wbName: wb.Name);
            }
        }
    }
}