using Microsoft.EntityFrameworkCore;
using RateReader.Data.Contexts;
using RateReader.Data.Factories.Interfaces;

namespace RateReader.Data.Factories
{
    internal class CurrencyContextFactory : IConetextFactory<DbCurrencyContext>
    {
        private readonly DbContextOptions<DbCurrencyContext> _options;

        public CurrencyContextFactory(DbContextOptions<DbCurrencyContext> options)
        {
            _options = options;
        }

        public DbCurrencyContext Create()
        {
            var context = new DbCurrencyContext(_options);
            return context;
        }
    }
}