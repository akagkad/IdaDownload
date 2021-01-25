using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    /// <summary>
    /// Model for ZA01, NG01 and KE02
    /// </summary>
    class AfricanDistressProperty {
        [Column("[Delivery Block]")] public string delBlock { get; set; }
        [Column("[Order Number]")] public int orderNumber { get; set; }
        [Column("[PO Number]")] public string poNumber { get; set; }
        [Column("[Ship to Name]")] public string shipToName { get; set; }
        [Column("[RDD]")] public string rdd { get; set; }
        [Column("[SKU]")] public int sku { get; set; }
        [Column("[SKU Description]")] public string skuDescription { get; set; }
        [Column("[D chain]")] public string dChainStatus { get; set; }
        [Column("[Order Qty]")] public double orderQty { get; set; }
        [Column("[Cut Qty]")] public double cutQty { get; set; }
        [Column("[Qty available to deliver]")] public double confirmedQty { get; set; }
        [Column("[Comments]")] public string comments { get; set; }
        [Column("[ATP Qty]")] public double atpQty { get; set; }
        [Column("[Recovery Date]")] public string recoveryDate { get; set; }
        [Column("[Recovery Qty]")] public double recoveryQty { get; set; }

        public AfricanDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.orderNumber = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.shipToName = genericDistressProperty.shipToName;
            this.rdd = genericDistressProperty.rdd;
            this.sku = genericDistressProperty.material;
            this.skuDescription = genericDistressProperty.materialDescription;
            this.orderQty = genericDistressProperty.orderQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.comments = genericDistressProperty.criticalItemComment;
            this.atpQty = genericDistressProperty.atp;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.recoveryQty = genericDistressProperty.recoveryQty;
            this.dChainStatus = genericDistressProperty.dChainStatus;
            this.delBlock = genericDistressProperty.deliveryBlock;
        }

        public override bool Equals(object obj) {
            return obj is AfricanDistressProperty property &&
                   delBlock == property.delBlock &&
                   orderNumber == property.orderNumber &&
                   poNumber == property.poNumber &&
                   shipToName == property.shipToName &&
                   rdd == property.rdd &&
                   sku == property.sku &&
                   skuDescription == property.skuDescription &&
                   dChainStatus == property.dChainStatus &&
                   orderQty == property.orderQty &&
                   cutQty == property.cutQty &&
                   confirmedQty == property.confirmedQty &&
                   comments == property.comments &&
                   atpQty == property.atpQty &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty;
        }

        public override int GetHashCode() {
            var hashCode = -1588623877;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delBlock);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dChainStatus);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comments);
            hashCode = hashCode * -1521134295 + atpQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            return hashCode;
        }
    }
}
