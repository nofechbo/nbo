using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataBase;

namespace DataBase
{
    public class DatabaseHandler
    {
        public void UpdateLauncherStatus(string launcherID, string info)
        {
            using (var db = new MissileDbContext())
            {
                var launcher = db.MissileLaunchers.FirstOrDefault(l => l.Code == launcherID);
                if (launcher == null)
                    throw new Exception($"Launcher with ID {launcherID} not found.");

                launcher.Location = info; // Assuming "info" contains a location update
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

                launcher.MissileCount = Math.Max(0, launcher.MissileCount - missilesFired); // Ensure count is non-negative
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
    }
}
