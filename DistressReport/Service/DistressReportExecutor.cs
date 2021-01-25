using DistressReport.Model;
using IDAUtil;
using IDAUtil.Service;
using lib;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DistressReport.Service {
    class DistressReportExecutor {
        private readonly IdaLog idaLog = new IdaLog();
        private readonly string salesOrg;
        public List<GenericDistressProperty> genericDistressList;
        private dynamic finalDistressList; //TODO: Change that somehow to be scrictly typed
        private readonly string id = $"{DateTime.Now} {Environment.MachineName}";

        public DistressReportExecutor(string salesOrg) {
            this.salesOrg = salesOrg;
        }

        public void startLogs() {
            idaLog.insertToActivityLog("Distress", "start", id, salesOrg);
        }

        /// <summary>
        /// Gets data from Dynamic GenericDistressProperty and writes it into an attributed class depending on sales organisation with it's speciffications 
        /// </summary>
        /// <returns></returns>
        public bool calculateSalesOrgSpecifficDistress() {
            switch (salesOrg) {
                case "CZ01":
                    List<GenericDistressProperty> czList = genericDistressList;

                    if (czList.Count > 0) {
                        finalDistressList = (from x in czList select new CZDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "ZA01":
                case "KE02":
                case "NG01":
                    List<GenericDistressProperty> aList = genericDistressList.Where(x => x.rejReason == "" && x.afterReleaseRej == "" && x.deliveryBlock != "Z4" && x.deliveryBlock != "04" && x.docType == "ZOR").ToList();

                    if (aList.Count > 0) {
                        finalDistressList = (from x in aList select new AfricanDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "GB01":
                    List<GenericDistressProperty> gbList = genericDistressList.Where(x => x.rejReason == "" && x.afterReleaseRej == "").ToList();

                    if (gbList.Count > 0) {
                        finalDistressList = (from x in gbList select new GBDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "ES01":
                    List<GenericDistressProperty> esList = genericDistressList.Where(x => x.rejReason == "").ToList();

                    if (esList.Count > 0) {
                        finalDistressList = (from x in esList select new ESDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "PT01":
                    List<GenericDistressProperty> ptList = genericDistressList.Where(x => x.rejReason == "").ToList();

                    if (ptList.Count > 0) {
                        finalDistressList = (from x in ptList select new ESDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "UA01":
                    if (genericDistressList.Count > 0) {
                        finalDistressList = (from x in genericDistressList select new UADistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "RU01":
                    if (genericDistressList.Count > 0) {
                        finalDistressList = (from x in genericDistressList select new RUDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "NL01":
                    List<GenericDistressProperty> nlList = genericDistressList.Where(x => x.rejReason == "").ToList();

                    if (nlList.Count > 0) {
                        finalDistressList = (from x in nlList select new NLDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "FR01":
                    List<GenericDistressProperty> frList = genericDistressList.Where(x => x.rejReason == "").ToList();

                    if (frList.Count > 0) {
                        finalDistressList = (from x in frList select new FRDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "IT01":
                case "GR01":
                    //Italy and Greece want all the columns
                    List<GenericDistressProperty> itList = genericDistressList.Where(x => x.rejReason == "").ToList();

                    if (itList.Count > 0) {
                        finalDistressList = itList;
                        return true;
                    } else {
                        return false;
                    }

                case "RO01":
                    List<GenericDistressProperty> roList = genericDistressList.Where(x => x.deliveryBlock != "Y2").ToList();

                    if (roList.Count > 0) {
                        finalDistressList = (from x in roList select new RODistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "DE01":
                    List<GenericDistressProperty> deList = genericDistressList.Where(x => x.rejReason == "" && x.afterReleaseRej == "").ToList();

                    if (deList.Count > 0) {
                        finalDistressList = (from x in deList select new DEDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "PL01":
                    List<GenericDistressProperty> plList = genericDistressList.ToList();

                    if (plList.Count > 0) {
                        finalDistressList = (from x in plList select new PLDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                case "TR01":
                    List<GenericDistressProperty> trList = genericDistressList.Where(x => x.rejReason == "").ToList();

                    if (trList.Count > 0) {
                        finalDistressList = (from x in trList select new TRDistressProperty(x)).ToList();
                        return true;
                    } else {
                        return false;
                    }

                default:
                    throw new NotImplementedException($"No implementation found for distress of {salesOrg}");
            }
        }

        public bool calculateGenericDistress(IDataCollectorServiceDistress dc) {
            var list = dc.getDistressList(salesOrg);

            if (!(list is null)) {
                genericDistressList = list;
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Uses using keyword to dispose of excel object in order to reopen it again because listToExcel function only works with closed workbooks
        /// </summary>
        /// <param name="email"></param>
        /// 
        public void sendInfo(string email) {

            IMailUtil mu = Create.mailUtil();
            IDBConnector dbxl = Create.dbXl();
            Workbook wb;

            string path = $"{Paths.DESKTOP}\\{salesOrg} {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}hour {DateTime.Now.Minute}minute Distress Report.xlsx";

            dbxl.listToExcel(list: finalDistressList, path: path);

            using (IExcelUtil xl = Create.xlUtil()) {
                try {
                    wb = xl.getWorkbook(path: path);
                } catch (Exception) {
                    System.Threading.Thread.Sleep(5000);
                    wb = xl.getWorkbook(path: path);
                }

                Worksheet ws = wb.Worksheets[1] as Worksheet;

                try {
                    ws.Name = "Distress Table";
                } catch (Exception) {
                    System.Threading.Thread.Sleep(2000);
                    ws.Name = "Distress Table";
                }

                try {
                    IdaExcelService.prettifyExcel(worksheet: ws, excelUtilObj: xl);
                } catch (Exception) {
                    System.Threading.Thread.Sleep(2000);
                    IdaExcelService.prettifyExcel(worksheet: ws, excelUtilObj: xl);
                }

                if (salesOrg == "ZA01") { highlightPartialCuts(xl, ws); }

                #region ifYouNeedAPivotTableJustAsk
                if (salesOrg.ToUpper() == "ES01" || salesOrg.ToUpper() == "PT01") {

                    wb.Worksheets.Add(After: ws);
                    Worksheet wsPivot = wb.Worksheets[2] as Worksheet;

                    try {
                        wsPivot.Name = "Pivot Table";
                    } catch (Exception) {
                        System.Threading.Thread.Sleep(2000);
                        wsPivot.Name = "Pivot Table";
                    }

                    addPivotTable(workbook: wb, worksheet: ws, worksheetWithPivot: wsPivot, excelUtilObj: xl);
                }
                #endregion

                if (salesOrg.ToUpper() == "FR01") {
                    convertColumnsToNumbers(xl, ws);
                }

                wb.Save();

                mu.mailOneAttachment(emailTo: email, cc: "", subject: $"{salesOrg} Distress Report", body: $"{mu.listToHTMLtable(finalDistressList)}", path: path);
                dbxl.closeConnection();
                xl.closeWBAndAllInstances(wbName: wb.Name);
            }
        }

        private static void convertColumnsToNumbers(IExcelUtil xl, Worksheet ws) {
            int[] columns = { 1, 2, 6, 8, 11, 14, 16, 17, 18, 19 };

            for (int i = 0; i < columns.Count(); i++) {
                xl.setColumnByIndexToNumberFormat(columns[i], ws);
            }
        }

        public void emptyListAction(string email) {
            IMailUtil mu = Create.mailUtil();
            mu.mailSimple(email, $"{salesOrg} No Distressed Lines", "Hello<br><br>There are no distressed lines<br><br>Kind Regards<br>IDA");
            idaLog.insertToActivityLog("Distress", "emptyList", id, salesOrg);
        }

        private static void highlightPartialCuts(IExcelUtil xl, Worksheet ws) {
            long lastColumn = xl.getColumnCount(row: 1, ws: ws);
            long lastRow = xl.getRowCount(column: 2, ws: ws);

            for (int i = 2; i < lastRow; i++) {
                if ((string)ws.Range["K" + i.ToString()].Value != "0") {
                    ws.Range[ws.Cells[i, 1], ws.Cells[i, lastColumn]].Interior.ColorIndex = 36;
                }
            }
        }

        private static void addPivotTable(Workbook workbook, Worksheet worksheet, Worksheet worksheetWithPivot, IExcelUtil excelUtilObj) {
            //converts last column to int
            long lastColumn = excelUtilObj.getColumnCount(1, worksheet);
            long lastRow = excelUtilObj.getRowCount(1, worksheet);
            Range rangeToConvertToInt = worksheet.Range[worksheet.Cells[1, lastColumn], worksheet.Cells[lastRow, lastColumn]];
            rangeToConvertToInt.Cells.NumberFormat = "#0";
            rangeToConvertToInt.Value = rangeToConvertToInt.Value;

            object useDefault = Type.Missing;
            string pivotTableName = @"DistressPivotTable";

            Range pivotData = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[excelUtilObj.getRowCount(1, worksheet), excelUtilObj.getColumnCount(1, worksheet)]];
            Range pivotDestination = worksheetWithPivot.Range["A1", useDefault];

            #region imAWott??
            workbook.PivotTableWizard(
                    SourceType: XlPivotTableSourceType.xlDatabase,
                    SourceData: pivotData,
                    TableDestination: pivotDestination,
                    TableName: pivotTableName,
                    RowGrand: true,
                    ColumnGrand: true,
                    SaveData: true,
                    HasAutoFormat: true,
                    AutoPage: useDefault,
                    Reserved: useDefault,
                    BackgroundQuery: false,
                    OptimizeCache: false,
                    PageFieldOrder: XlOrder.xlDownThenOver,
                    PageFieldWrapCount: 0,
                    ReadData: useDefault,
                    Connection: useDefault
            );
            #endregion

            PivotTable pivotTable = (PivotTable)worksheetWithPivot.PivotTables(pivotTableName);

            #region pivotTableFormatting
            pivotTable.RowAxisLayout(RowLayout: XlLayoutRowType.xlTabularRow);
            pivotTable.Format(Format: XlPivotFormatType.xlReport2);
            pivotTable.InGridDropZones = false;
            pivotTable.SmallGrid = false;
            pivotTable.ShowTableStyleRowStripes = true;
            pivotTable.TableStyle2 = "PivotStyleMedium9";
            #endregion

            #region pivotTableFieldAssignment
            PivotField skuPivotField = (PivotField)pivotTable.PivotFields(6);
            skuPivotField.Orientation = XlPivotFieldOrientation.xlRowField;
            skuPivotField.Position = 1;
            skuPivotField.Subtotals[1] = false;

            PivotField descriptionPivotField = (PivotField)pivotTable.PivotFields(10);
            descriptionPivotField.Orientation = XlPivotFieldOrientation.xlRowField;
            descriptionPivotField.Position = 2;
            descriptionPivotField.Subtotals[1] = false;

            PivotField commentsPivotField = (PivotField)pivotTable.PivotFields(11);
            commentsPivotField.Orientation = XlPivotFieldOrientation.xlRowField;
            commentsPivotField.Position = 3;
            commentsPivotField.Subtotals[1] = false;

            PivotField cutQtyPivotField = (PivotField)pivotTable.PivotFields(20);
            cutQtyPivotField.Orientation = XlPivotFieldOrientation.xlDataField;
            cutQtyPivotField.Function = XlConsolidationFunction.xlSum;
            #endregion

            worksheetWithPivot.UsedRange.Columns.AutoFit();
        }
        public void finishLogs() {
            idaLog.insertToActivityLog("Distress", "success", id, salesOrg);
        }
    }
}
