
using System.ComponentModel.DataAnnotations.Schema;

namespace KIWI {
    public class ConversionMaterial {
        [Column("[material]")]
        public int material { get; set; }
        [Column("[conversionIndex]")]
        public int conversionIndex { get; set; }
    }
}