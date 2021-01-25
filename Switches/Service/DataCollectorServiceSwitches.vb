Imports IDAUtil
Imports [lib]

Public Class DataCollectorServiceSwitches
    Private ReadOnly dataCollectorServer As IDataCollectorServer
    Private ReadOnly dataCollectorSap As IDataCollectorSap

    Public Sub New(dataCollectorServer As IDataCollectorServer, dataCollectorSap As IDataCollectorSap)
        Me.dataCollectorServer = dataCollectorServer
        Me.dataCollectorSap = dataCollectorSap
    End Sub

    Public Function getSwitchesList(salesOrg As String, id As String) As List(Of SwitchesProperty)
        Dim ZVList As List(Of ZV04IProperty) = getZvList(salesOrg)
        If ZVList Is Nothing Then Return Nothing
        Dim cdList As List(Of CustomerDataProperty) = GetCdList(salesOrg)
        Dim sdList As List(Of SwitchesDataProperty) = getSdList(salesOrg)

        Dim allSoldToList As List(Of SwitchesProperty) = GetLinq(id, sdList, ZVList, cdList, isSpecifficSoldTo:=False)
        Dim SpecifficSoldToList As List(Of SwitchesProperty) = GetLinq(id, sdList, ZVList, cdList, isSpecifficSoldTo:=True)
        Dim finalList As New List(Of SwitchesProperty)

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

    Private Function getFinalListWithStockDetails(list As List(Of SwitchesProperty), salesOrg As String) As List(Of SwitchesProperty)
        Dim sap As ISAPLib = Create.sapLib
        Dim co09 As New CO09(sap)

        Dim listOfUniqueOldSkus As List(Of Long) = (From x In list Select x.oldSku).Distinct.ToList
        Dim listOfUniqueNewSkus As List(Of Long) = (From x In list Select x.newSku).Distinct.ToList

        Dim co09ObjOldSkuList As New List(Of CO09Property)
        Dim co09ObjNewSkuList As New List(Of CO09Property)

        For Each item In listOfUniqueOldSkus
            co09ObjOldSkuList.Add(co09.getStockDetails(item, salesOrg))
        Next

        For Each item In listOfUniqueNewSkus
            co09ObjNewSkuList.Add(co09.getStockDetails(item, salesOrg))
        Next

        'assigns value to final list passed to func from both old and new sku CO09 lists
        For Each listItem In list
            For Each co09OldItem In co09ObjOldSkuList
                If listItem.oldSku = co09OldItem.sku Then
                    listItem.oldSkuATP = co09OldItem.ATP
                    listItem.oldSkuRecoveryDate = co09OldItem.recoveryDate
                    listItem.oldSkuRecoveryQty = co09OldItem.recoveryQty
                    Exit For
                End If
            Next

            For Each co09Newtem In co09ObjNewSkuList
                If listItem.newSku = co09Newtem.sku Then
                    listItem.newSkuATP = co09Newtem.ATP
                    listItem.newSkuRecoveryDate = co09Newtem.recoveryDate
                    listItem.newSkuRecoveryQty = co09Newtem.recoveryQty
                    Exit For
                End If
            Next
        Next

        Return list
    End Function

    Private Function GetLinq(id As String, sdList As List(Of SwitchesDataProperty), zvList As List(Of ZV04IProperty), cdList As List(Of CustomerDataProperty), isSpecifficSoldTo As Boolean) As List(Of SwitchesProperty)
        Return (From zv In zvList
                Join cd In cdList
                    On zv.soldTo Equals cd.soldTo And zv.shipTo Equals cd.shipTo
                Join sd In sdList
                   On zv.material Equals sd.oldSku And cd.country.ToLower Equals sd.country.ToLower
                Where getStockConditon(sd, zv) AndAlso getSoldToCondition(isSpecifficSoldTo, zv, sd)
                Select getSwitchPropertyObjs(zv, sd, cd, id)).ToList()
    End Function

    Private Shared Function getSoldToCondition(isSpecifficSoldTo As Boolean, zv As ZV04IProperty, sd As SwitchesDataProperty) As Boolean
        Return If(isSpecifficSoldTo, sd.soldTo = zv.soldTo, sd.soldTo = 0)
    End Function

    Private Function getSdList(salesOrg As String) As List(Of SwitchesDataProperty)
        Return dataCollectorServer.getSwitchesDataList(salesOrg).Where(Function(x) Date.Today >= x.startDate AndAlso Date.Today <= x.endDate).ToList
    End Function

    Private Function getZvList(salesOrg As String) As List(Of ZV04IProperty)
        Dim list As List(Of ZV04IProperty) = dataCollectorSap.getZV04IList(salesOrg, IDAEnum.Task.switches)
        If list Is Nothing Then
            Return Nothing
        Else
            Return list.Where(Function(x) x.delBlock <> IDAConsts.bypassOrderChangesBlock AndAlso x.delBlock <> "Z4" AndAlso x.delBlock <> "04" AndAlso x.rejReason = "").ToList
        End If
    End Function

    Private Function getSwitchPropertyObjs(zv As ZV04IProperty, sd As SwitchesDataProperty, cd As CustomerDataProperty, id As String) As SwitchesProperty
        Return New SwitchesProperty(cd.salesOrg, cd.country, zv.soldTo, If(sd.soldTo = 0, "All", "Speciffic"), zv.shipTo, zv.order, zv.shipToName, zv.item, zv.orderQty, zv.confirmedQty, sd.oldSku, sd.oldSkuCaseBarcode, sd.oldSkuUnitBarcode, Nothing, Nothing, Nothing, sd.newSku, sd.newSkuCaseBarcode, sd.newSkuUnitBarcode, Nothing, Nothing, Nothing, sd.startDate, sd.endDate, sd.needOutOfStockToSwitch, sd.switchAutomatic, sd.switchComment, id)
    End Function

    Private Function getStockConditon(sd As SwitchesDataProperty, zv As ZV04IProperty) As Boolean
        Return (sd.needOutOfStockToSwitch = True AndAlso zv.confirmedQty < zv.orderQty) OrElse sd.needOutOfStockToSwitch = False
    End Function
End Class
