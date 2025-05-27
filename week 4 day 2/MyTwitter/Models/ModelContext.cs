using System.Data.Entity;
namespace MyTwitter.Models
{
    public class MyTwitterContext : DbContext
    {
        public MyTwitterContext() : base("MyTwitter")
        {
        public DbSet<Tweets> Tweet { get; set; }
        public DbSet<Users> User { get; set; }
        }

    }