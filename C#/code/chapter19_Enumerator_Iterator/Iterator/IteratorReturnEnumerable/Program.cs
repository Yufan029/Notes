var myClass = new MyClass();

foreach (var item in myClass)
{
    Console.WriteLine(item);
}

System.Console.WriteLine("---");

foreach (var item in myClass.BlackAndWhite())
{
    Console.WriteLine(item);
}