using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace IDAUtil.Model.Properties.TcodeProperty.CO09Obj {
    [DebuggerDisplay("sku: {sku}, salesOrg: {salesOrg}, ATP: {ATP}")]
    public class CO09Property {
        [Column("[sku]")] public int sku { get; set; }
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[plant]")] public int plant { get; set; }
        [Column("[ATP]")] public double ATP { get; set; }
        [Column("[recoveryDate]")] public string recoveryDate { get; set; }
        [Column("[recoveryQty]")] public double recoveryQty { get; set; }
    }
}