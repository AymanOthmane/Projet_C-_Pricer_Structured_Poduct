using System;
using System.Security.Cryptography.X509Certificates;
using Accord.Math;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Distributions.Univariate;
using MathNet.Numerics.Distributions;
using ToolBox;

namespace Autocall{


    public class Athena{
        
    private double S; // Current stock price
    private double K; // Strike price
    private double r; // Risk-free interest rate
    private double barrier; //PDI Barrier
    private double div; // Dividend yield
    private double T; // Time to maturity (in years)
    private double vol; // Volatility of the underlying asset
    private double coupon; //Coupon of product
    private double dt; //Time step
    private bool frequency;
    
    public Athena(double S, double K, double r, double div, double T, double vol, double coupon, double dt, bool frequency)
    {
        this.S = S;
        this.K = K;
        this.r = r;
        this.div = div;
        this.T = T;
        this.vol = vol;
        this.coupon = coupon;
        this.frequency = frequency;
        this.dt = dt;
    }

        public double Pricing()
        {

            double[][] Paths = Generate_Paths.StockPaths(S, r, vol, dt, Convert.ToInt32(T/dt), 0, dt);  //double S0, double r, double vol, double dt, int nb_Simulations, int nb_steps, double mean_dist, double stdev_dist)
            double[][] Observations = ToolBox.ReturnDateFromIndex();
            double[] Payoff = new double[10000];
            double Price = 0;
            
            Observations = Observations.Transpose();
            double[] Observation = new double[10000];

            for (int i = 0; i <= Convert.ToInt32(T/dt)-1; i++)
            {
                Observation= Observations[i];
                for (int j = 0; j <= Observation.Length; j++)
                {
                    if (Observation[j]<K)
                    {
                        Payoff[j]=0;
                    }
                    else{
                        Payoff[j]=1+coupon;
                    };
                    
                }
                Price = Price + Math.Exp(-r) * Payoff.Average();

            }
            int k = Convert.ToInt32(T/dt)-1;
            Observation= Observations[Observation.Length-1];
            for (int j = 0; j <= Observation.Length; j++)
            {
                if (Observation[j]<K && Observation[j]>barrier)
                {
                    Payoff[j]=1;
                }
                else if (Observation[j]>K)
                {
                    Payoff[j]=1+coupon;
                }
                else{
                    Payoff[j] = Observation[j]/K;
                };
                
            }
            Price = Price + Math.Exp(-r) * Payoff.Average();
            return Price;
        }

    }

    public class Phoenix{
        
    private double S; // Current stock price
    private double K; // Strike price
    private double r; // Risk-free interest rate
    private double PDI_barrier; //PDI Barrier
    private double Cpn_barrier; //Coupon Barrier
    private double div; // Dividend yield
    private double T; // Time to maturity (in years)
    private double vol; // Volatility of the underlying asset
    private double coupon; //Coupon of product
    private double dt; //Time step
    private bool frequency;
    
    public Phoenix(double S, double K, double PDI_barrier, double Cpn_barrier, double r, double div, double T, double vol, double coupon, double dt, bool frequency)
    {
        this.S = S;
        this.K = K;
        this.PDI_barrier = PDI_barrier;
        this.Cpn_barrier = Cpn_barrier;
        this.r = r;
        this.div = div;
        this.T = T;
        this.vol = vol;
        this.coupon = coupon;
        this.frequency = frequency;
        this.dt = dt;
    }

        public double Pricing()
        {

            double[][] Paths = Generate_Paths.StockPaths(S, r, vol, dt, Convert.ToInt32(T/dt), 0, dt);  //double S0, double r, double vol, double dt, int nb_Simulations, int nb_steps, double mean_dist, double stdev_dist)
            double[][] Observations = ToolBox.ReturnDateFromIndex();
            double[] Payoff = new double[10000];
            double Price = 0;
            
            Observations = Observations.Transpose();
            double[] Observation = new double[10000];

            for (int i = 0; i <= Convert.ToInt32(T/dt)-1; i++)
            {
                Observation= Observations[i];
                for (int j = 0; j <= Observation.Length; j++)
                {
                    if (Observation[j]<K && Observation[j]>Cpn_barrier)
                    {
                        Payoff[j]=coupon;
                    }
                    else if (Observation[j]<Cpn_barrier)
                    {
                        Payoff[j]=0;
                    }
                    else{
                        Payoff[j]=1+coupon;
                    };
                    
                }
                Price = Price + Math.Exp(-r) * Payoff.Average();

            }
           
            Observation= Observations[Observation.Length-1];
            for (int j = 0; j <= Observation.Length; j++)
            {
                if (Observation[j]< Cpn_barrier && Observation[j]>PDI_barrier)
                {
                    Payoff[j]=1;
                }
                else if (Observation[j]>Cpn_barrier)
                {
                    Payoff[j]=1+coupon;
                }
                else{
                    Payoff[j] = Observation[j]/K;
                };
                
            }
            Price = Price + Math.Exp(-r) * Payoff.Average();
            return Price;
        }

    }



}



