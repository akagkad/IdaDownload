using IDAUtil;
using IDAUtil.Service;
using lib;
using System;

namespace CustomerReport {
    static class Controller {
        public static void executeCustomerMissingReport(string salesOrg) {
            var idaLog = new IdaLog();
            string id = $"{DateTime.Now} {Environment.MachineName}";
            IDBServerConnector dbServer = Create.dbServer();
            ISAPLib sap = Create.sapLib();
            idaLog.insertToActivityLog("missingCustomers", "startTime", id, salesOrg);
            var dcServer = new DataCollectorServer(dbServer);
            var dcSap = new DataCollectorSap(sap, Create.exportParses());
            var distList = new DistributionListCalculator(dbServer);
            var dc = new DataCollectorServiceMCR(dcServer, dcSap);
            var dcService = new DataCollectorServiceMCR(dcServer, dcSap);
            var customerMissingList = dcService.getMissingCustomers(salesOrg);
            IMailUtil mu = Create.mailUtil();
            string email = distList.getDistList(salesOrg, "missingCustomers");

            if (customerMissingList is null) {
                idaLog.insertToActivityLog("missingCustomers", "empty list", id, salesOrg);
                mu.mailSimple(email, $"{salesOrg} Customers missing in the Database {DateTime.Now}", $"Hello<br><br>There are no orders in ZV04HN<br><br>Kind Regards<br>IDA");
                return;
            }
            
            if (customerMissingList.Count > 0) {
                mu.mailSimple(email, $"{salesOrg} Customers missing in the Database {DateTime.Now}", $"Hello<br><br><br>{mu.listToHTMLtable(customerMissingList)}<br><br>Kind Regards<br>IDA");
                idaLog.insertToActivityLog("missingCustomers", "success", id, salesOrg);
            } else {
                idaLog.insertToActivityLog("missingCustomers", "empty list", id, salesOrg);
                mu.mailSimple(email, $"{salesOrg} Customers missing in the Database {DateTime.Now}", $"Hello<br><br><br>There are no missing customers in the database<br><br>Kind Regards<br>IDA");
            }
        }
    }
}