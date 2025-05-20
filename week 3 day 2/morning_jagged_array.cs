using System;

class Post
{
    public string Caption { get; set; }
    public int Likes { get; set; }

    public Post(string caption, int likes)
    {
        Caption = caption;
        Likes = likes;
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Enter number of users: ");
        int numUsers = int.Parse(Console.ReadLine());

        Post[][] usersPosts = new Post[numUsers][];

        for (int i = 0; i < numUsers; i++)
        {
            Console.Write($"\nUser {i + 1}: How many posts? ");
            int numPosts = int.Parse(Console.ReadLine());

            usersPosts[i] = new Post[numPosts];

            for (int j = 0; j < numPosts; j++)
            {
                Console.Write($"Enter caption for post {j + 1}: ");
                string caption = Console.ReadLine();

                Console.Write("Enter likes: ");
                int likes = int.Parse(Console.ReadLine());

                usersPosts[i][j] = new Post(caption, likes);
            }
        }

        Console.WriteLine("\n--- Displaying Instagram Posts ---");
        for (int i = 0; i < usersPosts.Length; i++)
        {
            Console.WriteLine($"User {i + 1}:");
            for (int j = 0; j < usersPosts[i].Length; j++)
            {
                Console.WriteLine($"Post {j + 1} - Caption: {usersPosts[i][j].Caption} | Likes: {usersPosts[i][j].Likes}");
            }
            Console.WriteLine();
        }
    }
}
