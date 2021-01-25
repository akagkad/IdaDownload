using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloorCutsNew.Model {
    class DE_list {

            [Column("[OrderNumber]")]
            public int OrderNumber { get; set; }

            [Column("[RDD]")]
            public string RDD { get; set; }

            [Column("[Material]")]
            public int Material { get; set; }

            [Column("[MaterialDescription]")]
            public string MaterialDescription { get; set; }

            [Column("[UPC]")]
            public string UPC { get; set; }

            [Column("[plant]")]
            public string plant { get; set; }

            [Column("[orderQty]")]
            public double orderQty { get; set; }

            [Column("[confirmedQty]")]
            public double confirmedQty { get; set; }

            [Column("[pONumber]")]
            public string pONumber { get; set; }

            [Column("[shipToName]")]
            public string shipToName { get; set; }

            [Column("[shipTo]")]
            public int shipTo { get; set; }

            [Column("[csrAsr]")]
            public string csrAsr { get; set; }

    }
    }
