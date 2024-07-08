using Buy_NET.API.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Buy_NET.API.Infrastructure.Data.Services
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationContext context =
                scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            context.Database.Migrate();
        }
    }
}