using DistressReport.Model;
using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.TcodeProperty.CO09Obj;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using lib;
using System.Collections.Generic;
using System.Linq;

namespace DistressReport.Service {
    class DataCollectorServiceDistress : IDataCollectorServiceDistress {

        private readonly IDataCollectorServer dataCollectorServer;
        private readonly IDataCollectorSap dataCollectorSap;

        public DataCollectorServiceDistress(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap) {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
        }

        public List<GenericDistressProperty> getDistressList(string salesOrg) {

            List<ZV04IProperty> zvIList = dataCollectorSap.getZV04IList(salesOrg, IDAEnum.Task.distress);

            if (zvIList is null) {
                return null;
            } else {
                zvIList = zvIList.Where(x => (x.orderQty > x.confirmedQty || x.confirmedQty == 0) && x.rejReason != "ZR").ToList();
            }

            List<ZV04HNProperty> zvHNList = dataCollectorSap.getZV04HNList(salesOrg, IDAEnum.Task.distress) ?? new List<ZV04HNProperty>();
            List<ZV04PProperty> zvPList = dataCollectorSap.getZV04PList(salesOrg, IDAEnum.Task.distress) ?? new List<ZV04PProperty>();
            List<CriticalItemsDataProperty> ciList = dataCollectorServer.getCriticalItemsDataList(salesOrg) ?? new List<CriticalItemsDataProperty>();
            List<CustomerDataProperty> cdList = dataCollectorServer.getCustomerDataList(salesOrg) ?? new List<CustomerDataProperty>();
            List<RejectionsProperty> rdList = dataCollectorServer.getRejectionsLogList(salesOrg) ?? new List<RejectionsProperty>();
            List<SwitchesProperty> sdList = dataCollectorServer.getSwitchesLogList(salesOrg) ?? new List<SwitchesProperty>();
            List<MM03Property> mmList = dataCollectorServer.getMM03List(salesOrg) ?? new List<MM03Property>();
            List<SkuDataProperty> skuList = dataCollectorServer.getSkuDataList(salesOrg) ?? new List<SkuDataProperty>();


            //Left outer join in linq done through "into group" giving a default value for objects that are null joined
            List<GenericDistressProperty> list = getGenericDistressList(salesOrg: salesOrg, zv04iObj: zvIList,
                                                                        zv04hnObj: zvHNList, zv04pObj: zvPList,
                                                                        criticalItemObj: ciList, customerDataObj: cdList,
                                                                        rejectionDataObj: rdList, switchesDataObj: sdList,
                                                                        materialMasterObj: mmList, skuObj: skuList);

            List<GenericDistressProperty> finalListWithStock = getFinalListWithStockDetails(list: list, salesOrg: salesOrg);

            finalListWithStock.OrderBy(x => x.order).ThenBy(x => x.item);

            return list;
        }

        private List<GenericDistressProperty> getGenericDistressList(string salesOrg, List<ZV04IProperty> zv04iObj, List<ZV04HNProperty> zv04hnObj, List<ZV04PProperty> zv04pObj,
                                                                     List<CriticalItemsDataProperty> criticalItemObj, List<CustomerDataProperty> customerDataObj,
                                                                     List<RejectionsProperty> rejectionDataObj, List<SwitchesProperty> switchesDataObj, List<MM03Property> materialMasterObj, List<SkuDataProperty> skuObj) {
            return (
                  from zvi in zv04iObj
                  join cd in customerDataObj on new { key0 = zvi.soldTo, key1 = zvi.shipTo } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                  join zvh in zv04hnObj on zvi.order equals zvh.order
                  join zvp in zv04pObj on new { key0 = zvi.material, key1 = zvi.order, key2 = zvi.item } equals new { key0 = zvp.material, key1 = zvp.order, key2 = zvp.item }
                  into zvpGroup
                  from zvpg in zvpGroup.DefaultIfEmpty(defaultValue: new ZV04PProperty { custMatNumb = "" })
                  join sd in switchesDataObj on new { key0 = zvi.material, key1 = cd.country.ToLower() } equals new { key0 = sd.oldSku, key1 = sd.country.ToLower() }
                  into sdGroup
                  from sdg in sdGroup.DefaultIfEmpty(defaultValue: new SwitchesProperty { newSku = 0 })
                  join rd in rejectionDataObj on new { key0 = zvi.material, key1 = cd.country.ToLower(), key2 = zvi.soldTo, key3 = zvi.shipTo } equals new { key0 = rd.sku, key1 = rd.country.ToLower(), key2 = rd.soldTo, key3 = rd.shipTo }
                  into rdGroup
                  from rdg in rdGroup.DefaultIfEmpty(new RejectionsProperty { rejectionReasonCode = "" })
                  join mm in materialMasterObj on new { key0 = zvi.material, key1 = cd.salesOrg.ToUpper() } equals new { key0 = mm.material, key1 = mm.organisation.ToUpper() }
                  into mmGroup
                  from mmg in mmGroup.DefaultIfEmpty(new MM03Property { dChainStatus = "", unitEAN = "", caseEAN = "" })
                  join ci in criticalItemObj on new { key0 = zvi.material, key1 = cd.salesOrg.ToUpper() } equals new { key0 = ci.sku, key1 = salesOrg.ToUpper() }
                  into ciGroup
                  from cig in ciGroup.DefaultIfEmpty(new CriticalItemsDataProperty { comments = "" })
                  join sku in skuObj on new { key0 = zvi.material, key1 = cd.salesOrg.ToUpper() } equals new { key0 = sku.sku, key1 = salesOrg.ToUpper() }
                  into skuGroup
                  from skug in skuGroup.DefaultIfEmpty(new SkuDataProperty { standardOrRepack = "" })
                  select getDistressProperty(zvi, zvh, zvpg, cd, sdg, rdg, mmg, cig, skug)
                  )
                  .Distinct()
                  .ToList();
        }

