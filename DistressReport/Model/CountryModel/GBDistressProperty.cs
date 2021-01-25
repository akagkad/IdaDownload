using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class GBDistressProperty {
        [Column("[PO Date]")] public string poDate { get; set; }
        [Column("[RDD]")] public string rdd { get; set; }
        [Column("[Order]")] public int order { get; set; }
        [Column("[PO Number]")] public string poNumber { get; set; }
        [Column("[Shipto Name]")] public string shipToName { get; set; }
        [Column("[Customer Code]")] public string cmir { get; set; }
        [Column("[Material]")] public int material { get; set; }
        [Column("[Material Description]")] public string materialDescription { get; set; }
        [Column("[Order Qty]")] public double orderQty { get; set; }
        [Column("[Confirmed Qty]")] public double confirmedQty { get; set; }
        [Column("[Cut Qty]")] public double cutQty { get; set; }
        [Column("[Recovery Date]")] public string recoveryDate { get; set; }
        [Column("[Recovery Qty]")] public double recoveryQty { get; set; }
        [Column("[Comment]")] public string comment { get; set; }
        [Column("[item]")] public int item { get; set; }
        [Column("[Delivery Block]")] public string deliveryBlock { get; set; }
        [Column("[Possible Switch]")] public int possibleSwitch { get; set; }
        [Column("[Possible Switch Description]")] public string possibleSwitchDescription { get; set; }
        [Column("[Case Barcode]")] public string caseBarcode { get; set; }
        [Column("[Unit Barcode]")] public string unitBarcode { get; set; }
        [Column("[ATP Qty]")] public double atpQty { get; set; }
        [Column("[routeCode]")] public string routeCode { get; set; }

        public GBDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.poDate = genericDistressProperty.poDate;
            this.rdd = genericDistressProperty.rdd;
            this.order = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.shipToName = genericDistressProperty.shipToName;
            this.cmir = genericDistressProperty.cmir;
            this.material = genericDistressProperty.material;
            this.materialDescription = genericDistressProperty.materialDescription;
            this.orderQty = genericDistressProperty.orderQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.recoveryQty = genericDistressProperty.recoveryQty;
            this.comment = genericDistressProperty.criticalItemComment;
            this.item = genericDistressProperty.item;
            this.deliveryBlock = genericDistressProperty.deliveryBlock;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.possibleSwitchDescription = genericDistressProperty.possibleSwitchDescription;
            this.caseBarcode = genericDistressProperty.caseBarcode;
            this.unitBarcode = genericDistressProperty.unitBarcode;
            this.atpQty = genericDistressProperty.atp;
            this.routeCode = genericDistressProperty.routeCode;
        }

        public override bool Equals(object obj) {
            return obj is GBDistressProperty property &&
                   poDate == property.poDate &&
                   rdd == property.rdd &&
                   order == property.order &&
                   poNumber == property.poNumber &&
                   shipToName == property.shipToName &&
                   cmir == property.cmir &&
                   material == property.material &&
                   materialDescription == property.materialDescription &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty &&
                   comment == property.comment &&
                   item == property.item &&
                   deliveryBlock == property.deliveryBlock &&
                   possibleSwitch == property.possibleSwitch &&
                   possibleSwitchDescription == property.possibleSwitchDescription &&
                   caseBarcode == property.caseBarcode &&
                   unitBarcode == property.unitBarcode &&
                   atpQty == property.atpQty &&
                   routeCode == property.routeCode;
        }

        public override int GetHashCode() {
            var hashCode = -1669147651;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(cmir);
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comment);
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryBlock);
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(possibleSwitchDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(caseBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitBarcode);
            hashCode = hashCode * -1521134295 + atpQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(routeCode);
            return hashCode;
        }
    }
}
