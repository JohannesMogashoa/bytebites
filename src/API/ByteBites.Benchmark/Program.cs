
using ByteBites.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        // This is the entry point for the benchmark application.
        // The benchmarks will be executed when the application runs.
        BenchmarkDotNet.Running.BenchmarkRunner.Run<RecipeApiBenchmark>();
    }
}