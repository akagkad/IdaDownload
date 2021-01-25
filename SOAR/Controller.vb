Imports IDAUtil
Imports [lib]

Public Module Controller

    Private sapLib As ISAPLib = Create.sapLib

    Public Sub executeWE05(messageVariant As String)

        Dim folderPath As String = $"\\Gbfrimpf000\common\SOAR\OTD\DOMESTIC SOAR\{messageVariant}{If(messageVariant = "KE", "02", "01")}\WE05"

        Dim winUtil As IWinUtil
        Dim we05 As WE05

        winUtil = Create.winUtil
        we05 = New WE05(sapLib, winUtil)

        Dim respone As WE05.ResponseWE05 = we05.extractReport(messageVariant, folderPath)

        If respone = WE05.ResponseWE05.successTable Then
            Using xlUtil = Create.xlUtil
                xlUtil.closeWBLikeAnyInstanceWaitTillClose("*WE05.xlsx")
            End Using
        End If

    End Sub

    Public Sub executeZV04HN(salesOrg As String)

        Dim folderPath As String = $"\\Gbfrimpf000\common\SOAR\OTD\DOMESTIC SOAR\{salesOrg}\ZV04HN"
        Dim fileName As String = Now().ToFileNameFormat & " ZV04HN.xlsx"

        Dim zv04hn = New ZV04HN(sapLib, salesOrg, IDAEnum.Task.SOAR)
        zv04hn.setParamsBeforeExecution()
        zv04hn.exportExcel(folderPath, fileName)

        Using xlUtil = Create.xlUtil
            xlUtil.closeWBAnyInstanceWaitTillClose(fileName)
        End Using

    End Sub

End Module
