using System.Collections;

class Spectrum : IEnumerable
{
    private string[] colors = { "Red", "Orange", "Yellow", "Green", "Blue", "Indigo", "Violet" };

    public IEnumerator GetEnumerator()
    {
        return new ColorEnumerator(colors);
    }
}