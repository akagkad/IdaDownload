using IDAUtil;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace KIWI {
    public class DataCompareService {
        public List<QtyConversionOrderProperty> getSalesDocumentToChangeQuantity(List<ZV04IProperty> zvo4iList, List<ConversionShiptTo> shipToConversionList, List<ConversionMaterial> materialConversionList) {
            // TODO: move to function
            var zv04IChangeList = from zv04Item in zvo4iList
                                  where string.IsNullOrEmpty(zv04Item.rejReason)
                                  join shipConversion in shipToConversionList on zv04Item.shipTo equals shipConversion.shipTo
                                  join materialConversion in materialConversionList on zv04Item.material equals materialConversion.material
                                  select new { zv04Item.order, zv04Item.shipToName, zv04Item.soldToName, zv04Item.soldTo, zv04Item.item, zv04Item.shipTo, zv04Item.material, zv04Item.orderQty, materialConversion.conversionIndex };

            // TODO: refactor
            var baseSalesDocumentQuery = from q in zv04IChangeList
                                         group q by new { q.order, q.shipTo, q.soldTo, q.shipToName, q.soldToName } into line
                                         select new QtyConversionOrderProperty() {
                                             orderNumber = line.Key.order,
                                             shipTo = line.Key.shipTo,
                                             soldTo = line.Key.soldTo,
                                             shipToName = line.Key.shipToName,
                                             soldToName = line.Key.soldToName,
                                             documentLineList = line.Aggregate(new List<DocumentLine>(), (x, q) => {
                                                 x.Add(new DocumentLine() { item = Conversions.ToInteger(q.item), material = Conversions.ToInteger(q.material), quantity = Conversions.ToInteger(q.orderQty) });
                                                 return x;
                                             }),
                                             documentLineChangeList = line.Aggregate(new List<DocumentLine>(), (x, q) => {
                                                 x.Add(new DocumentLine() { item = Conversions.ToInteger(q.item), material = Conversions.ToInteger(q.material), quantity = (int)(q.orderQty * (double)q.conversionIndex) });
                                                 return x;
                                             })
                                         };
            return baseSalesDocumentQuery.ToList();
        }

        public List<QtyConversionOrderProperty> removeAlreadyConvertedLines(List<ConversionLog> logConversionList, ref List<QtyConversionOrderProperty> salesDocumetList) {
            for (int i = salesDocumetList.Count - 1; i >= 0; i -= 1) {
                foreach (var logLine in logConversionList) {
                    // delete line if was already converted
                    if (salesDocumetList[i].orderNumber == logLine.orderNumber && isSuccessfullyConvertedLineAlreadyInLog(salesDocumetList[i], logLine)) {
                        salesDocumetList[i].removeLineByItem(logLine.item);
                        // delete document if all items in the document were changed previously
                        if (salesDocumetList[i].documentLineChangeList.Count == 0) {
                            salesDocumetList.RemoveAt(i);
                            
                            //exists function if all items were removed out of the list
                            if (salesDocumetList.Count == 0) { return salesDocumetList;}

                            break;
                        }
                    }
                }
            }
            return salesDocumetList;
        }

        private bool isSuccessfullyConvertedLineAlreadyInLog(QtyConversionOrderProperty document, ConversionLog logLine) {
            for (int i = document.documentLineChangeList.Count - 1; i >= 0; i -= 1) {
                if (document.documentLineChangeList[i].item == logLine.item && logLine.isConverted && logLine.isSaved) {
                    return true;
                }
            }

            return false;
        }
    }
}