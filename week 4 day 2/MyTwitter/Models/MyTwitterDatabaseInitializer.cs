using System.Collections.Generic;
using System.Data.Entity;
using MyTwitter.Models;

public class MyTwitterDatabaseInitializer : DropCreateDatabaseIfModelChanges<MyTwitterContext>
{
    protected override void Seed(MyTwitterContext context)
    {
        var users = GetUsers();
        users.ForEach(u => context.Users.Add(u));

        var tweets = GetTweets(users);
        tweets.ForEach(t => context.Tweets.Add(t));

        context.SaveChanges();
    }

    private static List<User> GetUsers()
    {
        return new List<User>
        {
            new User
            {
                UserId = 1,
                Username = "john_doe",
                Password = "password123",
                UserBio = "Just another tech enthusiast.",
                FollowersCount = 150,
                FollowingCount = 100
            },
            new User
            {
                UserId = 2,
                Username = "jane_smith",
                Password = "securepass",
                UserBio = "Lover of coffee and code.",
                FollowersCount = 200,
                FollowingCount = 180
            }
        };
    }

    private static List<Tweet> GetTweets(List<User> users)
    {
        return new List<Tweet>
        {
            new Tweet
            {
                TweetId = 1,
                UserId = users[0].UserId,
                User = users[0],
                Title = "Hello World!",
                Body = "My first tweet on MyTwitter.",
                Likes = 10,
                Tags = new List<string> { "#first", "#hello" }
            },
            new Tweet
            {
                TweetId = 2,
                UserId = users[1].UserId,
                User = users[1],
                Title = "Good Morning",
                Body = "Coffee and sunshine to start the day!",
                Likes = 25,
                Tags = new List<string> { "#morning", "#coffee" }
            }
        };
    }
}
