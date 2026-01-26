foreach (var color in Enum.GetValues(typeof(ConsoleColor)))
{
    Console.WriteLine(color);
}

System.Console.WriteLine("--- Using Custom ColorEnumerator ---");
var spectrum = new Spectrum();
foreach (var color in spectrum)
{
    Console.WriteLine(color);
}