using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PubDev.UnitTests.API.Entities;

namespace PubDev.UnitTests.API.Configuration.Tables;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .ToTable("Order");

        builder
            .HasKey(e => e.OrderId)
            .HasName("PK_Order");

        builder
            .Property(e => e.OrderId)
            .ValueGeneratedOnAdd();

        builder
            .Property(e => e.ClientId)
            .IsRequired();

        builder
            .Property(e => e.CreateDate)
            .HasDefaultValueSql("GETUTCDATE()")
            .IsRequired();

        builder
            .HasOne(e => e.Client)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.ClientId)
            .HasConstraintName("FK_Order_Client_ClientId");
    }
}
