using System;

namespace RateReader.Data.Entities
{
    public class DbCurrencyValue
    {
        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }
        public DbCurrency Currency { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Rate { get; set; }
        public int Amount { get; set; }
    }
}