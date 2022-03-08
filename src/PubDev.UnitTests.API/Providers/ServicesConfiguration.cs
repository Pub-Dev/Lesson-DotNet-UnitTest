using PubDev.UnitTests.API.Interfaces.Services;
using PubDev.UnitTests.API.Services;

namespace PubDev.UnitTests.API.Providers;

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
