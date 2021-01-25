using IDAUtil;
using IDAUtil.Service;
using IDAUtil.Support;
using lib;
using System;

namespace Switches {
    static class Controller {
        public static void executeSwitchTask(string salesOrg) {
            #region properties
            string id = $"{DateTime.Now} {Environment.MachineName}";
            var dbServer = Create.dbServer();
            var sap = Create.sapLib();
            //var mu = Create.mailUtil();
            var dcServer = new DataCollectorServer(dbServer);
            var dcSap = new DataCollectorSap(sap, Create.exportParses());
            var distList = new DistributionListCalculator(dbServer);
            var dc = new DataCollectorServiceSwitches(dcServer, dcSap);
            var executor = new SwitchesTaskExecutor(salesOrg, id);
            string email = distList.getDistList(salesOrg, "switches");
            #endregion

            //executor.startLogs(salesOrg);

            if (!executor.calculateSwitches(dcServer, dcSap)) {
              //  mu.mailSimple(email, $"{salesOrg} No manual or automatic switches {DateTime.Now}", $"Hello<br><br>There are no orders in ZV04I<br><br>Kind Regards<br>IDA");
                //executor.endLogs(salesOrg, "Manual Switches", "empty list");
                //executor.endLogs(salesOrg, "Automatic Switches", "empty list");
                return;
            }

            if (executor.switchList.Count > 0) {
                executor.populateSwitchesLog(dbServer);
            }
            if (executor.ManualSwitchesList.Count > 0) {
               // mu.mailSimple(email, $"{salesOrg} Manual Switches {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(executor.ManualSwitchesList)}<br><br>Kind Regards<br>IDA");
                executor.endLogs(salesOrg, "Manual Switches", "success");
            } else {
                //mu.mailSimple(email, $"{salesOrg} No Manual Switches {DateTime.Now}", $"Hello<br><br>There are no manual switches for you to action<br><br>Kind Regards<br>IDA");
                executor.endLogs(salesOrg, "Manual Switches", "empty list");
            }

            if (executor.AutomaticSwitchesList.Count > 0) {
                //mu.mailSimple(email, $"{salesOrg} Automatic Switches {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(executor.AutomaticSwitchesList)}<br><br>Kind Regards<br>IDA");
                executor.createAutomaticSwitchObjectList();

                try {
                    executor.runSwitchesInVA02();
                    executor.endLogs(salesOrg, "Automatic Switches", "success");
                   // executor.sendFailedSwitches(salesOrg, email, dbServer, mu);
                    //executor.sendReplacedCMIRs(salesOrg, email, mu, dbServer);
                } catch (Exception ex) {
                    GlobalErrorHandler.handle(salesOrg, "Automatic Switches", ex);
                   // executor.sendFailedSwitches(salesOrg, email + ";DLGBFrmsOTDCENTRALISEDTEAM@scj.com", dbServer, mu);
                }
            } else {
                //mu.mailSimple(email, $"{salesOrg} No Automatic Switches {DateTime.Now}", $"Hello<br><br>There are no automatic switches for IDA to action<br><br>Kind Regards<br>IDA");
                executor.endLogs(salesOrg, "Automatic Switches", "empty list");
            }
        }
    }
}