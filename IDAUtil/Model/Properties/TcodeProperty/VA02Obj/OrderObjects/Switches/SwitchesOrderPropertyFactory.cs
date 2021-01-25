using IDAUtil.Model.Properties.TaskProperty;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IDAUtil {
    public class SwitchesOrderPropertyFactory {
        public SwitchesOrderPropertyFactory() {
        }

        public List<SwitchesSapOrderProperty> getSapSwitchObjectList(List<SwitchesProperty> spl) {
            var sapSwitchObjectQuery = (from q in spl
                                        group q by new { q.order, q.salesOrg, q.soldTo } into line
                                        select new SwitchesSapOrderProperty() {
                                            salesOrg = line.Key.salesOrg,
                                            orderNumber = line.Key.order,
                                            soldTo = line.Key.soldTo,
                                            lineDetails = line.Aggregate(new List<SwitchesSapLineProperty>(), (x, q) => {
                                                x.Add(new SwitchesSapLineProperty() {
                                                    lineNumber = q.item,
                                                    oldSku = q.oldSku,
                                                    newSku = q.newSku,
                                                    isSameBarcode = (q.oldSkuCaseBarcode ?? "") == (q.newSkuCaseBarcode ?? "") && (q.oldSkuUnitBarcode ?? "") == (q.newSkuUnitBarcode ?? ""),
                                                    reason = $"{((q.switchForCustomer ?? "") == "All" ? "Country level switch. " : "Speciffic switch for " + q.soldTo + " - " + q.shipToName + ". ")} {(q.needOutOfStockToSwitch == true ? "Sku " + q.oldSku + " is out of stock for this order. (order qty - " + q.orderedQty + " confirmed qty - " + q.confirmedQty + " ) " : "Switch was performed regardless of stock levels. " + q.switchComment)}"
                                                });
                                                return x;
                                            })
                                        }).ToList();
            return sapSwitchObjectQuery;
        }
    }
}