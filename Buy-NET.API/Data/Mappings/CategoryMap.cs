using Buy_NET.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Buy_NET.API.Domain.Mappings;

public class CategoryMap : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories")
        .HasKey(p => p.Id);

        builder.Property(p => p.Name)
        .HasColumnType("VARCHAR")
        .IsRequired();

        builder.Property(p => p.Description)
        .HasColumnType("VARCHAR");

        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(c => c.CategoryId);
        
        builder.Property(u => u.RegistrationDate)
        .HasColumnType("timestamp")
        .IsRequired();
        
        builder.Property(u => u.InactivationDate)
        .HasColumnType("timestamp");
    }
}