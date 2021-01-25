Imports IDAUtil

Public MustInherit Class CalcModel
    Property isRddChangeAllowed As Boolean
    Property orderNumber As Integer
    Property newRdd As Date
    Property oldRdd As Date
    Property reason As String
    Property delBlock As String
    Property recommendedNewRouteCode As String
    Property id As String

    Public Sub New(calcOutputBean As CalculatedRddOutputBean)
        isRddChangeAllowed = calcOutputBean.isRddChangeAllowed
        orderNumber = calcOutputBean.orderNumber
        newRdd = calcOutputBean.newRecommendedRdd
        oldRdd = calcOutputBean.oldRdd
        delBlock = calcOutputBean.delBlock
        reason = calcOutputBean.reason
        recommendedNewRouteCode = calcOutputBean.newRecommendedRouteCode
        id = calcOutputBean.id
    End Sub

    Public MustOverride Function execute(va02 As VA02) As Boolean
End Class