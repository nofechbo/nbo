using System;
using System.Collections.Generic;
using DataBase;

namespace Command
{
    public class SendMissiles : ICommand
    {
        private readonly string launcherID;
        private readonly int missiles;
        private readonly DatabaseHandler _dbHandler;

        public SendMissiles(Dictionary<string, string> args, DatabaseHandler dbHandler)
        {
            if (!args.TryGetValue("launcherID", out launcherID!) || string.IsNullOrWhiteSpace(launcherID) ||
                !args.TryGetValue("info", out string missileCountStr) || !int.TryParse(missileCountStr, out missiles))
            {
                throw new ArgumentException("Invalid arguments for SendMissiles");
            }
            _dbHandler = dbHandler ?? throw new ArgumentNullException(nameof(dbHandler));
        }

        public string Execute()
        {
            _dbHandler.UpdateMissileCount(launcherID, missiles);
            return $"🚀Missiles supply sent to launcher: {launcherID}, missiles sent: {missiles}";
        }
    }
}
