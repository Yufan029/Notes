public class SimpleGeneric<T> : IMyIfc<T>, IMyIfc
{
    public T ReturnIt(T input)
    {
        return input;
    }

    public double ReturnIt(double input)
    {
        return input;
    }
}