using lib;
using Microsoft.VisualBasic;
using System;

namespace IDAUtil.SAP.TCode.Runners {
    public class RDDVA02Runner {
        private readonly IdaLog log;
        private readonly ISAPLib sap;
        private readonly IVA02 va02;

        public RDDVA02Runner(ISAPLib sap, IdaLog log, IVA02 va02) {
            this.sap = sap;
            this.log = log;
            this.va02 = va02;
        }

        public void runRDDChange(int orderNumber, DateTime oldRdd, DateTime newRdd, string id, string changeReason, string tableName) {
            string sapNewRdd = $"{newRdd.Day}.{newRdd.Month}.{newRdd.Year}";
            string sapOldRdd = $"{oldRdd.Day}.{oldRdd.Month}.{oldRdd.Year}";
            string csrNote = "";

            startRDDLog(orderNumber, id, tableName);

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

            va02.tryToSellectAllLines();
            sap.select(VA02ID.CHANGE_RDD_BTN_ID);
            sap.setText(VA02ID.CHANGE_RDD_TEXT_FIELD, sapNewRdd);
            sap.pressBtn(VA02ID.PRESS_OK_ON_RDD_CHANGE_FIELD_BTN_ID);
            sap.getRidOfPopUps();
            sap.pressEnter();
            sap.getRidOfPopUps();

            csrNote = $"RDD of {orderNumber} has been changed to {sapNewRdd} from {sapOldRdd}, Reason: {changeReason}";

            while (sap.isPopUp() || sap.idExists(VA02ID.SECOND_POPUP_WINDOW_ID)) {
                sap.pressEnter();
            }

            va02.soarAction(csrNote, "RDD", orderNumber);
            va02.save();
            va02.updateOrderSavedLog(tableName, orderNumber, id);
        }
        private void startRDDLog(int orderNumber, string id, string tableName) {
            log.update(tableName,
                new[] { "startTime" },
                new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                new[] { "orderNumber", "id" },
                new[] { orderNumber.ToString(), id });
        }
    }
}
