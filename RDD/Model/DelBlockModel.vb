Imports IDAUtil

Public Class DelBlockModel
    Inherits CalcModel

    Public Sub New(calcOutputBean As CalculatedRddOutputBean)
        MyBase.New(calcOutputBean)
    End Sub

    Public Overrides Function execute(va02 As VA02) As Boolean
        va02.runDeliveryBlockChange(orderNumber, id, reason, delBlock, "DeliveryDatesLog")
        Return True
    End Function
End Class
