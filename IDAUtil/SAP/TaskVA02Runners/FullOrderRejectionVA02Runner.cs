using IDAUtil.SAP.TCode;
using lib;
using Microsoft.VisualBasic;
using System;
using System.Linq;

namespace IDAUtil.SAP.TaskVA02Runners {
    public class FullOrderRejectionVA02Runner {
        #region private properties
        private readonly IdaLog log;
        private readonly ISAPLib sap;
        private readonly IVA02 va02;
        #endregion

        public FullOrderRejectionVA02Runner(ISAPLib sap, IdaLog log, IVA02 va02) {
            this.sap = sap;
            this.log = log;
            this.va02 = va02;
        }

        public void runFullOrderRejection(int orderNumber, string id, string changeReason, string rejReason, string tableName) {
            string csrNote;
            startFullOrderRejectionLog(orderNumber, id, tableName);

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

            string blockID = VA02ID.DEL_BLOCK_ID.Where(x => sap.idExists(x)).First();
            (sap.findById(blockID) as dynamic).Key = " ";

            sap.pressBtn(VA02ID.PRESS_REJECT_ORDER_BTN_ID);
            (sap.findById(VA02ID.REJECT_ORDER_FIELD_ID) as dynamic).Key = rejReason;
            sap.pressBtn(VA02ID.PRESS_CONFIRM_REJECTING_ORDER_BTN_ID);

            csrNote = $"Order has been rejected with {rejReason}, Reason: {changeReason}";

            va02.soarAction(csrNote, "Order Rejection", orderNumber);
            va02.save();
            va02.updateOrderSavedLog(tableName, orderNumber, id);
        }

        private void startFullOrderRejectionLog(int orderNumber, string id, string tableName) {
            log.update(tableName,
                new[] { "startTime" },
                new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                new[] { "orderNumber", "id" },
                new[] { orderNumber.ToString(), id });
        }
    }
}
