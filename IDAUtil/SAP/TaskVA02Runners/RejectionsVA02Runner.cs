using IDAUtil.Model.Properties.TcodeProperty.VA02;
using lib;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// CSR stands for Customer Service Represenative
/// CSR Note is required by business to populate reasons for changes inside of orders
/// </summary>
namespace IDAUtil.SAP.TCode.Runners {
    public class RejectionsVA02Runner {
        #region private properties
        private readonly IIdaLog log;
        private readonly ISAPLib sap;
        private readonly IVA02 va02;
        private readonly bool isRelease;
        private readonly bool isLog;
        #endregion

        public RejectionsVA02Runner(ISAPLib sap, IIdaLog log, IVA02 va02, bool isRelease, bool isLog) {
            this.sap = sap;
            this.log = log;
            this.va02 = va02;
            this.isRelease = isRelease;
            this.isLog = isLog;
        }

        public OrderStatus runRejections(RejectionsSapOrderProperty rejObj, string id, string tableName) {
            string csrNote = "";
            List<ReplacePartialCutProperty> rpcpList = new List<ReplacePartialCutProperty>();

            if (isLog) { startRejectionsLog(rejObj, id, tableName, isRelease); }

            OrderStatus status = va02.enterOrder(rejObj.orderNumber);

            if (status != OrderStatus.available) { return status; }

            string paymentTermsID = VA02ID.PAYMENT_TERMS_ID.Where(x => sap.idExists(x)).First();
            if ((sap.findById(paymentTermsID) as dynamic).text == "") {
                status = OrderStatus.orderMissingPaymentTerms;
                return status;
            }

            va02.bypassInitialPopups();

            ITable table = va02.getTable();

            va02.moveRejectionCodeColumnToIndexEight(table);

            csrNote = executeLineChanges(rejObj, id, tableName, csrNote, rpcpList, table);

            va02.soarAction(csrNote, "Line Rejections", rejObj.orderNumber);
            va02.save();

            return va02.getOrderStatusAfterSaving();
        }

        private string executeLineChanges(RejectionsSapOrderProperty rejObj, string id, string tableName, string csrNote, List<ReplacePartialCutProperty> rpcpList, ITable table) {
            foreach (var rejection in rejObj.lineDetails) {
                int sapLineNumber = (rejection.lineNumber / 10 - 1);
                bool shouldChange = isForChange(table, rejection, sapLineNumber);
                //TODO: Check if order qty can be changed prior to line split
                if (shouldChange) {
                    if (rejection.isReplacePartialCut) {

                        rpcpList.Add(getRPCProperty(table.getCellValue(sapLineNumber, "Un"), rejection));

                        rejection.reason = $"Split into rejected {rejection.orderedQty - rejection.confirmedQty} cases " +
                                           $"and confirmed {rejection.confirmedQty} cases to be sent for customer.";

                        changeQtyToConfirmedAmount(table, rejection, sapLineNumber);

                    } else {
                        table.setCellValue(sapLineNumber, VA02.rejectionCodeColumnIndex, rejection.rejectionCode);
                    }
                }

                sap.pressEnter();
                sap.getRidOfPopUps();

                csrNote += $"sku {rejection.sku} has{(shouldChange ? " " : " not ")}been rejected with {rejection.rejectionCode}. Reason: {rejection.reason}{Constants.vbCr}";

                if (isLog) { endLog(rejObj, id, tableName, rejection, shouldChange, isRelease); }
            }

            if (rpcpList.Count > 0) { populateCutSkus(rpcpList, table); }

            return csrNote;
        }

        private static ReplacePartialCutProperty getRPCProperty(string tableUnitsOfMeasure, RejectionsSapLineProperty rejection) {
            return new ReplacePartialCutProperty() {
                sku = rejection.sku,
                unitsOfMeasure = tableUnitsOfMeasure,
                rejCode = rejection.rejectionCode,
                cutQty = (rejection.orderedQty - rejection.confirmedQty)
            };
        }

        private static void changeQtyToConfirmedAmount(ITable table, RejectionsSapLineProperty rejection, int sapLineNumber) {
            if (table.getCellValue(sapLineNumber, "Un") == "MUN") {
                table.setCellValue(sapLineNumber, "Un", "MCS");
                table.setCellValue(sapLineNumber, "Order Quantity", rejection.confirmedQty);
                table.setCellValue(sapLineNumber, "Un", "MUN");
            } else {
                table.setCellValue(sapLineNumber, "Order Quantity", rejection.confirmedQty);
            }
        }

        private void endLog(RejectionsSapOrderProperty rejObj, string id, string tableName, RejectionsSapLineProperty rejection, bool shouldChange, bool isRelease) {
            log.update(
                        tableName: tableName,
                        columnNames: new[] { "status", "reason" },
                        values: new[] { shouldChange ? "success" : "fail", rejection.reason.Replace("'", "") },
                        conditionName: isRelease ? new[] { "[orderNumber]", "[id]", "[isDuringRelease]" } : new[] { "[orderNumber]", "[isDuringRelease]" },
                        conditionValue: isRelease ? new[] { rejObj.orderNumber.ToString(), id, "1" } : new[] { rejObj.orderNumber.ToString(), "0" },
                        customCondition: isRelease ? "" : "([status] IS NULL OR [status] = 'Fail: blocked by batch job' OR [status] = 'Fail: blocked by user')"
                        );
        }

        private void populateCutSkus(List<ReplacePartialCutProperty> replacePartialCutProperties, ITable table) {
            int emptyRowIndex = va02.getFirstEmptyRowIndex(table);

            foreach (var rpc in replacePartialCutProperties) {

                table.setCellValue(emptyRowIndex, "Material", rpc.sku);
                table.setCellValue(emptyRowIndex, "Order Quantity", rpc.cutQty);
                sap.pressEnter();
                sap.getRidOfPopUps();

                if (rpc.unitsOfMeasure == "MUN") {
                    table.setCellValue(emptyRowIndex, "Un", "MUN");
                    sap.pressEnter();
                    sap.getRidOfPopUps();
                }

                table.setCellValue(emptyRowIndex, VA02.rejectionCodeColumnIndex, rpc.rejCode);
                sap.pressEnter();

                emptyRowIndex++;
            }
        }

        private static bool isForChange(ITable table, RejectionsSapLineProperty rejection, int sapLineNumber) {
            if (!(table.getCell(sapLineNumber, VA02.rejectionCodeColumnIndex) as dynamic).Changeable) {
                rejection.reason = "RRC is not changeable.";
                return false;
            }

            if (int.Parse(table.getCellValue(sapLineNumber, "Material")) != rejection.sku) {
                rejection.reason = "RRC cannot be applied as the sku was changed.";
                return false;
            }

            return true;
        }

        private void startRejectionsLog(RejectionsSapOrderProperty rejObj, string id, string tableName, bool isRelease) {
            log.update(
                            tableName: tableName,
                            columnNames: new[] { "startTime" },
                            values: new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                            conditionName: isRelease ? new[] { "[orderNumber]", "[id]", "[isDuringRelease]" } : new[] { "[orderNumber]", "[isDuringRelease]" },
                            conditionValue: isRelease ? new[] { rejObj.orderNumber.ToString(), id, "1" } : new[] { rejObj.orderNumber.ToString(), "0" },
                            customCondition: isRelease ? "" : "([status] IS NULL OR [status] = 'Fail: blocked by batch job' OR [status] = 'Fail: blocked by user')"
                            );
        }
    }
}
