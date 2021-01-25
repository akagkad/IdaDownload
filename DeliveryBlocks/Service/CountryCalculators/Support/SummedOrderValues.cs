using System;

namespace DeliveryBlocks.Service.CountryCalculators.Support {
    public class SummedOrderValues {
        public int shipTo { get; set; }
        public DateTime rdd { get; set; }
        public double totalValue { get; set; }
        public double totalQty { get; set; }

        public SummedOrderValues() { }

        public SummedOrderValues(int shipTo, DateTime rdd, double totalValue, double totalQty) {
            this.shipTo = shipTo;
            this.rdd = rdd;
            this.totalValue = totalValue;
            this.totalQty = totalQty;
        }

        public override bool Equals(object obj) {
            return obj is SummedOrderValues values &&
                   shipTo == values.shipTo &&
                   rdd == values.rdd &&
                   totalValue == values.totalValue &&
                   totalQty == values.totalQty;
        }

        public override int GetHashCode() {
            var hashCode = 1299700092;
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + rdd.GetHashCode();
            hashCode = hashCode * -1521134295 + totalValue.GetHashCode();
            hashCode = hashCode * -1521134295 + totalQty.GetHashCode();
            return hashCode;
        }
    }
}
