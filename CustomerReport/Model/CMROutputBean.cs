using IDAUtil;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerReport {
    public class CMROutputBean {
        [Column("[soldTo]")]
        public long soldTo { get; set; }
        [Column("[soldToName]")]
        public string soldToName { get; set; }
        [Column("[shipTo]")]
        public long shipTo { get; set; }
        [Column("[shipToName]")]
        public string shipToName { get; set; }
        [Column("[salesOrg]")]
        public string salesOrg { get; set; }
        [Column("[id]")]
        public string id { get; set; }

        public CMROutputBean(ZV04HNProperty zv, string id) {
            soldTo = zv.soldto;
            soldToName = zv.soldtoName;
            shipTo = zv.shipto;
            shipToName = zv.shiptoName;
            this.id = id;
        }
    }
}