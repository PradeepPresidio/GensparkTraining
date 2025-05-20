using System;

class Program
{
    static void Main()
    {
        int[] arr = new int[10];
        int count = 0;

        Console.WriteLine("Enter 10 numbers: ");
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write($"Number {i + 1}: ");
            arr[i] = Convert.ToInt32(Console.ReadLine());

            if (arr[i] % 7 == 0)
            {
                count++;
            }
        }

        Console.WriteLine($"Number of integers divisible by 7 is {count}");
    }
}