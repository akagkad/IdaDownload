using IDAUtil;
using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.TcodeProperty.VA02;
using IDAUtil.SAP.TCode;
using IDAUtil.SAP.TCode.Runners;
using IDAUtil.Support;
using lib;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Rejections.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Rejections {
    public class RejectionsTaskExecutor {
        #region public properties
        public List<RejectionsProperty> releaseRejectionList;
        public List<RejectionsProperty> afterReleaseRejectionList;
        public List<RejectionsSapOrderProperty> automaticRejectionsObjectList = new List<RejectionsSapOrderProperty>();
        #endregion

        #region private properties
        private string salesOrg;
        private string id;
        private bool isReleaseRejections;
        private IIdaLog idaLog;
        private List<RejectionsProperty> rejectionsList;
        #endregion

        public RejectionsTaskExecutor(string salesOrg, string id, bool isReleaseRejections, IIdaLog idaLog) {
            this.salesOrg = salesOrg;
            this.id = id;
            this.isReleaseRejections = isReleaseRejections;
            this.idaLog = idaLog;
        }

        public bool calculateLists(IDataCollectorServiceRejections dataCollector, IDataCalculatorServiceRejections dataCalculator) {

            if (isReleaseRejections) {

                dataCollector.populateReleaseRejectionList(salesOrg);

                rejectionsList = dataCalculator.getRejectionsList(dataCollector);

                if (rejectionsList is null || rejectionsList.Count == 0) { return false; }

                rejectionsList = dataCollector.getFinalListWithStockDetails(rejectionsList, salesOrg);

                releaseRejectionList = rejectionsList.Where(x => x.confirmedQty > 0).ToList();
                afterReleaseRejectionList = rejectionsList.Where(x => x.confirmedQty == 0).ToList();
            } else {
                afterReleaseRejectionList = dataCollector.getReleaseRejectionsListFromLog(salesOrg);
                if (afterReleaseRejectionList.Count == 0) { return false; }
            }

            return true;
        }

        public void createAutomaticRejectionObjectList(IRejectionsOrderPropertyFactory rejectionOrderObj) {
            if (isReleaseRejections) {
                automaticRejectionsObjectList = rejectionOrderObj.getSapRejectionsObjectList(releaseRejectionList);
            } else {
                automaticRejectionsObjectList = rejectionOrderObj.getSapRejectionsObjectList(afterReleaseRejectionList);
            }
        }

        public void populateRejectionLog(IDBServerConnector dbServer) {
            dbServer.listToServer(rejectionsList, "RejectionsLog");
        }

        public void endLogs(string salesOrg, string rejectionType, string status) {
            idaLog.insertToActivityLog(rejectionType, status, id, salesOrg);
        }

        public void startLogs(string salesOrg, string rejectionType, string status) {
            idaLog.insertToActivityLog(rejectionType, status, id, salesOrg);
        }

        public void runRejectionsInVA02(string email, IDBServerConnector dbServer, IMailUtil mu) {
            string tableName = "RejectionsLog";
            int orderCount = automaticRejectionsObjectList.Count;

            try {
                switch (orderCount) {
                    case 1: {
                            // 1 session
                            runExecution(1, tableName);
                            break;
                        }
                    case int _ when orderCount < 5: {
                            // 2 sessions
                            runExecution(2, tableName);
                            break;
                        }
                    case int _ when orderCount > 4: {
                            // 3 sessions
                            runExecution(3, tableName);
                            break;
                        }
                }
            } catch (Exception ex) {
                GlobalErrorHandler.handle(salesOrg, "After release rejections", ex);
                sendFailedRejections(salesOrg, email, dbServer, mu);
            }
        }

        public void runExecution(byte session, string tableName) {
            var taskList = new List<Task>();
            var listList = automaticRejectionsObjectList.SplitOnAmountOfLists(session);
            byte realSession = Conversions.ToByte(session - 1);

            for (byte i = 0, loopTo = realSession; i <= loopTo; i++) {
                byte q = i; // multithreading issue solver
                taskList.Add(Task.Run(() => runSapSession(q, (List<RejectionsSapOrderProperty>)listList.ElementAtOrDefault(q), tableName)));
            }

            try {
                Task.WaitAll(taskList.ToArray());
            } catch (AggregateException ae) {
                foreach (var innerX in ae.InnerExceptions)
                    GlobalErrorHandler.handle(salesOrg, isReleaseRejections ? "Release Rejections" : "After Release Rejections", innerX);
            }

            while (realSession > 0) {
                var sap = Create.sapLib(realSession);
                sap.closeSessionWindow();
                realSession -= 1;
            }
        }

        private void runSapSession(byte session, List<RejectionsSapOrderProperty> list, string tableName) {
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

            IVA02 va02 = new VA02(sap, idaLog);
            RejectionsVA02Runner va02Runner = new RejectionsVA02Runner(sap, idaLog, va02, isReleaseRejections, true);

            foreach (var rejObj in list) {
                var status = va02Runner.runRejections(rejObj, id, tableName);
                updateOrderLog(tableName, rejObj, status);
            }

        }

        private void updateOrderLog(string tableName, RejectionsSapOrderProperty rejObj, OrderStatus status) {
            switch (status) {
                case OrderStatus.success: {
                        idaLog.update(
                            tableName: tableName,
                            columnNames: new[] { "endTime" },
                            values: new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                            conditionName: isReleaseRejections ? new[] { "orderNumber", "id", "isDuringRelease" } : new[] { "orderNumber", "isDuringRelease" },
                            conditionValue: isReleaseRejections ? new[] { rejObj.orderNumber.ToString(), id, "1" } : new[] { rejObj.orderNumber.ToString(), "0", },
                           customCondition: isReleaseRejections ? "" : "([endTime] IS NULL)"
                            );
                        break;
                    }

                case OrderStatus.failedToSave: {
                        updateFailedOrderLog("Failed to save order", tableName, rejObj.orderNumber);
                        break;
                    }

                case OrderStatus.blockedByBatchJob: {
                        updateFailedOrderLog("Blocked by batch job", tableName, rejObj.orderNumber);
                        break;
                    }

                case OrderStatus.blockedByUser: {
                        updateFailedOrderLog("Blocked by user", tableName, rejObj.orderNumber);
                        break;
                    }

                case OrderStatus.orderMissingPaymentTerms: {
                        updateFailedOrderLog("Cannot save order due to missing payment terms", tableName, rejObj.orderNumber);
                        break;
                    }

                default: {
                        throw new NotImplementedException();
                    }
            }
        }

        private void updateFailedOrderLog(string reason, string tableName, long orderNumber) {
            idaLog.update(
                tableName: tableName,
                columnNames: new[] { "status", "reason", "endTime" },
                values: new[] { "fail", reason, Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                conditionName: isReleaseRejections ? new[] { "[orderNumber]", "[id]", "[isDuringRelease]" } : new[] { "[orderNumber]", "[isDuringRelease]" },
                conditionValue: isReleaseRejections ? new[] { orderNumber.ToString(), id, "1" } : new[] { orderNumber.ToString(), "0" }
                );
        }

        private void sendFailedRejections(string salesOrg, string email, IDBServerConnector dbServer, IMailUtil mu) {
            string failedListQuery;
            if (isReleaseRejections) {
                failedListQuery = $"Select * from RejectionsLog where [id] = '{rejectionsList[0].id}' And (status is null or status <> 'success') AND isDuringRelease = 1";
            } else {
                failedListQuery = $"Select * from RejectionsLog where [salesOrg] = '{salesOrg}' AND (status is null or status <> 'success') AND isDuringRelease = 0 AND (Cast([startTime] as date) = Cast(CURRENT_TIMESTAMP as date))";
            }
            var rs = dbServer.executeQuery(failedListQuery);
            if (!rs.EOF)
                mu.mailSimple(email, $"{salesOrg} Failed {(isReleaseRejections ? "Release " : "After Release ") }Rejections  {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items if needed<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA");
        }
    }
}