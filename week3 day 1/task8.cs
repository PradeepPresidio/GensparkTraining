using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int[] arr1 = { 1, 3, 5 };
        int[] arr2 = { 2, 4, 6 };

        List<int> mergedList = new List<int>();
      
        mergedList.AddRange(arr1);
        mergedList.AddRange(arr2);

    
        int[] mergedArray = mergedList.ToArray();

        Console.WriteLine("Merged array:");
        foreach (int num in mergedArray)
        {
            Console.Write(num + " ");
        }
        Console.WriteLine();
    }
}
