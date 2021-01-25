'Imports System.IO
'Imports IDAUtil
'Imports [lib]
'Imports Moq

'<TestClass()>
'Public Class WE05RunnerTest

'    Private sapLib As ISAPLib

'    <TestInitialize>
'    Public Sub init()
'        Me.sapLib = Create.sapLib
'    End Sub

'    <TestCleanup>
'    Public Sub cleanUp()
'        sapLib = Nothing
'    End Sub

'    <TestMethod>
'    Public Sub someTest()
'        Dim q As Func(Of String, Boolean) = Function(x As String) ""
'        Dim we05 As New WE05(sapLib, New Mock(Of IWinUtil)().Object)
'        Try
'            we05.extractReport("IT", Paths.DESKTOP_ONE_DRIVE & "\New folder")
'        Catch ex As Exception

'        End Try

'    End Sub

'End Class