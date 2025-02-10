using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class MissileDbContext : DbContext
    {
        public DbSet<MissileLauncher> MissileLaunchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=C:\\Users\\student\\Downloads\\repos\\Server\\SQLDatabase\\missiles.db");
            }
        }
    }
}
