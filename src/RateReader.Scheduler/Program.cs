using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RateReader.Cnb.Providers;
using RateReader.CurrencyContext;
using RateReader.Data;
using RateReader.Scheduler.Services;

namespace RateReader.Scheduler
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var config = BuildConfiguration();

            var dataConnectionString = config.GetConnectionString("SqlDataConnection");
            var currencyOnDateUri = config.GetValue<string>("CurrencyOnDateUri");
            var yearCurrencyHistoryUri = config.GetValue<string>("YearCurrencyHistoryUri");
            
            var serviceConfig = new RateReaderServiceConfiguration
            {
                TimerPeriodSec = config.GetValue<int>("TimerPeriodSec")
            };

            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddLogging(e=>e.AddConsole())
                        .AddCurrencyDataContext(options => { options.ConnectionString = dataConnectionString; })
                        .AddCurrencyContext()
                        .AddCbnProviders(e =>
                        {
                            e.CurrencyOnDateUri = date => new Uri($"{currencyOnDateUri}?date={date:dd.MM.yyyy}");
                            e.YearCurrencyHistoryUri = year => new Uri($"{yearCurrencyHistoryUri}?year={year}");
                        })
                        .AddSingleton(serviceConfig);

                    services.AddHostedService<RateReaderService>();
                });

            if (isService)
            {
                await builder.RunAsServiceAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
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
