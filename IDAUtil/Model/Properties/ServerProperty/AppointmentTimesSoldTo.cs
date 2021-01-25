using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class AppointmentTimesSoldTo {
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[deliveryBlock]")] public string delBlock { get; set; }

        public AppointmentTimesSoldTo(int soldTo, string delBlock) {
            this.soldTo = soldTo;
            this.delBlock = delBlock;
        }

        public AppointmentTimesSoldTo() { }

        public override bool Equals(object obj) {
            return obj is AppointmentTimesSoldTo to &&
                   soldTo == to.soldTo &&
                   delBlock == to.delBlock;
        }

        public override int GetHashCode() {
            var hashCode = -1502042279;
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delBlock);
            return hashCode;
        }
    }
}
