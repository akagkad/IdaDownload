using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DistressReport.Model {
    public class GenericDistressProperty {
        [Column("[status]")] public string orderStatus { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[plant]")] public string plant { get; set; }
        [Column("[material]")] public int material { get; set; }
        [Column("[order]")] public int order { get; set; }
        [Column("[item]")] public int item { get; set; }
        [Column("[materialDescription]")] public string materialDescription { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[soldToName]")] public string soldToName { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[rejReason]")] public string rejReason { get; set; }
        [Column("[afterReleaseRej]")] public string afterReleaseRej { get; set; }
        [Column("[possibleSwitch]")] public int possibleSwitch { get; set; }
        [Column("[possibleSwitchDescription]")] public string possibleSwitchDescription { get; set; }
        [Column("[deliveryBlock]")] public string deliveryBlock { get; set; }
        [Column("[atp]")] public double atp { get; set; }
        [Column("[recoveryDate]")] public string recoveryDate { get; set; }
        [Column("[recoveryQty]")] public double recoveryQty { get; set; }
        [Column("[dChainStatus]")] public string dChainStatus { get; set; }
        [Column("[criticalItemComment]")] public string criticalItemComment { get; set; }
        [Column("[poDate]")] public string poDate { get; set; }
        [Column("[rdd]")] public string rdd { get; set; }
        [Column("[poNumber]")] public string poNumber { get; set; }
        [Column("[caseBarcode]")] public string caseBarcode { get; set; }
        [Column("[unitBarcode]")] public string unitBarcode { get; set; }
        [Column("[orderQty]")] public double orderQty { get; set; }
        [Column("[confirmedQty]")] public double confirmedQty { get; set; }
        [Column("[cutQty]")] public double cutQty { get; set; }
        [Column("[accountManager]")] public string accountManager { get; set; }
        [Column("[releaseDate]")] public string releaseDate { get; set; }
        [Column("[cmir]")] public string cmir { get; set; }
        [Column("[loadingDate]")] public string loadingDate { get; set; }
        [Column("[DocType]")] public string docType { get; set; }
        [Column("[Delivery Priority]")] public string delPriority { get; set; }
        [Column("[isPromo]")] public string starndardOrRepack { get; set; }
        [Column("[routeCode]")] public string routeCode { get; set; }
        [Column("[switchesComment]")] public string switchComment { get; set; }

        public GenericDistressProperty(string orderStatus,
                                       string country,
                                       string plant,
                                       int material,
                                       int order,
                                       int item,
                                       string materialDescription,
                                       int soldTo,
                                       string soldToName,
                                       int shipTo,
                                       string shipToName,
                                       string rejReason,
                                       string afterReleaseRej,
                                       int possibleSwitch,
                                       string possibleSwitchDescription,
                                       string deliveryBlock,
                                       double atp,
                                       string recoveryDate,
                                       double recoveryQty,
                                       string dChainStatus,
                                       string criticalItemComment,
                                       string poDate,
                                       DateTime rdd,
                                       string poNumber,
                                       string caseBarcode,
                                       string unitBarcode,
                                       double orderQty,
                                       double confirmedQty,
                                       double cutQty,
                                       string accountManager,
                                       string releaseDate,
                                       string cmir,
                                       DateTime loadingDate,
                                       string docType,
                                       string delPriority,
                                       string starndardOrRepack, string routeCode, string switchComment) {
            this.orderStatus = orderStatus;
            this.country = country;
            this.plant = plant;
            this.material = material;
            this.order = order;
            this.item = item;
            this.materialDescription = materialDescription;
            this.soldTo = soldTo;
            this.soldToName = soldToName;
            this.shipTo = shipTo;
            this.shipToName = shipToName;
            this.rejReason = rejReason;
            this.afterReleaseRej = afterReleaseRej;
            this.possibleSwitch = possibleSwitch;
            this.possibleSwitchDescription = possibleSwitchDescription;
            this.deliveryBlock = deliveryBlock;
            this.atp = atp;
            this.recoveryDate = recoveryDate;
            this.recoveryQty = recoveryQty;
            this.dChainStatus = dChainStatus;
            this.criticalItemComment = criticalItemComment;
            this.poDate = poDate;
            this.rdd = rdd.ToString("dd.MM.yyyy");
            this.poNumber = poNumber;
            this.caseBarcode = caseBarcode;
            this.unitBarcode = unitBarcode;
            this.orderQty = orderQty;
            this.confirmedQty = confirmedQty;
            this.cutQty = cutQty;
            this.accountManager = accountManager;
            this.releaseDate = releaseDate;
            this.cmir = cmir;
            this.loadingDate = loadingDate.ToString("dd.MM.yyyy");
            this.docType = docType;
            this.delPriority = delPriority;
            this.starndardOrRepack = starndardOrRepack;
            this.routeCode = routeCode;
            this.switchComment = switchComment;
        }

        public override bool Equals(object obj) {
            return obj is GenericDistressProperty property &&
                   orderStatus == property.orderStatus &&
                   country == property.country &&
                   plant == property.plant &&
                   material == property.material &&
                   order == property.order &&
                   item == property.item &&
                   materialDescription == property.materialDescription &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName &&
                   shipTo == property.shipTo &&
                   shipToName == property.shipToName &&
                   rejReason == property.rejReason &&
                   afterReleaseRej == property.afterReleaseRej &&
                   possibleSwitch == property.possibleSwitch &&
                   possibleSwitchDescription == property.possibleSwitchDescription &&
                   deliveryBlock == property.deliveryBlock &&
                   atp == property.atp &&
                   recoveryDate == property.recoveryDate &&
                   recoveryQty == property.recoveryQty &&
                   dChainStatus == property.dChainStatus &&
                   criticalItemComment == property.criticalItemComment &&
                   poDate == property.poDate &&
                   rdd == property.rdd &&
                   poNumber == property.poNumber &&
                   caseBarcode == property.caseBarcode &&
                   unitBarcode == property.unitBarcode &&
                   orderQty == property.orderQty &&
                   confirmedQty == property.confirmedQty &&
                   cutQty == property.cutQty &&
                   accountManager == property.accountManager &&
                   releaseDate == property.releaseDate &&
                   cmir == property.cmir &&
                   loadingDate == property.loadingDate &&
                   docType == property.docType &&
                   delPriority == property.delPriority &&
                   starndardOrRepack == property.starndardOrRepack &&
                   routeCode == property.routeCode &&
                   switchComment == property.switchComment;
        }

        public override int GetHashCode() {
            var hashCode = 299100645;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(orderStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(plant);
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + order.GetHashCode();
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rejReason);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(afterReleaseRej);
            hashCode = hashCode * -1521134295 + possibleSwitch.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(possibleSwitchDescription);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryBlock);
            hashCode = hashCode * -1521134295 + atp.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(recoveryDate);
            hashCode = hashCode * -1521134295 + recoveryQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(dChainStatus);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(criticalItemComment);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(rdd);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(poNumber);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(caseBarcode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(unitBarcode);
            hashCode = hashCode * -1521134295 + orderQty.GetHashCode();
            hashCode = hashCode * -1521134295 + confirmedQty.GetHashCode();
            hashCode = hashCode * -1521134295 + cutQty.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(accountManager);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(releaseDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(cmir);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loadingDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(docType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(delPriority);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(starndardOrRepack);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(routeCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(switchComment);
            return hashCode;
        }
    }
}