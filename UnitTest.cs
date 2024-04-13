using NUnit.Framework;
using DataRetrieval;
using System;

[TestFixture]
public class ProgramTests
{
    private readonly string filePath = @"C:\Users\carri\OneDrive - Université Paris-Dauphine\cours dauphine\M2\S1\C#\Connection Excel\Interface Excel.xlsx";

    [Test]
    public void TestNValueType()
    {
        var data = DataRetrieval.Program.GetDataFromExcel(filePath);
        Assert.That(data[0], Is.TypeOf<double>(), "Le type de N doit être double.");
    }

    [Test]
    public void TestFValueType()
    {
        var data = DataRetrieval.Program.GetDataFromExcel(filePath);
        Assert.That(data[1], Is.TypeOf<string>(), "Le type de f doit être string.");
    }

    [Test]
    public void TestMatValueType()
    {
        var data = DataRetrieval.Program.GetDataFromExcel(filePath);
        Assert.That(data[2], Is.TypeOf<DateTime>(), "Le type de mat doit être DateTime.");
    }

    [Test]
    public void TestAutocallValueType()
    {
        var data = DataRetrieval.Program.GetDataFromExcel(filePath);
        Assert.That(data[3], Is.TypeOf<bool>(), "Le type de autocall doit être bool.");
    }

    [Test]
    public void TestStrikeDateValueType()
    {
        var data = DataRetrieval.Program.GetDataFromExcel(filePath);
        Assert.That(data[4], Is.TypeOf<DateTime>(), "Le type de strike_date doit être DateTime.");
    }

    [Test]
    public void TestMaturityAndStrikeDate()
    {
        var data = DataRetrieval.Program.GetDataFromExcel(filePath);
        DateTime maturity = Convert.ToDateTime(data[3]);
        DateTime strikeDate = Convert.ToDateTime(data[4]);
        Assert.That(strikeDate, Is.LessThan(maturity), "La date de strike doit être antérieure à la maturité.");
    }
}
