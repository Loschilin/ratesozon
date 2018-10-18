namespace RateReader.Api.Models
{
    public class CurrencyResponseValueModel
    {
        public string Code { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Median { get; set; }
    }
}