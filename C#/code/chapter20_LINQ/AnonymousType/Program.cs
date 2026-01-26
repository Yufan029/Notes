var student = new
{
    Name = "Alice",
    Age = 20,
    Major = "Computer Science"
};

Console.WriteLine($"Name: {student.Name}, Age: {student.Age}, Major: {student.Major}");

//student.Age = 21; // This line will cause a compile-time error because properties of anonymous types are read-only.