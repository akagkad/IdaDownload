using lib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUnitTest {
    /// <summary>
    /// Summary description for GenericTest
    /// </summary>
    [TestClass]
    public class GenericTest {

        public GenericTest() { }

        [TestMethod]
        public void TestMethod1() {

            //todo try list as dynamic
            IDBConnector dbxl = Create.dbXl();
            string salesOrg = "TR01";

            string path = $"{Paths.DESKTOP}\\{salesOrg} {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}hour {DateTime.Now.Minute}minute Distress Report.xlsx";

            var list = new List<myList>() { new myList() {
                a = 1,
                b = "a"
                }
            };

            dbxl.listToExcel(list: list, path: path);

        }

        public class myList {
            [Column("[Country]")] public int a { get; set; }
            [Column("[salesOrg]")] public string b { get; set; }
        }

    }
}
