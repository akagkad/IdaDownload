using DeliveryBlocks.Service.CountryCalculators;
using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Service;
using lib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryBlocks.Service {
    public class DataCollectorServiceDeliveryBlocks : IDataCollectorServiceDeliveryBlocks {
        private readonly IDataCollectorServer dataCollectorServer;
        private readonly IDataCollectorSap dataCollectorSap;
        private readonly string salesOrg;
        private readonly string id;

        public DataCollectorServiceDeliveryBlocks(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap, string salesOrg, string id) {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
            this.salesOrg = salesOrg;
            this.id = id;
        }

        public List<DeliveryBlocksProperty> getDelBlockList() {

            List<DeliveryBlocksProperty> list = new List<DeliveryBlocksProperty>();

            switch (salesOrg.ToUpper()) {
                case "RU01":
                    list = RUDeliveryBlockCalculator.getDelBlockListRU01(dataCollectorServer, dataCollectorSap, salesOrg, id);
                    break;

                case "RO01":
                case "IT01":
                case "GR01":
                    list = SouthernClusterDeliveryBlocksCalculator.getDelBlockListRO01(dataCollectorServer, dataCollectorSap, salesOrg, id);
                    break;

                case "DE01":
                    list = DEDeliveryBlocksCalculator.getDelBlockListDE01(dataCollectorServer, dataCollectorSap, salesOrg, id);
                    break;

                case "ZA01":
                    list = ZADeliveryBlocksCalculator.getDelBlockListZA01(dataCollectorServer, dataCollectorSap, salesOrg, id);
                    break;

                case "FR01":
                    list = FRDeliveryBlocksCalculator.getDelBlockListFR01(dataCollectorServer, dataCollectorSap, salesOrg, id);
                    break;

                case "CZ01":
                    list = CZDeliveryBlocksCalculator.getDelBlockListCZ01(dataCollectorServer, dataCollectorSap, salesOrg, id);
                    break;

                case "GB01":
                    list = GBDeliveryBlocksCalculator.getDelBlockListGB01(dataCollectorServer, dataCollectorSap, salesOrg, id);
                    break;
                case "ES01":
                case "PT01":
                    //No Delivery blocks action for these countries
                    break;

                default:
                    throw new NotImplementedException($"No implementation found for calulating Del blocks for sales Org: {salesOrg}");
            }
                return list.OrderBy(x => x.newDeliveryBlock).ThenBy(x => x.shipTo).ToList();
        }
    }
}
