Imports [lib]

Public Class DistributionListCalculator

    Private ReadOnly dbServer As IDBServerConnector

    Public Sub New(dbServer As IDBServerConnector)
        Me.dbServer = dbServer
        dbServer.createConnectionToServerIniCF()
    End Sub

    Protected Overrides Sub Finalize()
        dbServer.closeConnection()
        MyBase.Finalize()
    End Sub

    Public Function getDistList(salesOrg As String, task As String) As String
        Dim list As List(Of DistributionListProperty) = dbServer.getObjectListByQuery(Of DistributionListProperty)($"SELECT [address] FROM DistributionListData WHERE salesOrg = '{salesOrg}' AND [name] = '{task}'")
        Dim listArray As String() = list.Select(Function(every) every.address).ToArray()

        Return Join(listArray, ";")
    End Function
End Class
