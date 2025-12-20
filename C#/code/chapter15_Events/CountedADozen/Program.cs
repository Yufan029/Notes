var incrementer = new Incrementer();
var dozenCounter = new Dozens(incrementer);

incrementer.DoCount();
System.Console.WriteLine($"Number of dozens within 100 = {dozenCounter.DozensCount}");