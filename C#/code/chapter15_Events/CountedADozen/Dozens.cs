

// The subscriber
class Dozens
{
    public int DozensCount { get; private set; }

    public Dozens(Incrementer incrementer)
    {
        DozensCount = 0;

        // register the event handler / subscribe to the event
        incrementer.CountedADozen += IncrementDozensCount;
    }


    // declare the event handler (callback)

    // 1 version
    // public void IncrementDozensCount()
    // {
    //     DozensCount++;
    // }

    // 2 version
    // public void IncrementDozensCount(Object? sender, EventArgs e)
    // {
    //     DozensCount++;
    // }

    // 3 version
    public void IncrementDozensCount(Object? sender, IncrementerEventArgs e)
    {
        System.Console.WriteLine($"Incremented at iteration: {e.IterationCount} in {sender?.ToString()}");
        DozensCount++;
    }
}