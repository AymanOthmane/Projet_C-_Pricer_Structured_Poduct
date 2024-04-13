

using System;
using MathNet.Numerics.Distributions;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics;
using ToolBox;

class HestonModel
{
    // Heston model parameters
    private double kappa;    // mean reversion speed
    private double theta;    // long-term volatility level
    private double sigma;    // volatility of volatility
    private double rho;      // correlation between asset price and volatility
    private double v0;       // initial variances
    public double r;        // risk-free interest rate
    private double S0;       // initial asset price

    public HestonModel(double kappa, double theta, double sigma, double rho, double v0, double r, double S0)
    {
        this.kappa = kappa;
        this.theta = theta;
        this.sigma = sigma;
        this.rho = rho;
        this.v0 = v0;
        this.r = r;
        this.S0 = S0;
    }

    // Generate a single sample path for the asset price and volatility
    public double[] SimulatePath(double T, int steps)
    {
        double dt = T / steps;
        double[] assetPrices = new double[steps + 1];
        double[] variances = new double[steps + 1];
        assetPrices[0] = S0;
        variances[0] = v0;

        NormalDistribution obj_rnd = new NormalDistribution(0, Math.Sqrt(1.0/252));
        Normal rnd2 = new Normal(0, Math.Sqrt(1.0/252));
        double[] Z1 = obj_rnd.Generate(steps);
        double[] Z2 = obj_rnd.Generate(steps);

        for (int i = 1; i <= steps; i++)
        {
 
            double dW1 =  Z1[i-1];
            double dW2 = rho * Z1[i-1] + Math.Sqrt(1 - rho * rho) * Z2[i-1];

            double volIncrement = kappa * (theta - variances[i - 1]) * dt +
                                   sigma * Math.Sqrt(variances[i - 1]) * dW2;
            double assetIncrement = r * assetPrices[i - 1] * dt +
                                    Math.Sqrt(variances[i - 1]) * assetPrices[i - 1] * dW1;

            variances[i] = Math.Max(variances[i - 1] + volIncrement, 0.0);
            assetPrices[i] = Math.Max(assetPrices[i - 1] + assetIncrement, 0.0);

        }

        return assetPrices;
    }
}

class MonteCarloPricer
{
    private HestonModel model;

    public MonteCarloPricer(HestonModel model)
    {
        this.model = model;
    }

    // Price European call option using Monte Carlo simulation
    public double PriceEuropeanCall(double T, int steps, int paths, double strike)
    {
        double sum = 0.0;
        double[] path = new double[steps+1];
        for (int i = 0; i < paths; i++)
        {
            path = model.SimulatePath(T, steps);
            double ST = path[path.Length - 1]; // Terminal asset price
            sum += Math.Max(ST - strike, 0.0);    

        }
        double discountedPrice = sum / paths * Math.Exp(-model.r * T);
        return discountedPrice;
    }

    // Price European put option using Monte Carlo simulation
    public double PriceEuropeanPut(double T, int steps, int paths, double strike)
    {
        double sum = 0.0;
        for (int i = 0; i < paths; i++)
        {
            double[] path = model.SimulatePath(T, steps);
            double ST = path[path.Length - 1]; // Terminal asset price
            // Console.WriteLine(ST);
            sum += Math.Max(strike - ST, 0.0);
        }
        double discountedPrice = sum / paths * Math.Exp(-model.r * T);
        // Console.WriteLine(discountedPrice);
        return discountedPrice;
    }
}

// class Program
// {
//     static async void Main(string[] args)
//     {
//         // Heston model parameters
//         double kappa = 1.0;
//         double theta = 0.16;
//         double sigma = 0.0625;
//         double rho = -0.8;
//         double v0 = 0.16;
//         double r = 0.00;
//         double S0 = 100.0;

//         // Option parameters
//         double T = 1.0;      // Time to expiration
//         double strike = 100; // Option strike price

//         // Simulation parameters
//         int steps = 252;   // Number of time steps
//         int paths = 1000;  // Number of simulation paths

//         HestonModel model = new HestonModel(kappa, theta, sigma, rho, v0, r, S0);
//         MonteCarloPricer pricer = new MonteCarloPricer(model);

//         double callPrice = pricer.PriceEuropeanCall(T, steps, paths, strike);
//         double putPrice = pricer.PriceEuropeanPut(T, steps, paths, strike);

//         Console.WriteLine("European Call Option Price: " + callPrice);
//         Console.WriteLine("European Put Option Price: " + putPrice);
        
//         // Yahoo.Finance.EquityData yahoo= new Yahoo.Finance.EquityData;
        
//         double[][] ST = Generate_Paths.StockPaths(S0, r, v0, 1.0, 10, 10, 0, 1.0);  //double S0, double r, double vol, double dt, int nb_Simulations, int nb_steps, double mean_dist, double stdev_dist){
//     }
// }

