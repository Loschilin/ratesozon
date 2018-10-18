using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using RateReader.Cnb.Providers.Services.Interfaces;

[assembly:InternalsVisibleTo("RateReader.Cnb.Providers.Tests")]

namespace RateReader.Cnb.Providers.Services
{
    internal class CurrentCurrencyReader : ICurrentCurrencyReader
    {
        private readonly ILogger<CurrentCurrencyReader> _logger;

        private static readonly Regex CurrencyRegexp = new Regex(
            "^[A-Za-z]{1,}\\|[A-Za-z]{1,}\\|[0-9]{1,}\\|[A-Z]{1,}\\|[0-9]{1,}.[0-9]{1,}$");

        public CurrentCurrencyReader(ILogger<CurrentCurrencyReader> logger)
        {
            _logger = logger;
        }

        public bool TryReadCurrency(string line, out CnbCurrentCurrency currency)
        {
            currency = null;

            if (!CurrencyRegexp.IsMatch(line))
            {
                return false;
            }

            var crs = line.Split('|');

            if (!decimal.TryParse(crs[4], NumberStyles.Any, 
                    CultureInfo.InvariantCulture, out var rate))
            {
                _logger.LogWarning($"Incorrect decimal Rate value {crs[4]}");
                return false;
            }

            if (!int.TryParse(crs[2], out var amount))
            {
                _logger.LogWarning($"Incorrect int Amount value {crs[2]}");
                return false;
            }

            currency = new CnbCurrentCurrency
            {
                Amount = amount,
                Code = crs[3],
                Country = crs[0],
                Currency = crs[1],
                Rate = rate
            };

            return true;
        }
    }
}