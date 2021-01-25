using IDAUtil;
using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.SAP.TCode;
using IDAUtil.SAP.TCode.Runners;
using IDAUtil.Service;
using IDAUtil.Support;
using lib;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Switches {
    public class SwitchesTaskExecutor {
        private IdaLog idaLog = new IdaLog();
        private readonly string salesOrg;
        private readonly string id;
        public List<SwitchesProperty> switchList = new List<SwitchesProperty>();
        public List<SwitchesProperty> AutomaticSwitchesList = new List<SwitchesProperty>();
        public List<SwitchesProperty> ManualSwitchesList = new List<SwitchesProperty>();
        public List<SwitchesSapOrderProperty> automaticSwitchObjectList = new List<SwitchesSapOrderProperty>();

        public SwitchesTaskExecutor(string salesOrg, string id) {
            this.salesOrg = salesOrg;
            this.id = id;
        }

        public void startLogs(string salesOrg) {
          //  idaLog.insertToActivityLog("Switches", "startTime", id, salesOrg);
        }

        public void populateSwitchesLog(IDBServerConnector dbserver) {
           // dbserver.listToServer(switchList, "SwitchLog");
        }

        public bool calculateSwitches(DataCollectorServer dcServer, DataCollectorSap dcSap) {
            var DCSS = new DataCollectorServiceSwitches(dcServer, dcSap);
            var list = DCSS.getSwitchesList(salesOrg, id);
            if (list is null) {
                return false;
            } else {
                switchList = list;
                AutomaticSwitchesList = switchList.Where(x => x.switchAutomatic == true).ToList();
                ManualSwitchesList = switchList.Where(x => x.switchAutomatic == false).ToList();
                return true;
            }
        }

        public void endLogs(string salesOrg, string switchType, string status) {
            idaLog.insertToActivityLog(switchType, status, id, salesOrg);
        }

        public void createAutomaticSwitchObjectList() {
            var switchOrderObj = new SwitchesOrderPropertyFactory();
            automaticSwitchObjectList = switchOrderObj.getSapSwitchObjectList(AutomaticSwitchesList);
        }

        public void runSwitchesInVA02() {
            string tableName = "SwitchLog";
            var orderCount = automaticSwitchObjectList.Count;
            switch (orderCount) {
                case 1: {
                        // 1 session
                        runExecution(1, tableName);
                        break;
                    }

                case var _ when orderCount < 5: {
                        // 2 sessions
                        runExecution(2, tableName);
                        break;
                    }

                case var _ when orderCount > 4: {
                        // 3 sessions
                        runExecution(3, tableName);
                        break;
                    }
            }
        }

        public void runExecution(byte session, string tableName) {
            var taskList = new List<Task>();
            var listList = automaticSwitchObjectList.SplitOnAmountOfLists(session);
            byte realSession = Conversions.ToByte(session - 1);
            for (byte i = 0, loopTo = realSession; i <= loopTo; i++) {
                byte q = i; // multithreading issue solver
                taskList.Add(Task.Run(() => runSapSession(q, (List<SwitchesSapOrderProperty>)listList.ElementAtOrDefault(q), tableName)));
            }

            try {
                Task.WaitAll(taskList.ToArray());
            } catch (AggregateException ae) {
                foreach (var innerX in ae.InnerExceptions) {
                    GlobalErrorHandler.handle(salesOrg, "Automatic Switches", innerX);
                }
            }

            while (realSession > 0) {
                var sap = Create.sapLib(realSession);
                sap.closeSessionWindow();
                realSession -= 1;
            }
        }

        private void runSapSession(byte session, List<SwitchesSapOrderProperty> list, string tableName) {
            ISAPLib sap;
            if (session > 0) {
                sap = Create.sapLib();
                sap.createSession();
                try {
                    sap = Create.sapLib(session);
                } catch (Exception) {
                    System.Threading.Thread.Sleep(3000);
                    sap = Create.sapLib(session);
                }
            } else {
                sap = Create.sapLib(session);
            }

            VA02 va02 = new VA02(sap, idaLog);
            SwitchesVA02Runner va02Runner = new SwitchesVA02Runner(sap, idaLog, va02);

            foreach (var switchObj in list) {
                var status = va02Runner.runSwitches(switchObj, id, tableName);
                updateOrderLog(tableName, switchObj, status);
            }
        }

        private void updateOrderLog(string tableName, SwitchesSapOrderProperty switchObj, OrderStatus status) {
            switch (status) {
                case OrderStatus.success: {
                        idaLog.update(
                            tableName,
                            new[] { "endTime" },
                            new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                            new[] { "[orderNumber]", "[id]", "[SwitchAutomatic]" },
                            new[] { switchObj.orderNumber.ToString(), id, "1" });
                        break;
                    }

                case OrderStatus.failedToSave: {
                        updateFailedOrderLog("Failed to save order", tableName, switchObj.orderNumber);
                        break;
                    }

                case OrderStatus.bothSkusAreZ4: {
                        updateFailedOrderLog("Old and new skus are in Z4", tableName, switchObj.orderNumber);
                        break;
                    }

                case OrderStatus.blockedByBatchJob: {
                        updateFailedOrderLog("Blocked by batch job", tableName, switchObj.orderNumber);
                        break;
                    }

                case OrderStatus.blockedByUser: {
                        updateFailedOrderLog("Blocked by user", tableName, switchObj.orderNumber);
                        break;
                    }

                case OrderStatus.realeasedOrRejected: {
                        updateFailedOrderLog("Order released or rejected", tableName, switchObj.orderNumber);
                        break;
                    }

                case OrderStatus.bothSkusAreNotApproved: {
                        updateFailedOrderLog("Old and new skus are not approved", tableName, switchObj.orderNumber);
                        break;
                    }

                case OrderStatus.orderMissingPaymentTerms: {
                        updateFailedOrderLog("Cannot save order due to missing payment terms", tableName, switchObj.orderNumber);
                        break;
                    }

                default: {
                        throw new NotImplementedException();
                    }
            }
        }

        private void updateFailedOrderLog(string reason, string tableName, long orderNumber) {
            idaLog.update(
                tableName,
                new[] { "status", "reason", "endTime" },
                new[] { "fail", reason, Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                new[] { "[orderNumber]", "[id]", "[SwitchAutomatic]" },
                new[] { orderNumber.ToString(), id, "1" });
        }

        public void sendFailedSwitches(string salesOrg, string email, IDBServerConnector dbServer, IMailUtil mu) {
            string failedListQuery = $"Select * from SwitchLog where [id] = '{AutomaticSwitchesList[0].id}' And (status is null or status <> 'success') AND SwitchAutomatic = 1";
            var rs = dbServer.executeQuery(failedListQuery);
            if (!rs.EOF)
                mu.mailSimple(email, $"{salesOrg} Failed Switches {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items if needed<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA");
        }

        public void sendReplacedCMIRs(string salesOrg, string email, IMailUtil mu, IDBServerConnector dbServer) {
            string failedListQuery = $"Select distinct * from CMIR where [salesOrg] = '{salesOrg}'";
            var rs = dbServer.executeQuery(failedListQuery);
            if (!rs.EOF) {
                mu.mailSimple(
                    "DLGBFrmsOTDCENTRALISEDTEAM@scj.com;" + email,
                    $"{salesOrg} Replaced CMIR's during switches {DateTime.Now}",
                    $"Hello <br>Dear Centralised Team representative of the sales organisation: {salesOrg}<br>Please do the below cmir links that were discovered from the switch task<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA"
                    );
            }

            dbServer.executeQuery($"Delete from CMIR where [salesOrg] = '{salesOrg}'");
        }
    }
}