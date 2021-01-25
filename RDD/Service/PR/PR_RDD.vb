Imports RDD

Public Class PR_RDD
    Public Sub GetCalculatedRDDList()
        Dim rddList As List(Of RddOutputBeanGeneric) = getRddList()
        Dim rddcalc As New RddDataCalculator(getBankHolidayList(), True)

        Dim i As Integer = 0
        For i = 0 To rddList.Count - 1

        Next i
    End Sub

    Private Function getBankHolidayList() As List(Of Date)
        Throw New NotImplementedException()
    End Function

    Private Function getRddList() As List(Of RddOutputBeanGeneric)
        Dim list As New List(Of CalculatedRddOutputBean)


    End Function
End Class
