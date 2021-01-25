'Imports [lib]
'Imports IDAUtil
'Imports Moq
'Imports Switches

'<TestClass()> Public Class SwitchesTest
'    'TODO: doesnt work without customer data
'    <TestMethod()> Public Sub executeSwitches()

'        Dim dcServer As New Mock(Of IDataCollectorServer)
'        Dim dcSap As New Mock(Of IDataCollectorSap)

'        dcServer.Setup(Function(x) x.getSwitchesDataList(It.IsAny(Of String))).Returns(getSwitchesData)
'        dcSap.Setup(Function(x) x.getZV04IList(It.IsAny(Of String), It.IsAny(Of IDAEnum.Task))).Returns(getZV04IData)

'        Dim calculator As New DataCollectorServiceSwitches(dcServer.Object, dcSap.Object)

'        Dim firstSoldTo As Int64 = 123
'        Dim secondSoldTo As Int64 = 456
'        Dim firstSku As Int64 = 66
'        Dim secondSku As Int64 = 77

'        'testing correct soldTos are assigned to switches
'        Assert.AreEqual(firstSoldTo, calculator.getSwitchesList("", "").First.soldTo)
'        Assert.AreEqual(secondSoldTo, calculator.getSwitchesList("", "").Last.soldTo)

'        'testing soldTos get correct switches when confirmed qty >= order qty and between start and end dates
'        Assert.AreEqual(firstSku, calculator.getSwitchesList("", "").First.newSku)
'        Assert.AreEqual(secondSku, calculator.getSwitchesList("", "").Last.newSku)

'        'testing soldTos get correct isAutomatic
'        Assert.IsTrue(calculator.getSwitchesList("", "").First.switchAutomatic)
'        Assert.IsFalse(calculator.getSwitchesList("", "").Last.switchAutomatic)

'        'testing soldTos get correct need out of stock to switch
'        Assert.IsTrue(calculator.getSwitchesList("", "").First.needOutOfStockToSwitch)
'        Assert.IsFalse(calculator.getSwitchesList("", "").Last.needOutOfStockToSwitch)
'    End Sub

'    Private Function getSwitchesData() As List(Of SwitchesDataProperty)
'        Dim list As New List(Of SwitchesDataProperty) From {
'            New SwitchesDataProperty() With {
'                    .soldTo = 123,
'                    .salesOrg = "ZA01",
'                    .country = "b",
'                    .oldSku = 55,
'                    .newSku = 66,
'                    .startDate = #04/01/2020#,
'                    .endDate = #05/01/2020#,
'                    .switchAutomatic = True,
'                    .needOutOfStockToSwitch = True
'                 }
'        }

'        list.Add(New SwitchesDataProperty() With {
'                    .soldTo = 0,
'                    .salesOrg = "ZA01",
'                    .country = "a",
'                    .startDate = #04/01/2020#,
'                    .endDate = #05/01/2020#,
'                    .oldSku = 55,
'                    .newSku = 77,
'                    .switchAutomatic = False,
'                    .needOutOfStockToSwitch = False
'                    }
'                )

'        Return list
'    End Function

'    Private Function getZV04IData() As List(Of ZV04IProperty)
'        Dim list As New List(Of ZV04IProperty) From {
'            New ZV04IProperty() With {
'                    .soldTo = 123,
'                    .material = 55,
'                    .confirmedQty = 10,
'                    .orderQty = 20
'                 }
'        }

'        list.Add(New ZV04IProperty With {
'                    .soldTo = 456,
'                    .material = 55,
'                    .confirmedQty = 10,
'                    .orderQty = 20
'                    }
'                )

'        Return list
'    End Function

'    Private Function getCustomerData() As List(Of CustomerDataProperty)
'        Dim list As New List(Of CustomerDataProperty) From {
'            New CustomerDataProperty() With {
'                    .soldTo = 123,
'                    .country = "a"
'                 }
'        }

'        list.Add(New CustomerDataProperty With {
'                    .soldTo = 456,
'                    .country = "b"
'                    }
'                )

'        Return list
'    End Function

'    <TestMethod()> Public Sub getSwitchesDataModel()
'        Initializer.modelFrom_CF_ServerToClipboard("select top 0 * from rejectionsData")
'    End Sub
'End Class
