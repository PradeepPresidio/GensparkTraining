using System;
using System.Linq;

class Program
{
    static bool isCellValid(int[,] board, int cellCount)
    {
        int startRow = (cellCount / 3)*3;
        int startCol = (cellCount % 3)*3;
        int[] cell = new int[9];
        int count = 0;
        for(int i = startRow;i<startRow+3;i++)
        {
            for(int j = startCol; j < startCol+3; j++)
            {
                cell[count++] = board[i,j];
                Console.WriteLine($"{i} {j} {board[i,j]}");
            }
        }
        for (int i = 0; i < 9; i++)
            Console.Write(" " + cell[i] + " ");
        Console.WriteLine("");
        return cell.All(n => n > 0 && n<10 ) && cell.Distinct().Count()==9;
    }
    static void Main()
    {
        int[,] board = new int[9, 9]
{
    {5,3,4,6,7,8,9,1,2},
    {6,7,2,1,9,5,3,4,8},
    {1,9,8,3,4,2,5,6,7},
    {8,5,9,7,6,1,4,2,3},
    {4,2,6,8,5,3,7,9,1},
    {7,1,3,9,2,4,8,5,6},
    {9,6,1,5,3,7,2,8,4},
    {2,8,7,4,1,9,6,3,5},
    {3,4,5,2,8,6,1,7,9}
};
        //int[,] board = new int[9, 9];
        //Console.WriteLine("Enter the Sudoku board (9 rows, each with 9 numbers 1–9):");

        //for (int i = 0; i < 9; i++)
        //{
        //    string[] input = Console.ReadLine().Split();
        //    if (input.Length != 9)
        //    {
        //        Console.WriteLine("Each row must have exactly 9 numbers.");
        //        return;
        //    }
        //    for (int j = 0; j < 9; j++)
        //    {
        //        board[i, j] = Convert.ToInt32(input[j]);
        //    }
        //}

        bool allRowsValid = true;

        for (int i = 0; i < 9; i++)
        {
            int[] row = new int[9];
            for (int j = 0; j < 9; j++)
            {
                row[j] = board[i, j];
            }

            bool isValid = row.All(n => n >= 1 && n <= 9) && row.Distinct().Count() == 9;

            if (!isValid)
            {
                Console.WriteLine($"Row {i + 1} is invalid.");
                allRowsValid = false;
            }
        }
        bool allColumnsValid = true;
        for (int i = 0; i < 9; i++)
        {
            int[] col = new int[9];
            for (int j = 0; j < 9; j++)
            {
                col[j] = board[j, i];
            }

            bool isValid = col.All(n => n >= 1 && n <= 9) && col.Distinct().Count() == 9;

            if (!isValid)
            {
                Console.WriteLine($"Column {i + 1} is invalid.");
                allColumnsValid = false;
            }
        }
        bool allCellsValid = true;
        for(int i = 0;i < 9;i++)
        {
            if (!isCellValid(board, i))
            {
                allCellsValid = false;    
            Console.WriteLine($"Cell {i + 1} is invalid");
            }

        }

        if (allRowsValid && allColumnsValid && allCellsValid)
            Console.WriteLine("Sudoku is valid.");
        else
            Console.WriteLine("Sudoku is invalid.");
    }
}
