/*using System.Text;
using MyFactory;

Console.WriteLine("Testing Factory...");

// Create factory instance
var factory = new Factory<string, string, object>();

// Lambda expression
factory.Add("Custom", data => "Custom object with data: " + data);
Console.WriteLine(factory.Create("Custom", "Lambda"));

try
{
    factory.Create("Customize", "Lambda");
}
catch (ArgumentException)
{
    Console.WriteLine("ArgumentException caught");
}

// Constructor reference
factory.Add("StringBuilder", data => new StringBuilder(data));
Console.WriteLine(((StringBuilder)factory.Create("StringBuilder", "Hello")).Append(" World!"));

// Reference to an instance method of an arbitrary object of a particular type
factory.Add("ArbitraryInstance", data => data!.Length);
Console.WriteLine(factory.Create("ArbitraryInstance", "Tzur the great!"));

// Anonymous class (converted to lambda)
factory.Add("AnonymousClass", data => "Anonymous Class with data: " + data);
Console.WriteLine(factory.Create("AnonymousClass", "Anonymous Class Example"));

// Static method reference
factory.Add("Integer", data => (object)int.Parse(data!));
Console.WriteLine(factory.Create("Integer", "123"));

// Complex lambda expression
factory.Add("ComplexObject", data => new ComplexObject("Complex Object: " + data, data!.Length));

// Nested factory test
var nestedFactory = new Factory<string, string?, Factory<string, string, object>>();
nestedFactory.Add("Nested", _ => factory);
var retrievedFactory = nestedFactory.Create("Nested", null);
Console.WriteLine("Nested Factory Test: " + retrievedFactory.Create("Custom", "Nested Example"));

// Complex object test
var complex = (ComplexObject)factory.Create("ComplexObject", "Test Data");
Console.WriteLine("Complex Object Test: " + complex);
*/
