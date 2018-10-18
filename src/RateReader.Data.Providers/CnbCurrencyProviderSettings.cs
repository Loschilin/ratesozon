using System;

namespace RateReader.Cnb.Providers
{
    public class CnbCurrencyProviderSettings
    {
        public Func<DateTime, Uri> CurrencyOnDateUri { get; set; }
        public Func<int, Uri> YearCurrencyHistoryUri { get; set; }
    }
}