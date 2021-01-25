using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    class TRDistressProperty {
        [Column("[Siparis Durumu__Status]")] public string orderStatus { get; set; }
        [Column("[Siparis Yansima Tarihi__PO Date]")] public string poDate { get; set; }
        [Column("[Teslimat Tarihi__Req Del Date]")] public string rdd { get; set; }
        [Column("[Blokaj Bekletilme Kodu__Del Block]")] public string deliveryBlock { get; set; }
        [Column("[DHLe aktarilma tarihi__Loading Date]")] public string loadingDate { get; set; }
        [Column("[Musteri Numarasi__Sold To]")] public int soldTo { get; set; }
        [Column("[Musteri Ismi__Sold To Name]")] public string soldToName { get; set; }
        [Column("[Depo Numarasi__Ship To]")] public int shipTo { get; set; }
        [Column("[Depo Ismi __Ship To Name]")] public string shipToName { get; set; }
        [Column("[Musteri Siparis Numarasi __PO]")] public string poNumber { get; set; }
        [Column("[SCJ Siparis Numarasi__Order]")] public int orderNumber { get; set; }
        [Column("[Urun__SKU]")] public int sku { get; set; }
        [Column("[Urun Tanimi__SKU Desc]")] public string skuDescription { get; set; }
        [Column("[Talep miktari__Order Qty]")] public double orderQty { get; set; }
        [Column("[Onaylanan miktar__Conf Qty]")] public double confirmedQty { get; set; }
        [Column("[Karsilanamayan miktar__Cut Qty]")] public double cutQty { get; set; }
        [Column("[Yansidktan sonra iptal edilecekler_After Release Rejection]")] public string afterReleaseRejection { get; set; }
        [Column("[Yorum__Comment]")] public string comment { get; set; }
        [Column("[Tahmini stoga giris tarihi__Rec Date]")] public string recoveryDate { get; set; }
        [Column("[Tahmini stoga giris miktari__Rec Qty]")] public double recoveryQty { get; set; }
        [Column("[Satilabilir stok miktari__ATP]")] public double qtpQty { get; set; }
        [Column("[Urun kod degisikligi__Poss Switch]")] public int possibleSwitch { get; set; }
        [Column("[Yeni kod tanimi__Poss Switch Desc]")] public string possibleSwitchDesc { get; set; }
        [Column("[Urun durumu__DChain]")] public string dChainStatus { get; set; }

        public TRDistressProperty(GenericDistressProperty genericDistressProperty) {
            this.orderStatus = genericDistressProperty.orderStatus;
            this.poDate = genericDistressProperty.poDate;
            this.rdd = genericDistressProperty.rdd;
            this.deliveryBlock = genericDistressProperty.deliveryBlock;
            this.loadingDate = genericDistressProperty.loadingDate;
            this.soldTo = genericDistressProperty.soldTo;
            this.soldToName = genericDistressProperty.soldToName;
            this.shipTo = genericDistressProperty.shipTo;
            this.shipToName = genericDistressProperty.shipToName;
            this.poNumber = genericDistressProperty.poNumber;
            this.orderNumber = genericDistressProperty.order;
            this.sku = genericDistressProperty.material;
            this.skuDescription = genericDistressProperty.materialDescription;
            this.orderQty = genericDistressProperty.orderQty;
            this.confirmedQty = genericDistressProperty.confirmedQty;
            this.cutQty = genericDistressProperty.cutQty;
            this.afterReleaseRejection = genericDistressProperty.afterReleaseRej;
            this.comment = genericDistressProperty.criticalItemComment;
            this.recoveryDate = genericDistressProperty.recoveryDate;
            this.recoveryQty = genericDistressProperty.recoveryQty;
            this.qtpQty = genericDistressProperty.atp;
            this.possibleSwitch = genericDistressProperty.possibleSwitch;
            this.possibleSwitchDesc = genericDistressProperty.possibleSwitchDescription;
            this.dChainStatus = genericDistressProperty.dChainStatus;
        }

        public override bool Equals(object obj) {
            return obj is TRDistressProperty property &&
                   orderStatus == property.orderStatus &&
                   poDate == property.poDate &&
                   rdd == property.rdd &&
                   deliveryBlock == property.deliveryBlock &&
                   loadingDate == property.loadingDate &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   poNumber == property.poNumber &&
                   orderNumber == property.orderNumber &&
                   sku == property.sku &&
                   skuDescription == property.skuDescription &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty &&
                   afterReleaseRejection == property.afterReleaseRejection &&
                   comment == property.comment &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty &&
                   qtpQty == property.qtpQty &&
                   possibleSwitch == property.possibleSwitch &&
                   possibleSwitchDesc == property.possibleSwitchDesc &&
                   dChainStatus == property.dChainStatus;
        }

        public override int GetHashCode() {
            var hashCode = 1339624184;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(orderStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryBlock);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loadingDate);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuDescription);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(afterReleaseRejection);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(comment);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + qtpQty.GetHashCode();
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(possibleSwitchDesc);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dChainStatus);
            return hashCode;
        }
    }
}
