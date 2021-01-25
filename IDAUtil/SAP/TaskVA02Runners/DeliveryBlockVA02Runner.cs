using lib;
using Microsoft.VisualBasic;
using System;
using System.Linq;

namespace IDAUtil.SAP.TCode.Runners {
    public class DeliveryBlockVA02Runner {
        #region private properties
        private readonly IdaLog log;
        private readonly ISAPLib sap;
        private readonly IVA02 va02;
        #endregion

        public DeliveryBlockVA02Runner(ISAPLib sap, IdaLog log, IVA02 va02) {
            this.sap = sap;
            this.log = log;
            this.va02 = va02;
        }

        public void runDeliveryBlockChange(int orderNumber, string id, string changeReason, string delBlock, string tableName) {
            string csrNote;

            startDelBlockLog(orderNumber, id, tableName);

            OrderStatus status = va02.enterOrder(orderNumber);

            if (status != OrderStatus.available) {
                va02.updateLog(status, tableName, orderNumber.ToString(), id);
                return;
            }

            va02.bypassInitialPopups();

            string blockID = VA02ID.DEL_BLOCK_ID.Where(x => sap.idExists(x)).First();
            status = va02.isChangeNeeded();

            if (status != OrderStatus.available) {
                va02.updateLog(status, tableName, orderNumber.ToString(), id);
                return;
            }

            (sap.findById(blockID) as dynamic).Key = delBlock;

            if (delBlock == " ") { delBlock = "empty"; }
            csrNote = $"Delivery block of {orderNumber} has been changed to {delBlock}, Reason: {changeReason}";

            va02.soarAction(csrNote, "Delivery Block", orderNumber);
            
            va02.save();
            va02.updateOrderSavedLog(tableName, orderNumber, id);
        }

        private void startDelBlockLog(int orderNumber, string id, string tableName) {
            log.update(tableName,
                new[] { "startTime" },
                new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss") },
                new[] { "orderNumber", "id" },
                new[] { orderNumber.ToString(), id });
        }
    }
}
