using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateReader.Data.Contexts;
using RateReader.Data.Factories;
using RateReader.Data.Factories.Interfaces;

namespace RateReader.Data
{
    public static class Config
    {
        public static IServiceCollection AddCurrencyDataContext(this IServiceCollection services,
            Action<CurrentcyDbContextOptions> configure)
        {
            var libOptions = new CurrentcyDbContextOptions();
            configure(libOptions);

            var options = new DbContextOptionsBuilder<DbCurrencyContext>()
                .UseSqlServer(libOptions.ConnectionString).Options;

            services.AddSingleton(options)
                .AddFactories();
            return services;
        }

        private static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<IConetextFactory<DbCurrencyContext>, CurrencyContextFactory>();
            return services;
        }
    }
}