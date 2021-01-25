Imports System.Text.RegularExpressions
Imports IDAUtil

Public Class RddDataCalculator
    Private bankHolidayList As List(Of BankHolidayProperty)
    Private isSkipWeekend As Boolean

    Private tempDate As Date = DateTime.Today
    Private tempReason As String
    Private tempDelBlock As String
    Private tempRouteCode As String

    Enum CountryException
        'CZ and PL exception - change route codes if the orders have 100% case fill rate in the first release and change route codes to 1 for the first release
        CEHUB

        'Romania has a contract with LSP to reduce lead time to a minimum of 1
        'Romania exception - reduce route code to a minimum of 1 to accommodate for short lead time instead of postponing date and/or blocking the order
        Romania

        'GB exception - change route code to emergency on fridays for orders with delivery on saturday
        GB

        'ES and PT flag orders with rdd that are more than 7 days in advance
        SpainAndPortugal

        'KE and NG has 0 days lead time meaning release on the same day
        KEAndNG

        'orders on weekends when its not allowed
        weekendOrders

        'no exception
        noException
    End Enum

    Public Sub New(bankHolidayList As List(Of BankHolidayProperty), isSkipWeekend As Boolean)
        Me.bankHolidayList = bankHolidayList
        Me.isSkipWeekend = isSkipWeekend
    End Sub

    Public Function getCalculatedRDDList(rddList As List(Of RddOutputBean)) As List(Of CalculatedRddOutputBean)
        Dim calculatedRDDList As New List(Of CalculatedRddOutputBean)

        For Each rdd As RddOutputBean In rddList
            calculatedRDDList.Add(calculateRDDTasks(rdd))
        Next

        Return calculatedRDDList
    End Function

    Public Function calculateRDDTasks(rdd As RddOutputBean) As CalculatedRddOutputBean
        If rdd.route = "" Then
            Return routeCodeMissingAction(rdd)
        End If

        findClosestRDDByLeadTime(rdd) 'sets tempDate as the closest rdd by lead time

        Dim countryException As CountryException = getCountryException(rdd)
        Dim preReleaseTime As DateTime = getLastPrereleaseTime(rdd.salesOrg)

        If TimeOfDay > preReleaseTime AndAlso preReleaseTime <> #00:00:00# Then tempReason &= $"RDD moved by 1 day for tomorrows release.{vbCr}"

        'Adds bypass changes block for all orders that have short lead time
        If tempDate > rdd.oldRdd AndAlso Not rdd.isRddChangeAllowed AndAlso (countryException = CountryException.noException OrElse countryException = CountryException.SpainAndPortugal) Then
            shortLeadTimeAction()
        End If

        'adds +1 day to the calculated date until it matches customer speciffic day of delivery
        If getDeliveryDayCondition(rdd) Then
            specifficDeliveryDayAction(rdd)
            'recalculate if the exception is still relevant
            countryException = getCountryException(rdd)
        End If

        countryExceptionAction(rdd, countryException)

        addBHListToTempReason(rdd)

        If tempReason = "" Then
            tempReason &= $"Rdd changed to next available day.{vbCr}"
        End If

        Dim calculatedValues As CalculatedRddOutputBean = getCalculatedRddOutputBean(rdd, tempDate, tempReason, tempDelBlock, tempRouteCode)

        clearTempValues()

        Return calculatedValues
    End Function

    Private Function getDeliveryDayCondition(rdd As RddOutputBean) As Boolean
        Return rdd.deliveryDay <> "" AndAlso Not rdd.deliveryDay.ToLower.Contains(Format(tempDate, "dddd").ToLower) AndAlso (tempDate > rdd.oldRdd AndAlso Not rdd.deliveryDay.ToLower.Contains(Format(rdd.oldRdd, "dddd").ToLower))
    End Function

    Private Function getCalculatedRddOutputBean(rdd As RddOutputBean, CalculatedDate As Date, calculatedReason As String, CalculatedDelBLock As String, CalculatedRouteCode As String) As CalculatedRddOutputBean
        Return New CalculatedRddOutputBean(rdd) With {
                    .newRecommendedRdd = CalculatedDate,
                    .reason = calculatedReason,
                    .delBlock = CalculatedDelBLock,
                    .newRecommendedRouteCode = CalculatedRouteCode
                }
    End Function

    Private Sub addBHListToTempReason(rdd As RddOutputBean)
        Dim usedBHList As List(Of String) = getUsedBHList(rdd)
        Dim bhStr As String = String.Join(", ", usedBHList)
        If bhStr <> "" Then
            tempReason &= $"Present bank holidays: {bhStr}.{vbCr}"
        End If
    End Sub

    Private Sub clearTempValues()
        tempDate = DateTime.Today
        tempDelBlock = ""
        tempReason = ""
        tempRouteCode = ""
    End Sub

    Private Function getUsedBHList(rdd As RddOutputBean) As List(Of String)
        Dim usedBHList As New List(Of String)
        For Each bh In bankHolidayList
            If bh.nationalDate >= DateTime.Today AndAlso bh.nationalDate <= tempDate AndAlso getBHCondition(rdd, bh) Then
                usedBHList.Add(Format(bh.nationalDate, "dd.MM.yyyy"))
                usedBHList.Add(bh.country)
            End If
        Next

        Return usedBHList
    End Function

    Private Shared Function getBHCondition(rdd As RddOutputBean, bh As BankHolidayProperty) As Boolean
        Select Case rdd.salesOrg
            Case "NL01"
                Return bh.salesOrg.ToUpper = "DE01"
            Case "ES01"
                Return bh.country.ToLower = rdd.country.ToLower OrElse bh.country.ToLower = "spain"
            Case Else
                Return bh.country.ToLower = rdd.country.ToLower
        End Select
    End Function

    Private Sub specifficDeliveryDayAction(rdd As RddOutputBean)
        Dim deliveryDaysAccepted As String = Regex.Replace(rdd.deliveryDay, "[^a-zA-Z]", "")

        If deliveryDaysAccepted <> "" Then
            While Not deliveryDaysAccepted.ToLower.Contains(Format(tempDate, "dddd").ToLower) OrElse bankHolidayList.Where(Function(x) x.nationalDate = tempDate).Count > 0
                tempDate = DateAdd(DateInterval.Day, 1, tempDate)
            End While
        End If
        'changes block to hold order if the order is not short lead time
        If Not tempDelBlock = IDAConsts.bypassOrderChangesBlock AndAlso Not rdd.isRddChangeAllowed Then
            tempDelBlock = IDAConsts.holdOrderBlock
        End If

        tempReason &= $"Holding order to contact the customer for changing RDD to recommended specific acceptance day - {Format(tempDate, "dddd")}.{vbCr}"
    End Sub

    Private Sub RomaniaExceptionAction(rdd As RddOutputBean)
        'difference in days between calculated rdd's and current rdd's
        Dim rddDiff As Long = DateDiff("d", DateTime.Today, tempDate) - DateDiff("d", DateTime.Today, rdd.oldRdd)

        If rddDiff > 2 Then
            tempRouteCode = ""
            tempDelBlock = IDAConsts.bypassOrderChangesBlock
            tempReason &= $"Lead time too short. Unable to decrease route code.{vbCr}"
        Else
            'Replaces last digit of the route code string with the minimum value of a lead time i.e RO0003 to RO0001
            tempRouteCode = Replace(rdd.route, rdd.route.Last, rdd.route.Last.ToString - rddDiff)
            If tempDelBlock = IDAConsts.bypassOrderChangesBlock Then tempDelBlock = ""
            tempDate = rdd.oldRdd
            tempReason &= $"Route code changed to accommodate for short lead time to: {tempRouteCode}.{vbCr}"
        End If
    End Sub

    Private Function getCountryException(rdd As RddOutputBean) As CountryException
        Select Case True
            Case (rdd.salesOrg = "CZ01" OrElse rdd.salesOrg = "PL01") AndAlso rdd.isRouteCodeChangeAllowed
                Return CountryException.CEHUB
            Case rdd.salesOrg = "GB01" AndAlso rdd.isRouteCodeChangeAllowed AndAlso DateTime.Today.DayOfWeek = DayOfWeek.Friday AndAlso rdd.oldRdd < tempDate
                Return CountryException.GB
            Case rdd.salesOrg = "RO01" AndAlso rdd.isRouteCodeChangeAllowed AndAlso tempDate > rdd.oldRdd
                Return CountryException.Romania
            Case rdd.salesOrg = "ES01" OrElse rdd.salesOrg = "PT01"
                Return CountryException.SpainAndPortugal
            Case rdd.salesOrg = "KE02" AndAlso rdd.salesOrg = "NG01" AndAlso rdd.leadTime = 0 AndAlso rdd.oldRdd <> Date.Today
                Return CountryException.KEAndNG
            Case Else
                Return CountryException.noException
        End Select
    End Function

    Private Sub countryExceptionAction(rdd As RddOutputBean, countryException As CountryException)
        Select Case countryException
            Case CountryException.CEHUB
                CEHUBExceptionAction(rdd)
            Case CountryException.GB
                GBExceptionAction()
            Case CountryException.Romania
                RomaniaExceptionAction(rdd)
            Case CountryException.KEAndNG
                tempReason &= $"Zero days lead time. Moved rdd to todays date.{vbCr}"
                tempDate = Date.Today
            Case CountryException.SpainAndPortugal
                If DateDiff("d", tempDate, rdd.oldRdd) > 7 Then tempReason &= $"Warning: RDD more than 7 days in advance.{vbCr}"
                If Weekday(rdd.oldRdd, FirstDayOfWeek.Monday) > 5 AndAlso Not rdd.isRddChangeAllowed Then
                    tempReason &= $"RDD on weekends not allowed.{vbCr}"
                    tempDelBlock = IDAConsts.bypassOrderChangesBlock
                    tempDate = rdd.oldRdd
                End If

                'if the leadtime is more than lead time and BH exists then block 
                'if bh exists and lead time is short and customer allows move rdd
                'if bh exists and lead time is short and customer doesnt allow block

            Case CountryException.noException
                'nothing to be done
            Case Else
                Throw New NotImplementedException
        End Select
    End Sub

    Private Sub shortLeadTimeAction()
        tempDelBlock = IDAConsts.bypassOrderChangesBlock
        tempReason &= $"Order blocked due to short lead time.{vbCr}"
    End Sub

    Private Sub GBExceptionAction()
        tempRouteCode = "GB0000"
        tempReason &= $"Route code changed to emergency for a friday order to be released, New Rote code: {tempReason}.{vbCr}"
    End Sub

    Private Sub CEHUBExceptionAction(rdd As RddOutputBean)
        'Dim leadTimeDiff As Integer = DateDiff("d", tempDate, rdd.oldRdd) 'difference between calculated rdd and the current rdd
        'Dim leadTime As Integer
        'Dim maxLeadTime As Integer = 3

        'If rdd.oldRdd > tempDate AndAlso rdd.leadTime < maxLeadTime AndAlso rdd.caseFillRate = 100.0 Then
        '    Dim i As Date = tempDate
        '    While i <= rdd.oldRdd

        '        If bankHolidayList.Contains(New BankHolidayProperty With {.country = rdd.country, .salesOrg = rdd.salesOrg, .nationalDate = i}) OrElse Weekday(i, FirstDayOfWeek.Monday) > 5 Then
        '            leadTimeDiff -= 1
        '        End If

        '        i = i.AddDays(1)
        '    End While

        '    If (leadTimeDiff + rdd.leadTime) > maxLeadTime Then
        '        leadTime = maxLeadTime
        '    Else
        '        leadTime = leadTimeDiff + rdd.leadTime
        '    End If

        '    tempRouteCode = Replace(rdd.route, rdd.leadTime, leadTime)
        '    tempReason &= $"Order has 100% CFR. Route code increased to {leadTime} days to release order sooner to give more time to LSP.{vbCr}"
        '    tempDate = rdd.oldRdd
        'End If

        'If before first release change route codes to 1, if after first release then block orders
        Dim currentTime As Date = DateTime.Now

        If rdd.salesOrg = "CZ01" Then
            If currentTime < #09:00:00# AndAlso rdd.leadTime > 1 AndAlso rdd.isRouteCodeChangeAllowed AndAlso rdd.isOneDayLeadTimeAllowed AndAlso tempRouteCode = "" Then
                tempRouteCode = Replace(rdd.route, rdd.leadTime, 1)
                tempReason &= $"Route code decreased for first release.{vbCr}"
            End If

            If rdd.leadTime = 1 AndAlso currentTime > #09:00:00# AndAlso tempRouteCode = "" Then
                tempDelBlock = IDAConsts.bypassOrderChangesBlock

                If currentTime > #09:00:00# Then tempReason &= $"Not allowed to have route code for 1 day after first release.{vbCr}"
                If rdd.isRouteCodeChangeAllowed Then tempReason &= $"Not allowed to have route code change for this customer.{vbCr}"
                If rdd.isOneDayLeadTimeAllowed Then tempReason &= $"Not allowed to have route code of 1 day for this customer.{vbCr}"
            End If
        End If
    End Sub

    Private Function routeCodeMissingAction(rdd As RddOutputBean) As CalculatedRddOutputBean
        Dim reason As String = $"Warning!!! Route codes are missing!!!{vbCr}"
        Dim calcDelBlock As String = IDAConsts.bypassOrderChangesBlock

        Return New CalculatedRddOutputBean(rdd) With {
        .newRecommendedRdd = rdd.oldRdd,
        .reason = reason,
        .delBlock = calcDelBlock,
        .newRecommendedRouteCode = ""
    }
    End Function

    Private Sub findClosestRDDByLeadTime(rdd As RddOutputBean)
        Dim salesOrgLastPreReleaseTime As DateTime = getLastPrereleaseTime(rdd.salesOrg)
        Dim rddSupport = CreateRDD.RddCalculatorSupport(tempDate, isSkipWeekend, bankHolidayList, rdd.salesOrg, rdd.country)
        Dim wdCount As Integer = rdd.leadTime

        If bankHolidayList.Where(Function(x) x.nationalDate = tempDate AndAlso getBHCondition(rdd, x)).Count > 0 Then
            wdCount += 1
        End If

        '#00:00:00# means country doesnt have pre release
        If salesOrgLastPreReleaseTime <> #00:00:00# AndAlso TimeOfDay >= salesOrgLastPreReleaseTime Then
            wdCount += 1
        End If

        While wdCount > 0
            tempDate = rddSupport.getNextWorkingDay(tempDate, rdd.country)
            wdCount -= 1

            If bankHolidayList.Where(Function(x) x.nationalDate = tempDate AndAlso getBHCondition(rdd, x)).Count > 0 Then
                wdCount += 1
            Else
                'NL01 Exception
                If wdCount = 0 AndAlso bankHolidayList.Where(Function(x) x.nationalDate = tempDate AndAlso rdd.salesOrg = "NL01").Count > 0 Then
                    wdCount += 1
                End If
            End If
        End While
    End Sub

    Private Function getLastPrereleaseTime(salesOrg As String) As DateTime
        Select Case salesOrg
            Case "ZA01"
                Return #15:30:00#
            Case "TR01"
                Return #16:00:00#
            Case "RU01"
                Return #14:00:00#
            Case Else
                Return #00:00:00#
        End Select
    End Function
End Class
