Imports IDAUtil
Imports [lib]

Public Class ConversionDAO

    Private ReadOnly dbServer As IDBServerConnector

    Public Sub New(dbServer As IDBServerConnector)
        Me.dbServer = dbServer
    End Sub

    Public Function getQuantityConversionMaterialList() As List(Of ConversionMaterial)
        dbServer.createConnectionToServerIniCF()
        Dim conversionMaterialList As List(Of ConversionMaterial)
        conversionMaterialList = dbServer.getObjectListByQuery(Of ConversionMaterial)("SELECT * FROM QuantityConversionMaterial")
        dbServer.closeConnection()
        Return conversionMaterialList
    End Function

    Public Function getQuantityConversionShipToList() As List(Of ConversionShiptTo)
        dbServer.createConnectionToServerIniCF()
        Dim conversionShipToList As List(Of ConversionShiptTo)
        conversionShipToList = dbServer.getObjectListByQuery(Of ConversionShiptTo)("SELECT * FROM QuantityConversionShipTo")
        dbServer.closeConnection()
        Return conversionShipToList
    End Function

    Public Function getQuantityConversionLogList(fromDocument As Long, toDocument As Long) As List(Of ConversionLog)
        dbServer.createConnectionToServerIniCF()
        Dim conversionLogList As List(Of ConversionLog)
        conversionLogList = dbServer.getObjectListByQuery(Of ConversionLog)($"SELECT * FROM QuantityConversionLog WHERE orderNumber >= {fromDocument} AND orderNumber <= {toDocument}")
        dbServer.closeConnection()
        Return conversionLogList
    End Function
End Class
