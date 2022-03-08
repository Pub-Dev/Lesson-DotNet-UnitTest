using PubDev.UnitTests.API.Interfaces.Repositories;
using PubDev.UnitTests.API.Repositories;

namespace PubDev.UnitTests.API.Providers;

public static class RepositoriesConfiguration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();

        return services;
    }
}
