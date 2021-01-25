using IDAUtil.Support;
using lib;
using System;

namespace DistressReport {
    static class App {
        public static void Main(string[] args) {
            string salesOrg = args[0];

            //string salesOrg = "FR01";

            IServerLogger log = Create.serverLogger(157);
            log.start();

            try {
                Controller.executeDistressReport(salesOrg);
                log.finish("success");
            } catch (Exception ex) {
                GlobalErrorHandler.handle(salesOrg, "Distress", ex);
                log.finish("error");
            }
        }
    }
}