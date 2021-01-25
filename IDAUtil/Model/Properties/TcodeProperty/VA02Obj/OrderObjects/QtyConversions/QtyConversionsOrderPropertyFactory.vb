Public Module QtyConversionsOrderPropertyFactory
    Public Function createSalesDocumentList(zv04IList As List(Of ZV04IProperty)) As List(Of QtyConversionOrderProperty)
        Dim baseSalesDocumentQuery =
        From q In zv04IList
        Group q By line = New With {Key q.order, q.shipTo, q.soldTo, q.shipToName, q.docDate, q.pONumber, q.delivery}
        Into Group
        Select base = New QtyConversionOrderProperty With {
            .orderNumber = line.order,
            .deliveryNumber = line.delivery,
            .docDate = line.docDate,
            .pONumber = line.pONumber,
            .shipTo = line.shipTo,
            .soldTo = line.soldTo,
            .soldToName = line.shipToName,
            .documentLineList = Group.Aggregate(New List(Of DocumentLine), Function(x, q)
                                                                               x.Add(New DocumentLine() With {.item = q.item, .material = q.material, .quantity = q.orderQty})
                                                                               Return x
                                                                           End Function),
            .documentLineChangeList = .documentLineList
        }
        Return baseSalesDocumentQuery.ToList()
    End Function
End Module
