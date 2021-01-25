using IDAUtil.Model.Properties.ServerProperty;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryBlocks.Model.CountryModel {
    public class RODeliveryBlocksProperty {
        [Column("[id]")] public string id { get; set; }
        [Column("[orderStatus]")] public string orderStatus { get; set; }
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[orderNumber]")] public int orderNumber { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[soldToName]")] public string soldToName { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[currentDeliveryBlock]")] public string currentDeliveryBlock { get; set; }
        [Column("[newDeliveryBlock]")] public string newDeliveryBlock { get; set; }
        [Column("[reason]")] public string reason { get; set; }

        public RODeliveryBlocksProperty(DeliveryBlocksProperty deliveryBlocksProperty) {
            this.id = deliveryBlocksProperty.id;
            this.orderStatus = deliveryBlocksProperty.orderStatus;
            this.salesOrg = deliveryBlocksProperty.salesOrg;
            this.country = deliveryBlocksProperty.country;
            this.orderNumber = deliveryBlocksProperty.orderNumber;
            this.soldTo = deliveryBlocksProperty.soldTo;
            this.soldToName = deliveryBlocksProperty.soldToName;
            this.shipTo = deliveryBlocksProperty.shipTo;
            this.shipToName = deliveryBlocksProperty.shipToName;
            this.currentDeliveryBlock = deliveryBlocksProperty.currentDeliveryBlock;
            this.newDeliveryBlock = deliveryBlocksProperty.newDeliveryBlock;
            this.reason = deliveryBlocksProperty.reason;
        }
        public override bool Equals(object obj) {
            return obj is RODeliveryBlocksProperty property &&
                   id == property.id &&
                   orderStatus == property.orderStatus &&
                   salesOrg == property.salesOrg &&
                   country == property.country &&
                   orderNumber == property.orderNumber &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   currentDeliveryBlock == property.currentDeliveryBlock &&
                   newDeliveryBlock == property.newDeliveryBlock &&
                   reason == property.reason;
        }

        public override int GetHashCode() {
            var hashCode = 620312927;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(orderStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesOrg);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(currentDeliveryBlock);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(newDeliveryBlock);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(reason);
            return hashCode;
        }
    }
}
