using System;
using System.Collections.Generic;

namespace Command
{
    public class SendTechnician : ICommand
    {
        private readonly string launcherID;
        private readonly string info;

        public SendTechnician(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("launcherID", out launcherID!) || string.IsNullOrWhiteSpace(launcherID) ||
                !args.TryGetValue("info", out info!) || string.IsNullOrWhiteSpace(info))
            {
                throw new ArgumentException("Invalid arguments for SendTechnician");
            }
        }

        public void Execute()
        {
            Console.WriteLine($"Technician sent to launcher: {launcherID}, status: {info}");
        }
    }
}
