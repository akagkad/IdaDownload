using IDAUtil;
using lib;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

namespace KIWI {
    public class EmailService {
        public IMailUtil mailUtil;

        public EmailService(IMailUtil mailUtil) {
            this.mailUtil = mailUtil;
        }

        public object sendEmailNotification(string emailTo, string cc, string subject, List<QtyConversionOrderProperty> salesDocumetList) {
            string body;
            body = $"Hello,{Constants.vbCr}{getKIWIHTMLTable(salesDocumetList)}{Constants.vbCr}Kind regards,{Constants.vbCr}IDA";
            Array argpathArr = null;
            return mailUtil.mailArg(emailTo, cc, subject, body, pathArr: ref argpathArr);
        }

        public string getKIWIHTMLTable(List<QtyConversionOrderProperty> salesDocumetList) {
            string table;
            table = "<table style = 'border: 1px solid black; border-collapse: collapse; width: 100%;'><tr style = 'border: 1px solid black;'>";
            table += getHTMLColumnNames(new[] { "document", "item", "soldTo", "shipTo", "shipTo name", "SKU", "oldQty", "newQty", "isChanged", "status" });
            table += "</tr>";
            foreach (var salesDocument in salesDocumetList) {
                for (int i = 0, loopTo = salesDocument.documentLineChangeList.Count - 1; i <= loopTo; i++) {
                    table += "<tr style = 'border: 1px solid black; width:1%; white-space:nowrap;'>";
                    table += getHTMLCell(salesDocument.orderNumber.ToString());
                    table += getHTMLCell(salesDocument.documentLineChangeList[i].item.ToString());
                    table += getHTMLCell(salesDocument.soldTo.ToString());
                    table += getHTMLCell(salesDocument.shipTo.ToString());
                    table += getHTMLCell(salesDocument.shipToName);
                    table += getHTMLCell(salesDocument.documentLineChangeList[i].material.ToString());
                    table += getHTMLCell(salesDocument.documentLineList[i].quantity.ToString());
                    table += getHTMLCell(salesDocument.documentLineChangeList[i].quantity.ToString());
                    table += getHTMLCell(salesDocument.documentLineChangeList[i].isChanged ? salesDocument.documentLineChangeList[i].isChanged.ToString() : "");
                    table += getHTMLCell(salesDocument.documentLineChangeList[i].status);
                    table += "</tr>";
                }
            }

            table += "</table>";
            return table;
        }

        private string getHTMLColumnNames(string[] columnNameArr) {
            string text = "";
            foreach (var name in columnNameArr)
                text += "<th style = 'border: 1px solid black;'>" + name + "</th>";
            return text;
        }

        private string getHTMLCell(string text) {
            return "<td style = 'border: 1px solid black;' class='block'>" + text + "</td>";
        }
    }
}