using System;
class Program
{
    static void Main()
    {
        int a, b;
        Console.WriteLine("Enter 2 numbers");
        a = Convert.ToInt32(Console.ReadLine());
        b = Convert.ToInt32(Console.ReadLine());
        if (a > b)
            Console.WriteLine($"Largest number is {a}");
        else if(a<b)
            Console.WriteLine($"Largest number is {b}");
        else
            Console.WriteLine($"Both numbers are equal");
    }
}