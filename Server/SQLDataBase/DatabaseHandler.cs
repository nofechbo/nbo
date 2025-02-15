using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataBase;

namespace DataBase
{
    public class DatabaseHandler
    {
        public void UpdateLauncherLocation(string launcherID, string info)
        {
            using (var db = new MissileDbContext())
            {
                var launcher = db.MissileLaunchers.FirstOrDefault(l => l.Code == launcherID);
                if (launcher == null)
                    throw new Exception($"Launcher with ID {launcherID} not found.");

                launcher.Location = info;
                db.SaveChanges();
            }
        }

        public void UpdateMissileCount(string launcherID, int missilesFired)
        {
            using (var db = new MissileDbContext())
            {
                var launcher = db.MissileLaunchers.FirstOrDefault(l => l.Code == launcherID);
                if (launcher == null)
                    throw new Exception($"Launcher with ID {launcherID} not found.");

                launcher.MissileCount = Math.Max(0, launcher.MissileCount - missilesFired);
                db.SaveChanges();
            }
        }

        public void IncrementFailureCount(string launcherID)
        {
            using (var db = new MissileDbContext())
            {
                var launcher = db.MissileLaunchers.FirstOrDefault(l => l.Code == launcherID);
                if (launcher == null)
                    throw new Exception($"Launcher with ID {launcherID} not found.");

                launcher.FailureCount += 1;
                db.SaveChanges();
            }
        }

        public void IncrementFixedCount(string launcherID)
        {
            using (var db = new MissileDbContext())
            {
                var launcher = db.MissileLaunchers.FirstOrDefault(l => l.Code == launcherID);
                if (launcher == null)
                    throw new Exception($"Launcher with ID {launcherID} not found.");

                launcher.FixedFailures += 1;
                db.SaveChanges();
            }
        }

        // New method to get a launcher by its code
        public MissileLauncher? GetLauncherByCode(string launcherCode)
        {
            using (var db = new MissileDbContext())
            {
                return db.MissileLaunchers.FirstOrDefault(l => l.Code == launcherCode);
            }
        }

        // New method to add a new launcher to the database
        public void AddNewLauncher(string code, string location, string missileType)
        {
            using (var db = new MissileDbContext())
            {
                db.MissileLaunchers.Add(new MissileLauncher
                {
                    Code = code,
                    Location = location,
                    MissileType = missileType,
                    MissileCount = 10,  // default
                    FailureCount = 0,
                    FixedFailures = 0
                });
                db.SaveChanges();
            }
        }
    }

}
