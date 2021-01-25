using IDAUtil.Model.Properties.TcodeProperty.CO09Obj;
using IDAUtil.Service;
using lib;
using Microsoft.VisualBasic;
using System.Linq;

namespace IDAUtil {
    public class CO09 {
        private readonly ISAPLib sap;

        public CO09(ISAPLib saplib) {
            sap = saplib;
        }

        public CO09Property getStockDetails(int sku, string salesOrg) {
            int plant = new SalesOrgDetails().getPlant(salesOrg);
            sap.enterTCode("CO09");
            bool isOpenTable = isOpenStockAndDates(sku, plant.ToString());
            if (!isOpenTable) {
                return new CO09Property() {
                    salesOrg = salesOrg,
                    plant = plant,
                    sku = sku,
                    ATP = 0,
                    recoveryQty = 0,
                    recoveryDate = "SKU does not exists for this plant"
                };
            }

            var table = sap.getITableObject();
            double atp = double.Parse(table.getCellValue(0, 4));
            
            string recDate = "";
            double recQty = 0;
            int i = 0;
            // had to do this due to a table bug

            System.Threading.Thread.Sleep(1000);
            (sap.findById("wnd[0]") as dynamic).resizeWorkingPane(100, 25, 0);
            System.Threading.Thread.Sleep(1000);
            try {
                while (!string.IsNullOrEmpty(table.getCellValue(i, 1))) {
                    if (new[] { "POitem", "PurRqs", "ShpgNt" }.Contains(table.getCellValue(i, 1))) {
                        recDate = table.getCellValue(i, 0);
                        recQty = double.Parse(table.getCellValue(i, 3));
                        break;
                    }

                    i += 1;
                }
            } catch (TableWasNotScrolledException) {
            }

            return new CO09Property() {
                salesOrg = salesOrg,
                plant = plant,
                sku = sku,
                ATP = atp,
                recoveryQty = recQty,
                recoveryDate = string.IsNullOrEmpty(recDate) ? "No Date Available" : recDate
            };
        }

        private bool isOpenStockAndDates(long sku, string plant) {
            sap.setText(CO09ID.SKU_INPUT_TEXT_FIELD_ID, sku.ToString());
            sap.setText(CO09ID.PLANT_INPUT_TEXT_FIELD_ID, plant);
            sap.pressEnter();
            if (sap.getInfoBarMsg().Contains("Text for MRP")) {
                sap.pressEnter();
            }

            if (sap.getInfoBarMsg().Contains("not found")) {
                return false;
            }

            return true;
        }
    }
}