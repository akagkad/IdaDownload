using lib;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
using System.Linq;

namespace IDAUtil.SAP.TCode.Runners {
    public class QuantityConversionVA02Runner {
        #region private properties
        private readonly IdaLog log;
        private readonly ISAPLib sap;
        private readonly IVA02 va02;
        #endregion

        public QuantityConversionVA02Runner(ISAPLib sap, IdaLog log, IVA02 va02) {
            this.sap = sap;
            this.log = log;
            this.va02 = va02;
        }

        public bool runQuantityChange(QtyConversionOrderProperty order) {
            sap.enterTCode("VA02");
            sap.setText(lib.SAP_ALL_ID.VA02.ORDER_FLD, order.orderNumber.ToString());
            sap.pressEnter();
            va02.checkIfOrderBlocked(order.orderNumber);
            sap.pressEnter();
            sap.getRidOfPopUps();
            if (sap.getInfoBarMsg().Contains("exists with same PO"))
                sap.pressEnter();
            sap.getRidOfPopUps();
            var table = sap.getITableObject();
            if (changeQuantityInTable(table, order)) {
                string csrNote = $"Order number {order.orderNumber} had qty's changed in the following - {va02.getChangedListForCSR(order)}";
                va02.soarAction(csrNote, "Quantity Conversion", order.orderNumber);
                va02.save();
                if (sap.getInfoBarMsg().Contains($"{order.orderNumber} has been saved")) {
                    order.isSaved = true;
                    var itemArr = order.documentLineChangeList.Select(x => x.item).ToArray();
                    log.update("QuantityConversionLog",
                        new[] { "isSaved" },
                        new[] { "1" },
                        new[] { "orderNumber" },
                        new[] { order.orderNumber.ToString() },
                        $"item IN ({string.Join(",", itemArr)})");
                    return true;
                }
            }

            return false;
        }
        private bool changeQuantityInTable(ITable table, QtyConversionOrderProperty order) {
            for (int i = 0, loopTo = order.documentLineChangeList.Count - 1; i <= loopTo; i++) {
                long item = order.documentLineChangeList[i].item;
                int row = (int)(item / (double)10 - 1);
                double newQuantity = order.documentLineChangeList[i].quantity;
                long material = order.documentLineChangeList[i].material;
                try {
                    order.start(i);
                    log.insert("QuantityConversionLog", "orderNumber, shipTo, item, material, oldQty, newQty, startTime", $"{order.orderNumber}, {order.shipTo},{order.documentLineChangeList[i].item},{order.documentLineChangeList[i].material},{order.documentLineList[i].quantity},{order.documentLineChangeList[i].quantity},'{order.documentLineChangeList[i].startTime}'");
                    if (int.Parse(table.getCellValue(row, "Material")) == material) {
                        if (table.setCellValue(row, "Order Quantity", newQuantity)) {
                            order.documentLineChangeList[i].actionSuccess();
                            sap.pressEnter();
                            sap.getRidOfPopUps();
                        } else {
                            order.documentLineChangeList[i].actionFailed($"Failed to set value");
                        }
                    } else {
                        order.documentLineChangeList[i].actionFailed($"Expected: {material} Actual: {table.getCellValue(row, "Material")}");
                    }
                } catch (Exception ex) {
                    order.documentLineChangeList[i].actionFailed($"Error occured: {ex.Message}");
                    Debug.WriteLine(ex.Source + Constants.vbCr + ex.StackTrace);
                    return false;
                } finally {
                    order.finish(i);
                    log.update("QuantityConversionLog",
                        new[] { "endTime", "status", "isConverted" },
                        new[] { order.documentLineChangeList[i].endTime, order.documentLineChangeList[i].status, order.documentLineChangeList[i].isChanged ? "1" : "0" },
                        new[] { "orderNumber", "item" },
                        new[] { order.orderNumber.ToString(), item.ToString() });
                }
            }

            return true;
        }
    }
}
