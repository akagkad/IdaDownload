using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class NLDistressProperty {
        [Column("[Release Date]")] public string loadingDate { get; set; }
        [Column("[PO Date]")] public string poDate { get; set; }
        [Column("[RDD]")] public string rdd { get; set; }
        [Column("[SCJ Order Number]")] public int order { get; set; }
        [Column("[PO Number]")] public string poNumber { get; set; }
        [Column("[ShipTo Name]")] public string shipToName { get; set; }
        [Column("[SKU]")] public int sku { get; set; }
        [Column("[EAN]")] public string unitBarcode { get; set; }
        [Column("[SKU Description]")] public string skuDescription { get; set; }
        [Column("[Order QTY]")] public double orderQty { get; set; }
        [Column("[Cut QTY]")] public double cutQty { get; set; }
        [Column("[Comment]")] public string criticalItemComment { get; set; }
        [Column("[Possible Switch]")] public int possibleSwitch { get; set; }
        [Column("[Possible Switch Description]")] public string possibleSwitchDescription { get; set; }
        [Column("[After Release RRC]")] public string afterReleaseRejection { get; set; }

        public NLDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.loadingDate = genericDistressProperty.loadingDate;
            this.poDate = genericDistressProperty.poDate;
            this.rdd = genericDistressProperty.rdd;
            this.order = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.shipToName = genericDistressProperty.shipToName;
            this.sku = genericDistressProperty.material;
            this.unitBarcode = genericDistressProperty.unitBarcode;
            this.skuDescription = genericDistressProperty.materialDescription;
            this.orderQty = genericDistressProperty.orderQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.criticalItemComment = genericDistressProperty.criticalItemComment;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.possibleSwitchDescription = genericDistressProperty.possibleSwitchDescription;
            this.afterReleaseRejection = genericDistressProperty.afterReleaseRej;
        }

        public override bool Equals(object obj) {
            return obj is NLDistressProperty property &&
                   loadingDate == property.loadingDate &&
                   poDate == property.poDate &&
                   rdd == property.rdd &&
                   order == property.order &&
                   poNumber == property.poNumber &&
                   shipToName == property.shipToName &&
                   sku == property.sku &&
                   unitBarcode == property.unitBarcode &&
                   skuDescription == property.skuDescription &&
                   orderQty == property.orderQty &&
                   cutQty == property.cutQty &&
                   criticalItemComment == property.criticalItemComment &&
                   possibleSwitch == property.possibleSwitch &&
                   possibleSwitchDescription == property.possibleSwitchDescription &&
                   afterReleaseRejection == property.afterReleaseRejection;
        }

        public override int GetHashCode() {
            var hashCode = -1898700883;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loadingDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuDescription);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(criticalItemComment);
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(possibleSwitchDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(afterReleaseRejection);
            return hashCode;
        }
    }
}
