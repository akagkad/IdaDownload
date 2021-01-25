using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.TcodeProperty.ZV04Obj {
    public class ZV04PProperty {
        [Column("[status]")]
        public string status { get; set; }
        [Column("[order]")]
        public int order { get; set; }
        [Column("[item]")]
        public int item { get; set; }
        [Column("[delivery]")]
        public string delivery { get; set; }
        [Column("[shipment]")]
        public string shipment { get; set; }
        [Column("[docTyp]")]
        public string docTyp { get; set; }
        [Column("[docDate]")]
        public string docDate { get; set; }
        [Column("[delBlock]")]
        public string delBlock { get; set; }
        [Column("[reqDelDate]")]
        public string reqDelDate { get; set; }
        [Column("[delPriority]")]
        public string delPriority { get; set; }
        [Column("[pODate]")]
        public string pODate { get; set; }
        [Column("[loadingDate]")]
        public string loadingDate { get; set; }
        [Column("[material]")]
        public int material { get; set; }
        [Column("[materialDescription]")]
        public string materialDescription { get; set; }
        [Column("[upc]")]
        public string upc { get; set; }
        [Column("[plant]")]
        public string plant { get; set; }
        [Column("[orderQty]")]
        public string orderQty { get; set; }
        [Column("[confirmedQty]")]
        public string confirmedQty { get; set; }
        [Column("[deliveryQty]")]
        public string deliveryQty { get; set; }
        [Column("[itemNetValue]")]
        public string itemNetValue { get; set; }
        [Column("[rejReason]")]
        public string rejReason { get; set; }
        [Column("[rejReasonDescription]")]
        public string rejReasonDescription { get; set; }
        [Column("[pONumber]")]
        public string pONumber { get; set; }
        [Column("[soldTo]")]
        public int soldTo { get; set; }
        [Column("[soldToName]")]
        public string soldToName { get; set; }
        [Column("[salesDistrict]")]
        public string salesDistrict { get; set; }
        [Column("[shipTo]")]
        public int shipTo { get; set; }
        [Column("[shipToName]")]
        public string shipToName { get; set; }
        [Column("[csrAsr]")]
        public string csrAsr { get; set; }
        [Column("[csrAsrName]")]
        public string csrAsrName { get; set; }
        [Column("[tlfpItem]")]
        public string tlfpItem { get; set; }
        [Column("[pltItem]")]
        public string pltItem { get; set; }
        [Column("[layersItem]")]
        public string layersItem { get; set; }
        [Column("[looseCsItem]")]
        public string looseCsItem { get; set; }
        [Column("[totCsItem]")]
        public string totCsItem { get; set; }
        [Column("[tlfpCmmtItem]")]
        public string tlfpCmmtItem { get; set; }
        [Column("[pltCmmtItem]")]
        public string pltCmmtItem { get; set; }
        [Column("[layersCmmtItem]")]
        public string layersCmmtItem { get; set; }
        [Column("[looseCsCmmtItem]")]
        public string looseCsCmmtItem { get; set; }
        [Column("[totCsCmmtItem]")]
        public string totCsCmmtItem { get; set; }
        [Column("[csLayer]")]
        public string csLayer { get; set; }
        [Column("[itemWeight]")]
        public string itemWeight { get; set; }
        [Column("[wtUnit]")]
        public string wtUnit { get; set; }
        [Column("[itemCube]")]
        public string itemCube { get; set; }
        [Column("[volUnit]")]
        public string volUnit { get; set; }
        [Column("[custExpPrice]")]
        public string custExpPrice { get; set; }
        [Column("[scjComparisonPrice]")]
        public string scjComparisonPrice { get; set; }
        [Column("[scheduleLineDeliveryDate]")]
        public string scheduleLineDeliveryDate { get; set; }
        [Column("[custMatNumb]")]
        public string custMatNumb { get; set; }
        [Column("[receivingPoint]")]
        public string receivingPoint { get; set; }

        public override bool Equals(object obj) {
            return obj is ZV04PProperty property &&
                   status == property.status &&
                   order == property.order &&
                   item == property.item &&
                   delivery == property.delivery &&
                   shipment == property.shipment &&
                   docTyp == property.docTyp &&
                   docDate == property.docDate &&
                   delBlock == property.delBlock &&
                   reqDelDate == property.reqDelDate &&
                   delPriority == property.delPriority &&
                   pODate == property.pODate &&
                   loadingDate == property.loadingDate &&
                   material == property.material &&
                   materialDescription == property.materialDescription &&
                   upc == property.upc &&
                   plant == property.plant &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   deliveryQty == property.deliveryQty &&
                   itemNetValue == property.itemNetValue &&
                   rejReason == property.rejReason &&
                   rejReasonDescription == property.rejReasonDescription &&
                   pONumber == property.pONumber &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName &&
                   salesDistrict == property.salesDistrict &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   csrAsr == property.csrAsr &&
                   csrAsrName == property.csrAsrName &&
                   tlfpItem == property.tlfpItem &&
                   pltItem == property.pltItem &&
                   layersItem == property.layersItem &&
                   looseCsItem == property.looseCsItem &&
                   totCsItem == property.totCsItem &&
                   tlfpCmmtItem == property.tlfpCmmtItem &&
                   pltCmmtItem == property.pltCmmtItem &&
                   layersCmmtItem == property.layersCmmtItem &&
                   looseCsCmmtItem == property.looseCsCmmtItem &&
                   totCsCmmtItem == property.totCsCmmtItem &&
                   csLayer == property.csLayer &&
                   itemWeight == property.itemWeight &&
                   wtUnit == property.wtUnit &&
                   itemCube == property.itemCube &&
                   volUnit == property.volUnit &&
                   custExpPrice == property.custExpPrice &&
                   scjComparisonPrice == property.scjComparisonPrice &&
                   scheduleLineDeliveryDate == property.scheduleLineDeliveryDate &&
                   custMatNumb == property.custMatNumb &&
                   receivingPoint == property.receivingPoint;
        }

        public override int GetHashCode() {
            var hashCode = -1418997067;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(status);
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int>.Default.GetHashCode(item);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delivery);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipment);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(docTyp);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(docDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delBlock);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(reqDelDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delPriority);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(pODate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loadingDate);
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(upc);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(plant);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(orderQty);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(confirmedQty);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryQty);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemNetValue);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReasonDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(pONumber);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesDistrict);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(csrAsr);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(csrAsrName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(tlfpItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(pltItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(layersItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(looseCsItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(totCsItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(tlfpCmmtItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(pltCmmtItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(layersCmmtItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(looseCsCmmtItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(totCsCmmtItem);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(csLayer);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemWeight);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(wtUnit);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemCube);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(volUnit);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(custExpPrice);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(scjComparisonPrice);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(scheduleLineDeliveryDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(custMatNumb);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(receivingPoint);
            return hashCode;
        }
    }
}