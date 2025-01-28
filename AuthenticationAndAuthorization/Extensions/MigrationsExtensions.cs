using AuthenticationAndAuthorization.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAndAuthorization.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
