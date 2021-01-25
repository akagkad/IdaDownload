using lib;
using Microsoft.VisualBasic;
using System;

namespace IDAUtil.SAP.TCode.Runners {
    public class RouteCodeVA02Runner {
        #region private properties
        private readonly IdaLog log;
        private readonly ISAPLib sap;
        private readonly IVA02 va02;
        #endregion

        public RouteCodeVA02Runner(ISAPLib sap, IdaLog log, IVA02 va02) {
            this.sap = sap;
            this.log = log;
            this.va02 = va02;
        }
        public void runRouteCodeChange(int orderNumber, string id, string reason, string routeCode, string tableName, string salesOrg) {
            string csrNote = "";

            startRouteCodeLog(orderNumber, id, tableName);

            OrderStatus status = va02.enterOrder(orderNumber);

            if (status != OrderStatus.available) {
                va02.updateLog(status, tableName, orderNumber.ToString(), id);
                return;
            }
            va02.bypassInitialPopups();

            status = va02.isChangeNeeded();

            if (status != OrderStatus.available) {
                va02.updateLog(status, tableName, orderNumber.ToString(), id);
                return;
            }

            csrNote = $"Route Code of {orderNumber} has been changed to {routeCode}, Reason: {reason}";

            ITable table = va02.getTable();

            va02.moveRouteCodeColumnToIndexEight(table);

            int i = 0;
            while (table.getCellValue(i, VA02.routeCodeColumnIndex) != "") {
                try {
                    table.setCellValue(i, VA02.routeCodeColumnIndex, $"{routeCode}");  // have to use by index because the table retains its indexes by name after reordering but not actual indexes within a table
                } catch (Exception) {
                    sap.getRidOfPopUps();
                    try {
                        table.setCellValue(i, VA02.routeCodeColumnIndex, $"{routeCode}");
                    } catch (NotChangeableCellException) {
                    }
                }
                i++;
            }

            sap.pressEnter();
            sap.getRidOfPopUps();

            if (isPLAndCZExpection(orderNumber, id, tableName, salesOrg)) {
                return;
            }

            va02.soarAction(csrNote, "Route Code Changes", orderNumber);
            va02.save();
            va02.updateOrderSavedLog(tableName, orderNumber, id);
        }

        private void startRouteCodeLog(int orderNumber, string id, string tableName) {
            log.update(tableName,
                            new[] { "startTime" },
                            new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                            new[] { "orderNumber", "id" },
                            new[] { orderNumber.ToString(), id });
        }

        private bool isPLAndCZExpection(int orderNumber, string id, string tableName, string salesOrg) {
            if (salesOrg == "PL01" || salesOrg == "CZ01") {
                var timeout = DateAndTime.DateAdd(DateInterval.Second, 10, DateAndTime.Now);

                // buggfix - The data necessary to complete this operation is not yet available.
                while (DateTime.Now < timeout) {
                    System.Threading.Thread.Sleep(2000);
                    try {
                        if (!sap.idExists(VA02ID.TOTAL_ORDER_CASES_FIELD_ID)) {
                            (sap.findById(VA02ID.ADDITIONAL_INFO_B_TAB_ID) as dynamic).Select();
                        } else {
                            break;
                        }
                    } catch (Exception) {
                    }
                }

                string orderQty = (sap.findById(VA02ID.TOTAL_ORDER_CASES_FIELD_ID) as dynamic).Text();
                string confirmedQty = (sap.findById(VA02ID.TOTAL_CONFIRMED_CASES_FIELD_ID) as dynamic).Text();

                if (orderQty != confirmedQty) {
                    log.update(
                        tableName,
                        columnNames: new[] { "endTime", "status", "reason" },
                        values: new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), "fail", "Could not change route codes as the CFR becomes lower than 100%" },
                        conditionName: new[] { "orderNumber", "id" },
                        conditionValue: new[] { orderNumber.ToString(), id, });
                    return true;
                } else {
                    sap.goBack();
                    return false;
                }
            } else {
                return false;
            }
        }
    }
}
