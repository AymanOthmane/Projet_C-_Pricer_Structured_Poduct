using System;
using Accord.Statistics.Distributions.Fitting;
using Accord.Statistics.Distributions.Univariate;
using MathNet.Numerics.Distributions;
using ToolBox;

namespace Autocall{


    public class Athena{
        
    private double S; // Current stock price
    private double K; // Strike price
    private double r; // Risk-free interest rate
    private double div; // Dividend yield
    private double T; // Time to maturity (in years)
    private double vol; // Volatility of the underlying asset
    private double coupon; //Coupon of product
    private bool frequency;
    
    public Athena(double S, double K, double r, double div, double T, double vol, double coupon, bool frequency)
    {
        this.S = S;
        this.K = K;
        this.r = r;
        this.div = div;
        this.T = T;
        this.vol = vol;
        this.coupon = coupon;
        this.frequency = frequency;

        double[][] ST = Generate_Paths.StockPaths(S, r, vol, dt, T/dt, 0, dt);  //double S0, double r, double vol, double dt, int nb_Simulations, int nb_steps, double mean_dist, double stdev_dist)



    } 
    }
}