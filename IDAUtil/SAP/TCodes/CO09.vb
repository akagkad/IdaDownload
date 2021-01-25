Imports [lib]

Public Class CO09
    Private ReadOnly sap As ISAPLib

    Public Sub New(saplib As ISAPLib)
        sap = saplib
    End Sub

    Public Function getStockDetails(sku As Long, salesOrg As String) As CO09Property

        Dim plant As Long = New SalesOrgDetails().getPlant(salesOrg)

        sap.enterTCode("CO09")

        Dim isOpenTable As Boolean = isOpenStockAndDates(sku, plant)

        If Not isOpenTable Then
            Return New CO09Property With {
                                   .salesOrg = salesOrg,
                                   .plant = plant,
                                   .sku = sku,
                                   .ATP = 0,
                                   .recoveryQty = 0,
                                   .recoveryDate = "SKU does not exists for this plant"
            }
        End If

        Dim table = sap.getITableObject
        Dim atp As Long = table.getCellValue(0, 4)

        Dim recDate As String = ""
        Dim recQty As Long = 0

        Dim i As Integer = 0
        'had to do this due to a table bug

        Threading.Thread.Sleep(1000)
        sap.findById("wnd[0]").resizeWorkingPane(100, 25, 0)
        Threading.Thread.Sleep(1000)

        Try
            While table.getCellValue(i, 1) <> ""

                If {"POitem", "PurRqs", "ShpgNt "}.Contains(table.getCellValue(i, 1)) Then
                    recDate = table.getCellValue(i, 0)
                    recQty = table.getCellValue(i, 3)
                    Exit While
                End If

                i += 1
            End While
        Catch ex As TableWasNotScrolledException
        End Try

        Return New CO09Property With {
                                    .salesOrg = salesOrg,
                                    .plant = plant,
                                    .sku = sku,
                                    .ATP = atp,
                                    .recoveryQty = recQty,
                                    .recoveryDate = If(recDate = "", "No Date Avalable", recDate)
        }

    End Function

    Private Function isOpenStockAndDates(sku As Long, plant As String) As Boolean
        sap.setText(CO09ID.SKU_INPUT_TEXT_FIELD_ID, sku)
        sap.setText(CO09ID.PLANT_INPUT_TEXT_FIELD_ID, plant)
        sap.pressEnter()

        If sap.getInfoBarMsg().Contains("Text for MRP") Then
            sap.pressEnter()
        End If

        If sap.getInfoBarMsg().Contains("not found") Then
            Return False
        End If

        Return True
    End Function

End Class
