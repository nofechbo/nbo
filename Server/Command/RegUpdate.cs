using System;
using System.Collections.Generic;

namespace Command
{
    public class RegUpdate : ICommand
    {
        private readonly string launcherID;
        private readonly string info;

        public RegUpdate(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("launcherID", out launcherID!) || string.IsNullOrWhiteSpace(launcherID) ||
                !args.TryGetValue("info", out info!) || string.IsNullOrWhiteSpace(info))
            {
                throw new ArgumentException("Invalid arguments for RegUpdate");
            }
        }

        public void Execute()
        {
            Console.WriteLine($"update received for launcher: {launcherID}, info: {info}");
        }
    }
}
