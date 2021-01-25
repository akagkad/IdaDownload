namespace DeliveryBlocks.Service.CountryCalculators.Support {
    public class PromoOrderShipTo {
        public int shipTo { get; set; }

        public PromoOrderShipTo(int shipTo) {
            this.shipTo = shipTo;
        }

        public override bool Equals(object obj) {
            return obj is PromoOrderShipTo to &&
                   shipTo == to.shipTo;
        }

        public override int GetHashCode() {
            return -573137782 + shipTo.GetHashCode();
        }
    }
}
