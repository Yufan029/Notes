var spectrumStartUV = new Spectrum(true);

foreach (var color in spectrumStartUV)
{
    Console.WriteLine(color);
}

System.Console.WriteLine("---");

var spectrumStartIR = new Spectrum(false);
foreach (var color in spectrumStartIR)
{
    Console.WriteLine(color);
}