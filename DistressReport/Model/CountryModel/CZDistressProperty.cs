using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class CZDistressProperty {
        [Column("[Delivery Date]")] public string rdd { get; set; }
        [Column("[Order Number]")] public int orderNumber { get; set; }
        [Column("[PO Number]")] public string poNumber { get; set; }
        [Column("[Customer Name]")] public string customerName { get; set; }   //ship to name 
        [Column("[SKU]")] public int sku { get; set; }
        [Column("[Material Description]")] public string materialDescription { get; set; }
        [Column("[Comments]")] public string comments { get; set; }
        [Column("[Currently Rejected]")] public string rejReason { get; set; }
        [Column("[After Last Release Rejection]")] public string rejAfterRelease { get; set; }
        [Column("[Delivery Block]")] public string deliveryBlock { get; set; }
        [Column("[Recovery Date]")] public string recoveryDate { get; set; }
        [Column("[Order QTY]")] public double orderQty { get; set; }
        [Column("[Confirmed QTY]")] public double confirmedQty { get; set; }
        [Column("[Cut QTY]")] public double cutQty { get; set; }

        public CZDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.rdd = genericDistressProperty.rdd;
            this.orderNumber = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.customerName = genericDistressProperty.shipToName;
            this.sku = genericDistressProperty.material;
            this.materialDescription = genericDistressProperty.materialDescription;
            this.comments = genericDistressProperty.criticalItemComment;
            this.rejAfterRelease = genericDistressProperty.afterReleaseRej;
            this.rejReason = genericDistressProperty.rejReason;
            this.deliveryBlock = genericDistressProperty.deliveryBlock;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.orderQty = genericDistressProperty.orderQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.cutQty = genericDistressProperty.cutQty;
        }

        public override bool Equals(object obj) {
            return obj is CZDistressProperty property &&
                   rdd == property.rdd &&
                   orderNumber == property.orderNumber &&
                   poNumber == property.poNumber &&
                   customerName == property.customerName &&
                   sku == property.sku &&
                   materialDescription == property.materialDescription &&
                   comments == property.comments &&
                   rejReason == property.rejReason &&
                   rejAfterRelease == property.rejAfterRelease &&
                   deliveryBlock == property.deliveryBlock &&
                   recoveryDate == property.recoveryDate &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty;
        }

        public override int GetHashCode() {
            var hashCode = 144158616;
            hashCode = hashCode * -1521134295 + rdd.GetHashCode();
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(customerName);
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comments);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejAfterRelease);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryBlock);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            return hashCode;
        }
    }
}
