using Microsoft.EntityFrameworkCore;

namespace PubDev.UnitTests.API.Configuration;

public class PubDevDbContext : DbContext
{
    public PubDevDbContext(DbContextOptions<PubDevDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PubDevDbContext).Assembly);
    }
}
