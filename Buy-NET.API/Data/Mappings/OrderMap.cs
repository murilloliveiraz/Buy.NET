using Buy_NET.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Buy_NET.API.Domain.Mappings;

public class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders")
        .HasKey(p => p.Id);
        
        builder.Property(u => u.OrderDate)
        .HasColumnType("timestamp")
        .IsRequired();

        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(o => o.OrderId);
    }
}