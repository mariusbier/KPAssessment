using Microsoft.EntityFrameworkCore;
using ServiceAPI.Models;

namespace ServiceAPI.DataContext
{
    public class DatabaseContext : DbContext
    {
        DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}
