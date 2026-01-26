var simple = new Simple();
Console.WriteLine(simple.ReturnIt(42));
Console.WriteLine(simple.ReturnIt("Hello Simple string"));
Console.WriteLine(simple.ReturnIt(42.5));

var simpleGeneric = new SimpleGeneric<string>();
Console.WriteLine(simpleGeneric.ReturnIt("Hello Generics"));
Console.WriteLine(simpleGeneric.ReturnIt(100));
Console.WriteLine(simpleGeneric.ReturnIt(99.9));