using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using RateReader.Cnb.Providers.Services.Interfaces;

[assembly: InternalsVisibleTo("RateReader.Cnb.Providers.Tests")]

namespace RateReader.Cnb.Providers.Services
{
    internal class CurrencyHistoryLineReader : ICurrencyHistoryLineReader
    {
        private readonly ILogger<CurrencyHistoryLineReader> _logger;

        private static readonly Regex LineResponseRegex
            = new Regex("^[0-9]{2}.[A-Za-z]{3,4}\\s[0-9]{4}(\\|[0-9.]{1,}){1,}$");

        public CurrencyHistoryLineReader(ILogger<CurrencyHistoryLineReader> logger)
        {
            _logger = logger;
        }

        public bool TryRead(string line, out DateTime date, out decimal[] values)
        {
            date = DateTime.MinValue;
            values = new decimal[0];

            if (!LineResponseRegex.IsMatch(line))
            {
                _logger.LogWarning($"Incorrect line with currecy rate: {line}");
                return false;
            }

            var rts = line.Split('|').ToArray();

            if (!DateTime.TryParse(rts[0], out date))
            {
                _logger.LogError($"Incorrect date in currecy rate: {rts[0]}");
                return false;
            }

            try
            {
                values = rts.Skip(1)
                    .Select(e => decimal.Parse(e, CultureInfo.InvariantCulture)).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError($"Incorrect currency rate in line: {line}. {e.Message}");
                return false;
            }

            return true;
        }
    }
}