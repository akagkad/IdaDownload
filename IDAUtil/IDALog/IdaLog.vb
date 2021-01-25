Imports [lib]

Public Class IdaLog

    Dim db As IDBServerConnector = Create.dbServer

    Public Sub New()
        db.createConnectionToServerIniCF()
    End Sub

    Protected Overrides Sub Finalize()
        db.closeConnection()
    End Sub

    Public Sub insertToActivityLog(taskName As String, status As String, id As String, salesOrg As String)
        db.createConnectionToServerIniCF()
        Dim rs As ADODB.Recordset
        rs = db.executeQuery($"INSERT INTO ActivityLog ([id],[taskName],[salesOrg],[taskStatus],[updateTime],[pcName]) VALUES ('{id}','{taskName}','{salesOrg}','{status}','{DateTime.Now.ToDBFormat}','{Environment.MachineName}')")
    End Sub

    Public Sub populateFromRs(rs As ADODB.Recordset)
        db.rsToServer(rs, "DeliveryDatesLog")
    End Sub

    Public Sub update(tableName As String, columnNames As String(), values As String(), conditionName As String(), conditionValue As String(), Optional customCondition As String = "")
        Dim db As IDBServerConnector = Create.dbServer
        Dim columnsAndValuesStr As String = ""
        Dim conditionStr As String = ""

        db.createConnectionToServerIniCF()

        For i = 0 To columnNames.Count - 1
            columnsAndValuesStr &= $"[{columnNames(i)}] = '{values(i)}'"
            If Not i = columnNames.Count - 1 Then columnsAndValuesStr &= ", "
        Next i

        For i = 0 To conditionName.Count - 1
            conditionStr &= $"{conditionName(i)} = {IIf(IsNumeric(conditionValue(i)), conditionValue(i), "'" & conditionValue(i) & "'") }"
            If Not i = conditionName.Count - 1 Then conditionStr &= " AND "
        Next i

        If customCondition <> "" Then
            customCondition = " AND " & customCondition
            conditionStr &= customCondition
        End If

        Dim query As String
        query = $"UPDATE {tableName} SET {columnsAndValuesStr} WHERE {conditionStr}"
        db.executeQuery(query)

        db.closeConnection()
    End Sub

    Public Function insert(tableName As String, columnNames As String, values As String) As Boolean
        Try
            Dim query As String
            query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({values})"
            db.executeQuery(query)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class
