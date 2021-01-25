using IDAUtil;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rejections;
using Rejections.Service;
using System.Collections.Generic;
using lib;
using System;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Service;
using System.Linq;

namespace IDAUnitTest.Task {
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RejectionsTest {

        #region onlyAllRejections
        [TestMethod]
        public void rejDataCalculatorWithOnlyAllRejections_Should_ReturnListOfRejectionsForAllSkusThatHaveRejectionReasonInRejectionsData_When_normalState() {

            IDataCollectorServer dcServer = null;
            IDataCollectorSap dcSap = null;

            IDataCollectorServiceRejections dataCollector = new DataCollectorServiceRejections(dcServer, dcSap);

            dataCollector.rdjList = getRdListOnlyAllRejections();
            dataCollector.cdList = getCdListOnlyAllRejections();
            dataCollector.zvList = getZvListOnlyAllRejections();

            IDataCalculatorServiceRejections dataCalculator = new DataCalculatorServiceRejections(null);

            List<RejectionsProperty> list = dataCalculator.getRejectionsList(dataCollector);
            List<RejectionsProperty> expectedList = getExpectedListOnlyAllRejections();
            var xyar = list[0].EqualsDiff(expectedList[0]);
            CollectionAssert.AreEqual(expectedList, list);
        }

        private List<RejectionsProperty> getExpectedListOnlyAllRejections() {
            List<RejectionsProperty> list = new List<RejectionsProperty>() {
                new RejectionsProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey",
                    orderNumber = 1,
                    sku = 10,
                    item = 30,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                },
                new RejectionsProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey",
                    orderNumber = 2,
                    sku = 10,
                    item = 60,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                },
                new RejectionsProperty() {
                    soldTo = 3,
                    shipTo = 3,
                    country = "Turkey",
                    orderNumber = 3,
                    sku = 10,
                    item = 70,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                },
                new RejectionsProperty() {
                    soldTo = 4,
                    shipTo = 4,
                    country = "Turkey",
                    orderNumber = 4,
                    sku = 10,
                    item = 70,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                },
                new RejectionsProperty() {
                    soldTo = 5,
                    shipTo = 5,
                    country = "Turkey",
                    orderNumber = 5,
                    sku = 10,
                    item = 40,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                }
            };
            return list;
        }

        private List<RejectionsDataProperty> getRdListOnlyAllRejections() {
            List<RejectionsDataProperty> list = new List<RejectionsDataProperty>() {
                 new RejectionsDataProperty() {
                    soldTo = 0,
                    shipTo = 0,
                    country = "Turkey",
                    sku = 10,
                    rejectionReasonCode = "ZF"
                }
            };
            return list;
        }

        private List<CustomerDataProperty> getCdListOnlyAllRejections() {
            List<CustomerDataProperty> list = new List<CustomerDataProperty>() {
                new CustomerDataProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey"
                },
                new CustomerDataProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey"
                },
                new CustomerDataProperty() {
                    soldTo = 3,
                    shipTo = 3,
                    country = "Turkey"
                },
                new CustomerDataProperty() {
                    soldTo = 4,
                    shipTo = 4,
                    country = "Turkey"
                },
                new CustomerDataProperty() {
                    soldTo = 5,
                    shipTo = 5,
                    country = "Turkey"
                },
            };
            return list;
        }

        private List<ZV04IProperty> getZvListOnlyAllRejections() {
            List<ZV04IProperty> list = new List<ZV04IProperty>() {
                new ZV04IProperty() {
                    soldTo = 5,
                    shipTo = 5,
                    material = 10,
                    item = 40,
                    order = 5
                },
                new ZV04IProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    material = 10,
                    item = 30,
                    order = 1
                },
                new ZV04IProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    material = 10,
                    item = 60,
                    order = 2
                },
                new ZV04IProperty() {
                    soldTo = 3,
                    shipTo = 3,
                    material = 10,
                    item = 70,
                    order = 3
                },
                new ZV04IProperty() {
                    soldTo =4,
                    shipTo = 4,
                    material = 10,
                    item = 70,
                    order = 4
                }
            };
            return list;
        }
        #endregion

        #region shiptoAndSoldToAndAllRejections
        [TestMethod]
        public void rejDataCalculatorWithShiptoAndSoldToAndAllRejections_Should_ReturnListOfRejectionsForAllSkusThatHaveRejectionReasonInRejectionsData_When_NormalState() {

            IDataCollectorServer dcServer = null;
            IDataCollectorSap dcSap = null;

            IDataCollectorServiceRejections dataCollector = new DataCollectorServiceRejections(dcServer, dcSap);

            dataCollector.rdjList = getRdListWithShiptoAndSoldToAndAllRejections();
            dataCollector.zvList = getZvListWithShiptoAndSoldToAndAllRejections();
            dataCollector.cdList = getCdListWithShiptoAndSoldToAndAllRejections();

            IDataCalculatorServiceRejections dataCalculator = new DataCalculatorServiceRejections(null);

            List<RejectionsProperty> list = dataCalculator.getRejectionsList(dataCollector);
            List<RejectionsProperty> expectedList = getExpectedListWithShiptoAndSoldToAndAllRejections();

            var xyar = list[0].EqualsDiff(expectedList[0]);
            CollectionAssert.AreEqual(expectedList, list);
        }

        private List<RejectionsProperty> getExpectedListWithShiptoAndSoldToAndAllRejections() {
            List<RejectionsProperty> list = new List<RejectionsProperty>() {
                new RejectionsProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey",
                    orderNumber = 1,
                    sku = 10,
                    item = 30,
                    rejectionForCustomer = "Speciffic",
                    rejectionReasonCode = "Y4"
                },
                new RejectionsProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey",
                    orderNumber = 2,
                    sku = 10,
                    item = 60,
                    rejectionForCustomer = "Speciffic",
                    rejectionReasonCode = "Y4"
                },
                new RejectionsProperty() {
                    soldTo = 1,
                    shipTo = 3,
                    country = "Turkey",
                    orderNumber = 3,
                    sku = 10,
                    item = 70,
                    rejectionForCustomer = "Speciffic",
                    rejectionReasonCode = "Y5"
                },
                new RejectionsProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey",
                    orderNumber = 4,
                    sku = 10,
                    item = 40,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                },
                new RejectionsProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey",
                    orderNumber = 4,
                    sku = 10,
                    item = 70,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                }
            };
            return list;
        }

        private List<RejectionsDataProperty> getRdListWithShiptoAndSoldToAndAllRejections() {
            List<RejectionsDataProperty> list = new List<RejectionsDataProperty>() {
                new RejectionsDataProperty() {
                    soldTo = 1,
                    shipTo = 0,
                    country = "Turkey",
                    sku = 10,
                    rejectionReasonCode = "Y4"
                },
                 new RejectionsDataProperty() {
                    soldTo = 0,
                    shipTo = 0,
                    country = "Turkey",
                    sku = 10,
                    rejectionReasonCode = "ZF"
                },
                new RejectionsDataProperty() {
                    soldTo =1,
                    shipTo = 3,
                    country = "Turkey",
                    sku = 10,
                    rejectionReasonCode = "Y5"
                }
            };
            return list;
        }

        private List<CustomerDataProperty> getCdListWithShiptoAndSoldToAndAllRejections() {
            List<CustomerDataProperty> list = new List<CustomerDataProperty>() {
                new CustomerDataProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey"
                },
                new CustomerDataProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey"
                },
                new CustomerDataProperty() {
                    soldTo = 1,
                    shipTo = 3,
                    country = "Turkey"
                }
            };
            return list;
        }

        private List<ZV04IProperty> getZvListWithShiptoAndSoldToAndAllRejections() {
            List<ZV04IProperty> list = new List<ZV04IProperty>() {
                new ZV04IProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    material = 10,
                    item = 30,
                    order = 1
                },
                new ZV04IProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    material = 10,
                    item = 60,
                    order = 2
                },
                new ZV04IProperty() {
                    soldTo = 1,
                    shipTo = 3,
                    material = 10,
                    item = 70,
                    order = 3
                },
                new ZV04IProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    material = 10,
                    item = 70,
                    order = 4
                },
                new ZV04IProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    material = 10,
                    item = 40,
                    order = 4
                }
            };
            return list;
        }
        #endregion

        #region shiptoAndAllRejectionsWithDuplicatedSkusInSameOrders
        [TestMethod]
        public void rejDataCalculatorWithAllAndShipToRejections_Should_ReturnListOfRejectionsForAllSkusThatHaveRejectionReasonInRejectionsData_When_ThereAreMultipleDuplicateSkusInsideSameOrders() {

            IDataCollectorServer dcServer = null;
            IDataCollectorSap dcSap = null;

            IDataCollectorServiceRejections dataCollector = new DataCollectorServiceRejections(dcServer, dcSap);

            dataCollector.rdjList = getRdListWithAllAndShipToRejectionsDuplciatedSkusInOrders();
            dataCollector.zvList = getZvListWithAllAndShipToRejectionsDuplciatedSkusInOrders();
            dataCollector.cdList = getCdListWithAllAndShipToRejectionsDuplciatedSkusInOrders();

            IDataCalculatorServiceRejections dataCalculator = new DataCalculatorServiceRejections(null);

            List<RejectionsProperty> list = dataCalculator.getRejectionsList(dataCollector);
            List<RejectionsProperty> expectedList = getExpectedListWithAllAndShipToRejectionsDuplciatedSkusInOrders();
            var xyar = list[0].EqualsDiff(expectedList[0]);
            CollectionAssert.AreEqual(expectedList, list);
        }

        private List<RejectionsProperty> getExpectedListWithAllAndShipToRejectionsDuplciatedSkusInOrders() {
            List<RejectionsProperty> list = new List<RejectionsProperty>() {
                new RejectionsProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey",
                    orderNumber = 1,
                    sku = 10,
                    item = 30,
                    rejectionForCustomer = "Speciffic",
                    rejectionReasonCode = "Y4"
                },
                new RejectionsProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey",
                    orderNumber = 1,
                    sku = 10,
                    item = 60,
                    rejectionForCustomer = "Speciffic",
                    rejectionReasonCode = "Y4"
                },
                new RejectionsProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey",
                    orderNumber = 1,
                    sku = 10,
                    item = 70,
                    rejectionForCustomer = "Speciffic",
                    rejectionReasonCode = "Y4"
                },
                new RejectionsProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey",
                    orderNumber = 4,
                    sku = 10,
                    item = 40,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                },
                new RejectionsProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey",
                    orderNumber = 4,
                    sku = 10,
                    item = 70,
                    rejectionForCustomer = "All",
                    rejectionReasonCode = "ZF"
                }
            };
            return list;
        }

        private List<RejectionsDataProperty> getRdListWithAllAndShipToRejectionsDuplciatedSkusInOrders() {
            List<RejectionsDataProperty> list = new List<RejectionsDataProperty>() {
                new RejectionsDataProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey",
                    sku = 10,
                    rejectionReasonCode = "Y4"
                },
                 new RejectionsDataProperty() {
                    soldTo = 0,
                    shipTo = 0,
                    country = "Turkey",
                    sku = 10,
                    rejectionReasonCode = "ZF"
                }
            };
            return list;
        }

        private List<CustomerDataProperty> getCdListWithAllAndShipToRejectionsDuplciatedSkusInOrders() {
            List<CustomerDataProperty> list = new List<CustomerDataProperty>() {
                new CustomerDataProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    country = "Turkey"
                },
                new CustomerDataProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    country = "Turkey"
                }
            };
            return list;
        }

        private List<ZV04IProperty> getZvListWithAllAndShipToRejectionsDuplciatedSkusInOrders() {
            List<ZV04IProperty> list = new List<ZV04IProperty>() {
                new ZV04IProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    material = 10,
                    item = 30,
                    order = 1
                },
                new ZV04IProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    material = 10,
                    item = 60,
                    order = 1
                },
                new ZV04IProperty() {
                    soldTo = 1,
                    shipTo = 1,
                    material = 10,
                    item = 70,
                    order = 1
                },
                new ZV04IProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    material = 10,
                    item = 70,
                    order = 4
                },
                new ZV04IProperty() {
                    soldTo = 2,
                    shipTo = 2,
                    material = 10,
                    item = 40,
                    order = 4
                }
            };
            return list;
        }
        #endregion

    }
}
