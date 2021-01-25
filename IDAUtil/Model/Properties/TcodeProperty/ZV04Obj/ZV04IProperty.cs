using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.TcodeProperty.ZV04Obj {
    public class ZV04IProperty {
        [Column("[status]")]
        public string status { get; set; }
        [Column("[order]")]
        public int order { get; set; }
        [Column("[item]")]
        public int item { get; set; }
        [Column("[delivery]")]
        public int delivery { get; set; }
        [Column("[shipment]")]
        public int shipment { get; set; }
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
        public double orderQty { get; set; }
        [Column("[confirmedQty]")]
        public double confirmedQty { get; set; }
        [Column("[deliveryQty]")]
        public double deliveryQty { get; set; }
        [Column("[itemNetValue]")]
        public double itemNetValue { get; set; }
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
        [Column("[salesOffice]")]
        public string salesOffice { get; set; }
        [Column("[salesGroup]")]
        public string salesGroup { get; set; }
        [Column("[shipTo]")]
        public int shipTo { get; set; }
        [Column("[shipToName]")]
        public string shipToName { get; set; }
        [Column("[csrAsr]")]
        public string csrAsr { get; set; }
        [Column("[csrAsrName]")]
        public string csrAsrName { get; set; }
        [Column("[scjAgent]")]
        public string scjAgent { get; set; }
        [Column("[scjAgentName]")]
        public string scjAgentName { get; set; }
        [Column("[scjAgentReg]")]
        public string scjAgentReg { get; set; }
        [Column("[scjAgentRegName]")]
        public string scjAgentRegName { get; set; }
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
        [Column("[itemCategory]")]
        public string itemCategory { get; set; }

        public ZV04IProperty() { }

        public ZV04IProperty(string status, int order, int item, int delivery, int shipment, string docTyp, string docDate, string delBlock, string reqDelDate, string delPriority, string pODate, string loadingDate, int material, string materialDescription, string upc, string plant, double orderQty, double confirmedQty, double deliveryQty, double itemNetValue, string rejReason, string rejReasonDescription, string pONumber, int soldTo, string soldToName, string salesDistrict, string salesOffice, string salesGroup, int shipTo, string shipToName, string csrAsr, string csrAsrName, string scjAgent, string scjAgentName, string scjAgentReg, string scjAgentRegName, string tlfpItem, string pltItem, string layersItem, string looseCsItem, string totCsItem, string tlfpCmmtItem, string pltCmmtItem, string layersCmmtItem, string looseCsCmmtItem, string totCsCmmtItem, string csLayer, string itemWeight, string wtUnit, string itemCube, string volUnit, string itemCategory) {
            this.status = status;
            this.order = order;
            this.item = item;
            this.delivery = delivery;
            this.shipment = shipment;
            this.docTyp = docTyp;
            this.docDate = docDate;
            this.delBlock = delBlock;
            this.reqDelDate = reqDelDate;
            this.delPriority = delPriority;
            this.pODate = pODate;
            this.loadingDate = loadingDate;
            this.material = material;
            this.materialDescription = materialDescription;
            this.upc = upc;
            this.plant = plant;
            this.orderQty = orderQty;
            this.confirmedQty = confirmedQty;
            this.deliveryQty = deliveryQty;
            this.itemNetValue = itemNetValue;
            this.rejReason = rejReason;
            this.rejReasonDescription = rejReasonDescription;
            this.pONumber = pONumber;
            this.soldTo = soldTo;
            this.soldToName = soldToName;
            this.salesDistrict = salesDistrict;
            this.salesOffice = salesOffice;
            this.salesGroup = salesGroup;
            this.shipTo = shipTo;
            this.shipToName = shipToName;
            this.csrAsr = csrAsr;
            this.csrAsrName = csrAsrName;
            this.scjAgent = scjAgent;
            this.scjAgentName = scjAgentName;
            this.scjAgentReg = scjAgentReg;
            this.scjAgentRegName = scjAgentRegName;
            this.tlfpItem = tlfpItem;
            this.pltItem = pltItem;
            this.layersItem = layersItem;
            this.looseCsItem = looseCsItem;
            this.totCsItem = totCsItem;
            this.tlfpCmmtItem = tlfpCmmtItem;
            this.pltCmmtItem = pltCmmtItem;
            this.layersCmmtItem = layersCmmtItem;
            this.looseCsCmmtItem = looseCsCmmtItem;
            this.totCsCmmtItem = totCsCmmtItem;
            this.csLayer = csLayer;
            this.itemWeight = itemWeight;
            this.wtUnit = wtUnit;
            this.itemCube = itemCube;
            this.volUnit = volUnit;
            this.itemCategory = itemCategory;
        }

        public override bool Equals(object obj) {
            return obj is ZV04IProperty property &&
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
                   salesOffice == property.salesOffice &&
                   salesGroup == property.salesGroup &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   csrAsr == property.csrAsr &&
                   csrAsrName == property.csrAsrName &&
                   scjAgent == property.scjAgent &&
                   scjAgentName == property.scjAgentName &&
                   scjAgentReg == property.scjAgentReg &&
                   scjAgentRegName == property.scjAgentRegName &&
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
                   itemCategory == property.itemCategory;
        }

        public override int GetHashCode() {
            var hashCode = 640234196;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(status);
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + delivery.GetHashCode();
            hashCode = hashCode * -1521134295 + shipment.GetHashCode();
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
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + deliveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double>.Default.GetHashCode(itemNetValue);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReasonDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(pONumber);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesDistrict);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesOffice);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesGroup);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(csrAsr);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(csrAsrName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(scjAgent);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(scjAgentName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(scjAgentReg);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(scjAgentRegName);
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
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemCategory);
            return hashCode;
        }
    }
}