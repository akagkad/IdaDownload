using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class DEDistressProperty {
        [Column("[Country]")] public string country { get; set; }
        [Column("[Order Number]")] public int orderNumber { get; set; }
        [Column("[PO Number]")] public string poNumber { get; set; }
        [Column("[RDD]")] public string rdd { get; set; }
        [Column("[Ship to]")] public int shipTo { get; set; }
        [Column("[Ship to Name]")] public string shipToName { get; set; }
        [Column("[Material]")] public int material { get; set; }
        [Column("[Material Description]")] public string materialDescription { get; set; }
        [Column("[Unit Barcode]")] public string unitBarcode { get; set; }
        [Column("[Case Barcode]")] public string caseBarcode { get; set; }
        [Column("[Order Qty]")] public double orderQty { get; set; }
        [Column("[Confirmed Qty]")] public double confirmedQty { get; set; }
        [Column("[Cut Qty]")] public double cutQty { get; set; }
        [Column("[Recovery Date]")] public string recoveryDate { get; set; }
        [Column("[Recovery Qty]")] public double recoveryQty { get; set; }
        [Column("[D chain]")] public string dChainStatus { get; set; }
        [Column("[ATP Qty]")] public double atpQty { get; set; }
        [Column("[Possible Switch]")] public int possibleSwitch { get; set; }
        [Column("[Possible Switch Description]")] public string possibleSwitchDesc { get; set; }
        [Column("[Delivery Block]")] public string delBlock { get; set; }
        [Column("[Comments]")] public string comments { get; set; }

        public DEDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.country = genericDistressProperty.country;
            this.orderNumber = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.rdd = genericDistressProperty.rdd;
            this.shipTo = genericDistressProperty.shipTo;
            this.shipToName = genericDistressProperty.shipToName;
            this.material = genericDistressProperty.material;
            this.materialDescription = genericDistressProperty.materialDescription;
            this.unitBarcode = genericDistressProperty.unitBarcode;
            this.caseBarcode = genericDistressProperty.caseBarcode;
            this.orderQty = genericDistressProperty.orderQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.recoveryQty = genericDistressProperty.recoveryQty;
            this.dChainStatus = genericDistressProperty.dChainStatus;
            this.atpQty = genericDistressProperty.atp;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.possibleSwitchDesc = genericDistressProperty.possibleSwitchDescription;
            this.delBlock = genericDistressProperty.deliveryBlock;
            this.comments = genericDistressProperty.criticalItemComment;
        }

        public override bool Equals(object obj) {
            return obj is DEDistressProperty property &&
                   country == property.country &&
                   orderNumber == property.orderNumber &&
                   poNumber == property.poNumber &&
                   rdd == property.rdd &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   material == property.material &&
                   materialDescription == property.materialDescription &&
                   unitBarcode == property.unitBarcode &&
                   caseBarcode == property.caseBarcode &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty &&
                   dChainStatus == property.dChainStatus &&
                   atpQty == property.atpQty &&
                   possibleSwitch == property.possibleSwitch &&
                   possibleSwitchDesc == property.possibleSwitchDesc &&
                   delBlock == property.delBlock &&
                   comments == property.comments;
        }

        public override int GetHashCode() {
            var hashCode = 2138344132;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(caseBarcode);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dChainStatus);
            hashCode = hashCode * -1521134295 + atpQty.GetHashCode();
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(possibleSwitchDesc);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delBlock);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comments);
            return hashCode;
        }
    }
}
