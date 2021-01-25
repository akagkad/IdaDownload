Imports System.ComponentModel.DataAnnotations.Schema
Imports IDAUtil

Public MustInherit Class OutputBean
    <Column("[soldTo]")>
    Public Property soldTo As Long
    <Column("[soldToName]")>
    Public Property soldToName As String
    <Column("[leadTime]")>
    Public Property leadTime As Integer
    <Column("[route]")>
    Public Property route As String
    <Column("[oldRdd]")>
    Public Property oldRdd As Date
    <Column("[orderNumber]")>
    Public Property orderNumber As Integer
    <Column("[docTyp]")>
    Public Property docTyp As String
    <Column("[shipTo]")>
    Public Property shipTo As Long
    <Column("[shipToName]")>
    Public Property shipToName As String
    <Column("[salesOrg]")>
    Public Property salesOrg As String
    <Column("[country]")>
    Public Property country As String
    <Column("[isRddChangeAllowed]")>
    Public Property isRddChangeAllowed As Boolean
    <Column("[isRouteCodeChangeAllowed]")>
    Public Property isRouteCodeChangeAllowed As Boolean
    <Column("[isOneDayLeadTimeAllowed]")>
    Public Property isOneDayLeadTimeAllowed As Boolean
    <Column("[deliveryDay]")>
    Public Property deliveryDay As String 'String of days that are available to deliver separated by " " i.e. "Monday Tuesday Friday"
    <Column("[loadingDate]")>
    Public Property loadingDate As Date
    <Column("[id]")>
    Public Property id As String
    <Column("[caseFillRate]")>
    Public Property caseFillRate As Double


    Public Shared Operator =(ByVal expectedList As OutputBean, ByVal outputList As OutputBean) As Boolean
        Return expectedList.leadTime = outputList.leadTime AndAlso
            expectedList.soldTo = outputList.soldTo AndAlso
            expectedList.soldToName = outputList.soldToName
    End Operator

    Public Shared Operator <>(ByVal expectedList As OutputBean, ByVal outputList As OutputBean) As Boolean
        Return Not expectedList = outputList
    End Operator

End Class

Public Class CalculatedRddOutputBean
    Inherits OutputBean

    <Column("[newRecommendedRdd]")>
    Public Property newRecommendedRdd As Date
    <Column("[reason]")>
    Public Property reason As String
    <Column("[delBLock]")>
    Public Property delBlock As String
    <Column("[newRecommendedRouteCode]")>
    Public Property newRecommendedRouteCode As String

    Public Sub New(rddOutput As RddOutputBean)
        soldTo = rddOutput.soldTo
        soldToName = rddOutput.soldToName
        leadTime = rddOutput.leadTime
        salesOrg = rddOutput.salesOrg
        shipTo = rddOutput.shipTo
        shipToName = rddOutput.shipToName
        docTyp = rddOutput.docTyp
        oldRdd = rddOutput.oldRdd
        orderNumber = rddOutput.orderNumber
        salesOrg = rddOutput.salesOrg
        country = rddOutput.country
        isRddChangeAllowed = rddOutput.isRddChangeAllowed
        isRouteCodeChangeAllowed = rddOutput.isRouteCodeChangeAllowed
        isOneDayLeadTimeAllowed = rddOutput.isOneDayLeadTimeAllowed
        deliveryDay = rddOutput.deliveryDay
        loadingDate = rddOutput.loadingDate
        route = rddOutput.route
        caseFillRate = rddOutput.caseFillRate
        id = rddOutput.id
    End Sub

    Public Sub New()
    End Sub

End Class

Public Class RddOutputBean

    Inherits OutputBean

    Public Sub New()
    End Sub

    Public Sub New(zv As ZV04HNProperty, cm As CustomerDataProperty, id As String)
        soldTo = zv.soldto
        soldToName = zv.soldtoName
        shipTo = zv.shipto
        shipToName = zv.shiptoName
        oldRdd = zv.reqDelDate
        orderNumber = zv.order
        docTyp = zv.docTyp
        salesOrg = cm.salesOrg.ToUpper
        country = cm.country.ToLower
        isRddChangeAllowed = cm.changeRDDActionAllowed
        isRouteCodeChangeAllowed = cm.changeRouteCodeActionAllowed
        deliveryDay = cm.deliveryDay
        loadingDate = zv.loadingDate
        route = zv.route
        caseFillRate = zv.FillRate
        isOneDayLeadTimeAllowed = cm.oneDayLeadTimeAllowed
        leadTime = getLeadTime(salesOrg, cm, zv)
        Me.id = id
    End Sub

    Private Function getLeadTime(salesOrg As String, cm As CustomerDataProperty, zv As ZV04HNProperty) As Integer
        Select Case salesOrg
            Case "ZA01", "RU01", "UA01", "GR01", "CZ01", "PL01", "IT01"
                Return If(zv.route <> "", Right(zv.route, 2), 0)
            Case "ES01"
                Select Case zv.route
                    Case "ES0011"
                        Return 1
                    Case "ES0000"
                        Return 2
                    Case "ES0002"
                        Return 3
                    Case "ES0003"
                        Return 4
                    Case "ESTN05"
                        Return 5
                    Case "ESGC05"
                        Return 5
                    Case Else
                        Return 0
                End Select
            Case Else
                Return cm.leadTime
        End Select
    End Function
End Class