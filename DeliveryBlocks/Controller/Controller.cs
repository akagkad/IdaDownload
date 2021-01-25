using DeliveryBlocks.Service;
using IDAUtil.Service;
using lib;
using System;
using System.Linq;

namespace DeliveryBlocks {
    public static class Controller {
        public static void executeDeliveryBlocks(string salesOrg) {
            string id = $"{DateTime.Now} {Environment.MachineName}";
            IDBServerConnector dbServer = Create.dbServer();
            ISAPLib sap = Create.sapLib();
            IDataCollectorServer dcServer = new DataCollectorServer(dbServer);
            IDataCollectorSap dcSap = new DataCollectorSap(sap, Create.exportParses());
            IDataCollectorServiceDeliveryBlocks dc = new DataCollectorServiceDeliveryBlocks(id: id, dataCollectorServer: dcServer, dataCollectorSap: dcSap, salesOrg: salesOrg);
            IDistributionListCalculator distList = new DistributionListCalculator(dbServer);
            IMailUtil mu = Create.mailUtil();

            string email = distList.getDistList(salesOrg, "deliveryBlocks");
            var executor = new DeliveryBlocksExecutor(salesOrg: salesOrg, id: id);
            string[] salesOrgsWithNoDelBlockAction = { "ES01", "PT01" };

            executor.startLog();

            if (executor.calculateDeliveryBlockList(dc)) {
                executor.populateDeliveryBlocksLog(dbServer);
                executor.sendEmail(email: email, mu: mu, isEmptyList: false);
                executor.runDeliveryBlocksInVA02();
                executor.finishLog(isEmpty: false);
            } else {
                if (!salesOrgsWithNoDelBlockAction.Contains(salesOrg)) {
                    executor.sendEmail(email: email, mu: mu, isEmptyList: true);
                    executor.finishLog(isEmpty: true);
                }
            }

            executor.sendFailedDeliveryBlocks(salesOrg, email, dbServer, mu);

            executor.rejectLastReleaseBlockedOrders(dbServer, dcSap, dcServer, mu, email);

            executor.sendCurrentBlocksInSystem(mu, dcSap, email);

            if (salesOrgsWithNoDelBlockAction.Contains(salesOrg)) { executor.finishLog(isEmpty: false); }
        }
    }
}