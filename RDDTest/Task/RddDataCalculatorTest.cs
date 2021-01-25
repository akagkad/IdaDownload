using IDAUtil;
using IDAUtil.Model.Properties.ServerProperty;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDD;
using System;
using System.Collections.Generic;

namespace IDAUnitTest.Task {
    /// <summary>
    /// RDD task tests that check for correctness of calculation of Requested delivery days, Route codes, Delivery blocks and any speciffic sales org exceptions
    /// </summary>

    [TestClass()]
    public class RddDataCalculatorTestRO01 {

        #region TestMethods
        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfUntouchedRDD_When_OrdersShouldBeUntouchedBecauseRDDIsInTheFuture() {

            var rddListOrderIsUntouched = getRddListWhereOrderIsUntouchedDueToRDDsBeingInTheFuture();
            var expectedListOrderIsUntouched = getExpectedListWhereOrderIsUntouchedDueToRDDsBeingInTheFuture();

            int i = 0;
            var loopTo = rddListOrderIsUntouched.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region NotForChangeTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("30/07/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("30/07/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("30/07/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfNewRDDOnly_When_CustomerAllowsRDDChangeOnlyAndRDDIsShortForLeadTime() {

            var rddListOrderChangesRDDOnly = getRddListWhereOrderChangesRDDOnly();
            var expectedListOrderChangesRDDOnly = getExpectedListWhereOrderChangesRDDOnly();

            int i = 0;
            var loopTo = rddListOrderChangesRDDOnly.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                    #region ChangeRddOnlyTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListOrderChangesRDDOnly[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderChangesRDDOnly[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListOrderChangesRDDOnly[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderChangesRDDOnly[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListOrderChangesRDDOnly[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderChangesRDDOnly[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfShortenedRoutesOnly_When_CustomerOnlyAllowsRouteCodeShorteningAndRDDIsShortForLeadTimeAndShorteningRouteCodeIsPossible() {

            var rddListShortenedRouteCode = getRddListWhereRouteCodeNeedsToBeShortened();
            var expectedListShortenedRouteCode = getExpectedListWhereRouteCodeNeedsToBeShortened();

            int i = 0;
            var loopTo = rddListShortenedRouteCode.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShortenRouteCodeTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListShortenedRouteCode[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListShortenedRouteCode[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListShortenedRouteCode[i].delBlock, rddcalc.calculateRDDTasks(rddListShortenedRouteCode[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListShortenedRouteCode[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListShortenedRouteCode[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfBlocks_When_CustomerOnlyAllowsRouteCodeShorteningAndRDDIsShortForLeadTimeAndShorteningRouteCodeIsNotPossible() {

            var rddListBlocked = getRddListWhereOrderWillGetBlockedButShorteningRouteIsAllowed();
            var expectedListBlocked = getExpectedListWhereOrderWillGetBlockedButShorteningRouteIsAllowed();

            int i = 0;
            var loopTo = rddListBlocked.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].delBlock, rddcalc.calculateRDDTasks(rddListBlocked[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfShortenedRoutesAndNewRDD_When_CustomerAllowsRouteCodeShorteningAndRDDChangesAndRDDIsShortForLeadTimeWithSpecifficDeliveryDay() {

            var rddListShortenedRouteCodeAndChangeRDD = getRddListWhereRouteCodeNeedsToBeShortenedAndRDDToBeChangedWithSpecifficDeliveryDay();
            var expectedListShortenedRouteCodeAndChangeRDD = getExpectedListWhereRouteCodeNeedsToBeShortenedAndRDDToBeChangedWithSpecifficDeliveryDay();

            int i = 0;
            var loopTo = rddListShortenedRouteCodeAndChangeRDD.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListShortenedRouteCodeAndChangeRDD[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListShortenedRouteCodeAndChangeRDD[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListShortenedRouteCodeAndChangeRDD[i].delBlock, rddcalc.calculateRDDTasks(rddListShortenedRouteCodeAndChangeRDD[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListShortenedRouteCodeAndChangeRDD[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListShortenedRouteCodeAndChangeRDD[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfBlocks_When_CustomerDoesntAllowAnyChangesToTheOrder() {

            var rddListBlocked = getRddListWhereOrderWillGetBlocked();
            var expectedListBlocked = getExpectedListWhereOrderWillGetBlocked();

            int i = 0;
            var loopTo = rddListBlocked.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].delBlock, rddcalc.calculateRDDTasks(rddListBlocked[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfDecreasedRDD_When_CustomerAllowsRDDChangesAndCurrentRDDIsOnWeekend() {

            var rddListBlocked = getRddListWhereOrderIsPlacedOnWeekend();
            var expectedListBlocked = getExpectedListWhereOrderIsPlacedOnWeekend();

            int i = 0;
            var loopTo = rddListBlocked.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].delBlock, rddcalc.calculateRDDTasks(rddListBlocked[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfReducedRDDAndRoute_When_CustomerAllowsRDDChangesAndCurrentRDDIsOnWeekendWithSpecifficDayAndInTheFutureAndRDDAndRouteCodesAllowedToChange() {

            var rddListBlocked = getRddListWhereOrderIsPlacedOnWeekendWithSpecifficDayAndInTheFutureAndRDDAndRouteCodesAllowedToChange();
            var expectedListBlocked = getExpectedListWhereOrderIsPlacedOnWeekendWithSpecifficDayAndInTheFutureAndRDDAndRouteCodesAllowedToChange();

            int i = 0;
            var loopTo = rddListBlocked.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].delBlock, rddcalc.calculateRDDTasks(rddListBlocked[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("01/07/2020"));
                Assert.AreEqual(expectedListBlocked[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListBlocked[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOFMovedRDDAndIncreaedRoutes_When_CustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndIsInTheFutureAndRouteCodeChangeIsAllowed() {

            var rddList = getCalculatedRddListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndIsInTheFutureAndRouteCodeChangeIsAllowed();
            var expectedRddList = getExpectedListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndIsInTheFutureAndRouteCodeChangeIsAllowed();

            int i = 0;
            var loopTo = rddList.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("27/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("27/08/2020"));
                Assert.AreEqual(expectedRddList[i].delBlock, rddcalc.calculateRDDTasks(rddList[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("27/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfIncreaedRDDandRoutes_When_CustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndFallsOnTodaysDateAndRouteCodeChangeIsAllowed() {

            var rddList = getCalculatedRddListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndFallsOnTodaysDateAndRouteCodeChangeIsAllowed();
            var expectedRddList = getExpectedListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndFallsOnTodaysDateAndRouteCodeChangeIsAllowed();

            int i = 0;
            var loopTo = rddList.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("05/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("05/08/2020"));
                Assert.AreEqual(expectedRddList[i].delBlock, rddcalc.calculateRDDTasks(rddList[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("05/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfIncreasedRoute_When_CustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed() {

            var rddList = getCalculatedRddListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed();
            var expectedRddList = getExpectedListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed();
            int i = 0;
            var loopTo = rddList.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed(), isSkipWeekend: true, tempDate: DateTime.Parse("13/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed(), isSkipWeekend: true, tempDate: DateTime.Parse("13/08/2020"));
                Assert.AreEqual(expectedRddList[i].delBlock, rddcalc.calculateRDDTasks(rddList[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed(), isSkipWeekend: true, tempDate: DateTime.Parse("13/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfDecreasedRoute_When_OnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNoSpecifficDay() {

            var rddList = getRddListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNoSpecifficDay();
            var expectedRddList = getExpectedListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNoSpecifficDay();
            int i = 0;
            var loopTo = rddList.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("14/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("14/08/2020"));
                Assert.AreEqual(expectedRddList[i].delBlock, rddcalc.calculateRDDTasks(rddList[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("14/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfIncreasedRoute_When_OnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndSpecifficDayAndRDDInTheFuture() {

            var rddList = getRddListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndSpecifficDayAndRDDInTheFuture();
            var expectedRddList = getExpectedListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndSpecifficDayAndRDDInTheFuture();
            int i = 0;
            var loopTo = rddList.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("17/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("17/08/2020"));
                Assert.AreEqual(expectedRddList[i].delBlock, rddcalc.calculateRDDTasks(rddList[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("17/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfBlocks_When_OnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNotSpecifficDayAndRDDInTheFuture() {

            var rddList = getRddListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNotSpecifficDayAndRDDInTheFuture();
            var expectedRddList = getExpectedListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNotSpecifficDayAndRDDInTheFuture();
            int i = 0;
            var loopTo = rddList.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region ShrotenRouteCodeAndChangeRDDTest
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("17/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRdd);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("17/08/2020"));
                Assert.AreEqual(expectedRddList[i].delBlock, rddcalc.calculateRDDTasks(rddList[i]).delBlock);
                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayList(), isSkipWeekend: true, tempDate: DateTime.Parse("17/08/2020"));
                Assert.AreEqual(expectedRddList[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddList[i]).newRecommendedRouteCode);
                #endregion

            }
        }
        # endregion

        #region Banh Holiday functions

        private List<BankHolidayProperty> getBankHolidayList() {
            var list = new List<BankHolidayProperty>() {
                //new BankHolidayProperty() {
                //    salesOrg = "RO01",
                //    country = "Romania",
                //    nationalDate = DateTime.Parse("01/01/2020")
                //}
            };
            return list;
        }

        private List<BankHolidayProperty> getBankHolidayListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed() {
            var list = new List<BankHolidayProperty>() {
                new BankHolidayProperty() {
                    salesOrg = "RO01",
                    country = "Romania",
                    nationalDate = DateTime.Parse("14/08/2020")
                }
            };
            return list;
        }

        #endregion

        #region UntouchedListDueToRDDsBeingInTheFutureObj
        private List<RddOutputBean> getRddListWhereOrderIsUntouchedDueToRDDsBeingInTheFuture() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    deliveryDay = "MONDAY / THURSDAY",
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("03/08/2020")
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOrderIsUntouchedDueToRDDsBeingInTheFuture() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    deliveryDay = "MONDAY / THURSDAY",
                    oldRdd = DateTime.Parse("03/08/2020"),
                    newRecommendedRdd = DateTime.Parse("03/08/2020"),
                    newRecommendedRouteCode = null
                }
            };
            return list;
        }
        #endregion

        #region ChangeRddOnlyListObj
        private List<RddOutputBean> getRddListWhereOrderChangesRDDOnly() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = false,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("01/07/2020")
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOrderChangesRDDOnly() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = false,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("01/07/2020"),
                    newRecommendedRdd = DateTime.Parse("06/07/2020"),
                    newRecommendedRouteCode = null
                }
            };
            return list;
        }
        #endregion

        #region ShortenRouteListObj
        private List<RddOutputBean> getRddListWhereRouteCodeNeedsToBeShortened() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = false,
                    oldRdd = DateTime.Parse("02/07/2020")
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereRouteCodeNeedsToBeShortened() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = false,
                    delBlock = null,
                    oldRdd = DateTime.Parse("02/07/2020"),
                    newRecommendedRdd = DateTime.Parse("02/07/2020"),
                    newRecommendedRouteCode = "RO0001"
                }
            };
            return list;
        }
        #endregion

        #region ShortenRouteAndChangeRDDListObj
        private List<RddOutputBean> getRddListWhereRouteCodeNeedsToBeShortenedAndRDDToBeChangedWithSpecifficDeliveryDay() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("25/06/2020"),
                    deliveryDay = "Friday"
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereRouteCodeNeedsToBeShortenedAndRDDToBeChangedWithSpecifficDeliveryDay() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("25/06/2020"),
                    deliveryDay = "Friday",
                    newRecommendedRdd = DateTime.Parse("03/07/2020"),
                    newRecommendedRouteCode = "RO0002"
                }
            };
            return list;
        }
        #endregion

        #region BlockOrderListShortRouteAllowedObj
        private List<RddOutputBean> getRddListWhereOrderWillGetBlockedButShorteningRouteIsAllowed() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = false,
                    oldRdd = DateTime.Parse("25/06/2020"),
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOrderWillGetBlockedButShorteningRouteIsAllowed() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = false,
                    delBlock = "Z9",
                    oldRdd = DateTime.Parse("25/06/2020"),
                    newRecommendedRdd = DateTime.Parse("02/07/2020"),
                    newRecommendedRouteCode = null
                }
            };
            return list;
        }
        #endregion

        #region BlockOrderListNoChangesAllowedObj
        private List<RddOutputBean> getRddListWhereOrderWillGetBlocked() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = false,
                    soldToName = "aaa",
                    isRddChangeAllowed = false,
                    oldRdd = DateTime.Parse("03/07/2020"),
                    deliveryDay = "Thursday"
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOrderWillGetBlocked() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 3,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0003",
                    country = "Romania",
                    isRouteCodeChangeAllowed = false,
                    soldToName = "aaa",
                    isRddChangeAllowed = false,
                    delBlock = "Z9",
                    oldRdd = DateTime.Parse("03/07/2020"),
                    newRecommendedRdd = DateTime.Parse("02/07/2020"),
                    newRecommendedRouteCode = null,
                    deliveryDay = "Thursday"
                }
            };
            return list;
        }
        #endregion

        #region ChangeRDDListWeekendRDDObj
        private List<RddOutputBean> getRddListWhereOrderIsPlacedOnWeekend() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = false,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("04/07/2020"),
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOrderIsPlacedOnWeekend() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = false,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("04/07/2020"),
                    newRecommendedRdd = DateTime.Parse("03/07/2020"),
                    newRecommendedRouteCode = null,
                }
            };
            return list;
        }
        #endregion

        #region ChangeRDDListWeekendRDDWithSpecifficDayAndInTheFutureObj
        private List<RddOutputBean> getRddListWhereOrderIsPlacedOnWeekendWithSpecifficDayAndInTheFutureAndRDDAndRouteCodesAllowedToChange() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("19/07/2020"),
                    deliveryDay = "Thursday"
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOrderIsPlacedOnWeekendWithSpecifficDayAndInTheFutureAndRDDAndRouteCodesAllowedToChange() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("19/07/2020"),
                    deliveryDay = "Thursday",
                    newRecommendedRdd = DateTime.Parse("02/07/2020"),
                    newRecommendedRouteCode = "RO0001",
                }
            };
            return list;
        }
        #endregion

        #region ChangeRDDListRDDWithSpecifficDayAndInTheFutureObj
        private List<RddOutputBean> getCalculatedRddListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndIsInTheFutureAndRouteCodeChangeIsAllowed() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("31/08/2020"),
                    deliveryDay = "THURSDAY"
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndIsInTheFutureAndRouteCodeChangeIsAllowed() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("31/08/2020"),
                    deliveryDay = "THURSDAY",
                    newRecommendedRdd = DateTime.Parse("03/09/2020"),
                    newRecommendedRouteCode = "RO0003",
                }
            };
            return list;
        }
        #endregion

        #region ChangeRDDListRDDWithSpecifficDayAndFallsOnTodaysDateObj
        private List<RddOutputBean> getCalculatedRddListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndFallsOnTodaysDateAndRouteCodeChangeIsAllowed() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("07/08/2020"),
                    deliveryDay = "WEDNESDAY"
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereCustomerAllowsRDDChangesAndCurrentRDDIsNotOnSpecifficDayAndFallsOnTodaysDateAndRouteCodeChangeIsAllowed() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("07/08/2020"),
                    deliveryDay = "WEDNESDAY",
                    newRecommendedRdd = DateTime.Parse("12/08/2020"),
                    newRecommendedRouteCode = "RO0003",
                }
            };
            return list;
        }
        #endregion

        #region ChangeRDDListRDDWithSpecifficDayAndHasBankHolidayAndRouteCodeAndRDDAllowedToChangeObj
        private List<RddOutputBean> getCalculatedRddListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("17/08/2020"),
                    deliveryDay = "MONDAY"
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereCustomerAllowsRDDChangesAndCurrentRDDIsOnSpecifficDayAndHasBankHolidayAndRouteCodeChangeIsAllowed() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("17/08/2020"),
                    deliveryDay = "MONDAY",
                    newRecommendedRdd = DateTime.Parse("17/08/2020"),
                    newRecommendedRouteCode = "RO0001",
                }
            };
            return list;
        }
        #endregion

        #region ShortenRouteListAndRddChangeIsAllowedObj
        private List<RddOutputBean> getRddListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNoSpecifficDay() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("17/08/2020")
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNoSpecifficDay() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("17/08/2020"),
                    newRecommendedRdd = DateTime.Parse("17/08/2020"),
                    newRecommendedRouteCode = "RO0001"
                }
            };
            return list;
        }
        #endregion

        #region IncreaseRouteCodeListAndRddChangeIsAllowedAndSpecifficDayAndFallsOnSpecifficDayObj
        private List<RddOutputBean> getRddListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndSpecifficDayAndRDDInTheFuture() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    deliveryDay = "TUESDAY / THURSDAY / FRIDAY",
                    oldRdd = DateTime.Parse("20/08/2020")
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndSpecifficDayAndRDDInTheFuture() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("20/08/2020"),
                    deliveryDay = "TUESDAY / THURSDAY / FRIDAY",
                    newRecommendedRdd = DateTime.Parse("20/08/2020"),
                    newRecommendedRouteCode = "RO0003"
                }
            };
            return list;
        }
        #endregion

        #region IncreaseRouteCodeListAndRddChangeIsAllowedAndSpecifficDayAndFallsOnSpecifficDayObj
        private List<RddOutputBean> getRddListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNotSpecifficDayAndRDDInTheFuture() {
            var list = new List<RddOutputBean>() {
                new RddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    oldRdd = DateTime.Parse("20/08/2020")
                }
            };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhereOnlyRouteCodeNeedsToBeShortenedAndRDDChangesAllowedAndNotSpecifficDayAndRDDInTheFuture() {
            var list = new List<CalculatedRddOutputBean>() {
                new CalculatedRddOutputBean() {
                    leadTime = 2,
                    soldTo = 123,
                    salesOrg = "RO01",
                    route = "RO0002",
                    country = "Romania",
                    isRouteCodeChangeAllowed = true,
                    soldToName = "aaa",
                    isRddChangeAllowed = true,
                    delBlock = null,
                    oldRdd = DateTime.Parse("20/08/2020"),
                    newRecommendedRdd = DateTime.Parse("20/08/2020"),
                    newRecommendedRouteCode = "RO0003"
                }
            };
            return list;
        }
        #endregion
    }

    [TestClass()]
    public class RddDataCalculatorTestES01 {

        #region ThereIsABHAndRecomendedRDDIsTheSameAsOldRDDAndLeadTimeIsCorrect
        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfBlockedOrdersWithReasonOfRDDFellOnBH_When_ThereIsABHAndRecomendedRDDIsTheSameAsOldRDDAndLeadTimeIsCorrect() {

            var rddListOrderIsUntouched = getRddListWhenThereIsABHAndRecomendedRDDIsTheSameAsOldRDDAndLeadTimeIsCorrect();
            var expectedListOrderIsUntouched = getExpectedListWhenThereIsABHAndRecomendedRDDIsTheSameAsOldRDDAndLeadTimeIsCorrect();

            int i = 0;
            var loopTo = rddListOrderIsUntouched.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region test
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListNationalCorrectRDD(), isSkipWeekend: true, tempDate: DateTime.Parse("30/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListNationalCorrectRDD(), isSkipWeekend: true, tempDate: DateTime.Parse("30/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListNationalCorrectRDD(), isSkipWeekend: true, tempDate: DateTime.Parse("30/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        private List<RddOutputBean> getRddListWhenThereIsABHAndRecomendedRDDIsTheSameAsOldRDDAndLeadTimeIsCorrect() {
            var list = new List<RddOutputBean>() {
                    new RddOutputBean() {
                        leadTime = 3,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0002",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = false,
                        oldRdd = DateTime.Parse("05/11/2020"),
                        region = "30"
                    }
                };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhenThereIsABHAndRecomendedRDDIsTheSameAsOldRDDAndLeadTimeIsCorrect() {
            var list = new List<CalculatedRddOutputBean>() {
                    new CalculatedRddOutputBean() {
                        leadTime = 3,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0002",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = false,
                        delBlock = null,
                        oldRdd = DateTime.Parse("05/11/2020"),
                        newRecommendedRdd = DateTime.Parse("05/11/2020"),
                        newRecommendedRouteCode = null,
                        region = "30"
                    }
                };
            return list;
        }
        #endregion

        #region ThereIsARegionalBHOnRDDAndCustomerDoesNotAllowRddChanges
        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfBlockedOrdersWithReasonOfRDDFellOnBH_When_ThereIsARegionalBHOnRDDAndCustomerDoesNotAllowRddChanges() {

            var rddListOrderIsUntouched = getRddListWhenThereIsARegionalBHOnRDDAndCustomerDoesNotAllowRddChanges();
            var expectedListOrderIsUntouched = getExpectedListWhenThereIsARegionalBHOnRDDAndCustomerDoesNotAllowRddChanges();

            int i = 0;
            var loopTo = rddListOrderIsUntouched.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region test
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        private List<RddOutputBean> getRddListWhenThereIsARegionalBHOnRDDAndCustomerDoesNotAllowRddChanges() {
            var list = new List<RddOutputBean>() {
                    new RddOutputBean() {
                        leadTime = 2,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0002",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = false,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        region = "3"
                    }
                };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhenThereIsARegionalBHOnRDDAndCustomerDoesNotAllowRddChanges() {
            var list = new List<CalculatedRddOutputBean>() {
                    new CalculatedRddOutputBean() {
                        leadTime = 2,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0002",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = false,
                        delBlock = IDAConsts.DelBlocks.holdOrderBlock,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        newRecommendedRdd = DateTime.Parse("02/11/2020"),
                        newRecommendedRouteCode = null,
                        region = "3"
                    }
                };
            return list;
        }
        #endregion

        #region ThereIsARegionalBHOnRDDAndCustomerDoesAllowRddChanges
        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfMovedRDD_When_ThereIsARegionalBHOnRDDAndCustomerDoesAllowRddChanges() {

            var rddListOrderIsUntouched = getRddListWhenThereIsARegionalBHOnRDDAndCustomerDoesAllowRddChanges();
            var expectedListOrderIsUntouched = getExpectedListWhenThereIsARegionalBHOnRDDAndCustomerDoesAllowRddChanges();

            int i = 0;
            var loopTo = rddListOrderIsUntouched.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region test
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        private List<RddOutputBean> getRddListWhenThereIsARegionalBHOnRDDAndCustomerDoesAllowRddChanges() {
            var list = new List<RddOutputBean>() {
                    new RddOutputBean() {
                        leadTime = 2,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0002",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = true,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        region = "3"
                    }
                };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhenThereIsARegionalBHOnRDDAndCustomerDoesAllowRddChanges() {
            var list = new List<CalculatedRddOutputBean>() {
                    new CalculatedRddOutputBean() {
                        leadTime = 2,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0002",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = true,
                        delBlock = null,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        newRecommendedRdd = DateTime.Parse("02/11/2020"),
                        newRecommendedRouteCode = null,
                        region = "3"
                    }
                };
            return list;
        }
        #endregion

        #region ThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsMoreThanLeadTime
        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfBlockedOrdersWithReasonOfNeedingToReleaseOrderManually_When_ThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsMoreThanLeadTime() {

            var rddListOrderIsUntouched = getRddListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsMoreThanLeadTime();
            var expectedListOrderIsUntouched = getExpectedListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsMoreThanLeadTime();

            int i = 0;
            var loopTo = rddListOrderIsUntouched.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region test
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("12/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("12/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("12/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        private List<RddOutputBean> getRddListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsMoreThanLeadTime() {
            var list = new List<RddOutputBean>() {
                    new RddOutputBean() {
                        leadTime = 2,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "es0",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = true,
                        oldRdd = DateTime.Parse("15/10/2020"),
                        region = "1"
                    }
                };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsMoreThanLeadTime() {
            var list = new List<CalculatedRddOutputBean>() {
                    new CalculatedRddOutputBean() {
                        leadTime = 2,
                        soldTo = 123,
                        salesOrg = "es01",
                        country = "spain",
                        route = "es0",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = true,
                        delBlock = IDAConsts.DelBlocks.holdOrderBlock,
                        oldRdd = DateTime.Parse("15/10/2020"),
                        newRecommendedRdd = DateTime.Parse("15/10/2020"),
                        newRecommendedRouteCode = null,
                        region = "1"
                    }
                };
            return list;
        }
        #endregion

        #region ThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsEqualOrLessThanLeadTimeAndRDDChangesAreAllowed
        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfMovedRDD_When_ThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsEqualOrLessThanLeadTimeAndRDDChangesAreAllowed() {

            var rddListOrderIsUntouched = getRddListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsEqualOrLessThanLeadTimeAndRDDChangesAreAllowed();
            var expectedListOrderIsUntouched = getExpectedListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsEqualOrLessThanLeadTimeAndRDDChangesAreAllowed();

            int i = 0;
            var loopTo = rddListOrderIsUntouched.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region test
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListNational(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListNational(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListNational(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        private List<RddOutputBean> getRddListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsEqualOrLessThanLeadTimeAndRDDChangesAreAllowed() {
            var list = new List<RddOutputBean>() {
                    new RddOutputBean() {
                        leadTime = 3,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0003",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = true,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        region = "20"
                    }
                };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsEqualOrLessThanLeadTimeAndRDDChangesAreAllowed() {
            var list = new List<CalculatedRddOutputBean>() {
                    new CalculatedRddOutputBean() {
                        leadTime = 3,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0003",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = true,
                        delBlock = null,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        newRecommendedRdd = DateTime.Parse("03/11/2020"),
                        newRecommendedRouteCode = null,
                        region = "20"
                    }
                };
            return list;
        }
        #endregion

        #region ThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsLessOrEqualThanLeadTimeAndRDDChangesNotAllowed
        [TestMethod]
        public void getCalculatedRddList_Should_returnListOfBlockedOrdersWithReasonOfNeedingToAgreeWithCustomerOnRdd_When_ThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsLessOrEqualThanLeadTimeAndRDDChangesNotAllowed() {

            var rddListOrderIsUntouched = getRddListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsLessOrEqualThanLeadTimeAndRDDChangesNotAllowed();
            var expectedListOrderIsUntouched = getExpectedListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsLessOrEqualThanLeadTimeAndRDDChangesNotAllowed();

            int i = 0;
            var loopTo = rddListOrderIsUntouched.Count - 1;

            for (i = 0; i <= loopTo; i++) {

                #region test
                var rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRdd, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRdd);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].delBlock, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).delBlock);

                rddcalc = new RddDataCalculator(bankHolidayList: getBankHolidayListRegional(), isSkipWeekend: true, tempDate: DateTime.Parse("28/10/2020"));
                Assert.AreEqual(expectedListOrderIsUntouched[i].newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddListOrderIsUntouched[i]).newRecommendedRouteCode);
                #endregion

            }
        }

        private List<RddOutputBean> getRddListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsLessOrEqualThanLeadTimeAndRDDChangesNotAllowed() {
            var list = new List<RddOutputBean>() {
                    new RddOutputBean() {
                        leadTime = 3,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0003",
                        country = "spain",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = false,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        region = "5"
                    }
                };
            return list;
        }

        private List<CalculatedRddOutputBean> getExpectedListWhenThereIsARegionalOrNationalBHBetweenTodayAndRDDAndTheRangeBetweenTodayAndRDDIsLessOrEqualThanLeadTimeAndRDDChangesNotAllowed() {
            var list = new List<CalculatedRddOutputBean>() {
                    new CalculatedRddOutputBean() {
                        leadTime = 3,
                        soldTo = 123,
                        salesOrg = "es01",
                        route = "ES0003",
                        country = "Romania",
                        isRouteCodeChangeAllowed = false,
                        soldToName = "aaa",
                        isRddChangeAllowed = false,
                        delBlock = IDAConsts.DelBlocks.holdOrderBlock,
                        oldRdd = DateTime.Parse("30/10/2020"),
                        newRecommendedRdd = DateTime.Parse("03/11/2020"),
                        newRecommendedRouteCode = null,
                        region = "5"
                    }
                };
            return list;
        }
        #endregion

        #region Bank Holiday functions
        private List<BankHolidayProperty> getBankHolidayListRegional() {
            var list = new List<BankHolidayProperty>() {
                new BankHolidayProperty() {
                    salesOrg = "es01",
                    country = "spain",
                    nationalDate = DateTime.Parse("14/10/2020"),
                    region = "1"
                },
                new BankHolidayProperty() {
                    salesOrg = "es01",
                    country = "spain",
                    nationalDate = DateTime.Parse("30/10/2020"),
                    region = "3"
                },
                new BankHolidayProperty() {
                    salesOrg = "es01",
                    country = "spain",
                    nationalDate = DateTime.Parse("29/10/2020"),
                    region = "5"
                },
            };
            return list;
        }

        private List<BankHolidayProperty> getBankHolidayListNational() {
            var list = new List<BankHolidayProperty>() {
                new BankHolidayProperty() {
                    salesOrg = "es01",
                    country = "spain",
                    nationalDate = DateTime.Parse("29/10/2020"),
                    region = ""
                }
            };
            return list;
        }

        private List<BankHolidayProperty> getBankHolidayListNationalCorrectRDD() {
            var list = new List<BankHolidayProperty>() {
                new BankHolidayProperty() {
                    salesOrg = "es01",
                    country = "spain",
                    nationalDate = DateTime.Parse("02/11/2020"),
                    region = ""
                }
            };
            return list;
        }
        #endregion
    }
}