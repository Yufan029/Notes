var spectrum = new Spectrum();

foreach (var color in spectrum.UVtoIR)
{
    Console.WriteLine(color);
}

System.Console.WriteLine("---");

foreach (var color in spectrum.IRtoUV)
{
    Console.WriteLine(color);
}