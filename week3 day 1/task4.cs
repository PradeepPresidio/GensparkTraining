using System;
class Program
{
    static void Main()
    {
        string name = null, password = null;
        for (int i = 0; i < 3; i++)
        {
            Console.WriteLine("Enter Name");
            name = Console.ReadLine();
            Console.WriteLine("Enter Password");
            password = Console.ReadLine();
            if (name == "Admin" && password == "pass")
            { 
                Console.WriteLine("Welcome Admin");
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid Credentials. Retrying");
            }
        }
        Console.WriteLine("Invalid Attempts for 3 times.Exiting");
} }