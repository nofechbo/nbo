/*#if DEBUG  // Ensures this code runs only in debug mode, not in tests
using Command;

Console.WriteLine("Testing RegUpdate...");
var regArgs = new Dictionary<string, string> { { "update", "System patch applied" } };
var regUpdate = new RegUpdate(regArgs);
regUpdate.Execute(); // Expected output: "update: System patch applied"

Console.WriteLine("\nTesting SendMissiles...");

// Test for SendMissiles - valid input
var missileArgs = new Dictionary<string, string>
{
    { "missile", "Tomahawk" },
    { "location", "Target Alpha" },
    { "numMissiles", "5" }
};
var sendMissiles = new SendMissiles(missileArgs);
sendMissiles.Execute();

// Test for SendMissiles - missing numMissiles (should throw exception)
try
{
    var invalidMissileArgs = new Dictionary<string, string>
    {
        { "missile", "Cruise" },
        { "location", "Target Beta" }
    };
    var sendMissilesInvalid = new SendMissiles(invalidMissileArgs); // Should throw ArgumentException
}
catch (ArgumentException)
{
    Console.WriteLine("Invalid key caught for SendMissiles (missing numMissiles)");
}

// Test for SendMissiles - invalid numMissiles (should default to 10)
var missileArgsInvalidNum = new Dictionary<string, string>
{
    { "missile", "Interceptor" },
    { "location", "Target Gamma" },
    { "numMissiles", "invalid" } // Invalid number
};
var sendMissilesInvalidNum = new SendMissiles(missileArgsInvalidNum);
sendMissilesInvalidNum.Execute();


// Testing SendTechnician
Console.WriteLine("\nTesting SendTechnician...");

// Valid test for SendTechnician
var technicianArgs = new Dictionary<string, string>
{
    { "missile", "Tomahawk" },
    { "location", "Base Alpha" }
};
var sendTechnician = new SendTechnician(technicianArgs);
sendTechnician.Execute(); // Expected output: "technician sent to location: Base Alpha, for missile: Tomahawk"

// Test for SendTechnician - missing missile key (should throw exception)
try
{
    var invalidTechnicianArgs = new Dictionary<string, string>
    {
        { "location", "Base Beta" }
    };
    var sendTechnicianInvalid = new SendTechnician(invalidTechnicianArgs); // Should throw ArgumentException
}
catch (ArgumentException)
{
    Console.WriteLine("Invalid key caught for SendTechnician (missing missile)");
}

// Test for SendTechnician - missing location key (should throw exception)
try
{
    var invalidTechnicianArgs = new Dictionary<string, string>
    {
        { "missile", "Cruise" }
    };
    var sendTechnicianInvalid = new SendTechnician(invalidTechnicianArgs); // Should throw ArgumentException
}
catch (ArgumentException)
{
    Console.WriteLine("Invalid key caught for SendTechnician (missing location)");
}

// Test for SendTechnician - empty missile value (should throw exception)
try
{
    var emptyTechnicianArgs = new Dictionary<string, string>
    {
        { "missile", " " },
        { "location", "Base Gamma" }
    };
    var sendTechnicianEmpty = new SendTechnician(emptyTechnicianArgs); // Should throw ArgumentException
}
catch (ArgumentException)
{
    Console.WriteLine("Invalid key caught for SendTechnician (empty missile value)");
}

// Test for SendTechnician - empty location value (should throw exception)
try
{
    var emptyTechnicianArgs = new Dictionary<string, string>
    {
        { "missile", "Ballistic" },
        { "location", " " }
    };
    var sendTechnicianEmpty = new SendTechnician(emptyTechnicianArgs); // Should throw ArgumentException
}
catch (ArgumentException)
{
    Console.WriteLine("Invalid key caught for SendTechnician (empty location value)");
}
#endif
*/