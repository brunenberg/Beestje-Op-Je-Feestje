using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BeestjeLibrary.DataAccess;

namespace BeestjeLibrary.DataAccess {
    public class BOJFContextFactory : IDesignTimeDbContextFactory<BOJFContext> {
        public BOJFContext CreateDbContext(string[] args) {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<BOJFContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));

            return new BOJFContext(optionsBuilder.Options);
        }
    }
}
