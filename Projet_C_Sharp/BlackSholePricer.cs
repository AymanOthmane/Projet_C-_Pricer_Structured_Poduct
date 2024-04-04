

// using System;
// using System.Linq;
// using MathNet.Numerics.Distributions;

// class Program1
// {
//     static double PriceOptionKI(double S, double K, double B, double r, double div, double T, double vol, double n, bool isCall)
//     {
//         double[] simulations = n;
//         var z = Normal.Samples(simulations).ToArray();
//         var ST = z.Select(zValue => S * Math.Exp(((r - div) - 0.5 * vol * vol) * T + vol * Math.Sqrt(T) * zValue));

//         double[] payoff;
//         if (isCall)
//             payoff = ST.Select(stValue => Math.Max(stValue - K, 0)).ToArray();
//         else
//             payoff = ST.Select(stValue => Math.Max(K - stValue, 0)).ToArray();

//         for (int i = 0; i < simulations; i++)
//         {
//             if (payoff[i] < B)
//                 payoff[i] = 0;
//         }

//         var price = payoff.Select(payoffValue => Math.Exp(-r * T) * payoffValue).Average();
//         return price;
//     }

//     static double PriceBSOption(double S, double K, double r, double div, double T, double vol, bool isCall)
//     {
//         var d1 = (Math.Log(S / K) + (r - div + 0.5 * vol * vol) * T) / (vol * Math.Sqrt(T));
//         var d2 = d1 - vol * Math.Sqrt(T);

//         double price;
//         if (isCall)
//             price = S * Math.Exp(-div * T) * Normal.CDF(0, 1, d1) - K * Math.Exp(-r * T) * Normal.CDF(0, 1, d2);
//         else
//             price = K * Math.Exp(-r * T) * Normal.CDF(0, 1, -d2) - S * Math.Exp(-div * T) * Normal.CDF(0, 1, -d1);

//         return price;
//     }

//     static void Main()
//     {
//         double n = 100000; // Number of simulations
//         double T = 1;    // Time to maturity
//         double K = 100;  // Strike price
//         double B = 10;   // Barrier price

//         // Define parameters for each underlying
//         double S_US = 100;  // Current stock price
//         double r_US = 0.05; // Risk-free rate
//         double vol_US = 0.13;  // Volatility
//         double div_US = 0.019; // Dividend yield

//         double S_EU = 100;  // Current stock price
//         double r_EU = 0.045; // Risk-free rate
//         double vol_EU = 0.15;  // Volatility
//         double div_EU = 0.0296; // Dividend yield

//         Console.WriteLine(PriceOptionKI(S_US, K, B, r_US, div_US, T, vol_US, n, true));
//         Console.WriteLine(PriceBSOption(100, 100, 0.045, 0.0296, 1, 0.14, true));
//     }
// }
