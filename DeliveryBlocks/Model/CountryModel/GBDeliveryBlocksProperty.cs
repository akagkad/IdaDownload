using IDAUtil;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryBlocks.Model.CountryModel {
    class GBDeliveryBlocksProperty {
        [Column("[staus]")] public string status { get; set; }
        [Column("[Order Number]")] public int orderNumber { get; set; }
        [Column("[Doc Type]")] public string docType { get; set; }
        [Column("[Doc Date]")] public DateTime docDate { get; set; }
        [Column("[Delivery Block]")] public string delBlock { get; set; }
        [Column("[Delivery Block Desc]")] public string delBlockDesc { get; set; }
        [Column("[Requested Delivery Date]")] public DateTime rdd { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[soldToName]")] public string soldToName { get; set; }

        public GBDeliveryBlocksProperty(ZV04HNProperty zv) {
            this.status = zv.status;
            this.orderNumber = zv.order;
            this.docType = zv.docTyp;
            this.docDate = zv.docDate;
            this.delBlock = zv.delBlock;
            this.delBlockDesc = zv.delBlockDesc;
            this.rdd = zv.reqDelDate;
            this.soldTo = zv.soldto;
            this.soldToName = zv.soldtoName;
        }

        public override bool Equals(object obj) {
            return obj is GBDeliveryBlocksProperty property &&
                   status == property.status &&
                   orderNumber == property.orderNumber &&
                   docType == property.docType &&
                   docDate == property.docDate &&
                   delBlock == property.delBlock &&
                   delBlockDesc == property.delBlockDesc &&
                   rdd == property.rdd &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName;
        }

        public override int GetHashCode() {
            var hashCode = -344981876;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(status);
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(docType);
            hashCode = hashCode * -1521134295 + docDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delBlock);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delBlockDesc);
            hashCode = hashCode * -1521134295 + rdd.GetHashCode();
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            return hashCode;
        }
    }
}
