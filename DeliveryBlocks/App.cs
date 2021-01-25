using IDAUtil;
using IDAUtil.Support;
using lib;
using System;

namespace DeliveryBlocks {
    class App {
        static void Main(string[] args) {

            //Controller.executeDeliveryBlocks("ES01");

            string salesOrg = args[0];

            IServerLogger log = Create.serverLogger(161);
            log.start(salesOrg);

            try {
                Controller.executeDeliveryBlocks(salesOrg);
                log.finish(salesOrg);
            } catch (Exception ex) {
                GlobalErrorHandler.handle(salesOrg, "Delivery Blocks", ex);
                log.finish(salesOrg + " Error");
            }

        }
    }
}
