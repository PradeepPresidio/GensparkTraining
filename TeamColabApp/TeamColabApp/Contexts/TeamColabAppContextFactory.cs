using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TeamColabApp.Contexts
{
    public class TeamColabAppContextFactory : IDesignTimeDbContextFactory<TeamColabAppContext>
    {
        public TeamColabAppContext CreateDbContext(string[] args)
        {
            // Build config to read from appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // This is important
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TeamColabAppContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new TeamColabAppContext(optionsBuilder.Options);
        }
    }
}
