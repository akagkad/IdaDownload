using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerReport {
    public class MissingCustomersProperty {
        [Column("[order]")]
        public long order { get; set; }
        [Column("[soldto]")]
        public long soldto { get; set; }
        [Column("[soldtoName]")]
        public string soldtoName { get; set; }
        [Column("[shipto]")]
        public long shipto { get; set; }
        [Column("[shiptoName]")]
        public string shiptoName { get; set; }

        public MissingCustomersProperty(long order, long soldto, string soldtoName, long shipto, string shiptoName) {
            this.order = order;
            this.soldto = soldto;
            this.soldtoName = soldtoName;
            this.shipto = shipto;
            this.shiptoName = shiptoName;
        }

        public override bool Equals(object obj) {
            return obj is MissingCustomersProperty property &&
                   order == property.order &&
                   soldto == property.soldto &&
                   soldtoName == property.soldtoName &&
                   shipto == property.shipto &&
                   shiptoName == property.shiptoName;
        }

        public override int GetHashCode() {
            var hashCode = 695662973;
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + soldto.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldtoName);
            hashCode = hashCode * -1521134295 + shipto.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shiptoName);
            return hashCode;
        }
    }
}