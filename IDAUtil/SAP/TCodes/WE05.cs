using lib;
using Microsoft.VisualBasic;
using System;

namespace IDAUtil {
    public class WE05 {
        private readonly ISAPLib sapLib;
        private readonly IWinUtil winUtil;

        public WE05(ISAPLib sapLib, IWinUtil winUtil) {
            this.sapLib = sapLib;
            this.winUtil = winUtil;
        }

        public enum ResponseWE05 {
            noItems,
            successSingleValue,
            successTable
        }

        public ResponseWE05 extractReport(string messageVariant, string folderPath) {

            // 2 - inbound idocs only
            // 51 - Application not posted (Hardstop workflow)
            executeWE05(2.ToString(), 51.ToString(), messageVariant);
            string screenShotPath;
            string fileName;
            screenShotPath = folderPath + @"\" + DateAndTime.Now.ToFileNameFormat() + " WE05.jpg";
            fileName = DateAndTime.Now.ToFileNameFormat() + " WE05.xlsx";
            
            if (sapLib.isPopUp()) {
                sapLib.printScreenOfCurrentSession(screenShotPath);
                sapLib.pressEnter();
                return ResponseWE05.noItems;
            }

            if (isTableExists()) {
                sapLib.openExport();
                sapLib.exportExcel(folderPath, fileName);
                sapLib.printScreenOfCurrentSession(screenShotPath);
                return ResponseWE05.successTable;
            } else {
                screenShotPath = folderPath + @"\" + DateAndTime.Now.ToFileNameFormat() + " single item only found WE05.jpg";
                try {
                    sapLib.printScreenOfCurrentSession(screenShotPath);
                } catch (Exception) {
                    System.Threading.Thread.Sleep(3000);
                    sapLib.printScreenOfCurrentSession(screenShotPath);
                }

                return ResponseWE05.successSingleValue;
            }
        }

        private void executeWE05(string direction, string status, string messageVariant) {
            string dateFrom = DateAndTime.DateAdd(DateInterval.Day, -90, DateAndTime.Now).ToSAPFormat();
            string dateTo = DateAndTime.Now.ToSAPFormat();
            sapLib.enterTCode("WE05");
            sapLib.setText(lib.SAP_ALL_ID.WE05.MESSAGE_VARIAN_FLD, messageVariant);
            if ((messageVariant ?? "") == "FR") {
                sapLib.setMultipleSelection(new[] { "BE" }, lib.SAP_ALL_ID.WE05.MESSAGE_VARIANT_MULTIPLE_SELECTION_BTN);
            }

            sapLib.setText(lib.SAP_ALL_ID.WE05.CREATED_ON_FROM_FLD, dateFrom);
            sapLib.setText(lib.SAP_ALL_ID.WE05.CREATED_ON_TO_FLD, dateTo);

            sapLib.setText(lib.SAP_ALL_ID.WE05.DIRECTION_FLD, direction);
            sapLib.setText(lib.SAP_ALL_ID.WE05.CURRENT_STATUS_FLD, status);

            //filter table by orders only
            sapLib.setText("wnd[0]/usr/tabsTABSTRIP_IDOCTABBL/tabpSOS_TAB/ssub%_SUBSCREEN_IDOCTABBL:RSEIDOC2:1100/ctxtIDOCTP-LOW", "ORDERS04");

            sapLib.pressF8();
        }

        private bool setMessageTypeFilter(string msgType) {
            (sapLib.findById(SAP_ID.OPEN_EXPORT_CONTEXT_MENU_BTN_ARR[0]) as dynamic).selectColumn("MESTYP"); // Message Type
            (sapLib.findById(SAP_ID.OPEN_EXPORT_CONTEXT_MENU_BTN_ARR[0]) as dynamic).pressToolbarButton("&MB_FILTER");
            sapLib.setText(SAP_ID.TABLE_FILTER_FIRST_VALUE_FLD, msgType);
            sapLib.pressEnter();
            return true;
        }

        private bool isTableExists() {
            return sapLib.idExists(lib.SAP_ALL_ID.WE05.SELECTED_IDOCS_TABLE);
        }

        private bool isTableEmpty() {
            return (sapLib.findById(lib.SAP_ALL_ID.WE05.SELECTED_IDOCS_TABLE) as dynamic).RowCount == 0;
        }
    }
}