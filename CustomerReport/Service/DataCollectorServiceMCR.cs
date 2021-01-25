using IDAUtil;
using IDAUtil.Service;
using lib;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CustomerReport {
    public class DataCollectorServiceMCR {
        private readonly DataCollectorServer dataCollectorServer;
        private readonly DataCollectorSap dataCollectorSap;

        public DataCollectorServiceMCR(DataCollectorServer dataCollectorServer, DataCollectorSap dataCollectorSap) {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
        }

        public List<MissingCustomersProperty> getMissingCustomers(string salesOrg) {
            var ZVList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.rdd);
            var customerDataList = dataCollectorServer.getCustomerDataList(salesOrg);
            if (ZVList is null) {
                return null;
            }

            List<MissingCustomersProperty> query = (from cml in ZVList.Where(x => customerDataList.All(y => y.shipTo != x.shipto || x.soldto != y.soldTo)).ToList()
                                                    select new MissingCustomersProperty(cml.order, cml.soldto, cml.soldtoName, cml.shipto, cml.shiptoName)).Distinct((x, y) => x.soldto == y.soldto && x.shipto == y.shipto).ToList();
            return query;
        }
    }
}