class Animal { public int Legs = 4; }

class Dog : Animal { }

delegate void Action1<in T>(T a);

class Program
{
    static void AnimalAction(Animal a)
    {
        System.Console.WriteLine(a.Legs);
    }

    static void Main()
    {
        Action1<Animal> actAnimal = AnimalAction;
        Action1<Dog> actDog = actAnimal; // Contravariance allows this assignment

        actDog(new Dog());
    }
}