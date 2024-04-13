// page 21 pour if
// loop page 70


using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    // public static void Main(string[] args)
    // {
    //     DateTime startDate = DateTime.Parse("2018-10-08");
    //     int years = 6;

    //     DateTime endDate = GetEndDate(startDate, years);

    //     DateTime nextBusinessDay = GetNextBusinessDay(endDate);

    //     bool isBusinessDay = IsBusinessDay(endDate);

    //     int businessDays = GetNumberOfBusinessDays(startDate, nextBusinessDay);

    //     var businessYears = GetNumberOfBusinessYears(businessDays);

    //     double dt = TimeVariation(endDate);


    //     Console.WriteLine($"Date de début: {startDate.ToString("dd/MM/yyyy")}");
    //     Console.WriteLine($"Jour ouvrable: {isBusinessDay}");
    //     Console.WriteLine($"Prochain jour ouvrable: {nextBusinessDay.ToString("dd/MM/yyyy")}");
    //     Console.WriteLine(endDate - startDate);
    //     Console.WriteLine($"Nombres de jours ouvrables: {businessDays}");
    //     Console.WriteLine($"Nombres d'années ouvrables: {businessYears}");
    //     Console.WriteLine($"dt: {string.Format("{0:0.00000000}", dt)}");
    // }
}
