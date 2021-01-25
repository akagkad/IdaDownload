Imports IDAUtil

Public Class RDDDataCalculatorSupport
    Private ReadOnly d As Date
    Private bankHolidayList As List(Of BankHolidayProperty)
    Private isSkipWeekend As Boolean
    Private reason As String
    Private salesOrg As String
    Private country As String

    Public Sub New(d As Date, isSkipWeekend As Boolean, bankHolidayList As List(Of BankHolidayProperty), salesOrg As String, country As String)
        Me.d = d
        Me.bankHolidayList = bankHolidayList
        Me.isSkipWeekend = isSkipWeekend
        Me.reason = reason
        Me.salesOrg = salesOrg
        Me.country = country
    End Sub

    Public Function getNextWorkingDay(d As Date, country As String) As Date
        Dim isUpdated As Boolean = False

        While bankHolidayList.Where(Function(x) x.nationalDate = d AndAlso getBHCondition(country, x)).Count > 0 OrElse (isSkipWeekend AndAlso Weekday(d, FirstDayOfWeek.Monday) > 4)
            d = DateAdd(DateInterval.Day, 1, d)
            isUpdated = True
        End While

        If Not isUpdated Then
            d = DateAdd(DateInterval.Day, 1, d)
        End If

        Return d
    End Function

    Private Function getBHCondition(country As String, x As BankHolidayProperty) As Boolean
        Select Case x.salesOrg
            Case "NL01"
                Return x.salesOrg.ToUpper = "DE01"
            Case "ES01"
                Return x.salesOrg.ToUpper = salesOrg.ToUpper AndAlso (x.country.ToLower = country.ToLower OrElse x.country.ToLower = "spain")
            Case Else
                Return x.salesOrg.ToUpper = salesOrg.ToUpper AndAlso x.country.ToLower = country.ToLower
        End Select
    End Function
End Class
