using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class CriticalItemsDataProperty {
            [Column("[salesOrg]")] public string salesOrg { get; set; }
            [Column("[sku]")] public int sku { get; set; }
            [Column("[description]")] public string description { get; set; }
            [Column("[caseBarcode]")] public string caseBarcode { get; set; }
            [Column("[unitBarcode]")] public string unitBarcode { get; set; }
            [Column("[atp]")] public double atp { get; set; }
            [Column("[recoveryDate]")] public string recoveryDate { get; set; }
            [Column("[recoveryQty]")] public double recoveryQty { get; set; }
            [Column("[comments]")] public string comments { get; set; }
    }
}