using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service.CountryCalculators.Support {
    class CalculatorSupport {

        public static List<DeliveryBlocksProperty> getFinalListShipToAndRequestedDeliveryDateCriteria(List<DeliveryBlocksProperty> joinedList) {
            List<SummedOrderValues> summedList = getSummedList(joinedList);

            List<DeliveryBlocksProperty> blockList = getBlockList(joinedList, summedList);

            List<DeliveryBlocksProperty> unblockList = getUnblockList(joinedList, summedList);

            List<DeliveryBlocksProperty> finalList = new List<DeliveryBlocksProperty>();

            finalList.AddRange(blockList);
            finalList.AddRange(unblockList);

            return finalList;
        }

        private static List<SummedOrderValues> getSummedList(List<DeliveryBlocksProperty> joinedList) {
            return joinedList.
                    GroupBy(x => new { x.shipTo, x.rdd }).
                    Select(g => new SummedOrderValues(g.Key.shipTo, g.Key.rdd, g.Sum(s => s.currentVal), g.Sum(s => s.currentQty))).ToList();
        }

        private static List<DeliveryBlocksProperty> getBlockList(List<DeliveryBlocksProperty> joinedList, List<SummedOrderValues> summedList) {

            List<DeliveryBlocksProperty> blockList =
                joinedList.
                    Where(x => summedList.
                        Any(y => x.shipTo == y.shipTo && x.rdd == y.rdd &&
                                (
                                    (x.minQty > y.totalQty && x.minVal == 0) ||
                                    (x.minVal > y.totalValue && x.minQty == 0) ||
                                    (x.minVal > y.totalValue && x.minQty > y.totalQty && x.minQty > 0 && x.minVal > 0)
                                ) &&
                            string.IsNullOrEmpty(x.currentDeliveryBlock) &&
                            (x.orderStatus.ToLower() == "open order" || x.orderStatus.ToLower() == "credit hold")
                            )).ToList();

            blockList.ForEach(z => {
                z.newDeliveryBlock = IDAConsts.DelBlocks.belowMOQDelBlock;
                    z.currentQty = summedList.Where(y => y.shipTo == z.shipTo).First().totalQty;
                    z.currentVal = summedList.Where(x => x.shipTo == z.shipTo).First().totalValue;
                }
            );

            blockList.ForEach(x => x.reason = $"Blocked due to insufficient summed up { (x.minQty > x.currentQty ? "quantity" : "value") } for all current ship to orders " +
                                                                $"of { (x.minQty > x.currentQty ? x.currentQty : Math.Round(x.currentVal, 2)) } " +
                                                                $"out of minimum { (x.minQty > x.currentQty ? x.minQty : x.minVal) }");

            return blockList;
        }

        private static List<DeliveryBlocksProperty> getUnblockList(List<DeliveryBlocksProperty> joinedList, List<SummedOrderValues> summedList) {
            List<DeliveryBlocksProperty> unblockList =
              joinedList.
                  Where(x => summedList.
                      Any(y => x.shipTo == y.shipTo && x.rdd == y.rdd &&
                                (
                                    (x.minQty <= y.totalQty && x.minVal == 0) ||
                                    (x.minVal <= y.totalValue && x.minQty == 0) ||
                                    ((x.minVal <= y.totalValue || x.minQty <= y.totalQty) && x.minQty > 0 && x.minVal > 0)
                                  ) &&
                            x.currentDeliveryBlock == IDAConsts.DelBlocks.belowMOQDelBlock)).ToList();

            unblockList.ForEach(z => {
                    z.newDeliveryBlock = IDAConsts.DelBlocks.noBlock;
                    z.currentQty = summedList.Where(y => y.shipTo == z.shipTo && y.rdd == z.rdd).First().totalQty;
                    z.currentVal = summedList.Where(x => x.shipTo == z.shipTo && x.rdd == z.rdd).First().totalValue;
                }
            );

            unblockList.ForEach(x => x.reason = $"Block removed due to reaching sufficient summed up { (x.currentQty > x.minQty ? "quantity" : "value") } for all current ship to orders " +
                                                                $"of { (x.currentQty > x.minQty ? x.currentQty : Math.Round(x.currentVal)) } " +
                                                                $"out of minimum { (x.currentQty > x.minQty ? x.minQty : x.minVal) } ");
            return unblockList;
        }
    }
}
