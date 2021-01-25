Imports IDAUtil
Imports [lib]

Module DataCollectorFactory

    Public Function createDataCollectorSAP(salesOrg As String) As DataCollectorSAP
        Return New DataCollectorSAP(
            New ZV04I(Create.sapLib,
                      salesOrg,
                      IDAEnum.Task.quantityConversion),
            Create.exportParses)
    End Function

End Module
