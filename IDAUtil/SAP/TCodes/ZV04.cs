using lib;
using System;
using System.Collections.Generic;

namespace IDAUtil {
    public abstract class ZV04 {
        protected string tCode;

        public List<String> Material { get; set; }
        public List<string> Description { get; set; }
        public int CSRASR { get; set; }
        public string PONumber { get; set; }
        public List<string> plant { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime RescheduleDate { get; set; }
        public List<int> PurchaseDocument { get; set; }
        public string[] docType { get; set; }
        public ISAPLib sap { get; set; }
        public bool isOpenOrder { get; set; }
        public bool isCreditHold { get; set; }
        public bool isWithDelivery { get; set; }
        public bool isNotPGI { get; set; }
        public bool isNotInvoiced { get; set; }
        public string salesOrg { get; set; }
        public IDAEnum.Task task { get; set; }

        public ZV04(ISAPLib sap, string salesOrg, IDAEnum.Task task) {
            this.sap = sap;
            this.salesOrg = salesOrg;
            this.task = task;
        }

        public void setParamsBeforeExecution() {
            sap.enterTCode(tCode);
            setSalesOrg();
            setDocStatus();
            setDocType();
        }

        public void extractReport() {
            sap.pressF8();
            sap.tableToClipboard();
        }

        public void exportExcel(string path, string fileName) {
            sap.pressF8();
            sap.openExport();
            sap.exportExcel(path, fileName);
        }

        private void setDocStatus() {
            var switchExpr = task;
            switch (switchExpr) {
                case IDAEnum.Task.rdd:
                case IDAEnum.Task.quantityConversion:
                case IDAEnum.Task.switches:
                case IDAEnum.Task.rejections:
                case IDAEnum.Task.distress: {
                        isOpenOrder = true;
                        isCreditHold = true;
                        isNotInvoiced = false;
                        isNotPGI = false;
                        isWithDelivery = false;
                        break;
                    }

                case IDAEnum.Task.SOAR:
                case IDAEnum.Task.customerMissingReport:
                case IDAEnum.Task.missingCMIRReport:
                case IDAEnum.Task.deliveryBlocks: {
                        isOpenOrder = true;
                        isCreditHold = true;
                        isNotInvoiced = true;
                        isNotPGI = true;
                        isWithDelivery = true;
                        break;
                    }

                default: {
                        throw new NotImplementedException("Failed getting document statuses for ZV04");
                    }
            }

            try {
                sap.setCheckboxStatus(ZV04ID.OPEN_ORDER_CHECKBOX_ID, isOpenOrder);
                sap.setCheckboxStatus(ZV04ID.CREDIT_HOLD_CHECKBOX_ID, isCreditHold);
                sap.setCheckboxStatus(ZV04ID.WITH_DELIVERY_CHECKBOX_ID, isWithDelivery);
                sap.setCheckboxStatus(ZV04ID.NOT_PGI_CHECKBOX_ID, isNotPGI);
                sap.setCheckboxStatus(ZV04ID.NOT_INVOICED_CHECKBOX_ID, isNotInvoiced);
            } catch (Exception) {
                sap.setCheckboxStatus(ZV04ID.OPEN_ORDER_CHECKBOX_ID, isOpenOrder);
                sap.setCheckboxStatus(ZV04ID.CREDIT_HOLD_CHECKBOX_ID, isCreditHold);
                sap.setCheckboxStatus(ZV04ID.WITH_DELIVERY_CHECKBOX_ID, isWithDelivery);
                sap.setCheckboxStatus(ZV04ID.NOT_PGI_CHECKBOX_ID, isNotPGI);
                sap.setCheckboxStatus(ZV04ID.NOT_INVOICED_CHECKBOX_ID, isNotInvoiced);
            }
        }

        private void setSalesOrg() {
            sap.setText(ZV04ID.SALES_ORG_TEXT_FIELD_ID, salesOrg);
        }

        private void setDocType() {
            string[] docType;
            var switchExpr = salesOrg;
            switch (switchExpr) {
                case "GB01":
                case "TR01":  {
                        docType = new[] { "ZOR", "ZEC" };
                        break;
                    }

                default: {
                        docType = new[] { "ZOR" };
                        break;
                    }
            }

            sap.setMultipleSelection(docType, ZV04ID.DOCT_TYPE_MULTISELECTION_BTN_IDD);
        }
    }

    public class ZV04HN : ZV04 {
        public ZV04HN(ISAPLib sap, string salesOrg, IDAEnum.Task task) : base(sap, salesOrg, task) {
            tCode = "ZV04HN";
        }
    }

    public class ZV04I : ZV04 {
        public DateTime loadingDateFrom { get; set; }
        public DateTime loadingDateTo { get; set; }
        public List<int> materialNumber { get; set; }
        public List<int> saleDocItem { get; set; }

        public ZV04I(ISAPLib sap, string salesOrg, IDAEnum.Task task) : base(sap, salesOrg, task) {
            tCode = "ZV04I";
        }
    }

    public class ZV04P : ZV04 {
        public ZV04P(ISAPLib sap, string salesOrg, IDAEnum.Task task) : base(sap, salesOrg, task) {
            tCode = "ZV04P";
        }
    }
}