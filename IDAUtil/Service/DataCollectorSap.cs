using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Model.Properties.TcodeProperty.ZPURRSObj;
using IDAUtil.Model.Properties.TcodeProperty.MD04Obj;
using lib;
using System.Collections.Generic;
using System;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using SAPFEWSELib;
using System.Linq;

namespace IDAUtil.Service
{
    public class DataCollectorSap : IDataCollectorSap
    {
        private readonly ISAPLib sap;
        private readonly IExportParser parser;

        public DataCollectorSap(ISAPLib sap, IExportParser parser)
        {
            this.sap = sap;
            this.parser = parser;
        }

        public List<ZV04HNProperty> getZV04HNList(string salesOrg, IDAEnum.Task idaTask)
        {
            var ZV04HN = new ZV04HN(sap, salesOrg, idaTask);
            ZV04HN.setParamsBeforeExecution();
            ZV04HN.extractReport();
            var listZV04HN = parser.getObjectListFromClipboard<ZV04HNProperty>();
            return listZV04HN;
        }

        public List<ZV04IProperty> getZV04IList(string salesOrg, IDAEnum.Task idaTask)
        {
            var zv04I = new ZV04I(sap, salesOrg, idaTask);
            zv04I.setParamsBeforeExecution();
            zv04I.extractReport();
            var list = parser.getObjectListFromClipboard<ZV04IProperty>();
            return list;
        }



        public List<ZV04PProperty> getZV04PList(string salesOrg, IDAEnum.Task idaTask)
        {
            var zv04P = new ZV04P(sap, salesOrg, idaTask);
            zv04P.setParamsBeforeExecution();
            zv04P.extractReport();
            var list = parser.getObjectListFromClipboard<ZV04PProperty>();
            return list;
        }

        public List<ZPURRSProperty> getZPURRSList(string salesOrg, IDAEnum.Task idaTask)
        {
            var ZPURRS = new ZPURRS(sap, salesOrg, idaTask);
            ZPURRS.setParamsBeforeExecution();
            ZPURRS.extractReport();
            List<ZPURRSProperty> listZPURRS = parser.getObjectListFromClipboard<ZPURRSProperty>();
           if (!(listZPURRS is null)) listZPURRS.RemoveAll(x => x.Material == 0);
            
            return listZPURRS;
        }

        public List<ZV04IProperty> getZV04IfloorCuts(List<MD04Property> MD04List, string salesOrg, IDAEnum.Task idaTask) {
            var ZV04I = new ZV04I(sap, salesOrg, idaTask);
            //ZV04I.setParamsBeforeExecution();
            sap.enterTCode("ZV04I");

            ZV04I.sap.setText("wnd[0]/usr/ctxtS_LDDAT-LOW", DateTime.Now.Date.ToString("dd'.'MM'.'yyyy"));
            ZV04I.sap.setText("wnd[0]/usr/ctxtS_LDDAT-HIGH", DateTime.Now.Date.ToString("dd'.'MM'.'yyyy"));
            ZV04I.sap.setText("wnd[0]/usr/ctxtS_VKORG-LOW", salesOrg);
            ZV04I.sap.setCheckboxStatus(ZV04ID.OPEN_ORDER_CHECKBOX_ID, true);
            ZV04I.sap.setCheckboxStatus(ZV04ID.CREDIT_HOLD_CHECKBOX_ID, true);
            ZV04I.sap.setCheckboxStatus(ZV04ID.WITH_DELIVERY_CHECKBOX_ID, false);
            ZV04I.sap.setCheckboxStatus(ZV04ID.NOT_PGI_CHECKBOX_ID, false);
            ZV04I.sap.setCheckboxStatus(ZV04ID.NOT_INVOICED_CHECKBOX_ID, false);
            //ZV04I.sap.pressBtn("wnd[0]/usr/btn%_S_MATNR_%_APP_%-VALU_PUSH");
            if (MD04List is null) {
                return null;
            }
            else { 
            string[] arrayList = new string[MD04List.Count];
            for (int i = 0; i < MD04List.Count; i++) { arrayList[i] = MD04List[i].sku; }
            //string[] arrayLis1t = MD04List.ToArray();
            ZV04I.sap.setMultipleSelection(arrayList, "wnd[0]/usr/btn%_S_MATNR_%_APP_%-VALU_PUSH");
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{salesOrg} {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}hour {DateTime.Now.Minute}minute Orders with past PO date.xlsx";

            ZV04I.extractReport();

            var listZV04HNfloorCuts = parser.getObjectListFromClipboard<ZV04IProperty>();

            //ZV04I.sap.pressBtn("wnd[0]/tbar[1]/btn[43]");
            //ZV04I.sap.setText("wnd[1]/usr/ctxtDY_PATH", path);
            //ZV04I.sap.setText("wnd[1]/usr/ctxtDY_FILENAME", "AlexFloorCuts.xlsm");

            //ZV04I.sap.pressBtn("wnd[1]/tbar[0]/btn[11]");
            //ZV04I.sap.exportExcel(@"C:\Users\g168180\Documents\SAP\SAP GUI\", "AlexFloorCuts.xlsx");

            return listZV04HNfloorCuts;
        }
        }

        public List<MD04Property> getMD04List(string material, IDAEnum.Task idaTask, List<MD04Property> listMD04, string salesOrg)
        {
            var MD04 = new MD04(sap);
            var listMD041 = new List<string>();

            if (!MD04.getStockDetails(material, salesOrg))
            {
                listMD04.Add(new MD04Property() { sku = material });
            }
            //listMD04.Add(MD04.getStockDetails(material, "ES01"));
            // MD04.getStockDetails(material,"ES01");

            //List<MD04Property> listMD04 = parser.getObjectListFromClipboard<MD04Property>();
            //GuiTableControl table1 = new GuiTableControl();
            // GuiTableControl table = MD04.extractReport() as GuiTableControl;

            // List<MD04Property> listMD04 = MD04.TableToList(table);
            //string help1 = table.getCellValue(0, 5);
            //help1 = help1.Replace(".000", "");
            //help1 = help1.Replace(",", "");

            //int stock = Int32.Parse(help1);


            //  int i = 0;
            //while ((!String.IsNullOrEmpty(table.getCellValue(i, 0)) && ((DateTime.Parse(table.getCellValue(i, 1)) <= DateAndTime.Now && (table.getCellValue(i, 2) == "CurOrd" || table.getCellValue(i, 2) == "Dlvr")){

            //    stock = stock - (Int32.Parse(table.getCellValue(i, 1).Replace(".000", "")));
            //    i++;
            //}

            //if stock < 0 {

            //}
            return listMD04;
            //return listMD04;
        }
    }
}