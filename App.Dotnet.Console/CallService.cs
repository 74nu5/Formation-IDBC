namespace App.Dotnet.Console;

using System.Diagnostics;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Console = System.Console;

internal class CallService
{
    private readonly HttpClient client;

    private readonly ILogger<CallService> logger;

    private readonly int nombreTestsMax;

    private readonly string urlToTest;

    public CallService(IHttpClientFactory factory, IConfiguration configuration, ILogger<CallService> logger)
    {
        this.client = factory.CreateClient();
        this.logger = logger;
        this.nombreTestsMax = configuration.GetValue("nombreTestsMax", 0);
        this.urlToTest = configuration.GetValue("urlToTest", "https://google.fr");
    }

    public async Task Sequenciel()
    {
        var tempsTotal = 0L;
        for (var i = 0; i < this.nombreTestsMax; i++)
        {
            var watch = Stopwatch.StartNew();
            var response = await this.client.GetAsync(new Uri(this.urlToTest)).ConfigureAwait(false);
            watch.Stop();
            _ = response.EnsureSuccessStatusCode();
            this.logger.LogInformation("{i}/ Http code = {StatusCode}", i, response.StatusCode);
            tempsTotal += watch.ElapsedMilliseconds;
            this.logger.LogInformation("{i}/ {ElapsedMilliseconds} ms", i, watch.ElapsedMilliseconds);
        }

        this.logger.LogInformation("Moyenne d'appel : {Moyenne} ms", tempsTotal / this.nombreTestsMax);
    }

    public async Task Parallel()
    {
        var tempsTotal = 0L;

        async Task<long> Selector(int i)
        {
            var watch = Stopwatch.StartNew();
            var response = await this.client.GetAsync(new Uri(this.urlToTest)).ConfigureAwait(false);
            watch.Stop();
            _ = response.EnsureSuccessStatusCode();
            this.logger.LogInformation("{i}/ Http code = {StatusCode}", i, response.StatusCode);
            _ = Interlocked.Add(ref tempsTotal, watch.ElapsedMilliseconds);
            this.logger.LogInformation("{i}/ {ElapsedMilliseconds} ms", i, watch.ElapsedMilliseconds);
            return watch.ElapsedMilliseconds;
        }

        var range = Enumerable.Range(0, this.nombreTestsMax);

        var tasks = range.Select(Selector);

        var results = await Task.WhenAll(tasks);
        this.logger.LogInformation("Moyenne d'appel : {Moyenne} ms", results.Average());
    }
}
