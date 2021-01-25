using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.SAP.TaskVA02Runners;
using IDAUtil.SAP.TCode;
using IDAUtil.SAP.TCode.Runners;
using IDAUtil.Service;
using IDAUtil.Support;
using lib;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBlocks.Service {
    class RejectionExecutor {
        private const string belowMOQBlock = "ZF";
        private const string tableName = "FullOrderRejectionLog";

        private IDataCollectorSap dcSap;
        private IDataCollectorServer dcServer;
        public List<RejectionFullOrderProperty> ordersToRejectList;
        private IdaLog idaLog;
        private string id;
        private string salesOrg;

        public RejectionExecutor(IDataCollectorSap dcSap, IDataCollectorServer dcServer, string salesOrg, string id, IdaLog idaLog) {
            this.dcSap = dcSap;
            this.dcServer = dcServer;
            this.salesOrg = salesOrg;
            this.idaLog = idaLog;
            this.id = id;
        }

        public bool calculateOrdersToRejectList() {
            List<ZV04HNProperty> zvhList = dcSap.getZV04HNList(salesOrg, IDAEnum.Task.rejections);
            List<CustomerDataProperty> cdList = dcServer.getCustomerDataList(salesOrg);

            ordersToRejectList = (
                                      from z in zvhList
                                      join c in cdList
                                      on new { key0 = z.soldto, key1 = z.shipto } equals new { key0 = c.shipTo, key1 = c.shipTo }
                                      where z.delBlock.ToUpper() == belowMOQBlock
                                      select new RejectionFullOrderProperty(id,
                                                                            c.salesOrg,
                                                                            c.country,
                                                                            z.soldto,
                                                                            z.shipto,
                                                                            z.shiptoName,
                                                                            z.order,
                                                                            z.pONumber,
                                                                            "ZK",
                                                                            z.orderQty,
                                                                            z.ordNetValue,
                                                                            "Failed to reach MOQ & MOV")
                                    ).ToList();

            if (ordersToRejectList.Count > 0) {
                return true;
            } else {
                return false;
            }

        }

        public void populateFullOrderRejectionLog(IDBServerConnector dbServer) {
            dbServer.listToServer(ordersToRejectList, tableName);
        }

        public void runRejectionsInVA02() {
            var orderCount = ordersToRejectList.Count;
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
            var listList = ordersToRejectList.SplitOnAmountOfLists(session);

            byte realSession = Conversions.ToByte(session - 1);
            for (byte i = 0, loopTo = realSession; i <= loopTo; i++) {
                byte q = i; // multithreading issue solver
                taskList.Add(Task.Run(() => runSapSession(q, (List<RejectionFullOrderProperty>)listList.ElementAtOrDefault(q))));
            }

            try {
                Task.WaitAll(taskList.ToArray());
            } catch (AggregateException ae) {
                foreach (var innerX in ae.InnerExceptions) {
                    GlobalErrorHandler.handle(salesOrg, "Full Order Rejection", innerX);
                }
            }

            while (realSession > 0) {
                var sap = Create.sapLib(realSession);
                sap.closeSessionWindow();
                realSession -= 1;
            }
        }

        private void runSapSession(byte session, List<RejectionFullOrderProperty> list) {
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
            FullOrderRejectionVA02Runner va02Runner = new FullOrderRejectionVA02Runner(sap, idaLog, va02);

            foreach (var rejObject in list) {
                va02Runner.runFullOrderRejection(rejObject.orderNumber, id, rejObject.reason, rejObject.rejCode, tableName);
            }
        }
    }
}
