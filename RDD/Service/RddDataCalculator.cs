using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace RDD {
    public class RddDataCalculator {
        #region properties
        private RDDDataCalculatorSupport rddSupport;
        private List<BankHolidayProperty> bankHolidayList;
        private bool isSkipWeekend;
        private DateTime tempDate;
        private DateTime initialTempDate = DateTime.Parse("01/01/0001"); //initialised to have value in the beginning
        private string tempReason;
        private string tempDelBlock;
        private string tempRouteCode;
        #endregion

        public enum CountryException {
            // CZ and PL exception - change route codes if the orders have 100% case fill rate in the first release and change route codes to 1 for the first release
            CEHUB,

            // Romania has a contract with LSP to reduce lead time to a minimum of 1
            // Romania exception - reduce route code to a minimum of 1 to accommodate for short lead time instead of postponing date and/or blocking the order
            Romania,

            // GB exception - change route code to emergency on fridays for orders with delivery on saturday
            GB,

            // ES and PT flag orders with rdd that are more than 7 days in advance
            SpainAndPortugal,

            // KE and NG has 0 days lead time meaning release on the same day
            KEAndNG,

            // orders on weekends when its not allowed
            weekendOrders,

            // no exception
            noException
        }

        public RddDataCalculator(List<BankHolidayProperty> bankHolidayList, bool isSkipWeekend, DateTime tempDate) {
            this.bankHolidayList = bankHolidayList;
            this.isSkipWeekend = isSkipWeekend;
            this.tempDate = tempDate;
        }

        public List<CalculatedRddOutputBean> getCalculatedRDDList(List<RddOutputBean> rddList) {
            var calculatedRDDList = new List<CalculatedRddOutputBean>();

            foreach (RddOutputBean rdd in rddList) {
                calculatedRDDList.Add(calculateRDDTasks(rdd));
            }

            return calculatedRDDList;
        }

        public CalculatedRddOutputBean calculateRDDTasks(RddOutputBean rdd) {
            if (string.IsNullOrEmpty(rdd.route)) { return routeCodeMissingAction(rdd); }

            rddSupport = new RDDDataCalculatorSupport(isSkipWeekend, bankHolidayList);

            var countryException = CountryException.noException;
            initialTempDate = tempDate;

            if (rdd.salesOrg != "RO01") {
                // sets tempDate as the closest rdd by lead time
                findClosestRDDByLeadTime(rdd);

                countryException = getCountryException(rdd);

                // Adds bypass changes block for all orders that have short lead time
                if (tempDate > rdd.oldRdd && !rdd.isRddChangeAllowed && (countryException == CountryException.noException || countryException == CountryException.SpainAndPortugal)) {
                    shortLeadTimeAction();
                }

                // adds +1 day to the calculated date until it matches customer speciffic day of delivery
                if (getDeliveryDayCondition(rdd)) {
                    specifficDeliveryDayAction(rdd);
                    // recalculate if the exception is still relevant
                    countryException = getCountryException(rdd);
                }

                countryExceptionAction(rdd, countryException);
            } else {
                countryException = getCountryException(rdd);
                countryExceptionAction(rdd, countryException);
            }

            addBHListToTempReason(rdd);
            if (string.IsNullOrEmpty(tempReason)) { tempReason += $"Rdd changed to next available day.{Constants.vbCr}"; }

            var calculatedValues = getCalculatedRddOutputBean(rdd, tempDate, tempReason, tempDelBlock, tempRouteCode);
            clearTempValues();
            return calculatedValues;
        }

        private bool getDeliveryDayCondition(RddOutputBean rdd) {
            return !string.IsNullOrEmpty(rdd.deliveryDay) && !rdd.deliveryDay.ToLower().Contains(Microsoft.VisualBasic.Strings.Format(tempDate, "dddd").ToLower());
        }

        private CalculatedRddOutputBean getCalculatedRddOutputBean(RddOutputBean rdd, DateTime CalculatedDate, string calculatedReason, string CalculatedDelBLock, string CalculatedRouteCode) {
            return new CalculatedRddOutputBean(rdd) {
                newRecommendedRdd = CalculatedDate,
                reason = calculatedReason,
                delBlock = CalculatedDelBLock,
                newRecommendedRouteCode = CalculatedRouteCode
            };
        }

        private void addBHListToTempReason(RddOutputBean rdd) {
            var usedBHList = getUsedBHList(rdd);
            string bhStr = string.Join(" ", usedBHList);
            if (!string.IsNullOrEmpty(bhStr)) { tempReason += $"Present bank holidays: {bhStr}.{Constants.vbCr}"; }
        }

        private void clearTempValues() {
            tempDate = DateTime.Today;
            initialTempDate = DateTime.Today;
            tempDelBlock = "";
            tempReason = "";
            tempRouteCode = "";
        }

        private List<string> getUsedBHList(RddOutputBean rdd) {
            var usedBHList = new List<string>();

            foreach (var bh in bankHolidayList) {
                if (bh.nationalDate >= initialTempDate && bh.nationalDate <= tempDate && rddSupport.isBankHoliday(bh, rdd)) {
                    
                    if (string.IsNullOrEmpty(bh.region)) {
                        usedBHList.Add("Country: " + bh.country + " -");
                    } else {
                        usedBHList.Add("Region: " + bh.region + " -");
                    }

                    usedBHList.Add("Date: " + Strings.Format(bh.nationalDate, "dd.MM.yyyy") + "  ");
                }
            }

            return usedBHList;
        }

        private void specifficDeliveryDayAction(RddOutputBean rdd) {
            string deliveryDaysAccepted = Regex.Replace(rdd.deliveryDay, "[^a-zA-Z]", "");

            if (!string.IsNullOrEmpty(deliveryDaysAccepted)) {
                while (!deliveryDaysAccepted.ToLower().Contains(Microsoft.VisualBasic.Strings.Format(tempDate, "dddd").ToLower()) || bankHolidayList.Where(x => x.nationalDate == tempDate).Count() > 0) {
                    tempDate = DateAndTime.DateAdd(DateInterval.Day, 1, tempDate);
                }
            }

            // changes block to hold order if the order is not short lead time
            if (tempDate != rdd.oldRdd) {
                if (!((tempDelBlock ?? "") == IDAConsts.DelBlocks.leadTimeBlock) && !rdd.isRddChangeAllowed) {
                    tempDelBlock = IDAConsts.DelBlocks.holdOrderBlock;
                    tempReason += $"Holding order to contact the customer for changing RDD to recommended specific acceptance day - {Microsoft.VisualBasic.Strings.Format(tempDate, "dddd")}.{Constants.vbCr}";
                } else {
                    tempReason += $"Changing RDD to recommended specific acceptance day - {Microsoft.VisualBasic.Strings.Format(tempDate, "dddd")}.{Constants.vbCr}";
                }

            }
        }

        private void RomaniaExceptionAction(RddOutputBean rdd) {

            if (rdd.oldRdd.DayOfWeek.ToString() == "Saturday" || rdd.oldRdd.DayOfWeek.ToString() == "Sunday") {
                tempReason += $"Old RDD was on weekend.{Constants.vbCr}";
            }

            int leadTimeDiff = 0;
            int leadTimeToChange = 0;
            int maxLeadTime = 3;
            int minLeadTime = 1;

            if (!string.IsNullOrEmpty(rdd.deliveryDay)) {

                if (getRealLeadTimeDifference(rdd, ((TimeSpan)(rdd.oldRdd - initialTempDate)).Days) >= 3 && rdd.deliveryDay.ToLower().Contains(Microsoft.VisualBasic.Strings.Format(rdd.oldRdd, "dddd").ToLower())) {
                    leadTimeToChange = rdd.leadTime;
                    leadTimeDiff = 3;
                    rdd.leadTime = 3;
                    findClosestRDDByLeadTime(rdd);
                    rdd.leadTime = leadTimeToChange;
                } else {
                    specifficDeliveryDayAction(rdd);
                }

                if (rdd.isRouteCodeChangeAllowed) {
                    //Difference between calculated rdd and the current rdd in order to find out what will be the correct route code
                    if (rdd.isRddChangeAllowed) {
                        if (leadTimeDiff == 0) {
                            leadTimeDiff = getRealLeadTimeDifference(rdd, ((TimeSpan)(tempDate - initialTempDate)).Days);
                        }
                    } else {
                        if (rdd.oldRdd > initialTempDate) {
                            leadTimeDiff = ((TimeSpan)(rdd.oldRdd - initialTempDate)).Days;
                        } else {
                            leadTimeDiff = getRealLeadTimeDifference(rdd, ((TimeSpan)(tempDate - initialTempDate)).Days);
                        }
                    }

                    if (leadTimeDiff > 0) {

                        if (leadTimeDiff > maxLeadTime) { leadTimeDiff = maxLeadTime; }

                        // Replaces last digit of the route code string with the minimum value of a lead time i.e RO0003 to RO0001
                        if (!(rdd.route.Last() == leadTimeDiff.ToString().Last())) {
                            tempRouteCode = Strings.Replace(rdd.route, Conversions.ToString(rdd.route.Last()), leadTimeDiff.ToString());
                        }

                    } else {
                        // adds plus 1 day to the intial date to recalculate closest rdd
                        tempDate = initialTempDate.AddDays(1);
                        specifficDeliveryDayAction(rdd);

                        if (rdd.isRddChangeAllowed) {
                            leadTimeDiff = getRealLeadTimeDifference(rdd, ((TimeSpan)(tempDate - initialTempDate)).Days);
                        } else {

                            if (rdd.oldRdd > initialTempDate) {
                                leadTimeDiff = ((TimeSpan)(rdd.oldRdd - initialTempDate)).Days;
                            } else {
                                leadTimeDiff = getRealLeadTimeDifference(rdd, ((TimeSpan)(tempDate - initialTempDate)).Days);
                            }

                        }

                        if (rdd.route.Last() != leadTimeDiff.ToString().Last()) {
                            if (leadTimeDiff > maxLeadTime) { leadTimeDiff = maxLeadTime; }
                            if (leadTimeDiff < minLeadTime) { leadTimeDiff = minLeadTime; }
                            tempRouteCode = Strings.Replace(rdd.route, Conversions.ToString(rdd.route.Last()), leadTimeDiff.ToString());
                        }
                    }

                }

            } else {

                if (rdd.isRouteCodeChangeAllowed) {

                    if (getRealLeadTimeDifference(rdd, ((TimeSpan)(rdd.oldRdd - initialTempDate)).Days) >= 3) {
                        leadTimeDiff = 3;
                        leadTimeToChange = rdd.leadTime;
                        rdd.leadTime = 3;
                        findClosestRDDByLeadTime(rdd);
                        rdd.leadTime = leadTimeToChange;
                    } else {
                        rdd.leadTime = 1;
                        findClosestRDDByLeadTime(rdd);
                    }

                    if (rdd.isRddChangeAllowed) {

                        if (rdd.oldRdd != tempDate) {
                            leadTimeDiff = getRealLeadTimeDifference(rdd, ((TimeSpan)(tempDate - initialTempDate)).Days);
                        } else {
                            leadTimeDiff = rdd.leadTime;
                        }

                    } else {

                        if (rdd.oldRdd > initialTempDate && leadTimeToChange != 0) {
                            leadTimeDiff = ((TimeSpan)(rdd.oldRdd - initialTempDate)).Days;
                        } else {
                            leadTimeDiff = getRealLeadTimeDifference(rdd, ((TimeSpan)(tempDate - initialTempDate)).Days);
                        }

                    }

                    if (leadTimeDiff > 0) {
                        if (leadTimeDiff > maxLeadTime) { leadTimeDiff = maxLeadTime; }

                        // Replaces last digit of the route code string with the minimum value of a lead time i.e RO0003 to RO0001
                        if (rdd.route.Last() != leadTimeDiff.ToString().Last()) {
                            tempRouteCode = Strings.Replace(rdd.route, Conversions.ToString(rdd.route.Last()), leadTimeDiff.ToString());
                        }
                        if (leadTimeToChange != 0) {
                            tempRouteCode = Strings.Replace(rdd.route, Conversions.ToString(rdd.route.Last()), maxLeadTime.ToString());
                        }
                    }
                }

            }

            if (rdd.oldRdd <= initialTempDate && !rdd.isRddChangeAllowed && rdd.isRouteCodeChangeAllowed) {
                tempRouteCode = null;
                tempDelBlock = IDAConsts.DelBlocks.leadTimeBlock;
                tempReason += $"Customer does not allow changes for RDD and Routes cannot be shortened.{Constants.vbCr}";
            }

            if (!rdd.isRouteCodeChangeAllowed && !rdd.isRddChangeAllowed) {
                tempRouteCode = null;
                tempDelBlock = IDAConsts.DelBlocks.leadTimeBlock;
                tempReason += $"Customer does not allow changes for RDD and Route.{Constants.vbCr}";
            }

            if (tempDate == initialTempDate) {
                findClosestRDDByLeadTime(rdd);
            }

            if (tempRouteCode != null && !rdd.isRddChangeAllowed) {
                tempDate = rdd.oldRdd;
            }

            if (!string.IsNullOrEmpty(rdd.deliveryDay)) {
                if (!rdd.deliveryDay.ToLower().Contains(Microsoft.VisualBasic.Strings.Format(tempDate, "dddd").ToLower())) {
                    tempDate = rdd.oldRdd;
                    tempRouteCode = null;

                    if (!rdd.deliveryDay.ToLower().Contains(Microsoft.VisualBasic.Strings.Format(rdd.oldRdd, "dddd").ToLower())) {
                        tempDelBlock = IDAConsts.DelBlocks.holdOrderBlock;
                    }

                    tempReason += $"Could not change rdd to speciffic day. Customer may have made an exception. Please check with customer.{Constants.vbCr}";
                }
            }

        }

        private int getRealLeadTimeDifference(RddOutputBean rdd, int leadTimeDiff) {
            DateTime i;

            if (tempDate != rdd.oldRdd) {
                i = tempDate;

                while (i <= rdd.oldRdd) {
                    if (bankHolidayList.Contains(new BankHolidayProperty { country = rdd.country, salesOrg = rdd.salesOrg, nationalDate = i }) || DateAndTime.Weekday(i, FirstDayOfWeek.Monday) > 5) {
                        leadTimeDiff--;
                    }
                    i = i.AddDays(1);
                }

            } else {
                i = initialTempDate;
                leadTimeDiff = 0;

                while (i < rdd.oldRdd) {
                    if (!(bankHolidayList.Contains(new BankHolidayProperty { country = rdd.country, salesOrg = rdd.salesOrg, nationalDate = i })) &&
                        DateAndTime.Weekday(i, FirstDayOfWeek.Monday) < 6) {
                        leadTimeDiff++;
                    }
                    i = i.AddDays(1);
                }
            }

            return leadTimeDiff;
        }

        private CountryException getCountryException(RddOutputBean rdd) {
            switch (true) {
                case object _ when ((rdd.salesOrg.ToUpper() ?? "") == "CZ01" || (rdd.salesOrg.ToUpper() ?? "") == "PL01") && rdd.isRouteCodeChangeAllowed: {
                        return CountryException.CEHUB;
                    }

                case object _ when (rdd.salesOrg.ToUpper() ?? "") == "GB01" && rdd.isRouteCodeChangeAllowed && DateTime.Today.DayOfWeek == DayOfWeek.Friday && rdd.oldRdd < tempDate: {
                        return CountryException.GB;
                    }

                case object _ when (rdd.salesOrg.ToUpper() ?? "") == "RO01": {
                        return CountryException.Romania;
                    }

                case object _ when (rdd.salesOrg.ToUpper() ?? "") == "ES01" || (rdd.salesOrg.ToUpper() ?? "") == "PT01": {
                        return CountryException.SpainAndPortugal;
                    }

                case object _ when (rdd.salesOrg.ToUpper() ?? "") == "KE02" && (rdd.salesOrg.ToUpper() ?? "") == "NG01" && rdd.leadTime == 0 && rdd.oldRdd != DateTime.Today: {
                        return CountryException.KEAndNG;
                    }

                default: {
                        return CountryException.noException;
                    }
            }
        }

        private void countryExceptionAction(RddOutputBean rdd, CountryException countryException) {
            switch (countryException) {
                case CountryException.CEHUB: {
                        CEHUBExceptionAction(rdd);
                        break;
                    }

                case CountryException.GB: {
                        GBExceptionAction();
                        break;
                    }

                case CountryException.Romania: {
                        RomaniaExceptionAction(rdd);
                        break;
                    }

                case CountryException.KEAndNG: {
                        tempReason += $"Zero days lead time. Moved rdd to todays date.{Constants.vbCr}";
                        tempDate = DateTime.Today;
                        break;
                    }

                case CountryException.SpainAndPortugal: {
                        esAndPTAction(rdd);
                        esBankHolidayAction(rdd);
                        break;
                    }

                // nothing to be done
                case CountryException.noException: {
                        break;
                    }

                default: {
                        throw new NotImplementedException();
                    }
            }
        }

        private void esAndPTAction(RddOutputBean rdd) {
            if (DateAndTime.DateDiff("d", tempDate, rdd.oldRdd) > 7) { tempReason += $"WARNING: RDD more than 7 days in advance.{Constants.vbCr}"; }

            if (DateAndTime.Weekday(rdd.oldRdd, FirstDayOfWeek.Monday) > 5 && !rdd.isRddChangeAllowed) {
                tempReason += $"WARNING: RDD on weekends are not allowed.{Constants.vbCr}";
                tempDelBlock = IDAConsts.DelBlocks.leadTimeBlock;
                tempDate = rdd.oldRdd;
            }
        }

        private void esBankHolidayAction(RddOutputBean rdd) {
            if (rdd.salesOrg.ToUpper() == "ES01") {
                var bhList = getUsedBHList(rdd);
                var doesContainBHRegion = bhList.Where(x => x.Contains("Region")).ToList().Count > 0;
                var doesContainBHNational = bhList.Where(x => x.Contains("Country")).ToList().Count > 0;
                var leadTimeDiff = (rdd.oldRdd - tempDate).Days;
                var realLeadTimeDiff = (leadTimeDiff != 0) ? getRealLeadTimeDifference(rdd, leadTimeDiff) : 0;

                if (doesContainBHRegion && tempDate == rdd.oldRdd && realLeadTimeDiff == 0) {
                    tempReason += "Due to BH order needs to be released manually early. ";
                    tempDelBlock = IDAConsts.DelBlocks.holdOrderBlock;
                }

                if ((doesContainBHRegion || doesContainBHNational) && tempDate != rdd.oldRdd && !rdd.isRddChangeAllowed) {
                    tempReason += "Due to BH order needs to be agreed with customer. ";
                    tempDelBlock = IDAConsts.DelBlocks.holdOrderBlock;
                }

                if (bankHolidayList.Where(x => x.nationalDate == rdd.oldRdd && (x.region == rdd.region || string.IsNullOrEmpty(x.region))).ToList().Count > 0 && !rdd.isRddChangeAllowed) {
                    tempReason += "RDD falls on BH. ";
                    tempDelBlock = IDAConsts.DelBlocks.holdOrderBlock;
                }
            }
        }

        private void shortLeadTimeAction() {
            tempDelBlock = IDAConsts.DelBlocks.leadTimeBlock;
            tempReason += $"Order blocked due to short lead time.{Constants.vbCr}";
        }

        private void GBExceptionAction() {
            tempRouteCode = "GB0000";
            tempReason += $"Route code changed to emergency for a friday order to be released, New Rote code: {tempReason}.{Constants.vbCr}";
        }

        private void CEHUBExceptionAction(RddOutputBean rdd) {

            int leadTimeDiff = ((TimeSpan)(rdd.oldRdd - tempDate)).Days;   //difference between calculated rdd and the current rdd
            int leadTime;
            int maxLeadTime = 3;

            if (rdd.oldRdd > tempDate && rdd.leadTime < maxLeadTime && rdd.caseFillRate == 100.0) {

                leadTimeDiff = getRealLeadTimeDifference(rdd, leadTimeDiff);

                if ((leadTimeDiff + rdd.leadTime) > maxLeadTime) {
                    leadTime = maxLeadTime;
                } else {
                    leadTime = leadTimeDiff + rdd.leadTime;
                }

                tempRouteCode = rdd.route.Replace(rdd.leadTime.ToString(), leadTime.ToString());
                tempReason += $"Order has 100% CFR. Route code increased to {leadTime} days to release order sooner to give more time to LSP.{Constants.vbCr}";
                tempDate = rdd.oldRdd;
            }

            // If before first release change route codes to 1, if after first release then block orders
            var currentHour = DateTime.Now.Hour;
            if ((rdd.salesOrg ?? "") == "CZ01") {
                if (currentHour < 9 && rdd.leadTime > 1 && rdd.isRouteCodeChangeAllowed && rdd.isOneDayLeadTimeAllowed && string.IsNullOrEmpty(tempRouteCode) && tempDate > rdd.oldRdd) {
                    tempRouteCode = Strings.Replace(rdd.route, rdd.leadTime.ToString(), 1.ToString());
                    tempReason += $"Route code decreased for first release.{Constants.vbCr}";
                }

                if (rdd.leadTime == 1 && currentHour > 9 && string.IsNullOrEmpty(tempRouteCode)) {
                    tempDelBlock = IDAConsts.DelBlocks.leadTimeBlock;
                    if (currentHour > 9) tempReason += $"Not allowed to have route code for 1 day after first release.{Constants.vbCr}";
                    if (rdd.isRouteCodeChangeAllowed) tempReason += $"Not allowed to have route code change for this customer.{Constants.vbCr}";
                    if (rdd.isOneDayLeadTimeAllowed) tempReason += $"Not allowed to have route code of 1 day for this customer.{Constants.vbCr}";
                }
            }
        }

        private CalculatedRddOutputBean routeCodeMissingAction(RddOutputBean rdd) {
            var reason = $"Warning!!! Route codes are missing!!!{Constants.vbCr}";
            var calcDelBlock = IDAConsts.DelBlocks.leadTimeBlock;

            return new CalculatedRddOutputBean(rdd) {
                newRecommendedRdd = rdd.oldRdd,
                reason = reason,
                delBlock = calcDelBlock,
                newRecommendedRouteCode = ""
            };
        }

        private void findClosestRDDByLeadTime(RddOutputBean rdd) {

            int wdCount = rdd.leadTime;

            if (rddSupport.getBankHolidayCondition(tempDate, rdd)) {
                wdCount++;
            }

            if (rddSupport.getPrereleaseCondition(rdd.salesOrg)) {
                wdCount += 1;
                tempReason += $"RDD moved by 1 day for tomorrows release.{Constants.vbCr}";
            }

            while (wdCount > 0) {
                tempDate = rddSupport.getNextWorkingDay(tempDate, rdd);
                wdCount--;
                if (rddSupport.getBankHolidayCondition(tempDate, rdd) || (isSkipWeekend && rddSupport.isWeekend(tempDate))) { wdCount++; }
            }

        }
    }
}