using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.TaskProperty {
    public class SwitchesProperty {
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[switchForCustomer]")] public string switchForCustomer { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[orderNumber]")] public int order { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[item]")] public int item { get; set; }
        [Column("[orderedQty]")] public double orderedQty { get; set; }
        [Column("[confirmedQty]")] public double confirmedQty { get; set; }
        [Column("[oldSku]")] public int oldSku { get; set; }
        [Column("[oldSkuDescription]")] public string oldSkuDescription { get; set; }
        [Column("[oldSkuCaseBarcode]")] public string oldSkuCaseBarcode { get; set; }
        [Column("[oldSkuUnitBarcode]")] public string oldSkuUnitBarcode { get; set; }
        [Column("[oldSkuATP]")] public double oldSkuATP { get; set; }
        [Column("[oldSkuRecoveryDate]")] public string oldSkuRecoveryDate { get; set; }
        [Column("[oldSkuRecoveryQty]")] public double oldSkuRecoveryQty { get; set; }
        [Column("[newSku]")] public int newSku { get; set; }
        [Column("[newSkuDescription]")] public string newSkuDescription { get; set; }
        [Column("[newSkuCaseBarcode]")] public string newSkuCaseBarcode { get; set; }
        [Column("[newSkuUnitBarcode]")] public string newSkuUnitBarcode { get; set; }
        [Column("[newSkuATP]")] public double newSkuATP { get; set; }
        [Column("[newSkuRecoveryDate]")] public string newSkuRecoveryDate { get; set; }
        [Column("[newSkuRecoveryQty]")] public double newSkuRecoveryQty { get; set; }
        [Column("[startDate]")] public DateTime startDate { get; set; }
        [Column("[endDate]")] public DateTime endDate { get; set; }
        [Column("[needOutOfStockToSwitch]")] public bool needOutOfStockToSwitch { get; set; }
        [Column("[SwitchAutomatic]")] public bool switchAutomatic { get; set; }
        [Column("[switchComment]")] public string switchComment { get; set; }
        [Column("[id]")] public string id { get; set; }

        public SwitchesProperty() {
        }

        public SwitchesProperty(string salesOrg, string country, int soldTo, string switchForCustomer, int shipTo, int order, string shipToName, int item, double orderedQty, double confirmedQty, int oldSku, string oldSkuDescription , string oldSkuCaseBarcode, string oldSkuUnitBarcode, int oldSkuATP, DateTime oldSkuRecoveryDate, int oldSkuRecoveryQty, int newSku, string newSkuDescription , string newSkuCaseBarcode, string newSkuUnitBarcode, int newSkuATP, DateTime newSkuRecoveryDate, int newSkuRecoveryQty, DateTime startDate, DateTime endDate, bool needOutOfStockToSwitch, bool switchAutomatic, string switchComment, string id) {
            this.salesOrg = salesOrg;
            this.country = country;
            this.soldTo = soldTo;
            this.switchForCustomer = switchForCustomer;
            this.shipTo = shipTo;
            this.order = order;
            this.shipToName = shipToName;
            this.item = item;
            this.orderedQty = orderedQty;
            this.confirmedQty = confirmedQty;
            this.oldSku = oldSku;
            this.oldSkuDescription = oldSkuDescription;
            this.oldSkuCaseBarcode = oldSkuCaseBarcode;
            this.oldSkuUnitBarcode = oldSkuUnitBarcode;
            this.oldSkuATP = oldSkuATP;
            this.oldSkuRecoveryDate = Conversions.ToString(oldSkuRecoveryDate);
            this.oldSkuRecoveryQty = oldSkuRecoveryQty;
            this.newSku = newSku;
            this.newSkuDescription = newSkuDescription;
            this.newSkuCaseBarcode = newSkuCaseBarcode;
            this.newSkuUnitBarcode = newSkuUnitBarcode;
            this.newSkuATP = newSkuATP;
            this.newSkuRecoveryDate = Conversions.ToString(newSkuRecoveryDate);
            this.newSkuRecoveryQty = newSkuRecoveryQty;
            this.startDate = startDate;
            this.endDate = endDate;
            this.needOutOfStockToSwitch = needOutOfStockToSwitch;
            this.switchAutomatic = switchAutomatic;
            this.switchComment = switchComment;
            this.id = id;
        }

        public override bool Equals(object obj) {
            return obj is SwitchesProperty property &&
                   salesOrg == property.salesOrg &&
                   country == property.country &&
                   soldTo == property.soldTo &&
                   switchForCustomer == property.switchForCustomer &&
                   shipTo == property.shipTo &&
                   order == property.order &&
                   shipToName == property.shipToName &&
                   item == property.item &&
                   orderedQty == property.orderedQty &&
                   confirmedQty == property.confirmedQty &&
                   oldSku == property.oldSku &&
                   oldSkuDescription == property.oldSkuDescription &&
                   oldSkuCaseBarcode == property.oldSkuCaseBarcode &&
                   oldSkuUnitBarcode == property.oldSkuUnitBarcode &&
                   oldSkuATP == property.oldSkuATP &&
                   oldSkuRecoveryDate == property.oldSkuRecoveryDate &&
                   oldSkuRecoveryQty == property.oldSkuRecoveryQty &&
                   newSku == property.newSku &&
                   newSkuDescription == property.newSkuDescription &&
                   newSkuCaseBarcode == property.newSkuCaseBarcode &&
                   newSkuUnitBarcode == property.newSkuUnitBarcode &&
                   newSkuATP == property.newSkuATP &&
                   newSkuRecoveryDate == property.newSkuRecoveryDate &&
                   newSkuRecoveryQty == property.newSkuRecoveryQty &&
                   startDate == property.startDate &&
                   endDate == property.endDate &&
                   needOutOfStockToSwitch == property.needOutOfStockToSwitch &&
                   switchAutomatic == property.switchAutomatic &&
                   switchComment == property.switchComment &&
                   id == property.id;
        }

        public override int GetHashCode() {
            int hashCode = 1329786655;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesOrg);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(switchForCustomer);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + orderedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + oldSku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(oldSkuDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(oldSkuCaseBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(oldSkuUnitBarcode);
            hashCode = hashCode * -1521134295 + oldSkuATP.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(oldSkuRecoveryDate);
            hashCode = hashCode * -1521134295 + oldSkuRecoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + newSku.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(newSkuDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(newSkuCaseBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(newSkuUnitBarcode);
            hashCode = hashCode * -1521134295 + newSkuATP.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(newSkuRecoveryDate);
            hashCode = hashCode * -1521134295 + newSkuRecoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + startDate.GetHashCode();
            hashCode = hashCode * -1521134295 + endDate.GetHashCode();
            hashCode = hashCode * -1521134295 + needOutOfStockToSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + switchAutomatic.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(switchComment);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(id);
            return hashCode;
        }
    }
}