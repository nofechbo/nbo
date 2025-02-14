using System;
using System.Collections.Generic;
using DataBase;

namespace Command
{
    public class SendTechnician : ICommand
    {
        private readonly string launcherID;
        private readonly DatabaseHandler _dbHandler;

        public SendTechnician(Dictionary<string, string> args, DatabaseHandler dbHandler)
        {
            if (!args.TryGetValue("launcherID", out launcherID!) || string.IsNullOrWhiteSpace(launcherID))
            {
                throw new ArgumentException("Invalid arguments for SendTechnician");
            }
            _dbHandler = dbHandler ?? throw new ArgumentNullException(nameof(dbHandler));
        }

        public string Execute()
        {
            _dbHandler.IncrementFixedCount(launcherID);
            return $"👨‍🔧 Technician sent to launcher: {launcherID}";
        }
    }
}
