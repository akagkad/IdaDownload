Public Class SwitchesSapOrderProperty
    Public Property salesOrg As String
    Public Property order As Long
    Public Property soldTo As Long
    Public Property lineDetails As List(Of SwitchesSapLineProperty)

    Public Sub New()
    End Sub

End Class

Public Class SwitchesSapLineProperty
    Public Property oldSku As Long
    Public Property lineNumber As Long
    Public Property isSameBarcode As Boolean
    Public Property reason As String
    Public Property newSku As Long

    Public Sub New()
    End Sub

End Class