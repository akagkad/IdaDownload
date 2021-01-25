using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class SkuDataProperty {
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[sku]")] public int sku { get; set; }
        [Column("[unitBarcode]")] public string unitBarcode { get; set; }
        [Column("[caseBarcode]")] public string caseBarcode { get; set; }
        [Column("[unitPerCase]")] public int unitPerCase { get; set; }
        [Column("[casePerLayer]")] public int casePerLayer { get; set; }
        [Column("[casePerPallet]")] public int casePerPallet { get; set; }
        [Column("[layerPerPallet]")] public int layerPerPallet { get; set; }
        [Column("[isStackable]")] public bool isStackable { get; set; }
        [Column("[standardOrRepack]")] public string standardOrRepack { get; set; }
        [Column("[sizePallet]")] public string sizePallet { get; set; }

    }
}
