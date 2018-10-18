using System;
using Microsoft.Extensions.DependencyInjection;
using RateReader.Cnb.Providers.Services;
using RateReader.Cnb.Providers.Services.Interfaces;

namespace RateReader.Cnb.Providers
{
    public static class Config
    {
        public static IServiceCollection AddCbnProviders(this IServiceCollection services,
            Action<CnbCurrencyProviderSettings> configure)
        {
            var settings = new CnbCurrencyProviderSettings();
            configure(settings);

            if (settings.CurrencyOnDateUri == null)
            {
                throw new ApplicationException("Func CurrencyOnDateUri is null");
            }

            if (settings.YearCurrencyHistoryUri == null)
            {
                throw new ApplicationException("Func YearCurrencyHistoryUri is null");
            }

            services.AddSingleton(settings);

            services
                .AddSingleton<ICurrencyDataProvier, HttpCnbCurrencyProvider>()
                .AddSingleton<ICurrentCurrencyReader, CurrentCurrencyReader>()
                .AddSingleton<ICurrencyHistoryHeaderReader, CurrencyHistoryHeaderReader>()
                .AddSingleton<ICurrencyHistoryLineReader, CurrencyHistoryLineReader>();

            return services;
        }

    }
}