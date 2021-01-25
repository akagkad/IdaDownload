using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloorCutsNew.Model {
    class ZA_NG_KElist {

        [Column("[OrderNumber]")]
        public int OrderNumber { get; set; }

        [Column("[docTyp]")]
        public string docTyp { get; set; }
        [Column("[docDate]")]
        public string docDate { get; set; }

        [Column("[RDD]")]
        public string RDD { get; set; }

        [Column("[Material]")]
        public int Material { get; set; }

        [Column("[MaterialDescription]")]
        public string MaterialDescription { get; set; }

        [Column("[orderQty]")]
        public double orderQty { get; set; }
        [Column("[confirmedQty]")]
        public double confirmedQty { get; set; }

        [Column("[pONumber]")]
        public string pONumber { get; set; }

        public int soldTo { get; set; }
        [Column("[soldToName]")]
        public string soldToName { get; set; }

        [Column("[shipToName]")]
        public string shipToName { get; set; }

        [Column("[shipTo]")]
        public int shipTo { get; set; }

        [Column("[csrAsr]")]
        public string csrAsr { get; set; }

    }
}


