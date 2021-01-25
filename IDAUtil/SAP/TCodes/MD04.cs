using IDAUtil.Model.Properties.TcodeProperty.MD04Obj;
using IDAUtil.Service;
using lib;
using Microsoft.VisualBasic;
using System.Linq;
using System;

namespace IDAUtil
{
    public class MD04
    {
        private readonly ISAPLib sap;

        public MD04(ISAPLib saplib)
        {
            sap = saplib;
        }

        public bool getStockDetails(string sku, string salesOrg)
        {
            int plant = new SalesOrgDetails().getPlant(salesOrg);
            sap.enterTCode("MD04");
            bool isOpenTable = isOpenStockAndDates(sku, plant.ToString());
            //if (!isOpenTable)
            //{
            //    return new MD04Property()
            //    {
            //        salesOrg = salesOrg,
            //        plant = plant,
            //        sku = sku,
            //        ATP = 0,
            //        recoveryQty = 0,
            //        recoveryDate = "SKU does not exists for this plant"
            //    };
            //}

            var table = sap.getITableObject();
            string qty = table.getCellValue(0, 8);
            double atp = 0;
            if (!String.IsNullOrEmpty(qty)) { atp = double.Parse(qty); }

            string recDate = "";
            string recQty = "";
            int i = 1;
            // had to do this due to a table bug

            System.Threading.Thread.Sleep(1000);
            (sap.findById("wnd[0]") as dynamic).resizeWorkingPane(100, 25, 0);
            System.Threading.Thread.Sleep(1000);
            try {
                while (!string.IsNullOrEmpty(table.getCellValue(i, 1)) && DateTime.Parse(table.getCellValue(i, 1)) <= DateAndTime.Now) {
                    //if (new[] { "Delvry", "CusOrd", "ShpgNt" }.Contains(table.getCellValue(i, 2)))
                   if (new[] {"Delvry"}.Contains(table.getCellValue(i, 2)) &&  (DateTime.Parse(table.getCellValue(i, 1)) == DateAndTime.Today)) {
                        recQty = table.getCellValue(i, 8);
                        recQty = recQty.Replace("-", "");
                        atp = atp - double.Parse(recQty);
                    }
                    if ((new[] { "CusOrd" }.Contains(table.getCellValue(i, 2))) && (DateTime.Parse(table.getCellValue(i, 1)) <= DateAndTime.Now)) {
                        recQty = table.getCellValue(i, 8);
                        recQty = recQty.Replace("-", "");
                        atp = atp - double.Parse(recQty);
                    }

                    //{
                    //{
                    //    recDate = table.getCellValue(i, 1);
                    //    recQty = double.Parse(table.getCellValue(i, 8));
                    //    return new MD04Property()

                    //}
                    // recQty = table.getCellValue(i, 8);
                    //recQty = recQty.Replace("-", "");
                    //atp = atp - double.Parse(recQty);
                    //}
                    i += 1;
                }

                //if (i > 1) {
                //    recQty = table.getCellValue(i-1, 9);

                //    if (recQty.Contains("-")) {
                //        recQty = recQty.Replace("-", "");
                //        atp = -double.Parse(recQty);
                //    }
                //    else {
                //        atp = double.Parse(table.getCellValue(i-1, 9));
                //    }

                //}
                //else {
                //    atp = double.Parse(table.getCellValue(i, 9));

                //}
            if (atp <= 0)
                { return false; }
                else return true;
            }
            catch (TableWasNotScrolledException)
            {
            }

            //    return new MD04Property()
            //    {
            //        salesOrg = salesOrg,
            //        plant = plant,
            //        sku = sku,
            //        ATP = atp,
            //        recoveryQty = recQty,
            //        recoveryDate = string.IsNullOrEmpty(recDate) ? "No Date Available" : recDate
            //    };
            return true;
        }

        private bool isOpenStockAndDates(string sku, string plant)
        {
            sap.setText(MD04ID.SKU_INPUT_TEXT_FIELD_ID, sku.ToString());
            sap.setText(MD04ID.PLANT_TEXT_FIELD_ID, plant);
            sap.pressEnter();
            if (sap.getInfoBarMsg().Contains("error"))
            {
                sap.pressEnter();
            }

            if (sap.getInfoBarMsg().Contains("not found"))
            {
                return false;
            }

            return true;
        }
    }
}