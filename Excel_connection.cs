using OfficeOpenXml;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static void Main()
    {
        string filePath = @"C:\Users\carri\OneDrive - Université Paris-Dauphine\cours dauphine\M2\S1\C#\Connection Excel\Interface Excel.xlsx";
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using (var package = new ExcelPackage(new System.IO.FileInfo(filePath)))
        {
            int numberOfWorksheets = package.Workbook.Worksheets.Count;
            Console.WriteLine(numberOfWorksheets);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            if (worksheet != null)
            {
                double N = worksheet.Cells[5,3].GetValue<double>();
                string f = worksheet.Cells[6,3].GetValue<string>();
                DateTime mat = worksheet.Cells[7,3].GetValue<DateTime>();
                mat.ToString("d");
                bool autocall = worksheet.Cells[8,3].GetValue<bool>();
                DateTime strike_date = worksheet.Cells[9,3].GetValue<DateTime>();
                strike_date.ToString("d");
            }
            
            else
            {
                Console.WriteLine("La feuille de calcul 'Interface' n'a pas été trouvée.");
            }
        }
    }
}
