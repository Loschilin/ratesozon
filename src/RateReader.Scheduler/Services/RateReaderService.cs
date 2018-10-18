using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RateReader.Cnb.Providers.Services.Interfaces;
using RateReader.CurrencyContext.Factories.Interfaces;
using RateReader.CurrencyContext.Repositories.Interfaces;

namespace RateReader.Scheduler.Services
{
    public class RateReaderService : IHostedService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyDataProvier _currencyDataProvier;
        private readonly ICurrecyFactory _currecyFactory;
        private readonly RateReaderServiceConfiguration _configuration;
        private readonly ILogger<RateReaderService> _logger;
        private System.Timers.Timer _timer = null;

        public RateReaderService(
            ICurrencyRepository currencyRepository,
            ICurrencyDataProvier currencyDataProvier,
            ICurrecyFactory currecyFactory,
            RateReaderServiceConfiguration configuration,
            ILogger<RateReaderService> logger)
        {
            _currencyRepository = currencyRepository;
            _currencyDataProvier = currencyDataProvier;
            _currecyFactory = currecyFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer
            {
                Interval = TimeSpan.FromSeconds(_configuration.TimerPeriodSec).TotalMilliseconds
            };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            _timer.Start();
            
            return Task.CompletedTask;
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs eaArgs)
        {
            var timer = (System.Timers.Timer)sender;
            var date = DateTime.Today;
            timer.Stop();
            try
            {
                var currentCureciesTask = _currencyDataProvier.GetCurrentCurrencyAsync(
                    date, async e =>
                    {
                        var currency = _currecyFactory.Create(e.Code, e.Amount, e.Rate, date);
                        await _currencyRepository.SaveAsync(currency);
                    });
                currentCureciesTask.Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetCurrentCurrencyAsync ERROR!");
            }
            
            timer.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

    }
}
