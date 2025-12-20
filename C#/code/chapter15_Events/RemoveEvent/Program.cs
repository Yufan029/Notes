
var publisher = new Publisher();
var subscribe = new Subscriber();

System.Console.WriteLine("Add MethodA and MethodB:");
publisher.SimpleEvent += subscribe.MethodA;
publisher.SimpleEvent += subscribe.MethodB;
publisher.RaiseEvent();

System.Console.WriteLine("Remove MethodB:");
publisher.SimpleEvent -= subscribe.MethodB;
publisher.RaiseEvent();

System.Console.WriteLine("Add MethodA again:");
publisher.SimpleEvent += subscribe.MethodA;
publisher.RaiseEvent();

class Publisher
{
    public event EventHandler? SimpleEvent;

    public void RaiseEvent()
    {
        SimpleEvent?.Invoke(this, EventArgs.Empty);
    }
}

class Subscriber
{
    public void MethodA(object? sender, EventArgs e) => System.Console.WriteLine("\tAAA");
    public void MethodB(object? sender, EventArgs e) => System.Console.WriteLine("\tBBB");
}