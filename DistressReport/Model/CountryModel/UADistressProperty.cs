using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class UADistressProperty {
        [Column("[Sold To Number]")] public int soldTo { get; set; }
        [Column("[Ship To Name]")] public string shipToName { get; set; }
        [Column("[Order Number]")] public int orderNumber { get; set; }
        [Column("[PO Number]")] public string poNumber { get; set; }
        [Column("[Material Number]")] public int sku { get; set; }
        [Column("[Material Description]")] public string materialDescription { get; set; }
        [Column("[Order QTY]")] public double orderQty { get; set; }
        [Column("[Confirmed QTY]")] public double confirmedQty { get; set; }
        [Column("[Cut QTY]")] public double cutQty { get; set; }
        [Column("[RRC]")] public string rejReason { get; set; }
        [Column("[After Release RRC]")] public string afterReleaseRejReason { get; set; }
        [Column("[Possible Switch]")] public int possibleSwitch { get; set; }
        [Column("[Delivery Block]")] public string deliveryBlock { get; set; }
        [Column("[ATP]")] public double atp { get; set; }
        [Column("[D Chain]")] public string dChain { get; set; }

        public UADistressProperty(GenericDistressProperty genericDistressProperty) {
            this.soldTo = genericDistressProperty.soldTo;
            this.shipToName = genericDistressProperty.shipToName;
            this.orderNumber = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.sku = genericDistressProperty.material;
            this.materialDescription = genericDistressProperty.materialDescription;
            this.orderQty = genericDistressProperty.orderQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.rejReason = genericDistressProperty.rejReason;
            this.afterReleaseRejReason = genericDistressProperty.afterReleaseRej;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.deliveryBlock = genericDistressProperty.possibleSwitchDescription;
            this.atp = genericDistressProperty.atp;
            this.dChain = genericDistressProperty.dChainStatus;
        }

        public override bool Equals(object obj) {
            return obj is UADistressProperty property &&
                   soldTo == property.soldTo &&
                   shipToName == property.shipToName &&
                   orderNumber == property.orderNumber &&
                   poNumber == property.poNumber &&
                   sku == property.sku &&
                   materialDescription == property.materialDescription &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty &&
                   rejReason == property.rejReason &&
                   afterReleaseRejReason == property.afterReleaseRejReason &&
                   possibleSwitch == property.possibleSwitch &&
                   deliveryBlock == property.deliveryBlock &&
                   atp == property.atp &&
                   dChain == property.dChain;
        }

        public override int GetHashCode() {
            var hashCode = 1894250171;
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(afterReleaseRejReason);
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryBlock);
            hashCode = hashCode * -1521134295 + atp.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dChain);
            return hashCode;
        }
    }
}
