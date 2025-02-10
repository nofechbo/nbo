using System;
using System.Linq;
using System.Collections.Generic;
using DataBase;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Diagnostics;

namespace ReportGenerator
{
    public class ReportService 
    {
        private void PrintTableHeader(string title, string[] headers, int[] columnWidths)
        {
            Console.WriteLine($"\n🔹 {title}");
            Console.WriteLine(new string('-', columnWidths.Sum() + headers.Length + 1));

            // Print table headers
            Console.Write("|");
            for (int i = 0; i < headers.Length; ++i)
                Console.Write($" {headers[i].PadRight(columnWidths[i])} |");
            Console.WriteLine();

            Console.WriteLine(new string('-', columnWidths.Sum() + headers.Length + 1));
        }

        public void GetAllLaunchers()
        {
            using (var db = new MissileDbContext()) 
            {
                var launchers = db.MissileLaunchers.ToList();

                if (!launchers.Any())
                {
                    Console.WriteLine("\n✅ No missile launchers found.");
                    return;
                }

                PrintTableHeader("All Missile Launchers",
                    new[] { "ID", "Code", "Location", "Missile Type", "Count", "Failures", "Fixed" },
                    new[] { 3, 9, 15, 15, 6, 8, 6 });

                foreach (var launcher in launchers)
                {
                    Console.WriteLine($"| {launcher.Id,-3} | {launcher.Code,-9} | {launcher.Location,-15} | {launcher.MissileType,-15} | {launcher.MissileCount,-6} | {launcher.FailureCount,-8} | {launcher.FixedFailures,-6} |");
                }
                Console.WriteLine(new string('-', 70));
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

                if (!failedLaunchers.Any())
                {
                    Console.WriteLine("\n✅ No launchers with failures.");
                    return;
                }

                PrintTableHeader("Launchers with Failures",
                    new[] { "ID", "Code", "Failures" },
                    new[] { 3, 9, 8 });

                foreach (var launcher in failedLaunchers)
                {
                    Console.WriteLine($"| {launcher.Id,-3} | {launcher.Code,-9} | {launcher.FailureCount,-8} |");
                }
                Console.WriteLine(new string('-', 30));
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

                if (!stockSummary.Any())
                {
                    Console.WriteLine("\n✅ No missile stock data available.");
                    return;
                }

                PrintTableHeader("Missile Stock Summary",
                    new[] { "Missile Type", "Total Missiles" },
                    new[] { 15, 14 });

                foreach (var stock in stockSummary)
                {
                    Console.WriteLine($"| {stock.MissileType,-15} | {stock.TotalMissiles,-14} |");
                }
                Console.WriteLine(new string('-', 34));
            }
        }

        public void GetUnfixedFailures()
        {
            using (var db = new MissileDbContext()) 
            {
                var unfixedFailures = db.MissileLaunchers
                    .Where(l => l.FailureCount > l.FixedFailures)
                    .Select(l => new 
                    { 
                        l.Id, 
                        l.Code, 
                        l.FailureCount, 
                        l.FixedFailures, 
                        UnfixedCount = l.FailureCount - l.FixedFailures 
                    })
                    .OrderByDescending(l => l.UnfixedCount)
                    .ToList();

                if (!unfixedFailures.Any())
                {
                    Console.WriteLine("\n✅ No launchers with unfixed failures.");
                    return;
                }

                PrintTableHeader("Launchers with Unfixed Failures",
                    new[] { "ID", "Code", "Failures", "Fixed", "Unfixed" },
                    new[] { 3, 9, 8, 6, 8 });

                foreach (var launcher in unfixedFailures)
                {
                    Console.WriteLine($"| {launcher.Id,-3} | {launcher.Code,-9} | {launcher.FailureCount,-8} | {launcher.FixedFailures,-6} | {launcher.UnfixedCount,-8} |");
                }
                Console.WriteLine(new string('-', 50));
            }
        }

         public void GeneratePdfReport()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var filePath = "MissileReport.pdf";

            using (var db = new MissileDbContext())
            {
                var launchers = db.MissileLaunchers.ToList();

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(30);
                        page.Header().AlignCenter().Text($"Missile Launcher Report - {DateTime.Now}\n").FontSize(18).Bold();

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("ID").Bold();
                                header.Cell().Text("Code").Bold();
                                header.Cell().Text("Location").Bold();
                                header.Cell().Text("Missile Type").Bold();
                                header.Cell().Text("Count").Bold();
                                header.Cell().Text("Failures").Bold();
                                header.Cell().Text("Fixed Failures").Bold();
                            });

                            foreach (var launcher in launchers)
                            {
                                var hasUnfixedFailures = launcher.FailureCount > launcher.FixedFailures;
                                var color = hasUnfixedFailures ? Colors.Red.Medium : Colors.Black;

                                table.Cell().Text(launcher.Id.ToString()).FontColor(color);
                                table.Cell().Text(launcher.Code).FontColor(color);
                                table.Cell().Text(launcher.Location).FontColor(color);
                                table.Cell().Text(launcher.MissileType).FontColor(color);
                                table.Cell().Text(launcher.MissileCount.ToString()).FontColor(color);
                                table.Cell().Text(launcher.FailureCount.ToString()).FontColor(color);
                                table.Cell().Text(launcher.FixedFailures.ToString()).FontColor(color);
                            }
                        });
                    });
                }).GeneratePdf(filePath);

                Console.WriteLine("PDF report generated successfully: " + filePath);
                Process.Start(new ProcessStartInfo { FileName = filePath, UseShellExecute = true });
            }
        }
    }
}



/*
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
*/