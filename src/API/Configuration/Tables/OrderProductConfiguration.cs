using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PubDev.Store.API.Entities;

namespace PubDev.Store.API.Configuration.Tables;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("OrderProduct");

        builder
            .HasKey(e => e.OrderProductId)
            .HasName("PK_OrderProduct");

        builder
            .Property(e => e.OrderProductId)
            .ValueGeneratedOnAdd();

        builder
            .Property(e => e.OrderId)
            .IsRequired();

        builder
            .Property(e => e.ProductId)
            .IsRequired();

        builder
            .Property(e => e.Quantity)
            .IsRequired();

        builder
           .Property(e => e.CreateDate)
           .HasDefaultValueSql("GETUTCDATE()")
           .IsRequired();

        builder
            .HasOne(e => e.Order)
            .WithMany(e => e.OrderProducts)
            .HasForeignKey(e => e.OrderId)
            .HasConstraintName("FK_OrderProduct_Order_OrderId");

        builder
            .HasOne(e => e.Product)
            .WithMany(e => e.OrderProducts)
            .HasForeignKey(e => e.ProductId)
            .HasConstraintName("FK_OrderProduct_Product_ProductId");

        builder
            .HasIndex(x => new { x.OrderId, x.ProductId })
            .IsUnique();
    }
}