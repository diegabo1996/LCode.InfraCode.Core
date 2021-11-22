using OfficeOpenXml;
using System.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Base._3._0
{
    public static class HerramientasExcel
    {
        public static byte[]ExportarDataTableExcel(DataTable Tabla)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage pck = new ExcelPackage())
            {
                
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("WppMonitor");
                ws.Cells["A1"].LoadFromDataTable(Tabla, true);
                return pck.GetAsByteArray();
            }
        }
    }
}
