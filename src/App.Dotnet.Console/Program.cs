namespace App.Dotnet.Console;

using System.Diagnostics;

using Microsoft.Extensions.Configuration;

using Console = System.Console;

internal static class Program
{
    private static async Task Main()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var nombreTestsMax = configuration.GetValue("NombreTestsMax", 10);
        var urlToTest = configuration.GetValue("UrlToTest", "localhost");


        var httpClient = new HttpClient();

        await Parallele(nombreTestsMax, urlToTest!);
        await Sequenciel(nombreTestsMax, httpClient, urlToTest!);
        _ = Console.ReadLine();
    }

    private static async Task Sequenciel(int nombreTestsMax, HttpClient httpClient, string urlToTest)
    {
        var tempsTotal = 0L;
        for (var i = 0; i < nombreTestsMax; i++)
        {
            var watch = Stopwatch.StartNew();
            var response = await httpClient.GetAsync(new Uri(urlToTest)).ConfigureAwait(false);
            watch.Stop();
            _ = response.EnsureSuccessStatusCode();
            Console.WriteLine($"{i}/ Http code = {response.StatusCode}");
            tempsTotal += watch.ElapsedMilliseconds;
            Console.WriteLine($"{i}/ {watch.ElapsedMilliseconds} ms");
        }

        Console.WriteLine($"Moyenne d'appel : {tempsTotal / nombreTestsMax} ms");
    }

    private static async Task Parallele(int nombreTestsMax, string urlToTest)
    {
        var tempsTotal = 0L;
        var httpClient = new HttpClient();

        async Task<long> Selector(int i)
        {
            var watch = Stopwatch.StartNew();
            var response = await httpClient.GetAsync(new Uri(urlToTest)).ConfigureAwait(false);
            watch.Stop();
            _ = response.EnsureSuccessStatusCode();
            Console.WriteLine($"{i}/ Http code = {response.StatusCode}");
            _ = Interlocked.Add(ref tempsTotal, watch.ElapsedMilliseconds);
            Console.WriteLine($"{i}/ {watch.ElapsedMilliseconds} ms");
            return watch.ElapsedMilliseconds;
        }

        var range = Enumerable.Range(0, nombreTestsMax);


        var tasks = range.Select(Selector);

        var results = await Task.WhenAll(tasks);
        Console.WriteLine($"Moyenne d'appel : {results.Average()} ms");
    }
}


