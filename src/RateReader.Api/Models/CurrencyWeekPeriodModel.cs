using System.Collections.Generic;

namespace RateReader.Api.Models
{
    public class CurrencyWeekPeriodModel
    {
        public int StartDay { get; set; }
        public int EndDay { get; set; }

        public List<CurrencyResponseValueModel> Currencies { get; set; }
            = new List<CurrencyResponseValueModel>();
    }
}