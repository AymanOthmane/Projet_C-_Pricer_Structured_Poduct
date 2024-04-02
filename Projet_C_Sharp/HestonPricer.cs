

using System;
using MathNet.Numerics.Distributions;

class HestonModel
{
    // Heston model parameters
    private double kappa;    // mean reversion speed
    private double theta;    // long-term volatility level
    private double sigma;    // volatility of volatility
    private double rho;      // correlation between asset price and volatility
    private double v0;       // initial volatility
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
        double[] volatilities = new double[steps + 1];
        assetPrices[0] = S0;
        volatilities[0] = v0;

        Normal rnd1 = new Normal(0, Math.Sqrt(1.0/252));
        Normal rnd2 = new Normal(0, Math.Sqrt(1.0/252));

        for (int i = 1; i <= steps; i++)
        {
            double Z1 = rnd1.Sample();
            double Z2 = rnd2.Sample();

            double dW1 =  Z1;
            double dW2 = (rho * Z1 + Math.Sqrt(1 - rho * rho) * Z2);

            double volIncrement = kappa * (theta - volatilities[i - 1]) * dt +
                                   sigma * Math.Sqrt(volatilities[i - 1]) * dW2;
            double assetIncrement = r * assetPrices[i - 1] * dt +
                                    volatilities[i - 1] * assetPrices[i - 1] * dW1;

            volatilities[i] = Math.Max(volatilities[i - 1] + volIncrement, 0.0);
            assetPrices[i] = Math.Max(assetPrices[i - 1] + assetIncrement, 0.0);
            // Console.WriteLine(assetPrices[i] + " " + volatilities[i]);

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

class Program
{
    static void Main(string[] args)
    {
        // Heston model parameters
        double kappa = 1.0;
        double theta = 0.4;
        double sigma = 2.0;
        double rho = -0.8;
        double v0 = 0.4;
        double r = 1.00;
        double S0 = 1.0;

        // Option parameters
        double T = 10.0;      // Time to expiration
        double strike = 2; // Option strike price

        // Simulation parameters
        int steps = 252;   // Number of time steps
        int paths = 1000;  // Number of simulation paths

        HestonModel model = new HestonModel(kappa, theta, sigma, rho, v0, r, S0);
        MonteCarloPricer pricer = new MonteCarloPricer(model);

        double callPrice = pricer.PriceEuropeanCall(T, steps, paths, strike);
        double putPrice = pricer.PriceEuropeanPut(T, steps, paths, strike);

        Console.WriteLine("European Call Option Price: " + callPrice);
        Console.WriteLine("European Put Option Price: " + putPrice);

        
        // for (int i = 0; i < 25; i++)
        // {
        // double randomGaussianValue = normalDist.Sample();
        // Console.WriteLine(randomGaussianValue);
        //     // Console.WriteLine(Z1);
        //     // Console.WriteLine(Z2);
           
        // }
        Console.WriteLine(S0-strike* Math.Exp(-model.r * T));
        Console.WriteLine(callPrice-putPrice);
    }
}

