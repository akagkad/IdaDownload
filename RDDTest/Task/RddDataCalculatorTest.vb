Imports IDAUtil
Imports RDD

<TestClass()>
Public Class RddDataCalculatorTest

    <TestInitialize>
    Public Sub init()
    End Sub

    <TestCleanup>
    Public Sub cleanUp()
    End Sub

    <TestMethod>
    Public Sub getCalculatedRddList_Should_returnListOfNewRDD_When_allGood()
        Dim rddList As List(Of RddOutputBean) = getRddList()
        Dim expectedList As List(Of CalculatedRddOutputBean) = getExpectedList()
        Dim rddcalc As New RddDataCalculator(getBankHolidayList(), True)

        Dim i As Integer = 0
        For i = 0 To rddList.Count - 1
            Assert.AreEqual(expectedList(i).newRecommendedRdd, rddcalc.calculateRDDTasks(rddList(i)).newRecommendedRdd)
            Assert.AreEqual(expectedList(i).delBlock, rddcalc.calculateRDDTasks(rddList(i)).delBlock)
            Assert.AreEqual(expectedList(i).newRecommendedRouteCode, rddcalc.calculateRDDTasks(rddList(i)).newRecommendedRouteCode)
            Assert.AreNotSame("", rddcalc.calculateRDDTasks(rddList(i)).reason)
        Next i

    End Sub

    Private Function getBankHolidayList() As List(Of BankHolidayProperty)
        Dim list As New List(Of BankHolidayProperty)

        'list.Add(New BankHolidayProperty() With {
        '.nationalDate = #02/27/2020#,
        '.salesOrg = "NL01"
        '})

        Return list
    End Function

    Private Function getRddList() As List(Of RddOutputBean)
        Dim list As New List(Of RddOutputBean) From {
            New RddOutputBean() With {
                .leadTime = 1,
                .soldTo = 123,
                .salesOrg = "PL01",
                .route = "PL0001",
                .isRouteCodeChangeAllowed = True,
                .isOneDayLeadTimeAllowed = False,
                .soldToName = "aaa",
                .isRddChangeAllowed = False,
                .caseFillRate = 100.0,
                .oldRdd = #05/11/2020#
                 }
        }

        Return list
    End Function

    Private Function getExpectedList() As List(Of CalculatedRddOutputBean)
        Dim list As New List(Of CalculatedRddOutputBean) From {
            New CalculatedRddOutputBean() With {
                .leadTime = 1,
                .salesOrg = "PL01",
                .newRecommendedRdd = #05/11/2020#,
                .soldTo = 123,
                .isRouteCodeChangeAllowed = True,
                .isOneDayLeadTimeAllowed = False,
                .delBlock = "",
                .newRecommendedRouteCode = "PL0002",
                .route = "PL0001",
                .caseFillRate = 100.0,
                .soldToName = "aaa",
                .isRddChangeAllowed = False,
                .oldRdd = #05/11/2020#
                 }
        }

        Return list
    End Function

End Class