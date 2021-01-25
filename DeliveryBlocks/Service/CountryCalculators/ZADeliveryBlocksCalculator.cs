using DeliveryBlocks.Service.CountryCalculators.Support;
using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service.CountryCalculators {
    public class ZADeliveryBlocksCalculator {
        public static List<DeliveryBlocksProperty> getDelBlockListZA01(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {

            List<DeliveryBlocksProperty> joinedList = getJoinedList(dataCollectorServer, dataCollectorSap, salesOrg, id);

            if (joinedList is null) { return null; }

            return CalculatorSupport.getFinalListShipToAndRequestedDeliveryDateCriteria(joinedList);
        }

        private static List<DeliveryBlocksProperty> getJoinedList(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {

            List<ZV04HNProperty> zvHNList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks);

            if (zvHNList is null) { return null; }

            List<CustomerDataProperty> cdList = dataCollectorServer.getCustomerDataList(salesOrg);

            List<DeliveryBlocksProperty> list = (
                from zvhn in zvHNList
                join cd in cdList on new { key0 = zvhn.soldto, key1 = zvhn.shipto } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                select new DeliveryBlocksProperty(
                            id: id,
                            orderStatus: zvhn.status,
                            salesOrg: cd.salesOrg,
                            country: cd.country,
                            orderNumber: zvhn.order,
                            poNumber: zvhn.pONumber,
                            soldTo: zvhn.soldto,
                            soldToName: zvhn.shiptoName,
                            shipTo: zvhn.shipto,
                            shipToName: zvhn.shiptoName,
                            currentDeliveryBlock: zvhn.delBlock,
                            newDeliveryBlock: null,
                            currentQty: zvhn.orderQty,
                            minQty: cd.minimumOrderCaseQuantity,
                            currentVal: zvhn.ordNetValue,
                            minVal: cd.minimumOrderValue,
                            reason: null,
                            customerEmails: cd.belowMOQandMOVEmails,
                            poDate: zvhn.pODate,
                            rdd: zvhn.reqDelDate
                            )
                ).Where(x =>
                (x.orderStatus.ToLower() != "not invoice" || x.orderStatus.ToLower() != "invoiced") &&
                (x.minQty > 0 || x.minVal > 0)
            ).ToList();

            return list;
        }
    }
}
