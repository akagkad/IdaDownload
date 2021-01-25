using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace FloorCutsNew
{
    public class PastPOdateProperty
    {

        [Column("[material]")]
        public int material { get; set; }
        [Column("[materialDescription]")]
        public string materialDescription { get; set; }
        [Column("[Requested Delivery Date]")]
        public string rdd { get; set; }

        [Column("[Purchasing Document]")]
        public int PurchDoc { get; set; }

        [Column("[Plant]")]
        public int plant { get; set; }
        [Column("[Remaining Quantity]")]
        public double RemainQty { get; set; }

        public PastPOdateProperty(int material, string materialDescription, String rdd, int PurchDoc, int plant, double RemainQty)
        {
            this.material = material;
            this.materialDescription = materialDescription;
            this.rdd = rdd;
            this.PurchDoc = PurchDoc;
            this.plant = plant;
            this.RemainQty = RemainQty;

        }

        public override bool Equals(object obj)
        {
            return obj is PastPOdateProperty property &&

                   material == property.material &&
                   materialDescription == property.materialDescription &&
                   rdd == property.rdd &&
                   PurchDoc == property.PurchDoc &&
                   plant == property.plant &&
                   RemainQty == property.RemainQty;
        }

        public override int GetHashCode()
        {
            var hashCode = 104675644;
            // hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CMIR);
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + materialDescription.GetHashCode();
            hashCode = hashCode * -1521134295 + rdd.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(materialDescription);
            hashCode = hashCode * -1521134295 + PurchDoc.GetHashCode();
            hashCode = hashCode * -1521134295 + plant.GetHashCode();
            hashCode = hashCode * -1521134295 + RemainQty.GetHashCode();
            return hashCode;
        }
    }
}
