Imports IDAUtil

Module App

    Public Sub Main(args As String())
        Dim salesOrg As String = args(0)
        Dim isRelease As Boolean = args(1)

        Try
            executeRejectionsDuringReleaseTask(salesOrg, isRelease)
        Catch ex As Exception
            Dim gle As New GlobalErrorHandler
            gle.handle(salesOrg, If(isRelease, "Release Rejections", "After Release Rejections"), ex)
        End Try

    End Sub

End Module
