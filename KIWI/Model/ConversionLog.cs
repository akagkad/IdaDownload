using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KIWI {
    public class ConversionLog {
        [Column("[orderNumber]")]
        public int orderNumber { get; set; }
        [Column("[shipTo]")]
        public int shipTo { get; set; }
        [Column("[item]")]
        public int item { get; set; }
        [Column("[material]")]
        public int material { get; set; }
        [Column("[oldQty]")]
        public int oldQty { get; set; }
        [Column("[newQty]")]
        public int newQty { get; set; }
        [Column("[isConverted]")]
        public bool isConverted { get; set; }
        [Column("[startTime]")]
        public DateTime startTime { get; set; }
        [Column("[endTime]")]
        public DateTime endTime { get; set; }
        [Column("[status]")]
        public string status { get; set; }
        [Column("[isSaved]")]
        public bool isSaved { get; set; }
    }
}