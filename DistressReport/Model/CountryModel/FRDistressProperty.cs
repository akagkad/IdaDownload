using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace DistressReport.Model {
    class FRDistressProperty {
        [Column("[Commande SCJ]")] public int order { get; set; }
        [Column("[Num de client]")] public string poNumber { get; set; }
        [Column("[DATE TRANSMISSION]")] public string loadingDate { get; set; }
        [Column("[Date livraison]")] public string rdd { get; set; }
        [Column("[Entrepôt]")] public string shipToName { get; set; }
        [Column("[SKU SCJ]")] public int sku { get; set; }
        [Column("[SKU SCJ Status]")] public string skuStatus { get; set; }
        [Column("[EAN]")] public string unitBarcode { get; set; }
        [Column("[Description]")] public string materialDescription { get; set; }
        [Column("[Code de rejet]")] public string rejReason { get; set; }
        [Column("[Switch sur SKU]")] public int possibleSwitch { get; set; }
        [Column("[Type de switch]")] public string switchType { get; set; }
        [Column("[Code de blocage commande]")] public string deliveryBlock { get; set; }
        [Column("[Stock]")] public double atp { get; set; }
        [Column("[Commentaire]")] public string comments { get; set; }
        [Column("[date retour en stock]")] public string recoveryDate { get; set; }
        [Column("[quantité prévue]")] public double recoveryQty { get; set; }
        [Column("[quantité commandée]")] public double orderQty { get; set; }
        [Column("[Colis en rupture]")] public double cutQty { get; set; }
        [Column("[Commercial]")] public string accountManager { get; set; }
        [Column("[Standard or Repack]")] public string standardOrRepack { get; set; }

        public FRDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.order = genericDistressProperty.order;
            this.poNumber = genericDistressProperty.poNumber;
            this.loadingDate = genericDistressProperty.loadingDate;
            this.rdd = genericDistressProperty.rdd;
            this.shipToName = genericDistressProperty.shipToName;
            this.sku = genericDistressProperty.material;
            this.skuStatus = genericDistressProperty.dChainStatus;
            this.unitBarcode = genericDistressProperty.unitBarcode;
            this.materialDescription = genericDistressProperty.materialDescription;
            this.rejReason = genericDistressProperty.rejReason;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.switchType = genericDistressProperty.switchComment;
            this.deliveryBlock = genericDistressProperty.deliveryBlock;
            this.atp = genericDistressProperty.atp;
            this.comments = genericDistressProperty.criticalItemComment;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.recoveryQty = genericDistressProperty.recoveryQty;
            this.orderQty = genericDistressProperty.orderQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.accountManager = genericDistressProperty.accountManager;
            this.standardOrRepack = genericDistressProperty.starndardOrRepack;
        }

        public override bool Equals(object obj) {
            return obj is FRDistressProperty property &&
                   order == property.order &&
                   poNumber == property.poNumber &&
                   loadingDate == property.loadingDate &&
                   rdd == property.rdd &&
                   shipToName == property.shipToName &&
                   sku == property.sku &&
                   skuStatus == property.skuStatus &&
                   unitBarcode == property.unitBarcode &&
                   materialDescription == property.materialDescription &&
                   rejReason == property.rejReason &&
                   possibleSwitch == property.possibleSwitch &&
                   switchType == property.switchType &&
                   deliveryBlock == property.deliveryBlock &&
                   atp == property.atp &&
                   comments == property.comments &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty &&
                   orderQty == property.orderQty &&
                   cutQty == property.cutQty &&
                   accountManager == property.accountManager &&
                   standardOrRepack == property.standardOrRepack;
        }

        public override int GetHashCode() {
            var hashCode = 1664318609;
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loadingDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(switchType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryBlock);
            hashCode = hashCode * -1521134295 + atp.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comments);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(accountManager);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(standardOrRepack);
            return hashCode;
        }
    }
}
