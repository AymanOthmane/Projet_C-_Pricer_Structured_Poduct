


using System;
using System.Globalization;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DerivativesProduct
{
    public class Time
    {

        private DateTime startDate; // Start date 
        private double[] S; // Current stock price
        private double K; // Strike price
        private double r; // Risk-free interest rate
        private double div; // Dividend yield
        private double c; // coupon asked by the investor (buyer of the financial product)
        private int T; // Time to maturity (in years)
        private double vol; // Volatility of the underlying asset
        private double barrier; // Barrier of the decrease price for the underlying asset
        private bool freq;
        private bool isCall; // Flag indicating whether the option is a call (true) or a put (false)
        private bool isLong; // Flag indicating whether the option is long (true) or a short (false)

        public Time(DateTime startDate, double[] S, double K, double r, double div, int T, double vol, double barrier, bool isCall, bool isLong)
        {
            this.startDate = startDate;
            this.S = S;
            this.K = K;
            this.r = r;
            this.div = div;
            this.T = T;
            this.vol = vol;
            this.barrier = barrier;
            this.isCall = isCall;
            this.isLong = isLong;
        }

        // Return the end date of the financial product with its maturity
        public DateTime GetEndDate()
        {
            return startDate.AddYears(T);
        }

        // Return a boolean (True/False) rather the date entered is a business day/ weekday (true) or not (false)
        public bool IsBusinessDay(DateTime date)
        {
            return !date.DayOfWeek.Equals(DayOfWeek.Saturday) && !date.DayOfWeek.Equals(DayOfWeek.Sunday);
        }

        // Return the next business day of a date
        public DateTime GetNextBusinessDay(DateTime date)
        {
            while (!IsBusinessDay(date))
            {
                date = date.AddDays(1);
            }

            return date;
        }

        public DateTime GetEndBusinessDate()
        {
            DateTime enddate;

            enddate = GetEndDate();
            enddate = GetNextBusinessDay(enddate);


            return enddate;
        }

        // Return the number of business days between 2 dates
        public int GetNumberOfBusinessDays(DateTime startDate, DateTime endDate)
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

        // Return the number of regular years based on a number of days
        public int GetNumberOfBusinessYears(int days)
        {
            return days / 365;
        }

        // Return the time variation considering if the year is a leap year (253 business days) or not (252 business days)
        public double TimeVariation(DateTime date)
        {
            double dt;

            if (DateTime.IsLeapYear(date.Year))
            {
                dt = 1.0 / 253.0;
            }
            else
            {
                dt = 1.0 / 252.0;
            }

            return dt;
        }

        // Return a list of coupons' calculation dates
        public List<DateTime> CalculationDate()
        {
            List<DateTime> datesList = new List<DateTime>();
            DateTime date = startDate;
            int t = 1;

            if (freq)
            {
                while (t < T)
                {
                    date = date.AddYears(1);
                    if (IsBusinessDay(date) == false)
                    {
                        date = GetNextBusinessDay(date);
                    }
                    datesList.Add(date);
                    t++;
                }
            }
            else
            {
                while (t < T)
                {
                    date = date.AddMonths(3);
                    if (IsBusinessDay(date) == false)
                    {
                        date = GetNextBusinessDay(date);
                    }
                    datesList.Add(date);
                    t++;
                }
            }

            return datesList;
        }

        // Return the column's index of coupon payoff from its date (annualy or quarterly)
        public List<int> ReturnDateFromIndex(List<DateTime> datesList)
        {
            List<int> indexList = new List<int>();
            int dist;

            foreach (DateTime date in datesList)
            {
                dist = GetNumberOfBusinessDays(startDate, date);
                indexList.Add(dist);
            }
            return indexList;
        }

        static void Main(string[] args)
        {
            DateTime startDate = new DateTime(2024, 4, 1);
            double[] stockPrices = { 100, 105, 110, 108, 102 };
            double strikePrice = 100;
            double riskFreeRate = 0.05;
            double dividendYield = 0.02;
            int timeToMaturity = 5; // Exemple de temps de maturité en années
            double volatility = 0.2;
            double barrier = 95;
            bool isCall = true; // Long call option
            bool isLong = true; // Long position
            bool freq = true; // Exemple de fréquence annuelle

            Time time = new Time(startDate, stockPrices, strikePrice, riskFreeRate, dividendYield, timeToMaturity, volatility, barrier, isCall, isLong);

            List<DateTime> calculationDates = time.CalculationDate();
            List<int> calculationIndex = time.ReturnDateFromIndex(calculationDates);

            // Affichage des dates de calcul
            foreach (DateTime date in calculationDates)
            {
                Console.WriteLine(date.ToString("yyyy-MM-dd"));
            }

            foreach (int ind in calculationIndex)
            {
                Console.WriteLine(ind);
            }
        }

    }

}