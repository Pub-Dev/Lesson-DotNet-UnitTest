using PubDev.Store.API.Interfaces.Repositories;
using PubDev.Store.API.Repositories;

namespace PubDev.Store.API.Providers;

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
