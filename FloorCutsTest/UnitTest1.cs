using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDAUtil;
using IDAUtil.Model.Properties.TcodeProperty.MD04Obj;
using IDAUtil.SAP.TCode;
using lib;
using IDAUtil.Model.Properties.TcodeProperty.ZPURRSObj;
using System.Collections.Generic;
using IDAUtil.SAP.TCode.Runners;

namespace FloorCutsTest
{
    [TestClass]
    public class UnitTest1
    {

        private readonly ISAPLib sap = Create.sapLib();
        [TestMethod]
        public void TestMethod1()
        {
            List<ZPURRSProperty> zpurrss = getZPURRSList();
            List<MD04Property> md04s = getMD04List();
            ITable test1 = new TableControl(sap,"");
            test1.setCellValue(0, 8, "200,000");
            test1.setCellValue(1, 8, "100,000");
            test1.setCellValue(2, 8, "150,000");
            test1.setCellValue(0, 2, "Stock");
            test1.setCellValue(1, 2, "CusOrd");
            test1.setCellValue(2, 2, "POitem");


            //List<string> materialsAfterCheckofPOdate = getMD04MaterialsList();

        }

        private List<ZPURRSProperty> getZPURRSList()
        {
            List<ZPURRSProperty> list = new List<ZPURRSProperty>() {
                new ZPURRSProperty() {
                    Material = 1
                    
                },
                  new ZPURRSProperty() {
                  Material = 2
                },
                    new ZPURRSProperty() {
                    Material = 3
                },
                    new ZPURRSProperty() {
                   Material = 4
                },
                    new ZPURRSProperty() {
                    Material = 5
                }
            };
            return list;
        }

        private List<MD04Property> getMD04List()
        {
            List<MD04Property> list = new List<MD04Property>() {
                 new MD04Property() {
                    sku = "Stock",
                    recoveryDate = "07.12.2020",
                    plant=10,
                   
                    salesOrg = "20,000"

                },
                new MD04Property() {
                    sku = "POitem",
                    recoveryDate = "06.12.2020",
                    plant=10,

                    salesOrg = "10,000"
                },
                  new MD04Property() {
                    sku = "CusOrd",
                    recoveryDate = "05.12.2020",
                    plant=10,

                    salesOrg = "8,000"
                },
                   new MD04Property() {
                    sku = "CusOrd",
                    recoveryDate = "08.12.2020",
                    plant=10,

                    salesOrg = "38,000"
                },
                    new MD04Property() {
                    sku = "Stock",
                    recoveryDate = "05.12.2020",
                    plant=1,

                    salesOrg = "8,000"
                },
                    new MD04Property() {
                    sku = "CusOrd",
                    recoveryDate = "07.12.2020",
                    plant=1,

                    salesOrg = "7,000"
                },
                    new MD04Property() {
                    sku = "POitem",
                    recoveryDate = "04.12.2020",
                    plant=1,
                    salesOrg = "7,000"
                },
                     new MD04Property() {
                     sku = "Stock",
                    recoveryDate = "04.12.2020",
                    plant=2,
                    salesOrg = "7,000"

                }
            };
            return list;
        }
    }
}
