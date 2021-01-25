using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.TcodeProperty.VA02;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace IDAUtil.Model.Properties.ServerProperty {
    public class RejectionsOrderPropertyFactory : IRejectionsOrderPropertyFactory {
        public RejectionsOrderPropertyFactory() {
        }

        public List<RejectionsSapOrderProperty> getSapRejectionsObjectList(List<RejectionsProperty> rpl) {
            var sapRejectionObjectQuery = (from r in rpl
                                        group r by new { r.orderNumber, r.salesOrg } into line
                                        select new RejectionsSapOrderProperty() {
                                            salesOrg = line.Key.salesOrg,
                                            orderNumber = line.Key.orderNumber,
                                            lineDetails = line.Aggregate(new List<RejectionsSapLineProperty>(), (x, q) => {
                                                x.Add(new RejectionsSapLineProperty() {
                                                    lineNumber = q.item,
                                                    sku = q.sku,
                                                    orderedQty = q.orderedQty,
                                                    confirmedQty = q.confirmedQty,
                                                    rejectionCode = q.rejectionReasonCode.ToUpper(),
                                                    isReplacePartialCut = q.isReplacePartialCut,
                                                    reason = $"{((q.rejectionForCustomer ?? "") == "All" ? "Country level rejection. " : "Speciffic rejection for " + q.soldTo + ". ")} {(q.needOutOfStockToReject == true ? "Sku " + q.sku + " is out of stock for this order. (order qty - " + q.orderedQty + " confirmed qty - " + q.confirmedQty + " ) " : "Rejection was performed regardless of stock levels. " + q.rejectionComment)}"
                                                });
                                                return x;
                                            })
                                        }).ToList();
            return sapRejectionObjectQuery;
        }
    }
}