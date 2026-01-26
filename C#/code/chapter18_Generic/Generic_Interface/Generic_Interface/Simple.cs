public class Simple : IMyIfc<int>, IMyIfc<string>, IMyIfc
{
    public int ReturnIt(int input)
    {
        return input;
    }

    public string ReturnIt(string input)
    {
        return input;
    }

    public double ReturnIt(double input)
    {
        return input;
    }
}