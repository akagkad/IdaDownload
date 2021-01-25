using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class RODistressProperty {
        [Column("[Material]")] public int material { get; set; }
        [Column("[Order Number]")] public int orderNumber { get; set; }
        [Column("[Item]")] public int item { get; set; }
        [Column("[Material Description]")] public string materialDescription { get; set; }
        [Column("[Sold To]")] public int soldTo { get; set; }
        [Column("[Sold To Name]")] public string soldToName { get; set; }
        [Column("[Ship To]")] public int shipTo { get; set; }
        [Column("[Ship To Name]")] public string shipToName { get; set; }
        [Column("[Reason for Rejection]")] public string rejReason { get; set; }
        [Column("[Possible Switch]")] public int possibleSwitch { get; set; }
        [Column("[Possible Switch Description]")] public string possibleSwitchDescription { get; set; }
        [Column("[Delivery Block]")] public string deliveryBlock { get; set; }
        [Column("[ATP Qty]")] public double atpQty { get; set; }
        [Column("[D Chain Status]")] public string dChainStatus { get; set; }
        [Column("[PO Date]")] public string poDate { get; set; }
        [Column("[Loading Date]")] public string loadingDate { get; set; }
        [Column("[RDD]")] public string rdd { get; set; }
        [Column("[Recovery Date]")] public string recoveryDate { get; set; }
        [Column("[Recovery Qty]")] public double recoveryQty { get; set; }
        [Column("[Order Qty]")] public double orderQty { get; set; }
        [Column("[Confirmed Qty]")] public double confirmedQty { get; set; }
        [Column("[Cut Qty]")] public double cutQty { get; set; }
        [Column("[Delivery Priority]")] public string delPriority { get; set; }

        public RODistressProperty(GenericDistressProperty genericDistressProperty) {
            material = genericDistressProperty.material;
            orderNumber = genericDistressProperty.order;
            item = genericDistressProperty.item;
            materialDescription = genericDistressProperty.materialDescription;
            soldTo = genericDistressProperty.soldTo;
            soldToName = genericDistressProperty.soldToName;
            shipTo = genericDistressProperty.shipTo;
            shipToName = genericDistressProperty.shipToName;
            rejReason = genericDistressProperty.rejReason;
            possibleSwitch = genericDistressProperty.possibleSwitch;
            possibleSwitchDescription = genericDistressProperty.possibleSwitchDescription;
            deliveryBlock = genericDistressProperty.deliveryBlock;
            atpQty = genericDistressProperty.atp;
            recoveryDate = genericDistressProperty.recoveryDate;
            recoveryQty = genericDistressProperty.recoveryQty;
            dChainStatus = genericDistressProperty.dChainStatus;
            poDate = genericDistressProperty.poDate;
            rdd = genericDistressProperty.rdd;
            orderQty = genericDistressProperty.orderQty;
            confirmedQty = genericDistressProperty.confirmedQty;
            cutQty = genericDistressProperty.cutQty;
            delPriority = genericDistressProperty.delPriority;
            loadingDate = genericDistressProperty.loadingDate;
        }

        public override bool Equals(object obj) {
            return obj is RODistressProperty property &&
                   material == property.material &&
                   orderNumber == property.orderNumber &&
                   item == property.item &&
                   materialDescription == property.materialDescription &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   rejReason == property.rejReason &&
                   possibleSwitch == property.possibleSwitch &&
                   possibleSwitchDescription == property.possibleSwitchDescription &&
                   deliveryBlock == property.deliveryBlock &&
                   atpQty == property.atpQty &&
                   dChainStatus == property.dChainStatus &&
                   poDate == property.poDate &&
                   loadingDate == property.loadingDate &&
                   rdd == property.rdd &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty &&
                   delPriority == property.delPriority;
        }

        public override int GetHashCode() {
            var hashCode = 1613432682;
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(possibleSwitchDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryBlock);
            hashCode = hashCode * -1521134295 + atpQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dChainStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loadingDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delPriority);
            return hashCode;
        }
    }
}
