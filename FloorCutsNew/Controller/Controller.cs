using IDAUtil;
using IDAUtil.Service;
using lib;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using IDAUtil.Model.Properties.TcodeProperty.MD04Obj;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using System.Linq;
using System.Collections;
using FloorCutsNew.Model;

namespace FloorCutsNew
{
    class Controller
    {
        public static void executePastPOdate(string salesOrg)
        {

            var idaLog = new IdaLog();
            string id = $"{DateTime.Now} {Environment.MachineName}";


            // idaLog.insertToActivityLog("FloorCuts", "startTime", id, salesOrg);

            var dbServer = Create.dbServer();

            var sap = Create.sapLib();
            DataCollectorServer dcServer = new DataCollectorServer(dbServer);
            DataCollectorSap dcSap = new DataCollectorSap(sap, Create.exportParses());
            DataCollectorServiceFloorCut dcService = new DataCollectorServiceFloorCut(dcServer, dcSap);

            DistributionListCalculator distList = new DistributionListCalculator(dbServer);
            string email = distList.getDistList(salesOrg, "FloorCuts");
            //string email = "akagkad@scj.com";
            List<int> ZPURRSList = dcService.getpastPOdates(salesOrg);
            IMailUtil mu = Create.mailUtil();
            //List<List<int>> list = new List<List<int>>();
            if (ZPURRSList is null ) {
                mu.mailSimple(email, $"{salesOrg} Orders with past PO date {DateTime.Now}", $"Hello<br><br><br>There are no Orders with past PO date<br><br>Kind Regards<br>IDA");

                return;
            }
            List<string> MD04List = new List<string>();
            var listMD04 = new List<MD04Property>();
            foreach (int item in ZPURRSList)
            {
                string item1 = item.ToString();
                listMD04 = dcService.getMD04(item1, listMD04, salesOrg);

            }
            List<ZV04IProperty> finalList = new List<ZV04IProperty>();

            if (listMD04.Count == 0 ) {
                mu.mailSimple(email, $"{salesOrg} Orders with past PO date {DateTime.Now}", $"Hello<br><br><br>There are no Orders with past PO date<br><br>Kind Regards<br>IDA");
                return;
            }
        

 
            else finalList = dcService.checkStock(listMD04, salesOrg);
            //ArrayList cats = new ArrayList();
            if (  !(finalList is null) && (finalList.Count > 0)) {
                switch (salesOrg.ToUpper()) {
                    case "ES01":
                    case "PT01":
                    case "PL01":
                    case "CZ01":
                        //foreach (MD04Property item in listMD04) {
                        //    cats.AddRange(item.sku, item.plant);
                        //}
                        //var cats = (from item in listMD04
                        //            select new { item.sku, item.plant })
                        //                          .ToList();
                        //var results = listMD04.Select(x => new MD04Property() {
                        //    plant = x.plant,

                        //}).ToList();
                        var results = (from a in finalList
                                       select new  ES_PT_PL_CZ_list { OrderNumber=a.order, RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription , UPC = a.upc, orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, shipToName = a.shipToName }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results)}<br><br>Find report with PO Past of today<br>IDA");

                        //sendEmailWithExcelFile(salesOrg, results, mu, email);


                        break;

                    case "ZA01":
                    case "NG01":
                    case "KE02":
                        var results1 = (from a in finalList
                                       select new ZA_NG_KElist { OrderNumber = a.order, docTyp= a.docTyp , docDate = a.docDate , RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription,  orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, shipToName = a.shipToName }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results1)}<br><br>Find report with PO Past of today<br>IDA");
                        //sendEmailWithExcelFile(salesOrg, results1, mu, email);
                        break;

                    case "GB01":
                    case "NL01":
                        var results2 = (from a in finalList
                                        select new GB_NL_list { OrderNumber = a.order, docTyp = a.docTyp, RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription, orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, soldTo = a.soldTo , soldToName = a.soldToName , shipToName = a.shipToName , shipTo = a.shipTo}).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results2)}<br><br>Find report with PO Past of today<br>IDA");
                        break;
                         
