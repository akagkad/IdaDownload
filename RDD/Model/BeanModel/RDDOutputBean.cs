using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using IDAUtil.Model.Properties.TcodeProperty.ZV04Obj;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RDD {
    public abstract class OutputBean {
        [Column("[soldTo]")] public long soldTo { get; set; }
        [Column("[soldToName]")] public string soldToName { get; set; }
        [Column("[leadTime]")] public int leadTime { get; set; }
        [Column("[route]")] public string route { get; set; }
        [Column("[oldRdd]")] public DateTime oldRdd { get; set; }
        [Column("[orderNumber]")] public int orderNumber { get; set; }
        [Column("[docTyp]")] public string docTyp { get; set; }
        [Column("[shipTo]")] public long shipTo { get; set; }
        [Column("[shipToName]")] public string shipToName { get; set; }
        [Column("[salesOrg]")] public string salesOrg { get; set; }
        [Column("[country]")] public string country { get; set; }
        [Column("[isRddChangeAllowed]")] public bool isRddChangeAllowed { get; set; }
        [Column("[isRouteCodeChangeAllowed]")] public bool isRouteCodeChangeAllowed { get; set; }
        [Column("[isOneDayLeadTimeAllowed]")] public bool isOneDayLeadTimeAllowed { get; set; }
        [Column("[deliveryDay]")] public string deliveryDay { get; set; } // String of days that are available to deliver separated by " " i.e. "Monday Tuesday Friday"
        [Column("[loadingDate]")] public DateTime loadingDate { get; set; }
        [Column("[id]")] public string id { get; set; }
        [Column("[caseFillRate]")] public double caseFillRate { get; set; }
        [Column("[region]")] public string region { get; set; }

        public override bool Equals(object obj) {
            return obj is OutputBean bean &&
                   soldTo == bean.soldTo &&
                   soldToName == bean.soldToName &&
                   leadTime == bean.leadTime &&
                   route == bean.route &&
                   oldRdd == bean.oldRdd &&
                   orderNumber == bean.orderNumber &&
                   docTyp == bean.docTyp &&
                   shipTo == bean.shipTo &&
                   shipToName == bean.shipToName &&
                   salesOrg == bean.salesOrg &&
                   country == bean.country &&
                   isRddChangeAllowed == bean.isRddChangeAllowed &&
                   isRouteCodeChangeAllowed == bean.isRouteCodeChangeAllowed &&
                   isOneDayLeadTimeAllowed == bean.isOneDayLeadTimeAllowed &&
                   deliveryDay == bean.deliveryDay &&
                   loadingDate == bean.loadingDate &&
                   id == bean.id &&
                   caseFillRate == bean.caseFillRate &&
                   region == bean.region;
        }

        public override int GetHashCode() {
            var hashCode = -809470603;
            hashCode = hashCode * -1521134295 + soldTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(soldToName);
            hashCode = hashCode * -1521134295 + leadTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(route);
            hashCode = hashCode * -1521134295 + oldRdd.GetHashCode();
            hashCode = hashCode * -1521134295 + orderNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(docTyp);
            hashCode = hashCode * -1521134295 + shipTo.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(shipToName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(salesOrg);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(country);
            hashCode = hashCode * -1521134295 + isRddChangeAllowed.GetHashCode();
            hashCode = hashCode * -1521134295 + isRouteCodeChangeAllowed.GetHashCode();
            hashCode = hashCode * -1521134295 + isOneDayLeadTimeAllowed.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(deliveryDay);
            hashCode = hashCode * -1521134295 + loadingDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(id);
            hashCode = hashCode * -1521134295 + caseFillRate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(region);
            return hashCode;
        }
    }

    public class CalculatedRddOutputBean : OutputBean {
        [Column("[newRecommendedRdd]")] public DateTime newRecommendedRdd { get; set; }
        [Column("[reason]")] public string reason { get; set; }
        [Column("[delBLock]")] public string delBlock { get; set; }
        [Column("[newRecommendedRouteCode]")] public string newRecommendedRouteCode { get; set; }

        public CalculatedRddOutputBean(RddOutputBean rddOutput) {
            soldTo = rddOutput.soldTo;
            soldToName = rddOutput.soldToName;
            leadTime = rddOutput.leadTime;
            salesOrg = rddOutput.salesOrg;
            shipTo = rddOutput.shipTo;
            shipToName = rddOutput.shipToName;
            docTyp = rddOutput.docTyp;
            oldRdd = rddOutput.oldRdd;
            orderNumber = rddOutput.orderNumber;
            salesOrg = rddOutput.salesOrg;
            country = rddOutput.country;
            isRddChangeAllowed = rddOutput.isRddChangeAllowed;
            isRouteCodeChangeAllowed = rddOutput.isRouteCodeChangeAllowed;
            isOneDayLeadTimeAllowed = rddOutput.isOneDayLeadTimeAllowed;
            deliveryDay = rddOutput.deliveryDay;
            loadingDate = rddOutput.loadingDate;
            route = rddOutput.route;
            caseFillRate = rddOutput.caseFillRate;
            region = rddOutput.region;
            id = rddOutput.id;
        }

        public CalculatedRddOutputBean() {
        }
    }

    public class RddOutputBean : OutputBean {
        public RddOutputBean() {
        }

        public RddOutputBean(ZV04HNProperty zv, CustomerDataProperty cm, string id) {
            soldTo = zv.soldto;
            soldToName = zv.soldtoName;
            shipTo = zv.shipto;
            shipToName = zv.shiptoName;
            oldRdd = zv.reqDelDate;
            orderNumber = zv.order;
            docTyp = zv.docTyp;
            salesOrg = cm.salesOrg.ToUpper();
            country = cm.country.ToLower();
            isRddChangeAllowed = cm.changeRDDActionAllowed;
            isRouteCodeChangeAllowed = cm.changeRouteCodeActionAllowed;
            deliveryDay = cm.deliveryDay;
            loadingDate = zv.loadingDate;
            route = zv.route;
            caseFillRate = zv.FillRate;
            isOneDayLeadTimeAllowed = cm.oneDayLeadTimeAllowed;
            leadTime = getLeadTime(salesOrg, cm, zv);
            region = cm.region;
            this.id = id;
        }

        private int getLeadTime(string salesOrg, CustomerDataProperty cm, ZV04HNProperty zv) {
            switch (salesOrg) {

                //  case "RO01":
                case "ZA01":
                case "TR01":
                case "RU01":
                case "UA01":
                case "GR01":
                case "CZ01":
                case "PL01":
                case "IT01": {
                        return zv.route != ""  ? int.Parse(Strings.Right(zv.route, 2)) : 0;
                    }

                case "ES01": {
                        var switchExpr = zv.route;
                        switch (switchExpr) {
                            case "ES0011": {
                                    return 1;
                                }

                            case "ES0000": {
                                    return 2;
                                }

                            case "ES0002": {
                                    return 3;
                                }

                            case "ES0003": {
                                    return 4;
                                }

                            case "ESTN05": {
                                    return 5;
                                }

                            case "ESGC05": {
                                    return 5;
                                }

                            default: {
                                    return 0;
                                }
                        }
                    }

                default: {
                        return cm.leadTime;
                    }
            }
        }
    }
}