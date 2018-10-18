using System;

namespace RateReader.Cnb.Providers
{
    public class CnbCurrencyHistory
    {
        public int Amount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}