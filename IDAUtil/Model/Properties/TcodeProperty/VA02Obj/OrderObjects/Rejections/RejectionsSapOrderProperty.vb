Public Class RejectionsSapOrderProperty
    Public Property salesOrg As String
    Public Property orderNumber As Long
    Public Property lineDetails As List(Of RejectionsSapLineProperty)

    Public Sub New()
    End Sub
End Class

Public Class RejectionsSapLineProperty
    Public Property sku As Long
    Public Property lineNumber As Long
    Public Property orderedQty As Long
    Public Property confirmedQty As Long
    Public Property rejectionCode As String
    Public Property isReplacePartialCut As Boolean
    Public Property reason As String

    Public Sub New()
    End Sub
End Class