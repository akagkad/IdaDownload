Imports IDAUtil
Imports [lib]

Public Class DataCollectorServiceRejections
    Private ReadOnly dataCollectorServer As IDataCollectorServer
    Private ReadOnly dataCollectorSap As IDataCollectorSap

    Public Sub New(dataCollectorServer As IDataCollectorServer, dataCollectorSap As IDataCollectorSap)
        Me.dataCollectorServer = dataCollectorServer
        Me.dataCollectorSap = dataCollectorSap
    End Sub

    Public Function getRejectionsList(salesOrg As String, id As String) As List(Of RejectionsProperty)
        Dim ZVList As List(Of ZV04IProperty) = getZvList(salesOrg)
        If ZVList Is Nothing Then Return Nothing
        Dim cdList As List(Of CustomerDataProperty) = GetCdList(salesOrg)
        Dim rdList As List(Of RejectionsDataProperty) = getRdList(salesOrg)

        Dim allSoldToList As List(Of RejectionsProperty) = GetLinq(id, rdList, ZVList, cdList, isSpecifficSoldTo:=False)
        Dim SpecifficSoldToList As List(Of RejectionsProperty) = GetLinq(id, rdList, ZVList, cdList, isSpecifficSoldTo:=True)
        Dim finalList As New List(Of RejectionsProperty)

        If SpecifficSoldToList.Count > 0 Then
            finalList.AddRange(SpecifficSoldToList)

            'removes speciffic sold to switches from general switches for all sold tos
            allSoldToList = allSoldToList.Where(Function(x) SpecifficSoldToList.Any(Function(y) y.soldTo <> x.soldTo)).ToList
        End If

        finalList.AddRange(allSoldToList)

        finalList = getFinalListWithStockDetails(finalList, salesOrg)

        finalList.OrderBy(Function(x) x.soldTo).ThenBy(Function(x) x.item)

        Return finalList
    End Function

    Private Function GetCdList(salesOrg As String) As List(Of CustomerDataProperty)
        Return dataCollectorServer.getCustomerDataList(salesOrg)
    End Function

    Private Function getFinalListWithStockDetails(list As List(Of RejectionsProperty), salesOrg As String) As List(Of RejectionsProperty)
        Dim sap As ISAPLib = Create.sapLib
        Dim co09 As New CO09(sap)

        Dim listOfUniqueSkus As List(Of Long) = (From x In list Select x.sku).Distinct.ToList

        Dim co09ObjSkuList As New List(Of CO09Property)

        For Each item In listOfUniqueSkus
            co09ObjSkuList.Add(co09.getStockDetails(item, salesOrg))
        Next

        'assigns value to final list passed to func from CO09 lists
        For Each listItem In list
            For Each co09Item In co09ObjSkuList
                If listItem.sku = co09Item.sku Then
                    listItem.skuATP = co09Item.ATP
                    listItem.skuRecoveryDate = co09Item.recoveryDate
                    listItem.skuRecoveryQty = co09Item.recoveryQty
                    Exit For
                End If
            Next
        Next

        Return list
    End Function

    Private Function GetLinq(id As String, rdList As List(Of RejectionsDataProperty), zvList As List(Of ZV04IProperty), cdList As List(Of CustomerDataProperty), isSpecifficSoldTo As Boolean) As List(Of RejectionsProperty)
        Return (From zv In zvList
                Join cd In cdList
                    On zv.soldTo Equals cd.soldTo And zv.shipTo Equals cd.shipTo
                Join rd In rdList
                   On zv.material Equals rd.sku And cd.country.ToLower Equals rd.country.ToLower
                Where getStockConditon(rd, zv) AndAlso getSoldToCondition(isSpecifficSoldTo, zv, rd)
                Select getSwitchPropertyObjs(zv, rd, cd, id)).ToList()
    End Function

    Private Shared Function getSoldToCondition(isSpecifficSoldTo As Boolean, zv As ZV04IProperty, rd As RejectionsDataProperty) As Boolean
        Return If(isSpecifficSoldTo, rd.soldTo = zv.soldTo, rd.soldTo = 0)
    End Function

    Private Function getRdList(salesOrg As String) As List(Of RejectionsDataProperty)
        Return dataCollectorServer.getRejectionsDataList(salesOrg).Where(Function(x) Date.Today >= x.startDate AndAlso Date.Today <= x.endDate).ToList
    End Function

    Private Function getZvList(salesOrg As String) As List(Of ZV04IProperty)
        Dim list As List(Of ZV04IProperty) = dataCollectorSap.getZV04IList(salesOrg, IDAEnum.Task.rejections)
        If list Is Nothing Then
            Return Nothing
        Else
            Return list.Where(Function(x) x.delBlock <> IDAConsts.bypassOrderChangesBlock AndAlso x.delBlock <> "Z4" AndAlso x.delBlock <> "04" AndAlso x.rejReason = "").ToList
        End If
    End Function

    Private Function getSwitchPropertyObjs(zv As ZV04IProperty, rd As RejectionsDataProperty, cd As CustomerDataProperty, id As String) As RejectionsProperty
        Return New RejectionsProperty(
            salesOrg:=cd.salesOrg,
            country:=cd.country,
            soldTo:=zv.soldTo,
            rejectionForCustomer:=If(rd.soldTo = 0, "All", "Speciffic"),
            isReplacePartialCut:=(cd.replaceObsoletePartialCutsAllowed AndAlso zv.confirmedQty = 0 AndAlso rd.needOutOfStockToReject),
            shipTo:=zv.shipTo,
            shipToName:=zv.shipToName,
            orderNumber:=zv.order,
            item:=zv.item,
            sku:=rd.sku,
            rejectionReasonCode:=rd.rejectionReasonCode,
            orderedQty:=zv.orderQty,
            confirmedQty:=zv.confirmedQty,
            skuUnitBarcode:=rd.skuUnitBarcode,
            skuCaseBarcode:=rd.skuCaseBarcode,
            skuATP:=Nothing,
            skuRecoveryDate:=Nothing,
            skuRecoveryQty:=Nothing,
            startDate:=rd.startDate,
            endDate:=rd.endDate,
            needOutOfStockToReject:=rd.needOutOfStockToReject,
            rejectionComment:=rd.rejectionComment,
            id:=id
            )
    End Function

    Private Function getStockConditon(rd As RejectionsDataProperty, zv As ZV04IProperty) As Boolean
        Return (rd.needOutOfStockToReject = True AndAlso zv.confirmedQty < zv.orderQty) OrElse rd.needOutOfStockToReject = False
    End Function
End Class
