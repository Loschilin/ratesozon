using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RateReader.Cnb.Providers.Services.Interfaces;

namespace RateReader.Cnb.Providers.Services
{
    internal class HttpCnbCurrencyProvider: ICurrencyDataProvier
    {
        private readonly CnbCurrencyProviderSettings _settings;
        private readonly ILogger<HttpCnbCurrencyProvider> _logger;
        private readonly ICurrentCurrencyReader _currentCurrencyReader;
        private readonly ICurrencyHistoryHeaderReader _historyHeaderReader;
        private readonly ICurrencyHistoryLineReader _historyLineReader;
        private readonly HttpClient _client;
        
        public HttpCnbCurrencyProvider(
            CnbCurrencyProviderSettings settings, 
            ILogger<HttpCnbCurrencyProvider> logger,
            ICurrentCurrencyReader currentCurrencyReader,
            ICurrencyHistoryHeaderReader historyHeaderReader,
            ICurrencyHistoryLineReader historyLineReader)
        {
            _settings = settings;
            _logger = logger;
            _currentCurrencyReader = currentCurrencyReader;
            _historyHeaderReader = historyHeaderReader;
            _historyLineReader = historyLineReader;
            _client = new HttpClient();
        }

        protected virtual async Task SendHttpGetRequest(
            string uri, Func<string, Task> responseLineHandle)
        {
            using (var serviceResult = await _client.GetAsync(uri))
            {
                if (!serviceResult.IsSuccessStatusCode)
                {
                    var message = await serviceResult.Content.ReadAsStringAsync();

                    _logger.LogError($"Request error {serviceResult.StatusCode} " +
                                     $"get {uri}. {message}");
                    return;
                }

                using (var resultStream = await serviceResult.Content.ReadAsStreamAsync())
                using (var reader = new StreamReader(resultStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        await responseLineHandle(line);
                    }
                }
            }
        }

        public async Task GetCurrentCurrencyAsync(
            DateTime valueOnDate, Func<CnbCurrentCurrency, Task> handler)
        {
            var uri = _settings.CurrencyOnDateUri(valueOnDate);
            await SendHttpGetRequest(uri.ToString(), async line =>
            {
                if (!_currentCurrencyReader.TryReadCurrency(line, out var currency))
                {
                    return;
                }

                await handler(currency);
            });
        }
        
        public async Task GetCurrencyHistoryAsync(
            int year, Func<CnbCurrencyHistory, Task> handler)
        {
            var uri = _settings.YearCurrencyHistoryUri(year);
            ResponseHistoryHeader[] headers = null;

            await SendHttpGetRequest(uri.ToString(), async line =>
            {
                if (headers == null && !_historyHeaderReader.TyrReadHeaders(line, out headers))
                {
                    _logger.LogError(
                        $"Wrong response. Line is not header curency: {line}");
                    return;
                }

                if (!_historyLineReader.TryRead(line, out var ratesDate, out var rateValues))
                {
                    return;
                }

                if (rateValues.Length != headers.Length)
                {
                    _logger.LogWarning(
                        "Length headers <> length values in response currency history");
                    return;
                }

                for (var i = 0; i < rateValues.Length; i++)
                {
                    var header = headers[i];
                    var value = rateValues[i];

                    var currency = new CnbCurrencyHistory
                    {
                        Amount = header.Amount,
                        Code = header.Code,
                        Date = ratesDate,
                        Rate = value
                    };

                    await handler(currency);
                }

            });
        }
        
    }
}
