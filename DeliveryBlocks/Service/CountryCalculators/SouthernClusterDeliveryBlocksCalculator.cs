using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service {
    public static class SouthernClusterDeliveryBlocksCalculator {
        /// <summary>
        /// Removing all delivery blocks by conditions:
        /// RO01 - remove all blocks apart from Z9 and Y2
        /// IT01 - remove all blocks apart from  Z9, Y2 and Z4
        /// GR01 - remove all ZF blocks
        /// </summary>
        /// <param name="dataCollectorServer"></param>
        /// <param name="dataCollectorSap"></param>
        /// <param name="salesOrg"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<DeliveryBlocksProperty> getDelBlockListRO01(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {
            List<ZV04HNProperty> zvHNList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks);

            if (zvHNList is null) { return null; }

            switch (salesOrg.ToUpper()) {
                case "RO01":
                    zvHNList = zvHNList
                        .Where(x => !string.IsNullOrEmpty(x.delBlock) &&
                                    x.delBlock.ToUpper() != IDAConsts.DelBlocks.leadTimeBlock &&
                                    x.delBlock.ToUpper() != IDAConsts.DelBlocks.investigationPendingBlock)
                        .ToList();
                    break;
                case "IT01":
                    zvHNList = zvHNList
                        .Where(x => !string.IsNullOrEmpty(x.delBlock) &&
                                    x.delBlock.ToUpper() != IDAConsts.DelBlocks.leadTimeBlock &&
                                    x.delBlock.ToUpper() != IDAConsts.DelBlocks.investigationPendingBlock &&
                                    x.delBlock.ToUpper() != IDAConsts.DelBlocks.exportPaperMissingBlock &&
                                    x.delBlock.ToUpper() != IDAConsts.DelBlocks.GOEBlock)
                        .ToList();
                    break;
                case "GR01":
                    zvHNList = zvHNList
                        .Where(x => x.delBlock.ToUpper() == IDAConsts.DelBlocks.belowMOQDelBlock)
                        .ToList();
                    break;
                default:
                    throw new NotImplementedException($"There is no such sales org - {salesOrg} in southern cluster");
            }

            List<CustomerDataProperty> cdList = dataCollectorServer.getCustomerDataList(salesOrg);

            List<DeliveryBlocksProperty> finalList = (from zv in zvHNList
                                                      join cd in cdList on new { key0 = zv.soldto, key1 = zv.shipto } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                                                      select getDelBlockProperty(salesOrg, id, zv, cd)
                                                     )
                                                     .ToList();

            return finalList;
        }

        private static DeliveryBlocksProperty getDelBlockProperty(string salesOrg, string id, ZV04HNProperty zv, CustomerDataProperty cd) {
            return new DeliveryBlocksProperty(
                                            id: id,
                                            orderStatus: zv.status,
                                            salesOrg: salesOrg,
                                            country: cd.country,
                                            orderNumber: zv.order,
                                            poNumber: zv.pONumber,
                                            soldTo: zv.soldto,
                                            soldToName: zv.soldtoName,
                                            shipTo: zv.shipto,
                                            shipToName: zv.shiptoName,
                                            currentDeliveryBlock: zv.delBlock,
                                            newDeliveryBlock: IDAConsts.DelBlocks.noBlock,
                                            currentQty: default,
                                            minQty: default,
                                            currentVal: default,
                                            minVal: default,
                                            reason: "Removing delivery block for orders to be released",
                                            customerEmails: null,
                                            poDate: zv.pODate,
                                            rdd: zv.reqDelDate
                                            );
        }
    }
}
