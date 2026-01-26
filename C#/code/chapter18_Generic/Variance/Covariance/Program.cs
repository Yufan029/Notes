class Animal
{
    public int Legs = 4;
}

class Dog : Animal {}

delegate T Factory<out T>();

public class Program
{
    static Dog DogMaker()
    {
        return new Dog();
    }

    public static void Main()
    {
        Factory<Dog> dogFactory = DogMaker;
        Factory<Animal> animalFactory = dogFactory;
        
        System.Console.WriteLine("My animal has " + animalFactory().Legs + " legs.");
    }
}