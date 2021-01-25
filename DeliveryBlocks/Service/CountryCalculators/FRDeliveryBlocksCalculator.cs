using DeliveryBlocks.Service.CountryCalculators.Support;
using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service.CountryCalculators {
    static public class FRDeliveryBlocksCalculator {

        /// <summary>
        /// Delivery blocks in France are applied on orders that either dont meet the minimum order value or quantity per all orders currently available for each individual ship to
        /// Those orders are blocked wih delivery block code ZF
        /// Exception to this rule is if the order has a promote sku. Those sku's are marked as "REPACK" in the skuData table
        /// List has to be ordered by rdd
        /// </summary>
        /// <param name="dataCollectorServer"></param>
        /// <param name="dataCollectorSap"></param>
        /// <param name="salesOrg"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        public static List<DeliveryBlocksProperty> getDelBlockListFR01(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {

            List<DeliveryBlocksProperty> joinedList = getJoinedList(dataCollectorServer, dataCollectorSap, salesOrg, id);

            if (joinedList is null) { return null; }

            return CalculatorSupport.getFinalListShipToAndRequestedDeliveryDateCriteria(joinedList).OrderBy(x => x.rdd).ToList();
        }

        private static List<DeliveryBlocksProperty> getJoinedList(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {

            List<ZV04IProperty> zvIList = dataCollectorSap.getZV04IList(salesOrg, IDAEnum.Task.deliveryBlocks);

            if (zvIList is null) { return null; }

            List<SkuDataProperty> skuList = dataCollectorServer.getSkuDataList(salesOrg);

            List<PromoOrderShipTo> promoOrderShipToList = (
                                                            from zvi in zvIList
                                                            join skuData in skuList
                                                            on zvi.material equals skuData.sku
                                                            where skuData.standardOrRepack.ToUpper() == "REPACK"
                                                            group zvi by zvi.shipTo into g
                                                            select new PromoOrderShipTo(g.Key)
                                                           )
                                                           .Distinct()
                                                           .ToList();

            List<ZV04HNProperty> zvHNList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks);

            //remove shipTo's from joinedList that have at least 1 promotion order
            zvHNList = zvHNList
                .Where(x => !promoOrderShipToList
                    .Any(y => x.shipto == y.shipTo)
                )
                .ToList();

            List<CustomerDataProperty> cdList = dataCollectorServer.getCustomerDataList(salesOrg);

            List<DeliveryBlocksProperty> list = (
                                                from zvh in zvHNList
                                                join cd in cdList on new { key0 = zvh.soldto, key1 = zvh.shipto } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                                                select getDelBlockProperty(id, zvh, cd)
                                                )
                                                    .Where(x =>
                                                        (x.orderStatus.ToLower() != "Not Invoice" || x.orderStatus.ToLower() != "Invoiced") &&
                                                        (x.minQty > 0 || x.minVal > 0)
                                                    )
                                                    .ToList();

            return list;
        }

        private static DeliveryBlocksProperty getDelBlockProperty(string id, ZV04HNProperty zvh, CustomerDataProperty cd) {
            return new DeliveryBlocksProperty(
                                        id: id,
                                        orderStatus: zvh.status,
                                        salesOrg: cd.salesOrg,
                                        country: cd.country,
                                        orderNumber: zvh.order,
                                        poNumber: zvh.pONumber,
                                        soldTo: zvh.soldto,
                                        soldToName: zvh.soldtoName,
                                        shipTo: zvh.shipto,
                                        shipToName: zvh.shiptoName,
                                        currentDeliveryBlock: zvh.delBlock,
                                        newDeliveryBlock: null,
                                        currentQty: zvh.orderQty,
                                        minQty: cd.minimumOrderCaseQuantity,
                                        currentVal: zvh.ordNetValue,
                                        minVal: cd.minimumOrderValue,
                                        reason: null,
                                        customerEmails: null,
                                        poDate: zvh.pODate,
                                        rdd: zvh.reqDelDate
                                        );
        }
    }
}

