using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class MissileDbContext : DbContext
    {
        public DbSet<MissileLauncher> MissileLaunchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=missiles.db"); // SQLite database file
    }
}