

/*using DataBase;

using System;
using System.Linq;

class Program
{
    static void Main()
    {
        using (var db = new MissileDbContext())
        {
            db.Database.EnsureCreated(); // Create the database if it doesn't exist

            // Insert a new missile launcher if the database is empty
            if (!db.MissileLaunchers.Any())
            {
                db.MissileLaunchers.Add(new MissileLauncher
                {
                    Code = "ML-001",  // Unique Identifier
                    Location = "Base Alpha",
                    MissileType = "Tomahawk",
                    MissileCount = 10,
                    FailureCount = 0,
                    FixedFailures = 0
                });

                db.SaveChanges();
                Console.WriteLine("New missile launcher added.");
            }

            // Retrieve and display all missile launchers
            var launchers = db.MissileLaunchers.ToList();
            Console.WriteLine("\nMissile Launchers:");
            foreach (var launcher in launchers)
            {
                Console.WriteLine($"ID: {launcher.Id} | Code: {launcher.Code} | Location: {launcher.Location} | " +
                                  $"Missile Type: {launcher.MissileType} | Count: {launcher.MissileCount} | " +
                                  $"Failures: {launcher.FailureCount} | Fixed: {launcher.FixedFailures}");
            }
        }
    }
}
*/