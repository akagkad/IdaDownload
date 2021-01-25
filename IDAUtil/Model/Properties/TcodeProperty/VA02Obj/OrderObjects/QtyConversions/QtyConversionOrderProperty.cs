using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;

namespace IDAUtil {
    public class QtyConversionOrderProperty {
        public int orderNumber { get; set; }
        public int shipTo { get; set; }
        public int soldTo { get; set; }
        public string soldToName { get; set; }
        public string shipToName { get; set; }
        public string docDate { get; set; }
        public string pONumber { get; set; }
        public long deliveryNumber { get; set; }
        public bool isSaved { get; set; }
        public List<DocumentLine> documentLineList { get; set; }
        public List<DocumentLine> documentLineChangeList { get; set; }

        public string start(int i) {
            return documentLineChangeList[i].start();
        }

        public string finish(int i) {
            return documentLineChangeList[i].finish();
        }

        public void removeLineByIndex(int index) {
            documentLineList.RemoveAt(index);
            documentLineChangeList.RemoveAt(index);
        }

        public bool removeLineByItem(int item) {
            return Conversions.ToBoolean(documentLineList.RemoveAll(x => x.item == item)) && Conversions.ToBoolean(documentLineChangeList.RemoveAll(x => x.item == item));
        }

        public override bool Equals(object obj) {
            return obj is QtyConversionOrderProperty property &&
                   orderNumber == property.orderNumber &&
                   shipTo == property.shipTo &&
                   soldTo == property.soldTo &&
                   soldToName == property.soldToName &&
                   shipToName == property.shipToName &&
                   docDate == property.docDate &&
                   pONumber == property.pONumber &&
                   deliveryNumber == property.deliveryNumber &&
                   isSaved == property.isSaved &&
                   EqualityComparer<List<DocumentLine>>.Default.Equals(documentLineList, property.documentLineList) &&
                   EqualityComparer<List<DocumentLine>>.Default.Equals(documentLineChangeList, property.documentLineChangeList);
        }

        public override int GetHashCode() {
            var hashCode = -190178278;
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(docDate);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(pONumber);
            hashCode = hashCode * -1521134295 + deliveryNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + isSaved.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<List<DocumentLine>>.Default.GetHashCode(documentLineList);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<DocumentLine>>.Default.GetHashCode(documentLineChangeList);
            return hashCode;
        }
    }

    public class DocumentLine {
        public int item { get; set; }
        public int material { get; set; }
        public double quantity { get; set; }
        public bool isChanged { get; set; }
        public string status { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }

        public void actionFailed(string status) {
            isChanged = false;
            this.status = status;
        }

        public void actionSuccess(string status = "Success") {
            isChanged = true;
            this.status = status;
        }

        public string start() {
            startTime = Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss");
            return startTime;
        }

        public string finish() {
            endTime = Strings.Format(DateTime.Now, "yyyyMMdd HH:mm:ss");
            return endTime;
        }

        public override bool Equals(object obj) {
            return obj is DocumentLine line &&
                   item == line.item &&
                   material == line.material &&
                   quantity == line.quantity &&
                   isChanged == line.isChanged &&
                   status == line.status &&
                   startTime == line.startTime &&
                   endTime == line.endTime;
        }

        public override int GetHashCode() {
            var hashCode = -1241902631;
            hashCode = hashCode * -1521134295 + item.GetHashCode();
            hashCode = hashCode * -1521134295 + material.GetHashCode();
            hashCode = hashCode * -1521134295 + quantity.GetHashCode();
            hashCode = hashCode * -1521134295 + isChanged.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(status);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(startTime);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(endTime);
            return hashCode;
        }
    }
}
