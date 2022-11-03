using App.Dotnet.Console;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices(services =>
{
    services.TryAddTransient<CallService>();
    _ = services.AddHttpClient();
});

using var host = builder.Build();

var callService = host.Services.GetRequiredService<CallService>();

await callService.Parallel();
await callService.Sequenciel();
await host.RunAsync();
