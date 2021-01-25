Imports IDAUtil

Public Class RDDModel
    Inherits CalcModel

    Public Sub New(calcOutputBean As CalculatedRddOutputBean)
        MyBase.New(calcOutputBean)
    End Sub

    Public Overrides Function execute(va02 As VA02) As Boolean
        va02.runRDDChange(orderNumber, oldRdd, newRdd, id, reason, "DeliveryDatesLog")
        Return True
    End Function
End Class
