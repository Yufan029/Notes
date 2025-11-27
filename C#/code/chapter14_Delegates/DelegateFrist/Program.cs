namespace DelegateFrist;

// Declare delegate TYPE.
delegate void MyDel(int value);

class Program
{
    void PrintLow(int value)
    {
        Console.WriteLine("Low: " + value);
    }

    void PrintHigh(int value)
    {
        Console.WriteLine("High: " + value);
    }

    static void Main(string[] args)
    {
        Program program = new Program();

        // Declare delegate variable.
        MyDel del;

        Random rand = new Random();
        int randomValue = rand.Next(99);

        del = randomValue < 50
            ? new MyDel(program.PrintLow)
            : new MyDel(program.PrintHigh);
        
        del(randomValue);
    }
}
