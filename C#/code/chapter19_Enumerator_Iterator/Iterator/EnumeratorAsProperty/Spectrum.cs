class Spectrum
{
    string[] colors = { "violet", "blue", "cyan", "green", "yellow", "orange", "red" };
    bool listFromUVtoIR = true;

    public Spectrum(bool listFroUVtoIR)
    {
        listFromUVtoIR = listFroUVtoIR;
    }

    public IEnumerator<string> GetEnumerator()
    {
        return listFromUVtoIR ? UVtoIR : IRtoUV;
    }

    public IEnumerator<string> UVtoIR
    {
        get
        {
            for (var i = 0; i < colors.Length; i++)
            {
                yield return colors[i];
            }
        }
    }

    public IEnumerator<string> IRtoUV
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