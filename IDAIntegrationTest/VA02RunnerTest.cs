using System;
using System.Collections.Generic;
using IDAUtil;
using IDAUtil.SAP.TCode.Runners;
using lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Switches;

namespace IDAIntegrationTest {
    [TestClass()]
    public class VA02RunnerTest {
        [TestMethod]
        public void runSwitches_Should_NotFail_When_ConnectedToMultiSapSessions() {
            var executor = new SwitchesTaskExecutor("", "");
            var list = getSwitchesSapOrderList();
            var timer = Create.timeCount();
            timer.start();
            executor.automaticSwitchObjectList = list;
            executor.runSwitchesInVA02();
            double vremya = timer.finish();
            var mu = Create.mailUtil();
            // executor.sendReplacedCMIRs("Some Sales Org", "drybak@scj.com", mu)
        }

        private static List<SwitchesSapOrderProperty> getSwitchesSapOrderList() {
            var list = new List<SwitchesSapOrderProperty>() {
                new SwitchesSapOrderProperty() {
                    orderNumber = 206523629,
                    lineDetails = new List<SwitchesSapLineProperty>() {
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 10,
                            oldSku = 683471,
                            newSku = 671832,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 20,
                            oldSku = 696759,
                            newSku = 302530,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 30,
                            oldSku = 659079,
                            newSku = 301994,
                            reason = "huison"
                        }
                    }
                },
                new SwitchesSapOrderProperty() {
                    orderNumber = 206523627,
                    lineDetails = new List<SwitchesSapLineProperty>() {
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 10,
                            oldSku = 305990,
                            newSku = 313382,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 20,
                            oldSku = 305991,
                            newSku = 313243,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 30,
                            oldSku = 305993,
                            newSku = 313240,
                            reason = "huison"
                        }
                    }
                },
                new SwitchesSapOrderProperty() {
                    orderNumber = 206523628,
                    lineDetails = new List<SwitchesSapLineProperty>() {
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 10,
                            oldSku = 661755,
                            newSku = 313244,
                            reason = "huison"
                        }
                    }
                },
                new SwitchesSapOrderProperty() {
                    orderNumber = 206523631,
                    lineDetails = new List<SwitchesSapLineProperty>() {
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 10,
                            oldSku = 661755,
                            newSku = 313244,
                            reason = "huison"
                        }
                    }
                },
                new SwitchesSapOrderProperty() {
                    orderNumber = 206523630,
                    lineDetails = new List<SwitchesSapLineProperty>() {
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 50,
                            oldSku = 659079,
                            newSku = 301994,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 120,
                            oldSku = 673684,
                            newSku = 301397,
                            reason = "huison"
                        }
                    }
                },
                new SwitchesSapOrderProperty() {
                    orderNumber = 206523632,
                    lineDetails = new List<SwitchesSapLineProperty>() {
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 10,
                            oldSku = 675778,
                            newSku = 655402,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 120,
                            oldSku = 675778,
                            newSku = 655402,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 300,
                            oldSku = 675778,
                            newSku = 655402,
                            reason = "huison"
                        },
                        new SwitchesSapLineProperty() {
                            isSameBarcode = true,
                            lineNumber = 420,
                            oldSku = 675778,
                            newSku = 655402,
                            reason = "huison"
                        }
                    }
                }
            };
            return list;
        }

        [TestMethod]
        public void runRddChanges_Should_ChangeRoutes_When_ConnectedToSap() {
            var sap = Create.sapLib();
            IdaLog idaLog = new IdaLog();
            VA02 va02 = new VA02(sap, idaLog);
            RDDVA02Runner va02Runner = new RDDVA02Runner(sap, idaLog, va02);

            // tCode.runRDDChange(206522986, #03/04/2020#, #03/03/2020#, "XUY", "IDA Test: testing greyed out csr entering", "DeliveryDatesLog")
            va02Runner.runRDDChange(206522986, DateTime.Parse("2020-04-03"), DateTime.Parse("2020-03-03"), "XUY", "IDA Test: testing greyed out csr entering", "DeliveryDatesLog");
        }

        [TestMethod]
        public void runRouteCodeChanges_Should_ChangeRoutes_When_ConnectedToSap() {
            var sap = Create.sapLib();
            // Dim tCode As New SwitchesVA02Runner(sap, Log)

            // tCode.runRouteCodeChange(206522985, "XUY", "IDA Test: testing greyed out csr entering", "ZA0001", "DeliveryDatesLog")

        }

        [TestMethod]
        public void runDeliveryBlocksChanges_Should_ChangeRoutes_When_ConnectedToSap() {
            var sap = Create.sapLib();
            // Dim tCode As New VA02(sap)

            // tCode.runDeliveryBlockChange(206522985, "XUY", "XYUSON", "Z8", "DeliveryDatesLog")
        }

        [TestMethod]
        public void runQuantityChange_Should_ChangeQTYs_When_ConnectedToSap() {
            var sap = Create.sapLib();
            // Dim tCode As New VA02(sap)

            // tCode.runQuantityChange(getBaseSalesDocumentList.First)
        }

        public static List<QtyConversionOrderProperty> getBaseSalesDocumentList() {
            return new List<QtyConversionOrderProperty>() {
                new QtyConversionOrderProperty() {
                    orderNumber = 206522823,
                    shipTo = 56514,
                    documentLineList = new List<DocumentLine>() { new DocumentLine() { item = 10, material = 173240, quantity = 1 } },
                    documentLineChangeList = new List<DocumentLine>() { new DocumentLine() { item = 10, material = 173240, quantity = 4 } }
                }
            };
        }
    }
}