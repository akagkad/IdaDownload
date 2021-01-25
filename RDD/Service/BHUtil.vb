Public Module BHUtil

    Public Function isSkipWeekend(salesOrg As String) As Boolean

        Select Case salesOrg
            Case "RU01", "UA01"
                Return False
            Case Else
                Return True
        End Select

    End Function

End Module
