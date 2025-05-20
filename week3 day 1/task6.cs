using System;
using System.Collections.Generic;
class Program
{
    static void Main()
    {
        int[] arr = new int[10];
        Dictionary<int, int> freqTable = new Dictionary<int, int>();
        Console.WriteLine("Enter 10 elements");
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = Convert.ToInt32(Console.ReadLine());
            if (freqTable.ContainsKey(arr[i]))
            {
                freqTable[arr[i]]++;
            }
            else
            {
                freqTable[arr[i]] = 1;
            }
        }
        foreach (var pair in freqTable)
        {
            Console.WriteLine($"{pair.Key} orrurs {pair.Value} times");
        }
    }
}
