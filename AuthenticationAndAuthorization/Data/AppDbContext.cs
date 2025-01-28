using AuthenticationAndAuthorization.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAndAuthorization.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
