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
        private readonly DatabaseHandler _dbHandler;

        public event Action<string> MalfunctionOccurred;

        public Launcher(string code, string location, string missileType, DatabaseHandler dbHandler)
        {
            Code = code;
            Location = location;
            MissileType = missileType;
            _dbHandler = dbHandler ?? throw new ArgumentNullException(nameof(dbHandler));
        }

        public void AlertMalfunction()
        {
            Console.WriteLine($"🚨 Malfunction detected in launcher {Code} at {Location}!");
            MalfunctionOccurred?.Invoke(Code);

            // Update the malfunction count in the database
            _dbHandler.IncrementFailureCount(Code);
        }

        public void RegisterNewLauncher()
        {
            var existingLauncher = _dbHandler.GetLauncherByCode(Code);
            if (existingLauncher == null)
            {
                // Launcher does not exist, so add it
                _dbHandler.AddNewLauncher(Code, Location, MissileType); // Add the new launcher to DB
                Console.WriteLine($"✅ Launcher {Code} added to the database.");
            }
            else
            {
                // Launcher already exists
                Console.WriteLine($"⚠️ Launcher {Code} already exists in the database.");
            }
        }
    }
}
