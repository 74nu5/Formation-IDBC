namespace BlazorApp.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.TryAddTransient<AddressesApiService>();

        _ = services.AddHttpClient("Addresses",
                                   (provider, httpClient) =>
                                   {
                                       var configuration = provider.GetRequiredService<IConfiguration>();
                                       var localApi = new Uri(configuration["Api:Url"]);
                                       httpClient.BaseAddress = new(localApi, "api/Addresses/");
                                       httpClient.DefaultRequestHeaders.Add("X-LOGGING", string.Empty);
                                   });

        return services;
    }
}
