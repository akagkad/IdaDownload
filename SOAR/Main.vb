Imports IDAUtil

Public Module Main

    Sub Main(args As String())
        Dim soarReport As String = "All" 'args(0)  always runs for all anyway so no point to have individual at the moment
        Dim SOs As String() =
                            {"FR01",
                             "NL01",
                             "DE01",
                             "IT01",
                             "GR01",
                             "PL01",
                             "CZ01",
                             "PT01",
                             "ES01",
                             "RO01",
                             "GB01",
                             "TR01",
                             "UA01",
                             "RU01",
                             "RU01",
                             "ZA01",
                             "KE02",
                             "NG01"}

        Select Case soarReport
            Case "WE05"
                Controller.executeWE05(Left(args(1), 2))
            Case "ZV04HN"
                Controller.executeZV04HN(args(1))
            Case "All"
                runAll(SOs)
            Case Else
                Throw New NotImplementedException("no report for " & soarReport)
        End Select
    End Sub

    Private Sub runAll(SOs() As String)
        If Weekday(Date.Today, FirstDayOfWeek.Monday) <> 7 Then
            For Each so In SOs
                Try
                    Controller.executeWE05(Left(so, 2))
                    Controller.executeZV04HN(so)
                Catch ex As Exception
                    Dim gle As New GlobalErrorHandler
                    gle.handle(so, "SOAR", ex)
                End Try
            Next
        End If

        Try
            Controller.executeZV04HN("EG01")
            Controller.executeZV04HN("EG02")
            Controller.executeZV04HN("SA01")
        Catch ex As Exception
            Dim gle As New GlobalErrorHandler
            gle.handle("EGandSA", "SOAR", ex)
        End Try
    End Sub
End Module
