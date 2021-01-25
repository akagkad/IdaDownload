using System.ComponentModel.DataAnnotations.Schema;


namespace FloorCutsNew.Model {
    class ES_PT_PL_CZ_list {

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

        [Column("[orderQty]")]
        public double orderQty { get; set; }
        [Column("[confirmedQty]")]
        public double confirmedQty { get; set; }

        [Column("[pONumber]")]
        public string pONumber { get; set; }

        [Column("[shipToName]")]
        public string shipToName { get; set; }

    }
}

