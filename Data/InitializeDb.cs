namespace Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class InitializeDb : IHostedService
{
    private readonly IServiceProvider provider;

    public InitializeDb(IServiceProvider provider)
        => this.provider = provider;

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = this.provider.CreateAsyncScope();
        await using var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        _ = await dataContext.Database.EnsureCreatedAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task StopAsync(CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
