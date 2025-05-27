class Program
{
    static void Main(string[] args)
    {
        Database.SetInitializer(new MyTwitterDatabaseInitializer());

        using (var context = new MyTwitterContext())
        {
            // This will trigger database creation and seeding
            var users = context.Users.ToList();
            Console.WriteLine("Users count: " + users.Count);
        }
    }
}
