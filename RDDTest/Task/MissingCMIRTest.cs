
// Imports IDAUtil
// Imports [lib]
// Imports MissingCMIRReport

// <TestClass()>
// Public Class MissingCMIRTest

// <TestMethod()>
// Public Sub TestMethod1()
// Dim PList As List(Of ZV04PProperty) = getZV04P()
// Dim customerDataList As List(Of CustomerDataProperty) = getCM()
// Dim query As List(Of MissingCMIRProperty) =
// (From p In PList
// Join cm In customerDataList
// On p.soldTo Equals cm.soldTo And p.shipTo Equals cm.shipTo
// Where (p.custMatNumb Is Nothing OrElse p.custMatNumb = "") AndAlso cm.cmirCheckAllowed
// Select New MissingCMIRProperty(p.material, p.materialDescription, p.soldTo, p.soldToName, p.order, p.item)).ToList

// Dim expectedCount As Integer = 2
// Assert.AreEqual(expectedCount, query.Count)
// End Sub

// Private Function getCM() As List(Of CustomerDataProperty)
// Dim list As New List(Of CustomerDataProperty) From {
// New CustomerDataProperty() With {
// .soldTo = 1,
// .shipTo = 2,
// .cmirCheckAllowed = True
// }
// }

// list.Add(New CustomerDataProperty() With {
// .soldTo = 55,
// .shipTo = 65,
// .cmirCheckAllowed = False
// })

// list.Add(New CustomerDataProperty() With {
// .soldTo = 33,
// .shipTo = 34,
// .cmirCheckAllowed = True
// })
// Return list
// End Function

// Private Function getZV04P() As List(Of ZV04PProperty)
// Dim list As New List(Of ZV04PProperty) From {
// New ZV04PProperty() With {
// .soldTo = 1,
// .shipTo = 2
// }
// }

// list.Add(New ZV04PProperty() With {
// .soldTo = 55,
// .shipTo = 65
// })

// list.Add(New ZV04PProperty() With {
// .soldTo = 33,
// .shipTo = 34,
// .custMatNumb = ""
// })
// Return list
// End Function


// <TestMethod()>
// Public Sub TestMethod2()
// Initializer.modelFrom_CF_ServerToClipboard("CustomerData")
// End Sub

// End Class


// Imports IDAUtil
// Imports [lib]
// Imports MissingCMIRReport

// <TestClass()>
// Public Class MissingCMIRTest

// <TestMethod()>
// Public Sub TestMethod1()
// Dim PList As List(Of ZV04PProperty) = getZV04P()
// Dim customerDataList As List(Of CustomerDataProperty) = getCM()
// Dim query As List(Of MissingCMIRProperty) =
// (From p In PList
// Join cm In customerDataList
// On p.soldTo Equals cm.soldTo And p.shipTo Equals cm.shipTo
// Where (p.custMatNumb Is Nothing OrElse p.custMatNumb = "") AndAlso cm.cmirCheckAllowed
// Select New MissingCMIRProperty(p.material, p.materialDescription, p.soldTo, p.soldToName, p.order, p.item)).ToList

// Dim expectedCount As Integer = 2
// Assert.AreEqual(expectedCount, query.Count)
// End Sub

// Private Function getCM() As List(Of CustomerDataProperty)
// Dim list As New List(Of CustomerDataProperty) From {
// New CustomerDataProperty() With {
// .soldTo = 1,
// .shipTo = 2,
// .cmirCheckAllowed = True
// }
// }

// list.Add(New CustomerDataProperty() With {
// .soldTo = 55,
// .shipTo = 65,
// .cmirCheckAllowed = False
// })

// list.Add(New CustomerDataProperty() With {
// .soldTo = 33,
// .shipTo = 34,
// .cmirCheckAllowed = True
// })
// Return list
// End Function

// Private Function getZV04P() As List(Of ZV04PProperty)
// Dim list As New List(Of ZV04PProperty) From {
// New ZV04PProperty() With {
// .soldTo = 1,
// .shipTo = 2
// }
// }

// list.Add(New ZV04PProperty() With {
// .soldTo = 55,
// .shipTo = 65
// })

// list.Add(New ZV04PProperty() With {
// .soldTo = 33,
// .shipTo = 34,
// .custMatNumb = ""
// })
// Return list
// End Function


// <TestMethod()>
// Public Sub TestMethod2()
// Initializer.modelFrom_CF_ServerToClipboard("CustomerData")
// End Sub

// End Class

