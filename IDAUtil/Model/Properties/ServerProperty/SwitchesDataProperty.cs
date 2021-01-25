using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class SwitchesDataProperty {
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[oldSku]")] public int oldSku { get; set; }
        [Column("[oldSkuDescription]")] public string oldSkuDescription { get; set; }
        [Column("[oldSkuCaseBarcode]")] public string oldSkuCaseBarcode { get; set; }
        [Column("[oldSkuUnitBarcode]")] public string oldSkuUnitBarcode { get; set; }
        [Column("[startDate]")] public DateTime startDate { get; set; }
        [Column("[endDate]")] public DateTime endDate { get; set; }
        [Column("[needOutOfStockToSwitch]")] public bool needOutOfStockToSwitch { get; set; }
        [Column("[newSku]")] public int newSku { get; set; }
        [Column("[newSkuDescription]")] public string newSkuDescription { get; set; }
        [Column("[newSkuCaseBarcode]")] public string newSkuCaseBarcode { get; set; }
        [Column("[newSkuUnitBarcode]")] public string newSkuUnitBarcode { get; set; }
        [Column("[switchAutomatic]")] public bool switchAutomatic { get; set; }
        [Column("[switchComment]")] public string switchComment { get; set; }
    }
}