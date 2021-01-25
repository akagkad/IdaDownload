using IDAUtil;
using lib;
using Microsoft.VisualBasic;

namespace SOAR {
    public static class Controller {
        private static ISAPLib sapLib = Create.sapLib();
        /// <summary>
        /// WE05 runs for each message variant for all apart from FR01 where it takes 2 message variants - FR and BE
        /// the execution is filtered by inbound documents only and status 51
        /// The 3 possible outcomes are: 
        /// 1)SAP execution window has items and the program saves it in the excel file + screenshot of the sap window in jpeg format
        /// 2) sap execution has no items it will save the screenshot that nothing was found
        /// 3) sap execution has only one item and shows as a single view item and the screenshot is saved
        /// </summary>
        /// <param name="messageVariant"></param>
        public static void executeWE05(string messageVariant) {
            string folderPath = $@"\\Gbfrimpf000\common\SOAR\OTD\DOMESTIC SOAR\{messageVariant}{((messageVariant ?? "") == "KE" ? "02" : "01")}\WE05";
            IWinUtil winUtil = Create.winUtil();
            WE05 we05 = new WE05(sapLib, winUtil);
            var respone = we05.extractReport(messageVariant, folderPath);
            if (respone == WE05.ResponseWE05.successTable) {
                using (var xlUtil = Create.xlUtil()) {
                    xlUtil.closeWBLikeAnyInstanceWaitTillClose("*WE05.xlsx");
                }
            }
        }

        /// <summary>
        /// ZV04HN runs using the sales organisation data and with default document dates: (from: (3 months before today),  to: (today))
        /// execution is then saved in an excel file
        /// </summary>
        /// <param name="salesOrg"></param>
        public static void executeZV04HN(string salesOrg) {
            string folderPath = $@"\\Gbfrimpf000\common\SOAR\OTD\DOMESTIC SOAR\{salesOrg}\ZV04HN";
            string fileName = DateAndTime.Now.ToFileNameFormat() + " ZV04HN.xlsx";
            var zv04hn = new ZV04HN(sapLib, salesOrg, IDAEnum.Task.SOAR);
            zv04hn.setParamsBeforeExecution();
            zv04hn.exportExcel(folderPath, fileName);
            using (var xlUtil = Create.xlUtil()) {
                xlUtil.closeWBAnyInstanceWaitTillClose(fileName);
            }
        }
    }
}