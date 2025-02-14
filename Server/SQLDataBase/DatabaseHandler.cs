using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataBase;

namespace DataBase
{
    public class DatabaseHandler
    {
         private readonly MissileDbContext _dbContext;

        public DatabaseHandler(MissileDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        
        public MissileLauncher GetLauncherByCode(string launcherCode)
        {
            return _dbContext.MissileLaunchers.FirstOrDefault(l => l.Code == launcherCode);
        }

        public void AddNewLauncher(string code, string location, string missileType)
        {
            _dbContext.MissileLaunchers.Add(new MissileLauncher
            {
                Code = code,
                Location = location,
                MissileType = missileType,
                MissileCount = 10,  // default
                FailureCount = 0,
                FixedFailures = 0
            });
            _dbContext.SaveChanges();
        }
        
        public void UpdateLauncherLocation(string launcherCode, string info)
        {
            var launcher = _dbContext.MissileLaunchers.FirstOrDefault(l => l.Code == launcherCode);
            if (launcher == null) {
                throw new Exception($"Launcher with ID {launcherCode} not found.");
            }
            launcher.Location = info;
            _dbContext.SaveChanges();
        }

        public void UpdateMissileCount(string launcherCode, int missilesFired)
        {
            var launcher = _dbContext.MissileLaunchers.FirstOrDefault(l => l.Code == launcherCode);
            if (launcher == null){
                throw new Exception($"Launcher with ID {launcherCode} not found.");
            }
            launcher.MissileCount = Math.Max(0, launcher.MissileCount - missilesFired); // Ensure count is non-negative
            _dbContext.SaveChanges();

        }

        public void IncrementFailureCount(string launcherCode)
        {
            var launcher = _dbContext.MissileLaunchers.FirstOrDefault(l => l.Code == launcherCode);
            if (launcher == null){
                throw new Exception($"Launcher with ID {launcherCode} not found.");
            }
            launcher.FailureCount += 1;
            _dbContext.SaveChanges();
        }

        public void IncrementFixedCount(string launcherCode)
        {
            var launcher = _dbContext.MissileLaunchers.FirstOrDefault(l => l.Code == launcherCode);
            if (launcher == null){
                throw new Exception($"Launcher with ID {launcherCode} not found.");
            }
            launcher.FixedFailures += 1;
            _dbContext.SaveChanges();
        }
    }
}
