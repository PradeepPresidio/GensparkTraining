using System;

class Program
{
    static void Main()
    {
        int[] arr = { 10, 20, 30, 40, 50 };

        Console.WriteLine("Original array:");
        PrintArray(arr);
        int first = arr[0];
        if (arr.Length > 1)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }
            arr[arr.Length - 1] = first;
        }
        Console.WriteLine("Array after left rotation by one:");
        PrintArray(arr);
    }

    static void PrintArray(int[] arr)
    {
        foreach (int num in arr)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }
}
