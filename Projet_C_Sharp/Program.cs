using System;
using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics;
using ToolBox;
using Autocall;

namespace Program{

    public class ControleCenter{
        public static void Main(string[] args)
        {
            string filePath = Directory.GetCurrentDirectory() + @"\Interface Excel.xlsx";
            var data = Data.GetDataFromExcel(filePath);
            
            // foreach (var item in data)
            // {
            //     Console.WriteLine(item);
            // }
            if (Convert.ToString(data[3]) == "ATHENA")
            {
                Athena product = new Athena(Convert.ToDateTime(data[7]),Convert.ToDouble(data[5]),Convert.ToDouble(data[5]),0.02,Convert.ToInt32(data[2]),Convert.ToDouble(data[6]),Convert.ToDouble(data[4]),0.00396825396825396825396825396825,Convert.ToBoolean(data[1]));
                Console.WriteLine(product.Pricing());
            }
            else{
                Console.WriteLine("Pas rentré dans le if");
            }
        }
    }
}