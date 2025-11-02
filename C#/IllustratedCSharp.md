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


## Chapter 4##
C    - functions and data types

C++  - functions and classes.

C#   - a set of type declarations


1. What is type?
    - A template for creating data structures.

2. decimal can represent decimal fractional numbers exactly. used for monetary calculations.

3. dynamic 
    - Used when using assemblies written in dynamic languages,
    - Like IronPython, IronRuby these are .NET languages as well, but they are dynamically typed.
    - If a variable declared as dynamic, it will not do the type check in compile time, 
    - But package the info and operation together then trying to use it in run time, if cannot be run, throw exception.

A running program uses two regions of memory to store data: ***stack*** & ***heap***
![alt text](image-6.png)
![alt text](image-5.png)
> For any object of a reference type, all its data members are stored in the heap, regardless of whether they are of value type or reference type.