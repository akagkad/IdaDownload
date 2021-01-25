Public Class SalesOrgDetails
    Public Sub New()
    End Sub

    Public Function getDistChan(salesOrg As String) As Long
        Dim distChantDict = New Dictionary(Of String, Long) From {{"CZ01", 78}, {"DE01", 67}, {"ES01", 73}, {"FR01", 71}, {"GB01", 66}, {"GR01", 81}, {"IT01", 72}, {"KE02", 12}, {"NG01", 11}, {"NL01", 70}, {"PL01", 75}, {"PT01", 74}, {"RO01", 80}, {"RU01", 86}, {"SE01", 83}, {"TR01", 87}, {"UA01", 85}, {"ZA01", 76}, {"CH06", 98}, {"EG01", 14}, {"EG02", 15}, {"SA01", 16}, {"GH01", 13}}

        Return distChantDict(salesOrg)
    End Function

    Public Function getPlant(salesOrg As String) As Long
        Dim plantDict = New Dictionary(Of String, Long) From {{"CZ01", 6610}, {"DE01", 6530}, {"ES01", 6560}, {"FR01", 6520}, {"GB01", 6540}, {"GR01", 6650}, {"IT01", 6550}, {"KE02", 7500}, {"NG01", 7400}, {"NL01", 6530}, {"PL01", 6610}, {"PT01", 6560}, {"RO01", 6640}, {"RU01", 7010}, {"SE01", 6670}, {"TR01", 7050}, {"UA01", 6910}, {"ZA01", 7110}, {"CH06", 6110}, {"EG01", 7305}, {"EG02", 7315}, {"SA01", 7200}, {"GH01", 7510}}

        Return plantDict(salesOrg)
    End Function
End Class

