// Imports IDAUtil
// Imports MissingCustomerReport
// Imports [lib]

// <TestClass()>
// Public Class CustomerMissingTest

// <TestMethod()>
// Public Sub CustomerMissing_ShouldReturn_OneElement_WhenDuplicateValuesFound()
// Dim zv As List(Of ZV04HNProperty) = getZVList()
// Dim cm As List(Of CustomerDataProperty) = getCMList()

// Dim query As List(Of MissingCustomersProperty) =
// (From cml In (zv.Where(Function(x) cm.All(Function(y) y.soldTo <> x.soldto AndAlso y.shipTo <> x.shipto)).ToList)
// Select New MissingCustomersProperty(cml.order, cml.soldto, cml.soldtoName, cml.shipto, cml.shiptoName)).Distinct(Function(x, y) x.soldto = y.soldto AndAlso x.shipto = y.shipto).ToList

// Assert.IsTrue(query.Count = 2)
// End Sub

// Private Function getZVList() As List(Of ZV04HNProperty)
// Dim list As New List(Of ZV04HNProperty)

// list.Add(New ZV04HNProperty() With {
// .soldto = 456,
// .soldtoName = "c",
// .shipto = 654,
// .shiptoName = "d"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 456,
// .soldtoName = "c",
// .shipto = 654,
// .shiptoName = "d"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 123,
// .soldtoName = "a",
// .shipto = 321,
// .shiptoName = "b"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 123,
// .soldtoName = "a",
// .shipto = 321,
// .shiptoName = "b"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 55,
// .soldtoName = "a",
// .shipto = 5523,
// .shiptoName = "b"
// })
// list.Add(New ZV04HNProperty() With {
// .soldto = 55,
// .soldtoName = "a",
// .shipto = 5523,
// .shiptoName = "b"
// })

// Return list
// End Function

// Private Function getCMList() As List(Of CustomerDataProperty)
// Dim list As New List(Of CustomerDataProperty)
// list.Add(New CustomerDataProperty() With {
// .soldTo = 456,
// .soldToName = "c",
// .shipTo = 654,
// .shipToName = "d"
// })

// list.Add(New CustomerDataProperty() With {
// .soldTo = 456,
// .soldToName = "c",
// .shipTo = 654,
// .shipToName = "d"
// })

// Return list
// End Function
// End Class
// Imports IDAUtil
// Imports MissingCustomerReport
// Imports [lib]

// <TestClass()>
// Public Class CustomerMissingTest

// <TestMethod()>
// Public Sub CustomerMissing_ShouldReturn_OneElement_WhenDuplicateValuesFound()
// Dim zv As List(Of ZV04HNProperty) = getZVList()
// Dim cm As List(Of CustomerDataProperty) = getCMList()

// Dim query As List(Of MissingCustomersProperty) =
// (From cml In (zv.Where(Function(x) cm.All(Function(y) y.soldTo <> x.soldto AndAlso y.shipTo <> x.shipto)).ToList)
// Select New MissingCustomersProperty(cml.order, cml.soldto, cml.soldtoName, cml.shipto, cml.shiptoName)).Distinct(Function(x, y) x.soldto = y.soldto AndAlso x.shipto = y.shipto).ToList

// Assert.IsTrue(query.Count = 2)
// End Sub

// Private Function getZVList() As List(Of ZV04HNProperty)
// Dim list As New List(Of ZV04HNProperty)

// list.Add(New ZV04HNProperty() With {
// .soldto = 456,
// .soldtoName = "c",
// .shipto = 654,
// .shiptoName = "d"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 456,
// .soldtoName = "c",
// .shipto = 654,
// .shiptoName = "d"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 123,
// .soldtoName = "a",
// .shipto = 321,
// .shiptoName = "b"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 123,
// .soldtoName = "a",
// .shipto = 321,
// .shiptoName = "b"
// })

// list.Add(New ZV04HNProperty() With {
// .soldto = 55,
// .soldtoName = "a",
// .shipto = 5523,
// .shiptoName = "b"
// })
// list.Add(New ZV04HNProperty() With {
// .soldto = 55,
// .soldtoName = "a",
// .shipto = 5523,
// .shiptoName = "b"
// })

// Return list
// End Function

// Private Function getCMList() As List(Of CustomerDataProperty)
// Dim list As New List(Of CustomerDataProperty)
// list.Add(New CustomerDataProperty() With {
// .soldTo = 456,
// .soldToName = "c",
// .shipTo = 654,
// .shipToName = "d"
// })

// list.Add(New CustomerDataProperty() With {
// .soldTo = 456,
// .soldToName = "c",
// .shipTo = 654,
// .shipToName = "d"
// })

// Return list
// End Function
// End Class
