var parser = new StringParser();

try
{
    string input = "regupdate:12345,needMaintenance"; // Example input
    var result = parser.Parse(input);

    foreach (var kvp in result)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
}
catch (InvalidCommandException ex)
{
    Console.WriteLine($"Parsing Error: {ex.Message}");
}
