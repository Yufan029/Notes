
// The delegate, the event and event handler must follow the pattern
public delegate void Handler();


// The publisher
class Incrementer
{
    // declare the event

    // 1 version
    //public event Handler? CountedADozen;

    // 2 version
    //public event EventHandler? CountedADozen;

    // 3 version - generic delegate using custom class
    public event EventHandler<IncrementerEventArgs>? CountedADozen;

    public void DoCount()
    {
        for (int i = 1; i < 100; i++)
        {
            if (i % 12 == 0 && CountedADozen != null)
            {
                // raise the event

                // 1 version
                //CountedADozen();

                // 2 version
                //CountedADozen(this, EventArgs.Empty);

                // 3 version
                CountedADozen(this, new IncrementerEventArgs { IterationCount = i });
            }
        }
    }
}