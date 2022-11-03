namespace Data;

using Data.AccessLayer;
using Data.AccessLayer.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddData(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsBuilder)
    {
        _ = services.AddDbContext<DataContext>(optionsBuilder);
        _ = services.AddHostedService<InitializeDb>();

        services.TryAddTransient<IAddressAccessLayer, AddressAccessLayer>();
        services.TryAddTransient<IPersonAccessLayer, PersonAccessLayer>();
        services.TryAddTransient<ICompanyAccessLayer, CompanyAccessLayer>();

        return services;
    }
}
