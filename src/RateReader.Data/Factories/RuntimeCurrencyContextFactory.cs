using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RateReader.Data.Contexts;

namespace RateReader.Data.Factories
{
    internal class RuntimeCurrencyContextFactory: IDesignTimeDbContextFactory<DbCurrencyContext>
    {
        private const string AppSettingsFilePath = "/../RateReader.Api";
        private const string ConnectionStringName = "SqlDataConnection";

        public DbCurrencyContext CreateDbContext(string[] args)
        {
            var path = string.Concat(Directory.GetCurrentDirectory(), AppSettingsFilePath);

            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json");
            var config = builder.Build();
            var connectionString = config.GetConnectionString(ConnectionStringName);

            var options = new DbContextOptionsBuilder<DbCurrencyContext>()
                .UseSqlServer(connectionString).Options;

            var result = new DbCurrencyContext(options);

            return result;
        }
    }
}