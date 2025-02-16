using System;
using System.Threading.Tasks;
using ReportGenerator; 

namespace Command
{
    public class GetReport : ICommand
    {
        private static ReportService? _reportService;

        private static ReportService ReportService
        {
            get
            {
                if (_reportService == null)
                {
                    _reportService = new ReportService();
                }
                return _reportService;
            }
        }

        public string Execute()
        {
            Console.WriteLine("\n📊 Welcome to the Report Generator!");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. Generate Report for All Missile Launchers");
                Console.WriteLine("2. Generate Report for Launchers with Failures");
                Console.WriteLine("3. Generate Missile Stock Summary");
                Console.WriteLine("4. Generate Report for Launchers with Unfixed Failures");
                Console.WriteLine("5. Generate PDF Report");
                Console.WriteLine("6. Exit");

                var userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ReportService.GetAllLaunchers();
                        break;
                    case "2":
                        ReportService.GetLaunchersWithFailures();
                        break;
                    case "3":
                        ReportService.GetMissileStockSummary();
                        break;
                    case "4":
                        ReportService.GetUnfixedFailures();
                        break;
                    case "5":
                        // Generate the PDF report
                        ReportService.GeneratePdfReport();
                        break;
                    case "6":
                        return "✅ Exiting the report generator.";
                    default:
                        return "❌ Invalid selection, please choose a valid option.";
                }

            return "Goodbye!";
        }
    }
}
