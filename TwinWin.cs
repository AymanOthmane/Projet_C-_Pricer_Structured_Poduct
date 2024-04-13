using System;

namespace DerivativesProduct
{
    public class TwinWin
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

        public TwinWin(double[] S, double K, double r, double div, double T, double vol, double barrier, bool isCall, bool isLong)
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
        public double TwinWinOption()
        {
            double payoff = 0;

             
            for (int i = 0; i < S.Length; i++)
            {
                // Whether a Knock-out Event has occurred or not, if Stock(t=T) is greater than or equal to Stock(t=0)
                if (S[i] <= barrier * S[0])
                {
                    payoff = S[S.Length - 1] / S[0];
                    break; 
                }

                // Whether a Knock-out Event has not occurred and Stock(t=T) is less than Stock(t=0)
                if (S[i] > barrier * S[0] && S[S.Length - 1] < S[0])
                {

                    payoff =  1 + (S[S.Length - 1] / S[0]);
                    break;
                }

                // If a Knock-out Event has occurred and Stock(t=T) is less than Stock(t=0) 
                if (S[i] <= barrier * S[0] && S[S.Length - 1] < S[0])
                {

                    payoff = 1 + (S[S.Length - 1] / S[0]);
                    break;
                }
            }
            return payoff;
        }

        
    }
}
