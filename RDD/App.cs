using IDAUtil.Support;
using lib;
using System;

namespace RDD {
    static class App {
        public static void Main(string[] args) {

            //Controller.executeRDDTask("ES01");

            string salesOrg = "es01";

            //IServerLogger log = Create.serverLogger(138);
            //log.start();

            try {
                Controller.executeRDDTask(salesOrg);
                //log.finish("success");
            } catch (Exception ex) {
                GlobalErrorHandler.handle(salesOrg, "RDD", ex);
                //log.finish("error");
            }

        }
    }
}