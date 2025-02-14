using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using DataBase;

namespace LauncherManagement
{
    public class LauncherPoller
    {
        private readonly LauncherListener _listener;
        private readonly HashSet<string> _trackedLaunchers = new();
        private bool _isPolling;
        private Task _pollingTask;
        private readonly int _pollingIntervalMs = 500;
         private readonly DatabaseHandler _dbHandler; 

        public LauncherPoller(LauncherListener listener, DatabaseHandler dbHandler)
        {
            _listener = listener ?? throw new ArgumentNullException(nameof(listener));
            _dbHandler = dbHandler ?? throw new ArgumentNullException(nameof(dbHandler));
        }

        public void StartPolling()
        {
            if (_isPolling) return; // Prevent multiple instances

            _isPolling = true;
            _pollingTask = Task.Run(() => PollLaunchers());
            Console.WriteLine("📡 Launcher polling started...");
        }

        public void StopPolling()
        {
            _isPolling = false;
            _pollingTask?.Wait();
            Console.WriteLine("📡 Launcher polling stopped.");
        }

        private void PollLaunchers()
        {
            while (_isPolling)
            {
                try
                {
                    FetchAndRegisterLaunchers();
                    Thread.Sleep(_pollingIntervalMs); // Wait before polling again
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Polling error: {ex.Message}");
                }
            }
        }

        private void FetchAndRegisterLaunchers()
        {
            using (var db = new MissileDbContext())
            {
                var newLaunchers = db.MissileLaunchers
                    .Select(l => new Launcher(l.Code, l.Location, l.MissileType, _dbHandler))
                    .ToList();

                foreach (var launcher in newLaunchers)
                {
                    if (!_trackedLaunchers.Contains(launcher.Code)) 
                    {
                        _trackedLaunchers.Add(launcher.Code);
                        _listener.RegisterLauncher(launcher);

                        Task.Delay(500).Wait();
                        Console.WriteLine($"📡 New launcher registered: {launcher.Code}");
                    }
                }
            }
        }


        public void RemoveLauncher(string launcherCode)
        {
            using (var db = new MissileDbContext())
            {
                // Find the launcher by its code
                var launcher = db.MissileLaunchers.FirstOrDefault(l => l.Code == launcherCode);

                if (launcher != null)
                {
                    // Remove the launcher from the DbSet
                    db.MissileLaunchers.Remove(launcher);
                    // Save changes to the database
                    db.SaveChanges();

                    Console.WriteLine($"✅ Launcher {launcherCode} removed from the database.");
                }
                else
                {
                    Console.WriteLine($"⚠️ Launcher with code {launcherCode} not found in the database.");
                }
            }
        }

    }
}
