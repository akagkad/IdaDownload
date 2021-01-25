Public Class SwitchesOrderPropertyFactory

    Public Sub New()
    End Sub

    Public Function getSapSwitchObjectList(spl As List(Of SwitchesProperty)) As List(Of SwitchesSapOrderProperty)
        Dim sapSwitchObjectQuery As List(Of SwitchesSapOrderProperty) =
            (From q In spl
             Group q By line = New With {Key q.order, q.salesOrg, q.soldTo}
                                    Into Group
             Select New SwitchesSapOrderProperty With {
            .salesOrg = line.salesOrg,
            .order = line.order,
            .soldTo = line.soldTo,
            .lineDetails = Group.Aggregate(New List(Of SwitchesSapLineProperty),
                Function(x, q)
                    x.Add(New SwitchesSapLineProperty() With {
                        .lineNumber = q.item,
                        .oldSku = q.oldSku,
                        .newSku = q.newSku,
                        .isSameBarcode = (q.oldSkuCaseBarcode = q.newSkuCaseBarcode AndAlso q.oldSkuUnitBarcode = q.newSkuUnitBarcode),
                        .reason = $"{If(q.switchForCustomer = "All", "Country level switch. ", "Speciffic switch for " & q.soldTo & " - " & q.shipToName & ". ")} {If(q.needOutOfStockToSwitch = True, "Sku " & q.oldSku & " is out of stock for this order. (order qty - " & q.orderedQty & " confirmed qty - " & q.confirmedQty & " ) ", "Switch was performed regardless of stock levels. " & q.switchComment)}"})
                    Return x
                End Function)}).ToList

        Return sapSwitchObjectQuery
    End Function
End Class
