
int[] scores = new int[] { -10, 23, 45, -34, 67, 89, -12 };
System.Console.WriteLine($"Before: {string.Join(", ", scores)}");

ref int highestScore = ref PointerToHighestPositive(scores);
highestScore = 1000;
System.Console.WriteLine($"After: {string.Join(", ", scores)}");


static ref int PointerToHighestPositive(int[] numbers)
{
    int highestIndex = -1;
    int highestValue = int.MinValue;

    for (int i = 0; i < numbers.Length; i++)
    {
        if (numbers[i] > 0 && numbers[i] > highestValue)
        {
            highestValue = numbers[i];
            highestIndex = i;
        }
    }

    if (highestIndex == -1)
    {
        throw new System.InvalidOperationException("No positive numbers in the array.");
    }

    return ref numbers[highestIndex];
}

