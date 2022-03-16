using PubDev.Store.API.Interfaces.Services;
using PubDev.Store.API.Services;

namespace PubDev.Store.API.Providers;

public static class ServicesConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
