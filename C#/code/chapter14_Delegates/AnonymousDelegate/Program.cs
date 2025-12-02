delegate void MyDel(int value);

delegate void ParamsDel(int x, params int[] values);

class Program
{
    static void Main(string[] args)
    {
        // 1. no out parameter
        // 2. parameter is not used in the anonymous method
        // parentheses can be omitted
        MyDel del = delegate
        {
            Console.WriteLine($"This is an anonymous delegate.");
        };

        del(3);

        ParamsDel pdel = delegate (int x, int[] values)
        {
            Console.WriteLine($"x = {x}, values length = {values.Length}");
            Console.WriteLine($"values: {string.Join(", ", values)}");
            Console.WriteLine($"value 1: {values[0]} ");
            Console.WriteLine($"last value: {values[values.Length - 1]} ");
        };

        pdel(0, 1, 2, 3, 4);

        int outter = 5;
        MyDel del2 = delegate (int value)
        {
            // accessing outer variable 'outter'
            Console.WriteLine($"Value + outter = {value + outter}");
        };

        del2(10);
    }
}
