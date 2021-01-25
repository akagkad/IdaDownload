using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class RejectionsDataProperty {
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[soldTo]")] public int soldTo { get; set; }
        [Column("[shipTo]")] public int shipTo { get; set; }
        [Column("[sku]")] public int sku { get; set; }
        [Column("[skuDescription]")] public string skuDescription { get; set; }
        [Column("[skuUnitBarcode]")] public string skuUnitBarcode { get; set; }
        [Column("[skuCaseBarcode]")] public string skuCaseBarcode { get; set; }
        [Column("[rejectionReasonCode]")] public string rejectionReasonCode { get; set; }
        [Column("[needOutOfStockToReject]")] public bool needOutOfStockToReject { get; set; }
        [Column("[startDate]")] public DateTime startDate { get; set; }
        [Column("[endDate]")] public DateTime endDate { get; set; }
        [Column("[rejectionComment]")] public string rejectionComment { get; set; }
    }
}