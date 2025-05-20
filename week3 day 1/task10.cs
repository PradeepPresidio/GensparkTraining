using System;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] row = new int[9];
        Console.WriteLine("Enter 9 numbers (1 to 9):");
        for (int i = 0; i < 9; i++)
        {
            row[i] = Convert.ToInt32(Console.ReadLine());
        }

        bool isValid = row.All(n => n >= 1 && n <= 9) && row.Distinct().Count() == 9;

        if (isValid)
            Console.WriteLine("Valid Sudoku row");
        else
            Console.WriteLine("Invalid Sudoku row");
    }
}
