using IDAUtil.Support;
using lib;
using System;

namespace CMIRReport {
    static class App {
        public static void Main(string[] args) {

            Controller.executeMissingCMIRReport("DE01");

            string salesOrg = args[0];

            var log = Create.serverLogger(140);
            log.start();

            try {
                Controller.executeMissingCMIRReport(salesOrg);
                log.finish("success");
            } catch (Exception ex) {
                GlobalErrorHandler.handle(salesOrg, "Missing CMIR Report", ex);
                log.finish("error");
            }
        }
    }
}