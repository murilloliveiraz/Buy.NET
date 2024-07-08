using Buy_NET.API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Buy_NET.API.Domain.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users")
        .HasKey(u => u.Id);

        builder.Property(u => u.Email)
        .HasColumnType("VARCHAR")
        .IsRequired();

        builder.Property(u => u.Password)
        .HasColumnType("VARCHAR")
        .IsRequired();
        
        builder.Property(u => u.Password)
        .HasColumnType("VARCHAR")
        .IsRequired();
        
        builder.Property(u => u.Role)
        .HasColumnType("VARCHAR");
        
        builder.Property(u => u.RegistrationDate)
        .HasColumnType("timestamp")
        .IsRequired();
        
        builder.Property(u => u.InactivationDate)
        .HasColumnType("timestamp");
    }
}