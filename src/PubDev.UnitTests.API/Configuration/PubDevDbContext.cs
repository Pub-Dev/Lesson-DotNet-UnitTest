using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace PubDev.UnitTests.API.Configuration;

[ExcludeFromCodeCoverage]
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
