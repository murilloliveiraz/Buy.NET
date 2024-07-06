using Buy_NET.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Buy_NET.API.Domain.Mappings;

public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("orderitems")
        .HasKey(i => i.Id);
        
        builder.Property(i => i.Quantity)
        .HasColumnType("INTEGER")
        .IsRequired();

        builder.HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId);
        
        builder.HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId);
    }
}