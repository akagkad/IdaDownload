Imports IDAUtil

Public Class DataCompareService

    Public Function getSalesDocumentToChangeQuantity(zvo4iList As List(Of ZV04IProperty), shipToConversionList As List(Of ConversionShiptTo), materialConversionList As List(Of ConversionMaterial)) As List(Of QtyConversionOrderProperty)
        'TODO: move to function
        Dim zv04IChangeList = From zv04Item In zvo4iList Where zv04Item.rejReason = ""
                              Join shipConversion In shipToConversionList On zv04Item.shipTo Equals shipConversion.shipTo
                              Join materialConversion In materialConversionList On zv04Item.material Equals materialConversion.material
                              Select zv04Item.order, zv04Item.shipToName, zv04Item.soldToName, zv04Item.soldTo, zv04Item.item, zv04Item.shipTo, zv04Item.material, zv04Item.orderQty, materialConversion.conversionIndex

        'TODO: refactor
        Dim baseSalesDocumentQuery =
            From q In zv04IChangeList
            Group q By line = New With {Key q.order, q.shipTo, q.soldTo, q.shipToName, q.soldToName}
                                    Into Group
            Select New QtyConversionOrderProperty With {
            .orderNumber = line.order,
            .shipTo = line.shipTo,
            .soldTo = line.soldTo,
            .shipToName = line.shipToName,
            .soldToName = line.soldToName,
            .documentLineList = Group.Aggregate(New List(Of DocumentLine), Function(x, q)
                                                                               x.Add(New DocumentLine() With {.item = q.item, .material = q.material, .quantity = q.orderQty})
                                                                               Return x
                                                                           End Function),
            .documentLineChangeList = Group.Aggregate(New List(Of DocumentLine), Function(x, q)
                                                                                     x.Add(New DocumentLine() With {.item = q.item, .material = q.material, .quantity = q.orderQty * q.conversionIndex})
                                                                                     Return x
                                                                                 End Function)
            }

        Return baseSalesDocumentQuery.ToList()
    End Function

    Public Function removeAlreadyConvertedLines(logConversionList As List(Of ConversionLog), ByRef salesDocumetList As List(Of QtyConversionOrderProperty)) As List(Of QtyConversionOrderProperty)
        For i = salesDocumetList.Count - 1 To 0 Step -1
            For Each logLine In logConversionList
                'delete line if was already converted
                If salesDocumetList(i).orderNumber = logLine.orderNumber AndAlso isSuccessfullyConvertedLineAlreadyInLog(salesDocumetList(i), logLine) Then
                    salesDocumetList(i).removeLineByItem(logLine.item)
                    'delete document if all items in the document were changed priviously
                    If salesDocumetList(i).documentLineChangeList.Count = 0 Then
                        salesDocumetList.RemoveAt(i)
                    End If
                End If
            Next
        Next
        Return salesDocumetList
    End Function

    Private Function isSuccessfullyConvertedLineAlreadyInLog(document As QtyConversionOrderProperty, logLine As ConversionLog) As Boolean
        For i = document.documentLineChangeList.Count - 1 To 0 Step -1
            If document.documentLineChangeList(i).item = logLine.item AndAlso logLine.isConverted AndAlso logLine.isSaved Then
                Return True
            End If
        Next
        Return False
    End Function

    <Obsolete>
    Public Function getNotPreviouslyChangedDocuments(salesDocumentList As List(Of QtyConversionOrderProperty), logConversionList As List(Of ConversionLog)) As List(Of QtyConversionOrderProperty)
        For i = salesDocumentList.Count - 1 To 0 Step -1
            For j = 0 To logConversionList.Count - 1
                If logConversionList(j).isConverted AndAlso salesDocumentList(i).orderNumber = logConversionList(j).orderNumber Then
                    salesDocumentList.RemoveAt(i)
                End If
            Next
        Next
        Return salesDocumentList
    End Function

End Class
