using System;

class Program
{
    static void Main()
    {
        Console.Write("Enter a message (lowercase letters only): ");
        string message = Console.ReadLine();

        string encrypted = Encrypt(message);
        string decrypted = Decrypt(encrypted);

        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {decrypted}");
    }

    static string Encrypt(string input)
    {
        string result = "";
        foreach (char c in input)
        {
           
            char shifted = (char)((c - 'a' + 3) % 26 + 'a');
            result += shifted;
        }
        return result;
    }

    static string Decrypt(string input)
    {
        string result = "";
        foreach (char c in input)
        {
          
            char shifted = (char)((c - 'a' - 3 + 26) % 26 + 'a');
            result += shifted;
        }
        return result;
    }
}
