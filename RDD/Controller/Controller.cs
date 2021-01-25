using IDAUtil.Service;
using lib;
using System;

namespace RDD {
    public static class Controller {
        public static void executeRDDTask(string salesOrg) {
            string id = $"{DateTime.Now} {Environment.MachineName}";
            var dbServer = Create.dbServer();
            var sap = Create.sapLib();
            var dcServer = new DataCollectorServer(dbServer);
            var dcSap = new DataCollectorSap(sap, Create.exportParses());
            var distList = new DistributionListCalculator(dbServer);
            var dc = new DataCollectorServiceRDD(dcServer, dcSap);
            string email = distList.getDistList(salesOrg, "deliveryDateChanges");
            var executor = new RddTaskExecutor(salesOrg);

            try {
                //executor.startLogs(salesOrg, id);
                executor.setRddList(salesOrg, id, dc);

                // check when sap is empty
                if (executor.rddList is null) {
                    executor.emptyListAction(salesOrg, id, email, true);
                    return;
                }

                executor.setBHList(salesOrg, dc);
                executor.prepareList();
                executor.calculateLists();

                if (executor.calculatedList.Count > 0) {
                    executor.populateDeliveryDatesLog(dbServer);
                    executor.sendEmailWithChanges(salesOrg, email);
                    // TODO: move to multisession
                    executor.runVA02(sap);
                    executor.sendFailedRdds(salesOrg, email, dbServer);
                } else {
                    executor.emptyListAction(salesOrg, id, email, false);
                }

                //If salesOrg = "IT01" Then executor.sendIT01Report(salesOrg, email, dbServer)
            } catch (Exception ex) {
                executor.sendFailedRdds(salesOrg, email, dbServer);
                executor.handleError(salesOrg, id, ex);
                dbServer.closeConnection();
                return;
            }

            executor.finishLogs(salesOrg, id, "success");
            dbServer.closeConnection();
        }
    }
}