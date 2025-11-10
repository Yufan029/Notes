## Chapter 1 ##
1. .Net Framework provides a feature called platform invoke (P/Invoke)
    - allows code written for .NET to call and use code not written for .NET

## Chapter 2 ##
2. An assembly is either an executable or a DLL.
    - Code in assembly is intermediate language, Common Intermediate Lanuage(CIL)
    
    ![alt text](image.png)  ![alt text](image-1.png)

3. CIL compiled to native machine code only when it's called to run. JIT only compiles what is needed, and keep it in memory for future use.
So after JIT compile, the code under the management of CLR, like memory release, checking param's type etc.

    These codes are: *Managed Code*

    On the contrary: code which doesn't run under the control of CLR, say Win32, C++ DLL, is called *Unmanaged code*.

4. What is CLI (Common Language Infrastructure)
![alt text](image-2.png)

---

>The holy grail - 圣杯，渴望而不可及
>  - something that want very much, but cannot achieve,

---

## Chapter 3 ##
5. The Console type provided via BCL

```C#
Console.WriteLine("The value: {0,-10:C}.", 500); // minus means left alignment
int myInt = 500;
Console.WriteLine($"The value: {myInt,10:C}.");  // 10 is the minimum width

double myDouble = 12.345678;
Console.WriteLine("{0,-10:F4} -- Fixed Point, 4 dec places.", myDouble);     // ==> 12.3457
```
![alt text](image-3.png)


## Chapter 4 ##
- C    - functions and data types

- C++  - functions and classes.

- C#   - a set of type declarations


- What is type?
  - A template for creating data structures.
    ![alt text](image-7.png)

- decimal can represent decimal fractional numbers exactly. used for monetary calculations.

- dynamic 
  - Used when using assemblies written in dynamic languages,
  - Like IronPython, IronRuby these are .NET languages as well, but they are dynamically typed.
  - If a variable declared as dynamic, it will not do the type check in compile time, 
  - But package the info and operation together then trying to use it in run time, if cannot be run, throw exception.

- A running program uses two regions of memory to store data: ***stack*** & ***heap***
![alt text](image-6.png)
![alt text](image-5.png)
> For any object of a reference type, all its data members are stored in the heap, regardless of whether they are of value type or reference type.

## Chapter 5 ##
> A running program is a set of objects interacting with each other.

## Chapter 6 ##
- Beginning with C# 7.0 you can declare a separate method inside another method.
- Isolate the inner method from outside.
- Called *local functions*

- Formal parameter Vs Actual parameter(argument)
 - Formal parameter is the parameter used for declare the method.
 - Actual parameter is the value used for invoke the method.

- Value Parameters Vs Reference Parameters
  - For value parameters, the system allocates memory on the stack for formal parameters.
  
  - Reference parameters ***DO NOT*** allocate memory on the stack for the formal parameteres.
    - The formal parameter name acts as an ***alias*** for the actual parameter variable, referring to the same memory location.
  ```c#
    void MyMethod(ref int val) {...}
    int y = 1;
    MyMethod(ref y);
  ```

- *Output parameters*
  - Like reference parameters, the formal parameters of *output parameters* act as *aliases*
  ```c#
    MyMethod(out MyClass a1, out int a2);
  ```

- *parameter Arrays*
  ```c#
    void ListInts(params int[] inVals) {...}
  ```
    - Expanded Form
      ```c#
        ListInts( 1, 2, 3 );
      ```

### Ref Local and Ref Return
- *ref local* allows a variable to be an alias for another variable.
  ```c#
    static void Main() {
      int x = 2;

      ref int y = ref x;    // *ref local* feature
      Console.WriteLine($"x = {x}, y = {y}");   // x = 2, y = 2

      y = 5;
      Console.WriteLine($"x = {x}, y = {y}");   // x = 5, y = 5

      x = 6;
      Console.WriteLine($"x = {x}, y = {y}");   // x = 6, y = 6
    }
  ```

- *ref return* lets you send a reference *out of a method* where it can be used in the calling context.
  - Some constrains, page 116.
  ```c#
    class Simple
    {
      private int Score = 5;

      public ref int RefToValue()     // *ref return*
      {
        return ref Score;
      }

      public void Display()
      {
        console.WriteLIne($"Value inside class object: {Score}");
      }
    }

    class Program
    {
      static void Main()
      {
        Simple s = new Simple();
        s.Display();

        ref int v1Outside = ref s.RefToValue();     // ref local conjunction with ref return
        
        v1Outside = 10;
        s.Display();
      }
    }
  ```

### Stack Frames ###
- When a method is called, memory is allocated at the top of the stack to hold a number of data items associated with the method.
- This chunk of memory is called the *stack frame* for the method.