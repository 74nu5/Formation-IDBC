namespace App.Framework.Console
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class Program
    {
        private static async Task Main()
        {
            var nombreTestsMax = int.Parse(ConfigurationManager.AppSettings["NombreTestsMax"]);
            var urlToTest = ConfigurationManager.AppSettings["UrlToTest"];
            var httpClient = new HttpClient();

            await Parallel(nombreTestsMax, urlToTest);
            await Sequenciel(nombreTestsMax, httpClient, urlToTest);
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

        private static async Task Parallel(int nombreTestsMax, string urlToTest)
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
}
