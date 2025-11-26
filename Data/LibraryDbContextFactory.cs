using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmartLibraryAPI.Data
{
    public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();

            // Build configuration to read appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables() // optional: allows overriding with env vars
                .Build();

            // Get connection string from config, or fallback to default
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? "Host=localhost;Port=5432;Database=LibraryManagementSystem;Username=TheTwoPoints;Password=5212006";

            optionsBuilder.UseNpgsql(connectionString);

            return new LibraryDbContext(optionsBuilder.Options);
        }
    }
}
