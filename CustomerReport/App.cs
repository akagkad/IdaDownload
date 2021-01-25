using IDAUtil;
using IDAUtil.Support;
using lib;
using System;

namespace CustomerReport {
    static class App {
        public static void Main(string[] args) {
            string salesOrg = args[0];
            //string salesOrg = "RU01";

            var log = Create.serverLogger(139);
            log.start();

            try {
                Controller.executeCustomerMissingReport(salesOrg);
                log.finish("success");
            } catch (Exception ex) {
                GlobalErrorHandler.handle(salesOrg, "Missing Customers Report", ex);
                log.finish("error");
            }
        }
    }
}