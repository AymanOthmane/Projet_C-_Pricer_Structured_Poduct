


using System;
using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics;
using ToolBox;

static void Main()
        {
            string filePath = @"...\Interface Excel.xlsx";

            var data = Data.GetDataFromExcel(filePath);

            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
        }