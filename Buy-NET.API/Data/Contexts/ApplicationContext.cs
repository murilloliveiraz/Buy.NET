using Buy_NET.API.Domain.Mappings;
using Buy_NET.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Buy_NET.API.Data.Contexts;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options){}

    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMap());
    }
    
}