using System.Collections.Generic;

namespace KIWI.Model {
    public class KiwiConversionException {
        public List<int> soldTo { get; set; } = new List<int> { 57071, 56514, 56691, 56925, 56927, 172307, 80176 };
        public List<int> material { get; set; } = new List<int> { 322413, 322412 };

        public KiwiConversionException() { }

        public override bool Equals(object obj) {
            return obj is KiwiConversionException list &&
                   EqualityComparer<List<int>>.Default.Equals(soldTo, list.soldTo) &&
                   EqualityComparer<List<int>>.Default.Equals(material, list.material);
        }

        public override int GetHashCode() {
            var hashCode = 1816942026;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(soldTo);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(material);
            return hashCode;
        }
    }
}