                    case "IT01":
                    case "RO01":
                        var results3 = (from a in finalList
                                        select new IT_RO_list { OrderNumber = a.order, docTyp = a.docTyp, RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription, UPC = a.upc , orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, soldTo = a.soldTo, soldToName = a.soldToName, shipToName = a.shipToName, shipTo = a.shipTo }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results3)}<br><br>Find report with PO Past of today<br>IDA");

                        break;

                    case "DE01":
                        var results4 = (from a in finalList
                                        select new DE_list { OrderNumber = a.order, RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription, UPC = a.upc, plant = a.plant , orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, shipToName = a.shipToName, shipTo = a.shipTo , csrAsr=a.csrAsr }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results4)}<br><br>Find report with PO Past of today<br>IDA");
                        break;

                    case "FR01":
                        var results5 = (from a in finalList
                                        select new FR_list { OrderNumber = a.order, docTyp = a.docTyp , RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription, orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, soldTo = a.soldTo, soldToName = a.soldToName, shipToName = a.shipToName, shipTo = a.shipTo, csrAsr = a.csrAsr }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results5)}<br><br>Find report with PO Past of today<br>IDA");
                        break;

                    case "GR01":
                        var results6 = (from a in finalList
                                        select new GR_list { OrderNumber = a.order, RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription, orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, soldTo = a.soldTo, soldToName = a.soldToName, shipToName = a.shipToName, shipTo = a.shipTo }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results6)}<br><br>Find report with PO Past of today<br>IDA");
                        break;

                    case "RU01":
                        var results7 = (from a in finalList
                                        select new RU_list { OrderNumber = a.order, docDate = a.docDate, RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription, orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, soldTo = a.soldTo, soldToName = a.soldToName }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results7)}<br><br>Find report with PO Past of today<br>IDA");
                        break;

                    case "TR01":
                        var results8 = (from a in finalList
                                        select new TR_list { OrderNumber = a.order, docDate = a.docDate, RDD = a.reqDelDate, Material = a.material, MaterialDescription = a.materialDescription, orderQty = a.orderQty, confirmedQty = a.confirmedQty, pONumber = a.pONumber, soldTo = a.soldTo, soldToName = a.soldToName, shipToName = a.shipToName, shipTo = a.shipTo }).ToList();

                        mu.mailSimple(email, $"{salesOrg} {DateTime.Now} PO past report date:", $"Hello<br><br>{mu.listToHTMLtable(results8)}<br><br>Find report with PO Past of today<br>IDA");
                        break;

                    default:

                        break;
                }
            }
            else {
                // idaLog.insertToActivityLog("missingCMIR", "empty list", id, salesOrg);
                mu.mailSimple(email, $"{salesOrg} Orders with past PO date {DateTime.Now}", $"Hello<br><br><br>There are no Orders with past PO date<br><br>Kind Regards<br>IDA");
            }
            
            //if (listMD04.Count > 0) {
            //    sendEmailWithExcelFile(salesOrg, listMD04, mu, email);
            //    // idaLog.insertToActivityLog("missingCMIR", "success", id, salesOrg);
            //}
            //else {
            //    // idaLog.insertToActivityLog("missingCMIR", "empty list", id, salesOrg);
            //    mu.mailSimple(email, $"{salesOrg} Orders with past PO date {DateTime.Now}", $"Hello<br><br><br>There are no Orders with past PO date<br><br>Kind Regards<br>IDA");
            //}

        }



        private static void sendEmailWithExcelFile(string salesOrg, List<ES_PT_PL_CZ_list> results, IMailUtil mu, string email) {
            throw new NotImplementedException();
        }

        private static void sendEmailWithExcelFile(string salesOrg, List<MD04Property> listMD04, IMailUtil mu, string email)
        {
            IDBConnector dbxl = Create.dbXl();
            Workbook wb;

            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{salesOrg} {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}hour {DateTime.Now.Minute}minute Orders with past PO dates.xlsx";

            dbxl.listToExcel(list: listMD04, path: path);

            using (IExcelUtil xl = Create.xlUtil())
            {
                try
                {
                    wb = xl.getWorkbook(path: path);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(5000);
                    wb = xl.getWorkbook(path: path);
                }

                Worksheet ws = wb.Worksheets[1] as Worksheet;

                try
                {
                    IdaExcelService.prettifyExcel(worksheet: ws, excelUtilObj: xl);
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(2000);
                    IdaExcelService.prettifyExcel(worksheet: ws, excelUtilObj: xl);
                }

                wb.Save();

                mu.mailOneAttachment(emailTo: email, cc: "", subject: $"{salesOrg} Orders with past PO date", body: $"{mu.listToHTMLtable(listMD04)}", path: path);
                dbxl.closeConnection();
                xl.closeWBAndAllInstances(wbName: wb.Name);
            }
        }
    }
}