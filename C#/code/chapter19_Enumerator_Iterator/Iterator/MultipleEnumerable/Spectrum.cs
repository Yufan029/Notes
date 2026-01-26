class Spectrum
{
    string[] colors = { "violet", "blue", "cyan", "green", "yellow", "orange", "red" };

    // public IEnumerable<string> UVtoIR()
    // {
    //     for (var i = 0; i < colors.Length; i++)
    //     {
    //         yield return colors[i];
    //     }
    // }

    public IEnumerable<string> UVtoIR
    {
        get
        {
            for (var i = 0; i < colors.Length; i++)
            {
                yield return colors[i];
            }
        }
    }

    // public IEnumerable<string>IRtoUV()
    // {
    //     for (var i = colors.Length - 1; i >= 0; i--)
    //     {
    //         yield return colors[i];
    //     }
    // }

    public IEnumerable<string> IRtoUV
    {
        get
        {
            for (var i = colors.Length - 1; i >= 0; i--)
            {
                yield return colors[i];
            }   
        }
    }
}