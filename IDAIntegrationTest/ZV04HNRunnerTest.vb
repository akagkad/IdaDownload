'Imports System.Windows.Forms
'Imports IDAUtil
'Imports [lib]


'<TestClass()>
'Public Class ZV04HNRunnerTest

'    <TestMethod>
'    Public Sub ZV04HN_Should_returnClipboard_When_allGood()
'        Dim sap As ISAPLib = Create.sapLib
'        Dim ZV04HN As New ZV04HN(sap, "ZA01", IDAEnum.Task.rdd)

'        ZV04HN.setParamsBeforeExecution()
'        Clipboard.Clear()
'        ZV04HN.extractReport()

'        Assert.IsTrue(Clipboard.ContainsText)

'        Dim ep As New ExportParser(New ParseUtil, New AttributeUtil, Create.winUtil)
'        Dim list As List(Of ZV04HNProperty) = ep.getObjectListFromClipboard(Of ZV04HNProperty)()

'    End Sub

'    <TestMethod>
'    Public Sub someMethod_Should_someBehaviour_When_someState()
'        modelFromExcelToClipboard(Paths.DESKTOP & "\export.xlsx")
'    End Sub

'End Class
