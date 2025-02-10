using System;

namespace Command
{
    public class SendMissiles : ICommand
    {
        private readonly string launcherID;
        private readonly string info;

        public SendMissiles(Dictionary<string, string> args)
        {
            if (!args.TryGetValue("launcherID", out launcherID!) || string.IsNullOrWhiteSpace(launcherID) ||
                !args.TryGetValue("info", out info!) || string.IsNullOrWhiteSpace(info))
            {
                throw new ArgumentException("Invalid arguments for SendMissiles");
            }
        }

        public void Execute()
        {
            Console.WriteLine($"Missiles launched from launcher: {launcherID}, status: {info}");
        }
    }
}
