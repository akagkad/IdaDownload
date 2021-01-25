using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.TaskProperty {
    public class RejectionsProperty {
        #region Properties
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[rejectionForCustomer]")] public string rejectionForCustomer { get; set; }
        [Column("[isReplacePartialCut]")] public bool isReplacePartialCut { get; set; }
        [Column("[isDuringRelease]")] public bool isDuringRelease { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[orderNumber]")] public int orderNumber { get; set; }
        [Column("[item]")] public int item { get; set; }
        [Column("[sku]")] public int sku { get; set; }
        [Column("[rejectionReasonCode]")] public string rejectionReasonCode { get; set; }
        [Column("[orderedQty]")] public double orderedQty { get; set; }
        [Column("[confirmedQty]")] public double confirmedQty { get; set; }
        [Column("[skuUnitBarcode]")] public string skuUnitBarcode { get; set; }
        [Column("[skuCaseBarcode]")] public string skuCaseBarcode { get; set; }
        [Column("skuATP]")] public double skuATP { get; set; }
        [Column("[skuRecoveryDate]")] public string skuRecoveryDate { get; set; }
        [Column("[skuRecoveryQty]")] public double skuRecoveryQty { get; set; }
        [Column("[startDate]")] public DateTime startDate { get; set; }
        [Column("[endDate]")] public DateTime endDate { get; set; }
        [Column("[needOutOfStockToReject]")] public bool needOutOfStockToReject { get; set; }
        [Column("[rejectionComment]")] public string rejectionComment { get; set; }
        [Column("[id]")] public string id { get; set; }
        #endregion

        public RejectionsProperty() { }

        public RejectionsProperty(string salesOrg,
                                  string country,
                                  int soldTo,
                                  string rejectionForCustomer,
                                  bool isReplacePartialCut,
                                  bool isDuringRelease,
                                  int shipTo,
                                  string shipToName,
                                  int orderNumber,
                                  int item,
                                  int sku,
                                  string rejectionReasonCode,
                                  double orderedQty,
                                  double confirmedQty,
                                  string skuUnitBarcode,
                                  string skuCaseBarcode,
                                  int skuATP,
                                  string skuRecoveryDate,
                                  int skuRecoveryQty,
                                  DateTime startDate,
                                  DateTime endDate,
                                  bool needOutOfStockToReject,
                                  string rejectionComment,
                                  string id) {
            this.salesOrg = salesOrg;
            this.country = country;
            this.soldTo = soldTo;
            this.rejectionForCustomer = rejectionForCustomer;
            this.isReplacePartialCut = isReplacePartialCut;
            this.isDuringRelease = isDuringRelease;
            this.shipTo = shipTo;
            this.shipToName = shipToName;
            this.orderNumber = orderNumber;
            this.item = item;
            this.sku = sku;
            this.rejectionReasonCode = rejectionReasonCode;
            this.orderedQty = orderedQty;
            this.confirmedQty = confirmedQty;
            this.skuUnitBarcode = skuUnitBarcode;
            this.skuCaseBarcode = skuCaseBarcode;
            this.skuATP = skuATP;
            this.skuRecoveryDate = skuRecoveryDate;
            this.skuRecoveryQty = skuRecoveryQty;
            this.startDate = startDate;
            this.endDate = endDate;
            this.needOutOfStockToReject = needOutOfStockToReject;
            this.rejectionComment = rejectionComment;
            this.id = id;
        }
        public override bool Equals(object obj) {
            return obj is RejectionsProperty property &&
                   salesOrg == property.salesOrg &&
                   country == property.country &&
                   soldTo == property.soldTo &&
                   rejectionForCustomer == property.rejectionForCustomer &&
                   isReplacePartialCut == property.isReplacePartialCut &&
                   isDuringRelease == property.isDuringRelease &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   orderNumber == property.orderNumber &&
                   item == property.item &&
                   sku == property.sku &&
                   rejectionReasonCode == property.rejectionReasonCode &&
                   orderedQty == property.orderedQty &&
                   confirmedQty == property.confirmedQty &&
                   skuUnitBarcode == property.skuUnitBarcode &&
                   skuCaseBarcode == property.skuCaseBarcode &&
                   skuATP == property.skuATP &&
                   skuRecoveryDate == property.skuRecoveryDate &&
                   skuRecoveryQty == property.skuRecoveryQty &&
                   startDate == property.startDate &&
                   endDate == property.endDate &&
                   needOutOfStockToReject == property.needOutOfStockToReject &&
                   rejectionComment == property.rejectionComment &&
                   id == property.id;
        }
        public override int GetHashCode() {
            var hashCode = 889723991;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesOrg);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejectionForCustomer);
            hashCode = hashCode * -1521134295 + isReplacePartialCut.GetHashCode();
            hashCode = hashCode * -1521134295 + isDuringRelease.GetHashCode();
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + sku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejectionReasonCode);
            hashCode = hashCode * -1521134295 + orderedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuUnitBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuCaseBarcode);
            hashCode = hashCode * -1521134295 + skuATP.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(skuRecoveryDate);
            hashCode = hashCode * -1521134295 + skuRecoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + startDate.GetHashCode();
            hashCode = hashCode * -1521134295 + endDate.GetHashCode();
            hashCode = hashCode * -1521134295 + needOutOfStockToReject.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejectionComment);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(id);
            return hashCode;
        }
    }
}