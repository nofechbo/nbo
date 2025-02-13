using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StringParser
{
    public virtual Dictionary<string, string> Parse(string input)
    {
        var argsMap = new Dictionary<string, string>();

        var parts = input.Split(":", 2); 

        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
        {
            throw new InvalidCommandException("Invalid command format: Expected 'command:launcherID,info'");
        }

        argsMap.Add("command", parts[0].Trim()); // Command

        var arguments = parts[1].Split(","); // Split arguments by comma

        if (arguments.Length != 2)
        {
            throw new InvalidCommandException("Invalid argument format: Expected exactly two arguments (launcherID, info)");
        }

        argsMap.Add("launcherID", arguments[0].Trim()); // First argument is launcher ID
        argsMap.Add("info", arguments[1].Trim()); // Second argument is always "info"

        return argsMap;
    }
}



public class InvalidCommandException : Exception
{
    public InvalidCommandException(string message) : base(message) { }
}

