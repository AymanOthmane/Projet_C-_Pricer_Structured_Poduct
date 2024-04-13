using System;
using System.Reflection.Metadata.Ecma335;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using OfficeOpenXml;
using System.Collections.Generic;

namespace ToolBox
{
    public static class Generate_Paths{

        public static double[][] StockPaths(double S0, double r, double vol, double dt, int nb_steps, double mean_dist, double stdev_dist){
            NormalDistribution obj_rnd = new NormalDistribution(mean: mean_dist, stdDev: stdev_dist);
            int nb_Simulations = 100000;
            double[][] ST = new double[nb_Simulations][];
            double[] Z = new double[nb_steps - 1];
            for (int i = 0; i<nb_Simulations;i++)
            {
                double[] Path = new double[nb_steps];
                ST[i] = new double[nb_steps];
                Path[0] = S0;
                Z = obj_rnd.Generate(nb_steps);

                for (int j = 1; j<nb_steps;j++){
                    Path[j] = Path[j-1] * Math.Exp(((r) - 0.5 * vol * vol) * dt + vol * Math.Sqrt(dt) * Z[j]);
                }
                ST[i]=Path;
                
            }

            return ST;
        }
    }

    public class DateCalculator
    {
        public static DateTime GetEndDate(DateTime startDate, int years)
        {
            return startDate.AddYears(years);
        }

        public static bool IsBusinessDay(DateTime date)
        {
            return !date.DayOfWeek.Equals(DayOfWeek.Saturday) && !date.DayOfWeek.Equals(DayOfWeek.Sunday);
        }

        public static DateTime GetNextBusinessDay(DateTime date)
        {
            while (!IsBusinessDay(date))
            {
                date = date.AddDays(1);
            }

            return date;
        }

        public DateTime GetEndBusinessDate(DateTime startDate, int years)
        {
            DateTime enddate;

            enddate = GetEndDate(startDate,years);
            enddate = GetNextBusinessDay(enddate);


            return enddate;
        }

        public static int GetNumberOfBusinessDays(DateTime startDate, DateTime endDate)
        {
            int count = 0;

            while (startDate <= endDate)
            {
                if (IsBusinessDay(startDate))
                {
                    count++;
                }

                startDate = startDate.AddDays(1);
            }

            return count;
        }

        public static int GetNumberOfBusinessYears(int businessDays)
        {
            return businessDays / 365;
        }

        public static double TimeVariation(DateTime date)
        {
            double dt;

            if (DateTime.IsLeapYear(date.Year) && IsBusinessDay(DateTime.Parse(date.Year + "-02-29")))
            {
                dt = 1.0 / 253.0;
            }
            else
            {
                dt = 1.0 / 252.0;
            }

            return dt;
        }

        public static List<DateTime> CalculationDate( DateTime startDate, bool frequency, int T)
        {
            List<DateTime> datesList = new List<DateTime>();
            DateTime date = startDate;
            int t = 1;

            if (frequency)
            {
                while (t < T)
                {
                    date = date.AddYears(1);
                    if (IsBusinessDay(date) == false)
                    {
                        date = GetNextBusinessDay(date);
                    }
                    datesList.Add(date);
                    t++;
                }
            }
            else
            {
                while (t < T)
                {
                    date = date.AddMonths(3);
                    if (IsBusinessDay(date) == false)
                    {
                        date = GetNextBusinessDay(date);
                    }
                    datesList.Add(date);
                    t++;
                }
            }

            return datesList;
        }

        public static List<int> ReturnDateFromIndex(DateTime startDate, bool frequency, int T)
        {
            List<DateTime> datesList = CalculationDate( startDate,  frequency,  T);
            List<int> indexList = new List<int>();
            int dist;

            foreach (DateTime date in datesList)
            {
                dist = GetNumberOfBusinessDays(startDate, date);
                indexList.Add(dist);
            }
            return indexList;
        }



    }

    public class Data{
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
                        double coupon = worksheet.Cells[9, 3].GetValue<double>();
                        DateTime strike_date = worksheet.Cells[10, 3].GetValue<DateTime>();

                        dataList.Add(N);
                        dataList.Add(f);
                        dataList.Add(mat);
                        dataList.Add(autocall);
                        dataList.Add(coupon);
                        dataList.Add(strike_date);
                    }
                    else
                    {
                        Console.WriteLine("La feuille de calcul 'Interface' n'a pas été trouvée.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Une erreur s'est produite : "+ " Veillez à ce que le fichier excel soit bien fermé." + ex.Message);
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
