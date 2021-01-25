using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class DistributionListProperty {
        [Column("[name]")]
        public string name { get; set; }
        [Column("[salesOrg]")]
        public string salesOrg { get; set; }
        [Column("[country]")]
        public string country { get; set; }
        [Column("[address]")]
        public string address { get; set; }
    }
}