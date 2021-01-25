using IDAUtil;
using IDAUtil.Support;
using lib;
using Microsoft.VisualBasic;
using System;

namespace SOAR {

    public static class App {
        /// <summary>
        /// SOAR script runs reports as per args that are passed to save in the SOAR folder
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {
            IServerLogger log = Create.serverLogger(154);

            string soarReport = args[0];
            //string soarReport = "WE05";

            var SOs = new string[] { "FR01", "NL01", "DE01", "IT01", "GR01", "PL01", "CZ01", "PT01", "ES01", "RO01", "GB01", "TR01", "UA01", "RU01", "ZA01", "KE02", "NG01" };
            
            log.start();

            switch (soarReport) {
                case "WE05": {
                        Controller.executeWE05(Strings.Left(args[1], 2));
                        log.finish();
                        break;
                    }

                case "ZV04HN": {
                        Controller.executeZV04HN(args[1]);
                        log.finish();
                        break;
                    }

                case "All": {
                        runAll(SOs);
                        log.finish();
                        break;
                    }

                default: {
                        throw new NotImplementedException("no report for " + soarReport);
                    }
            }
        }

        private static void runAll(string[] SOs) {
            if (DateAndTime.Weekday(DateTime.Today, FirstDayOfWeek.Monday) != 7) {
                foreach (var so in SOs) {
                    try {
                        Controller.executeWE05(Strings.Left(so, 2));
                        Controller.executeZV04HN(so);
                    } catch (Exception ex) {
                        GlobalErrorHandler.handle(so, "SOAR", ex);
                    }
                }
            }

            try {
                Controller.executeZV04HN("EG01");
                Controller.executeZV04HN("EG02");
                Controller.executeZV04HN("SA01");
            } catch (Exception ex) {
                GlobalErrorHandler.handle("EGandSA", "SOAR", ex);
            }
        }
    }
}