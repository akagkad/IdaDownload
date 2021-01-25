using IDAUtil.Support;
using lib;
using System;

namespace Rejections {
    static class App {
        public static void Main(string[] args) {

            //TODO: Add ship to & sold to name for the email report

            //Controller.executeRejections("ZA01", true);

            string salesOrg = args[0];
            bool isRelease = (args[1] == "Release");

            IServerLogger log = Create.serverLogger(147);
            log.start();

            try {
                Controller.executeRejections(salesOrg, isRelease);
                log.finish("success");
            } catch (Exception ex) {
                GlobalErrorHandler.handle(salesOrg, isRelease ? "Release Rejections" : "After Release Rejections", ex);
                log.finish("error");
            }
        }
    }
}