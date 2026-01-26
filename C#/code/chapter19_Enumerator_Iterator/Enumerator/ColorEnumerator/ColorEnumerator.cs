using System.Collections;

class ColorEnumerator : IEnumerator
{
    private string[] colors;
    private int position = -1;

    public ColorEnumerator(string[] colors)
    {
        this.colors = colors;
    }

    // Not type safe, ideally implement IEnumerator<string>
    public object Current
    {
        get
        {
            if (position < 0 || position >= colors.Length)
                throw new InvalidOperationException();
            return colors[position];
        }
    }

    public bool MoveNext()
    {
        position++;
        return (position < colors.Length);
    }

    public void Reset()
    {
        position = -1;
    }
}