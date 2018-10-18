using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RateReader.Cnb.Providers;
using RateReader.Cnb.Providers.Services.Interfaces;
using RateReader.CurrencyContext;
using RateReader.CurrencyContext.Factories.Interfaces;
using RateReader.CurrencyContext.Repositories.Interfaces;
using RateReader.Data;

namespace RateReader.Console
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var config = BuildConfiguration();
            var serviceProvider = BuildServiceProvider(config);

            var yearsToHandle = config.GetSection("YearsToHandle").Get<int[]>().Distinct().ToArray();
            var handledPackageSize = config.GetValue<int>("HandledPackageSize");

            var dataProvider = serviceProvider.GetService<ICurrencyDataProvier>();
            var curencyRepository = serviceProvider.GetService<ICurrencyRepository>();
            var currencyFactory = serviceProvider.GetService<ICurrecyFactory>();

            foreach (var year in yearsToHandle)
            {
                System.Console.WriteLine($"Import data for {year} year. Wait...");
                var recordsToHandle = new List<CnbCurrencyHistory>();
                
                await dataProvider.GetCurrencyHistoryAsync(year, async h =>
                {
                    recordsToHandle.Add(h);

                    if (recordsToHandle.Count != handledPackageSize)
                    {
                        return;
                    }

                    await Load(curencyRepository, currencyFactory, recordsToHandle);
                    recordsToHandle.Clear();
                });

                if (recordsToHandle.Any())
                {
                    await Load(curencyRepository, currencyFactory, recordsToHandle);
                }
            }
            
            System.Console.WriteLine("Done.");
            System.Console.ReadLine();
        }

        private static async Task Load(ICurrencyRepository curencyRepository, 
            ICurrecyFactory currecyFactory,
            IEnumerable<CnbCurrencyHistory> recordsToHandle)
        {
            var currecies = recordsToHandle
                .Select(e => currecyFactory.Create(e.Code, e.Amount, e.Rate, e.Date))
                .ToArray();

            await curencyRepository.SaveAsync(currecies);
        }

        private static IServiceProvider BuildServiceProvider(IConfiguration config)
        {
            var dataConnectionString = config.GetConnectionString("SqlDataConnection");
            var currencyOnDateUri = config.GetValue<string>("CurrencyOnDateUri");
            var yearCurrencyHistoryUri = config.GetValue<string>("YearCurrencyHistoryUri");
            
            var provider = new ServiceCollection()
                .AddLogging(e => e.AddConsole())
                .AddCurrencyContext()
                .AddCurrencyDataContext(e=>e.ConnectionString = dataConnectionString)
                .AddCbnProviders(e =>
                {
                    e.CurrencyOnDateUri = date => new Uri($"{currencyOnDateUri}?date={date:dd.MM.yyyy}");
                    e.YearCurrencyHistoryUri = year => new Uri($"{yearCurrencyHistoryUri}?year={year}");
                })
                .BuildServiceProvider();

            return provider;
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            return builder.Build();
        }
    }
}
