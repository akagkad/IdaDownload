using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service.CountryCalculators {
    class GBDeliveryBlocksCalculator {
        /// <summary>
        /// UK removes ZV delivery blocks from orders when appointment times have been filled in
        /// UK adds ZV delivery block when appointment times are not filled in
        /// </summary>
        /// <param name="dataCollectorServer"></param>
        /// <param name="dataCollectorSap"></param>
        /// <param name="salesOrg"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<DeliveryBlocksProperty> getDelBlockListGB01(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {

            List<ZV04HNProperty> zvHNList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.deliveryBlocks)
                                                .Where(x => x.status.ToLower() == "open order" || x.status.ToLower() == "credit hold")
                                                .ToList();

            if (zvHNList is null) { return null; }

            List<AppointmentTimesSoldTo> appTimesSoldToList = dataCollectorServer.getGBAppointmentTimesCustomers();

            List<DeliveryBlocksProperty> blockList = getDeliveryBlockList(salesOrg, id, zvHNList, appTimesSoldToList, true);

            List<DeliveryBlocksProperty> unBlockList = getDeliveryBlockList(salesOrg, id, zvHNList, appTimesSoldToList, false);

            List<DeliveryBlocksProperty> finalList = new List<DeliveryBlocksProperty>();
            finalList.AddRange(blockList);
            finalList.AddRange(unBlockList);

            return finalList;
        }

        private static List<DeliveryBlocksProperty> getDeliveryBlockList(string salesOrg, string id, List<ZV04HNProperty> zvHNList, List<AppointmentTimesSoldTo> appTimesSoldToList, bool isForBlock) {
            return (from zv in zvHNList
                    join appTimesSoldTo in appTimesSoldToList
                    on zv.soldto equals appTimesSoldTo.soldTo
                    where getDelBlockCondition(isForBlock, zv, appTimesSoldTo)
                    select getDelBlockProperty(salesOrg, id, isForBlock, zv, appTimesSoldTo)
                    )
                    .ToList();
        }

        private static DeliveryBlocksProperty getDelBlockProperty(string salesOrg, string id, bool isForBlock, ZV04HNProperty zv, AppointmentTimesSoldTo appTimesSoldTo) {
            return new DeliveryBlocksProperty(id: id,
                                              orderStatus: zv.status,
                                              salesOrg: salesOrg,
                                              country: "United Kingdom",
                                              orderNumber: zv.order,
                                              poNumber: null,
                                              soldTo: zv.soldto,
                                              soldToName: zv.soldtoName,
                                              shipTo: zv.shipto,
                                              shipToName: zv.shiptoName,
                                              currentDeliveryBlock: zv.delBlock,
                                              newDeliveryBlock: $"{(isForBlock ? appTimesSoldTo.delBlock : IDAConsts.DelBlocks.noBlock)}",
                                              currentQty: default,
                                              minQty: default,
                                              currentVal: default,
                                              minVal: default,
                                              reason: $"{(isForBlock ? "Order missing appointment times" : "Order has appointment times")}",
                                              customerEmails: null,
                                              poDate: null,
                                              rdd: DateTime.Now);
        }

        private static bool getDelBlockCondition(bool isForBlock, ZV04HNProperty zv, AppointmentTimesSoldTo appTimesSoldTo) {
            if (isForBlock) {
                return (string.IsNullOrEmpty(zv.appointmentTimes) || zv.appointmentTimes == "\r\n") && (string.IsNullOrEmpty(zv.delBlock) || (zv.delBlock != appTimesSoldTo.delBlock && zv.delBlock != IDAConsts.DelBlocks.investigationPendingBlock && zv.delBlock != IDAConsts.DelBlocks.leadTimeBlock));
            } else {
                return (!string.IsNullOrEmpty(zv.appointmentTimes) && zv.appointmentTimes != "\r\n") && (zv.delBlock == IDAConsts.DelBlocks.AppointmentTimesBlock || zv.delBlock == IDAConsts.DelBlocks.holdOrderBlock);
            }
        }
    }
}
