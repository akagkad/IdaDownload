using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RDD {
    public class DataCollectorServiceRDD {
        private readonly DataCollectorServer dataCollectorServer;
        private readonly DataCollectorSap dataCollectorSap;

        public DataCollectorServiceRDD(DataCollectorServer dataCollectorServer, DataCollectorSap dataCollectorSap) {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
        }

        public List<RddOutputBean> GetRddList(string salesOrg, string id) {

            var ZVList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.rdd);

            if (ZVList is null) {
                return null;
            } else {
                ZVList = ZVList.Where(x => getZVCondition(x, salesOrg)).ToList();
            }

            var customerDataList = dataCollectorServer.getCustomerDataList(salesOrg);

            var query = (from zv in ZVList
                          join cm in customerDataList on new { key0 = zv.shipto, key1 = zv.soldto } equals new { key0 = cm.shipTo, key1 = cm.soldTo }
                          select new RddOutputBean(zv, cm, id)).ToList();
            return query;
        }

        public List<BankHolidayProperty> getBHList(string salesOrg) {
            return dataCollectorServer.getBHList(salesOrg);
        }

        private bool getZVCondition(ZV04HNProperty zv, string salesOrg) {

            bool flag = (zv.delBlock ?? "") != IDAConsts.DelBlocks.leadTimeBlock;

            switch (salesOrg) {
                case "ZA01":
                case "NG01":
                case "KE02": {
                        flag = flag && (zv.delBlock ?? "") != "Z4" && (zv.delBlock ?? "") != "04";
                        break;
                    }

                case "ES01":
                case "PT01": {
                        flag = flag && (zv.delBlock ?? "") != "ZG";
                        break;
                    }

                case "PL01":
                case "CZ01": {
                        flag = flag && (zv.delBlock ?? "") != "Z8" && (zv.delBlock ?? "") != "ZV" && (zv.delBlock ?? "") != "ZW";
                        break;
                    }
                case "RO01": {
                        flag = flag && zv.route != "ROEMGY";
                        break;
                    }

                default: {
                        break;
                    }
            }

            return flag;
        }
    }
}