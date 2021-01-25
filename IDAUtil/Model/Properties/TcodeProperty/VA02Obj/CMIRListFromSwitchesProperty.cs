using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil {
    public class CMIRListFromSwitchesProperty {
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[salesOrg]")] public int soldTo { get; set; }
        [Column("[salesOrg]")] public int sku { get; set; }
        [Column("[salesOrg]")] public string cmir { get; set; }
    }
}