using IDAUtil;
using IDAUtil.Service;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CMIRReport {
    public class DataCollectorServiceCMIR {
        private readonly DataCollectorServer dataCollectorServer;
        private readonly DataCollectorSap dataCollectorSap;

        public DataCollectorServiceCMIR(DataCollectorServer dataCollectorServer, DataCollectorSap dataCollectorSap) {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
        }

        public List<MissingCMIRProperty> getMissingCMIRS(string salesOrg) {
            var PList = dataCollectorSap.getZV04PList(salesOrg, IDAEnum.Task.missingCMIRReport);
            var customerDataList = dataCollectorServer.getCustomerDataList(salesOrg);
            
            if (PList is null) { return null; }

            var query = (from p in PList
                         join cm in customerDataList on new { key0 = p.soldTo, key1 = p.shipTo } equals new { key0 = cm.soldTo, key1 = cm.shipTo }
                         where (string.IsNullOrEmpty(p.custMatNumb) && cm.cmirCheckAllowed && string.IsNullOrEmpty(p.rejReason))
                         select new MissingCMIRProperty(p.material, p.materialDescription, p.soldTo, p.soldToName, p.order, p.item)
                         ).ToList();
            return query;
        }
    }
}