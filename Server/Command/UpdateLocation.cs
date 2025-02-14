using System;
using System.Collections.Generic;
using DataBase;

namespace Command
{
    public class UpdateLocation : ICommand
    {
        private readonly string launcherID;
        private readonly string location;
        private readonly DatabaseHandler _dbHandler;

        public UpdateLocation(Dictionary<string, string> args, DatabaseHandler dbHandler)
        {
            if (!args.TryGetValue("launcherID", out launcherID!) || string.IsNullOrWhiteSpace(launcherID) ||
                !args.TryGetValue("info", out location!) || string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentException("Invalid arguments for RegUpdate");
            }
            _dbHandler = dbHandler ?? throw new ArgumentNullException(nameof(dbHandler));
        }

        public string Execute()
        {
            _dbHandler.UpdateLauncherLocation(launcherID, location);
            return $"🔄 Update received for launcher: {launcherID}, new location: {location}";
        }
    }
}
