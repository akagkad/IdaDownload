using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.SAP.TCode;
using IDAUtil.SAP.TCode.Runners;
using lib;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RDD {
    public class RddTaskExecutor {
        private IdaLog idaLog = new IdaLog();
        private readonly IMailUtil mu = Create.mailUtil();
        private string salesOrg;
        public List<RddOutputBean> rddList = new List<RddOutputBean>();
        private List<BankHolidayProperty> bhList = new List<BankHolidayProperty>();
        private List<CalculatedRddOutputBean> calculatedRDDList = new List<CalculatedRddOutputBean>();
        private List<CalculatedRddOutputBean> calculatedDelBlockList = new List<CalculatedRddOutputBean>();
        private List<CalculatedRddOutputBean> calculatedRouteCodeList = new List<CalculatedRddOutputBean>();
        public List<CalculatedRddOutputBean> calculatedList = new List<CalculatedRddOutputBean>();
        public List<CalculatedRddOutputBean> preparedList = new List<CalculatedRddOutputBean>();

        public RddTaskExecutor(string salesOrg) {
            this.salesOrg = salesOrg;
        }

        public void startLogs(string salesOrg, string id) {
            idaLog.insertToActivityLog("DeliveryDateChanges", "startTime", id, salesOrg);
        }

        public void setBHList(string salesOrg, DataCollectorServiceRDD dc) {
            bhList = dc.getBHList(salesOrg);
        }

        public void setRddList(string salesOrg, string id, DataCollectorServiceRDD dc) {
            rddList = dc.GetRddList(salesOrg, id);
        }

        public void prepareList() {
            var rddCalc = new RddDataCalculator(bhList, BHUtil.isSkipWeekend(salesOrg), DateTime.Today);
            var list = new List<CalculatedRddOutputBean>();
            var switchExpr = salesOrg;
            
            switch (switchExpr) {
                case "ZA01":
                case "KE02":
                case "NG01": {
                        list = rddCalc.getCalculatedRDDList(rddList).Where(x => x.oldRdd != x.newRecommendedRdd || x.oldRdd == x.newRecommendedRdd && (!string.IsNullOrEmpty(x.delBlock) || !string.IsNullOrEmpty(x.newRecommendedRouteCode))).ToList();
                        break;
                    }
                case "RO01": {
                        list = rddCalc.getCalculatedRDDList(rddList).Where(x => (x.oldRdd < x.newRecommendedRdd || x.oldRdd.DayOfWeek.ToString() == "Saturday" || x.oldRdd.DayOfWeek.ToString() == "Sunday") || x.oldRdd == x.newRecommendedRdd && (!string.IsNullOrEmpty(x.delBlock) || !string.IsNullOrEmpty(x.newRecommendedRouteCode))).ToList();
                        break;
                    }
                default: {
                        list = rddCalc.getCalculatedRDDList(rddList).Where(x => x.oldRdd < x.newRecommendedRdd || x.oldRdd == x.newRecommendedRdd && (!string.IsNullOrEmpty(x.delBlock) || !string.IsNullOrEmpty(x.newRecommendedRouteCode))).ToList();
                        break;
                    }
            }

            var sortedList = new List<CalculatedRddOutputBean>();
            sortedList = list.OrderBy(x => x.delBlock).ToList();
            preparedList = sortedList;
        }

        public void calculateLists() {
            // separating into calculated lists
            calculatedRDDList = preparedList.Where(x => string.IsNullOrEmpty(x.delBlock) && x.isRddChangeAllowed).ToList();
            calculatedDelBlockList = preparedList.Where(x => !string.IsNullOrEmpty(x.delBlock)).ToList();
            calculatedRouteCodeList = preparedList.Where(x => (x.route ?? "") != (x.newRecommendedRouteCode ?? "") && !string.IsNullOrEmpty(x.newRecommendedRouteCode) && x.isRouteCodeChangeAllowed).ToList();
            
            // list to populate the log with
            calculatedList.AddRange(calculatedRDDList);
            calculatedList.AddRange(calculatedDelBlockList);
            calculatedList.AddRange(calculatedRouteCodeList);
        }

        public void runVA02(ISAPLib sap) {
            string tableName = "DeliveryDatesLog";
            IVA02 va02 = new VA02(sap, idaLog);

            foreach (var item in calculatedRDDList) {
                RDDVA02Runner va02RDDRunner = new RDDVA02Runner(sap, idaLog, va02);
                va02RDDRunner.runRDDChange(item.orderNumber, item.oldRdd, item.newRecommendedRdd, item.id, item.reason, tableName);
            }

            foreach (var item in calculatedDelBlockList) {
                DeliveryBlockVA02Runner va02DelBlockRunner = new DeliveryBlockVA02Runner(sap, idaLog, va02);
                va02DelBlockRunner.runDeliveryBlockChange(item.orderNumber, item.id, item.reason, item.delBlock, tableName);
            }

            foreach (var item in calculatedRouteCodeList) {
                RouteCodeVA02Runner va02RouteCodeRunner = new RouteCodeVA02Runner(sap, idaLog, va02);
                va02RouteCodeRunner.runRouteCodeChange(item.orderNumber, item.id, item.reason, item.newRecommendedRouteCode, tableName, salesOrg.ToUpper());
            }
        }

        public void populateDeliveryDatesLog(IDBServerConnector dbserver) {
            dbserver.listToServer(calculatedList, "DeliveryDatesLog");
        }

        public void sendEmailWithChanges(string salesOrg, string email) {
            mu.mailSimple(email, $"{salesOrg} RDD changes {DateTime.Now}", $"Hello<br><br><br>{mu.listToHTMLtable(calculatedList)}<br><br>Kind Regards<br>IDA");
        }

        public void emptyListAction(string salesOrg, string id, string email, bool IsEmptySap) {
            idaLog.insertToActivityLog("DeliveryDateChanges", "empty list", id, salesOrg);
            mu.mailSimple(email, $"{salesOrg} No RDD changes {DateTime.Now}", $"Hello<br>{(IsEmptySap ? "No orders in ZV04HN" : "All RDD's are correct")} <br><br>Kind regards<br>IDA");
        }

        public void sendFailedRdds(string salesOrg, string email, IDBServerConnector dbServer) {
            if (calculatedList.Count > 0) {
                string failedListQuery = $"Select * from DeliveryDatesLog where [id] = '{calculatedList[0].id}' And (status is null or status <> 'success')";
                var rs = dbServer.executeQuery(failedListQuery);

                if (!rs.EOF) {
                    mu.mailSimple(email, $"{salesOrg} Failed RDD items {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items manually<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA");
                }
            }
        }

        public void handleError(string salesOrg, string id, Exception ex) {
            string filepath = $@"{IDAConsts.Paths.errorFilePath}\{salesOrg} RDD error {Strings.Format(DateTime.Now, "yyyy.MM.dd mm.hh")}.txt";
            var f = Create.fileUtil();
            f.writeToTxtFile(filepath, ex.Source + Constants.vbCr + ex.Message + Constants.vbCr + ex.StackTrace);
            mu.mailSimple(IDAConsts.adminEmail, $"{salesOrg} RDD: Error {Information.Err().Number}", $"{mu.getLink(filepath, "Your error info is here")}");
            idaLog.insertToActivityLog("DeliveryDateChanges", "fail", id, salesOrg);
        }

        public void finishLogs(string salesOrg, string id, string status) {
            idaLog.insertToActivityLog("DeliveryDateChanges", status, id, salesOrg);
        }

        public void sendIT01Report(string salesOrg, string email, IDBServerConnector dbServer) {
            string changedOrdersToReleaseTodayList = $"Select * from DeliveryDatesLog where salesOrg = '{salesOrg}' [startTime] > GETDATE() AND status = 'success'";
            var rs = dbServer.executeQuery(changedOrdersToReleaseTodayList);
            mu.mailSimple(email, $"{salesOrg} Failed RDD items {DateTime.Now}", $"Hello <br>Please investigate and action the below failed items manually<br><br>{mu.rsToHTMLtable(rs)}<br><br>Kind regards<br>IDA");
        }
    }
}