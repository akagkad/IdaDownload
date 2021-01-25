using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.TaskProperty {
    public class RejectionFullOrderProperty {
        [Column("[id]")] public string id { get; set; }
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[orderNumber]")] public int orderNumber { get; set; }
        [Column("[porNumber]")] public string poNumber { get; set; }
        [Column("[rejCode]")] public string rejCode { get; set; }
        [Column("[reason]")] public string reason { get; set; }
        [Column("[orderQty]")] public double orderQty { get; set; }
        [Column("[netValue]")] public double netValue { get; set; }


        public RejectionFullOrderProperty() { }

        public RejectionFullOrderProperty(string id, string salesOrg, string country, int soldTo, int shipTo, string shipToName, int orderNumber, string poNumber, string rejCode, double orderQty, double netValue, string reason) {
            this.id = id;
            this.salesOrg = salesOrg;
            this.country = country;
            this.soldTo = soldTo;
            this.shipTo = shipTo;
            this.shipToName = shipToName;
            this.orderNumber = orderNumber;
            this.poNumber = poNumber;
            this.rejCode = rejCode;
            this.orderQty = orderQty;
            this.netValue = netValue;
            this.reason = reason;
        }

        public override bool Equals(object obj) {
            return obj is RejectionFullOrderProperty property &&
                   id == property.id &&
                   salesOrg == property.salesOrg &&
                   country == property.country &&
                   soldTo == property.soldTo &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   orderNumber == property.orderNumber &&
                   poNumber == property.poNumber &&
                   rejCode == property.rejCode &&
                   orderQty == property.orderQty &&
                   netValue == property.netValue &&
                   reason == property.reason;
        }

        public override int GetHashCode() {
            var hashCode = 796112326;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesOrg);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejCode);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + netValue.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(reason);
            return hashCode;
        }
    }
}
