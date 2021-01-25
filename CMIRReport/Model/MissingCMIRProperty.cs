using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMIRReport {
    public class MissingCMIRProperty {
        [Column("[CMIR]")]
        public string CMIR { get; set; }
        [Column("[material]")]
        public int material { get; set; }
        [Column("[order]")]
        public int order { get; set; }
        [Column("[item]")]
        public int item { get; set; }
        [Column("[materialDescription]")]
        public string materialDescription { get; set; }
        [Column("[soldTo]")]
        public int soldTo { get; set; }
        [Column("[soldToName]")]
        public string soldToName { get; set; }

        public MissingCMIRProperty(int material, string materialDescription, int soldTo, string soldToName, int order, int item) {
            this.material = material;
            this.materialDescription = materialDescription;
            this.soldTo = soldTo;
            this.soldToName = soldToName;
            this.order = order;
            this.item = item;
            CMIR = "";
        }

        public override bool Equals(object obj) {
            return obj is MissingCMIRProperty property &&
                   CMIR == property.CMIR &&
                   material == property.material &&
                   order == property.order &&
                   item == property.item &&
                   materialDescription == property.materialDescription &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName;
        }

        public override int GetHashCode() {
            var hashCode = 104675644;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CMIR);
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            return hashCode;
        }
    }
}