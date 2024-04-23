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
            int nb_Simulations = 10000;
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
                    package = new ExcelPackage(new FileInfo(filePath));
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    if (worksheet != null)
                    {
                        double N = worksheet.Cells[5, 3].GetValue<double>();
                        string f = worksheet.Cells[6, 3].GetValue<string>();
                        double mat = worksheet.Cells[7, 3].GetValue<double>();
                        string product = worksheet.Cells[8, 3].GetValue<string>();
                        double coupon = worksheet.Cells[9, 3].GetValue<double>();
                        double price = worksheet.Cells[5, 9].GetValue<double>();
                        double vol = worksheet.Cells[5, 10].GetValue<double>();
                        DateTime strike_date = worksheet.Cells[10, 3].GetValue<DateTime>();

                        dataList.Add(N);
                        if (f=="Annuelle ")
                        {
                            dataList.Add(true);
                        }
                        else 
                        {
                            dataList.Add(false);
                        }
                        dataList.Add(mat);
                        dataList.Add(product);
                        dataList.Add(coupon);
                        dataList.Add(price);
                        dataList.Add(vol);
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
  
        public static double Rate(double mat)
        {
            
            Dictionary<Double,Double> Rates = new Dictionary<double, double>
            {
                {1/52,0.03902},
                {2/52,0.03903},
                {1/12,0.03905},
                {1/6,0.03893},
                {1/4,0.0383},
                {1/2,0.0368},
                {3/4,0.03528},
                {1.0,0.03394},
                {1.5,0.03135},
                {2.0,0.02961},
                {3.0,0.02734},
                {4.0,0.02603},
                {5.0,0.02534},
                {6.0,0.02496},
                {7.0,0.02484},
                {8.0,0.02485},
                {9.0,0.02492},
                {10.0,0.0251},
                {12.0,0.02544},
                {15.0,0.02588},
                {20.0,0.02563},
                {30.0,0.02402},
            };
            
            return Rates[mat];

        }
    }


}
