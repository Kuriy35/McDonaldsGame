using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace McDonalds.Data
{
    public class McDonaldsContextFactory : IDesignTimeDbContextFactory<McDonaldsContext>
    {
        public McDonaldsContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<McDonaldsContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("McDonalds"));

            return new McDonaldsContext(optionsBuilder.Options);
        }
    }
}