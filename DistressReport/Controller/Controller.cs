using DistressReport.Service;
using IDAUtil.Service;
using lib;

namespace DistressReport {
    public static class Controller {
        public static void executeDistressReport(string salesOrg) {

            IDBServerConnector dbServer = Create.dbServer();
            ISAPLib sap = Create.sapLib();
            IDataCollectorServer dcServer = new DataCollectorServer(dbServer);
            IDataCollectorSap dcSap = new DataCollectorSap(sap, Create.exportParses());
            IDataCollectorServiceDistress dc = new DataCollectorServiceDistress(dcServer, dcSap);
            IDistributionListCalculator distList = new DistributionListCalculator(dbServer);

            string email = distList.getDistList(salesOrg, "distress");

            var executor = new DistressReportExecutor(salesOrg);

            executor.startLogs();

            if (!(executor.calculateGenericDistress(dc))) {
                executor.emptyListAction(email);
                return;
            }

            if (executor.calculateSalesOrgSpecifficDistress()) {
                executor.sendInfo(email);
                executor.finishLogs();
            } else {
                executor.emptyListAction(email);
            }
        }
    }
}
