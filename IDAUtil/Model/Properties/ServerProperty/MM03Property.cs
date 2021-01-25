using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class MM03Property {
        [Column("[material]")] public int material { get; set; }
        [Column("[organisation]")] public string organisation { get; set; }
        [Column("[unitEAN]")] public string unitEAN { get; set; }
        [Column("[caseEAN]")] public string caseEAN { get; set; }
        [Column("[dChainStatus]")] public string dChainStatus { get; set; }
        [Column("[unitPerCase]")] public int unitPerCase { get; set; }
    }
}
