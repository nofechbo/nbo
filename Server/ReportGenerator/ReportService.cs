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
                        // Set landscape orientation
                        page.Size(PageSizes.A4.Landscape());
                        page.Margin(40);

                        // Header with logo and system name
                        page.Header().Column(col =>
                        {
                            // Adjusted for landscape - put logo and title in a row
                            col.Item().Row(row =>
                            {
                                row.RelativeItem().AlignCenter().Height(80)
                                    .Image("rocket-launcher-logo-B51C8CAE2E-seeklogo.com.png", ImageScaling.FitArea);
                                
                                row.RelativeItem().Column(titleCol =>
                                {
                                    titleCol.Item().Text("Launcher Management System LTD")
                                        .FontSize(24)
                                        .Bold()
                                        .FontColor(Colors.Blue.Darken2)
                                        .AlignCenter();

                                    titleCol.Item().Text("System Status Report")
                                        .FontSize(14)
                                        .FontColor(Colors.Grey.Darken1)
                                        .AlignCenter();

                                    titleCol.Item().PaddingTop(5)
                                        .Text($"Generated on: {DateTime.Now:MMMM dd, yyyy HH:mm}")
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Medium)
                                        .AlignCenter();
                                });
                            });
                        });

                        // Add spacing and table - optimized for landscape
                        page.Content().PaddingTop(20).Table(table =>
                        {
                            // Adjusted column widths for landscape
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);     // ID
                                columns.RelativeColumn(3);      // Code
                                columns.RelativeColumn(4);      // Location
                                columns.RelativeColumn(4);      // Missile Type
                                columns.RelativeColumn(2);      // Count
                                columns.RelativeColumn(2);      // Failures
                                columns.RelativeColumn(2);      // Fixed
                            });

                            // Enhanced header style
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("ID").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Code").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Location").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Missile Type").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Count").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Failures").Bold();
                                header.Cell().Background(Colors.Grey.Lighten3).Padding(5).Text("Fixed").Bold();
                            });

                            // Enhanced row styling
                            foreach (var launcher in launchers)
                            {
                                var hasUnfixedFailures = launcher.FailureCount > launcher.FixedFailures;
                                var rowColor = hasUnfixedFailures ? Colors.Red.Lighten4 : Colors.White;
                                var textColor = hasUnfixedFailures ? Colors.Red.Darken1 : Colors.Grey.Darken3;

                                var isEvenRow = launchers.IndexOf(launcher) % 2 == 0;
                                if (!hasUnfixedFailures)
                                {
                                    rowColor = isEvenRow ? Colors.White : Colors.Grey.Lighten4;
                                }

                                table.Cell().Background(rowColor).Padding(5).Text(launcher.Id.ToString()).FontColor(textColor);
                                table.Cell().Background(rowColor).Padding(5).Text(launcher.Code).FontColor(textColor);
                                table.Cell().Background(rowColor).Padding(5).Text(launcher.Location).FontColor(textColor);
                                table.Cell().Background(rowColor).Padding(5).Text(launcher.MissileType).FontColor(textColor);
                                table.Cell().Background(rowColor).Padding(5).Text(launcher.MissileCount.ToString()).FontColor(textColor);
                                table.Cell().Background(rowColor).Padding(5).Text(launcher.FailureCount.ToString()).FontColor(textColor);
                                table.Cell().Background(rowColor).Padding(5).Text(launcher.FixedFailures.ToString()).FontColor(textColor);
                            }
                        });

                        // Footer
                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.Span("Page ").FontSize(10).FontColor(Colors.Grey.Medium);
                                text.CurrentPageNumber().FontSize(10).FontColor(Colors.Grey.Medium);
                                text.Span(" of ").FontSize(10).FontColor(Colors.Grey.Medium);
                                text.TotalPages().FontSize(10).FontColor(Colors.Grey.Medium);
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