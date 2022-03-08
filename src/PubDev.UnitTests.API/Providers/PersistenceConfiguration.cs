using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PubDev.UnitTests.API.Configuration;
using System.Data;

namespace PubDev.UnitTests.API.Providers;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PubDevDatabase");

        services.AddDbContextPool<PubDevDbContext>(options => options.UseSqlServer(connectionString));

        services.AddStartupTask<PubDevDbContext>((service, cancelationToken) =>
            service.Database.MigrateAsync(cancelationToken));

        services.AddScoped<IDbConnection>(x => new SqlConnection(connectionString));

        return services;
    }
}

