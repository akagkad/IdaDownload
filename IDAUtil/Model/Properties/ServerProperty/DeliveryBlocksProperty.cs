using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class DeliveryBlocksProperty {
        [Column("[id]")] public string id { get; set; }
        [Column("[orderStatus]")] public string orderStatus { get; set; }
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[orderNumber]")] public int orderNumber { get; set; }
        [Column("[poNumber]")] public string poNumber { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[soldToName]")] public string soldToName { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[currentDeliveryBlock]")] public string currentDeliveryBlock { get; set; }
        [Column("[newDeliveryBlock]")] public string newDeliveryBlock { get; set; }
        [Column("[currentQty]")] public double currentQty { get; set; }
        [Column("[minQty]")] public double minQty { get; set; }
        [Column("[currentVal]")] public double currentVal { get; set; }
        [Column("[minVal]")] public double minVal { get; set; }
        [Column("[reason]")] public string reason { get; set; }
        [Column("[customerEmails]")] public string customerEmails { get; set; }
        [Column("[poDate]")] public string poDate { get; set; }
        [Column("[RDD]")] public DateTime rdd { get; set; }

        public DeliveryBlocksProperty() { }

        public DeliveryBlocksProperty(string id, string orderStatus, string salesOrg, string country, int orderNumber, string poNumber, int soldTo, string soldToName, int shipTo, string shipToName, string currentDeliveryBlock, string newDeliveryBlock, double currentQty, double minQty, double currentVal, double minVal, string reason, string customerEmails, string poDate, DateTime rdd) {
            this.id = id;
            this.orderStatus = orderStatus;
            this.salesOrg = salesOrg;
            this.country = country;
            this.orderNumber = orderNumber;
            this.poNumber = poNumber;
            this.soldTo = soldTo;
            this.soldToName = soldToName;
            this.shipTo = shipTo;
            this.shipToName = shipToName;
            this.currentDeliveryBlock = currentDeliveryBlock;
            this.newDeliveryBlock = newDeliveryBlock;
            this.currentQty = currentQty;
            this.minQty = minQty;
            this.currentVal = currentVal;
            this.minVal = minVal;
            this.reason = reason;
            this.customerEmails = customerEmails;
            this.poDate = poDate;
            this.rdd = rdd;
        }
    }
}