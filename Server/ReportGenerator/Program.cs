using System;


namespace ReportGenerator {
    class Program
    {
        static void Main()
        {
            var reportService = new ReportService();

            while (true)
            {
                Console.WriteLine("\n📊 Select a Report:");
                Console.WriteLine("1 - All Missile Launchers");
                Console.WriteLine("2 - Launchers with Failures");
                Console.WriteLine("3 - Missile Stock Summary");
                Console.WriteLine("4 - Exit");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        reportService.GetAllLaunchers();
                        break;
                    case "2":
                        reportService.GetLaunchersWithFailures();
                        break;
                    case "3":
                        reportService.GetMissileStockSummary();
                        break;
                    case "4":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("❌ Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}