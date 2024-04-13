using System;
using System.Reflection.Metadata.Ecma335;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;


namespace ToolBox
{
    public static class Generate_Paths{

        public static double[][] StockPaths(double S0, double r, double vol, double dt, int nb_Simulations, int nb_steps, double mean_dist, double stdev_dist){
            NormalDistribution obj_rnd = new NormalDistribution(mean: mean_dist, stdDev: stdev_dist);
            double div=0.0;
            double[][] ST = new double[nb_Simulations][];
            double[] Z = new double[nb_steps - 1];
            // double[] Path = new double[nb_steps];
            // Path[0] = S0;
            for (int i = 0; i<nb_Simulations;i++)
            {
                double[] Path = new double[nb_steps];
                ST[i] = new double[nb_steps];
                Path[0] = S0;
                Z = obj_rnd.Generate(nb_steps);

                for (int j = 1; j<nb_steps;j++){
                    Path[j] = Path[j-1] * Math.Exp(((r-div) - 0.5 * vol * vol) * dt + vol * Math.Sqrt(dt) * Z[j]);
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

    }
}

// class NormalDistributionGenerator
// {
//     private Random random;

//     public NormalDistributionGenerator()
//     {
//         // Initialize the Random object
//         random = new Random();
//     }

//     // Generate a single random sample from a normal distribution with mean mu and standard deviation sigma
//     public double Generate(double mu, double sigma)
//     {
//         double u1 = 1.0 - random.NextDouble(); // Uniform random number between 0 and 1
//         double u2 = 1.0 - random.NextDouble(); // Uniform random number between 0 and 1

//         // Box-Muller transform to generate a normally distributed random number
//         double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

//         // Apply mean and standard deviation
//         return mu + sigma * z;
//     }
// }

// class Programs
// {
//     static void Main(string[] args)
//     {
//         // Example usage
//         NormalDistributionGenerator generator = new NormalDistributionGenerator();
//         double mu = 0; // Mean
//         double sigma = 1; // Standard deviation

//         // Generate a single random sample from the normal distribution
//         double sample = generator.Generate(mu, sigma);
//         Console.WriteLine("Generated sample: " + sample);
//     }
// }
