using DeliveryBlocks.Model.CountryModel;
using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.SAP.TCode;
using IDAUtil.SAP.TCode.Runners;
using IDAUtil.Service;
using IDAUtil.Support;
using lib;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBlocks.Service {
    public class DeliveryBlocksExecutor {
        #region Properties
        private const bool isLog = true; // when testing functions
        private IdaLog idaLog = new IdaLog();
        private readonly string salesOrg;
        private string id;
        public List<DeliveryBlocksProperty> deliveryBlockList;
        private string tableName = "DeliveryBlockLog";
        #endregion

        public DeliveryBlocksExecutor(string salesOrg, string id) {
            this.salesOrg = salesOrg;
            this.id = id;
        }

        public bool calculateDeliveryBlockList(IDataCollectorServiceDeliveryBlocks dataCollector) {

            deliveryBlockList = dataCollector.getDelBlockList();

            if (deliveryBlockList is null || deliveryBlockList.Count == 0) {
                return false;
            } else {
                return true;
            }

        }

        public void populateDeliveryBlocksLog(IDBServerConnector dbServer) {
            dbServer.listToServer(deliveryBlockList, tableName);
        }

        public void startLog() {
            idaLog.insertToActivityLog("Delivery Blocks", "start", id, salesOrg);
        }

        public void finishLog(bool isEmpty) {
            if (isEmpty) {
                idaLog.insertToActivityLog("Delivery Blocks", "EmptyList", id, salesOrg);
            } else {
                idaLog.insertToActivityLog("Delivery Blocks", "success", id, salesOrg);
            }
        }

        public void sendEmail(string email, IMailUtil mu, bool isEmptyList) {
            if (isEmptyList) {
                mu.mailSimple(email, $"{salesOrg} No Delivery Blocks Found {DateTime.Now}", "Hello<br><br>No delivery block changes to action<br><br>Kind Regards<br>IDA");
            } else {
                sendSpecifficCountryList(email, mu);
            }
        }

        private void sendSpecifficCountryList(string email, IMailUtil mu) {

            List<DeliveryBlocksProperty> deliveryBlockForCustomerEmailList;

            switch (salesOrg) {

                case "RO01":
                    List<RODeliveryBlocksProperty> roList = (from delblock in deliveryBlockList select new RODeliveryBlocksProperty(delblock)).ToList();
                    mu.mailSimple(email, $"{salesOrg} Delivery Blocks {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(roList)}<br><br>Kind Regards<br>IDA");

                    break;
                case "RU01":

                    mu.mailSimple(email, $"{salesOrg} Delivery Blocks {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(deliveryBlockList)}<br><br>Kind Regards<br>IDA");

                    deliveryBlockForCustomerEmailList = deliveryBlockList.
                        Distinct(x => x.shipTo).
                        Where(y => y.newDeliveryBlock != " " && !(string.IsNullOrEmpty(y.customerEmails))).
                        ToList();

                    foreach (DeliveryBlocksProperty x in deliveryBlockForCustomerEmailList) {
                        try {
                            sendRU01TemplateEmail(mu, x);
                        } catch (Exception) {
                            mu.mailSimple(email, $"Delivery Blocks Automated Email Error -  Email Does not exist", $"Hello<br><br> There was an eror sending email to {x.customerEmails}.<br>Please check and correct it<br><br>Kind Regards<br>IDA");
                        }
                    }

                    break;

                case "CZ01":
                    mu.mailSimple(email, $"{salesOrg} Delivery Blocks {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(deliveryBlockList)}<br><br>Kind Regards<br>IDA");

                    deliveryBlockForCustomerEmailList = deliveryBlockList.
                       Distinct(x => x.shipTo).
                       Where(y => y.newDeliveryBlock != " " && !(string.IsNullOrEmpty(y.customerEmails))).
                       ToList();

                    foreach (DeliveryBlocksProperty x in deliveryBlockForCustomerEmailList) {
                        try {
                            sendCZ01TemplateEmail(mu, x);
                        } catch (Exception) {
                            mu.mailSimple(email, $"Delivery Blocks Automated Email Error -  Email Does not exist", $"Hello<br><br> There was an eror sending email to {x.customerEmails}.<br>Please check and correct it<br><br>Kind Regards<br>IDA");
                        }
                    }

                    break;

                case "GB01":
                case "PT01":
                case "ES01":
                    //GB doesnt want email of what the script is planning to change
                    //ES and PT do not apply any blocks but only send report of what is blocked
                    break;

                default:
                    mu.mailSimple(email, $"{salesOrg} Delivery Blocks {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(deliveryBlockList)}<br><br>Kind Regards<br>IDA");

                    break;
            }
        }

        private static void sendRU01TemplateEmail(IMailUtil mu, DeliveryBlocksProperty x) {
            mu.mailSimple(x.customerEmails,
                                        $"{x.shipToName} {DateTime.Now} Автоматический имейл", $"Добрый день,<br>Уважаемые Партнеры,<br><br>" +
                                        $"Пожалуйста, обратите внимаение на то что Ваш заказ {x.poNumber} от {x.poDate} меньше Вашего минимального кванта доставки.<br> " +
                                        $"{(x.minQty > x.currentQty ? $"Просьба разместить дозаказ в количестве  {Math.Round(x.minQty - x.currentQty)} коробов.<br>" : "")}" +
                                        $"{(x.minQty > x.currentQty && x.minVal > x.currentVal ? "Или " : "Просьба ")} {(x.minVal > x.currentVal ? $"разместить дозаказ в общей стоимости {Math.Round(x.minVal - x.currentVal)} руб.<br>" : "")}" +
                                        $"Для достижения минимального кванта и осуществления доставки к указанной дате {x.rdd.Date}.<br>" +
                                        $"<br>С Уважением,<br>SC Johnson"); ;
        }

        private static void sendCZ01TemplateEmail(IMailUtil mu, DeliveryBlocksProperty x) {
            mu.mailSimple(x.customerEmails,
                                        $"{x.shipToName} {DateTime.Now} Automated Email", $"Good Day,<br><br>" +
                                        $"Please order  {Math.Round(x.minQty - x.currentQty)} more cases in order to achieve minimum order quantity of  {x.minQty} for PO:  {x.poNumber}" +
                                        $"<br><br>Kind Regards,<br>SC Johnson"); ;
        }

        public void runDeliveryBlocksInVA02() {
            var orderCount = deliveryBlockList.Count;
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
            var listList = deliveryBlockList.SplitOnAmountOfLists(session);

            byte realSession = Conversions.ToByte(session - 1);
            for (byte i = 0, loopTo = realSession; i <= loopTo; i++) {
                byte q = i; // multithreading issue solver
                taskList.Add(Task.Run(() => runSapSession(q, (List<DeliveryBlocksProperty>)listList.ElementAtOrDefault(q))));
            }

            try {
                Task.WaitAll(taskList.ToArray());
            } catch (AggregateException ae) {
                foreach (var innerX in ae.InnerExceptions) {
                    GlobalErrorHandler.handle(salesOrg, "Delivery Blocks", innerX);
                }
            }

            while (realSession > 0) {
                var sap = Create.sapLib(realSession);
                sap.closeSessionWindow();
                realSession -= 1;
            }
        }

        private void runSapSession(byte session, List<DeliveryBlocksProperty> list) {
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
            DeliveryBlockVA02Runner va02Runner = new DeliveryBlockVA02Runner(sap, idaLog, va02);

            foreach (var blockObject in list) {
                va02Runner.runDeliveryBlockChange(blockObject.orderNumber, id, blockObject.reason, blockObject.newDeliveryBlock, tableName);
            }
        }

        public void sendFailedDeliveryBlocks(string salesOrg, string email, IDBServerConnector dbServer, IMailUtil mu) {
            if (deliveryBlockList.Count > 0) {
                string failedListQuery = $"Select * from {tableName} where [id] = '{deliveryBlockList[0].id}' And (status is null or status <> 'success')";
                var rs = dbServer.executeQuery(failedListQuery);

                if (!rs.EOF) {
                    mu.mailSimple(email, $"{salesOrg} Failed Delivery Blocks {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items if needed<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA");
                }
            }
        }

        public void rejectLastReleaseBlockedOrders(IDBServerConnector dbServer, IDataCollectorSap dcSap, IDataCollectorServer dcServer, IMailUtil mu, string email) {
            //Reject orders that failed to reach min val and qty for ZA01 during the day

            TimeSpan lastReleaseTimeZATime = new TimeSpan(14, 0, 0);

            if (salesOrg.ToUpper() == "ZA01" && DateAndTime.Now.TimeOfDay >= lastReleaseTimeZATime) {

                RejectionExecutor rejectionExecutor = new RejectionExecutor(dcSap, dcServer, salesOrg, id, idaLog);

                if (rejectionExecutor.calculateOrdersToRejectList()) {
                    mu.mailSimple(email, $"{salesOrg} Orders to be rejected failing to reach MOQ {DateTime.Now}", $"Hello<br><br>{mu.listToHTMLtable(rejectionExecutor.ordersToRejectList)}<br><br>Kind Regards<br>IDA");
                    rejectionExecutor.populateFullOrderRejectionLog(dbServer);
                    rejectionExecutor.runRejectionsInVA02();
                } else {
                    mu.mailSimple(email, $"{salesOrg} No orders to be rejected failing to reach MOQ {DateTime.Now}", $"Hello<br><br>There are no orders that failed to reach MOQ<br><br>Kind Regards<br>IDA");
                }
            }
        }

        public void sendCurrentBlocksInSystem(IMailUtil mu, IDataCollectorSap dcSap, string email) {
            switch (salesOrg.ToUpper()) {
                case "GB01":
                    emailCurrentBlocksGB(mu, dcSap, email);
                    break;
                case "ES01":
                case "PT01":
                    emailCurrentBlocksESAndPT(mu, dcSap, email);
                    break;
                case "RU01":
                case "CZ01":
                case "FR01":
                    emailCurrentBlocksGeneric(mu, dcSap, email);
                    break;
                default:
                    return;
            }
        }

        private void emailCurrentBlocksGeneric(IMailUtil mu, IDataCollectorSap dcSap, string email) {
            List<ZV04HNProperty> zvHNList = dcSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks).Where(x => (x.status.ToLower() == "open order" || x.status.ToLower() == "credit hold")).ToList();

            List<GBDeliveryBlocksProperty> blockList = getBlockListGeneric(zvHNList: zvHNList); 

            mu.mailSimple(email, $"{salesOrg} {DateTime.Now} Main Delivery Blocks", $"Hello<br><br>{mu.listToHTMLtable(blockList)}<br><br>Kind Regards<br>IDA");
        }

        private void emailCurrentBlocksGB(IMailUtil mu, IDataCollectorSap dcSap, string email) {
            List<ZV04HNProperty> zvHNList = dcSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks).Where(x => (x.status.ToLower() == "open order" || x.status.ToLower() == "credit hold")).ToList(); ;

            List<GBDeliveryBlocksProperty> z8BlockList = getBlockListGB(zvHNList: zvHNList, isZ8: true); //Z8 block list only that exist in zv04hn
            List<GBDeliveryBlocksProperty> mainBlockList = getBlockListGB(zvHNList: zvHNList, isZ8: false); //All the rest delivery block list apart from Z8 that exist in zv04hn

            mu.mailSimple(email, $"{salesOrg} {DateTime.Now} Z8 Delivery Blocks", $"Hello<br><br>{mu.listToHTMLtable(z8BlockList)}<br><br>Kind Regards<br>IDA");
            mu.mailSimple(email, $"{salesOrg} {DateTime.Now} Main Delivery Blocks", $"Hello<br><br>{mu.listToHTMLtable(mainBlockList)}<br><br>Kind Regards<br>IDA");
        }

        private void emailCurrentBlocksESAndPT(IMailUtil mu, IDataCollectorSap dcSap, string email) {
            List<ZV04HNProperty> zvHNList = dcSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks)
                .Where(x => (x.status.ToLower() == "open order" || x.status.ToLower() == "credit hold"))
                .ToList(); ;

            List<ESAndPTDeliveryBlocksProperty> centralisedBlockList = getBlockListESAndPT(zvHNList: zvHNList, isCentralised: true)
                .OrderBy(x => x.delBlock)
                .ThenBy(x => x.csrName)
                .ToList();

            List<ESAndPTDeliveryBlocksProperty> collaborationBlockList = getBlockListESAndPT(zvHNList: zvHNList, isCentralised: false)
                .OrderBy(x => x.delBlock)
                .ThenBy(x => x.csrName)
                .ToList();

            mu.mailSimple(email, $"{salesOrg} {DateTime.Now} Delivery Blocks", $"Hello<br><br>Centralised Team:<br>{ (centralisedBlockList.Count > 0 ? mu.listToHTMLtable(centralisedBlockList) : "No Blocks") }<br><br>" +
                                                                               $"Collaboration Team:<br>{ (collaborationBlockList.Count > 0 ? mu.listToHTMLtable(collaborationBlockList) : "No Blocks") }<br><br>Kind Regards<br>IDA");
        }

        private List<ESAndPTDeliveryBlocksProperty> getBlockListESAndPT(List<ZV04HNProperty> zvHNList, bool isCentralised) {
            if (salesOrg.ToUpper() == "PT") {
                return (from zv in zvHNList
                        where isCentralised ? zv.delBlock == "ZG" || zv.delBlock == "Z8" : zv.delBlock != "Z8" && zv.delBlock != "ZG" && !string.IsNullOrEmpty(zv.delBlock)
                        select new ESAndPTDeliveryBlocksProperty(zv)
                    ).ToList();
            } else {
                return (from zv in zvHNList
                        where isCentralised ? zv.delBlock == "ZG" : zv.delBlock != "ZG" && !string.IsNullOrEmpty(zv.delBlock)
                        select new ESAndPTDeliveryBlocksProperty(zv)
                    ).ToList();
            }
        }

        private List<GBDeliveryBlocksProperty> getBlockListGB(List<ZV04HNProperty> zvHNList, bool isZ8) {
            return (from zv in zvHNList
                    where isZ8 ? zv.delBlock == "Z8" : zv.delBlock != "Z8" && !string.IsNullOrEmpty(zv.delBlock)
                    select new GBDeliveryBlocksProperty(zv)
                       ).ToList();
        }

        private List<GBDeliveryBlocksProperty> getBlockListGeneric(List<ZV04HNProperty> zvHNList) {
            return (from zv in zvHNList
                       where !string.IsNullOrEmpty(zv.delBlock)
                       select new GBDeliveryBlocksProperty(zv)
                       ).ToList();
        }

    }
}
