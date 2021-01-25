using lib;
using Microsoft.VisualBasic;
using SAPFEWSELib;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace IDAUtil.SAP.TCode {
    public enum OrderStatus {
        available = 0,
        blockedByUser = 1,
        blockedByBatchJob = 2,
        realeasedOrRejected = 4,
        bothSkusAreZ4 = 5,
        success = 6,
        failedToSave = 7,
        bothSkusAreNotApproved = 8,
        orderMissingPaymentTerms = 9
    }

    public class VA02 : IVA02 {
        #region properties
        private readonly IIdaLog log;
        private readonly ISAPLib sap;
        public const int routeCodeColumnIndex = 8;
        public const int rejectionCodeColumnIndex = 8;
        #endregion

        public VA02(ISAPLib sap, IIdaLog log) {
            this.sap = sap;
            this.log = log;
        }

        public OrderStatus getOrderStatusAfterSaving() {
            if (sap.getInfoBarMsg().Contains("saved")) {
                return OrderStatus.success;
            } else {
                for (int i = 0; i <= 5; i++) {
                    System.Threading.Thread.Sleep(2000);
                    save();
                    if (sap.getInfoBarMsg().Contains("saved")) { return OrderStatus.success; }
                }
                return OrderStatus.failedToSave;
            }
        }

        public void soarAction(string csrNote, string taskName, int orderNumber) {
            if (sap.idExists(VA02ID.ORDER_TOOLBOX_BUTTON_ID)) {
                uploadNoteToOrder(csrNote, taskName);
            } else {
                tryToOpenCSRNotes();
                setCSRNotes(csrNote);
            }
        }

        private void uploadNoteToOrder(string note, string taskName) {
            var name = $"{taskName} at {Strings.Format(DateTime.Now, "HH:mm:ss")}";

            string toolBoxButton = @"%GOS_TOOLBOX";
            string addAttachmentButton = @"CREATE_ATTA";
            string createNoteButton = @"NOTE_CREA";

            (sap.findById(VA02ID.ORDER_TOOLBOX_BUTTON_ID) as dynamic).pressButton(toolBoxButton);
            (sap.findById(VA02ID.ORDER_TOOLBOX_MENU_ID) as dynamic).PressContextButton(addAttachmentButton);

            //cannot dynamically find when to press button because the menu freezes the button press
            System.Threading.Thread.Sleep(5000);
            (sap.findById(VA02ID.ORDER_TOOLBOX_MENU_ID) as dynamic).selectContextMenuItem(createNoteButton);
            
            sap.setText(VA02ID.ORDER_NOTE_UPLOAD_DESCRIPTION_TEXTFIELD_ID, name);
            sap.setText(VA02ID.ORDER_NOTE_UPLOAD_BODY_TEXTFIELD_ID, note);

            sap.pressEnter();
        }

        public void bypassInitialPopups() {
            if (sap.getInfoBarMsg().Contains("exists with same PO")) { sap.pressEnter(); }

            sap.getRidOfPopUps();
        }

        public OrderStatus enterOrder(int order) {
            OrderStatus status;
            sap.enterTCode("VA02");
            sap.setText(VA02ID.ORDER_NUNBER_INPUT_TEXT_FIELD_ID, order.ToString());
            sap.pressEnter();
            sap.getRidOfPopUps();
            if (sap.getInfoBarMsg().Contains("exists with same")) { sap.pressEnter(); }

            status = checkIfOrderBlocked(order);
            return status;
        }

        public bool isLineChanged(ITable table, SwitchesSapLineProperty lineSwitch, int sapLineNumber, ref string reason) {
            switch (true) {
                case object _ when sap.getInfoBarMsg().Contains("Future"): {
                        reason = "SKU is future status";
                        putBackPreviousSku(table, lineSwitch, sapLineNumber);
                        return false;
                    }

                case object _ when sap.getInfoBarMsg().Contains("excluded"): {
                        reason = "SKU is excluded";
                        putBackPreviousSku(table, lineSwitch, sapLineNumber);
                        return false;
                    }

                case object _ when sap.getInfoBarMsg().Contains("not defined"): {
                        reason = "SKU is not defined for the sales organisation";
                        putBackPreviousSku(table, lineSwitch, sapLineNumber);
                        return false;
                    }

                case object _ when sap.getInfoBarMsg().Contains("not approved"): {
                        reason = "SKU is not approved for the sales organisation";
                        sap.pressEnter();
                        sap.getRidOfPopUps();
                        putBackPreviousSku(table, lineSwitch, sapLineNumber);
                        return false;
                    }

                default: {
                        return true;
                    }
            }
        }

        public void putBackPreviousSku(ITable table, SwitchesSapLineProperty lineSwitch, int sapLineNumber) {
            table.setCellValue(sapLineNumber, table.columnDict["Material"].First(), lineSwitch.oldSku);
            sap.pressEnter();
            sap.getRidOfPopUps();
        }

        public string getChangedListForCSR(QtyConversionOrderProperty order) {
            string text = "";
            for (int i = 0, loopTo = order.documentLineChangeList.Count - 1; i <= loopTo; i++)
                text += $"sku: {order.documentLineChangeList[i].material}, old qty:{order.documentLineList[i].quantity}, new qty:{order.documentLineChangeList[i].quantity};" + Constants.vbCr;
            return text;
        }

        public void save() {
            sap.pressSave();
            sap.getRidOfPopUps();
            if (sap.idExists(lib.SAP_ALL_ID.VA02.INCOMPLETE_DOCUMENT_MSGBOX)) { sap.pressBtn(lib.SAP_ALL_ID.VA02.INCOMPLETE_DOCUMENT_OK_BTN); }
            sap.getRidOfPopUps();
        }

        public void updateOrderSavedLog(string tableName, int orderNumber, string id) {
            if (sap.getInfoBarMsg().Contains("saved")) {
                log.update(tableName,
                    new[] { "endTime", "status" },
                    new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), "success" },
                    new[] { "orderNumber", "id" },
                    new[] { orderNumber.ToString(), id });
            } else {
                log.update(tableName,
                    new[] { "endTime", "status" },
                    new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), "Fail: could not save order" },
                    new[] { "orderNumber", "id" },
                    new[] { orderNumber.ToString(), id });
            }
        }

        public void setCSRNotes(string csr) {
            // when csr notes are greyed out need to double click them, save them and come out to populate them
            string oldText = sap.getText(VA02ID.CSR_FIELD_ID);

            if (!(sap.findById(VA02ID.CSR_FIELD_ID) as dynamic).Changeable) {
                (sap.findById(VA02ID.PRESS_DELETE_CSR_NOTES_BTN_ID) as dynamic).Press();
            }

            sap.setText(VA02ID.CSR_FIELD_ID, "IDA change! " + csr + Constants.vbCr + oldText);
        }

        public void updateLog(OrderStatus status, string tableName, string orderNumber, string id) {
            string outcome = "";
            switch (status) {
                case OrderStatus.blockedByBatchJob: {
                        outcome = "Fail: blocked by batch job";
                        break;
                    }

                case OrderStatus.blockedByUser: {
                        outcome = "Fail: blocked by user";
                        break;
                    }

                case OrderStatus.realeasedOrRejected: {
                        outcome = "Fail: order released or rejected";
                        break;
                    }
                case OrderStatus.orderMissingPaymentTerms: {
                        outcome = "Fail: order missing payment terms";
                        break;
                    }

                default: {
                        throw new NotImplementedException();
                    }
            }

            log.update(tableName,
                new[] { "endTime", "status" },
                new[] { Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss"), outcome },
                new[] { "orderNumber", "id" },
                new[] { orderNumber, id });
        }

        public OrderStatus isChangeNeeded() {
            var timeout = DateAndTime.DateAdd(DateInterval.Second, 10, DateAndTime.Now);

            string blockID = VA02ID.DEL_BLOCK_ID.Where(x => sap.idExists(x)).First();
            string paymentTermsID = VA02ID.PAYMENT_TERMS_ID.Where(x => sap.idExists(x)).First();

            //default as available just in case code will not be able to get the real status
            OrderStatus tempStatus = OrderStatus.available;

            while (DateTime.Now < timeout) {

                System.Threading.Thread.Sleep(2000);

                try {
                    if (!(sap.findById(blockID) as dynamic).Changeable) {
                        tempStatus = OrderStatus.realeasedOrRejected;
                        break;
                    } else if ((sap.findById(paymentTermsID) as dynamic).text == "") {
                        tempStatus = OrderStatus.orderMissingPaymentTerms;
                        break;
                    } else {
                        tempStatus = OrderStatus.available;
                        break;
                    }
                } catch (Exception) {
                }
            }
            return tempStatus;
        }

        public void tryToSellectAllLines() {
            var timeout = DateAndTime.DateAdd(DateInterval.Second, 10, DateAndTime.Now);
            GuiTableControl myTable;
            GuiTableRow firstRow;
            while (DateTime.Now < timeout) {
                // buggfix com objects late binding
                System.Threading.Thread.Sleep(2000);

                // buggfix - The data necessary to complete this operation is not yet available.
                try {
                    myTable = (GuiTableControl)sap.findById(VA02ID.FIRST_ROW_OF_SALES_TAB_TABLE_ID.Where(x => sap.idExists(x)).First());
                    firstRow = (GuiTableRow)myTable.Rows.Item(1);
                    if (!firstRow.Selected) {
                        sap.pressBtn(VA02ID.SELECT_ALL_BTN_ID.Where(x => sap.idExists(x)).First());
                    } else {
                        break;
                    }
                } catch (Exception) {
                }
            }
        }

        public void tryToOpenCSRNotes() {
            var timeout = DateAndTime.DateAdd(DateInterval.Second, 10, DateAndTime.Now);

            // buggfix - The data necessary to complete this operation is not yet available.
            while (DateTime.Now < timeout) {
                System.Threading.Thread.Sleep(2000);
                try {
                    if (!sap.idExists(VA02ID.CSR_FIELD_ID)) {
                        sap.select(VA02ID.PRESS_OPEN_NOTES_BTN_ID);
                    } else {
                        break;
                    }
                } catch (Exception) {
                }
            }
        }

        public ITable getTable() {
            ITable table;
            // sometimes table is not found when sap is slow
            try {
                table = sap.getITableObject();
            } catch (Exception) {
                System.Threading.Thread.Sleep(1000);
                table = sap.getITableObject();
            }

            return table;
        }

        public int getFirstEmptyRowIndex(ITable table) {
            int i = 0;

            while ((table.getCell(i, 1) as dynamic).Text != "") {
                i++;
            }

            return i;
        }

        // move rejection column to visible location due to buggy GUITableControl
        public void moveRejectionCodeColumnToIndexEight(ITable table) {
            (sap.findById(table.id) as dynamic).ReorderTable("0 1 2 3 4 5 6 7 24 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 66 67 68 69 70 71 72 73 74 75 76 77 78 79 80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95 96 97 98 99 100 101 102");
        }

        public void moveRouteCodeColumnToIndexEight(ITable table) {
            (sap.findById(table.id) as dynamic).ReorderTable("0 1 2 3 4 5 6 7 66 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 42 43 44 45 46 47 48 49 50 51 52 53 54 55 56 57 58 59 60 61 62 63 64 65 67 68 69 70 71 72 73 74 75 76 77 78 79 80 81 82 83 84 85 86 87 88 89 90 91 92 93 94 95 96 97 98 99 100 101 102");
        }

        public OrderStatus checkIfOrderBlocked(int orderNumber) {
            bool isOrderBlocked = sap.getInfoBarMsg().Contains("is currently being processed");
            if (isOrderBlocked) {
                MailUtil mu = (MailUtil)Create.mailUtil();
                string guid = Strings.Replace(Strings.Right(sap.getInfoBarMsg(), 8), ")", "");
                bool validGuid = Regex.Match(guid, @"[A-Z]\d{6}").Success;
                if (validGuid) {
                    mu.mailSimple(guid, $"Please leave {orderNumber}", $"Automated IDA reply.{Constants.vbCr}You have 60 seconds to leave the order {orderNumber}");
                    var timeout = DateAndTime.DateAdd(DateInterval.Second, 60, DateAndTime.Now);
                    while (DateAndTime.Now < timeout) {
                        if (sap.getInfoBarMsg().Contains("is currently being processed")) {
                            sap.pressEnter();
                            System.Threading.Thread.Sleep(10000); // that's 10 seconds Daniel...
                        } else {
                            return OrderStatus.available;
                        }
                    }
                } else {
                    return OrderStatus.blockedByBatchJob;
                }

                return OrderStatus.blockedByUser;
            } else {
                return OrderStatus.available;
            }
        }
    }
}