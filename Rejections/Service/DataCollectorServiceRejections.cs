using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.TcodeProperty.CO09Obj;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Rejections {
    public class DataCollectorServiceRejections : IDataCollectorServiceRejections {

        private readonly IDataCollectorServer dataCollectorServer;
        private readonly IDataCollectorSap dataCollectorSap;

        public List<RejectionsDataProperty> rdjList { get; set; }
        public List<CustomerDataProperty> cdList { get; set; }
        public List<ZV04IProperty> zvList { get; set; }

        public DataCollectorServiceRejections(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap) {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
        }

        public void populateReleaseRejectionList(string salesOrg) {
            rdjList = getRdList(salesOrg);
            cdList = getCdList(salesOrg);
            zvList = getZvList(salesOrg);
        }

        public List<RejectionsProperty> getReleaseRejectionsListFromLog(string salesOrg) {
            return dataCollectorServer.getRejectionsLogList(salesOrg).Distinct((x, y) => x.orderNumber == y.orderNumber && x.sku == y.sku).ToList();
        }

        private List<CustomerDataProperty> getCdList(string salesOrg) {
            return dataCollectorServer.getCustomerDataList(salesOrg);
        }

        private List<RejectionsDataProperty> getRdList(string salesOrg) {
            return dataCollectorServer.getRejectionsDataList(salesOrg).Where(x => DateTime.Today >= x.startDate && DateTime.Today <= x.endDate).ToList();
        }

        private List<ZV04IProperty> getZvList(string salesOrg) {
            var list = dataCollectorSap.getZV04IList(salesOrg, IDAEnum.Task.rejections);

            if (list is null) {
                return null;
            } else {
                return list.Where(x => (x.delBlock ?? "") != IDAConsts.DelBlocks.leadTimeBlock && (x.delBlock ?? "") != "Z4" && (x.delBlock ?? "") != "04" && string.IsNullOrEmpty(x.rejReason)).ToList();
            }
        }

        public List<RejectionsProperty> getFinalListWithStockDetails(List<RejectionsProperty> list, string salesOrg) {
            var sap = Create.sapLib();
            var co09 = new CO09(sap);
            var listOfUniqueSkus = (from x in list select x.sku).Distinct().ToList();
            var co09ObjSkuList = new List<CO09Property>();

            foreach (var item in listOfUniqueSkus) {
                co09ObjSkuList.Add(co09.getStockDetails(item, salesOrg));
            }

            // assigns value to final list passed to func from CO09 lists
            foreach (var listItem in list) {
                foreach (var co09Item in co09ObjSkuList) {
                    if (listItem.sku == co09Item.sku) {
                        listItem.skuATP = co09Item.ATP;
                        listItem.skuRecoveryDate = co09Item.recoveryDate;
                        listItem.skuRecoveryQty = co09Item.recoveryQty;
                        break;
                    }
                }
            }

            return list;
        }

    }
}