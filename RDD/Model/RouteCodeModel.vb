Imports IDAUtil

Public Class RouteCodeModel
    Inherits CalcModel

    Public Sub New(calcOutputBean As CalculatedRddOutputBean)
        MyBase.New(calcOutputBean)
    End Sub

    Public Overrides Function execute(va02 As VA02) As Boolean
        va02.runRouteCodeChange(orderNumber, id, reason, recommendedNewRouteCode, "DeliveryDatesLog")
        Return True
    End Function
End Class
