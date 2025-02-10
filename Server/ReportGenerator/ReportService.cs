using System;
using System.Linq;
using System.Collections.Generic;
using DataBase;
using Microsoft.EntityFrameworkCore;

namespace ReportGenerator
{
    public class ReportService 
        {
            public void GetAllLaunchers()
            {
                using (var db = new MissileDbContext()) 
                {
                    var launchers = db.MissileLaunchers.ToList();
                    Console.WriteLine("\n🔹 All Missile Launchers:");
                    foreach (var launcher in launchers)
                    {
                        Console.WriteLine($"ID: {launcher.Id}, Code: {launcher.Code}, Location: {launcher.Location}, " +
                                        $"Missile Type: {launcher.MissileType}, Count: {launcher.MissileCount}, " +
                                        $"Failures: {launcher.FailureCount}, Fixed: {launcher.FixedFailures}");
                    }
                }
            }

            public void GetLaunchersWithFailures()
            {
                using (var db = new MissileDbContext()) 
                {
                    var failedLaunchers = db.MissileLaunchers
                        .Where(l => l.FailureCount > 0)
                        .OrderByDescending(l => l.FailureCount)
                        .ToList();

                    Console.WriteLine("\n🔹 Launchers with Failures:");
                    foreach (var launcher in failedLaunchers)
                    {
                        Console.WriteLine($"ID: {launcher.Id}, Code: {launcher.Code}, Failures: {launcher.FailureCount}");
                    }
                }
            }

            public void GetMissileStockSummary()
            {
                using (var db = new MissileDbContext()) 
                {
                    var stockSummary = db.MissileLaunchers
                        .GroupBy(l => l.MissileType)
                        .Select(g => new
                        {
                            MissileType = g.Key,
                            TotalMissiles = g.Sum(l => l.MissileCount)
                        })
                        .OrderByDescending(x => x.TotalMissiles)
                        .ToList();

                    Console.WriteLine("\n🔹 Missile Stock Summary:");
                    foreach (var stock in stockSummary)
                    {
                        Console.WriteLine($"Missile Type: {stock.MissileType}, Total Missiles: {stock.TotalMissiles}");
                    }
                }
            }
        }
}
