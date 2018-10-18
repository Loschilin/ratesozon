namespace RateReader.Cnb.Providers
{
    public class CnbCurrentCurrency
    {
        public string Country { get; set; }
        public string Currency { get; set; }
        public int Amount { get; set; }
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }
}