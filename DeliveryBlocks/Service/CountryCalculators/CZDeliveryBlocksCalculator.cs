using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service.CountryCalculators {
    /// <summary>
    /// Delivery Blocks in CZ are done based on confirmed qty per each order
    /// If the confirmed qty is not reached the order goes on block and an email is to the relevant stakeholder 
    /// If the confirmed qty is reached the order gets it's block removed
    /// If the order has been previously blocked (checked through log) but the order has been manually unlocked by user then the order doesn't get blocked anymore. - Requested for exceptions to minimum order rules
    /// </summary>
    class CZDeliveryBlocksCalculator {

        public static List<DeliveryBlocksProperty> getDelBlockListCZ01(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {

            List<ZV04HNProperty> zvHNList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks);
            List<CustomerDataProperty> cdList = dataCollectorServer.getCustomerDataList(salesOrg);

            List<DeliveryBlocksProperty> joinedList = getJoinedList(id, zvHNList, cdList)
                .Where(x => (x.orderStatus.ToLower() == "open order" || x.orderStatus.ToLower() == "credit hold"))
                .ToList();

            List<DeliveryBlocksProperty> blockList = getBlockList(joinedList);

            //get previously blocked orders from log
            List<DeliveryBlocksProperty> prevDelBlockList = dataCollectorServer.getDelBlockLogList(salesOrg);
            List<int> preOrdersBlockedList = prevDelBlockList.Select(x => x.orderNumber).ToList();

            //remove previously blocked orders that have been unblocked by users in order to not reapply blocks
            blockList.RemoveAll(x => preOrdersBlockedList.Contains(x.orderNumber));

            List<DeliveryBlocksProperty> unBlockList = getUnBlockList(joinedList);

            List<DeliveryBlocksProperty> finalList = new List<DeliveryBlocksProperty>();

            finalList.AddRange(blockList);
            finalList.AddRange(unBlockList);

            return finalList;
        }

        private static List<DeliveryBlocksProperty> getJoinedList(string id, List<ZV04HNProperty> zvHNList, List<CustomerDataProperty> cdList) {
            return (
                from zvh in zvHNList
                join cd in cdList on new { key0 = zvh.soldto, key1 = zvh.shipto } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                select getDelBlockProperty(id, zvh, cd)
                ).ToList();
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
                                        currentQty: zvh.confirmedQty,
                                        minQty: cd.minimumOrderCaseQuantity,
                                        currentVal: zvh.ordNetValue,
                                        minVal: cd.minimumOrderValue,
                                        reason: null,
                                        customerEmails: cd.belowMOQandMOVEmails,
                                        poDate: zvh.pODate,
                                        rdd: zvh.reqDelDate
                                        );
        }

        private static List<DeliveryBlocksProperty> getBlockList(List<DeliveryBlocksProperty> groupedList) {
            List<DeliveryBlocksProperty> tempBlockList = groupedList.Where(x =>
                 ((x.currentQty < x.minQty && x.minQty > 0) || (x.currentVal < x.minVal) && x.minVal > 0) &&
                 x.currentDeliveryBlock.ToUpper() != IDAConsts.DelBlocks.belowMOQDelBlock
             ).ToList();

            if (tempBlockList.Count > 0) {
                tempBlockList.ForEach(x => x.newDeliveryBlock = IDAConsts.DelBlocks.belowMOQDelBlock);
                tempBlockList.Where(x => x.minVal != 0 && x.minVal > x.currentVal).ToList().ForEach(x => x.reason += $"Minimum value not met - needed {x.minVal - x.currentVal} more.");
                tempBlockList.Where(x => x.minQty != 0 && x.minQty > x.currentQty).ToList().ForEach(x => x.reason += $"Minimum qty not met - needed {x.minQty - x.currentQty} more.");
            }

            return tempBlockList;
        }

        private static List<DeliveryBlocksProperty> getUnBlockList(List<DeliveryBlocksProperty> groupedList) {
            List<DeliveryBlocksProperty> tempUnBlockList = groupedList
                .Where(x =>
                    (x.currentQty >= x.minQty && x.minQty > 0) &&
                    (x.currentVal >= x.minVal && x.minVal > 0) &&
                    string.IsNullOrEmpty(x.currentDeliveryBlock)
                )
                .ToList();

            if (tempUnBlockList.Count > 0) { tempUnBlockList.ForEach(x => x.newDeliveryBlock = IDAConsts.DelBlocks.noBlock); }

            return tempUnBlockList;
        }
    }
}
