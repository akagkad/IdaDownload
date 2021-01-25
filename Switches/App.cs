using IDAUtil.Support;
using lib;
using System;

namespace Switches {
    static class App {
        public static void Main(string[] args) {

            Controller.executeSwitchTask("RU01");

            string salesOrg = args[0];

           // IServerLogger log = Create.serverLogger(144);
           // log.start();

            try {
                Controller.executeSwitchTask(salesOrg);
               // log.finish("success");
            } catch (Exception ex) {
             //   GlobalErrorHandler.handle(salesOrg, "Switches", ex);
                //log.finish("error");
            }
        }
    }
}