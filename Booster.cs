using System;

namespace DerivativesProduct
{
    public class Booster
    {

        private double[] S; // Current stock price
        private double K; // Strike price
        private double r; // Risk-free interest rate
        private double div; // Dividend yield
        private double c; // coupon asked by the investor (buyer of the financial product)
        private double T; // Time to maturity (in years)
        private double vol; // Volatility of the underlying asset
        private double barrier; // Barrier of the decrease price for the underlying asset  
        private bool isCall; // Flag indicating whether the option is a call (true) or a put (false)
        private bool isLong; // Flag indicating whether the option is long (true) or a short (false)

        public Booster(double[] S, double K, double r, double div, double T, double vol, double barrier, bool isCall, bool isLong)
        {
            this.S = S;
            this.K = K;
            this.r = r;
            this.div = div;
            this.c = c;
            this.T = T;
            this.vol = vol;
            this.barrier = barrier;
            this.isCall = isCall;
            this.isLong = isLong;
        }

        // Return the payoff depending on the product's conditions
        public double BoosterOption()
        {
            double payoff;

            // If Stock(t=T) is greater than or equal to Stock(t=0)
            if (S[S.Length - 1] >= S[0])
            {
                payoff = 1 + 1.5 + ((S[S.Length - 1] - S[0]) / S[0]);
            }
            // Else if no Knock-in Event has  occurred :
            else
            {
                payoff = S[S.Length - 1] / S[0];
            }

            return payoff;
        }

    }
}
