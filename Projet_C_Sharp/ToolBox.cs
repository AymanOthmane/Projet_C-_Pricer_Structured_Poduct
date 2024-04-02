using System;

class NormalDistributionGenerator
{
    private Random random;

    public NormalDistributionGenerator()
    {
        // Initialize the Random object
        random = new Random();
    }

    // Generate a single random sample from a normal distribution with mean mu and standard deviation sigma
    public double Generate(double mu, double sigma)
    {
        double u1 = 1.0 - random.NextDouble(); // Uniform random number between 0 and 1
        double u2 = 1.0 - random.NextDouble(); // Uniform random number between 0 and 1

        // Box-Muller transform to generate a normally distributed random number
        double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

        // Apply mean and standard deviation
        return mu + sigma * z;
    }
}

class Programs
{
    static void Main(string[] args)
    {
        // Example usage
        NormalDistributionGenerator generator = new NormalDistributionGenerator();
        double mu = 0; // Mean
        double sigma = 1; // Standard deviation

        // Generate a single random sample from the normal distribution
        double sample = generator.Generate(mu, sigma);
        Console.WriteLine("Generated sample: " + sample);
    }
}
