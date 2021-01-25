using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.ServerProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;

namespace Rejections.Service {
    /// <summary>
    /// Populate docs
    /// </summary>
    enum CustomerCondition {
        All,
        SoldTo,
        ShipTo
    }

    public class DataCalculatorServiceRejections : IDataCalculatorServiceRejections {
        private string id { get; set; }

        public DataCalculatorServiceRejections(string id) {
            this.id = id;
        }

        public List<RejectionsProperty> getRejectionsList(IDataCollectorServiceRejections dcsr) {
            if (dcsr.zvList is null || dcsr.zvList.Count == 0) { return null; }

            List<RejectionsProperty> allCustomerRejectionList = getInitialRejPropertyList(id, dcsr, customerCondition: CustomerCondition.All);
            List<RejectionsProperty> soldToOnlyCustomerRejectionList = getInitialRejPropertyList(id, dcsr, customerCondition: CustomerCondition.SoldTo);
            List<RejectionsProperty> shiToOnlyCustomerRejectionList = getInitialRejPropertyList(id, dcsr, customerCondition: CustomerCondition.ShipTo);

            List<RejectionsProperty> finalList = new List<RejectionsProperty>();

            if (shiToOnlyCustomerRejectionList.Count > 0) { allCustomerRejectionList = allCustomerRejectionList.Where(x => shiToOnlyCustomerRejectionList.Any(y => y.shipTo != x.shipTo && y.soldTo != x.soldTo)).ToList();  }
            if (soldToOnlyCustomerRejectionList.Count > 0) { allCustomerRejectionList = allCustomerRejectionList.Where(x => soldToOnlyCustomerRejectionList.Any(y => y.soldTo != x.soldTo)).ToList(); }

            if (shiToOnlyCustomerRejectionList.Count > 0) { soldToOnlyCustomerRejectionList = soldToOnlyCustomerRejectionList.Where(x => shiToOnlyCustomerRejectionList.Any(y => y.shipTo != x.shipTo)).ToList(); } 

            finalList.AddRange(shiToOnlyCustomerRejectionList);
            finalList.AddRange(soldToOnlyCustomerRejectionList);
            finalList.AddRange(allCustomerRejectionList);

            finalList = finalList.OrderBy(x => x.soldTo).ThenBy(x => x.shipTo).ThenBy(x => x.orderNumber).ThenBy(x => x.item).ToList();

            return finalList;
        }

        private List<RejectionsProperty> getInitialRejPropertyList(string id, IDataCollectorServiceRejections dcsr, CustomerCondition customerCondition) {
            return (
                        from zv in dcsr.zvList
                        join cd in dcsr.cdList on new { key0 = zv.soldTo, key1 = zv.shipTo } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                        join rd in dcsr.rdjList on new { key0 = zv.material, key1 = cd.country.ToLower() } equals new { key0 = rd.sku, key1 = rd.country.ToLower() }
                        where getStockConditon(rd: rd, zv: zv) && getCustomerCondition(rejCondition: customerCondition, zv: zv, rd: rd)
                        select getRejectionsPropertyObjs(zv: zv, rd: rd, cd: cd, id: id)
                    ).ToList();
        }

        private static bool getCustomerCondition(CustomerCondition rejCondition, ZV04IProperty zv, RejectionsDataProperty rd) {
            switch (rejCondition) {
                case CustomerCondition.All:
                    return rd.soldTo == 0;
                case CustomerCondition.SoldTo:
                    return rd.soldTo == zv.soldTo && rd.shipTo == 0;
                case CustomerCondition.ShipTo:
                    return rd.soldTo == zv.soldTo && rd.shipTo == zv.shipTo;
                default:
                    throw new NotImplementedException("Cound not find implementation for customer condition");
            }
        }

        private RejectionsProperty getRejectionsPropertyObjs(ZV04IProperty zv, RejectionsDataProperty rd, CustomerDataProperty cd, string id) {
            return new RejectionsProperty(
                salesOrg: cd.salesOrg,
                country: cd.country,
                soldTo: zv.soldTo,
                rejectionForCustomer: rd.soldTo == 0 ? "All" : "Speciffic",
                isReplacePartialCut: (cd.replaceObsoletePartialCutsAllowed && zv.confirmedQty > 0 && rd.needOutOfStockToReject),
                isDuringRelease: (zv.confirmedQty > 0),
                shipTo: zv.shipTo,
                shipToName: zv.shipToName,
                orderNumber: zv.order,
                item: zv.item,
                sku: rd.sku,
                rejectionReasonCode: rd.rejectionReasonCode.ToUpper(),
                orderedQty: zv.orderQty,
                confirmedQty: zv.confirmedQty,
                skuUnitBarcode: rd.skuUnitBarcode,
                skuCaseBarcode: rd.skuCaseBarcode,
                skuATP: default,
                skuRecoveryDate: null,
                skuRecoveryQty: default,
                startDate: rd.startDate,
                endDate: rd.endDate,
                needOutOfStockToReject: rd.needOutOfStockToReject,
                rejectionComment: rd.rejectionComment,
                id: id);
        }

        private bool getStockConditon(RejectionsDataProperty rd, ZV04IProperty zv) {
            return (rd.needOutOfStockToReject == true && zv.confirmedQty < zv.orderQty) || rd.needOutOfStockToReject == false;
        }
    }
}
