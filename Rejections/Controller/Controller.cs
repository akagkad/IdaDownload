using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Service;
using lib;
using Rejections.Service;
using System;

namespace Rejections {
    static class Controller {
        public static void executeRejections(string salesOrg, bool isRelease) {
            #region properties
            string id = $"{DateTime.Now} {Environment.MachineName}";
            
            IIdaLog idaLog = new IdaLog();
            IDBServerConnector dbServer = Create.dbServer();
            ISAPLib sap = Create.sapLib();
            IMailUtil mu = Create.mailUtil();
            IDataCollectorServer dcServer = new DataCollectorServer(dbServer);
            IDataCollectorSap dcSap = new DataCollectorSap(sap, Create.exportParses());
            IDataCollectorServiceRejections dataCollector = new DataCollectorServiceRejections(dcServer, dcSap);
            IDataCalculatorServiceRejections dataCalculator = new DataCalculatorServiceRejections(id);
            IDistributionListCalculator dlc = new DistributionListCalculator(dbServer);

            RejectionsOrderPropertyFactory rejectionOrderObj = new RejectionsOrderPropertyFactory();

            string email = dlc.getDistList(salesOrg, "rejections");
            var executor = new RejectionsTaskExecutor(salesOrg, id, isRelease, idaLog);
            #endregion

            executor.startLogs(salesOrg, isRelease ? "Release Rejections" : "After Release Rejections", "start");

            if (!executor.calculateLists(dataCollector, dataCalculator)) {
                string tempText = isRelease ? " " : " after ";
                mu.mailSimple(email, $"{salesOrg} No Rejections {DateTime.Now}", $"Hello<br><br>There are no{tempText}release rejections<br><br>Kind Regards<br>IDA");
                executor.endLogs(salesOrg, isRelease ? "Release Rejections" : "After Release Rejections", "empty list");
                return;
            }

            if (isRelease) {
                #region releaseExecution
                executor.populateRejectionLog(dbServer);

                if (executor.afterReleaseRejectionList.Count > 0) {
                    mu.mailSimple(email, $"{salesOrg} After release rejections {DateTime.Now}", $"Hello<br><br>Here are the rejections that will be executed after last release today:<br><br>{mu.listToHTMLtable(executor.afterReleaseRejectionList)}<br><br>Kind Regards<br>IDA");
                } else {
                    mu.mailSimple(email, $"{salesOrg} No after release rejections {DateTime.Now}", $"Hello<br><br>There are no after release rejections found in this release<br><br>Kind Regards<br>IDA");
                    executor.endLogs(salesOrg, "After Release Rejections", "empty list");
                }

                if (executor.releaseRejectionList.Count > 0) {
                    executor.createAutomaticRejectionObjectList(rejectionOrderObj);
                    mu.mailSimple(email, $"{salesOrg} Release rejections {DateTime.Now}", $"Hello<br><br>Here are the rejections during release:<br><br>{mu.listToHTMLtable(executor.releaseRejectionList)}<br><br>Kind Regards<br>IDA");
                    executor.runRejectionsInVA02(email, dbServer, mu);
                    executor.endLogs(salesOrg, "Release Rejections", "success");
                } else {
                    mu.mailSimple(email, $"{salesOrg} No release rejections {DateTime.Now}", $"Hello<br><br>There are no rejections found for this release<br><br>Kind Regards<br>IDA");
                    executor.endLogs(salesOrg, "Release Rejections", "empty list");
                }
                #endregion
            } else {
                #region afterReleaseExecution
                executor.createAutomaticRejectionObjectList(rejectionOrderObj);
                mu.mailSimple(email, $"{salesOrg} After release rejections {DateTime.Now}", $"Hello<br><br>Here are the after release rejections gathered throughout the day that are about to be executed:<br><br>{mu.listToHTMLtable(executor.afterReleaseRejectionList)}<br><br>Kind Regards<br>IDA");
                executor.runRejectionsInVA02(email,dbServer,mu);
                executor.endLogs(salesOrg, "After Release Rejections", "success");
                #endregion
            }
        }
    }
}