// See https://aka.ms/new-console-template for more information

using App.Formation.Benchmark;

using BenchmarkDotNet.Running;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<Md5VsSha256>();
    }
}
