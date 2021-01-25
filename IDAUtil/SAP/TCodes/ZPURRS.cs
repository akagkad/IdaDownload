using IDAUtil.Model.Properties.TcodeProperty.ZPURRSObj;
using IDAUtil.Service;
using lib;
using Microsoft.VisualBasic;
using System.Linq;
using System;

namespace IDAUtil
{
    public class ZPURRS
    {
        protected string tCode;
        public ISAPLib sap { get; set; }
        public IDAEnum.Task task { get; set; }

        public string salesOrg { get; set; }

        public ZPURRS(ISAPLib saplib)
        {
            sap = saplib;
        }

        public ZPURRS(ISAPLib sap, string salesOrg, IDAEnum.Task task)
        {
            this.sap = sap;
            this.salesOrg = salesOrg;
            this.task = task;
        }
        public void setParamsBeforeExecution()
        {
            sap.enterTCode("ZPURRS");
            setLayout();
            setPurchOrganization();
            setPlant();
            setDeliveryDateS();

            setMultiplePurchOrganization();
        }
        public void extractReport()
        {
            sap.pressF8();
            sap.tableToClipboard();
        }

        public void exportExcel(string path, string fileName)
        {
            sap.pressF8();
            sap.openExport();
            sap.exportExcel(path, fileName);
        }

        private void setPurchOrganization()
        {
            sap.setText(ZPURRSID.PUR_ORG_TEXT_FIELD_ID, "0400");


        }

        private void setMultiplePurchOrganization()
        {
            string[] PurchOrganization;
            PurchOrganization = new[] { "6300" };
            sap.pressBtn("wnd[0]/usr/btn%_S_EKORG_%_APP_%-VALU_PUSH");
            sap.setMultipleSelection(PurchOrganization, ZPURRSID.PUR_ORG_MULTISELECTION_BTN_IDD);
        }

        private void setLayout()
        {
            sap.setText(ZPURRSID.LAYOUT_TEXT_FIELD_ID, "");
        }

        private void setPlant()
        {
            int plant = new SalesOrgDetails().getPlant(salesOrg);
            sap.setText(ZPURRSID.PLANT_TEXT_FIELD_ID, plant.ToString());
        }

        private void setDeliveryDateS()
        {
            DateTime dateOnly1 = DateTime.Now.Date;
            DateTime dateOnly2 = dateOnly1.AddMonths(-1);
            //sap.setText(ZPURRSID.FINISH_DELIVERY_DATE_INPUT_TEXT_FIELD_ID, dateOnly1.ToString("dd'.'MM'.'yyyy"));
            sap.setText(ZPURRSID.START_DELIVERY_DATE_INPUT_TEXT_FIELD_ID, dateOnly2.ToString("dd'.'MM'.'yyyy"));
            sap.setText(ZPURRSID.FINISH_DELIVERY_DATE_INPUT_TEXT_FIELD_ID, dateOnly1.ToString("dd'.'MM'.'yyyy"));
        }

    }
}

