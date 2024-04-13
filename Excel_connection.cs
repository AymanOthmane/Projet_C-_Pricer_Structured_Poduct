using OfficeOpenXml;
using System;
using System.Collections.Generic;

namespace DataRetrieval
{
    public class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\carri\OneDrive - Université Paris-Dauphine\cours dauphine\M2\S1\C#\Connection Excel\Interface Excel.xlsx";

            var data = GetDataFromExcel(filePath);

            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        }

        public static List<object> GetDataFromExcel(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            List<object> dataList = new List<object>();
            ExcelPackage package = null;

            try
            {
                package = new ExcelPackage(new System.IO.FileInfo(filePath));
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                if (worksheet != null)
                {
                    double N = worksheet.Cells[5, 3].GetValue<double>();
                    string f = worksheet.Cells[6, 3].GetValue<string>();
                    DateTime mat = worksheet.Cells[7, 3].GetValue<DateTime>();
                    bool autocall = worksheet.Cells[8, 3].GetValue<bool>();
                    DateTime strike_date = worksheet.Cells[9, 3].GetValue<DateTime>();

                    dataList.Add(N);
                    dataList.Add(f);
                    dataList.Add(mat);
                    dataList.Add(autocall);
                    dataList.Add(strike_date);
                }
                else
                {
                    Console.WriteLine("La feuille de calcul 'Interface' n'a pas été trouvée.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une erreur s'est produite : " + ex.Message);
            }
            finally
            {
                if (package != null)
                {
                    package.Dispose();
                }
            }

            return dataList;
        }
    }
}
