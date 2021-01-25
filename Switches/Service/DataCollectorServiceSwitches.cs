using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TaskProperty;
using IDAUtil.Model.Properties.TcodeProperty.CO09Obj;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using IDAUtil.Service;
using lib;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Switches {
    public class DataCollectorServiceSwitches {
        private readonly IDataCollectorServer dataCollectorServer;
        private readonly IDataCollectorSap dataCollectorSap;

        public DataCollectorServiceSwitches(IDataCollectorServer dataCollectorServer, IDataCollectorSap dataCollectorSap) {
            this.dataCollectorServer = dataCollectorServer;
            this.dataCollectorSap = dataCollectorSap;
        }

        public List<SwitchesProperty> getSwitchesList(string salesOrg, string id) {
            var ZVList = getZvList(salesOrg);
            if (ZVList is null) { return null;}

            var cdList = getCdList(salesOrg);
            var sdList = getSdList(salesOrg);

            var allSoldToList = getInitialSwitchPropertyList(id, sdList, ZVList, cdList, isSpecifficSoldTo: false);
            var SpecifficSoldToList = getInitialSwitchPropertyList(id, sdList, ZVList, cdList, isSpecifficSoldTo: true);
            var finalList = new List<SwitchesProperty>();

            if (SpecifficSoldToList.Count > 0) {

                finalList.AddRange(SpecifficSoldToList);

                //remove all speciffic sold to switch from general switches
                allSoldToList = allSoldToList
                    .Where(x => !SpecifficSoldToList
                    .Any(y => y.soldTo == x.soldTo && y.oldSku == x.oldSku))
                    .ToList();
            }

            finalList.AddRange(allSoldToList);
            finalList = getFinalListWithStockDetails(finalList, salesOrg);
            finalList.OrderBy(x => x.soldTo).ThenBy(x => x.item);

            return finalList;
        }

        private List<CustomerDataProperty> getCdList(string salesOrg) {
            return dataCollectorServer.getCustomerDataList(salesOrg);
        }

        private List<SwitchesProperty> getFinalListWithStockDetails(List<SwitchesProperty> list, string salesOrg) {
            var sap = Create.sapLib();
            var co09 = new CO09(sap);

            var listOfUniqueOldSkus = (from x in list select x.oldSku).Distinct().ToList();
            var listOfUniqueNewSkus = (from x in list select x.newSku).Distinct().ToList();

            var co09ObjOldSkuList = new List<CO09Property>();
            var co09ObjNewSkuList = new List<CO09Property>();

            foreach (var item in listOfUniqueOldSkus) co09ObjOldSkuList.Add(co09.getStockDetails(item, salesOrg));
            foreach (var item in listOfUniqueNewSkus) co09ObjNewSkuList.Add(co09.getStockDetails(item, salesOrg));

            // assigns value to final list passed to func from both old and new sku CO09 lists
            foreach (var listItem in list) {
                foreach (var co09OldItem in co09ObjOldSkuList) {
                    if (listItem.oldSku == co09OldItem.sku) {
                        listItem.oldSkuATP = co09OldItem.ATP;
                        listItem.oldSkuRecoveryDate = co09OldItem.recoveryDate;
                        listItem.oldSkuRecoveryQty = co09OldItem.recoveryQty;
                        break;
                    }
                }

                foreach (var co09Newtem in co09ObjNewSkuList) {
                    if (listItem.newSku == co09Newtem.sku) {
                        listItem.newSkuATP = co09Newtem.ATP;
                        listItem.newSkuRecoveryDate = co09Newtem.recoveryDate;
                        listItem.newSkuRecoveryQty = co09Newtem.recoveryQty;
                        break;
                    }
                }
            }
            return list;
        }

        private List<SwitchesProperty> getInitialSwitchPropertyList(string id, List<SwitchesDataProperty> sdList, List<ZV04IProperty> zvList, List<CustomerDataProperty> cdList, bool isSpecifficSoldTo) {
            return (
                    from zv in zvList
                    join cd in cdList on new { key0 = zv.soldTo, key1 = zv.shipTo } equals new { key0 = cd.soldTo, key1 = cd.shipTo }
                    join sd in sdList on new { key0 = zv.material, key1 = cd.country.ToLower() } equals new { key0 = sd.oldSku, key1 = sd.country.ToLower() }
                    where getStockConditon(sd, zv) && getSoldToCondition(isSpecifficSoldTo, zv, sd)
                    select getSwitchPropertyObjs(zv, sd, cd, id)
                    ).ToList();
        }

        private static bool getSoldToCondition(bool isSpecifficSoldTo, ZV04IProperty zv, SwitchesDataProperty sd) {
            return isSpecifficSoldTo ? sd.soldTo == zv.soldTo : sd.soldTo == 0;
        }

        private List<SwitchesDataProperty> getSdList(string salesOrg) {
            return dataCollectorServer.getSwitchesDataList(salesOrg).Where(x => DateTime.Today >= Conversions.ToDate(x.startDate) && DateTime.Today <= Conversions.ToDate(x.endDate)).ToList();
        }

        private List<ZV04IProperty> getZvList(string salesOrg) {
            var list = dataCollectorSap.getZV04IList(salesOrg, IDAEnum.Task.switches);

            if (list is null) {
                return null;
            } else {
                return list.Where(x => (x.delBlock ?? "") != IDAConsts.DelBlocks.leadTimeBlock && (x.delBlock ?? "") != "Z4" && (x.delBlock ?? "") != "04" && string.IsNullOrEmpty(x.rejReason)).ToList();
            }
        }

        private SwitchesProperty getSwitchPropertyObjs(ZV04IProperty zv, SwitchesDataProperty sd, CustomerDataProperty cd, string id) {
            return new SwitchesProperty(salesOrg: cd.salesOrg,
                                        country: cd.country,
                                        soldTo: zv.soldTo,
                                        switchForCustomer: sd.soldTo == 0 ? "All" : "Speciffic",
                                        shipTo: zv.shipTo,
                                        order: zv.order,
                                        shipToName: zv.shipToName,
                                        item: zv.item,
                                        orderedQty: zv.orderQty,
                                        confirmedQty: zv.confirmedQty,
                                        oldSku: sd.oldSku,
                                        oldSkuDescription: sd.oldSkuDescription,
                                        oldSkuCaseBarcode: sd.oldSkuCaseBarcode,
                                        oldSkuUnitBarcode: sd.oldSkuUnitBarcode,
                                        oldSkuATP: default,
                                        oldSkuRecoveryDate: default,
                                        oldSkuRecoveryQty: default,
                                        newSku: sd.newSku,
                                        newSkuDescription: sd.newSkuDescription,
                                        newSkuCaseBarcode: sd.newSkuCaseBarcode,
                                        newSkuUnitBarcode: sd.newSkuUnitBarcode,
                                        newSkuATP: default,
                                        newSkuRecoveryDate: default,
                                        newSkuRecoveryQty: default,
                                        startDate: Conversions.ToDate(sd.startDate),
                                        endDate: Conversions.ToDate(sd.endDate),
                                        needOutOfStockToSwitch: sd.needOutOfStockToSwitch,
                                        switchAutomatic: sd.switchAutomatic,
                                        switchComment: !string.IsNullOrEmpty(sd.switchComment) ? sd.switchComment.Replace("'","") : "" ,
                                        id: id);
        }

        private bool getStockConditon(SwitchesDataProperty sd, ZV04IProperty zv) {
            return (sd.needOutOfStockToSwitch == true && zv.confirmedQty < zv.orderQty) || sd.needOutOfStockToSwitch == false;
        }
    }
}