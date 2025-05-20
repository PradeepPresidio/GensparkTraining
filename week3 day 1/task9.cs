using System;

class Program
{
    static void Main()
    {
        string secret = "GAME";
        int attempts = 0;

            
            bool[] guessCheck = new bool[4];
        while (true)
        {
        attempts++;
        Console.WriteLine("\nEnter your 4-letter guess: ");
        string guess = Console.ReadLine().ToUpper();
            int bulls = 0, cows = 0;
            if (guess.Length != 4)
            {
                Console.WriteLine("Guess must be 4 letters.");
                continue;
            }


            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == secret[i])
                {
                    bulls++;
                    guessCheck[i] = true;
                }
                else
                {
                    cows++;
                    guessCheck[i] = false;
                }
            }
            Console.WriteLine($"Secrect Word : {secret} Guess Word : {guess}");
            Console.Write($" {bulls} Bulls, {cows} Cows ");
            string correct = "", incorrect = "";
            for(int i = 0;i < 4;i++)
            {
                if (guessCheck[i])
                    correct += secret[i];
                else
                    incorrect += secret[i];
            }
            Console.Write("Correct : " + correct + " Incorrect : " + incorrect);

            if (bulls == 4)
            {
                Console.WriteLine($"Congrats! You guessed the word in {attempts} attempts.");
                break;
            }
        }
    }
}
