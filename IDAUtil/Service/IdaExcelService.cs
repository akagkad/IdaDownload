using lib;
using Microsoft.Office.Interop.Excel;

namespace IDAUtil.Service {
    public static class IdaExcelService {
        public static void prettifyExcel(Worksheet worksheet, IExcelUtil excelUtilObj) {
            var range = worksheet.UsedRange;
            long lastColumn = excelUtilObj.getColumnCount(row: 1, ws: worksheet);

            #region gottaBuildThatWall
            range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;

            range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;

            range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;

            range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;

            range.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlInsideVertical].Weight = XlBorderWeight.xlThin;

            range.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
            range.Borders[XlBordersIndex.xlInsideHorizontal].Weight = XlBorderWeight.xlThin;
            #endregion


            //this is full of workarounds due to excel interop bugs like object not set
            Range firstRow = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, lastColumn]];

            if (firstRow is null) { firstRow = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, lastColumn]]; }

            Interior firstRowInterior = firstRow.Interior;

            object firstRowColorIndex = firstRowInterior.ColorIndex;

            firstRowColorIndex = 34;

            range.Columns.AutoFit();
        }
    }
}
