using System;
// using Excel = Microsoft.Office.Interop.Excel;
using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Univariate;
using ToolBox;
using Accord.IO;
// using Accord.Math;


namespace Options
{

    public class Vanilla_Option{
    private double S; // Current stock price
    private double K; // Strike price
    private double r; // Risk-free interest rate
    private double div; // Dividend yield
    private double T; // Time to maturity (in years)
    private double vol; // Volatility of the underlying asset
    private bool isCall; // Flag indicating whether the option is a call (true) or a put (false)
        public Vanilla_Option(double S, double K, double r, double div, double T, double vol, bool isCall)
        {
            this.S = S;
            this.K = K;
            this.r = r;
            this.div = div;
            this.T = T;
            this.vol = vol;
            this.isCall = isCall;
        }
        
        
        public static double Price_BS_Option(double S, double K, double r, double div, double T, double vol, bool isCall)
        {
            double d1 = (Math.Log(S / K) + (r - div + 0.5 * vol * vol) * T) / (vol * Math.Sqrt(T));
            double d2 = d1 - vol * Math.Sqrt(T);

            if (isCall)
            {
                return S * Math.Exp(-div * T) * Normal.CDF(0,1,d1) - K * Math.Exp(-r * T) * Normal.CDF(0,1,d2);
            }
            else
            {
                return K * Math.Exp(-r * T) * Normal.CDF(0,1,-d2) - S * Math.Exp(-div * T) * Normal.CDF(0,1,-d1);
            }
        }

        static void Main(string[] args){

            Console.WriteLine(Price_BS_Option(100,100,0.05,0,1,20,true));
        }
    }
    public class Knock_In_Option
    {
        // Define the common parameters
        private static int n = 100000;  // Number of simulations
        private static double T = 1;    // Time to maturity
        private static double K = 100;  // Strike price
        private static double B = 10;   // Barrier price

        // Define parameters for each underlying
        // private static double S_US = 100;  // Current stock price
        // private static double r_US = 0.05; // Risk-free rate
        // private static double vol_US = 0.13;  // Volatility
        // private static double div_US = 0.019; // Dividend yield

        private static double S_EU = 100;  // Current stock price
        private static double r_EU = 0.045; // Risk-free rate
        private static double div_EU = 0.0;
        private static double vol_EU = 0.15;  // Volatility

        public static double Price_Option_KI(double S, double K, double B, double r, double div, double T, double vol, int n, bool isCall)
        {
            // Simulate the stock price paths
            var obj_rnd = new NormalDistribution(mean: 0, stdDev: Math.Sqrt(1.0/252));
            double[] z = obj_rnd.Generate(n);
            double[][] ST = new double[n][];

            // Calculate the option payoff
            double[] payoff = new double[n];
            for (int i = 0; i < n; i++)
            {
                if (isCall)
                {
                    payoff[i] = Math.Max(ST[i][] - K, 0);
                }
                else
                {
                    payoff[i] = Math.Max(K - ST[i][], 0);
                }

                if (payoff[i] < B)
                {
                    payoff[i] = 0;
                }

                payoff[i] = Math.Exp(-r * T) * payoff[i];
            }

            // Calculate the option price
            double price = payoff.Average();
            return price;
        }
    }

    
}