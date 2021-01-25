
Public Class QtyConversionOrderProperty
    Public Property orderNumber As Long
    Public Property shipTo As Long
    Public Property soldTo As Long
    Public Property soldToName As String
    Public Property shipToName As String
    Public Property docDate As String
    Public Property pONumber As String
    Public Property deliveryNumber As Integer
    Public Property isSaved As Boolean
    Public Property documentLineList As List(Of DocumentLine)
    Public Property documentLineChangeList As List(Of DocumentLine)

    Public Overrides Function Equals(obj As Object) As Boolean
        Return _
            orderNumber = obj.orderNumber AndAlso
            shipTo = obj.shipTo AndAlso
            soldTo = obj.soldTo AndAlso
            soldToName = obj.soldToName AndAlso
            shipToName = obj.shipToName AndAlso
            docDate = obj.docDate AndAlso
            pONumber = obj.pONumber AndAlso
            deliveryNumber = obj.deliveryNumber AndAlso
            documentLineList.SequenceEqual(obj.documentLineList) AndAlso
            documentLineChangeList.SequenceEqual(obj.documentLineChangeList)
    End Function

    Public Function start(i As Integer) As String
        Return documentLineChangeList(i).start()
    End Function

    Public Function finish(i As Integer) As String
        Return documentLineChangeList(i).finish()
    End Function

    Public Sub removeLineByIndex(index As Integer)
        documentLineList.RemoveAt(index)
        documentLineChangeList.RemoveAt(index)
    End Sub

    Public Function removeLineByItem(item As Integer) As Boolean
        Return documentLineList.RemoveAll(Function(x) x.item = item) AndAlso
            documentLineChangeList.RemoveAll(Function(x) x.item = item)
    End Function

End Class

Public Class DocumentLine
    Public Property item As Integer
    Public Property material As Integer
    Public Property quantity As Integer
    Public Property isChanged As Boolean
    Public Property status As String
    Public Property startTime As String
    Public Property endTime As String

    Public Sub actionFailed(status As String)
        isChanged = False
        Me.status = status
    End Sub

    Public Sub actionSuccess(Optional status As String = "Success")
        isChanged = True
        Me.status = status
    End Sub

    Public Function start() As String
        startTime = Format(DateTime.Now(), "yyyyMMdd HH:mm:ss")
        Return startTime
    End Function

    Public Function finish() As String
        endTime = Format(DateTime.Now(), "yyyyMMdd HH:mm:ss")
        Return endTime
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return item = obj.item AndAlso material = obj.material AndAlso quantity = obj.quantity
    End Function

End Class


