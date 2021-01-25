using IDAUtil;
using IDAUtil.Model.Properties.TcodeProperty.VA02;
using IDAUtil.SAP.TCode;
using lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using IDAUtil.SAP.TCode.Runners;

namespace SAPTest {
    [TestClass]
    public class VA02Test {
        private readonly ISAPLib sap = Create.sapLib();
        private readonly IdaLog idaLog = new IdaLog();

        [TestMethod]
        public void soarAction_Should_UploadFileToOrder_When_ButtonForUploadingOrdersExist() {
            IVA02 va02 = new VA02(sap, idaLog);
            sap.enterTCode("VA02");
            va02.enterOrder(208736896);
            va02.soarAction("this is testing the csr notes function to upload a note into the order with enough text", "DeliveryBlocks", 208736896);
        }

        [TestMethod]
        public void runRejections_Should_SpltLineCorrectly_When_UnitsOfMeasuresAreMUN() {
            IVA02 va02 = new VA02(sap, idaLog);
            RejectionsSapOrderProperty rejectionsSapOrderProperty = getRejList();
            sap.enterTCode("VA02");
            va02.enterOrder(rejectionsSapOrderProperty.orderNumber);
            va02.bypassInitialPopups();
            ITable table = va02.getTable();
            var rejectionsVA02Runner = new RejectionsVA02Runner(sap: sap, log: idaLog, va02: va02, isRelease: true, isLog: false);
            rejectionsVA02Runner.runRejections(rejectionsSapOrderProperty, "someID", "someTable");

        }

        private RejectionsSapOrderProperty getRejList() {
            // ord qty 3.333,000 MCS conf 225,000 MCS
            // ord qty 3.333,000 MCS conf 225,000 MCS

            return new RejectionsSapOrderProperty() {
                orderNumber = 208018026,
                lineDetails = new List<RejectionsSapLineProperty>() {
                       new RejectionsSapLineProperty {
                           sku = 691082,
                           orderedQty = 3333,
                           confirmedQty = 225
                       }
                    }
            };
        }

    }
}

