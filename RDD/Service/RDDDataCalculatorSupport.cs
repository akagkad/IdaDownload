using IDAUtil.Model.Properties.ServerProperty;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RDD {
    public class RDDDataCalculatorSupport {
        private List<BankHolidayProperty> bankHolidayList;
        private bool isSkipWeekend;

        public RDDDataCalculatorSupport(bool isSkipWeekend, List<BankHolidayProperty> bankHolidayList) {
            this.bankHolidayList = bankHolidayList;
            this.isSkipWeekend = isSkipWeekend;
        }

        public DateTime getNextWorkingDay(DateTime d, RddOutputBean rdd) {
            bool isUpdated = false;
            while (getIncreaseDayCondition(d, rdd)) {
                d = DateAndTime.DateAdd(DateInterval.Day, 1, d);
                isUpdated = true;
            }

            if (!isUpdated) {
                d = DateAndTime.DateAdd(DateInterval.Day, 1, d);
            }

            return d;
        }

        private bool getIncreaseDayCondition(DateTime d, RddOutputBean rdd) {
            return bankHolidayList.Where(x => x.nationalDate == d && isBankHoliday(x, rdd)).Count() > 0 || (isSkipWeekend && isWeekend(d));
        }

        public bool isWeekend(DateTime d) {
            return (d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday);
        }

        public bool getBankHolidayCondition(DateTime d, RddOutputBean rdd) {
            return bankHolidayList.Where(x => x.nationalDate == d && isBankHoliday(x, rdd)).Count() > 0;
        }

        public bool isBankHoliday(BankHolidayProperty bh, RddOutputBean rdd) {
            var bhSalesOrg = bh.salesOrg.ToUpper();
            var rddSalesOrg = rdd.salesOrg.ToUpper();
            var bhCountry = bh.country.ToLower();
            var rddCountry = rdd.country.ToLower();
            var bhRegion = bh.region ?? "";
            var rddRegion = rdd.region ?? "";

            switch (bhSalesOrg) {
                case "NL01": { return (bhSalesOrg == "DE01"); }

                case "ES01": { return ((bhSalesOrg == rddSalesOrg && bhRegion == rddRegion) || string.IsNullOrEmpty(bh.region)); }

                default: { return (bhSalesOrg == rddSalesOrg && bhCountry == rddCountry); }
            }
        }

        public bool getPrereleaseCondition(string salesOrg) {
            var salesOrgLastPreReleaseTime = getLastPrereleaseTime(salesOrg);
            return (salesOrgLastPreReleaseTime != null && DateAndTime.Now.TimeOfDay >= salesOrgLastPreReleaseTime);
        }

        private static TimeSpan? getLastPrereleaseTime(string salesOrg) {
            switch (salesOrg) {
                case "ZA01": return new TimeSpan(15, 30, 0);

                case "TR01": return new TimeSpan(16, 00, 0);

                case "RU01": return new TimeSpan(14, 00, 0);

                default: return null;
            }
        }
    }
}