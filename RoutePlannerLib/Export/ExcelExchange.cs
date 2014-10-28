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

            excelWorkSheet.Cells[1, 1] = "From";
            excelWorkSheet.Cells[1, 2] = "To";
            excelWorkSheet.Cells[1, 3] = "Distance";
            excelWorkSheet.Cells[1, 3] = "Transport Mode";
            for (int i = 1; i < 5; i++)
            {
                excelWorkSheet.Cells[1, i].Font.Size = 14;
            }

            int j = 2;
            foreach (var l in links)
            {
                excelWorkSheet.Cells[j, 1] = l.FromCity.Name + " (" + l.FromCity.Country + ")";
                excelWorkSheet.Cells[j, 2] = l.ToCity.Name + " (" + l.ToCity.Country + ")";
                excelWorkSheet.Cells[j, 3] = l.Distance;
                excelWorkSheet.Cells[j, 4] = l.TransportMode.ToString();
                ++j;
            }
            excelWorkBook.SaveAs(fileName);
            excelWorkBook.Close();
            return "Ok";

        }
    }
}