        private GenericDistressProperty getDistressProperty(ZV04IProperty zvi, ZV04HNProperty zvh, ZV04PProperty zvp,
                                                            CustomerDataProperty cd, SwitchesProperty sd,
                                                            RejectionsProperty rd, MM03Property mm,
                                                            CriticalItemsDataProperty ci, SkuDataProperty sku) {
            return new GenericDistressProperty(
                                                orderStatus: zvh.status,
                                                country: cd.country ?? "",
                                                plant: zvi.plant,
                                                material: zvi.material,
                                                order: zvi.order,
                                                item: zvi.item,
                                                materialDescription: zvi.materialDescription,
                                                soldTo: zvi.soldTo,
                                                soldToName: zvi.soldToName,
                                                shipTo: zvi.shipTo,
                                                shipToName: zvi.shipToName,
                                                rejReason: zvi.rejReason,
                                                afterReleaseRej: (zvi.rejReason != "" ? "" : rd.rejectionReasonCode) ?? "",
                                                possibleSwitch: sd.newSku,
                                                possibleSwitchDescription: sd.newSkuDescription,
                                                deliveryBlock: zvi.delBlock,
                                                atp: default,
                                                recoveryDate: default,
                                                recoveryQty: default,
                                                dChainStatus: mm.dChainStatus ?? "not available",
                                                criticalItemComment: ci.comments ?? "",
                                                poDate: zvi.pODate,
                                                rdd: zvh.reqDelDate,
                                                poNumber: zvi.pONumber ?? "",
                                                caseBarcode: mm.caseEAN ?? "not available",
                                                unitBarcode: mm.unitEAN ?? "not available",
                                                orderQty: zvi.orderQty,
                                                confirmedQty: zvi.confirmedQty,
                                                cutQty: (zvi.orderQty - zvi.confirmedQty),
                                                accountManager: cd.accountManager,
                                                releaseDate: zvi.loadingDate,
                                                cmir: zvp.custMatNumb,
                                                loadingDate: zvh.loadingDate,
                                                docType: zvi.docTyp,
                                                delPriority: zvi.delPriority,
                                                starndardOrRepack: sku.standardOrRepack,
                                                routeCode: zvh.route,
                                                switchComment: sd.switchComment
                                            );
        }

        private List<GenericDistressProperty> getFinalListWithStockDetails(List<GenericDistressProperty> list, string salesOrg) {
            var sap = Create.sapLib();
            var co09 = new CO09(sap);
            var listOfUniqueSkus = (from x in list select x.material).Distinct().ToList();
            var co09ObjSkuList = new List<CO09Property>();

            foreach (var item in listOfUniqueSkus) {
                co09ObjSkuList.Add(co09.getStockDetails(item, salesOrg));
            }

            // assigns value to final list passed to func from CO09 lists
            foreach (var listItem in list) {
                foreach (var co09Item in co09ObjSkuList) {
                    if (listItem.material == co09Item.sku) {
                        listItem.atp = co09Item.ATP;
                        listItem.recoveryDate = co09Item.recoveryDate;
                        listItem.recoveryQty = co09Item.recoveryQty;
                        break;
                    }
                }
            }

            return list;
        }
    }
}