using System;
using System.Linq;
using System.Collections.Generic;
using DataBase;
using Microsoft.EntityFrameworkCore;

public class ReportService
{
    private readonly DbContextOptions<MissileDbContext> _options;

    public ReportService()
    {
        var optionsBuilder = new DbContextOptionsBuilder<MissileDbContext>();
        optionsBuilder.UseSqlite("Data Source=missiles.db");
        _options = optionsBuilder.Options;
    }

    public void GetAllLaunchers()
    {
        try
        {
            using (var db = new MissileDbContext(_options))
            {
                var launchers = db.MissileLaunchers.ToList();

                if (!launchers.Any())
                {
                    Console.WriteLine("\n🔹 No missile launchers found.");
                    return;
                }

                Console.WriteLine("\n🔹 All Missile Launchers:");
                foreach (var launcher in launchers)
                {
                    Console.WriteLine($"ID: {launcher.Id}, Code: {launcher.Code}, Location: {launcher.Location}, " +
                                      $"Missile Type: {launcher.MissileType}, Count: {launcher.MissileCount}, " +
                                      $"Failures: {launcher.FailureCount}, Fixed: {launcher.FixedFailures}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving missile launchers: {ex.Message}");
        }
    }

    public void GetLaunchersWithFailures()
    {
        try
        {
            using (var db = new MissileDbContext(_options))
            {
                var failedLaunchers = db.MissileLaunchers
                    .Where(l => l.FailureCount > 0)
                    .OrderByDescending(l => l.FailureCount)
                    .ToList();

                if (!failedLaunchers.Any())
                {
                    Console.WriteLine("\n🔹 No failed launchers found.");
                    return;
                }

                Console.WriteLine("\n🔹 Launchers with Failures:");
                foreach (var launcher in failedLaunchers)
                {
                    Console.WriteLine($"ID: {launcher.Id}, Code: {launcher.Code}, Failures: {launcher.FailureCount}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving failed launchers: {ex.Message}");
        }
    }

    public void GetMissileStockSummary()
    {
        try
        {
            using (var db = new MissileDbContext(_options))
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

                if (!stockSummary.Any())
                {
                    Console.WriteLine("\n🔹 No missile stock data found.");
                    return;
                }

                Console.WriteLine("\n🔹 Missile Stock Summary:");
                foreach (var stock in stockSummary)
                {
                    Console.WriteLine($"Missile Type: {stock.MissileType}, Total Missiles: {stock.TotalMissiles}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error retrieving missile stock summary: {ex.Message}");
        }
    }
}
