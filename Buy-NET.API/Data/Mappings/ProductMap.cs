using Buy_NET.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Buy_NET.API.Domain.Mappings;

public class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products")
        .HasKey(p => p.Id);

        builder.Property(p => p.Name)
        .HasColumnType("VARCHAR")
        .IsRequired();

        builder.Property(p => p.Description)
        .HasColumnType("VARCHAR");
        
        builder.Property(p => p.Price)
        .HasColumnType("NUMERIC")
        .IsRequired();
        
        builder.Property(p => p.StockQuantity)
        .HasColumnType("integer")
        .IsRequired();

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();
        
        builder.Property(u => u.RegistrationDate)
        .HasColumnType("timestamp")
        .IsRequired();
        
        builder.Property(u => u.InactivationDate)
        .HasColumnType("timestamp");
    }
}