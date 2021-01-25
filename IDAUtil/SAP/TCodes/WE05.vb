Imports [lib]

Public Class WE05

    Private ReadOnly sapLib As ISAPLib
    Private ReadOnly winUtil As IWinUtil

    Public Sub New(sapLib As ISAPLib, winUtil As IWinUtil)
        Me.sapLib = sapLib
        Me.winUtil = winUtil
    End Sub

    Public Enum ResponseWE05
        noItems
        successSingleValue
        successTable
    End Enum

    Public Function extractReport(messageVariant As String, folderPath As String) As ResponseWE05

        '2 - inbound idocs only
        '51 - Application not posted (Hardstop workflow)
        executeWE05(2, 51, messageVariant)

        Dim screenShotPath As String
        Dim fileName As String
        screenShotPath = folderPath & "\" & Now().ToFileNameFormat & " WE05.jpg"
        fileName = Now().ToFileNameFormat & " WE05.xlsx"

        If sapLib.isPopUp() Then
            executeWE05("", "", messageVariant)
            If sapLib.isPopUp() Then
                sapLib.printScreenOfCurrentSession(screenShotPath)
                Return ResponseWE05.noItems
            End If
        End If

        If isTableExists() Then
            sapLib.findById(SAP_ALL_ID.WE05.IDOCS_NODE_TREE).selectedNode("IDoc")

            setMessageTypeFilter("ORDERS")
            'if no orders in the list then export at least sth
            If isTableEmpty() Then
                setMessageTypeFilter("")
            End If

            sapLib.openExport()
            sapLib.exportExcel(folderPath, fileName)
            sapLib.printScreenOfCurrentSession(screenShotPath)
            Return ResponseWE05.successTable
        Else
            screenShotPath = folderPath & "\" & Now().ToFileNameFormat & " single item only found WE05.jpg"
            Try
                sapLib.printScreenOfCurrentSession(screenShotPath)
            Catch ex As Exception
                Threading.Thread.Sleep(3000)
                sapLib.printScreenOfCurrentSession(screenShotPath)
            End Try

            Return ResponseWE05.successSingleValue
        End If

    End Function

    Private Sub executeWE05(direction As String, status As String, messageVariant As String)
        Dim dateFrom As String = DateAdd(DateInterval.Day, -90, Now()).ToSAPFormat
        Dim dateTo As String = Now().ToSAPFormat

        sapLib.enterTCode("WE05")

        sapLib.setText(SAP_ALL_ID.WE05.MESSAGE_VARIAN_FLD, messageVariant)

        If messageVariant = "FR" Then
            sapLib.setMultipleSelection({"BE"}, SAP_ALL_ID.WE05.MESSAGE_VARIANT_MULTIPLE_SELECTION_BTN)
        End If

        sapLib.setText(SAP_ALL_ID.WE05.CREATED_ON_FROM_FLD, dateFrom)
        sapLib.setText(SAP_ALL_ID.WE05.CREATED_ON_TO_FLD, dateTo)

        'sapLib.setText(SAP_ALL_ID.WE05.IDOC_NUMBER_FLD, "185197867")

        sapLib.setText(SAP_ALL_ID.WE05.DIRECTION_FLD, direction)
        sapLib.setText(SAP_ALL_ID.WE05.CURRENT_STATUS_FLD, status)
        sapLib.pressF8()

    End Sub

    Private Function setMessageTypeFilter(msgType As String) As Boolean
        sapLib.findById(SAP_ID.OPEN_EXPORT_CONTEXT_MENU_BTN_ARR(0)).selectColumn("MESTYP") ' Message Type
        sapLib.findById(SAP_ID.OPEN_EXPORT_CONTEXT_MENU_BTN_ARR(0)).pressToolbarButton("&MB_FILTER")
        sapLib.setText(SAP_ID.TABLE_FILTER_FIRST_VALUE_FLD, msgType)
        sapLib.pressEnter()
        Return True
    End Function

    Private Function isTableExists() As Boolean
        Return sapLib.idExists(SAP_ALL_ID.WE05.SELECTED_IDOCS_TABLE)
    End Function

    Private Function isTableEmpty() As Boolean
        Return sapLib.findById(SAP_ALL_ID.WE05.SELECTED_IDOCS_TABLE).RowCount = 0
    End Function


End Class
