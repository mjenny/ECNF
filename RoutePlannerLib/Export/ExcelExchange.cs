using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export
{
    public class ExcelExchange
    {
 
        public string WriteToFile(String fileName, City from, City to, List<Link> links)
        {
            object misValue = System.Reflection.Missing.Value; 
            Application excelApplication = new Application();
            Workbook excelWorkBook = excelApplication.Workbooks.Add(misValue);
            Worksheet excelWorkSheet = excelWorkBook.Worksheets.get_Item(1);

            excelWorkSheet.Cells[1, 1] = "Hallo Excel";
            excelWorkBook.SaveAs("TestFile.xlst");
            excelWorkBook.Close();
            return "Ok";

        }
    }
}
