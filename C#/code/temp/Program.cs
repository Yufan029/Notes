using System.Drawing;

var arr1 = new int[3] { 10, 20, 30 };
System.Console.WriteLine(arr1[0]);


int[][,] Arr; // An array of 2D arrays
Arr = new int[3][,]; // Instantiate an array of three 2D arrays.
Arr[0] = new int[,] { { 10, 20 },
                      { 100, 200 } };

Arr[1] = new int[,] { { 30, 40, 50 },
                      { 300, 400, 500 } };

Arr[2] = new int[,] { { 60, 70, 80, 90 },
                      { 600, 700, 800, 900 } };

for (int i = 0; i < Arr.GetLength(0); i++)
{
    for (int j = 0; j < Arr[i].GetLength(0); j++)
    {
        for (int k = 0; k < Arr[i].GetLength(1); k++)
        {
            Console.WriteLine($"[{ i }][{ j },{ k }] = { Arr[i][j,k] }");
        }
        Console.WriteLine("");
    }
    Console.WriteLine("");
}

int[,] rectArr = new int[3, 3];

int[][] jagArr = new int[3][];
jagArr[0] = new int[3];
jagArr[0][0] = 10;
jagArr[1] = new int[5];
jagArr[2] = new int[2];

System.Console.WriteLine($"rectArr.Rank = {rectArr.Rank}");
System.Console.WriteLine($"rectArr.Length = {rectArr.Length}");
System.Console.WriteLine($"jagArr.Rank = {jagArr.Rank}");
System.Console.WriteLine($"jagArr.Length = {jagArr.Length}");

int[][][] SomeArr = new int[4][][];
System.Console.WriteLine($"SomeArr.Rank = {SomeArr.Rank}"); // Output is: 1