
using System.Collections.Generic;

namespace RateReader.Api.Models
{
    public class CurrencyResponseModel
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public List<CurrencyWeekPeriodModel> WeekPeriods { get; set; }
            = new List<CurrencyWeekPeriodModel>();
        
    }
}