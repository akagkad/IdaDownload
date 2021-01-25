using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using System.Collections.Generic;
using System.Linq;

namespace IDAUtil {
    public static class QtyConversionsOrderPropertyFactory {
        public static List<QtyConversionOrderProperty> createSalesDocumentList(List<ZV04IProperty> zv04IList) {

            var baseSalesDocumentQuery = (from q in zv04IList
                                          group q by new { q.order, q.shipTo, q.soldTo, q.shipToName, q.docDate, q.pONumber, q.delivery } into line
                                          select new QtyConversionOrderProperty() {
                                              orderNumber = line.Key.order,
                                              deliveryNumber = line.Key.delivery,
                                              docDate = line.Key.docDate,
                                              pONumber = line.Key.pONumber,
                                              shipTo = line.Key.shipTo,
                                              soldTo = line.Key.soldTo,
                                              soldToName = line.Key.shipToName,
                                              documentLineList = line.Aggregate(new List<DocumentLine>(), (x, q) => {
                                                  x.Add(new DocumentLine() { item = q.item, material = q.material, quantity = q.orderQty });
                                                  return x;
                                              }),
                                              documentLineChangeList = line.Aggregate(new List<DocumentLine>(), (x, q) => {
                                                  x.Add(new DocumentLine() { item = q.item, material = q.material, quantity = q.orderQty });
                                                  return x;
                                              })
                                          });

            return baseSalesDocumentQuery.ToList();
        }
    }
}