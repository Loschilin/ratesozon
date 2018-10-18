using Microsoft.Extensions.DependencyInjection;
using RateReader.CurrencyContext.Factories;
using RateReader.CurrencyContext.Factories.Interfaces;
using RateReader.CurrencyContext.Repositories;
using RateReader.CurrencyContext.Repositories.Interfaces;

namespace RateReader.CurrencyContext
{
    public static class Config
    {
        public static IServiceCollection AddCurrencyContext(this IServiceCollection services)
        {
            return services
                .AddRepositoryes()
                .AddFactories();
        }

        private static IServiceCollection AddRepositoryes(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencyRepository, SqlCurrencyRepository>();
            return services;
        }

        private static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<ICurrecyFactory, CurrencyFactory>();
            return services;
        }
    }
}
