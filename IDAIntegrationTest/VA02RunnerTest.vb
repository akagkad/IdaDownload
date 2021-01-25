Imports IDAUtil
Imports IDAUtil.SAP.TCode.Runners
Imports [lib]
Imports Switches

<TestClass()> Public Class VA02RunnerTest

    <TestMethod>
    Public Sub runSwitches_Should_NotFail_When_ConnectedToMultiSapSessions()
        Dim executor As New SwitchesTaskExecutor("", "")
        Dim list As List(Of SwitchesSapOrderProperty) = getSwitchesSapOrderList()
        Dim timer = Create.timeCount()
        timer.start()
        executor.automaticSwitchObjectList = list
        executor.runSwitchesInVA02()
        Dim vremya = timer.finish()
        Dim mu As IMailUtil = Create.mailUtil
        'executor.sendReplacedCMIRs("Some Sales Org", "drybak@scj.com", mu)
    End Sub

    Private Shared Function getSwitchesSapOrderList() As List(Of SwitchesSapOrderProperty)
        Dim list As New List(Of SwitchesSapOrderProperty) From {
            New SwitchesSapOrderProperty() With {
                .orderNumber = 206523629,
                .lineDetails = New List(Of SwitchesSapLineProperty) From {
                    New SwitchesSapLineProperty With {
                    .isSameBarcode = True,
                    .lineNumber = 10,
                    .oldSku = 683471,
                    .newSku = 671832,
                    .reason = "huison"
                    },
                    New SwitchesSapLineProperty With {
                    .isSameBarcode = True,
                    .lineNumber = 20,
                    .oldSku = 696759,
                    .newSku = 302530,
                    .reason = "huison"
                    },
                    New SwitchesSapLineProperty With {
                    .isSameBarcode = True,
                    .lineNumber = 30,
                    .oldSku = 659079,
                    .newSku = 301994,
                    .reason = "huison"
                    }
                }
            },
            New SwitchesSapOrderProperty() With {
            .orderNumber = 206523627,
            .lineDetails = New List(Of SwitchesSapLineProperty) From {
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 10,
                .oldSku = 305990,
                .newSku = 313382,
                .reason = "huison"
                },
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 20,
                .oldSku = 305991,
                .newSku = 313243,
                .reason = "huison"
                },
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 30,
                .oldSku = 305993,
                .newSku = 313240,
                .reason = "huison"
                }
            }
        },
        New SwitchesSapOrderProperty() With {
            .orderNumber = 206523628,
            .lineDetails = New List(Of SwitchesSapLineProperty) From {
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 10,
                .oldSku = 661755,
                .newSku = 313244,
                .reason = "huison"
                }
            }
        },
        New SwitchesSapOrderProperty() With {
            .orderNumber = 206523631,
            .lineDetails = New List(Of SwitchesSapLineProperty) From {
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 10,
                .oldSku = 661755,
                .newSku = 313244,
                .reason = "huison"
                }
            }
        },
         New SwitchesSapOrderProperty() With {
            .orderNumber = 206523630,
            .lineDetails = New List(Of SwitchesSapLineProperty) From {
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 50,
                .oldSku = 659079,
                .newSku = 301994,
                .reason = "huison"
                },
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 120,
                .oldSku = 673684,
                .newSku = 301397,
                .reason = "huison"
                }
            }
        },
         New SwitchesSapOrderProperty() With {
            .orderNumber = 206523632,
            .lineDetails = New List(Of SwitchesSapLineProperty) From {
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 10,
                .oldSku = 675778,
                .newSku = 655402,
                .reason = "huison"
                },
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 120,
                .oldSku = 675778,
                .newSku = 655402,
                .reason = "huison"
                },
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 300,
                .oldSku = 675778,
                .newSku = 655402,
                .reason = "huison"
                },
                New SwitchesSapLineProperty With {
                .isSameBarcode = True,
                .lineNumber = 420,
                .oldSku = 675778,
                .newSku = 655402,
                .reason = "huison"
                }
            }
        }
    }
        Return list
    End Function

    <TestMethod>
    Public Sub runRddChanges_Should_ChangeRoutes_When_ConnectedToSap()
        Dim sap As ISAPLib = Create.sapLib
        'Dim tCode As New 

        'tCode.runRDDChange(206522986, #03/04/2020#, #03/03/2020#, "XUY", "IDA Test: testing greyed out csr entering", "DeliveryDatesLog")

    End Sub

    <TestMethod>
    Public Sub runRouteCodeChanges_Should_ChangeRoutes_When_ConnectedToSap()
        Dim sap As ISAPLib = Create.sapLib
        'Dim tCode As New SwitchesVA02Runner(sap, Log)

        'tCode.runRouteCodeChange(206522985, "XUY", "IDA Test: testing greyed out csr entering", "ZA0001", "DeliveryDatesLog")

    End Sub

    <TestMethod>
    Public Sub runDeliveryBlocksChanges_Should_ChangeRoutes_When_ConnectedToSap()
        Dim sap As ISAPLib = Create.sapLib
        'Dim tCode As New VA02(sap)

        'tCode.runDeliveryBlockChange(206522985, "XUY", "XYUSON", "Z8", "DeliveryDatesLog")
    End Sub

    <TestMethod>
    Public Sub runQuantityChange_Should_ChangeQTYs_When_ConnectedToSap()
        Dim sap As ISAPLib = Create.sapLib
        'Dim tCode As New VA02(sap)

        'tCode.runQuantityChange(getBaseSalesDocumentList.First)
    End Sub

    Public Shared Function getBaseSalesDocumentList() As List(Of QtyConversionOrderProperty)
        Return New List(Of QtyConversionOrderProperty) From {
                    New QtyConversionOrderProperty() With {.orderNumber = 206522823,
                        .shipTo = 56514,
                        .documentLineList = New List(Of DocumentLine) From {
                            New DocumentLine() With {.item = 10, .material = 173240, .quantity = 1}
                        },
                        .documentLineChangeList = New List(Of DocumentLine) From {
                            New DocumentLine() With {.item = 10, .material = 173240, .quantity = 4}
                        }
                    }
        }
    End Function
End Class