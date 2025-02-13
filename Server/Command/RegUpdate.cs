using System;
using System.Collections.Generic;
using DataBase;

namespace Command
{
    public class RegUpdate : ICommand
    {
        private readonly string launcherID;
        private readonly string info;
        private readonly DatabaseHandler _dbHandler;

        public RegUpdate(Dictionary<string, string> args, DatabaseHandler dbHandler)
        {
            if (!args.TryGetValue("launcherID", out launcherID!) || string.IsNullOrWhiteSpace(launcherID) ||
                !args.TryGetValue("info", out info!) || string.IsNullOrWhiteSpace(info))
            {
                throw new ArgumentException("Invalid arguments for RegUpdate");
            }
            _dbHandler = dbHandler ?? throw new ArgumentNullException(nameof(dbHandler));
        }

        public void Execute()
        {
            Console.WriteLine($"🔄 Update received for launcher: {launcherID}, info: {info}");
            _dbHandler.UpdateLauncherStatus(launcherID, info);
        }
    }
}
