using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IDAUtil.Model.Properties.TcodeProperty.ZPURRSObj
{
    public class ZPURRSProperty
    {

        [Column("[PurchGrp]")] public int PurchaseGroup { get; set; }
        [Column("[Vendor]")] public int Vendor { get; set; }
        [Column("[Material]")] public int Material { get; set; }
        [Column("[Descr]")] public string Descr { get; set; }
        [Column("[reqDelDate]")] public string Rdd { get; set; }
        [Column("[ReschDate]")] public string ReschDate { get; set; }
        [Column("[PurchDoc]")] public int PurchDoc { get; set; }
        [Column("[Item]")] public int Item { get; set; }
        [Column("[ExceptMsg]")] public string ExceptMsg { get; set; }
        [Column("[plant]")] public int Plant { get; set; }
        [Column("[VendorName]")] public string VendorName { get; set; }
        [Column("[MRPcont]")] public int MRPcont { get; set; }
        [Column("[Name]")] public string Name { get; set; }
        [Column("[RemainQty]")] public double RemainQty { get; set; }
        [Column("[Quantity]")] public double Quantity { get; set; }
        [Column("[QtyDelivered]")] public double QtyDelivered { get; set; }
        [Column("[POplndDelyTime]")] public string POplndDelyTime { get; set; }
        [Column("[OrderUnit]")] public string OrderUnit { get; set; }
        [Column("[ReschDueDate]")] public string ReschGRdueDate { get; set; }

        public ZPURRSProperty()
        {

        }

        public ZPURRSProperty(int PurchaseGroup, int Vendor, int Material, string Descr, string Rdd, string ReschDate, int PurchDoc, int Item, string ExceptMsg, int Plant, string VendorName, int MRPcont, string Name, double RemainQty, double Quantity, double QtyDelivered, string POplndDelyTime, string OrderUnit, string ReschGRdueDate)
        {
            this.PurchaseGroup = PurchaseGroup;
            this.Vendor = Vendor;
            this.Material = Material;
            this.Descr = Descr;
            this.Rdd = Rdd;
            this.ReschDate = ReschDate;
            this.PurchDoc = PurchDoc;
            this.Item = Item;
            this.ExceptMsg = ExceptMsg;
            this.Plant = Plant;
            this.VendorName = VendorName;
            this.MRPcont = MRPcont;
            this.Name = Name;
            this.RemainQty = RemainQty;
            this.Quantity = Quantity;
            this.QtyDelivered = QtyDelivered;
            this.POplndDelyTime = POplndDelyTime;
            this.OrderUnit = OrderUnit;
            this.ReschGRdueDate = ReschGRdueDate;
        }

        public override bool Equals(object obj)
        {
            return obj is ZPURRSProperty property &&
                   PurchaseGroup == property.PurchaseGroup &&
                   Vendor == property.Vendor &&
                   Material == property.Material &&
                   Descr == property.Descr &&
                   Rdd == property.Rdd &&
                   ReschDate == property.ReschDate &&
                   PurchDoc == property.PurchDoc &&
                   Item == property.Item &&
                   ExceptMsg == property.ExceptMsg &&
                   Plant == property.Plant &&
                   VendorName == property.VendorName &&
                   MRPcont == property.MRPcont &&
                   Name == property.Name &&
                   RemainQty == property.RemainQty &&
                   Quantity == property.Quantity &&
                   QtyDelivered == property.QtyDelivered &&
                   POplndDelyTime == property.POplndDelyTime &&
                   OrderUnit == property.OrderUnit &&
                   ReschGRdueDate == property.ReschGRdueDate;
        }

        public override int GetHashCode()
        {
            var hashCode = 640234196;

            hashCode = hashCode * -1521134295 + PurchaseGroup.GetHashCode();
            hashCode = hashCode * -1521134295 + Vendor.GetHashCode();
            hashCode = hashCode * -1521134295 + Material.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Descr);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Rdd);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReschDate);
            hashCode = hashCode * -1521134295 + PurchDoc.GetHashCode();
            hashCode = hashCode * -1521134295 + Item.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ExceptMsg);
            hashCode = hashCode * -1521134295 + Plant.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VendorName);
            hashCode = hashCode * -1521134295 + MRPcont.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<double>.Default.GetHashCode(Quantity);
            hashCode = hashCode * -1521134295 + EqualityComparer<double>.Default.GetHashCode(RemainQty);
            hashCode = hashCode * -1521134295 + EqualityComparer<double>.Default.GetHashCode(QtyDelivered);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(POplndDelyTime);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OrderUnit);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReschGRdueDate);

            return hashCode;
        }
    }
}
