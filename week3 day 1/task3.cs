using System;
class Program
{
    static void Main()
    {
        int a, b;
        Console.WriteLine("Enter 2 numbers ");
        a = Convert.ToInt32(Console.ReadLine());
        b = Convert.ToInt32(Console.ReadLine());
        char op;
        Console.WriteLine("Enter Operation to perform (+,-,*,/) ");
        op = Console.ReadLine()[0];
        if (op == '-')
            Console.WriteLine($"Difference between {a} and {b} is {a - b}");
        if (op == '+')
            Console.WriteLine($"Sum of {a} and {b} is {a + b}");
        if (op == '*')
            Console.WriteLine($"Product of {a} and {b} is {a * b}");
        if (op == '/')
            Console.WriteLine($"Division of {a} over {b} is {a/b}");

    }
}
