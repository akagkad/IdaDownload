
using System.ComponentModel.DataAnnotations.Schema;

namespace KIWI {
    public class ConversionShiptTo {
        [Column("[shipTo]")]
        public int shipTo { get; set; }
    }
}