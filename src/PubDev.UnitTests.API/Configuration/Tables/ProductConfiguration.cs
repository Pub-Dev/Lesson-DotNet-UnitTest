using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Configuration.Tables;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .ToTable("Product");

        builder
            .HasKey(e => e.ProductId)
            .HasName("PK_Product");

        builder
            .Property(e => e.ProductId)
            .ValueGeneratedOnAdd();

        builder
            .Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(e => e.Value)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder
            .Property(e => e.CreateDate)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();
    }
}
