using System;
using DataBase;
using Microsoft.EntityFrameworkCore;

namespace LauncherManagement
{
    public class Launcher
    {
        public string Code { get; }
        public string Location { get; }
        public string MissileType { get; }

        public event Action<string> MalfunctionOccurred;

        public Launcher(string code, string location, string missileType)
        {
            Code = code;
            Location = location;
            MissileType = missileType;
        }

        public void AlertMalfunction()
        {
            Console.WriteLine($"🚨 Malfunction detected in launcher {Code} at {Location}!");
            MalfunctionOccurred?.Invoke(Code);
        }

        public void RegisterNewLauncher()
        {
            using (var db = new MissileDbContext())
            {
                // Check if the launcher is already in the DB
                if (!db.MissileLaunchers.Any(l => l.Code == Code))
                {
                    db.MissileLaunchers.Add(new MissileLauncher
                    {
                        Code = Code,
                        Location = Location,
                        MissileType = MissileType,
                        MissileCount = 10, //default
                        FailureCount = 0,
                        FixedFailures = 0
                    });
                    db.SaveChanges();
                    Console.WriteLine($"✅ Launcher {Code} added to the database.");
                }
                else
                {
                    Console.WriteLine($"⚠️ Launcher {Code} already exists in the database.");
                }
            }
        }
    }
}
