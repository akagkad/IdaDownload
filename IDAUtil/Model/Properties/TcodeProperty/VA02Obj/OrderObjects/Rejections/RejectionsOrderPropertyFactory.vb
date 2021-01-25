Public Class RejectionsOrderPropertyFactory
    Public Sub New()
    End Sub

    Public Function getSapRejectionsObjectList(rpl As List(Of RejectionsProperty)) As List(Of RejectionsSapOrderProperty)
        Dim sapSwitchObjectQuery As List(Of RejectionsSapOrderProperty) =
            (From r In rpl
             Group r By line = New With {Key r.orderNumber, r.salesOrg}
                                    Into Group
             Select New RejectionsSapOrderProperty With {
            .salesOrg = line.salesOrg,
            .orderNumber = line.orderNumber,
            .lineDetails = Group.Aggregate(New List(Of RejectionsSapLineProperty),
                Function(x, q)
                    x.Add(New RejectionsSapLineProperty() With {
                        .lineNumber = q.item,
                        .sku = q.sku,
                        .orderedQty = q.orderedQty,
                        .confirmedQty = q.confirmedQty,
                        .rejectionCode = q.rejectionReasonCode,
                        .isReplacePartialCut = (q.isReplacePartialCut),
                        .reason = $"{If(q.rejectionForCustomer = "All", "Country level rejection. ", "Speciffic rejection for " & q.soldTo & " - " & q.shipToName & ". ")} {If(q.needOutOfStockToReject = True, "Sku " & q.sku & " is out of stock for this order. (order qty - " & q.orderedQty & " confirmed qty - " & q.confirmedQty & " ) ", "Rejection was performed regardless of stock levels. " & q.rejectionComment)}"})
                    Return x
                End Function)}).ToList

        Return sapSwitchObjectQuery
    End Function
End Class
