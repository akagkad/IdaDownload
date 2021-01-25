using lib;
using Microsoft.VisualBasic;
using System;

namespace IDAUtil.SAP.TCode.Runners {
    public class SwitchesVA02Runner {
        #region private properties
        private readonly IdaLog log;
        private readonly ISAPLib sap;
        private readonly IVA02 va02;
        #endregion
        public SwitchesVA02Runner(ISAPLib sap, IdaLog log, IVA02 va02) {
            this.sap = sap;
            this.log = log;
            this.va02 = va02;
        }

        public OrderStatus runSwitches(SwitchesSapOrderProperty switchObj, string id, string tableName) {
            string csrNote = "";

            startSwitchesLog(switchObj, id, tableName);

            OrderStatus status = va02.enterOrder(switchObj.orderNumber);

            if (status == OrderStatus.blockedByUser || status == OrderStatus.blockedByBatchJob) {
                return status;
            }

            va02.bypassInitialPopups();

            status = va02.isChangeNeeded();

            if (status != OrderStatus.available) {
                return status;
            }

            ITable table = va02.getTable();

            va02.moveRouteCodeColumnToIndexEight(table);

            foreach (var lineSwitch in switchObj.lineDetails) {
                int sapLineNumber = (lineSwitch.lineNumber / 10 - 1);
                bool shouldChange = isForChange(table, lineSwitch, sapLineNumber);
                bool isChanged = false;

                if (shouldChange) {
                    string unitOfMeasure = table.getCellValue(sapLineNumber, "Un");
                    string itemCategory = table.getCellValue(sapLineNumber, "ItCa");
                    string routeCode = table.getCellValue(sapLineNumber, VA02.routeCodeColumnIndex); // have to use by index because the table retains its indexes by name after reordering but not actual indexes within a table
                    string cmir = table.getCellValue(sapLineNumber, "Customer Material Numb");

                    table.setCellValue(sapLineNumber, "Material", lineSwitch.newSku);
                    sap.pressEnter();
                    sap.getRidOfPopUps();

                    string tempReason = "";
                    isChanged = va02.isLineChanged(table, lineSwitch, sapLineNumber, ref tempReason);

                    if (tempReason != "") lineSwitch.reason = tempReason;

                    if (sap.getInfoBarMsg().Contains("Z4")) {
                        return OrderStatus.bothSkusAreZ4;
                    }

                    if (sap.getInfoBarMsg().Contains("not approved")) {
                        return OrderStatus.bothSkusAreNotApproved;
                    }

                    unitsOfMeasureAction(table, sapLineNumber, unitOfMeasure);
                    itemCategoryAction(table, sapLineNumber, itemCategory);
                    routeCodeAction(table, sapLineNumber, routeCode);
                    cmirAction(switchObj, table, lineSwitch, sapLineNumber, cmir);
                }

                csrNote += $"sku {lineSwitch.oldSku} has{(isChanged ? " " : " not ")}been switched to {lineSwitch.newSku}. Reason: {lineSwitch.reason}{Constants.vbCr}";

                endSwtichesLog(switchObj, id, tableName, lineSwitch, isChanged);
            }

            va02.soarAction(csrNote, "Line Switches", switchObj.orderNumber);
            va02.save();
            return va02.getOrderStatusAfterSaving();
        }



        private void endSwtichesLog(SwitchesSapOrderProperty switchObj, string id, string tableName, SwitchesSapLineProperty lineSwitch, bool isChanged) {
            log.update(tableName,
                new[] { "status", "reason" },
                new[] { isChanged ? "success" : "fail", lineSwitch.reason.Replace("'","") },
                new[] { "[orderNumber]", "[id]", "[SwitchAutomatic]" },
                new[] { switchObj.orderNumber.ToString(), id, "1" });
        }

        private static bool isForChange(ITable table, SwitchesSapLineProperty lineSwitch, int sapLineNumber) {
            if (!(table.getCell(sapLineNumber, "Material") as dynamic).Changeable) {
                lineSwitch.reason = "Line is rejected";
                return false;
            }

            if (long.Parse(table.getCellValue(sapLineNumber, "Material")) == lineSwitch.newSku) {
                lineSwitch.reason = "Line was already switched";
                return false;
            }

            return true;
        }

        private void startSwitchesLog(SwitchesSapOrderProperty switchObj, string id, string tableName) {
            log.update(tableName,
                            new[] { "startTime" },
                            new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                            new[] { "[orderNumber]", "[id]", "[SwitchAutomatic]" },
                            new[] { switchObj.orderNumber.ToString(), id, "1" });
        }

        private void unitsOfMeasureAction(ITable table, int sapLineNumber, string unitOfMeasure) {
            if ((unitOfMeasure ?? "") != (table.getCellValue(sapLineNumber, "Un") ?? "")) {
                table.setCellValue(sapLineNumber, "Un", unitOfMeasure);
                sap.pressEnter();
                sap.getRidOfPopUps();
            }
        }

        private void itemCategoryAction(ITable table, int sapLineNumber, string itemCategory) {
            if ((itemCategory ?? "") != (table.getCellValue(sapLineNumber, "ItCa") ?? "")) {
                table.setCellValue(sapLineNumber, "ItCa", itemCategory);
                sap.pressEnter();
                sap.getRidOfPopUps();
            }
        }

        private void routeCodeAction(ITable table, int sapLineNumber, string routeCode) {
            if ((routeCode ?? "") != (table.getCellValue(sapLineNumber, VA02.routeCodeColumnIndex) ?? "")) {
                table.setCellValue(sapLineNumber, VA02.routeCodeColumnIndex, routeCode);
                sap.pressEnter();
                sap.getRidOfPopUps();
            }
        }

        private void cmirAction(SwitchesSapOrderProperty switchObj, ITable table, SwitchesSapLineProperty lineSwitch, int sapLineNumber, string cmir) {
            if (lineSwitch.isSameBarcode && cmir != "") {
                table.setCellValue(sapLineNumber, "Customer Material Numb", cmir);
                log.insert("CMIR",
                           "[salesOrg],[soldTo],[sku],[cmir]",
                           $@"'{switchObj.salesOrg}',{switchObj.soldTo},{lineSwitch.newSku},'{cmir}'");
            }
        }
    }
}
