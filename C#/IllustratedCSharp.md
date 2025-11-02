.Net Framework provides a feature called platform invoke (P/Invoke)
 - allows code written for .NET to call and use code not written for .NET

An assembly is either an executable or a DLL.
Code in assembly is intermediate language, Common Intermediate Lanuage(CIL)
![alt text](image.png)  ![alt text](image-1.png)

CIL compiled to native machine code only when it's called to run. JIT only compiles what is needed, and keep it in memory for future use.
So after JIT compile, the code under the management of CLR, like memory release, checking param's type etc.

These codes are: *Managed Code*

On the contrary: code which doesn't run under the control of CLR, say Win32, C++ DLL, is called *Unmanaged code*.

What is CLI (Common Language Infrastructure)
![alt text](image-2.png)

The holy grail - 圣杯，渴望而不可及
  - something that want very much, but cannot achieve,

The Console type provided via BCL

```c#
Console.WriteLine("The value: {0,-10:C}.", 500); // minus means left alignment
int myInt = 500;
Console.WriteLine($"The value: {myInt,10:C}.");  // 10 is the minimum width

double myDouble = 12.345678;
Console.WriteLine("{0,-10:F4} -- Fixed Point, 4 dec places.", myDouble);     // ==> 12.3457
```
![alt text](image-3.png)


