using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service {
    public static class DEDeliveryBlocksCalculator {
        /// <summary>
        /// Delivery blocks in Germany are applied on orders that either dont meet the minimum order value or quantity
        /// Orders on block Y2 & Z9 are ignored
        /// Those orders are blocked wih delivery block code ZF
        /// Exception to this rule is if the order has a display sku. Those sku's had "DP" or "DISPLAY" in the description. ZF block for them is removed
        /// If the order is already blocked with ZF but has had it quantiy or/and value changed to the qualifying amount the delivery block is lifted
        /// Also checks if the block has been lifted previously by user and does not reapply blocks 2nd time
        /// </summary>
        /// <param name="dataCollectorServer"></param>
        /// <param name="dataCollectorSap"></param>
        /// <param name="salesOrg"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<DeliveryBlocksProperty> getDelBlockListDE01(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {

            List<ZV04IProperty> zvIList = dataCollectorSap.getZV04IList(salesOrg, IDAEnum.Task.deliveryBlocks);

            List<ZV04HNProperty> zvHNList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks);

            if (zvIList is null) { return null; }

            zvHNList = zvHNList
                .Where(x => (x.status.ToLower() == "open order" || x.status.ToLower() == "credit hold") && 
                    x.delBlock != IDAConsts.DelBlocks.investigationPendingBlock && 
                    x.delBlock != IDAConsts.DelBlocks.leadTimeBlock)
                .ToList();

            List<CustomerDataProperty> cdList = dataCollectorServer.getCustomerDataList(salesOrg);

            List<DeliveryBlocksProperty> joinedList = getJoinedList(id, zvIList, zvHNList, cdList);

            List<DeliveryBlocksProperty> groupedList = getGroupedList(joinedList);

            List<DeliveryBlocksProperty> blockList = getBlockList(groupedList);

            //get previously blocked orders from log
            List<DeliveryBlocksProperty> blockedOrdersFromFromLogList = dataCollectorServer.getDelBlockLogList(salesOrg);
            List<int> previouslyBlockedOrderList = blockedOrdersFromFromLogList.Select(x => x.orderNumber).ToList();

            //remove previously blocked orders that have been unblocked by users in order to not reapply blocks
            blockList.RemoveAll(x => previouslyBlockedOrderList.Contains(x.orderNumber));

            List<OrderWithDisplays> orderWithDisplaysList = (from zvi in zvIList
                                                             where (zvi.materialDescription.ToUpper().Contains("DP") || zvi.materialDescription.ToUpper().Contains("DISPLAY")) &&
                                                                 (zvi.status.ToLower() == "open order" || zvi.status.ToLower() == "credit hold") &&
                                                                 zvi.delBlock == IDAConsts.DelBlocks.belowMOQDelBlock
                                                             group zvi by zvi.order into g
                                                             select new OrderWithDisplays(g.Key)
                                                             )
                                                             .Distinct()
                                                             .ToList();

            //remove orders with displays from blocklist
            blockList = blockList.Where(x => orderWithDisplaysList.Any(y => y.orderNumber != x.orderNumber)).ToList();

            List<DeliveryBlocksProperty> unBlockList = getUnBlockList(groupedList, orderWithDisplaysList);

            List<DeliveryBlocksProperty> finalList = new List<DeliveryBlocksProperty>();

            finalList.AddRange(blockList);
            finalList.AddRange(unBlockList);

            return finalList;
        }

        private static List<DeliveryBlocksProperty> getJoinedList(string id, List<ZV04IProperty> zvIList, List<ZV04HNProperty> zvHNList, List<CustomerDataProperty> cdList) {
            return (
                from zvi in zvIList
                join cd in cdList on new { key0 = zvi.soldTo, key1 = zvi.shipTo } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                join zvh in zvHNList on new { key0 = zvi.soldTo, key1 = zvi.shipTo } equals new { key0 = zvh.soldto, key1 = zvh.shipto }
                select getDelBlockProperty(id, zvi, cd, zvh)
                ).ToList();
        }

        private static DeliveryBlocksProperty getDelBlockProperty(string id, ZV04IProperty zvi, CustomerDataProperty cd, ZV04HNProperty zvh) {
            return new DeliveryBlocksProperty(
                                        id: id,
                                        orderStatus: zvh.status,
                                        salesOrg: cd.salesOrg,
                                        country: cd.country,
                                        orderNumber: zvi.order,
                                        poNumber: zvh.pONumber,
                                        soldTo: zvi.soldTo,
                                        soldToName: zvi.soldToName,
                                        shipTo: zvi.shipTo,
                                        shipToName: zvi.shipToName,
                                        currentDeliveryBlock: zvi.delBlock,
                                        newDeliveryBlock: null,
                                        currentQty: zvi.orderQty,
                                        minQty: cd.minimumOrderCaseQuantity,
                                        currentVal: zvi.itemNetValue,
                                        minVal: cd.minimumOrderValue,
                                        reason: null,
                                        customerEmails: null,
                                        poDate: zvh.pODate,
                                        rdd: zvh.reqDelDate
                                        );
        }

        private static List<DeliveryBlocksProperty> getGroupedList(List<DeliveryBlocksProperty> joinedList) {
            return joinedList.GroupBy(list => list.orderNumber).
                Select(lg => new DeliveryBlocksProperty(
                    id: lg.First().id,
                    orderStatus: lg.First().orderStatus,
                    salesOrg: lg.First().salesOrg,
                    country: lg.First().country,
                    orderNumber: lg.Key,
                    poNumber: lg.First().poNumber,
                    soldTo: lg.First().soldTo,
                    soldToName: lg.First().soldToName,
                    shipTo: lg.First().shipTo,
                    shipToName: lg.First().shipToName,
                    currentDeliveryBlock: lg.First().currentDeliveryBlock,
                    newDeliveryBlock: lg.First().newDeliveryBlock,
                    currentQty: lg.Sum(cq => cq.currentQty),
                    minQty: lg.First().minQty,
                    currentVal: lg.Sum(cs => cs.currentVal),
                    minVal: lg.First().minVal,
                    reason: lg.First().reason,
                    customerEmails: null,
                    poDate: lg.First().poDate,
                    rdd: lg.First().rdd
                    )).ToList();
        }

        private static List<DeliveryBlocksProperty> getBlockList(List<DeliveryBlocksProperty> groupedList) {
            List<DeliveryBlocksProperty> tempBlockList = groupedList
                .Where(x =>
                 ((x.currentQty < x.minQty && x.minQty > 0) || (x.currentVal < x.minVal) && x.minVal > 0) &&
                 x.currentDeliveryBlock.ToUpper() != IDAConsts.DelBlocks.belowMOQDelBlock
                )
                .ToList();

            if (tempBlockList.Count > 0) {
                tempBlockList.ForEach(x => x.newDeliveryBlock = IDAConsts.DelBlocks.belowMOQDelBlock);
                tempBlockList.Where(x => x.minVal != 0 && x.minVal > x.currentVal).ToList().ForEach(x => x.reason += $"Minimum value not met - needed {x.minVal - x.currentVal} more.");
                tempBlockList.Where(x => x.minQty != 0 && x.minQty > x.currentQty).ToList().ForEach(x => x.reason += $"Minimum qty not met - needed {x.minQty - x.currentQty} more.");
            }

            return tempBlockList;
        }

        private static List<DeliveryBlocksProperty> getUnBlockList(List<DeliveryBlocksProperty> groupedList, List<OrderWithDisplays> orderWithDisplaysList) {

            List<DeliveryBlocksProperty> unblockListWithDisplays = groupedList.Where(x => orderWithDisplaysList.Any(y => x.orderNumber == y.orderNumber)).ToList();

            if (unblockListWithDisplays.Count > 0) {
                unblockListWithDisplays.ForEach(x => x.newDeliveryBlock = IDAConsts.DelBlocks.noBlock);
                unblockListWithDisplays.ForEach(x => x.reason = "Order contains a display.");
            }

            List<DeliveryBlocksProperty> tempUnBlockList = groupedList
                .Where(x =>
                    (x.currentQty >= x.minQty && x.minQty > 0) &&
                    (x.currentVal >= x.minVal && x.minVal > 0) &&
                    string.IsNullOrEmpty(x.currentDeliveryBlock)
                )
                .ToList();

            if (tempUnBlockList.Count > 0) {
                tempUnBlockList.ForEach(x => x.newDeliveryBlock = IDAConsts.DelBlocks.noBlock);
                tempUnBlockList.ForEach(x => x.reason = $"Minimum quantity of {x.minQty} or minimum value of {x.minVal} has been met.");
            }

            tempUnBlockList.AddRange(unblockListWithDisplays);

            return tempUnBlockList;
        }
    }

    class OrderWithDisplays {
        public int orderNumber { get; set; }

        public OrderWithDisplays(int orderNumber) {
            this.orderNumber = orderNumber;
        }

        public override bool Equals(object obj) {
            return obj is OrderWithDisplays displays &&
                   orderNumber == displays.orderNumber;
        }

        public override int GetHashCode() {
            return -1615114010 + orderNumber.GetHashCode();
        }
    }
}