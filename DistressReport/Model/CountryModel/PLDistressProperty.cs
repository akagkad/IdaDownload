using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class PLDistressProperty {
        [Column("[Data zaladunku]")] public string poDate { get; set; }
        [Column("[Data Dostawy]")] public string rdd { get; set; }
        [Column("[SAP nr zamowienia]")] public int orderNumber { get; set; }
        [Column("[Nr zamowienia klienta]")] public string poNumber { get; set; }
        [Column("[Ship to]")] public int shipTo { get; set; }
        [Column("[Ship to opis]")] public string shipToName { get; set; }
        [Column("[SKU Number]")] public int sku { get; set; }
        [Column("[SKU opis]")] public string skuDesc { get; set; }
        [Column("[Case Barcode]")] public string caseBarcode { get; set; }
        [Column("[Unit Barcode]")] public string unitBarcode { get; set; }
        [Column("[Ilosc zamowiona]")] public double orderQty { get; set; }
        [Column("[Ilosc potwierdzona]")] public double confirmedQty { get; set; }
        [Column("[Cut qty]")] public double cutQty { get; set; }
        [Column("[Blokada]")] public string rejReason { get; set; }
        [Column("[Blokada after release]")] public string rejReasonAfterRelease { get; set; }
        [Column("[Possible Switch]")] public int possibleSwitch { get; set; }
        [Column("[Recovery date]")] public string recoveryDate { get; set; }
        [Column("[Komentarz]")] public string comments { get; set; }

        public PLDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.poDate = genericDistressProperty.poDate;
            this.rdd = genericDistressProperty.rdd;
            this.orderNumber = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.shipTo = genericDistressProperty.shipTo;
            this.shipToName = genericDistressProperty.shipToName;
            this.sku = genericDistressProperty.material;
            this.skuDesc = genericDistressProperty.materialDescription;
            this.caseBarcode = genericDistressProperty.caseBarcode;
            this.unitBarcode = genericDistressProperty.unitBarcode;
            this.orderQty = genericDistressProperty.orderQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.rejReason = genericDistressProperty.rejReason;
            this.rejReasonAfterRelease = genericDistressProperty.afterReleaseRej;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.comments = genericDistressProperty.criticalItemComment;
        }

        public override bool Equals(object obj) {
            return obj is PLDistressProperty property &&
                   poDate == property.poDate &&
                   rdd == property.rdd &&
                   orderNumber == property.orderNumber &&
                   poNumber == property.poNumber &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   sku == property.sku &&
                   skuDesc == property.skuDesc &&
                   caseBarcode == property.caseBarcode &&
                   unitBarcode == property.unitBarcode &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty &&
                   rejReason == property.rejReason &&
                   rejReasonAfterRelease == property.rejReasonAfterRelease &&
                   possibleSwitch == property.possibleSwitch &&
                   recoveryDate == property.recoveryDate &&
                   comments == property.comments;
        }

        public override int GetHashCode() {
            var hashCode = -1013757642;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuDesc);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(caseBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitBarcode);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReasonAfterRelease);
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comments);
            return hashCode;
        }
    }
}
