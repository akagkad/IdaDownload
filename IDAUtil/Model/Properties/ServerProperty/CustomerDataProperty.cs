using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty { 
    public class CustomerDataProperty {
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[distributionChannel]")] public int distributionChannel { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[soldToName]")] public string soldToName { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[cmirCheckAllowed]")] public bool cmirCheckAllowed { get; set; }
        [Column("[replaceObsoletePartialCutsAllowed]")] public bool replaceObsoletePartialCutsAllowed { get; set; }
        [Column("[changeRouteCodeActionAllowed]")] public bool changeRouteCodeActionAllowed { get; set; }
        [Column("[changeRDDActionAllowed]")] public bool changeRDDActionAllowed { get; set; }
        [Column("[oneDayLeadTimeAllowed]")] public bool oneDayLeadTimeAllowed { get; set; }
        [Column("[leadTime]")] public int leadTime { get; set; }
        [Column("[deliveryDay]")] public string deliveryDay { get; set; }
        [Column("[accountManager]")] public string accountManager { get; set; }
        [Column("[salesDistrict]")] public int salesDistrict { get; set; }
        [Column("[csrNumber]")] public int csrNumber { get; set; }
        [Column("[csrName]")] public string csrName { get; set; }
        [Column("[minimumOrderCaseQuantity]")] public int minimumOrderCaseQuantity { get; set; }
        [Column("[minimumOrderValue]")] public int minimumOrderValue { get; set; }
        [Column("[truckOverflowPalletQuantity]")] public int truckOverflowPalletQuantity { get; set; }
        [Column("[truckOverflowValue]")] public int truckOverflowValue { get; set; }
        [Column("[truckOverflowWeight]")] public int truckOverflowWeight { get; set; }
        [Column("[markedForDelitionZA01Only]")] public bool markedForDelitionZA01Only { get; set; }
        [Column("[region]")] public string region { get; set; }
        [Column("[orderConfirmationEmails]")] public string orderConfirmationEmails { get; set; }
        [Column("[deliveryConfirmationEmails]")] public string deliveryConfirmationEmails { get; set; }
        [Column("[dailyCutsEmails]")] public string dailyCutsEmails { get; set; }
        [Column("[belowMOQandMOVEmails]")] public string belowMOQandMOVEmails { get; set; }
    }
}